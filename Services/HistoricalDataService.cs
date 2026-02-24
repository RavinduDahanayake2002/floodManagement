using System.Data;
using System.Text;
using ExcelDataReader;

namespace FloodApp.Services;

public class HistoricalDataService
{
    private readonly string _filePath = "dataset/DI_Report103734.xls";
    
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
            using var stream = File.Open(activePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
            });

            var table = result.Tables[0];
            
            foreach (DataRow row in table.Rows)
            {
                var record = new FloodRecord
                {
                    Province = row["Province"]?.ToString() ?? "",
                    District = row["District"]?.ToString() ?? "",
                    Division = row["Division"]?.ToString() ?? "",
                };
                
                float.TryParse(row["Deaths"]?.ToString(), out float deaths);
                float.TryParse(row["Houses Destroyed"]?.ToString(), out float destroyed);
                float.TryParse(row["Affected"]?.ToString(), out float affected);
                
                record.Deaths = deaths;
                record.HousesDestroyed = destroyed;
                record.Affected = affected;
                
                // Calculate severity: (Deaths × 10) + (Houses_Destroyed × 2) + (Affected / 100)
                record.SeverityScore = (deaths * 10) + (destroyed * 2) + (affected / 100);
                
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
