input_data = """
1. Western Province
Colombo District (13 Divisions): Colombo, Dehiwala, Homagama, Kaduwela, Kesbewa, Kolonnawa, Kotte, Maharagama, Moratuwa, Padukka, Ratmalana, Seethawaka, Thimbirigasyaya.
Gampaha District (13 Divisions): Attanagalla, Biyagama, Divulapitiya, Dompe, Gampaha, Ja-Ela, Katana, Kelaniya, Mahara, Minuwangoda, Mirigama, Negombo, Wattala.
Kalutara District (14 Divisions): Agalawatta, Bandaragama, Beruwala, Bulathsinhala, Dodangoda, Horana, Ingiriya, Kalutara, Madurawela, Mathugama, Millaniya, Palindanuwara, Panadura, Walallavita.
2. Central Province
Kandy District (20 Divisions): Akurana, Delthota, Doluwa, Ganga Ihala Korale, Harispattuwa, Hatharaliyadda, Kandy, Kundasale, Medadumbara, Minipe, Panvila, Pasbage Korale, Pathadumbara, Pathahewaheta, Poojapitiya, Thumpane, Udadumbara, Udapalatha, Udunuwara, Yatinuwara.
Matale District (11 Divisions): Ambanganga Korale, Dambulla, Galewela, Laggala-Pallegama, Matale, Naula, Pallepola, Rattota, Ukuwela, Wilgamuwa, Yatawatta.
Nuwara Eliya District (5 Divisions): Ambagamuwa, Hanguranketha, Kothmale, Nuwara Eliya, Walapane.
3. Southern Province
Galle District (19 Divisions): Akmeemana, Ambalangoda, Baddegama, Balapitiya, Benthota, Bope-Poddala, Elpitiya, Galle, Gonapinuwala, Habaraduwa, Hikkaduwa, Imaduwa, Karandeniya, Karapitiya, Neluwa, Niyagama, Thawalama, Welivitiya-Divithura, Yakkalamulla.
Matara District (16 Divisions): Akuressa, Athuraliya, Devinuwara, Dickwella, Hakmana, Kamburupitiya, Kirinda Puhulwella, Kotapola, Malimbada, Matara, Mulatiyana, Pasgoda, Pitabeddara, Thihagoda, Weligama, Welipitiya.
Hambantota District (12 Divisions): Ambalantota, Angunakolapelessa, Beliatta, Hambantota, Katuwana, Lunugamvehera, Okewela, Sooriyawewa, Tangalle, Thissamaharama, Walasmulla, Weeraketiya.
4. Northern Province
Jaffna District (15 Divisions): Delft, Island North (Kayts), Island South (Velanai), Jaffna, Karainagar, Nallur, Thenmaradchi (Chavakachcheri), Vadamaradchi East (Maruthankerney), Vadamaradchi North (Point Pedro), Vadamaradchi South-West (Karaveddy), Valikamam East (Kopay), Valikamam North (Tellippalai), Valikamam South (Uduvil), Valikamam South-West (Sandilipay), Valikamam West (Chankanai).
Kilinochchi District (4 Divisions): Kandavalai, Karachchi, Pachchilaipalli, Poonakary.
Mannar District (5 Divisions): Madhu, Mannar, Manthai West, Musalai, Nanaddan.
Vavuniya District (4 Divisions): Vavuniya, Vavuniya North, Vavuniya South, Vengalacheddikulam.
Mullaitivu District (6 Divisions): Manthai East, Maritimepattu, Oddusuddan, Puthukudiyiruppu, Thunukkai, Welioya.
5. Eastern Province
Trincomalee District (11 Divisions): Gomarankadawala, Kantale, Kinniya, Kuchchaveli, Morawewa, Muttur, Padavi Sri Pura, Seruvila, Thampalakamam, Trincomalee Town and Gravets, Verugal.
Batticaloa District (14 Divisions): Eravur Pattu, Eravur Town, Kattankudy, Koralai Pattu (Valaichchenai), Koralai Pattu Central, Koralai Pattu North (Vaharai), Koralai Pattu South (Kiran), Koralai Pattu West (Oddamavadi), Manmunai North, Manmunai Pattu (Araipattai), Manmunai South & Eruvil Pattu, Manmunai South West, Manmunai West, Porativu Pattu.
Ampara District (20 Divisions): Addalachchenai, Akkaraipattu, Alayadiwembu, Ampara, Damana, Dehiattakandiya, Eragama, Kalmunai Muslim, Kalmunai Tamil, Karativu, Lahugala, Mahaoya, Navithanveli, Ninthavur, Padiyathalawa, Pottuvil, Sainthamaruthu, Sammanthurai, Thirukkovil, Uhana.
6. North Western Province
Kurunegala District (30 Divisions): Alawwa, Ambanpola, Bamunakotuwa, Bingiriya, Ehetuwewa, Galgamuwa, Ganewatta, Giribawa, Ibbagamuwa, Kobeigane, Kotavehera, Kuliyapitiya East, Kuliyapitiya West, Kurunegala, Maho, Mallawapitiya, Maspotha, Mawathagama, Narammala, Nikaweratiya, Panduwasnuwara, Panduwasnuwara East, Pannala, Polgahawela, Polpithigama, Rasnayakapura, Rideegama, Udubaddawa, Wariyapola, Weerambugedara.
Puttalam District (16 Divisions): Anamaduwa, Arachchikattuwa, Chilaw, Dankotuwa, Kalpitiya, Karuwalagaswewa, Madampe, Mahakumbukkadawala, Mahawewa, Mundel, Nattandiya, Nawagattegama, Pallama, Puttalam, Vanathavilluwa, Wennappuwa.
7. North Central Province
Anuradhapura District (22 Divisions): Galenbindunuwewa, Galnewa, Horowpothana, Ipalogama, Kahatagasdigiliya, Kebithigollewa, Kekirawa, Mahavilachchiya, Medawachchiya, Mihinthale, Nachchadoowa, Nochchiyagama, Nuwaragam Palatha Central, Nuwaragam Palatha East, Padaviya, Palagala, Palugaswewa, Rajanganaya, Rambewa, Thalawa, Thambuttegama, Thirappane.
Polonnaruwa District (7 Divisions): Dimbulagala, Elahera, Hingurakgoda, Lankapura, Medirigiriya, Thamankaduwa, Welikanda.
8. Uva Province
Badulla District (15 Divisions): Badulla, Bandarawela, Ella, Haldummulla, Hali-Ela, Haputale, Kandaketiya, Lunugala, Mahiyanganaya, Meegahakivula, Passara, Rideemaliyadda, Soranathota, Uva-Paranagama, Welimada.
Monaragala District (11 Divisions): Badalkumbura, Bibile, Buttala, Katharagama, Madulla, Medagama, Moneragala, Sevanagala, Siyambalanduwa, Thanamalvila, Wellawaya.
9. Sabaragamuwa Province
Ratnapura District (17 Divisions): Ayagama, Balangoda, Eheliyagoda, Elapatha, Embilipitiya, Godakawela, Imbulpe, Kahawatta, Kalawana, Kolonna, Kuruwita, Nivithigala, Opanayaka, Pelmadulla, Ratnapura, Weligepola.
Kegalle District (11 Divisions): Aranayaka, Bulathkohupitiya, Dehiowita, Deraniyagala, Galigamuwa, Kegalle, Mawanella, Rambukkana, Ruwanwella, Warakapola, Yatiyanthota.
"""

