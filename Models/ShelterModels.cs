namespace FloodApp.Models;

public enum ShelterType
{
    School,
    CommunityCenter,
    Temple,
    Hospital,
    Other
}

public class Shelter
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public ShelterType Type { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public string ContactNumber { get; set; } = "";
    public int Capacity { get; set; }
    public int CurrentOccupancy { get; set; }
    
    // Distance in km calculated dynamically relative to user's point
    public double DistanceKm { get; set; } 
    
    public bool IsFull => CurrentOccupancy >= Capacity;
}
