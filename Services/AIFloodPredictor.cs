using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace FloodApp.Services;

public class AIFloodPredictor
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    // API Key from appsettings.json or user input.
    private const string ApiKey = "AIzaSyAm0fpp6AENXQwuMpVvRGmslQ_aqmdXjF8"; 

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
        HistoricalRiskData historicalData)
    {
        try
        {
            // Calculate components exactly as user requested
            int pastEvents = historicalData.EventCount;
            float avgAffected = historicalData.AvgAffected;
            float avgHousesDamaged = historicalData.AvgHousesDamaged;
            float avgDeaths = historicalData.AvgDeaths;
            float severity = historicalData.AvgSeverity;

            string systemPrompt = $@"You are the SLIC Flood Risk Assistant for Sri Lanka Insurance Corporation.

When a user provides:
- Province
- District  
- Division
- Current Rainfall (mm)

Follow these steps:

STEP 1 — HISTORICAL SEVERITY SCORE
Look up past flood events in that District/Division from DMC data.
Calculate: Severity = (Deaths * 10) + (Houses_Destroyed * 2) + (Affected / 100)
Count total historical flood events in that area.

STEP 2 — RAINFALL RISK MULTIPLIER
If rainfall > 200mm -> multiplier = HIGH
If rainfall 100–200mm -> multiplier = MEDIUM  
If rainfall < 100mm -> multiplier = LOW

STEP 3 — FINAL RISK LEVEL
Combine historical severity + rainfall multiplier:
- HIGH: 3+ past events OR severity score > 500 AND rainfall > 100mm
- MEDIUM: 1–2 past events OR severity score 100–500
- LOW: 0 past events AND rainfall < 100mm

STEP 4 — OUTPUT this exact format:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🛡️ SLIC FLOOD RISK REPORT
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📍 Location   : [Division], [District], [Province]
🌧️ Rainfall   : [X] mm
📊 Risk Level : 🔴 HIGH / 🟡 MEDIUM / 🟢 LOW
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📜 Historical Flood Events : [count]
👥 Avg. People Affected    : [number]
🏚️ Avg. Houses Damaged     : [number]
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
⚠️  RECOMMENDATION:
[One clear action sentence based on risk level]

🏥 Nearest Shelter: [show if HIGH risk]
📞 Emergency: 117 (DMC Sri Lanka)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Always be calm, clear, and helpful. Never exaggerate risk.
If location is not found in dataset, say: 
""No historical flood records found for this area — treating as LOW risk. Stay alert during heavy rainfall.""

USER DATA:
Province: {province}
District: {district}
Division: {division}
Current Rainfall (mm): {currentRainfall}

PRECALCULATED HISTORICAL CONTEXT:
Historical Flood Events: {pastEvents}
Calculated Severity Score: {severity:F1}
Avg Deaths: {avgDeaths:F1}
Avg Houses Damaged: {avgHousesDamaged:F1}
Avg Affected: {avgAffected:F1}

Generate the SLIC FLOOD RISK REPORT now based on the above rules and data.";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = systemPrompt }
                        }
                    }
                }
            };

            string jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-latest:generateContent?key={ApiKey}";
            
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                
                // Parse the Gemini response
                using var document = JsonDocument.Parse(responseString);
                var root = document.RootElement;
                
                var candidates = root.GetProperty("candidates");
                if (candidates.GetArrayLength() > 0)
                {
                    var text = candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                    return text ?? "Unable to generate prediction text.";
                }
            }
            
            Console.WriteLine($"Gemini API Request failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            return "Error calling AI prediction service.";
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error querying Gemini API: {ex.Message}");
            return $"Error: {ex.Message}";
        }
    }
}
