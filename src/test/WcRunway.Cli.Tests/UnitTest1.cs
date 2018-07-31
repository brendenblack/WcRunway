using Shouldly;
using System;
using System.IO;
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
    }
}
