import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report, accuracy_score
import joblib
import os

print("--- Data Loading & Preprocessing ---")
# 1. Load the dataset
df = pd.read_excel('dataset/DI_Report103734.xls')
print(f"Loaded {len(df)} historical flood records.")

# 2. Add Severity Score based on prompt rules
df['SeverityScore'] = (df['Deaths'].fillna(0) * 10) + (df['Houses Destroyed'].fillna(0) * 2) + (df['Affected'].fillna(0) / 100)

# 3. Create 'Month' feature from Date
df['Date (YMD)'] = pd.to_datetime(df['Date (YMD)'], errors='coerce')
df['Month'] = df['Date (YMD)'].dt.month.fillna(6)

# 4. Generate Synthetic 'Non-Flood' Data and add Rainfall
# The current dataset only has floods. We need 'normal' days to teach the AI what safe looks like.
print("Generating synthetic background data...")
synthetic_data = []

np.random.seed(42)

for index, row in df.iterrows():
    # Record actual flood
    lat = pd.to_numeric(row['fichas.latitude'], errors='coerce')
    lng = pd.to_numeric(row['fichas.longitude'], errors='coerce')
    
    if pd.isna(lat) or pd.isna(lng): continue
        
    severity = row['SeverityScore']
    
    # 1. Flood Event (Label = 1)
    # Give it high rainfall since a flood happened
    synthetic_data.append({
        'Latitude': lat, 'Longitude': lng, 'Month': row['Month'],
        'Rainfall_mm': np.random.uniform(100, 300), 
        'Historical_Severity': severity,
        'IsFlood': 1
    })
    
    # 2. Non-Flood Event (Label = 0)
    # Same location, different month, low rainfall
    synthetic_data.append({
        'Latitude': lat + np.random.uniform(-0.05, 0.05),
        'Longitude': lng + np.random.uniform(-0.05, 0.05),
        'Month': np.random.randint(1, 13),
        'Rainfall_mm': np.random.uniform(0, 50),
        'Historical_Severity': severity, # Location history remains same
        'IsFlood': 0
    })

ml_df = pd.DataFrame(synthetic_data)
print(f"Final ML Dataset size: {len(ml_df)} rows")

print("\n--- Model Training ---")
X = ml_df[['Latitude', 'Longitude', 'Month', 'Rainfall_mm', 'Historical_Severity']]
y = ml_df['IsFlood']

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Using Random Forest as it handles non-linear relationships well
model = RandomForestClassifier(n_estimators=100, random_state=42)
model.fit(X_train, y_train)

# Evaluate
predictions = model.predict(X_test)
print("\nEvaluation Metrics:")
print(f"Accuracy: {accuracy_score(y_test, predictions):.2f}")
print(classification_report(y_test, predictions))

# Save the model
print("\n--- Saving Model ---")
os.makedirs('ml_models', exist_ok=True)
joblib.dump(model, 'ml_models/flood_rf_model.pkl')
print("Model saved to ml_models/flood_rf_model.pkl")

# Generate FastAPI server template
api_code = """from fastapi import FastAPI
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
"""

with open('ml_api.py', 'w') as f:
    f.write(api_code)

print("FastAPI server code generated as ml_api.py")
