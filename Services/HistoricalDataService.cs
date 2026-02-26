using System.Data;
using System.Text;
using ExcelDataReader;

namespace FloodApp.Services;

public class HistoricalDataService
{
    private readonly string _filePath = "dataset/flood_data_clean.csv";
    
    // Cache for parsed data: Province -> District -> Division -> List of flood records
    private List<FloodRecord> _allRecords = new();
    
    public HistoricalDataService()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        LoadData();
    }
    
    private void LoadData()
    {
        // Try paths
        string[] paths = { _filePath, "../" + _filePath };
        string? activePath = paths.FirstOrDefault(File.Exists);

        if (activePath == null)
        {
            Console.WriteLine("Warning: DI_Report103734.xls not found.");
            return;
        }
            
        try 
        {
            // Because we're using CSV now, we might need a different loader or a quick native parser.
            // Using standard CSV parsing since the dataset is now simple:
            var lines = File.ReadAllLines(activePath);
            var header = lines[0].Split(',').ToList();
            
            var provIdx = header.IndexOf("Province");
            var distIdx = header.IndexOf("District");
            var divIdx = header.IndexOf("Division");
            var housesIdx = header.IndexOf("Houses Damaged");
            var affectedIdx = header.IndexOf("Affected");
            var deathsIdx = header.IndexOf("Deaths");
            
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var row = lines[i].Split(',');

                if (row.Length <= Math.Max(affectedIdx, Math.Max(provIdx, divIdx))) continue;

                var record = new FloodRecord
                {
                    Province = row[provIdx],
                    District = row[distIdx],
                    Division = row[divIdx],
                };
                
                float deaths = deathsIdx >= 0 && row.Length > deathsIdx ? (float.TryParse(row[deathsIdx], out var pd) ? pd : 0) : 0;
                float disabled = housesIdx >= 0 && row.Length > housesIdx ? (float.TryParse(row[housesIdx], out var phd) ? phd : 0) : 0;
                float affected = affectedIdx >= 0 && row.Length > affectedIdx ? (float.TryParse(row[affectedIdx], out var pa) ? pa : 0) : 0;
                
                record.Deaths = deaths;
                record.HousesDestroyed = disabled;
                record.Affected = affected;
                
                // Calculate severity: (Deaths * 10) + (Houses_Destroyed * 2) + (Affected / 100)
                record.SeverityScore = (deaths * 10) + (disabled * 2) + (affected / 100);
                
                _allRecords.Add(record);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error parsing dataset: " + ex.Message);
        }
    }
    
    public HistoricalRiskData GetHistoricalRisk(string province, string district, string division)
    {
        if (string.IsNullOrEmpty(district) && string.IsNullOrEmpty(division))
            return new HistoricalRiskData(); // Empty

        var filtered = _allRecords.Where(r => 
            (string.IsNullOrEmpty(province) || r.Province.Contains(province, StringComparison.OrdinalIgnoreCase)) && 
            (string.IsNullOrEmpty(district) || r.District.Contains(district, StringComparison.OrdinalIgnoreCase)) && 
            (string.IsNullOrEmpty(division) || r.Division.Contains(division, StringComparison.OrdinalIgnoreCase)))
            .ToList();
            
        if (!filtered.Any() && !string.IsNullOrEmpty(district))
        {
            // Fallback: Try just district
            filtered = _allRecords.Where(r => 
                r.District.Contains(district, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
            
        if (!filtered.Any())
            return new HistoricalRiskData { EventCount = 0, AvgSeverity = 0, AvgAffected = 0, AvgHousesDamaged = 0, AvgDeaths = 0 };

        return new HistoricalRiskData
        {
            EventCount = filtered.Count,
            // Guard against empty sequences throwing InvalidOperationException
            AvgSeverity = filtered.Any() ? filtered.Average(r => r.SeverityScore) : 0,
            AvgAffected = filtered.Any() ? filtered.Average(r => r.Affected) : 0,
            AvgHousesDamaged = filtered.Any() ? filtered.Average(r => r.HousesDestroyed) : 0,
            AvgDeaths = filtered.Any() ? filtered.Average(r => r.Deaths) : 0
        };
    }
}

public class FloodRecord
{
    public string Province { get; set; } = "";
    public string District { get; set; } = "";
    public string Division { get; set; } = "";
    public float Deaths { get; set; }
    public float HousesDestroyed { get; set; }
    public float Affected { get; set; }
    public float SeverityScore { get; set; }
}

public class HistoricalRiskData
{
    public int EventCount { get; set; } = 0;
    public float AvgSeverity { get; set; } = 0;
    public float AvgAffected { get; set; } = 0;
    public float AvgHousesDamaged { get; set; } = 0;
    public float AvgDeaths { get; set; } = 0;
}
