using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Snowflake.Models
{
    public class LootChest
    {
        [JsonProperty("awardId")]
        public string AwardId { get; set; }

        [JsonProperty("itemId")]
        public int ItemId { get; set; }

        [JsonProperty("prize_type")]
        public string PrizeType { get; set; }

        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("rx_ts")]
        public long TimestampEpochSeconds { get; set; }
    }
}
