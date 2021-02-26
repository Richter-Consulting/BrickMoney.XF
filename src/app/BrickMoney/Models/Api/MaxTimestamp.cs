using Newtonsoft.Json;

namespace BrickMoney.Models.Api
{
    public class MaxTimestamp
    {
        [JsonProperty("max")]
        public int Max { get; set; }
    }
}
