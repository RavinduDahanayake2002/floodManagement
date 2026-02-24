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
            // 1. Prepare ML Request Model (Regression on Affected)
            var requestBody = new
            {
                division = division,
                month = DateTime.Now.Month
            };

            string jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // 2. Query Local Python ML API (FastAPI)
            string url = "http://127.0.0.1:8000/predict";
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(responseString);
                var root = document.RootElement;
                
                int predictedAffected = root.GetProperty("predicted_affected").GetInt32();
                string riskLevel = root.GetProperty("risk_level").GetString() ?? "LOW";
                
                string emoji = riskLevel == "HIGH" ? "🔴" : (riskLevel == "MEDIUM" ? "🟡" : "🟢");
                
                // 3. Format as Markdown so RiskLocation.razor requires 0 changes!
                string markdown = $@"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🧠 SLIC CUSTOM ML PREDICTION
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📍 Target Division : {division.ToUpper()}
📅 Current Month   : {DateTime.Now.ToString("MMMM")}
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 **AI Risk Level** : {emoji} {riskLevel}
🎯 Predicted Impact: {predictedAffected:N0} People Affected

📜 Historical Context:
- Processed {historicalData.EventCount} past events in {division}.
- Average Historical Affected: {historicalData.AvgAffected:F1}
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
⚠️ RECOMMENDATION: 
{(riskLevel == "HIGH" ? "Immediate Evacuation. Trigger Claims Preparation Pipeline." : "Monitor Weather Conditions. Typical Risk.")}";

                return markdown;
            }
            
            Console.WriteLine($"Python ML API Request failed: {response.StatusCode}");
            return "Error calling Custom ML prediction service. Is FastAPI running?";
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error querying Local ML API: {ex.Message}");
            return $"Error connecting to Local ML backend: {ex.Message}";
        }
    }
}
