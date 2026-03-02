# Quick‑Start Guide: GET `/api/flood/check` Endpoint

## Overview
This guide walks you through running the new flood‑check API endpoint that was added to the SLIC Flood Management application. Supervisors can use it to programmatically retrieve flood‑risk information for a specific location.

## Steps
| Step | Action | Command / Detail |
|------|--------|-------------------|
| **1️⃣ Prerequisites** | • .NET 8 SDK installed<br>• `dotnet` on your PATH | `dotnet --version` should show **8.x** |
| **2️⃣ Open a terminal** | Navigate to the project root | `cd d:\My_PaidProjects\slic-flood-management` |
| **3️⃣ Restore & Build** | Pull NuGet packages & compile the app | `dotnet build` *(should finish with “Build succeeded”)* |
| **4️⃣ Run the app** | Start the Blazor Server host (it also hosts the Minimal API) | `dotnet run`<br>or for hot‑reload: `dotnet watch run` |
| **5️⃣ Locate the URL** | By default the dev server listens on **https://localhost:5001** (HTTPS) and **http://localhost:5000** (HTTP). |
| **6️⃣ Call the endpoint** | Use a browser, curl, Postman, or any HTTP client. Example with curl: | ```bash
curl "https://localhost:5001/api/flood/check?province=Western&district=Colombo&division=Colombo&lat=6.9271&lng=79.8612" -k
```<br>(`-k` skips the self‑signed dev certificate) |
| **7️⃣ Expected JSON response** | You’ll receive a payload similar to: | ```json
{
  "riskLevel": "MEDIUM",
  "riskScore": 5.0,
  "currentRainfall": 12.3,
  "mlPredictedSeverity": "MEDIUM",
  "predictedAffected": 150,
  "historicalEventCount": 8,
  "historicalAvgAffected": 120.5,
  "message": "Located in Colombo flood zone",
  "wasEscalated": false,
  "escalationReason": null,
  "timestamp": "2026-03-02T16:30:00Z"
}
``` |
| **8️⃣ Verify integration** | • The UI pages (`/`, `/location`, `/risk`, etc.) should still work.<br>• The endpoint should return data without errors (check the console output for any exceptions). |
| **9️⃣ Stop the app** | Press **Ctrl +C** in the terminal when you’re done. |

## Tips & Gotchas
- **HTTPS certificate**: The dev server uses a self‑signed cert. Browsers will warn; you can proceed or use `curl -k` as shown.
- **Port conflicts**: If another process occupies ports **5000/5001**, stop it or change the ports in `launchSettings.json`.
- **Service dependencies**: The endpoint re‑uses existing services (`RiskService`, `WeatherService`, `MLPredictionService`, `HistoricalDataService`). No extra configuration is needed.
- **Testing with Postman**: Set the request method to **GET**, paste the full URL with query parameters, and hit **Send**.

That’s it—once the app is running, supervisors can hit the endpoint to retrieve flood‑risk information programmatically.
