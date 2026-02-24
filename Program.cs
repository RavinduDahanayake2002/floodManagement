using FloodApp.Components;
using FloodApp.Services;
using FloodApp.State;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<LocationService>();
builder.Services.AddSingleton<AdminService>();
builder.Services.AddScoped<AppState>();
builder.Services.AddSingleton<GeoJsonService>();
builder.Services.AddSingleton<RiskService>();
builder.Services.AddSingleton<ShelterService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<WeatherService>();
builder.Services.AddSingleton<HistoricalDataService>();
builder.Services.AddScoped<AIFloodPredictor>();


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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
