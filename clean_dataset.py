import pandas as pd

# Load the raw dataset
df = pd.read_excel('dataset/DI_Report103734.xls', engine='xlrd')

# Keep only the 5 required columns
keep_cols = ['Province', 'District', 'Division', 'Houses Damaged', 'Affected']
df_clean = df[keep_cols].copy()

# Drop rows where all numeric values are NaN
df_clean = df_clean.dropna(subset=['Houses Damaged', 'Affected'], how='all')

# Fill remaining NaN with 0 for numeric columns
df_clean['Houses Damaged'] = pd.to_numeric(df_clean['Houses Damaged'], errors='coerce').fillna(0).astype(int)
df_clean['Affected'] = pd.to_numeric(df_clean['Affected'], errors='coerce').fillna(0).astype(int)

# Strip whitespace from text columns
df_clean['Province'] = df_clean['Province'].str.strip()
df_clean['District'] = df_clean['District'].str.strip()
df_clean['Division'] = df_clean['Division'].str.strip()

# Drop rows with missing location info
df_clean = df_clean.dropna(subset=['Province', 'District', 'Division'])

# Save cleaned dataset
df_clean.to_csv('dataset/flood_data_clean.csv', index=False)

# Print summary
print(f"Original rows: {len(df)}")
print(f"Cleaned rows:  {len(df_clean)}")
print(f"\nColumns: {list(df_clean.columns)}")
print(f"\nSample data:")
print(df_clean.head(10).to_string(index=False))
print(f"\nUnique Provinces:  {df_clean['Province'].nunique()}")
print(f"Unique Districts:  {df_clean['District'].nunique()}")
print(f"Unique Divisions:  {df_clean['Division'].nunique()}")
