using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Snowflake.Models.Attack
{
    public class Alliance
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
