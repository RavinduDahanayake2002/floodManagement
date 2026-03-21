using FloodApp.Models;

namespace FloodApp.State;

public class AppState
{
    public int? SelectedProvinceId { get; private set; }
    public int? SelectedDistrictId { get; private set; }
    public int? SelectedDivisionId { get; private set; }
    public int? SelectedTownId { get; private set; }
    public LatLng? SelectedLatLng { get; private set; }
    public string? SelectedLocationName { get; private set; }
    public string? FullAddress { get; private set; }
    
    public LatLng? MapCenter { get; private set; }
    public int MapZoom { get; private set; } = 8;
    
    public RiskResult CurrentRisk { get; private set; } = RiskResult.Default;

    public event Action? OnChange;

    public void SetProvince(int? provinceId, LatLng? coords = null)
    {
        SelectedProvinceId = provinceId;
        SelectedDistrictId = null;
        SelectedDivisionId = null;
        SelectedTownId = null;
        
        if (coords != null)
        {
            MapCenter = coords;
            MapZoom = 9;
        }

        NotifyStateChanged();
    }

    public void SetDistrict(int? districtId, LatLng? coords = null)
    {
        SelectedDistrictId = districtId;
        SelectedDivisionId = null;
        SelectedTownId = null;
        
        if (coords != null)
        {
            MapCenter = coords;
            MapZoom = 11;
        }

        NotifyStateChanged();
    }

    public void SetDivision(int? divisionId, LatLng? coords = null, string? divisionName = null)
    {
        SelectedDivisionId = divisionId;
        SelectedTownId = null;
        
        if (coords != null)
        {
            SelectedLatLng = coords;
            MapCenter = coords;
            MapZoom = 12;
        }
        if (divisionName != null) {
            SelectedLocationName = divisionName;
        }

        NotifyStateChanged();
    }

    public void SetTown(int? townId, LatLng? coords, string? locationName)
    {
        SelectedTownId = townId;
        if (coords != null) SelectedLatLng = coords;
        if (locationName != null) SelectedLocationName = locationName;
        NotifyStateChanged();
    }

    public void SetCustomLocation(LatLng coords, string locationName)
    {
        SelectedLatLng = coords;
        SelectedLocationName = locationName;
        
        SelectedProvinceId = null;
        SelectedDistrictId = null;
        SelectedDivisionId = null;
        SelectedTownId = null;
        
        NotifyStateChanged();
    }
    
    public void SetExplicitLocation(LatLng latLng, string? locationName = null)
    {
        SelectedLatLng = latLng;
        SelectedLocationName = locationName;
        // Reset hierarchy if picking freely on map? Or keep context? 
        // User spec says "Choose on map" stores SelectedLatLng.
        NotifyStateChanged();
    }

    public void SetRiskResult(RiskResult result)
    {
        CurrentRisk = result;
        NotifyStateChanged();
    }

    public void SetFullAddress(string? address)
    {
        FullAddress = address;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
