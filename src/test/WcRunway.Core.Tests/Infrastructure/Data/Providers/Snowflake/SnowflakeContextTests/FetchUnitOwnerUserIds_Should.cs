using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Providers.Snowflake;
using Xunit;

namespace WcRunway.Core.Tests.Infrastructure.Data.Providers.Snowflake.SnowflakeContextTests
{
    public class FetchUnitOwnerUserIds_Should : IClassFixture<LiveSnowflakeContextFixture>
    {
        public FetchUnitOwnerUserIds_Should(LiveSnowflakeContextFixture fixture)
        {
            this.snowflake = fixture.Snowflake;
        }

        private readonly SnowflakeContext snowflake;

        // Far too slow to keep enabled
        //[Fact]
        //public void ReturnListOfIds()
        //{
        //    var owners = snowflake.FetchUnitOwnerUserIds(251);

        //    owners.Count.ShouldBeGreaterThan(0);
        //}
    }
}