district_map = {
    "Colombo": 1, "Gampaha": 2, "Kalutara": 3,
    "Kandy": 4, "Matale": 5, "Nuwara Eliya": 6,
    "Galle": 7, "Matara": 8, "Hambantota": 9,
    "Jaffna": 10, "Kilinochchi": 11, "Mannar": 12, "Vavuniya": 13, "Mullaitivu": 14,
    "Trincomalee": 17, "Batticaloa": 15, "Ampara": 16,
    "Kurunegala": 18, "Puttalam": 19,
    "Anuradhapura": 20, "Polonnaruwa": 21,
    "Badulla": 22, "Monaragala": 23,
    "Ratnapura": 24, "Kegalle": 25
}

csharp_code = "        _divisions = new List<Division>\n        {\n"
div_id = 1

for line in input_data.strip().split('\n'):
    line = line.strip()
    if not line or line[0].isdigit():
        continue
    
    if " District (" not in line:
        continue
        
    parts = line.split(" District (")
    district_name = parts[0].strip()
    
    if district_name not in district_map:
        print(f"Missing district mapping: {district_name}")
        continue
        
    d_id = district_map[district_name]
    
    div_parts = parts[1].split("): ")
    if len(div_parts) != 2:
        continue
        
    divisions_list = div_parts[1].rstrip('.').split(", ")
    csharp_code += f"            // {district_name} District\n"
    for div in divisions_list:
        clean_name = div.strip()
        csharp_code += f'            new() {{ Id = {div_id}, DistrictId = {d_id}, Name = "{clean_name}", Coords = new(0, 0) }},\n'
        div_id += 1

csharp_code += "        };\n"

with open("divisions_init.txt", "w", encoding="utf-8") as f:
    f.write(csharp_code)

print("Generated divisions_init.txt successfully using Python.")
