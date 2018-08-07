using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class User
    {
        [JsonProperty("UserID")]
        public int Id { get; set; }

        [JsonProperty("KXID", NullValueHandling = NullValueHandling.Ignore)]
        public int KXID { get; set; }

        [JsonProperty("FBID", NullValueHandling = NullValueHandling.Ignore)]
        public int FBID { get; set; }

        [JsonProperty("AddTime")]
        public long AddTime { get; set; }
        
        [JsonIgnore]
        public DateTimeOffset SignUpDate
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(AddTime);
            }
        }


        [JsonProperty("email")]
        public string EmailAddress { get; set; }
    }
}
