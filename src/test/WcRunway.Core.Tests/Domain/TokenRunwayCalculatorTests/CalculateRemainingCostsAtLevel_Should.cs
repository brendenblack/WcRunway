using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcRunway.Core.Domain;
using WcRunway.Core.Tests.Infrastructure.Data.Providers.GoogleSheets.SheetsUnitDataTests;
using Xunit;

namespace WcRunway.Core.Tests.Domain.TokenRunwayCalculatorTests
{
    public class CalculateRemainingCostsAtLevel_Should : IClassFixture<RefreshUnitsFixture> // TODO: break dependence on this fixture
    {
        public CalculateRemainingCostsAtLevel_Should(RefreshUnitsFixture fixture)
        {
            this.units = fixture.sut.Units;
            this.sut = new TokenRunwayCalculator();
        }

        private IEnumerable<Unit> units;
        private TokenRunwayCalculator sut;

        [Fact]
        public void TrackSkuUsedAsCost()
        {
            var phalanx = this.units.First(u => u.Id == 251);

            var result = sut.CalculateRemainingCostsAtLevel(phalanx, 10);


            result.SkuCosts.ShouldContainKey("unit_upgrade_cor_phalanx");
        }

        [Theory]
        [InlineData(1, 55)]
        [InlineData(2, 55)]
        [InlineData(3, 55)]
        [InlineData(4, 55)]
        [InlineData(5, 55)]
        [InlineData(6, 55)]
        [InlineData(7, 55)]
        [InlineData(8, 55)]
        [InlineData(9, 55)]
        [InlineData(10, 55)]
        [InlineData(11, 54)]
        [InlineData(12, 52)]
        [InlineData(13, 49)]
        [InlineData(14, 45)]
        [InlineData(15, 40)]
        [InlineData(16, 34)]
        [InlineData(17, 27)]
        [InlineData(18, 19)]
        [InlineData(19, 10)]
        [InlineData(20, 0)]
        public void CalculateCorrectTokenCount(int level, int expectedCount)
        {
            var phalanx = this.units.First(u => u.Id == 251);

            var result = sut.CalculateRemainingCostsAtLevel(phalanx, level);
            result.SkuCosts.TryGetValue("unit_upgrade_cor_phalanx", out int tokenCount);

            tokenCount.ShouldBe(expectedCount);
        }
    }
}
