import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestRegressor
from sklearn.preprocessing import LabelEncoder
from sklearn.metrics import mean_absolute_error, r2_score
import joblib
import os

print("--- Data Loading & Preprocessing ---")
df = pd.read_excel('dataset/DI_Report103734.xls')
print(f"Loaded {len(df)} historical flood records.")

# Clean Division Names
df['Division'] = df['Division'].astype(str).str.strip().str.upper()

# Handle Month from Date
df['Date (YMD)'] = pd.to_datetime(df['Date (YMD)'], errors='coerce')
df['Month'] = df['Date (YMD)'].dt.month.fillna(6).astype(int)

# Target Variable
df['Affected'] = df['Affected'].fillna(0).astype(int)

# We will only use rows where Affected > 0 to teach the model how bad the floods get
ml_df = df[df['Affected'] > 0][['Division', 'Month', 'Affected']].copy()

print(f"Filtered dataset to {len(ml_df)} valid impact events.")

# Save resulting CSV for user review
ml_df.to_csv('division_flood_data.csv', index=False)
print(f"Exported division_flood_data.csv with {len(ml_df)} rows")

print("\n--- Model Training (Regression) ---")

# Encode Division correctly
le = LabelEncoder()
ml_df['Division_Encoded'] = le.fit_transform(ml_df['Division'])

X = ml_df[['Division_Encoded', 'Month']]
y = ml_df['Affected']

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Using Random Forest Regressor
model = RandomForestRegressor(n_estimators=100, random_state=42)
model.fit(X_train, y_train)

predictions = model.predict(X_test)
print("\nEvaluation Metrics:")
print(f"Mean Absolute Error (Affected People): {mean_absolute_error(y_test, predictions):.2f}")
print(f"R-squared Score: {r2_score(y_test, predictions):.2f}")

print("\n--- Saving Division Regression Model ---")
os.makedirs('ml_models', exist_ok=True)
joblib.dump(model, 'ml_models/division_rf_model.pkl')
joblib.dump(le, 'ml_models/division_encoder.pkl')
print("Model and Encoder saved to ml_models/")
