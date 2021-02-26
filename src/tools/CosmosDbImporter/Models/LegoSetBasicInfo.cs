using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace BrickMoney
{
    public class LegoSetBasicInfo : LegoSetBasicInfoBase
    {
        [JsonProperty("id")] public int LegoSetId { get; set; }
    }
}
