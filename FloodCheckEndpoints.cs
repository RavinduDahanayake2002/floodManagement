using FloodApp.Models;
using FloodApp.Services;

namespace FloodApp;

public static class FloodCheckEndpoints
{
    public static void MapFloodCheckEndpoints(this WebApplication app)
    {
        app.MapGet("/api/flood/check", async (
            string? province,
            string? district,
            string? division,
            double? lat,
            double? lng,
            RiskService riskService,
            WeatherService weatherService,
            MLPredictionService mlPrediction,
            HistoricalDataService historicalData) =>
        {
            // Validate: need at least lat/lng OR division name
            if ((lat == null || lng == null) && string.IsNullOrEmpty(division))
            {
                return Results.BadRequest(new
                {
                    error = "Please provide either lat & lng coordinates, or a division name.",
                    example = "/api/flood/check?province=Western&district=Colombo&division=Colombo&lat=6.9271&lng=79.8612"
                });
            }

            var response = new FloodCheckResponse();

            // 1. Get current weather if coordinates are provided
            double currentRainfall = 0;
            if (lat.HasValue && lng.HasValue)
            {
                var weather = await weatherService.GetCurrentWeatherAsync(lat.Value, lng.Value);
                currentRainfall = weather?.Rain ?? 0;
            }
            response.CurrentRainfall = currentRainfall;

            // 2. Get geo-risk from RiskService if coordinates provided
            RiskResult riskResult;
            if (lat.HasValue && lng.HasValue)
            {
                var point = new LatLng(lat.Value, lng.Value);
                riskResult = await riskService.CalculateRiskAsync(point, currentRainfall);
            }
            else
            {
                riskResult = RiskResult.Default;
            }

            // 3. Get ML prediction for division
            var mlPred = !string.IsNullOrEmpty(division)
                ? mlPrediction.GetPredictionForDivision(division)
                : null;

            // 4. Get historical data
            var histData = historicalData.GetHistoricalRisk(
                province ?? "", district ?? "", division ?? "");

            // 5. Sync risk level with ML prediction (same logic as RiskLocation.razor)
            if (mlPred != null)
            {
                string mlSeverity = mlPred.PredictedSeverity;
                if (mlSeverity == "HIGH") riskResult.Level = RiskLevel.High;
                else if (mlSeverity == "MEDIUM") riskResult.Level = RiskLevel.Medium;
                else riskResult.Level = RiskLevel.Low;

                response.MlPredictedSeverity = mlPred.PredictedSeverity;
                response.PredictedAffected = mlPred.PredictedAffected;
            }

            // 6. Build response
            response.RiskLevel = riskResult.Level.ToString().ToUpper();
            response.RiskScore = riskResult.Score;
            response.Message = riskResult.Message;
            response.WasEscalated = riskResult.WasEscalated;
            response.EscalationReason = riskResult.EscalationReason;
            response.HistoricalEventCount = histData.EventCount;
            response.HistoricalAvgAffected = histData.AvgAffected;
            response.Timestamp = DateTime.UtcNow;

            return Results.Ok(response);
        })
        .WithName("CheckFloodLevel");

        app.MapGet("/api/geocode/reverse", async (double lat, double lon, GeocodingService geocodingService) =>
        {
            var address = await geocodingService.ReverseGeocodeAsync(lat, lon);
            if (string.IsNullOrEmpty(address))
                return Results.NotFound(new { error = "Address not found for these coordinates." });

            return Results.Ok(new { address });
        })
        .WithName("ReverseGeocode");

        app.MapGet("/api/geocode/search", async (string address, GeocodingService geocodingService) =>
        {
            var result = await geocodingService.GeocodeAsync(address);
            if (result == null)
                return Results.NotFound(new { error = "Coordinates not found for this address." });

            return Results.Ok(result);
        })
        .WithName("SearchAddress");
    }
}
