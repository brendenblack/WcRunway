using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace WcDataTests
{
    public class Class1
    {
        [Fact]
        public void ParseJson()
        {
            //var input = "[\r\n  {\r\n    \"hex\": 4,\r\n    \"platoon_id\": \"p15301057102561\"\r\n  },\r\n  {\r\n    \"hex\": 1,\r\n    \"platoon_id\": \"p15311628776111\"\r\n  },\r\n  {\r\n    \"hex\": 0,\r\n    \"platoon_id\": \"p15302253846751\"\r\n  }\r\n]";

            //input = input.Replace("\n", "");
            //var input = "[\n  {\n    hex: 1,\n    platoon_id: p15301057102561\n  },\n  {\n    hex: 0,\n    platoon_id: p15311628776111\n  },\n  {\n    hex: 5,\n    platoon_id: p15302253846751\n  },\n  {\n    hex: 4,\n    platoon_id: p15327125008741\n  }\n]";
            var input = "[  {    hex: 1,    platoon_id: p15301057102561  },  {    hex: 0,    platoon_id: p15311628776111  },  {    hex: 5,    platoon_id: p15302253846751  },  {    hex: 4,    platoon_id: p15327125008741  }]";
            var typeExample = new { names = new[] { new { hex = 0, platoon_id = "" } } };
            var result = JsonConvert.DeserializeObject<List<PlatoonPosition>>(input);

            result.ShouldNotBeNull();
        }

        public class PlatoonPosition
        {
            [JsonProperty("hex")]
            public int Hex { get; set; }

            [JsonProperty("platoon_id")]
            public string PlatoonId { get; set; }
        }
    }
}
