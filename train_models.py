import pandas as pd
import numpy as np
import json
import os
import joblib
from sklearn.ensemble import RandomForestRegressor, RandomForestClassifier
from sklearn.preprocessing import LabelEncoder

# 1. Load Data
file_path = 'dataset/DI_report13225.xls'
df = pd.read_csv(file_path, sep='\t', skiprows=1, on_bad_lines='skip', low_memory=False)
df.columns = df.columns.str.strip()

# Clean and select columns
df = df.rename(columns={
    'Date (YMD)': 'Date',
    'DS Division': 'Division'
})

keep_cols = ['Province', 'District', 'Division', 'Date', 'Houses Damaged', 'Affected', 'Deaths']
df_clean = df[[c for c in keep_cols if c in df.columns]].copy()

# Ensure numeric
df_clean['Affected'] = pd.to_numeric(df_clean['Affected'], errors='coerce').fillna(0).astype('int32')
df_clean['Houses Damaged'] = pd.to_numeric(df_clean['Houses Damaged'], errors='coerce').fillna(0).astype('int32')
df_clean['Deaths'] = pd.to_numeric(df_clean['Deaths'], errors='coerce').fillna(0).astype('int32')

# Drop invalid rows
df_clean = df_clean.dropna(subset=['Province', 'District', 'Division', 'Date'])
df_clean['Province'] = df_clean['Province'].str.strip()
df_clean['District'] = df_clean['District'].str.strip()
df_clean['Division'] = df_clean['Division'].str.strip()
df_clean['Date'] = pd.to_datetime(df_clean['Date'], errors='coerce')
df_clean = df_clean.dropna(subset=['Date'])

# 2. Feature Engineering for Model 1 (Predict Affected)
df_clean['Month'] = df_clean['Date'].dt.month
# NE Monsoon is typically Dec, Jan, Feb
df_clean['NE_Monsoon'] = df_clean['Month'].isin([12, 1, 2]).astype(int)

# Historical Average Affected per Division
division_avg_affected = df_clean.groupby('Division')['Affected'].mean().to_dict()
df_clean['Hist_Avg_Affected'] = df_clean['Division'].map(division_avg_affected)

# Prepare Model 1 Data
X1 = df_clean[['Hist_Avg_Affected', 'Month', 'NE_Monsoon']]
y1 = df_clean['Affected']

print("Training Model 1 (Random Forest Regressor) - Predict Affected")
rf_reg = RandomForestRegressor(n_estimators=100, random_state=42)
rf_reg.fit(X1, y1)
print(f"Model 1 R^2 Score: {rf_reg.score(X1, y1):.4f}")

# 3. Feature Engineering for Model 2 (Division Severity)
# Aggregate data at the division level
div_stats = df_clean.groupby(['Province', 'District', 'Division']).agg({
    'Affected': 'mean',
    'Houses Damaged': 'mean',
    'Date': 'count' # Number of events
}).reset_index()
div_stats = div_stats.rename(columns={'Date': 'Event_Count', 'Affected': 'Avg_Affected', 'Houses Damaged': 'Avg_Houses_Damaged'})

# Determine Severity Labels (Target for classification)
# Sort to cleanly cut into 24 HIGH, 70 MEDIUM, Rest LOW (approx based on user spec)
# We will sort by Avg Affected * Event count to get a historical risk proxy
div_stats['Risk_Proxy'] = div_stats['Avg_Affected'] * div_stats['Event_Count']
div_stats = div_stats.sort_values(by='Risk_Proxy', ascending=False).reset_index(drop=True)

# Assign labels based on rankings (Top 24 High, next 70 Med, Rest Low)
def assign_severity(idx):
    if idx < 24: return 'HIGH'
    elif idx < 24 + 70: return 'MEDIUM'
    else: return 'LOW'

div_stats['Severity_Target'] = [assign_severity(i) for i in range(len(div_stats))]

# Train Model 2
X2 = div_stats[['Avg_Affected', 'Event_Count', 'Avg_Houses_Damaged']]
y2 = div_stats['Severity_Target']

print("\nTraining Model 2 (Random Forest Classifier) - Division Severity")
rf_clf = RandomForestClassifier(n_estimators=100, random_state=42)
rf_clf.fit(X2, y2)
print(f"Model 2 Accuracy: {rf_clf.score(X2, y2) * 100:.2f}%")

# 4. Save Models
os.makedirs('ml_models', exist_ok=True)
joblib.dump(rf_reg, 'ml_models/rf_reg_affected.pkl')
joblib.dump(rf_clf, 'ml_models/rf_clf_severity.pkl')
print("\nModels saved to ml_models/")

# 5. Export Predictions for UI into JSON
# The UI needs to query divisions and get: Severity, Predicted Affected (assuming current month/monsoon conditions)
current_month = getattr(pd.Timestamp.now(), 'month')
is_ne_monsoon = 1 if current_month in [12, 1, 2] else 0

predictions = []
for index, row in div_stats.iterrows():
    div = row['Division']
    hist_avg = division_avg_affected.get(div, 0)
    
    # Predict current affected count using Model 1 for this division
    pred_affected = rf_reg.predict(pd.DataFrame([[hist_avg, current_month, is_ne_monsoon]], 
                                            columns=['Hist_Avg_Affected', 'Month', 'NE_Monsoon']))[0]
                                            
    # Predict severity using Model 2
    pred_sev = rf_clf.predict(pd.DataFrame([[row['Avg_Affected'], row['Event_Count'], row['Avg_Houses_Damaged']]], 
                                        columns=['Avg_Affected', 'Event_Count', 'Avg_Houses_Damaged']))[0]
                                        
    predictions.append({
        "division": div,
        "province": row['Province'],
        "district": row['District'],
        "predictedAffected": int(pred_affected),
        "predictedSeverity": pred_sev,
        "historicalEventCount": int(row['Event_Count']),
        "historicalAvgAffected": float(row['Avg_Affected'])
    })

os.makedirs('wwwroot/data', exist_ok=True)
with open('wwwroot/data/ml_predictions.json', 'w') as f:
    json.dump(predictions, f, indent=2)
print("Saved predictions to wwwroot/data/ml_predictions.json")

# 6. Save Clean Dataset for HistoricalDataService
df_clean.to_csv('dataset/flood_data_clean.csv', index=False)
print("Clean dataset saved to dataset/flood_data_clean.csv")
