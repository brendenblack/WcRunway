using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Snowflake.Models.Attack
{
    public class PlatoonAttackStagingLocation
    {
        [JsonProperty("hex")]
        public int Hex { get; set; }

        [JsonProperty("platoon_id")]
        public string PlatoonId { get; set; }
    }
}
