using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Implementation;
using Xunit;

namespace WcData.Tests.Infrastructure.Data.Providers.Snowflake.SnowflakeContextTests
{
    public class RecentDays_Should
    {
        public RecentDays_Should()
        {
            var mockLogger = new Mock<ILogger<SnowflakeContext>>();
            var dummyConnection = new SnowflakeConnectionDetails("abc", "username", "password");
            sut = new SnowflakeContext(mockLogger.Object, dummyConnection);
        }

        private readonly SnowflakeContext sut;


        [Fact]
        public void Return90AsDefault()
        {
           sut.RecentDays.ShouldBe(90);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(19274)]
        public void ReturnAssignedValueWhenGreaterThan1(int validDays)
        {
            sut.RecentDays = validDays;

            sut.RecentDays.ShouldBe(validDays);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ReturnDefaultValueWhenLessThan1(int invalidDays)
        {
            sut.RecentDays = invalidDays;

            sut.RecentDays.ShouldBe(90);
        }
    }
}
