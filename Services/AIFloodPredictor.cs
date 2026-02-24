using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace FloodApp.Services;

public class AIFloodPredictor
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    // Claude API Key
    private const string ApiKey = "YOUR_CLAUDE_API_KEY"; 

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
            var systemPrompt = @"You are the SLIC Flood Risk Assistant.
Analyze the provided location, current rainfall, and historical context to determine the flood risk.
Your response MUST start with '🛡️ SLIC FLOOD RISK REPORT' and include:
1. An overall risk level: MUST contain the exact word 'HIGH', 'MEDIUM', or 'LOW'.
2. Expected damages and affected people based on historical severity.
3. Recommended actions.
If the rainfall is high (> 50mm) or historical events are numerous, prioritize assessing the risk as HIGH.";

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
- Severity Score: {historicalData.SeverityScore:F2}

Please provide the risk assessment.";

            var requestBody = new
            {
                model = "claude-3-haiku-20240307",
                max_tokens = 1000,
                system = systemPrompt,
                messages = new[]
                {
                    new { role = "user", content = userPrompt }
                }
            };

            string jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
            request.Headers.Add("x-api-key", ApiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(responseString);
                var root = document.RootElement;
                
                // Extract Claude's text reply
                var responseText = root.GetProperty("content")[0].GetProperty("text").GetString();
                return responseText ?? "Failed to parse Claude response.";
            }
            
            var errorBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Claude API Request failed: {response.StatusCode} - {errorBody}");
            return $"Error calling Claude API: {response.StatusCode}\nPlease configure your actual API key.";
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error querying Claude API: {ex.Message}");
            return $"Error connecting to Claude API: {ex.Message}";
        }
    }
}
