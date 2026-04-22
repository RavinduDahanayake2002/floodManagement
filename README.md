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

### 🌦️ Real-Time Weather Insights
- **Open-Meteo Integration**: Live temperature, rainfall (mm), and forecast updates to predict immediate flood threats.

### 📋 Insurance Claim Management
- **Damage Reporting**: Policyholders can submit flood damage reports with location, description, and photos.
- **Admin Dashboard**: Review, approve, and process insurance claims with full audit history.

---

## 🏗️ Technical Stack

- **Frontend/Backend**: [.NET 8 Blazor Web App (Interactive Server Mode)](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- **REST API**: ASP.NET Core Minimal APIs with Swagger/OpenAPI documentation
- **Machine Learning**: Python (Scikit-learn, Random Forest), joblib for model serialization.
- **Mapping**: [Leaflet.js](https://leafletjs.com/) with OpenStreetMap.
- **Data APIs**:
  - **Geoapify**: For geocoding and location search.
  - **Open-Meteo**: For real-time meteorological data.

---

## 🔌 REST API Reference

The system exposes a fully documented REST API accessible via the **Swagger UI** at:

```
http://localhost:5200/api-docs
```

### Endpoints at a Glance

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET`  | `/api/flood/check` | Flood risk assessment (weather + ML + GIS) |
| `GET`  | `/api/ml/predictions/{division}` | ML severity & affected-people prediction |
| `GET`  | `/api/historical/events` | Historical flood events (filterable) |
| `GET`  | `/api/historical/summary` | Aggregate statistics (deadliest, decade freq) |
| `GET`  | `/api/claims` | List all insurance claims |
| `POST` | `/api/claims` | Submit a new damage report |
| `PATCH`| `/api/claims/{id}/status` | Update claim status |
| `GET`  | `/api/claims/summary` | Status-count breakdown |
| `GET`  | `/api/location/provinces` | All provinces |
| `GET`  | `/api/location/districts/{provinceId}` | Districts for a province |
| `GET`  | `/api/location/divisions/{districtId}` | DS divisions for a district |
| `GET`  | `/api/location/towns/{divisionId}` | Towns for a DS division |
| `GET`  | `/api/geocode/reverse` | Coordinates → address |
| `GET`  | `/api/geocode/search` | Address → coordinates |
| `GET`  | `/api/health` | API health check |

### Example: Flood Risk Check

```http
GET /api/flood/check?province=Western&district=Colombo&division=Colombo&lat=6.9271&lng=79.8612
```

**Response:**
```json
{
  "riskLevel": "HIGH",
  "riskScore": 0.82,
  "currentRainfall": 12.5,
  "mlPredictedSeverity": "HIGH",
  "predictedAffected": 1340,
  "historicalEventCount": 18,
  "historicalAvgAffected": 956.4,
  "message": "High flood risk detected based on GIS and ML analysis.",
  "wasEscalated": true,
  "escalationReason": "ML model escalated risk from MEDIUM to HIGH.",
  "timestamp": "2025-04-22T10:00:00Z"
}
```

### Example: Submit a Damage Claim

```http
POST /api/claims
Content-Type: application/json

{
  "fullName": "Nimal Perera",
  "address": "123 Galle Road, Colombo 03",
  "contactNumber": "0771234567",
  "description": "Ground floor completely flooded. Furniture and appliances damaged.",
  "latitude": 6.9016,
  "longitude": 79.8659,
  "damageRate": 7.5,
  "propertyValue": 5000000
}
```

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

### 📦 Prerequisites
- **[.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**: The only software required to run this project.
- **Git** (Optional): For cloning via command line instead of downloading the ZIP.
- **API Keys**: Add your `GEOAPIFY_API_KEY` to a `.env` file in the project root (same folder as `FloodApp.csproj`):
  ```
  GEOAPIFY_API_KEY=your_key_here
  ```

### ⚙️ Installation & Run

1. **Download the project**:
   - Clone via Git:
     ```powershell
     git clone <repository-url>
     ```
   - *OR* Download the ZIP folder from Git and extract it.

2. **Navigate into the project**:
   ```powershell
   cd path/to/slic-flood-management
   ```

3. **Restore Dependencies**:
   ```powershell
   dotnet restore
   ```

4. **Run the Application**:
   ```powershell
   dotnet run --urls "http://localhost:5200"
   ```
   > Open **[http://localhost:5200](http://localhost:5200)** in your browser.  
   > API documentation is at **[http://localhost:5200/api-docs](http://localhost:5200/api-docs)**.

### 📱 Testing on Any Device (Mobile, Tablet)

1. Ensure your computer and phone are on the **same Wi-Fi network**.
2. Run with LAN exposure:
   ```powershell
   dotnet run --urls "http://0.0.0.0:5200"
   ```
3. Find your computer's local IP (run `ipconfig` in terminal).
4. Open `http://<YOUR_IP_ADDRESS>:5200` in your phone's browser.

### 🚀 Running Without Installing .NET

1. Double-click the **`Publish-Standalone.bat`** file in the project folder.
2. Open the newly created **`Dist`** folder.
3. Copy `FloodApp.exe` (from the Windows folder) to any Windows machine and double-click to run. No installation required.

---

## 📂 Project Structure

| Folder | Purpose |
|--------|---------|
| **Pages** | Core UI routes (`Landing`, `RiskLocation`, `PremiumCalc`) |
| **Components** | UI widgets like `WeatherWidget`, `RiskMapView`, and `LocationForm` |
| **Services** | Backend logic for `RiskAssessment`, `WeatherService`, and `MLService` |
| **FloodCheckEndpoints.cs** | All REST API endpoint definitions |
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
