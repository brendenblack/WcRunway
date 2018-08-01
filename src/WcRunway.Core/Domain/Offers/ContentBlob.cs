using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Offers
{
    public class ContentBlob
    {
        [JsonProperty("unit_unlocks")]
        public List<UnitUnlock> UnitUnlocks { get; set; } = new List<UnitUnlock>();
    }
}
