using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        //[JsonProperty("kxid", NullValueHandling = NullValueHandling.Ignore)]
        //public string KXID { get; set; }

        //[JsonProperty("fbid", NullValueHandling = NullValueHandling.Ignore)]
        //public long? FBID { get; set; }

        //[JsonProperty("addtime")]
        //public long AddTime { get; set; }
        
        //[JsonIgnore]
        //public DateTimeOffset SignUpDate
        //{
        //    get
        //    {
        //        return DateTimeOffset.FromUnixTimeSeconds(AddTime);
        //    }
        //}


        //[JsonProperty("email")]
        //public string EmailAddress { get; set; }
    }
}
