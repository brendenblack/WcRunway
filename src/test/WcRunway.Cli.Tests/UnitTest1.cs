using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WcRunway.Core.Domain.Game;
using WcRunway.Core.Domain.Offers;
using Xunit;

namespace WcRunway.Cli.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test2()
        {
            var dir = "C:\\Mypath";
            var prefix = "myprefix";
            var code = "myprefixunl";

            var path = Path.Combine(dir, prefix, code + ".csv");

            path.ShouldBe("C:\\Mypath\\myprefix\\myprefixunl.csv");

        }

        [Fact]
        public void JsonParseTest()
        {
            var json = "{\"unit_unlocks\":[ {\"type\":217, \"level\":10} ]}";

            var content = JsonConvert.DeserializeObject<ContentBlob>(json);

            content.UnitUnlocks.Count.ShouldBe(1);
        }

        [Fact]
        public void JsonParseTest2()
        {
            var json = "{\"unit_unlocks\":[ {\"type\":217, \"level\":10} ]}";

            var content = JsonConvert.DeserializeObject<ContentBlob>(json);

            content.UnitUnlocks.First().Level.ShouldBe(10);
        }

        [Fact]
        public void InvalidJsonParseTest()
        {
            var json = "{\"unit_unlocks\": {\"type\":217, \"level\":10} ]}";

            Should.Throw<JsonSerializationException>(() => JsonConvert.DeserializeObject<ContentBlob>(json));
        }
    }
}
