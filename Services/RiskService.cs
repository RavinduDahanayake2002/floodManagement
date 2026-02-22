using FloodApp.Models;
using System.Text.Json;

namespace FloodApp.Services;

public class RiskService
{
    private readonly GeoJsonService _geoJsonService;

    public RiskService(GeoJsonService geoJsonService)
    {
        _geoJsonService = geoJsonService;
    }

    public async Task<RiskResult> CalculateRiskAsync(LatLng point, double currentRainfall = 0)
    {
        // 1. Load polygons
        var geoJson = await _geoJsonService.GetRiskZonesGeoJsonAsync();
        
        using var doc = JsonDocument.Parse(geoJson);
        var root = doc.RootElement;
        
        RiskResult baseResult;
        
        // 2. Iterate features and check point inside
        if (root.TryGetProperty("features", out var features))
        {
            foreach (var feature in features.EnumerateArray())
            {
                var geometry = feature.GetProperty("geometry");
                var type = geometry.GetProperty("type").GetString();
                
                if (type == "Polygon")
                {
                    var coords = geometry.GetProperty("coordinates");
                    var ring = coords[0];
                    var polygon = new List<LatLng>();
                    
                    foreach (var p in ring.EnumerateArray())
                    {
                        polygon.Add(new LatLng(p[1].GetDouble(), p[0].GetDouble()));
                    }

                    if (IsPointInPolygon(point, polygon))
                    {
                        var props = feature.GetProperty("properties");
                        var levelStr = props.GetProperty("riskLevel").GetString();
                        
                        RiskLevel level = Enum.TryParse<RiskLevel>(levelStr, true, out var r) ? r : RiskLevel.Unknown;
                        
                        baseResult = new RiskResult 
                        { 
                            Level = level, 
                            Message = $"Located in {props.GetProperty("description").GetString()}",
                            Score = level == RiskLevel.High ? 9.5 : (level == RiskLevel.Medium ? 5.0 : 1.0)
                        };
                        
                        // Apply weather-based escalation
                        return ApplyWeatherEscalation(baseResult, currentRainfall);
                    }
                }
            }
        }

        // If not in any zone, assume Safe
        baseResult = new RiskResult 
        { 
            Level = RiskLevel.Low, 
            Message = "No direct flood risk detected in this zone.", 
            Score = 0.5
        };
        
        return ApplyWeatherEscalation(baseResult, currentRainfall);
    }

    private RiskResult ApplyWeatherEscalation(RiskResult result, double currentRainfall)
    {
        var originalLevel = result.Level;
        
        // Escalation rules based on current rainfall
        if (currentRainfall > 25 && result.Level == RiskLevel.Medium)
        {
            result.Level = RiskLevel.High;
            result.Score = Math.Min(result.Score + 3.0, 10.0);
            result.WasEscalated = true;
            result.EscalationReason = $"Risk escalated due to heavy rainfall ({currentRainfall:F1}mm)";
        }
        else if (currentRainfall > 10 && result.Level == RiskLevel.Low)
        {
            result.Level = RiskLevel.Medium;
            result.Score = Math.Min(result.Score + 3.0, 10.0);
            result.WasEscalated = true;
            result.EscalationReason = $"Risk escalated due to significant rainfall ({currentRainfall:F1}mm)";
        }
        else if (currentRainfall > 5 && result.Level != RiskLevel.High)
        {
            result.Score = Math.Min(result.Score + 1.0, 10.0);
            result.WasEscalated = true;
            result.EscalationReason = $"Risk score increased due to rainfall ({currentRainfall:F1}mm)";
        }
        
        // Update color based on final level
        result.ColorCode = result.Level switch 
        {
            RiskLevel.High => "#EF4444",
            RiskLevel.Medium => "#F59E0B",
            RiskLevel.Low => "#10B981",
            _ => "#6B7280"
        };
        
        return result;
    }

    public async Task<(string ZoneName, double DistanceKm)?> FindNearestSafeZoneAsync(LatLng point)
    {
        var geoJson = await _geoJsonService.GetRiskZonesGeoJsonAsync();
        using var doc = JsonDocument.Parse(geoJson);
        var root = doc.RootElement;
        
        string? nearestZone = null;
        double nearestDistance = double.MaxValue;
        
        if (root.TryGetProperty("features", out var features))
        {
            foreach (var feature in features.EnumerateArray())
            {
                var props = feature.GetProperty("properties");
                var levelStr = props.GetProperty("riskLevel").GetString();
                
                if (levelStr == "Low")
                {
                    // Calculate centroid of the polygon
                    var geometry = feature.GetProperty("geometry");
                    var coords = geometry.GetProperty("coordinates")[0];
                    double sumLat = 0, sumLng = 0;
                    int count = 0;
                    
                    foreach (var p in coords.EnumerateArray())
                    {
                        sumLng += p[0].GetDouble();
                        sumLat += p[1].GetDouble();
                        count++;
                    }
                    
                    if (count > 0)
                    {
                        var centroid = new LatLng(sumLat / count, sumLng / count);
                        var distance = HaversineDistance(point, centroid);
                        
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestZone = props.GetProperty("description").GetString();
                        }
                    }
                }
            }
        }
        
        if (nearestZone != null)
        {
            return (nearestZone, nearestDistance);
        }
        
        return null;
    }

    // Haversine formula to calculate distance between two points on Earth
    private double HaversineDistance(LatLng p1, LatLng p2)
    {
        const double R = 6371; // Earth's radius in km
        var dLat = ToRad(p2.Lat - p1.Lat);
        var dLng = ToRad(p2.Lng - p1.Lng);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(p1.Lat)) * Math.Cos(ToRad(p2.Lat)) *
                Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
    
    private double ToRad(double deg) => deg * Math.PI / 180;

    // Ray Casting Algorithm
    private bool IsPointInPolygon(LatLng point, List<LatLng> polygon)
    {
        bool inside = false;
        int j = polygon.Count - 1;
        
        for (int i = 0; i < polygon.Count; i++)
        {
            if ( (polygon[i].Lng > point.Lng) != (polygon[j].Lng > point.Lng) &&
                 (point.Lat < (polygon[j].Lat - polygon[i].Lat) * (point.Lng - polygon[i].Lng) / (polygon[j].Lng - polygon[i].Lng) + polygon[i].Lat) )
            {
                inside = !inside;
            }
            j = i;
        }
        return inside;
    }
}

