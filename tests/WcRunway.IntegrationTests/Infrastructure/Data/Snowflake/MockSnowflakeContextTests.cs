using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Snowflake;
using Xunit;

namespace WcRunway.IntegrationTests.Infrastructure.Data.Snowflake
{
    public class MockSnowflakeContextTests
    {
        public MockSnowflakeContextTests()
        {
            this.sut = new MockSnowflakeContext();
        }

        private readonly MockSnowflakeContext sut;

        [Fact]
        public void ShouldReturnDictionary()
        {
            var ownership = this.sut.GetUnitOwnership(251);

            ownership.Count.ShouldBe(64512);
        }
        
    }
}
