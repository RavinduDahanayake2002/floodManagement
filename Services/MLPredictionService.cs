using System.Text.Json;

namespace FloodApp.Services;

public class MLPredictionService
{
    private readonly string _filePath = "wwwroot/data/ml_predictions.json";
    private List<MLDivisionPrediction> _predictions = new();

    public MLPredictionService()
    {
        LoadData();
    }

    private void LoadData()
    {
        string[] paths = { _filePath, "../" + _filePath };
        string? activePath = paths.FirstOrDefault(File.Exists);

        if (activePath == null)
        {
            Console.WriteLine("Warning: ml_predictions.json not found. Did you run train_models.py?");
            return;
        }

        try
        {
            string json = File.ReadAllText(activePath);
            _predictions = JsonSerializer.Deserialize<List<MLDivisionPrediction>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<MLDivisionPrediction>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error parsing ml_predictions.json: " + ex.Message);
        }
    }

    public MLDivisionPrediction? GetPredictionForDivision(string division)
    {
        if (string.IsNullOrEmpty(division)) return null;
        
        // Exact match or contains 
        return _predictions.FirstOrDefault(p => 
            p.Division.Equals(division, StringComparison.OrdinalIgnoreCase)) ??
            _predictions.FirstOrDefault(p => 
            p.Division.Contains(division, StringComparison.OrdinalIgnoreCase) || 
            division.Contains(p.Division, StringComparison.OrdinalIgnoreCase));
    }
}

public class MLDivisionPrediction
{
    public string Division { get; set; } = "";
    public string Province { get; set; } = "";
    public string District { get; set; } = "";
    public int PredictedAffected { get; set; }
    public string PredictedSeverity { get; set; } = "";
    public int HistoricalEventCount { get; set; }
    public double HistoricalAvgAffected { get; set; }
}
