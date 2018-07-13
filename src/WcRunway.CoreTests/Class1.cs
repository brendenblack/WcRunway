using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WcRunway.CoreTests
{
    public class Class1
    {
        [Fact]
        public void test()
        {
            var def = new { Sku = "", Quantity = 0 };
            var json = "{\"omegatitanpartcheck\":80}";

            var d = JsonConvert.DeserializeAnonymousType(json, def);

            d.Sku.ShouldBe("omegatitanpartcheck");
            d.Quantity.ShouldBe(80);
        }

        [Fact]
        public void test2()
        {
            var json = "{\"omegatitanpartcheck\":80}";

            var d = JObject.Parse(json);
            var t = d.First.ToObject<JProperty>();
            var sku = t.Name;
            var quantity = t.Value;

            sku.ShouldBe("omegatitanpartcheck");
            quantity.ShouldBe(80);
        }

        [Fact]
        public void test3()
        {
            var input = "3,000";
            int i;

            bool parse = Int32.TryParse(input, System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out i);
            if (!parse)
            {
                i = 0;
            }

            i.ShouldBe(3000);
        }
    }
}
