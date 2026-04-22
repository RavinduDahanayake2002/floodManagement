using FloodApp;
using FloodApp.Components;
using FloodApp.Services;
using FloodApp.State;

var builder = WebApplication.CreateBuilder(args);

// Load .env file if it exists
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        var parts = line.Split('=', 2);
        if (parts.Length != 2) continue;
        Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
    }
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<LocationService>();
builder.Services.AddSingleton<AdminService>();
builder.Services.AddSingleton<HistoricalFloodEventService>();
builder.Services.AddSingleton<AgentLocatorService>();
builder.Services.AddScoped<AppState>();
builder.Services.AddSingleton<GeoJsonService>();
builder.Services.AddSingleton<RiskService>();
builder.Services.AddSingleton<ShelterService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<WeatherService>();
builder.Services.AddSingleton<HistoricalDataService>();
builder.Services.AddSingleton<MLPredictionService>();

builder.Services.AddHttpClient<GeocodingService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapFloodCheckEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
