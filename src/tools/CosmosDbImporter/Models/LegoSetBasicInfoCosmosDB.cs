using BrickMoney;
using Newtonsoft.Json;

namespace BrickMoney
{
    public class LegoSetBasicInfoCosmosDB : LegoSetBasicInfoBase
    {
        [JsonProperty("id")] public string LegoSetId { get; set; }

        [JsonProperty("partitionKey")] public string PartitionKey { get; set; } = "default";
    }

    public class MaxResult
    {
        [JsonProperty("max")]
        public int Max { get; set; }
    }
}