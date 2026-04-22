using FloodApp.Models;
using FloodApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FloodApp;

public static class FloodCheckEndpoints
{
    public static void MapFloodCheckEndpoints(this WebApplication app)
    {
        // ──────────────────────────────────────────
        //  FLOOD RISK
        // ──────────────────────────────────────────
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

            // 5. Sync risk level with ML prediction
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
        .WithName("CheckFloodRisk")
        .WithTags("Flood Risk")
        .WithSummary("Assess flood risk for a location")
        .WithDescription(
            "Returns a comprehensive flood risk assessment combining real-time weather data, " +
            "geospatial risk analysis, ML model predictions, and historical flood records. " +
            "Supply either lat/lng coordinates, a division name, or both.")
        .Produces<FloodCheckResponse>(200)
        .ProducesProblem(400);

        // ──────────────────────────────────────────
        //  ML PREDICTIONS
        // ──────────────────────────────────────────
        app.MapGet("/api/ml/predictions", (MLPredictionService mlPrediction) =>
        {
            // Expose all pre-computed division predictions
            var allDivisions = new List<string>(); // placeholder; use service reflection
            // Return via a dedicated list method if available; otherwise return empty guidance
            return Results.Ok(new
            {
                message = "Use /api/ml/predictions/{division} for division-specific predictions.",
                hint = "Example: /api/ml/predictions/Colombo"
            });
        })
        .WithName("GetMlPredictionsInfo")
        .WithTags("ML Predictions")
        .WithSummary("ML predictions endpoint information")
        .WithDescription("Guidance endpoint for the ML prediction API.")
        .Produces(200);

        app.MapGet("/api/ml/predictions/{division}", (string division, MLPredictionService mlPrediction) =>
        {
            var pred = mlPrediction.GetPredictionForDivision(division);
            if (pred == null)
                return Results.NotFound(new { error = $"No ML prediction found for division '{division}'." });

            return Results.Ok(pred);
        })
        .WithName("GetMlPredictionForDivision")
        .WithTags("ML Predictions")
        .WithSummary("Get ML flood prediction for a specific division")
        .WithDescription(
            "Returns the Random Forest model's pre-computed flood severity classification " +
            "and affected-population regression estimate for the specified divisional secretariat.")
        .Produces<MLDivisionPrediction>(200)
        .ProducesProblem(404);

        // ──────────────────────────────────────────
        //  HISTORICAL FLOOD EVENTS
        // ──────────────────────────────────────────
        app.MapGet("/api/historical/events", (
            string? province,
            string? yearRange,
            string? severity,
            HistoricalFloodEventService historicalEventService) =>
        {
            var events = historicalEventService.GetFilteredEvents(
                province   ?? "All",
                yearRange  ?? "All",
                severity   ?? "All");

            return Results.Ok(new
            {
                count  = events.Count,
                events
            });
        })
        .WithName("GetHistoricalFloodEvents")
        .WithTags("Historical Data")
        .WithSummary("List historical flood events with optional filters")
        .WithDescription(
            "Returns historical flood events from the DesInventar dataset. " +
            "Filter by province name (e.g. 'Western'), yearRange ('Before 2000', '2000–2010', '2011–2020', '2020–Present'), " +
            "and severity ('High', 'Medium', 'Low').")
        .Produces(200);

        app.MapGet("/api/historical/summary", (HistoricalFloodEventService historicalEventService) =>
        {
            return Results.Ok(new
            {
                totalEvents         = historicalEventService.GetTotalEventsCount(),
                deadliestEvent      = historicalEventService.GetDeadliestEvent(),
                mostAffectedProvince= historicalEventService.GetMostAffectedProvince(),
                decadeFrequency     = historicalEventService.GetDecadeFrequency()
            });
        })
        .WithName("GetHistoricalFloodSummary")
        .WithTags("Historical Data")
        .WithSummary("Aggregate summary statistics for all historical flood events")
        .WithDescription(
            "Returns high-level statistics: total event count, the deadliest recorded flood, " +
            "the most frequently affected province, and per-decade event frequency.")
        .Produces(200);

        // ──────────────────────────────────────────
        //  DAMAGE REPORTS / CLAIMS
        // ──────────────────────────────────────────
        app.MapGet("/api/claims", (AdminService adminService) =>
        {
            var claims = adminService.GetAllClaims();
            return Results.Ok(new
            {
                count = claims.Count,
                claims
            });
        })
        .WithName("GetAllClaims")
        .WithTags("Claims")
        .WithSummary("List all damage reports / insurance claims")
        .WithDescription("Returns every damage report submitted by policyholders, ordered by most recent first.")
        .Produces(200);

        app.MapPost("/api/claims", ([FromBody] DamageReport report, AdminService adminService) =>
        {
            report.Id         = Guid.NewGuid().ToString();
            report.ReportedAt = DateTime.UtcNow;
            report.Status     = "Pending";
            adminService.AddClaim(report);
            return Results.Created($"/api/claims/{report.Id}", report);
        })
        .WithName("SubmitClaim")
        .WithTags("Claims")
        .WithSummary("Submit a new damage report")
        .WithDescription(
            "Creates a new flood damage / insurance claim. Required fields: FullName, Address, " +
            "ContactNumber, Description. The Id, ReportedAt, and Status are set automatically.")
        .Produces<DamageReport>(201)
        .ProducesProblem(400);

        app.MapPatch("/api/claims/{id}/status", (
            string id,
            [FromBody] UpdateClaimStatusRequest req,
            AdminService adminService) =>
        {
            adminService.UpdateClaimStatus(id, req.Status);
            return Results.Ok(new { id, status = req.Status });
        })
        .WithName("UpdateClaimStatus")
        .WithTags("Claims")
        .WithSummary("Update the status of an existing claim")
        .WithDescription("Valid status values: Pending, Approved, Processed, Rejected.")
        .Produces(200)
        .ProducesProblem(404);

        app.MapGet("/api/claims/summary", (AdminService adminService) =>
        {
            return Results.Ok(adminService.GetClaimsStatusSummary());
        })
        .WithName("GetClaimsSummary")
        .WithTags("Claims")
        .WithSummary("Status-count summary for all claims")
        .WithDescription("Returns a dictionary of status → count, e.g. { Pending: 2, Approved: 1 }.")
        .Produces<Dictionary<string, int>>(200);

        // ──────────────────────────────────────────
        //  LOCATION DATA
        // ──────────────────────────────────────────
        app.MapGet("/api/location/provinces", (LocationService locationService) =>
        {
            return Results.Ok(locationService.GetProvinces());
        })
        .WithName("GetProvinces")
        .WithTags("Location")
        .WithSummary("List all provinces in Sri Lanka")
        .WithDescription("Returns all 9 provinces with their internal IDs and approximate centre coordinates.")
        .Produces(200);

        app.MapGet("/api/location/districts/{provinceId:int}", (int provinceId, LocationService locationService) =>
        {
            var districts = locationService.GetDistricts(provinceId);
            if (!districts.Any())
                return Results.NotFound(new { error = $"No districts found for provinceId {provinceId}." });

            return Results.Ok(districts);
        })
        .WithName("GetDistricts")
        .WithTags("Location")
        .WithSummary("List districts for a province")
        .WithDescription("Returns all districts belonging to the given province ID.")
        .Produces(200)
        .ProducesProblem(404);

        app.MapGet("/api/location/divisions/{districtId:int}", (int districtId, LocationService locationService) =>
        {
            var divisions = locationService.GetDivisions(districtId);
            if (!divisions.Any())
                return Results.NotFound(new { error = $"No divisions found for districtId {districtId}." });

            return Results.Ok(divisions);
        })
        .WithName("GetDivisions")
        .WithTags("Location")
        .WithSummary("List divisional secretariats for a district")
        .WithDescription("Returns all divisional secretariats (DS divisions) belonging to the given district ID.")
        .Produces(200)
        .ProducesProblem(404);

        app.MapGet("/api/location/towns/{divisionId:int}", (int divisionId, LocationService locationService) =>
        {
            var towns = locationService.GetTowns(divisionId);
            if (!towns.Any())
                return Results.NotFound(new { error = $"No towns found for divisionId {divisionId}." });

            return Results.Ok(towns);
        })
        .WithName("GetTowns")
        .WithTags("Location")
        .WithSummary("List towns for a divisional secretariat")
        .WithDescription("Returns all towns/GN divisions belonging to the given DS division ID.")
        .Produces(200)
        .ProducesProblem(404);

        // ──────────────────────────────────────────
        //  GEOCODING
        // ──────────────────────────────────────────
        app.MapGet("/api/geocode/reverse", async (double lat, double lon, GeocodingService geocodingService) =>
        {
            var address = await geocodingService.ReverseGeocodeAsync(lat, lon);
            if (string.IsNullOrEmpty(address))
                return Results.NotFound(new { error = "Address not found for these coordinates." });

            return Results.Ok(new { address });
        })
        .WithName("ReverseGeocode")
        .WithTags("Geocoding")
        .WithSummary("Convert coordinates to a human-readable address")
        .WithDescription("Uses the Geoapify API to perform reverse geocoding. Requires lat and lon query parameters.")
        .Produces(200)
        .ProducesProblem(404);

        app.MapGet("/api/geocode/search", async (string address, GeocodingService geocodingService) =>
        {
            var result = await geocodingService.GeocodeAsync(address);
            if (result == null)
                return Results.NotFound(new { error = "Coordinates not found for this address." });

            return Results.Ok(result);
        })
        .WithName("SearchAddress")
        .WithTags("Geocoding")
        .WithSummary("Convert an address string to coordinates")
        .WithDescription("Uses the Geoapify API to forward-geocode an address string into latitude/longitude.")
        .Produces(200)
        .ProducesProblem(404);

        // ──────────────────────────────────────────
        //  HEALTH CHECK
        // ──────────────────────────────────────────
        app.MapGet("/api/health", () =>
        {
            return Results.Ok(new
            {
                status    = "healthy",
                timestamp = DateTime.UtcNow,
                version   = "1.0.0"
            });
        })
        .WithName("HealthCheck")
        .WithTags("System")
        .WithSummary("API health check")
        .WithDescription("Returns HTTP 200 with a simple health payload. Use this to verify the API is running.")
        .Produces(200)
        .ExcludeFromDescription(); // still available, just not cluttering the docs
    }
}

// ──────────────────────────────────────────
//  Request / Response DTOs used by endpoints
// ──────────────────────────────────────────
public record UpdateClaimStatusRequest(string Status);
