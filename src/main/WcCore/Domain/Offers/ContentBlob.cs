using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcCore.Domain.Offers
{
    public class ContentBlob
    {
        [JsonProperty("unit_unlocks")]
        public List<UnitUnlock> UnitUnlocks { get; set; } = new List<UnitUnlock>();
    }
}
