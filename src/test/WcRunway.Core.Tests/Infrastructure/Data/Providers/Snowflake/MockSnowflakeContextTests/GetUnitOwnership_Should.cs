using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Snowflake;
using Xunit;

namespace WcRunway.Core.Tests.Infrastructure.Data.Providers.Snowflake.MockSnowflakeContextTests
{
    public class GetUnitOwnership_Should
    {
        public GetUnitOwnership_Should()
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
