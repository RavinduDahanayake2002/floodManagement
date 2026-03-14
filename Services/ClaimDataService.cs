using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FloodApp.Services
{
    /// <summary>
    /// Model for a single SLIC insurance claim record.
    /// Populate wwwroot/data/claims.json when client data is available.
    /// </summary>
    public class ClaimRecord
    {
        public string PolicyNumber { get; set; } = "";
        public string ClaimId { get; set; } = "";
        public string ClaimType { get; set; } = ""; // "Flood" or "Landslide"
        public int ClaimYear { get; set; }
        public string ClaimMonth { get; set; } = "";
        public string District { get; set; } = "";
        public string Province { get; set; } = "";
        public long AmountLKR { get; set; }
        public string Status { get; set; } = ""; // e.g. "Settled", "Pending", "Rejected"
        public string Description { get; set; } = "";
    }

    /// <summary>
    /// Service for loading and querying SLIC claims data.
    /// Stub implementation — populate claims.json with real data to activate.
    /// </summary>
    public class ClaimDataService
    {
        private List<ClaimRecord> _claims = new();

        public ClaimDataService()
        {
            LoadClaims();
        }

        private void LoadClaims()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "claims.json");
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var parsed = JsonSerializer.Deserialize<List<ClaimRecord>>(json, opts);
                    if (parsed != null)
                        _claims = parsed;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading claims data: {ex.Message}");
            }
        }

        public List<ClaimRecord> GetAll() => _claims;

        public List<ClaimRecord> GetByType(string claimType)
            => _claims.Where(c => c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase)).ToList();

        public List<ClaimRecord> GetByDistrict(string district)
            => _claims.Where(c => c.District.Equals(district, StringComparison.OrdinalIgnoreCase)).ToList();

        public int GetTotalCount() => _claims.Count;

        public long GetTotalAmountLKR() => _claims.Sum(c => c.AmountLKR);
    }
}
