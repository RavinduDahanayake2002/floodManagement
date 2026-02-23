# SLIC Flood Management System

An intelligent flood risk assessment application for **Sri Lanka Insurance Corporation (SLIC)**, built with **.NET 8 Blazor**. This system helps users assess flood risks for specific locations across Sri Lanka, report property damage for insurance claims, and locate emergency shelters using real-time weather data and geospatial analytics.

## 🚀 Key Features

### 🌟 Interactive Splash Screen
- **Minimalist Design**: Sleek dark theme (`#111827`) matching the application branding.
- **SLIC Branding**: Official "SLIC General" logo integration.

### 🌍 Location Intelligence
- **Cascading Selection**: Select Province -> District -> Town hierarchy.
- **Smart Navigation**: Map automatically zooms and pans to the selected region.

### 🛡️ Risk Assessment
- **Advanced Analytics**: Uses "Point-in-Polygon" algorithms to determine flood risk.
- **Visual Mapping**: Interactive Leaflet map with colored risk overlays.
- **Risk Levels**: High, Medium, Low.

### 📝 Insurance Claims & Damage Reporting
- **Report Damage**: Seamlessly report flood damage for properties directly to SLIC.
- **Evidence Collection**: Attach photographic evidence for fast claims processing.

### 🏥 Emergency Shelter Directory
- **Shelter Locator**: Find the nearest emergency shelters based on your location.
- **Live Occupancy**: Displays shelter capacity and real-time occupancy.
- **Map Integration**: Shelters are visually marked on the interactive Leaflet map.

### 🌦️ Real-Time Weather
- **Live Integration**: Connects to [Open-Meteo API](https://open-meteo.com/).
- **Current Conditions**: Displays Temperature (°C), Rainfall (mm), and Weather forecasts.

---

## 🛠️ Technology Stack

- **Framework**: [.NET 8 Blazor Web App (Interactive Server)](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- **Language**: C#
- **Map Library**: [Leaflet.js](https://leafletjs.com/) with OpenStreetMap tiles
- **Styling**: Tailwind CSS / Custom CSS Variables
- **Data**: GeoJSON for risk zones, JSON for location data

---

## 🏁 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Installation & Run

1. **Navigate to project directory**:
   ```powershell
   cd floodApp
   ```

2. **Build the application**:
   ```powershell
   dotnet restore
   dotnet build
   ```

3. **Run the server**:
   ```powershell
   dotnet run --urls "http://localhost:5200"
   ```
   > **Tip**: If you encounter an "Address already in use" error, try changing the port number.

   **For Network Access (LAN):**
   ```powershell
   dotnet run --urls "http://0.0.0.0:5200"
   ```
   *Access via your machine's IP address (e.g., http://192.168.1.50:5200)*
   ```

4. **Access the App**:
   Open **[http://localhost:5200](http://localhost:5200)** in your browser.

---

## 📂 Project Structure

| Folder | Description |
|--------|-------------|
| **Pages** | Routable Razor components (`Landing`, `LocationSelection`, `RiskLocation`) |
| **Components** | Reusable UI widgets (`WeatherWidget`, `RiskMapView`, `LocationForm`) |
| **Services** | Business logic (`RiskService`, `WeatherService`, `LocationService`) |
| **Models** | C# data definitions (`Province`, `RiskResult`, `WeatherResult`) |
| **wwwroot** | Static assets (CSS, JS, Logos, GeoJSON data) |

---

## 📜 Credits

- **Weather Data**: Open-Meteo API (Free for non-commercial use)
- **Mapping**: OpenStreetMap & Leaflet
- **Development**: Powered by .NET 8 Blazor

---
*Developed for Sri Lanka Insurance Corporation (SLIC)*
