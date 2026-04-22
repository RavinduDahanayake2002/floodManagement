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

// ── Blazor ───────────────────────────────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── Application Services ─────────────────────────────────────────────────────
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

// ── OpenAPI / Swagger ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title       = "SLIC Flood Management API",
        Version     = "v1",
        Description =
            "REST API for the Sri Lanka Insurance Corporation (SLIC) Flood Management System. " +
            "Provides flood risk assessment, ML-based predictions, historical event data, " +
            "geocoding services, and insurance claim management.",
        Contact = new() { Name = "SLIC Flood Management Team" }
    });
});

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── HTTP Pipeline ─────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Swagger UI is available in all environments for this project
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SLIC Flood Management API v1");
    c.RoutePrefix = "api-docs"; // accessible at /api-docs
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// ── API Endpoints ─────────────────────────────────────────────────────────────
app.MapFloodCheckEndpoints();

// ── Blazor Pages ─────────────────────────────────────────────────────────────
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
