using FloodApp.Models;

namespace FloodApp.Services;

public class ShelterService
{
    private readonly List<Shelter> _mockShelters = new()
    {
        new Shelter { Name = "Colombo Royal College", Type = ShelterType.School, Lat = 6.9061, Lng = 79.8601, ContactNumber = "0112691042", Capacity = 1000, CurrentOccupancy = 450 },
        new Shelter { Name = "Gangarama Temple", Type = ShelterType.Temple, Lat = 6.9175, Lng = 79.8557, ContactNumber = "0112327084", Capacity = 500, CurrentOccupancy = 490 },
        new Shelter { Name = "Sugathadasa Indoor Stadium", Type = ShelterType.Other, Lat = 6.9458, Lng = 79.8653, ContactNumber = "0112431818", Capacity = 2000, CurrentOccupancy = 200 },
        new Shelter { Name = "Nugegoda Public Library", Type = ShelterType.CommunityCenter, Lat = 6.8687, Lng = 79.8906, ContactNumber = "0112811444", Capacity = 300, CurrentOccupancy = 120 },
        new Shelter { Name = "Kelaniya Raja Maha Vihara", Type = ShelterType.Temple, Lat = 6.9587, Lng = 79.9161, ContactNumber = "0112911225", Capacity = 1500, CurrentOccupancy = 800 },
        new Shelter { Name = "Dehiwala Maha Vidyalaya", Type = ShelterType.School, Lat = 6.8407, Lng = 79.8732, ContactNumber = "0112713456", Capacity = 800, CurrentOccupancy = 300 }
    };

    public Task<List<Shelter>> GetNearbySheltersAsync(LatLng location, double maxDistanceKm = 10.0, int limit = 3)
    {
        // Calculate distance for all mock shelters and filter
        var nearby = _mockShelters
            .Select(s => 
            {
                s.DistanceKm = CalculateDistance(location.Lat, location.Lng, s.Lat, s.Lng);
                return s;
            })
            .Where(s => s.DistanceKm <= maxDistanceKm)
            .OrderBy(s => s.DistanceKm)
            .Take(limit)
            .ToList();

        return Task.FromResult(nearby);
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        // Haversine formula
        var R = 6371; // Earth's radius in km
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
}
