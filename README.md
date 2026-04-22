# 🌊 SLIC Flood Management System

Welcome to the **SLIC Flood Management System**, a state-of-the-art risk assessment and emergency response platform. Developed for the **Sri Lanka Insurance Corporation (SLIC)**, this application leverages real-time data, geospatial intelligence, and machine learning to protect lives and property across Sri Lanka.

---

## ✨ Core Features

### 🌎 Smart Location Intelligence
- **Cascading Selection**: Choose Province → District → Divisional Secretariat with a seamless, responsive UI.
- **Geoapify Integration**: Precise geocoding tailored for Sri Lankan addresses.
- **Dynamic Mapping**: Interactive Leaflet.js maps with automated zooming and region highlighting.

- **Enhanced Historical Reporting**: Detailed risk assessment based on 20+ years of historical data and localized impact analysis.
  - **Affected Population**: Estimated impact for the current season.
  - **Risk Severity**: AI-classified "HIGH," "MEDIUM," or "LOW" risk ratings based on 20+ years of historical data.

### 🛡️ Interactive Risk Mapping
- **Heatmap Overlays**: Pulse and "Blast Radius" animations visualize risk zones in real-time.
- **Point-in-Polygon Engine**: Accurate risk detection using GeoJSON boundaries for Sri Lankan administrative divisions.

### 🏥 Emergency Shelter Management
- **Live Shelter Directory**: Locate the nearest safe zones instantly.
- **Capacity Tracking**: Real-time occupancy data to ensure shelters aren't overwhelmed.

### 🌦️ Real-Time Weather Insights
- **Open-Meteo Integration**: Live temperature, rainfall (mm), and forecast updates to predict immediate flood threats.

---

## 🏗️ Technical Stack

- **Frontend/Backend**: [.NET 8 Blazor Web App (Interactive Server Mode)](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- **Machine Learning**: Python (Scikit-learn, Random Forest), joblib for model serialization.
- **Mapping**: [Leaflet.js](https://leafletjs.com/) with OpenStreetMap.
- **Data APIs**: 
  - **Geoapify**: For geocoding and location search.
  - **Open-Meteo**: For real-time meteorological data.

---

## 🧠 Machine Learning Pipeline

Our predictive engine is trained on the **DesInventar (Sri Lanka)** historical flood dataset (1970–Present).

### Training the Models
If you wish to re-train the models with the latest `dataset/DI_report.xls`:
1. Ensure you have the Python 3.10+ environment set up.
2. Run the training script:
   ```powershell
   # Activate virtual environment
   .venv\Scripts\activate
   # Run the trainer
   python train_models.py
   ```
3. **Artifacts Generated**:
   - `ml_models/rf_reg_affected.pkl`: Regression model for impact prediction.
   - `ml_models/rf_clf_severity.pkl`: Classification model for risk levels.
   - `wwwroot/data/ml_predictions.json`: Pre-processed predictions for the Blazor UI.

---

## 🚀 Getting Started

<<<<<<< HEAD
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

### 🚀 How to Run on ANY Machine WITHOUT installing .NET
If you want to share this application with someone else and they **do not** have the `.NET 8 SDK` installed, you can compile the app into a "Self-Contained" single file.

1. Double-click the **`Publish-Standalone.bat`** file in the project folder.
2. It will compress the application and the entire `.NET` engine into a standalone package.
3. Open the newly created **`Dist`** folder. Inside, you will see folders for Windows, Mac, and Linux.
4. Simply copy the `FloodApp.exe` file (from the Windows folder) to any other Windows computer and double-click it to run your server locally! No installations string required.
=======
### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Python 3.10+](https://www.python.org/downloads/) (for ML training)
- **API Keys**: Add your `GEOAPIFY_API_KEY` to the `.env` file in the root directory.

### Quick Run
1. **Restore & Build**:
   ```powershell
   dotnet restore
   ```
2. **Launch Application**:
   ```powershell
   dotnet run --urls "http://localhost:5200"
   ```
3. **Internal Network Access**:
   Access the system via your LAN at `http://[YOUR_IP]:5200`.
>>>>>>> main

---

## 📂 Project Structure

| Folder | Purpose |
|--------|---------|
| **Pages** | Core UI routes (`Landing`, `RiskLocation`, `PremiumCalc`) |
| **Components** | UI widgets like `WeatherWidget`, `RiskMapView`, and `LocationForm` |
| **Services** | Backend logic for `RiskAssessment`, `WeatherService`, and `MLService` |
| **dataset** | Raw historical flood data (DesInventar) |
| **ml_models** | Serialized AI models (.pkl) |
| **wwwroot** | Static assets, GeoJSON maps, and pre-computed ML results |

---

## 📜 Credits & Data Sources

- **Weather Data**: Open-Meteo API
- **Geocoding**: Geoapify API
- **Historical Records**: DesInventar Disaster Information Management System
- **Framework**: Developed with passion using .NET 8 Blazor

---
*Developed for the Sri Lanka Insurance Corporation (SLIC)*
