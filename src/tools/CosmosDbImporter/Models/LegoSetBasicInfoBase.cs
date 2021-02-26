using Newtonsoft.Json;

namespace BrickMoney
{
    public abstract class LegoSetBasicInfoBase
    {
        [JsonProperty("nameEN")] public string NameEN { get; set; }

        [JsonProperty("nameDE")] public string NameDE { get; set; }

        [JsonProperty("year")] public int Year { get; set; }

        // Recommended retail price = Unverbindliche Preisempfehlung (UVP)
        [JsonProperty("rrPrice")] public double RrPrice { get; set; } 
    }
}