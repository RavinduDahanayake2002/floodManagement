namespace FloodApp.Models;

public enum RiskLevel
{
    Low,
    Medium,
    High,
    Unknown
}

public class RiskResult
{
    public RiskLevel Level { get; set; }
    public string Message { get; set; } = "";
    public string ColorCode { get; set; } = "#10B981"; // Default green for Low
    public double Score { get; set; }
    public bool WasEscalated { get; set; } = false;
    public string? EscalationReason { get; set; }

    public static RiskResult Default => new() 
    { 
        Level = RiskLevel.Unknown, 
        Message = "Select a location to check risk.", 
        ColorCode = "#6B7280" 
    };
}

public class DailyForecast
{
    public DateTime Date { get; set; }
    public double TempMax { get; set; }
    public double TempMin { get; set; }
    public double Precipitation { get; set; }
    public int WeatherCode { get; set; }
    public string WeatherDescription => WeatherCode switch
    {
        0 => "Clear sky",
        1 => "Mainly clear",
        2 => "Partly cloudy",
        3 => "Overcast",
        45 or 48 => "Foggy",
        51 or 53 or 55 => "Drizzle",
        56 or 57 => "Freezing drizzle",
        61 => "Light rain",
        63 => "Moderate rain",
        65 => "Heavy rain",
        66 or 67 => "Freezing rain",
        71 or 73 or 75 => "Snowfall",
        77 => "Snow grains",
        80 => "Light showers",
        81 => "Moderate showers",
        82 => "Heavy showers",
        85 or 86 => "Snow showers",
        95 => "Thunderstorm",
        96 or 99 => "Thunderstorm with hail",
        _ => "Unknown"
    };
    public string WeatherIcon => WeatherCode switch
    {
        0 => "☀️",
        1 or 2 => "⛅",
        3 => "☁️",
        45 or 48 => "🌫️",
        51 or 53 or 55 or 56 or 57 => "🌦️",
        61 or 63 or 80 or 81 => "🌧️",
        65 or 82 => "🌊",
        66 or 67 => "🧊",
        71 or 73 or 75 or 77 or 85 or 86 => "❄️",
        95 or 96 or 99 => "⛈️",
        _ => "🌡️"
    };
}

public class RainfallDay
{
    public DateTime Date { get; set; }
    public double Precipitation { get; set; }
}
