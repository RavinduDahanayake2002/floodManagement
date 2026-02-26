using FloodApp.Models;

namespace FloodApp.Services;

public class LocationService
{
    private readonly List<Province> _provinces;
    private readonly List<District> _districts;
    private readonly List<Division> _divisions;
    private readonly List<Division> _divisions;
    private readonly List<Town> _towns;

    public LocationService()
    {
        // Data Seed
        // Provinces
        _provinces = new List<Province>
        {
            new() { Id = 1, Name = "Western", Coords = new(6.9016, 80.0088) },
            new() { Id = 2, Name = "Central", Coords = new(7.2906, 80.6337) },
            new() { Id = 3, Name = "Southern", Coords = new(6.0535, 80.2210) },
            new() { Id = 4, Name = "Northern", Coords = new(9.6615, 80.0255) },
            new() { Id = 5, Name = "Eastern", Coords = new(7.7102, 81.6924) },
            new() { Id = 6, Name = "North Western", Coords = new(7.7601, 80.1206) },
            new() { Id = 7, Name = "North Central", Coords = new(8.3350, 80.4108) },
            new() { Id = 8, Name = "Uva", Coords = new(6.8649, 81.1802) },
            new() { Id = 9, Name = "Sabaragamuwa", Coords = new(6.7041, 80.3756) }
        };

        // Districts (Mapped 25 districts)
        _districts = new List<District>
        {
            // Western
            new() { Id = 1, ProvinceId = 1, Name = "Colombo", Coords = new(6.9271, 79.8612) },
            new() { Id = 2, ProvinceId = 1, Name = "Gampaha", Coords = new(7.0840, 79.9939) },
            new() { Id = 3, ProvinceId = 1, Name = "Kalutara", Coords = new(6.5854, 79.9607) },
            // Central
            new() { Id = 4, ProvinceId = 2, Name = "Kandy", Coords = new(7.2906, 80.6337) },
            new() { Id = 5, ProvinceId = 2, Name = "Matale", Coords = new(7.4667, 80.6167) },
            new() { Id = 6, ProvinceId = 2, Name = "Nuwara Eliya", Coords = new(6.9497, 80.7891) },
            // Southern
            new() { Id = 7, ProvinceId = 3, Name = "Galle", Coords = new(6.0321, 80.2170) },
            new() { Id = 8, ProvinceId = 3, Name = "Matara", Coords = new(5.9500, 80.5333) },
            new() { Id = 9, ProvinceId = 3, Name = "Hambantota", Coords = new(6.1245, 81.1010) },
            // Northern
            new() { Id = 10, ProvinceId = 4, Name = "Jaffna", Coords = new(9.6615, 80.0255) },
            new() { Id = 11, ProvinceId = 4, Name = "Kilinochchi", Coords = new(9.3802, 80.3770) },
            new() { Id = 12, ProvinceId = 4, Name = "Mannar", Coords = new(8.9766, 79.9043) },
            new() { Id = 13, ProvinceId = 4, Name = "Vavuniya", Coords = new(8.7514, 80.4971) },
            new() { Id = 14, ProvinceId = 4, Name = "Mullaitivu", Coords = new(9.2671, 80.8142) },
            // Eastern
            new() { Id = 15, ProvinceId = 5, Name = "Batticaloa", Coords = new(7.7102, 81.6924) },
            new() { Id = 16, ProvinceId = 5, Name = "Ampara", Coords = new(7.2833, 81.6667) },
            new() { Id = 17, ProvinceId = 5, Name = "Trincomalee", Coords = new(8.5667, 81.2333) },
            // North Western
            new() { Id = 18, ProvinceId = 6, Name = "Kurunegala", Coords = new(7.4833, 80.3667) },
            new() { Id = 19, ProvinceId = 6, Name = "Puttalam", Coords = new(8.0362, 79.8283) },
            // North Central
            new() { Id = 20, ProvinceId = 7, Name = "Anuradhapura", Coords = new(8.3114, 80.4037) },
            new() { Id = 21, ProvinceId = 7, Name = "Polonnaruwa", Coords = new(7.9403, 81.0188) },
            // Uva
            new() { Id = 22, ProvinceId = 8, Name = "Badulla", Coords = new(6.9934, 81.0550) },
            new() { Id = 23, ProvinceId = 8, Name = "Monaragala", Coords = new(6.8714, 81.3487) },
            // Sabaragamuwa
            new() { Id = 24, ProvinceId = 9, Name = "Ratnapura", Coords = new(6.7056, 80.3847) },
            new() { Id = 25, ProvinceId = 9, Name = "Kegalle", Coords = new(7.2513, 80.3464) }
        };


        _divisions = new List<Division>
        {
            // Colombo District
            new() { Id = 1, DistrictId = 1, Name = "Colombo", Coords = new(6.9355, 79.8487) },
            new() { Id = 2, DistrictId = 1, Name = "Dehiwala", Coords = new(6.8563, 79.8616) },
            new() { Id = 3, DistrictId = 1, Name = "Homagama", Coords = new(6.844, 80.0024) },
            new() { Id = 4, DistrictId = 1, Name = "Kaduwela", Coords = new(7.5, 80.8333) },
            new() { Id = 5, DistrictId = 1, Name = "Kesbewa", Coords = new(6.7914, 79.9378) },
            new() { Id = 6, DistrictId = 1, Name = "Kolonnawa", Coords = new(6.9329, 79.8848) },
            new() { Id = 7, DistrictId = 1, Name = "Kotte", Coords = new(6.9271, 79.8612) },
            new() { Id = 8, DistrictId = 1, Name = "Maharagama", Coords = new(6.848, 79.9265) },
            new() { Id = 9, DistrictId = 1, Name = "Moratuwa", Coords = new(6.773, 79.8816) },
            new() { Id = 10, DistrictId = 1, Name = "Padukka", Coords = new(6.8408, 80.0897) },
            new() { Id = 11, DistrictId = 1, Name = "Ratmalana", Coords = new(6.8175, 79.8672) },
            new() { Id = 12, DistrictId = 1, Name = "Seethawaka", Coords = new(6.9271, 79.8612) },
            new() { Id = 13, DistrictId = 1, Name = "Thimbirigasyaya", Coords = new(6.9271, 79.8612) },
            // Gampaha District
            new() { Id = 14, DistrictId = 2, Name = "Attanagalla", Coords = new(7.0323, 81.1977) },
            new() { Id = 15, DistrictId = 2, Name = "Biyagama", Coords = new(6.9423, 79.9842) },
            new() { Id = 16, DistrictId = 2, Name = "Divulapitiya", Coords = new(7.224, 80.0094) },
            new() { Id = 17, DistrictId = 2, Name = "Dompe", Coords = new(6.9503, 80.051) },
            new() { Id = 18, DistrictId = 2, Name = "Gampaha", Coords = new(7.0897, 79.9925) },
            new() { Id = 19, DistrictId = 2, Name = "Ja-Ela", Coords = new(7.0873, 79.9985) },
            new() { Id = 20, DistrictId = 2, Name = "Katana", Coords = new(7.0873, 79.9985) },
            new() { Id = 21, DistrictId = 2, Name = "Kelaniya", Coords = new(6.9553, 79.922) },
            new() { Id = 22, DistrictId = 2, Name = "Mahara", Coords = new(7.1602, 80.5724) },
            new() { Id = 23, DistrictId = 2, Name = "Minuwangoda", Coords = new(7.1663, 79.9533) },
            new() { Id = 24, DistrictId = 2, Name = "Mirigama", Coords = new(7.2439, 80.1212) },
            new() { Id = 25, DistrictId = 2, Name = "Negombo", Coords = new(7.2083, 79.8358) },
            new() { Id = 26, DistrictId = 2, Name = "Wattala", Coords = new(6.9892, 79.8917) },
            // Kalutara District
            new() { Id = 27, DistrictId = 3, Name = "Agalawatta", Coords = new(7.4798, 80.6059) },
            new() { Id = 28, DistrictId = 3, Name = "Bandaragama", Coords = new(6.7144, 79.988) },
            new() { Id = 29, DistrictId = 3, Name = "Beruwala", Coords = new(6.4788, 79.9828) },
            new() { Id = 30, DistrictId = 3, Name = "Bulathsinhala", Coords = new(6.5854, 79.9607) },
            new() { Id = 31, DistrictId = 3, Name = "Dodangoda", Coords = new(6.5399, 80.0446) },
            new() { Id = 32, DistrictId = 3, Name = "Horana", Coords = new(6.7159, 80.0626) },
            new() { Id = 33, DistrictId = 3, Name = "Ingiriya", Coords = new(6.7438, 80.176) },
            new() { Id = 34, DistrictId = 3, Name = "Kalutara", Coords = new(6.5831, 79.9593) },
            new() { Id = 35, DistrictId = 3, Name = "Madurawela", Coords = new(6.5854, 79.9607) },
            new() { Id = 36, DistrictId = 3, Name = "Mathugama", Coords = new(6.5854, 79.9607) },
            new() { Id = 37, DistrictId = 3, Name = "Millaniya", Coords = new(6.5854, 79.9607) },
            new() { Id = 38, DistrictId = 3, Name = "Palindanuwara", Coords = new(6.5854, 79.9607) },
            new() { Id = 39, DistrictId = 3, Name = "Panadura", Coords = new(6.7132, 79.9026) },
            new() { Id = 40, DistrictId = 3, Name = "Walallavita", Coords = new(6.5854, 79.9607) },
            // Kandy District
            new() { Id = 41, DistrictId = 4, Name = "Akurana", Coords = new(7.6432, 80.021) },
            new() { Id = 42, DistrictId = 4, Name = "Delthota", Coords = new(7.2906, 80.6337) },
            new() { Id = 43, DistrictId = 4, Name = "Doluwa", Coords = new(7.6179, 80.4226) },
            new() { Id = 44, DistrictId = 4, Name = "Ganga Ihala Korale", Coords = new(7.2906, 80.6337) },
            new() { Id = 45, DistrictId = 4, Name = "Harispattuwa", Coords = new(7.2906, 80.6337) },
            new() { Id = 46, DistrictId = 4, Name = "Hatharaliyadda", Coords = new(7.2906, 80.6337) },
            new() { Id = 47, DistrictId = 4, Name = "Kandy", Coords = new(7.2906, 80.6336) },
            new() { Id = 48, DistrictId = 4, Name = "Kundasale", Coords = new(7.2737, 80.7001) },
            new() { Id = 49, DistrictId = 4, Name = "Medadumbara", Coords = new(7.2906, 80.6337) },
            new() { Id = 50, DistrictId = 4, Name = "Minipe", Coords = new(7.2182, 80.9775) },
            new() { Id = 51, DistrictId = 4, Name = "Panvila", Coords = new(6.1311, 80.1267) },
            new() { Id = 52, DistrictId = 4, Name = "Pasbage Korale", Coords = new(7.2906, 80.6337) },
            new() { Id = 53, DistrictId = 4, Name = "Pathadumbara", Coords = new(7.2906, 80.6337) },
            new() { Id = 54, DistrictId = 4, Name = "Pathahewaheta", Coords = new(7.2906, 80.6337) },
            new() { Id = 55, DistrictId = 4, Name = "Poojapitiya", Coords = new(7.2906, 80.6337) },
            new() { Id = 56, DistrictId = 4, Name = "Thumpane", Coords = new(7.2906, 80.6337) },
            new() { Id = 57, DistrictId = 4, Name = "Udadumbara", Coords = new(7.2906, 80.6337) },
            new() { Id = 58, DistrictId = 4, Name = "Udapalatha", Coords = new(7.2906, 80.6337) },
            new() { Id = 59, DistrictId = 4, Name = "Udunuwara", Coords = new(7.2906, 80.6337) },
            new() { Id = 60, DistrictId = 4, Name = "Yatinuwara", Coords = new(7.2906, 80.6337) },
            // Matale District
            new() { Id = 61, DistrictId = 5, Name = "Ambanganga Korale", Coords = new(7.4675, 80.6234) },
            new() { Id = 62, DistrictId = 5, Name = "Dambulla", Coords = new(7.86, 80.6517) },
            new() { Id = 63, DistrictId = 5, Name = "Galewela", Coords = new(7.7589, 80.5683) },
            new() { Id = 64, DistrictId = 5, Name = "Laggala-Pallegama", Coords = new(7.4675, 80.6234) },
            new() { Id = 65, DistrictId = 5, Name = "Matale", Coords = new(7.4698, 80.6217) },
            new() { Id = 66, DistrictId = 5, Name = "Naula", Coords = new(7.7047, 80.6542) },
            new() { Id = 67, DistrictId = 5, Name = "Pallepola", Coords = new(7.6246, 80.6006) },
            new() { Id = 68, DistrictId = 5, Name = "Rattota", Coords = new(7.5205, 80.6798) },
            new() { Id = 69, DistrictId = 5, Name = "Ukuwela", Coords = new(7.4228, 80.6167) },
            new() { Id = 70, DistrictId = 5, Name = "Wilgamuwa", Coords = new(7.3739, 80.1905) },
            new() { Id = 71, DistrictId = 5, Name = "Yatawatta", Coords = new(7.5649, 80.5821) },
            // Nuwara Eliya District
            new() { Id = 72, DistrictId = 6, Name = "Ambagamuwa", Coords = new(7.3113, 80.5691) },
            new() { Id = 73, DistrictId = 6, Name = "Hanguranketha", Coords = new(6.9497, 80.7829) },
            new() { Id = 74, DistrictId = 6, Name = "Kothmale", Coords = new(6.9497, 80.7829) },
            new() { Id = 75, DistrictId = 6, Name = "Nuwara Eliya", Coords = new(6.9708, 80.7829) },
            new() { Id = 76, DistrictId = 6, Name = "Walapane", Coords = new(7.1226, 80.8473) },
            // Galle District
            new() { Id = 77, DistrictId = 7, Name = "Akmeemana", Coords = new(6.0535, 80.221) },
            new() { Id = 78, DistrictId = 7, Name = "Ambalangoda", Coords = new(6.2355, 80.0538) },
            new() { Id = 79, DistrictId = 7, Name = "Baddegama", Coords = new(6.1652, 80.1782) },
            new() { Id = 80, DistrictId = 7, Name = "Balapitiya", Coords = new(6.278, 80.0366) },
            new() { Id = 81, DistrictId = 7, Name = "Benthota", Coords = new(6.0535, 80.221) },
            new() { Id = 82, DistrictId = 7, Name = "Bope-Poddala", Coords = new(6.0535, 80.221) },
            new() { Id = 83, DistrictId = 7, Name = "Elpitiya", Coords = new(7.1864, 80.5954) },
            new() { Id = 84, DistrictId = 7, Name = "Galle", Coords = new(6.0461, 80.2103) },
            new() { Id = 85, DistrictId = 7, Name = "Gonapinuwala", Coords = new(6.1458, 80.1384) },
            new() { Id = 86, DistrictId = 7, Name = "Habaraduwa", Coords = new(5.9977, 80.2958) },
            new() { Id = 87, DistrictId = 7, Name = "Hikkaduwa", Coords = new(6.1407, 80.1012) },
            new() { Id = 88, DistrictId = 7, Name = "Imaduwa", Coords = new(7.5167, 80.8167) },
            new() { Id = 89, DistrictId = 7, Name = "Karandeniya", Coords = new(6.2704, 80.0794) },
            new() { Id = 90, DistrictId = 7, Name = "Karapitiya", Coords = new(6.0535, 80.221) },
            new() { Id = 91, DistrictId = 7, Name = "Neluwa", Coords = new(6.9, 81.0) },
            new() { Id = 92, DistrictId = 7, Name = "Niyagama", Coords = new(6.2342, 80.2674) },
            new() { Id = 93, DistrictId = 7, Name = "Thawalama", Coords = new(6.0535, 80.221) },
            new() { Id = 94, DistrictId = 7, Name = "Welivitiya-Divithura", Coords = new(6.0535, 80.221) },
            new() { Id = 95, DistrictId = 7, Name = "Yakkalamulla", Coords = new(6.1098, 80.3454) },
            // Matara District
            new() { Id = 96, DistrictId = 8, Name = "Akuressa", Coords = new(6.1007, 80.4693) },
            new() { Id = 97, DistrictId = 8, Name = "Athuraliya", Coords = new(5.9549, 80.555) },
            new() { Id = 98, DistrictId = 8, Name = "Devinuwara", Coords = new(5.9283, 80.5888) },
            new() { Id = 99, DistrictId = 8, Name = "Dickwella", Coords = new(5.9549, 80.555) },
            new() { Id = 100, DistrictId = 8, Name = "Hakmana", Coords = new(7.3167, 80.8) },
            new() { Id = 101, DistrictId = 8, Name = "Kamburupitiya", Coords = new(6.0833, 80.5667) },
            new() { Id = 102, DistrictId = 8, Name = "Kirinda Puhulwella", Coords = new(5.9549, 80.555) },
            new() { Id = 103, DistrictId = 8, Name = "Kotapola", Coords = new(6.2836, 80.5431) },
            new() { Id = 104, DistrictId = 8, Name = "Malimbada", Coords = new(5.9549, 80.555) },
            new() { Id = 105, DistrictId = 8, Name = "Matara", Coords = new(5.9485, 80.5353) },
            new() { Id = 106, DistrictId = 8, Name = "Mulatiyana", Coords = new(6.9608, 80.1067) },
            new() { Id = 107, DistrictId = 8, Name = "Pasgoda", Coords = new(6.2236, 80.6199) },
            new() { Id = 108, DistrictId = 8, Name = "Pitabeddara", Coords = new(6.2014, 80.4593) },
            new() { Id = 109, DistrictId = 8, Name = "Thihagoda", Coords = new(6.0167, 80.5667) },
            new() { Id = 110, DistrictId = 8, Name = "Weligama", Coords = new(5.975, 80.4297) },
            new() { Id = 111, DistrictId = 8, Name = "Welipitiya", Coords = new(6.451, 80.0078) },
            // Hambantota District
            new() { Id = 112, DistrictId = 9, Name = "Ambalantota", Coords = new(6.1196, 81.0214) },
            new() { Id = 113, DistrictId = 9, Name = "Angunakolapelessa", Coords = new(6.4472, 81.024) },
            new() { Id = 114, DistrictId = 9, Name = "Beliatta", Coords = new(6.0496, 80.7325) },
            new() { Id = 115, DistrictId = 9, Name = "Hambantota", Coords = new(6.1241, 81.1185) },
            new() { Id = 116, DistrictId = 9, Name = "Katuwana", Coords = new(6.2683, 80.6971) },
            new() { Id = 117, DistrictId = 9, Name = "Lunugamvehera", Coords = new(6.1246, 81.1185) },
            new() { Id = 118, DistrictId = 9, Name = "Okewela", Coords = new(6.1246, 81.1185) },
            new() { Id = 119, DistrictId = 9, Name = "Sooriyawewa", Coords = new(6.1246, 81.1185) },
            new() { Id = 120, DistrictId = 9, Name = "Tangalle", Coords = new(6.0234, 80.7974) },
            new() { Id = 121, DistrictId = 9, Name = "Thissamaharama", Coords = new(6.1246, 81.1185) },
            new() { Id = 122, DistrictId = 9, Name = "Walasmulla", Coords = new(6.1521, 80.6941) },
            new() { Id = 123, DistrictId = 9, Name = "Weeraketiya", Coords = new(6.151, 80.7643) },
            // Jaffna District
            new() { Id = 124, DistrictId = 10, Name = "Delft", Coords = new(9.5333, 79.6667) },
            new() { Id = 125, DistrictId = 10, Name = "Island North (Kayts)", Coords = new(9.6615, 80.0255) },
            new() { Id = 126, DistrictId = 10, Name = "Island South (Velanai)", Coords = new(9.6615, 80.0255) },
            new() { Id = 127, DistrictId = 10, Name = "Jaffna", Coords = new(9.6684, 80.0074) },
            new() { Id = 128, DistrictId = 10, Name = "Karainagar", Coords = new(9.743, 79.8728) },
            new() { Id = 129, DistrictId = 10, Name = "Nallur", Coords = new(9.6667, 80.0333) },
            new() { Id = 130, DistrictId = 10, Name = "Thenmaradchi (Chavakachcheri)", Coords = new(9.6615, 80.0255) },
            new() { Id = 131, DistrictId = 10, Name = "Vadamaradchi East (Maruthankerney)", Coords = new(9.6615, 80.0255) },
            new() { Id = 132, DistrictId = 10, Name = "Vadamaradchi North (Point Pedro)", Coords = new(9.6615, 80.0255) },
            new() { Id = 133, DistrictId = 10, Name = "Vadamaradchi South-West (Karaveddy)", Coords = new(9.6615, 80.0255) },
            new() { Id = 134, DistrictId = 10, Name = "Valikamam East (Kopay)", Coords = new(9.6615, 80.0255) },
            new() { Id = 135, DistrictId = 10, Name = "Valikamam North (Tellippalai)", Coords = new(9.6615, 80.0255) },
            new() { Id = 136, DistrictId = 10, Name = "Valikamam South (Uduvil)", Coords = new(9.6615, 80.0255) },
            new() { Id = 137, DistrictId = 10, Name = "Valikamam South-West (Sandilipay)", Coords = new(9.6615, 80.0255) },
            new() { Id = 138, DistrictId = 10, Name = "Valikamam West (Chankanai)", Coords = new(9.6615, 80.0255) },
            // Kilinochchi District
            new() { Id = 139, DistrictId = 11, Name = "Kandavalai", Coords = new(9.4678, 80.4896) },
            new() { Id = 140, DistrictId = 11, Name = "Karachchi", Coords = new(9.3803, 80.377) },
            new() { Id = 141, DistrictId = 11, Name = "Pachchilaipalli", Coords = new(9.3803, 80.377) },
            new() { Id = 142, DistrictId = 11, Name = "Poonakary", Coords = new(9.3803, 80.377) },
            // Mannar District
            new() { Id = 143, DistrictId = 12, Name = "Madhu", Coords = new(8.85, 80.2) },
            new() { Id = 144, DistrictId = 12, Name = "Mannar", Coords = new(8.9894, 79.8784) },
            new() { Id = 145, DistrictId = 12, Name = "Manthai West", Coords = new(8.981, 79.9044) },
            new() { Id = 146, DistrictId = 12, Name = "Musalai", Coords = new(8.981, 79.9044) },
            new() { Id = 147, DistrictId = 12, Name = "Nanaddan", Coords = new(8.8363, 79.9717) },
            // Vavuniya District
            new() { Id = 148, DistrictId = 13, Name = "Vavuniya", Coords = new(8.7514, 80.4971) },
            new() { Id = 149, DistrictId = 13, Name = "Vavuniya North", Coords = new(8.7514, 80.4971) },
            new() { Id = 150, DistrictId = 13, Name = "Vavuniya South", Coords = new(8.7514, 80.4971) },
            new() { Id = 151, DistrictId = 13, Name = "Vengalacheddikulam", Coords = new(8.7514, 80.4971) },
            // Mullaitivu District
            new() { Id = 152, DistrictId = 14, Name = "Manthai East", Coords = new(9.2671, 80.8142) },
            new() { Id = 153, DistrictId = 14, Name = "Maritimepattu", Coords = new(9.2671, 80.8142) },
            new() { Id = 154, DistrictId = 14, Name = "Oddusuddan", Coords = new(9.1513, 80.6526) },
            new() { Id = 155, DistrictId = 14, Name = "Puthukudiyiruppu", Coords = new(9.2671, 80.8142) },
            new() { Id = 156, DistrictId = 14, Name = "Thunukkai", Coords = new(9.2671, 80.8142) },
            new() { Id = 157, DistrictId = 14, Name = "Welioya", Coords = new(9.2671, 80.8142) },
            // Trincomalee District
            new() { Id = 158, DistrictId = 17, Name = "Gomarankadawala", Coords = new(8.6749, 80.9548) },
            new() { Id = 159, DistrictId = 17, Name = "Kantale", Coords = new(8.35, 81.0003) },
            new() { Id = 160, DistrictId = 17, Name = "Kinniya", Coords = new(8.4985, 81.1832) },
            new() { Id = 161, DistrictId = 17, Name = "Kuchchaveli", Coords = new(8.8146, 81.099) },
            new() { Id = 162, DistrictId = 17, Name = "Morawewa", Coords = new(8.5897, 80.8352) },
            new() { Id = 163, DistrictId = 17, Name = "Muttur", Coords = new(8.5874, 81.2152) },
            new() { Id = 164, DistrictId = 17, Name = "Padavi Sri Pura", Coords = new(8.5874, 81.2152) },
            new() { Id = 165, DistrictId = 17, Name = "Seruvila", Coords = new(8.3684, 81.3173) },
            new() { Id = 166, DistrictId = 17, Name = "Thampalakamam", Coords = new(8.5874, 81.2152) },
            new() { Id = 167, DistrictId = 17, Name = "Trincomalee Town and Gravets", Coords = new(8.5874, 81.2152) },
            new() { Id = 168, DistrictId = 17, Name = "Verugal", Coords = new(8.5874, 81.2152) },
            // Batticaloa District
            new() { Id = 169, DistrictId = 15, Name = "Eravur Pattu", Coords = new(7.7102, 81.6924) },
            new() { Id = 170, DistrictId = 15, Name = "Eravur Town", Coords = new(7.7782, 81.6038) },
            new() { Id = 171, DistrictId = 15, Name = "Kattankudy", Coords = new(7.6824, 81.7334) },
            new() { Id = 172, DistrictId = 15, Name = "Koralai Pattu (Valaichchenai)", Coords = new(7.7102, 81.6924) },
            new() { Id = 173, DistrictId = 15, Name = "Koralai Pattu Central", Coords = new(7.7102, 81.6924) },
            new() { Id = 174, DistrictId = 15, Name = "Koralai Pattu North (Vaharai)", Coords = new(7.7102, 81.6924) },
            new() { Id = 175, DistrictId = 15, Name = "Koralai Pattu South (Kiran)", Coords = new(7.7102, 81.6924) },
            new() { Id = 176, DistrictId = 15, Name = "Koralai Pattu West (Oddamavadi)", Coords = new(7.7102, 81.6924) },
            new() { Id = 177, DistrictId = 15, Name = "Manmunai North", Coords = new(7.7102, 81.6924) },
            new() { Id = 178, DistrictId = 15, Name = "Manmunai Pattu (Araipattai)", Coords = new(7.7102, 81.6924) },
            new() { Id = 179, DistrictId = 15, Name = "Manmunai South & Eruvil Pattu", Coords = new(7.7102, 81.6924) },
            new() { Id = 180, DistrictId = 15, Name = "Manmunai South West", Coords = new(7.7102, 81.6924) },
            new() { Id = 181, DistrictId = 15, Name = "Manmunai West", Coords = new(7.7102, 81.6924) },
            new() { Id = 182, DistrictId = 15, Name = "Porativu Pattu", Coords = new(7.7102, 81.6924) },
            // Ampara District
            new() { Id = 183, DistrictId = 16, Name = "Addalachchenai", Coords = new(7.2912, 81.6724) },
            new() { Id = 184, DistrictId = 16, Name = "Akkaraipattu", Coords = new(7.2165, 81.8538) },
            new() { Id = 185, DistrictId = 16, Name = "Alayadiwembu", Coords = new(7.2912, 81.6724) },
            new() { Id = 186, DistrictId = 16, Name = "Ampara", Coords = new(7.2975, 81.682) },
            new() { Id = 187, DistrictId = 16, Name = "Damana", Coords = new(7.8417, 80.5797) },
            new() { Id = 188, DistrictId = 16, Name = "Dehiattakandiya", Coords = new(7.2912, 81.6724) },
            new() { Id = 189, DistrictId = 16, Name = "Eragama", Coords = new(7.2912, 81.6724) },
            new() { Id = 190, DistrictId = 16, Name = "Kalmunai Muslim", Coords = new(7.2912, 81.6724) },
            new() { Id = 191, DistrictId = 16, Name = "Kalmunai Tamil", Coords = new(7.2912, 81.6724) },
            new() { Id = 192, DistrictId = 16, Name = "Karativu", Coords = new(7.3833, 81.8333) },
            new() { Id = 193, DistrictId = 16, Name = "Lahugala", Coords = new(7.4, 81.3) },
            new() { Id = 194, DistrictId = 16, Name = "Mahaoya", Coords = new(7.2912, 81.6724) },
            new() { Id = 195, DistrictId = 16, Name = "Navithanveli", Coords = new(7.4333, 81.7833) },
            new() { Id = 196, DistrictId = 16, Name = "Ninthavur", Coords = new(7.2912, 81.6724) },
            new() { Id = 197, DistrictId = 16, Name = "Padiyathalawa", Coords = new(7.4036, 81.2436) },
            new() { Id = 198, DistrictId = 16, Name = "Pottuvil", Coords = new(6.8762, 81.8267) },
            new() { Id = 199, DistrictId = 16, Name = "Sainthamaruthu", Coords = new(7.2912, 81.6724) },
            new() { Id = 200, DistrictId = 16, Name = "Sammanthurai", Coords = new(7.2912, 81.6724) },
            new() { Id = 201, DistrictId = 16, Name = "Thirukkovil", Coords = new(7.1203, 81.849) },
            new() { Id = 202, DistrictId = 16, Name = "Uhana", Coords = new(7.3667, 81.6333) },
            // Kurunegala District
            new() { Id = 203, DistrictId = 18, Name = "Alawwa", Coords = new(7.2971, 80.2343) },
            new() { Id = 204, DistrictId = 18, Name = "Ambanpola", Coords = new(7.9204, 80.2351) },
            new() { Id = 205, DistrictId = 18, Name = "Bamunakotuwa", Coords = new(7.8, 80.3167) },
            new() { Id = 206, DistrictId = 18, Name = "Bingiriya", Coords = new(7.6004, 79.9345) },
            new() { Id = 207, DistrictId = 18, Name = "Ehetuwewa", Coords = new(8.05, 80.3833) },
            new() { Id = 208, DistrictId = 18, Name = "Galgamuwa", Coords = new(7.9965, 80.2675) },
            new() { Id = 209, DistrictId = 18, Name = "Ganewatta", Coords = new(7.6535, 80.3597) },
            new() { Id = 210, DistrictId = 18, Name = "Giribawa", Coords = new(7.4818, 80.3609) },
            new() { Id = 211, DistrictId = 18, Name = "Ibbagamuwa", Coords = new(7.548, 80.4577) },
            new() { Id = 212, DistrictId = 18, Name = "Kobeigane", Coords = new(7.4818, 80.3609) },
            new() { Id = 213, DistrictId = 18, Name = "Kotavehera", Coords = new(7.4818, 80.3609) },
            new() { Id = 214, DistrictId = 18, Name = "Kuliyapitiya East", Coords = new(7.4818, 80.3609) },
            new() { Id = 215, DistrictId = 18, Name = "Kuliyapitiya West", Coords = new(7.4818, 80.3609) },
            new() { Id = 216, DistrictId = 18, Name = "Kurunegala", Coords = new(7.4839, 80.3683) },
            new() { Id = 217, DistrictId = 18, Name = "Maho", Coords = new(7.825, 80.275) },
            new() { Id = 218, DistrictId = 18, Name = "Mallawapitiya", Coords = new(7.4744, 80.391) },
            new() { Id = 219, DistrictId = 18, Name = "Maspotha", Coords = new(7.4818, 80.3609) },
            new() { Id = 220, DistrictId = 18, Name = "Mawathagama", Coords = new(7.9289, 80.0479) },
            new() { Id = 221, DistrictId = 18, Name = "Narammala", Coords = new(7.4337, 80.1971) },
            new() { Id = 222, DistrictId = 18, Name = "Nikaweratiya", Coords = new(7.7503, 80.1159) },
            new() { Id = 223, DistrictId = 18, Name = "Panduwasnuwara", Coords = new(7.4818, 80.3609) },
            new() { Id = 224, DistrictId = 18, Name = "Panduwasnuwara East", Coords = new(7.4818, 80.3609) },
            new() { Id = 225, DistrictId = 18, Name = "Pannala", Coords = new(6.4801, 79.9985) },
            new() { Id = 226, DistrictId = 18, Name = "Polgahawela", Coords = new(7.3381, 80.3003) },
            new() { Id = 227, DistrictId = 18, Name = "Polpithigama", Coords = new(7.8142, 80.4042) },
            new() { Id = 228, DistrictId = 18, Name = "Rasnayakapura", Coords = new(7.4818, 80.3609) },
            new() { Id = 229, DistrictId = 18, Name = "Rideegama", Coords = new(7.4818, 80.3609) },
            new() { Id = 230, DistrictId = 18, Name = "Udubaddawa", Coords = new(7.4759, 79.9655) },
            new() { Id = 231, DistrictId = 18, Name = "Wariyapola", Coords = new(7.6177, 80.245) },
            new() { Id = 232, DistrictId = 18, Name = "Weerambugedara", Coords = new(7.4818, 80.3609) },
            // Puttalam District
            new() { Id = 233, DistrictId = 19, Name = "Anamaduwa", Coords = new(7.8786, 80.008) },
            new() { Id = 234, DistrictId = 19, Name = "Arachchikattuwa", Coords = new(7.6671, 79.835) },
            new() { Id = 235, DistrictId = 19, Name = "Chilaw", Coords = new(7.5758, 79.7953) },
            new() { Id = 236, DistrictId = 19, Name = "Dankotuwa", Coords = new(7.2964, 79.8715) },
            new() { Id = 237, DistrictId = 19, Name = "Kalpitiya", Coords = new(8.2287, 79.7599) },
            new() { Id = 238, DistrictId = 19, Name = "Karuwalagaswewa", Coords = new(8.2333, 80.5167) },
            new() { Id = 239, DistrictId = 19, Name = "Madampe", Coords = new(7.4979, 79.8381) },
            new() { Id = 240, DistrictId = 19, Name = "Mahakumbukkadawala", Coords = new(7.8393, 79.8924) },
            new() { Id = 241, DistrictId = 19, Name = "Mahawewa", Coords = new(8.4333, 80.4833) },
            new() { Id = 242, DistrictId = 19, Name = "Mundel", Coords = new(7.8087, 79.8246) },
            new() { Id = 243, DistrictId = 19, Name = "Nattandiya", Coords = new(7.4123, 79.8624) },
            new() { Id = 244, DistrictId = 19, Name = "Nawagattegama", Coords = new(8.1419, 80.2648) },
            new() { Id = 245, DistrictId = 19, Name = "Pallama", Coords = new(7.6841, 79.9183) },
            new() { Id = 246, DistrictId = 19, Name = "Puttalam", Coords = new(8.0362, 79.8283) },
            new() { Id = 247, DistrictId = 19, Name = "Vanathavilluwa", Coords = new(8.0362, 79.8283) },
            new() { Id = 248, DistrictId = 19, Name = "Wennappuwa", Coords = new(7.3454, 79.8386) },
            // Anuradhapura District
            new() { Id = 249, DistrictId = 20, Name = "Galenbindunuwewa", Coords = new(8.5859, 80.5554) },
            new() { Id = 250, DistrictId = 20, Name = "Galnewa", Coords = new(8.2027, 80.3609) },
            new() { Id = 251, DistrictId = 20, Name = "Horowpothana", Coords = new(8.3114, 80.4037) },
            new() { Id = 252, DistrictId = 20, Name = "Ipalogama", Coords = new(8.1193, 80.0986) },
            new() { Id = 253, DistrictId = 20, Name = "Kahatagasdigiliya", Coords = new(8.4214, 80.6865) },
            new() { Id = 254, DistrictId = 20, Name = "Kebithigollewa", Coords = new(8.3114, 80.4037) },
            new() { Id = 255, DistrictId = 20, Name = "Kekirawa", Coords = new(8.0385, 80.5941) },
            new() { Id = 256, DistrictId = 20, Name = "Mahavilachchiya", Coords = new(8.3114, 80.4037) },
            new() { Id = 257, DistrictId = 20, Name = "Medawachchiya", Coords = new(8.5345, 80.4923) },
            new() { Id = 258, DistrictId = 20, Name = "Mihinthale", Coords = new(8.3114, 80.4037) },
            new() { Id = 259, DistrictId = 20, Name = "Nachchadoowa", Coords = new(8.3114, 80.4037) },
            new() { Id = 260, DistrictId = 20, Name = "Nochchiyagama", Coords = new(8.2631, 80.2044) },
            new() { Id = 261, DistrictId = 20, Name = "Nuwaragam Palatha Central", Coords = new(8.3114, 80.4037) },
            new() { Id = 262, DistrictId = 20, Name = "Nuwaragam Palatha East", Coords = new(8.3114, 80.4037) },
            new() { Id = 263, DistrictId = 20, Name = "Padaviya", Coords = new(8.845, 80.7625) },
            new() { Id = 264, DistrictId = 20, Name = "Palagala", Coords = new(8.3114, 80.4037) },
            new() { Id = 265, DistrictId = 20, Name = "Palugaswewa", Coords = new(8.6, 80.8348) },
            new() { Id = 266, DistrictId = 20, Name = "Rajanganaya", Coords = new(8.062, 80.251) },
            new() { Id = 267, DistrictId = 20, Name = "Rambewa", Coords = new(8.6185, 80.5104) },
            new() { Id = 268, DistrictId = 20, Name = "Thalawa", Coords = new(7.937, 80.4325) },
            new() { Id = 269, DistrictId = 20, Name = "Thambuttegama", Coords = new(8.3114, 80.4037) },
            new() { Id = 270, DistrictId = 20, Name = "Thirappane", Coords = new(8.3114, 80.4037) },
            // Polonnaruwa District
            new() { Id = 271, DistrictId = 21, Name = "Dimbulagala", Coords = new(7.8667, 81.1167) },
            new() { Id = 272, DistrictId = 21, Name = "Elahera", Coords = new(7.7279, 80.7899) },
            new() { Id = 273, DistrictId = 21, Name = "Hingurakgoda", Coords = new(8.0333, 80.95) },
            new() { Id = 274, DistrictId = 21, Name = "Lankapura", Coords = new(7.9403, 81.0188) },
            new() { Id = 275, DistrictId = 21, Name = "Medirigiriya", Coords = new(7.9403, 81.0188) },
            new() { Id = 276, DistrictId = 21, Name = "Thamankaduwa", Coords = new(7.9403, 81.0188) },
            new() { Id = 277, DistrictId = 21, Name = "Welikanda", Coords = new(6.0317, 80.3018) },
            // Badulla District
            new() { Id = 278, DistrictId = 22, Name = "Badulla", Coords = new(6.9802, 81.0577) },
            new() { Id = 279, DistrictId = 22, Name = "Bandarawela", Coords = new(6.8334, 80.9853) },
            new() { Id = 280, DistrictId = 22, Name = "Ella", Coords = new(6.8756, 81.0463) },
            new() { Id = 281, DistrictId = 22, Name = "Haldummulla", Coords = new(6.7603, 80.8844) },
            new() { Id = 282, DistrictId = 22, Name = "Hali-Ela", Coords = new(6.9536, 81.0299) },
            new() { Id = 283, DistrictId = 22, Name = "Haputale", Coords = new(6.7657, 80.951) },
            new() { Id = 284, DistrictId = 22, Name = "Kandaketiya", Coords = new(7.1695, 81.003) },
            new() { Id = 285, DistrictId = 22, Name = "Lunugala", Coords = new(7.0369, 81.2017) },
            new() { Id = 286, DistrictId = 22, Name = "Mahiyanganaya", Coords = new(6.9934, 81.055) },
            new() { Id = 287, DistrictId = 22, Name = "Meegahakivula", Coords = new(7.1363, 81.0468) },
            new() { Id = 288, DistrictId = 22, Name = "Passara", Coords = new(6.9346, 81.1574) },
            new() { Id = 289, DistrictId = 22, Name = "Rideemaliyadda", Coords = new(6.9934, 81.055) },
            new() { Id = 290, DistrictId = 22, Name = "Soranathota", Coords = new(7.0331, 81.0508) },
            new() { Id = 291, DistrictId = 22, Name = "Uva-Paranagama", Coords = new(6.9934, 81.055) },
            new() { Id = 292, DistrictId = 22, Name = "Welimada", Coords = new(6.9044, 80.9044) },
            // Monaragala District
            new() { Id = 293, DistrictId = 23, Name = "Badalkumbura", Coords = new(6.8922, 81.2396) },
            new() { Id = 294, DistrictId = 23, Name = "Bibile", Coords = new(7.1577, 81.2192) },
            new() { Id = 295, DistrictId = 23, Name = "Buttala", Coords = new(6.7608, 81.2488) },
            new() { Id = 296, DistrictId = 23, Name = "Katharagama", Coords = new(6.8728, 81.3507) },
            new() { Id = 297, DistrictId = 23, Name = "Madulla", Coords = new(7.0462, 80.9034) },
            new() { Id = 298, DistrictId = 23, Name = "Medagama", Coords = new(8.5833, 80.4) },
            new() { Id = 299, DistrictId = 23, Name = "Moneragala", Coords = new(6.8728, 81.3507) },
            new() { Id = 300, DistrictId = 23, Name = "Sevanagala", Coords = new(6.3635, 80.9197) },
            new() { Id = 301, DistrictId = 23, Name = "Siyambalanduwa", Coords = new(6.9067, 81.5461) },
            new() { Id = 302, DistrictId = 23, Name = "Thanamalvila", Coords = new(6.8728, 81.3507) },
            new() { Id = 303, DistrictId = 23, Name = "Wellawaya", Coords = new(6.7369, 81.1028) },
            // Ratnapura District
            new() { Id = 304, DistrictId = 24, Name = "Ayagama", Coords = new(7.2185, 80.4809) },
            new() { Id = 305, DistrictId = 24, Name = "Balangoda", Coords = new(6.6466, 80.7007) },
            new() { Id = 306, DistrictId = 24, Name = "Eheliyagoda", Coords = new(6.8502, 80.2624) },
            new() { Id = 307, DistrictId = 24, Name = "Elapatha", Coords = new(6.6546, 80.3712) },
            new() { Id = 308, DistrictId = 24, Name = "Embilipitiya", Coords = new(6.3439, 80.8489) },
            new() { Id = 309, DistrictId = 24, Name = "Godakawela", Coords = new(6.6828, 80.3992) },
            new() { Id = 310, DistrictId = 24, Name = "Imbulpe", Coords = new(6.6828, 80.3992) },
            new() { Id = 311, DistrictId = 24, Name = "Kahawatta", Coords = new(7.3246, 80.613) },
            new() { Id = 312, DistrictId = 24, Name = "Kalawana", Coords = new(7.7, 80.35) },
            new() { Id = 313, DistrictId = 24, Name = "Kolonna", Coords = new(6.4021, 80.6865) },
            new() { Id = 314, DistrictId = 24, Name = "Kuruwita", Coords = new(6.7765, 80.3621) },
            new() { Id = 315, DistrictId = 24, Name = "Nivithigala", Coords = new(6.6094, 80.4186) },
            new() { Id = 316, DistrictId = 24, Name = "Opanayaka", Coords = new(6.6081, 80.6181) },
            new() { Id = 317, DistrictId = 24, Name = "Pelmadulla", Coords = new(6.6222, 80.541) },
            new() { Id = 318, DistrictId = 24, Name = "Ratnapura", Coords = new(6.6858, 80.4036) },
            new() { Id = 319, DistrictId = 24, Name = "Weligepola", Coords = new(6.6828, 80.3992) },
            // Kegalle District
            new() { Id = 320, DistrictId = 25, Name = "Aranayaka", Coords = new(7.1509, 80.4614) },
            new() { Id = 321, DistrictId = 25, Name = "Bulathkohupitiya", Coords = new(7.1032, 80.3344) },
            new() { Id = 322, DistrictId = 25, Name = "Dehiowita", Coords = new(6.9776, 80.2624) },
            new() { Id = 323, DistrictId = 25, Name = "Deraniyagala", Coords = new(6.9314, 80.3352) },
            new() { Id = 324, DistrictId = 25, Name = "Galigamuwa", Coords = new(7.2398, 80.3116) },
            new() { Id = 325, DistrictId = 25, Name = "Kegalle", Coords = new(7.2523, 80.3436) },
            new() { Id = 326, DistrictId = 25, Name = "Mawanella", Coords = new(7.2519, 80.4453) },
            new() { Id = 327, DistrictId = 25, Name = "Rambukkana", Coords = new(7.6833, 80.1667) },
            new() { Id = 328, DistrictId = 25, Name = "Ruwanwella", Coords = new(7.0427, 80.2517) },
            new() { Id = 329, DistrictId = 25, Name = "Warakapola", Coords = new(7.2226, 80.1887) },
            new() { Id = 330, DistrictId = 25, Name = "Yatiyanthota", Coords = new(7.2513, 80.3464) },
        };


        // Auto-generated Divisions based on Districts
        _divisions = new List<Division>();
        foreach (var d in _districts)
        {
            _divisions.Add(new Division { Id = d.Id, DistrictId = d.Id, Name = d.Name + " Division", Coords = d.Coords });
        }

        // Sample Towns
        _towns = new List<Town>
        {
            // Western Province
            // Colombo (ID: 1)
            new() { Id = 1, DivisionId = 1, Name = "Colombo", Coords = new(6.9271, 79.8612) },
            new() { Id = 2, DivisionId = 1, Name = "Dehiwala-Mount Lavinia", Coords = new(6.8511, 79.8659) },
            new() { Id = 3, DivisionId = 1, Name = "Moratuwa", Coords = new(6.7991, 79.8767) },
            new() { Id = 4, DivisionId = 1, Name = "Sri Jayawardenepura Kotte", Coords = new(6.9108, 79.8878) },
            new() { Id = 5, DivisionId = 1, Name = "Maharagama", Coords = new(6.8494, 79.9236) },
            new() { Id = 6, DivisionId = 1, Name = "Homagama", Coords = new(6.8412, 79.9984) },
            new() { Id = 7, DivisionId = 1, Name = "Avissawella", Coords = new(6.9543, 80.2046) },
            new() { Id = 8, DivisionId = 1, Name = "Piliyandala", Coords = new(6.8018, 79.9227) },
            // Gampaha (ID: 2)
            new() { Id = 9, DivisionId = 2, Name = "Gampaha", Coords = new(7.0840, 79.9939) },
            new() { Id = 10, DivisionId = 2, Name = "Negombo", Coords = new(7.2008, 79.8737) },
            new() { Id = 11, DivisionId = 2, Name = "Wattala", Coords = new(6.9897, 79.8924) },
            new() { Id = 12, DivisionId = 2, Name = "Ja-Ela", Coords = new(7.0766, 79.8906) },
            new() { Id = 13, DivisionId = 2, Name = "Kelaniya", Coords = new(6.9554, 79.9173) },
            new() { Id = 14, DivisionId = 2, Name = "Minuwangoda", Coords = new(7.1704, 79.9482) },
            new() { Id = 15, DivisionId = 2, Name = "Katunayake", Coords = new(7.1758, 79.8727) },
            new() { Id = 16, DivisionId = 2, Name = "Divulapitiya", Coords = new(7.2403, 80.0123) },
            // Kalutara (ID: 3)
            new() { Id = 17, DivisionId = 3, Name = "Kalutara", Coords = new(6.5854, 79.9607) },
            new() { Id = 18, DivisionId = 3, Name = "Panadura", Coords = new(6.7115, 79.9074) },
            new() { Id = 19, DivisionId = 3, Name = "Beruwala", Coords = new(6.4789, 79.9828) },
            new() { Id = 20, DivisionId = 3, Name = "Horana", Coords = new(6.7154, 80.0626) },
            new() { Id = 21, DivisionId = 3, Name = "Matugama", Coords = new(6.5222, 80.1137) },
            new() { Id = 22, DivisionId = 3, Name = "Aluthgama", Coords = new(6.4357, 80.0019) },
            new() { Id = 23, DivisionId = 3, Name = "Bandaragama", Coords = new(6.7155, 79.9856) },

            // Central Province
            // Kandy (ID: 4)
            new() { Id = 24, DivisionId = 4, Name = "Kandy", Coords = new(7.2906, 80.6337) },
            new() { Id = 25, DivisionId = 4, Name = "Gampola", Coords = new(7.1643, 80.5694) },
            new() { Id = 26, DivisionId = 4, Name = "Peradeniya", Coords = new(7.2657, 80.5960) },
            new() { Id = 27, DivisionId = 4, Name = "Nawalapitiya", Coords = new(7.0543, 80.5332) },
            new() { Id = 28, DivisionId = 4, Name = "Katugastota", Coords = new(7.3192, 80.6273) },
            new() { Id = 29, DivisionId = 4, Name = "Kundasale", Coords = new(7.2863, 80.6865) },
            // Matale (ID: 5)
            new() { Id = 30, DivisionId = 5, Name = "Matale", Coords = new(7.4667, 80.6167) },
            new() { Id = 31, DivisionId = 5, Name = "Dambulla", Coords = new(7.8600, 80.6500) },
            new() { Id = 32, DivisionId = 5, Name = "Sigiriya", Coords = new(7.9570, 80.7603) },
            new() { Id = 33, DivisionId = 5, Name = "Galewela", Coords = new(7.7719, 80.5678) },
            new() { Id = 34, DivisionId = 5, Name = "Ukuwela", Coords = new(7.4243, 80.6276) },
            // Nuwara Eliya (ID: 6)
            new() { Id = 35, DivisionId = 6, Name = "Nuwara Eliya", Coords = new(6.9497, 80.7891) },
            new() { Id = 36, DivisionId = 6, Name = "Hatton", Coords = new(6.8920, 80.5947) },
            new() { Id = 37, DivisionId = 6, Name = "Talawakele", Coords = new(6.9387, 80.6622) },
            new() { Id = 38, DivisionId = 6, Name = "Ragala", Coords = new(6.9833, 80.7667) },
            new() { Id = 39, DivisionId = 6, Name = "Ginigathena", Coords = new(6.9911, 80.4907) },

            // Southern Province
            // Galle (ID: 7)
            new() { Id = 40, DivisionId = 7, Name = "Galle", Coords = new(6.0321, 80.2170) },
            new() { Id = 41, DivisionId = 7, Name = "Hikkaduwa", Coords = new(6.1362, 80.1242) },
            new() { Id = 42, DivisionId = 7, Name = "Ambalangoda", Coords = new(6.2346, 80.0543) },
            new() { Id = 43, DivisionId = 7, Name = "Elpitiya", Coords = new(6.2570, 80.1438) },
            new() { Id = 44, DivisionId = 7, Name = "Karapitiya", Coords = new(6.0594, 80.2227) },
            new() { Id = 45, DivisionId = 7, Name = "Bentota", Coords = new(6.4173, 79.9961) },
            // Matara (ID: 8)
            new() { Id = 46, DivisionId = 8, Name = "Matara", Coords = new(5.9500, 80.5333) },
            new() { Id = 47, DivisionId = 8, Name = "Weligama", Coords = new(5.9739, 80.4294) },
            new() { Id = 48, DivisionId = 8, Name = "Akuressa", Coords = new(6.1132, 80.4727) },
            new() { Id = 49, DivisionId = 8, Name = "Dikwella", Coords = new(5.9818, 80.6865) },
            new() { Id = 50, DivisionId = 8, Name = "Kamburupitiya", Coords = new(6.0847, 80.5516) },
            new() { Id = 51, DivisionId = 8, Name = "Hakmana", Coords = new(6.0792, 80.6552) },
            // Hambantota (ID: 9)
            new() { Id = 52, DivisionId = 9, Name = "Hambantota", Coords = new(6.1245, 81.1010) },
            new() { Id = 53, DivisionId = 9, Name = "Tangalle", Coords = new(6.0243, 80.7937) },
            new() { Id = 54, DivisionId = 9, Name = "Tissamaharama", Coords = new(6.2804, 81.2874) },
            new() { Id = 55, DivisionId = 9, Name = "Ambalantota", Coords = new(6.1215, 81.0182) },
            new() { Id = 56, DivisionId = 9, Name = "Beliatta", Coords = new(6.0385, 80.7437) },

            // Northern Province
            // Jaffna (ID: 10)
            new() { Id = 57, DivisionId = 10, Name = "Jaffna", Coords = new(9.6615, 80.0255) },
            new() { Id = 58, DivisionId = 10, Name = "Chavakachcheri", Coords = new(9.6558, 80.1772) },
            new() { Id = 59, DivisionId = 10, Name = "Point Pedro", Coords = new(9.8252, 80.2333) },
            new() { Id = 60, DivisionId = 10, Name = "Nallur", Coords = new(9.6706, 80.0381) },
            new() { Id = 61, DivisionId = 10, Name = "Kankesanthurai", Coords = new(9.8167, 80.0333) },
            // Kilinochchi (ID: 11)
            new() { Id = 62, DivisionId = 11, Name = "Kilinochchi", Coords = new(9.3802, 80.3770) },
            new() { Id = 63, DivisionId = 11, Name = "Paranthan", Coords = new(9.4419, 80.4042) },
            new() { Id = 64, DivisionId = 11, Name = "Poonakary", Coords = new(9.5100, 80.2100) },
            // Mannar (ID: 12)
            new() { Id = 65, DivisionId = 12, Name = "Mannar", Coords = new(8.9766, 79.9043) },
            new() { Id = 66, DivisionId = 12, Name = "Talaimannar", Coords = new(9.0967, 79.7289) },
            new() { Id = 67, DivisionId = 12, Name = "Murunkan", Coords = new(8.8300, 80.0400) },
            // Vavuniya (ID: 13)
            new() { Id = 68, DivisionId = 13, Name = "Vavuniya", Coords = new(8.7514, 80.4971) },
            new() { Id = 69, DivisionId = 13, Name = "Cheddikulam", Coords = new(8.6811, 80.3150) },
            // Mullaitivu (ID: 14)
            new() { Id = 70, DivisionId = 14, Name = "Mullaitivu", Coords = new(9.2671, 80.8142) },
            new() { Id = 71, DivisionId = 14, Name = "Puthukkudiyiruppu", Coords = new(9.3000, 80.6833) },
            new() { Id = 72, DivisionId = 14, Name = "Mankulam", Coords = new(9.1122, 80.4578) },

            // Eastern Province
            // Trincomalee (ID: 17)
            new() { Id = 73, DivisionId = 17, Name = "Trincomalee", Coords = new(8.5667, 81.2333) },
            new() { Id = 74, DivisionId = 17, Name = "Kinniya", Coords = new(8.4988, 81.1895) },
            new() { Id = 75, DivisionId = 17, Name = "Kantale", Coords = new(8.3614, 81.0069) },
            new() { Id = 76, DivisionId = 17, Name = "Mutur", Coords = new(8.4552, 81.2662) },
            // Batticaloa (ID: 15)
            new() { Id = 77, DivisionId = 15, Name = "Batticaloa", Coords = new(7.7102, 81.6924) },
            new() { Id = 78, DivisionId = 15, Name = "Kattankudy", Coords = new(7.6897, 81.7247) },
            new() { Id = 79, DivisionId = 15, Name = "Eravur", Coords = new(7.7719, 81.6069) },
            new() { Id = 80, DivisionId = 15, Name = "Valaichchenai", Coords = new(7.9303, 81.5303) },
            // Ampara (ID: 16)
            new() { Id = 81, DivisionId = 16, Name = "Ampara", Coords = new(7.2833, 81.6667) },
            new() { Id = 82, DivisionId = 16, Name = "Kalmunai", Coords = new(7.4167, 81.8167) },
            new() { Id = 83, DivisionId = 16, Name = "Sainthamaruthu", Coords = new(7.4042, 81.8317) },
            new() { Id = 84, DivisionId = 16, Name = "Akkaraipattu", Coords = new(7.2142, 81.8483) },
            new() { Id = 85, DivisionId = 16, Name = "Pottuvil", Coords = new(6.8741, 81.8336) },

            // North Western Province
            // Kurunegala (ID: 18)
            new() { Id = 86, DivisionId = 18, Name = "Kurunegala", Coords = new(7.4833, 80.3667) },
            new() { Id = 87, DivisionId = 18, Name = "Kuliyapitiya", Coords = new(7.4686, 80.0406) },
            new() { Id = 88, DivisionId = 18, Name = "Narammala", Coords = new(7.4328, 80.2178) },
            new() { Id = 89, DivisionId = 18, Name = "Wariyapola", Coords = new(7.6186, 80.2319) },
            new() { Id = 90, DivisionId = 18, Name = "Pannala", Coords = new(7.3308, 79.9869) },
            new() { Id = 91, DivisionId = 18, Name = "Polgahawela", Coords = new(7.3325, 80.2978) },
            // Puttalam (ID: 19)
            new() { Id = 92, DivisionId = 19, Name = "Puttalam", Coords = new(8.0362, 79.8283) },
            new() { Id = 93, DivisionId = 19, Name = "Chilaw", Coords = new(7.5758, 79.7952) },
            new() { Id = 94, DivisionId = 19, Name = "Wennappuwa", Coords = new(7.3917, 79.8406) },
            new() { Id = 95, DivisionId = 19, Name = "Marawila", Coords = new(7.4169, 79.8211) },
            new() { Id = 96, DivisionId = 19, Name = "Dankotuwa", Coords = new(7.2917, 79.8833) },
            new() { Id = 97, DivisionId = 19, Name = "Kalpitiya", Coords = new(8.2294, 79.7594) },

            // North Central Province
            // Anuradhapura (ID: 20)
            new() { Id = 98, DivisionId = 20, Name = "Anuradhapura", Coords = new(8.3114, 80.4037) },
            new() { Id = 99, DivisionId = 20, Name = "Kekirawa", Coords = new(8.0436, 80.5906) },
            new() { Id = 100, DivisionId = 20, Name = "Medawachchiya", Coords = new(8.5367, 80.4908) },
            new() { Id = 101, DivisionId = 20, Name = "Thambuttegama", Coords = new(8.1561, 80.3017) },
            new() { Id = 102, DivisionId = 20, Name = "Mihintale", Coords = new(8.3503, 80.5039) },
            new() { Id = 103, DivisionId = 20, Name = "Nochchiyagama", Coords = new(8.2611, 80.2167) },
            // Polonnaruwa (ID: 21)
            new() { Id = 104, DivisionId = 21, Name = "Polonnaruwa", Coords = new(7.9403, 81.0188) },
            new() { Id = 105, DivisionId = 21, Name = "Kaduruwela", Coords = new(7.9450, 81.0250) },
            new() { Id = 106, DivisionId = 21, Name = "Hingurakgoda", Coords = new(8.0500, 80.9833) },
            new() { Id = 107, DivisionId = 21, Name = "Medirigiriya", Coords = new(8.1633, 80.9822) },

            // Uva Province
            // Badulla (ID: 22)
            new() { Id = 108, DivisionId = 22, Name = "Badulla", Coords = new(6.9934, 81.0550) },
            new() { Id = 109, DivisionId = 22, Name = "Bandarawela", Coords = new(6.8319, 80.9994) },
            new() { Id = 110, DivisionId = 22, Name = "Haputale", Coords = new(6.7689, 80.9575) },
            new() { Id = 111, DivisionId = 22, Name = "Ella", Coords = new(6.8667, 81.0467) },
            new() { Id = 112, DivisionId = 22, Name = "Mahiyanganaya", Coords = new(7.3197, 81.0514) },
            new() { Id = 113, DivisionId = 22, Name = "Welimada", Coords = new(6.9056, 80.9039) },
            // Monaragala (ID: 23)
            new() { Id = 114, DivisionId = 23, Name = "Monaragala", Coords = new(6.8714, 81.3487) },
            new() { Id = 115, DivisionId = 23, Name = "Kataragama", Coords = new(6.4136, 81.3325) },
            new() { Id = 116, DivisionId = 23, Name = "Wellawaya", Coords = new(6.7375, 81.1008) },
            new() { Id = 117, DivisionId = 23, Name = "Buttala", Coords = new(6.7578, 81.2425) },
            new() { Id = 118, DivisionId = 23, Name = "Bibile", Coords = new(7.1611, 81.2269) },

            // Sabaragamuwa Province
            // Ratnapura (ID: 24)
            new() { Id = 119, DivisionId = 24, Name = "Ratnapura", Coords = new(6.7056, 80.3847) },
            new() { Id = 120, DivisionId = 24, Name = "Balangoda", Coords = new(6.6472, 80.7028) },
            new() { Id = 121, DivisionId = 24, Name = "Embilipitiya", Coords = new(6.3356, 80.8525) },
            new() { Id = 122, DivisionId = 24, Name = "Pelmadulla", Coords = new(6.6264, 80.5358) },
            new() { Id = 123, DivisionId = 24, Name = "Eheliyagoda", Coords = new(6.8483, 80.2608) },
            // Kegalle (ID: 25)
            new() { Id = 124, DivisionId = 25, Name = "Kegalle", Coords = new(7.2513, 80.3464) },
            new() { Id = 125, DivisionId = 25, Name = "Mawanella", Coords = new(7.2536, 80.4450) },
            new() { Id = 126, DivisionId = 25, Name = "Warakapola", Coords = new(7.2272, 80.1989) },
            new() { Id = 127, DivisionId = 25, Name = "Rambukkana", Coords = new(7.3275, 80.3992) },
            new() { Id = 128, DivisionId = 25, Name = "Yatiyanthota", Coords = new(7.0428, 80.2972) }
        };
    }

    public List<Province> GetProvinces() => _provinces;
    
    public List<District> GetDistricts(int provinceId) 
        => _districts.Where(d => d.ProvinceId == provinceId).ToList();
        
    public List<Division> GetDivisions(int districtId)
        => _divisions.Where(d => d.DistrictId == districtId).ToList();

    public List<Town> GetTowns(int divisionId)
        => _towns.Where(t => t.DivisionId == divisionId).ToList();

    public Town? GetTown(int townId) => _towns.FirstOrDefault(t => t.Id == townId);
}
