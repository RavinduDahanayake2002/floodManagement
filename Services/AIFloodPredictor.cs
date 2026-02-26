using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace FloodApp.Services;

public class AIFloodPredictor
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    // Gemini API Key
    private const string ApiKey = "YOUR_GEMINI_API_KEY"; 

    public AIFloodPredictor(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> PredictRiskAsync(
        string province, 
        string district, 
        string division, 
        double currentRainfall, 
        HistoricalRiskData historicalData,
        string mlPredictedSeverity)
    {
        try
        {
            var systemPrompt = $@"You are the SLIC Flood Risk Assistant.
Analyze the provided location, current rainfall, and historical context to determine the flood risk.
Your response MUST start with '🛡️ SLIC FLOOD RISK REPORT' and include:
1. An overall risk level: MUST match the ML Model prediction exactly: '{mlPredictedSeverity}'.
2. Expected damages and affected people based on historical severity.
3. Recommended actions.";

            var userPrompt = $@"
Location Details:
- Province: {province}
- District: {district}
- Division: {division}

Current Weather:
- Rainfall: {currentRainfall:F1} mm

Historical Risk Context for this Division:
- Total Past Flood Events: {historicalData.EventCount}
- Average People Affected: {historicalData.AvgAffected:F1}

Please provide the risk assessment.";

            // Fallback Simulation for Demonstrations (When API Key is not set)
            if (ApiKey == "YOUR_GEMINI_API_KEY" || string.IsNullOrWhiteSpace(ApiKey))
            {
                await Task.Delay(1500); // Simulate API call delay
                
                string simulatedLevel = mlPredictedSeverity;
                string simulatedActions = simulatedLevel == "HIGH" 
                    ? "- Evacuate immediately to higher ground.\n- Prepare emergency kits.\n- Follow DMC instructions."
                    : simulatedLevel == "MEDIUM" 
                    ? "- Monitor local weather channels.\n- Be prepared to move valuables.\n- Avoid standing water."
                    : "No immediate evacuation required. Monitor the situation.";

                return $@"🛡️ SLIC FLOOD RISK REPORT

**Overall Risk Level: {simulatedLevel} RISK**

Based on the analysis for {division} ({district}, {province}):
- **Current Rainfall**: {currentRainfall:F1} mm
- **Historical Context**: The area has seen {historicalData.EventCount} past flood events, affecting an average of {historicalData.AvgAffected:F0} people.

**Expected Impact:**
Given current meteorological parameters and historical precedent, the predicted damages are {(simulatedLevel == "HIGH" ? "severe, anticipating widespread inundation" : simulatedLevel == "MEDIUM" ? "moderate, with localized street flooding expected" : "minimal under current conditions")}.

**Recommended Actions:**
{simulatedActions}

*(Note: This is a generated simulation because the real Gemini API key has not been configured in the system yet).*";
            }

            var requestBody = new
            {
                system_instruction = new
                {
                    parts = new[] { new { text = systemPrompt } }
                },
                contents = new[]
                {
                    new
                    {
                        parts = new[] { new { text = userPrompt } }
                    }
                },
                generationConfig = new
                {
                    maxOutputTokens = 1000
                }
            };

            string jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={ApiKey}");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(responseString);
                var root = document.RootElement;
                
                // Extract Gemini's text reply
                var responseText = root
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();
                    
                return responseText ?? "Failed to parse Gemini response.";
            }
            
            var errorBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Gemini API Request failed: {response.StatusCode} - {errorBody}");
            return $"Error calling Gemini API: {response.StatusCode}\nPlease configure your actual API key in AIFloodPredictor.cs.";
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error querying Claude API: {ex.Message}");
            return $"Error connecting to Claude API: {ex.Message}";
        }
    }
}
