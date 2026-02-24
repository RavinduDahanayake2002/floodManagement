using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FloodApp.Models;

namespace FloodApp.Services;

public class AIFloodPredictor
{
    private readonly HttpClient _httpClient;

    public AIFloodPredictor(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AIFloodResult> PredictRiskAsync(double lat, double lng, double rainfall, double severityScore, int month)
    {
        try
        {
            var request = new PredictionRequest
            {
                Latitude = lat,
                Longitude = lng,
                Month = month,
                RainfallMm = rainfall,
                HistoricalSeverity = severityScore
            };

            var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:8000/predict", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PredictionResponse>();
                if (result != null)
                {
                    return new AIFloodResult
                    {
                        ProbabilityPercent = (int)Math.Round(result.Probability * 100),
                        RiskLevel = result.RiskLevel,
                        RainfallImpact = CalculateImpact(rainfall, 50, 100),
                        RiverImpact = 1, // Placeholder since river was removed for Python model simplicity
                        SoilImpact = 1   // Placeholder
                    };
                }
            }
            
            Console.WriteLine($"API Request failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error predicting AI risk: {ex.Message}");
        }

        // Fallback
        return new AIFloodResult();
    }

    private int CalculateImpact(double val, double medThreshold, double highThreshold)
    {
        if (val >= highThreshold) return 3; // High Impact
        if (val >= medThreshold) return 2;  // Medium Impact
        return 1; // Low Impact
    }

    private class PredictionRequest
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("rainfall_mm")]
        public double RainfallMm { get; set; }

        [JsonPropertyName("historical_severity")]
        public double HistoricalSeverity { get; set; }
    }

    private class PredictionResponse
    {
        [JsonPropertyName("probability")]
        public double Probability { get; set; }

        [JsonPropertyName("risk_level")]
        public string RiskLevel { get; set; } = "LOW";
    }
}

public class AIFloodResult
{
    public int ProbabilityPercent { get; set; }
    public string RiskLevel { get; set; } = "Low";
    public int RainfallImpact { get; set; } 
    public int RiverImpact { get; set; }
    public int SoilImpact { get; set; }
}
