from fastapi import FastAPI
from pydantic import BaseModel
import joblib
import pandas as pd

app = FastAPI(title="SLIC Flood Predictor API")
model = joblib.load('ml_models/flood_rf_model.pkl')

class PredictionRequest(BaseModel):
    latitude: float
    longitude: float
    month: int
    rainfall_mm: float
    historical_severity: float

@app.post("/predict")
def predict_risk(req: PredictionRequest):
    input_data = pd.DataFrame([{
        'Latitude': req.latitude,
        'Longitude': req.longitude,
        'Month': req.month,
        'Rainfall_mm': req.rainfall_mm,
        'Historical_Severity': req.historical_severity
    }])
    
    probability = model.predict_proba(input_data)[0][1]
    
    risk_level = "LOW"
    if probability > 0.7: risk_level = "HIGH"
    elif probability > 0.4: risk_level = "MEDIUM"

    return {
        "probability": float(probability),
        "risk_level": risk_level
    }
