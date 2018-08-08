using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class Unit
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("levels")]
        public IList<UnitLevel> Levels { get; set; } = new List<UnitLevel>();
    }
}
