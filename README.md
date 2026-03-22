# SLIC Flood Management System

An intelligent flood risk assessment application for **Sri Lanka Insurance Corporation (SLIC)**, built with **.NET 8 Blazor**. This system helps users assess flood risks for specific locations across Sri Lanka, report property damage for insurance claims, and locate emergency shelters using real-time weather data and geospatial analytics.

## 🚀 Key Features

### 🌟 Interactive Splash Screen
- **Minimalist Design**: Sleek dark theme (`#111827`) matching the application branding.
- **SLIC Branding**: Official "SLIC General" logo integration.

### 🌍 Location Intelligence
- **Cascading Selection**: Select Province -> District -> Town hierarchy.
- **Smart Navigation**: Map automatically zooms and pans to the selected region.

### 🤖 AI & Machine Learning Integration
- **Generative Assessment**: Connects to **Google Gemini API** to write custom, human-readable flood risk reports.
- **Predictive ML Model**: Uses **ML.NET Random Forest** algorithms to predict risk severity and affected population using historical flood datasets.

### 🛡️ Risk Assessment
- **Advanced Analytics**: Uses "Point-in-Polygon" algorithms to determine flood risk.
- **Visual Mapping**: Interactive Leaflet map with animated, color-coded risk overlays (Pulse / Blast Radius animation).
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
- **Machine Learning**: ML.NET (Random Forest Classification/Regression)
- **AI Engine**: Google Gemini API (1.5 Flash)
- **Styling**: Tailwind CSS / Custom CSS Variables
- **Data**: GeoJSON for risk zones, JSON for location data, CSV for historical training sets

---

## 🏁 Getting Started

### 📦 Prerequisites
- **[.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**: This is the ONLY software required to run this project. Download the installer for your operating system (Windows, macOS, or Linux) and install it.
- **Git** (Optional): If you want to clone via command line instead of downloading the source code ZIP.

### ⚙️ Installation & Run

1. **Download the project**:
   - Clone via Git: 
     ```powershell
     git clone <repository-url>
     ```
   - *OR* Download the ZIP folder from Git and extract it anywhere on your computer.

2. **Navigate into the project**:
   Open a terminal (Command Prompt, PowerShell, or Terminal) and navigate to the root folder (where `FloodApp.csproj` is located):
   ```powershell
   cd path/to/slic-flood-management
   ```

3. **Install Dependencies**:
   This command automatically downloads all required NuGet packages. You do not need to check these into Git:
   ```powershell
   dotnet restore
   ```

4. **Run the Application Locally**:
   Start the local development server:
   ```powershell
   dotnet run --urls "http://localhost:5200"
   ```
   > **Tip**: Open your web browser and go to **[http://localhost:5200](http://localhost:5200)** to view the app!

### 📱 How to test on any device (Mobile Phone, Tablet)
To access the application from a completely different device (like your iPhone or Android phone), you will need to run the application on your Local Area Network (LAN):

1. Make sure your computer and your phone are connected to the **same Wi-Fi network**.
2. Run this command instead to expose the app to your network:
   ```powershell
   dotnet run --urls "http://0.0.0.0:5200"
   ```
3. Find your computer's local IP Address (e.g., `192.168.1.15`). You can find this by typing `ipconfig` in your terminal.
4. On your phone's Safari/Chrome browser, type in `http://<YOUR_IP_ADDRESS>:5200` to load and use the app fully!

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
