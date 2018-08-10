using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Snowflake.Models.Attack
{
    public class DeployedUnit
    {
        [JsonProperty("item_id")]
        public int UnitId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("damage_dealt")]
        public long DamageDealt { get; set; }

        [JsonProperty("damage_received")]
        public long DamageReceived { get; set; }

        [JsonProperty("avg_level")]
        public int Level { get; set; }
    }
}
