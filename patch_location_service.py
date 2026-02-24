import re

file_path = "d:/My_PaidProjects/slic-flood-management/Services/LocationService.cs"

with open(file_path, "r", encoding="utf-8") as f:
    content = f.read()

# Replace DistrictId with DivisionId ONLY in the sample towns block
dist_init_end = content.find("        // Sample Towns")
part1 = content[:dist_init_end]
part2 = content[dist_init_end:]

# specifically replacing `DistrictId = `
part2 = part2.replace("DistrictId = ", "DivisionId = ")
content = part1 + part2

# Add `private readonly List<Division> _divisions;` after `List<District>`
content = re.sub(
    r"(private readonly List<District> _districts;)",
    r"\1\n    private readonly List<Division> _divisions;",
    content
)

divisions_init = """
        // Auto-generated Divisions based on Districts
        _divisions = new List<Division>();
        foreach (var d in _districts)
        {
            _divisions.Add(new Division { Id = d.Id, DistrictId = d.Id, Name = d.Name + " Division", Coords = d.Coords });
        }

"""
dist_init_end = content.find("        // Sample Towns")
content = content[:dist_init_end] + divisions_init + content[dist_init_end:]

# Replace GetTowns method and inject GetDivisions
content = re.sub(
    r"    public List<Town> GetTowns\(int districtId\).*?\.ToList\(\);",
    """    public List<Division> GetDivisions(int districtId)
        => _divisions.Where(d => d.DistrictId == districtId).ToList();

    public List<Town> GetTowns(int divisionId)
        => _towns.Where(t => t.DivisionId == divisionId).ToList();""",
    content,
    flags=re.DOTALL
)

with open(file_path, "w", encoding="utf-8") as f:
    f.write(content)

print("Updated LocationService.cs successfully.")
