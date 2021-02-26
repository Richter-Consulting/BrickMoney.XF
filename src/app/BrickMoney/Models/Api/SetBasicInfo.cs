using Newtonsoft.Json;

namespace BrickMoney.Models.Api
{
    public class SetBasicInfo
    {
        [JsonProperty("id")]
        public string SetId { get; set; }
        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; } = "default";
        [JsonProperty("nameEN")]
        public string NameEN { get; set; }

        [JsonProperty("nameDE")]
        public string NameDE { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
        [JsonProperty("rrPrice")]
        public double RrPrice { get; set; }
    }
}
