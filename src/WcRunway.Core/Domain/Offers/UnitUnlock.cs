using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Offers
{
    public class UnitUnlock
    {
        /// <summary>
        /// The Unit ID being unlocked
        /// </summary>
        [JsonProperty("type")]
        public int UnitId { get; set; }

        /// <summary>
        /// The level to unlock the unit at
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }
    }
}
