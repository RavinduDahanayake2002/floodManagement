using System.Data;
using System.Text;
using ExcelDataReader;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace FloodApp.Services;

public class FloodData
{
    [LoadColumn(0)] public float Latitude { get; set; }
    [LoadColumn(1)] public float Longitude { get; set; }
    [LoadColumn(2)] public float Rainfall { get; set; }
    [LoadColumn(3)] public float RiverLevel { get; set; }
    [LoadColumn(4)] public float SoilSaturation { get; set; }
    [LoadColumn(5)] public float Month { get; set; }
    [LoadColumn(6)] public bool IsFlood { get; set; }
}

public class FloodPrediction
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    public float Probability { get; set; }

    public float Score { get; set; }
}

public class AIFloodPredictor
{
    private readonly MLContext _mlContext;
    private ITransformer _model;
    private PredictionEngine<FloodData, FloodPrediction> _predictionEngine;
    private readonly string _modelPath = "flood_model.zip";

    public AIFloodPredictor()
    {
        _mlContext = new MLContext(seed: 0);
        
        // Ensure support for older Excel formats
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        InitializeModel();
    }

    private void InitializeModel()
    {
        if (File.Exists(_modelPath))
        {
            // Load existing model
            _model = _mlContext.Model.Load(_modelPath, out var schema);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<FloodData, FloodPrediction>(_model);
        }
        else
        {
            // Train a new model
            TrainModel();
        }
    }

    private void TrainModel()
    {
        var dataFiles = new[] { "dataset/DI_Report103734.xls", "../dataset/DI_Report103734.xls" };
        string dataPath = dataFiles.FirstOrDefault(File.Exists);

        var dataList = new List<FloodData>();
        var rand = new Random(42);

        if (dataPath != null)
        {
            using var stream = File.Open(dataPath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
            });

            var table = result.Tables[0];
            
            foreach (DataRow row in table.Rows)
            {
                // Parse Date to get Month
                string dateStr = row["Date (YMD)"]?.ToString() ?? "";
                float month = 6; // default mid-year
                if (DateTime.TryParse(dateStr, out DateTime dt))
                    month = dt.Month;
                else if (dateStr.Length >= 7 && int.TryParse(dateStr.Substring(5, 2), out int parsedMonth))
                    month = parsedMonth;

                // Parse Lat/Lng
                float.TryParse(row["fichas.latitude"]?.ToString(), out float lat);
                float.TryParse(row["fichas.longitude"]?.ToString(), out float lng);

                if (lat == 0 || lng == 0) continue; // Skip invalid rows

                // It's a real flood, so simulate high risk conditions
                dataList.Add(new FloodData
                {
                    Latitude = lat,
                    Longitude = lng,
                    Month = month,
                    Rainfall = rand.Next(50, 250),           // Heavy rain
                    RiverLevel = rand.Next(4, 10),           // High river
                    SoilSaturation = rand.Next(75, 100),     // Saturated soil
                    IsFlood = true
                });

                // Inject a synthetic NON-FLOOD data point for the same place, different conditions
                dataList.Add(new FloodData
                {
                    Latitude = lat + (float)(rand.NextDouble() * 0.1 - 0.05), // slightly jittered location
                    Longitude = lng + (float)(rand.NextDouble() * 0.1 - 0.05),
                    Month = rand.Next(1, 13),
                    Rainfall = rand.Next(0, 15),             // Low rain
                    RiverLevel = (float)(rand.NextDouble() * 2 + 0.5), // Low river (0.5 to 2.5)
                    SoilSaturation = rand.Next(20, 50),      // Dry soil
                    IsFlood = false
                });
            }
        }
        else
        {
            // Fallback generation if file is missing
            Console.WriteLine("Warning: Dataset not found. Generating dummy dataset for AI training.");
            for (int i = 0; i < 1000; i++)
            {
                bool isFlood = rand.NextDouble() > 0.5;
                dataList.Add(new FloodData
                {
                    Latitude = (float)(6 + rand.NextDouble() * 3), // Sri lanka bounds
                    Longitude = (float)(79 + rand.NextDouble() * 2),
                    Month = rand.Next(1, 13),
                    Rainfall = isFlood ? rand.Next(50, 200) : rand.Next(0, 20),
                    RiverLevel = isFlood ? rand.Next(5, 10) : rand.Next(1, 4),
                    SoilSaturation = isFlood ? rand.Next(80, 100) : rand.Next(20, 60),
                    IsFlood = isFlood
                });
            }
        }

        // Load data into ML.NET
        IDataView trainingData = _mlContext.Data.LoadFromEnumerable(dataList);

        // Define Pipeline
        var pipeline = _mlContext.Transforms.Concatenate("Features", 
                nameof(FloodData.Latitude), 
                nameof(FloodData.Longitude), 
                nameof(FloodData.Rainfall), 
                nameof(FloodData.RiverLevel), 
                nameof(FloodData.SoilSaturation), 
                nameof(FloodData.Month))
            .Append(_mlContext.BinaryClassification.Trainers.FastTree(
                labelColumnName: nameof(FloodData.IsFlood), 
                featureColumnName: "Features"));

        // Train Model
        Console.WriteLine("Training AI Flood Model...");
        _model = pipeline.Fit(trainingData);
        Console.WriteLine("Training Complete.");

        // Save for future
        _mlContext.Model.Save(_model, trainingData.Schema, _modelPath);

        // Create engine
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<FloodData, FloodPrediction>(_model);
    }

    public AIFloodResult PredictRisk(double lat, double lng, double rainfall, double riverLevel, double soilSaturation, int month)
    {
        var input = new FloodData
        {
            Latitude = (float)lat,
            Longitude = (float)lng,
            Rainfall = (float)rainfall,
            RiverLevel = (float)riverLevel,
            SoilSaturation = (float)soilSaturation,
            Month = month
        };

        var prediction = _predictionEngine.Predict(input);

        // Map probability to Risk Level
        string riskLevel = "Low";
        if (prediction.Probability >= 0.70f) riskLevel = "High";
        else if (prediction.Probability >= 0.40f) riskLevel = "Medium";

        return new AIFloodResult
        {
            ProbabilityPercent = (int)Math.Round(prediction.Probability * 100),
            RiskLevel = riskLevel,
            RainfallImpact = CalculateImpact(rainfall, 50, 100),
            RiverImpact = CalculateImpact(riverLevel, 4, 7),
            SoilImpact = CalculateImpact(soilSaturation, 60, 85)
        };
    }

    private int CalculateImpact(double val, double medThreshold, double highThreshold)
    {
        if (val >= highThreshold) return 3; // High Impact
        if (val >= medThreshold) return 2;  // Medium Impact
        return 1; // Low Impact
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
