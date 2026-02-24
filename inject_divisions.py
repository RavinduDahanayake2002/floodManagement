import sys

file_path = "d:/My_PaidProjects/slic-flood-management/Services/LocationService.cs"

with open(file_path, "r", encoding="utf-8") as f:
    content = f.read()

with open("divisions_init.txt", "r", encoding="utf-8") as f:
    divisions_init_code = f.read()

# Find the old auto-generated block
start_marker = "        // Auto-generated Divisions based on Districts"
end_marker = "        // Sample Towns"

start_idx = content.find(start_marker)
end_idx = content.find(end_marker)

if start_idx != -1 and end_idx != -1:
    new_content = content[:start_idx] + divisions_init_code + "\n" + content[end_idx:]
    
    with open(file_path, "w", encoding="utf-8") as f:
        f.write(new_content)
    print("Successfully injected real Divisions into LocationService.cs!")
else:
    print("Could not find insertion markers in LocationService.cs.")
