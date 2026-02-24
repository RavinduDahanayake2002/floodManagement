from fastapi import FastAPI
from pydantic import BaseModel
import joblib
import pandas as pd
import numpy as np

app = FastAPI(title="SLIC Division Flood Predictor API")
try:
    model = joblib.load('ml_models/division_rf_model.pkl')
    le = joblib.load('ml_models/division_encoder.pkl')
except:
    print("Warning: Models not found, train first using build_division_model.py")

class PredictionRequest(BaseModel):
    division: str
    month: int

@app.post("/predict")
def predict_risk(req: PredictionRequest):
    # Handle unknown divisions safely
    div = req.division.strip().upper()
    try:
        div_encoded = le.transform([div])[0]
    except:
        # Fallback if division not seen in training
        div_encoded = 0 

    input_data = pd.DataFrame([{
        'Division_Encoded': div_encoded,
        'Month': req.month
    }])
    
    predicted_affected = model.predict(input_data)[0]
    
    risk_level = "LOW"
    if predicted_affected > 1000: risk_level = "HIGH"
    elif predicted_affected > 100: risk_level = "MEDIUM"

    return {
        "predicted_affected": int(predicted_affected),
        "risk_level": risk_level,
        "division": div
    }
