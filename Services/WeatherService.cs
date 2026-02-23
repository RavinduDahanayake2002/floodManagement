using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FloodApp.Models;

namespace FloodApp.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherResult?> GetCurrentWeatherAsync(double lat, double lng)
    {
        try
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lng}&current=temperature_2m,rain,wind_speed_10m";
            
            var response = await _httpClient.GetFromJsonAsync<OpenMeteoCurrentResponse>(url);
            
            if (response?.Current == null) return null;

            return new WeatherResult
            {
                Temperature = response.Current.Temperature2m,
                Rain = response.Current.Rain,
                WindSpeed = response.Current.WindSpeed10m,
                UnitTemp = response.CurrentUnits.Temperature2m,
                UnitRain = response.CurrentUnits.Rain,
                UnitWind = response.CurrentUnits.WindSpeed10m
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching weather: {ex.Message}");
            return null;
        }
    }

    public async Task<List<DailyForecast>> GetForecastAsync(double lat, double lng)
    {
        try
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lng}&daily=temperature_2m_max,temperature_2m_min,precipitation_sum,weathercode&timezone=auto&forecast_days=7";
            
            var json = await _httpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);
            var daily = doc.RootElement.GetProperty("daily");

            var dates = daily.GetProperty("time").EnumerateArray().ToList();
            var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray().ToList();
            var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray().ToList();
            var precip = daily.GetProperty("precipitation_sum").EnumerateArray().ToList();
            var codes = daily.GetProperty("weathercode").EnumerateArray().ToList();

            var forecasts = new List<DailyForecast>();
            for (int i = 0; i < dates.Count; i++)
            {
                forecasts.Add(new DailyForecast
                {
                    Date = DateTime.Parse(dates[i].GetString()!),
                    TempMax = maxTemps[i].GetDouble(),
                    TempMin = minTemps[i].GetDouble(),
                    Precipitation = precip[i].GetDouble(),
                    WeatherCode = codes[i].GetInt32()
                });
            }

            return forecasts;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching forecast: {ex.Message}");
            return new List<DailyForecast>();
        }
    }

    public async Task<List<RainfallDay>> GetRainfallHistoryAsync(double lat, double lng)
    {
        try
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lng}&daily=precipitation_sum&timezone=auto&past_days=7&forecast_days=0";
            
            var json = await _httpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);
            var daily = doc.RootElement.GetProperty("daily");

            var dates = daily.GetProperty("time").EnumerateArray().ToList();
            var precip = daily.GetProperty("precipitation_sum").EnumerateArray().ToList();

            var history = new List<RainfallDay>();
            for (int i = 0; i < dates.Count; i++)
            {
                history.Add(new RainfallDay
                {
                    Date = DateTime.Parse(dates[i].GetString()!),
                    Precipitation = precip[i].GetDouble()
                });
            }

            return history;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching rainfall history: {ex.Message}");
            return new List<RainfallDay>();
        }
    }

    // Internal classes for Open-Meteo JSON structure
    private class OpenMeteoCurrentResponse
    {
        [JsonPropertyName("current_units")]
        public CurrentUnits CurrentUnits { get; set; } = new();

        [JsonPropertyName("current")]
        public CurrentData Current { get; set; } = new();
    }

    private class CurrentUnits
    {
        [JsonPropertyName("temperature_2m")]
        public string Temperature2m { get; set; } = "°C";

        [JsonPropertyName("rain")]
        public string Rain { get; set; } = "mm";

        [JsonPropertyName("wind_speed_10m")]
        public string WindSpeed10m { get; set; } = "km/h";
    }

    private class CurrentData
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature2m { get; set; }

        [JsonPropertyName("rain")]
        public double Rain { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed10m { get; set; }
    }
}

public class WeatherResult
{
    public double Temperature { get; set; }
    public double Rain { get; set; }
    public double WindSpeed { get; set; }
    public string UnitTemp { get; set; } = "°C";
    public string UnitRain { get; set; } = "mm";
    public string UnitWind { get; set; } = "km/h";
}

