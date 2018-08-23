using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Sheets.Models
{
    public class ContentBlob
    {
        [JsonProperty("unit_unlocks")]
        public List<UnitUnlock> UnitUnlocks { get; set; } = new List<UnitUnlock>();
    }
}
