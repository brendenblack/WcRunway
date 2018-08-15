using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.Data;
using Xunit;

namespace WcGraphTests.Data.StringExtensionsTests
{
    public class ToSnakeCase_Should
    {
        [Theory]
        [InlineData("This", "this")]
        [InlineData("ThisThat", "this_that")]
        public void ConvertStringToSnakeCase(string input, string expectedOutput)
        {
            input.ToSnakeCase().ShouldBe(expectedOutput);
        }
    }
}
