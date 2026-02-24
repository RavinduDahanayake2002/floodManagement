using FloodApp.Models;

namespace FloodApp.Services;

public class AdminService
{
    private readonly List<DamageReport> _claims = new();
    
    // Some sample data for the heatmap & dashboard
    public AdminService()
    {
        _claims.Add(new DamageReport { 
            Id = Guid.NewGuid().ToString(), FullName = "Nimal Perera", Address = "Colombo 07", 
            ContactNumber = "0771234567", Description = "Ground floor flooded", 
            Status = "Pending", ReportedAt = DateTime.UtcNow.AddHours(-2),
            Latitude = 6.9016, Longitude = 79.8659
        });
        _claims.Add(new DamageReport { 
            Id = Guid.NewGuid().ToString(), FullName = "Sunil Silva", Address = "Gampaha Town", 
            ContactNumber = "0719876543", Description = "Roof damage from storm", 
            Status = "Approved", ReportedAt = DateTime.UtcNow.AddDays(-1),
            Latitude = 7.0840, Longitude = 79.9939
        });
        _claims.Add(new DamageReport { 
            Id = Guid.NewGuid().ToString(), FullName = "Kamal Fernando", Address = "Kalutara South", 
            ContactNumber = "0755555555", Description = "Water entered kitchen", 
            Status = "Processed", ReportedAt = DateTime.UtcNow.AddDays(-2),
            Latitude = 6.5854, Longitude = 79.9607
        });
        _claims.Add(new DamageReport { 
            Id = Guid.NewGuid().ToString(), FullName = "Saman Kumara", Address = "Kaduwela", 
            ContactNumber = "0788888888", Description = "Vehicle submerged", 
            Status = "Rejected", ReportedAt = DateTime.UtcNow.AddDays(-3),
            Latitude = 6.9427, Longitude = 79.9825
        });
        _claims.Add(new DamageReport { 
            Id = Guid.NewGuid().ToString(), FullName = "Ruwan Wijesinghe", Address = "Malabe", 
            ContactNumber = "0701112223", Description = "Boundary wall collapsed", 
            Status = "Pending", ReportedAt = DateTime.UtcNow.AddHours(-1),
            Latitude = 6.9044, Longitude = 79.9631
        });
    }

    public List<DamageReport> GetAllClaims() => _claims.OrderByDescending(c => c.ReportedAt).ToList();

    public void AddClaim(DamageReport claim)
    {
        _claims.Add(claim);
    }

    public void UpdateClaimStatus(string id, string newStatus)
    {
        var claim = _claims.FirstOrDefault(c => c.Id == id);
        if (claim != null)
        {
            claim.Status = newStatus;
        }
    }
    
    public Dictionary<string, int> GetClaimsStatusSummary()
    {
        return _claims.GroupBy(c => c.Status)
                      .ToDictionary(g => g.Key, g => g.Count());
    }
}
