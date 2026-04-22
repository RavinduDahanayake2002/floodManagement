using System.ComponentModel.DataAnnotations;

namespace FloodApp.Models;

public class DamageReport
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string FullName { get; set; } = "";
    
    [Required]
    public string Address { get; set; } = "";
    
    [Required]
    public string ContactNumber { get; set; } = "";
    
    [Required]
    public string Description { get; set; } = "";
    
    public string? PhotoUrl { get; set; }
    
    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
    
    public string Status { get; set; } = "Pending";
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public double DamageRate { get; set; } // Percentage 0-100

    public string DamageLabel => DamageRate switch
    {
        > 5 => "High",
        >= 3 => "Medium",
        _ => "Normal"
    };

    public double PropertyValue { get; set; }
    public double PayoutAmount { get; set; }
}
