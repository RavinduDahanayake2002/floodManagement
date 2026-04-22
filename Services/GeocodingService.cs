using System.Net.Http.Json;
using System.Text.Json;
using FloodApp.Models;

namespace FloodApp.Services;

public class GeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;

    public GeocodingService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = Environment.GetEnvironmentVariable("GEOAPIFY_API_KEY") 
                  ?? configuration["GEOAPIFY_API_KEY"];
        
        _httpClient.BaseAddress = new Uri("https://api.geoapify.com/v1/geocode/");
    }

    /// <summary>
    /// Geocode an address using Geoapify API.
    /// Strictly limited to Sri Lanka.
    /// </summary>
    public async Task<GeocodeResult?> GeocodeAsync(string address)
    {
        if (string.IsNullOrEmpty(_apiKey)) return null;

        try
        {
            var url = $"search?text={Uri.EscapeDataString(address)}&apiKey={_apiKey}&filter=countrycode:lk&limit=1";
            var response = await _httpClient.GetFromJsonAsync<JsonElement>(url);

            var features = response.GetProperty("features");
            if (features.GetArrayLength() == 0)
                return null;

            var feature = features[0];
            var properties = feature.GetProperty("properties");
            var geometry = feature.GetProperty("geometry").GetProperty("coordinates");
            
            return new GeocodeResult
            {
                Lat = geometry[1].GetDouble(),
                Lon = geometry[0].GetDouble(),
                DisplayName = properties.GetProperty("formatted").GetString() ?? address
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GeocodingService] Geoapify Geocoding Error: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Reverse geocode coordinates using Geoapify API.
    /// Strictly limited to Sri Lanka.
    /// </summary>
    public async Task<string?> ReverseGeocodeAsync(double lat, double lon)
    {
        if (string.IsNullOrEmpty(_apiKey)) return null;

        try
        {
            var url = $"reverse?lat={lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}&lon={lon.ToString(System.Globalization.CultureInfo.InvariantCulture)}&apiKey={_apiKey}&filter=countrycode:lk&limit=1";
            var response = await _httpClient.GetFromJsonAsync<JsonElement>(url);

            var features = response.GetProperty("features");
            if (features.GetArrayLength() == 0)
                return null;

            return features[0].GetProperty("properties").GetProperty("formatted").GetString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GeocodingService] Geoapify Reverse Geocoding Error: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Checks whether coordinates fall within Sri Lanka's bounding box.
    /// </summary>
    public static bool IsInSriLanka(double lat, double lon)
        => lat >= 5.9 && lat <= 9.9 && lon >= 79.5 && lon <= 82.0;
}

public class GeocodeResult
{
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string DisplayName { get; set; } = "";
}
