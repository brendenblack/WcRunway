using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcRunway.Core.Domain;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets;
using Xunit;

namespace WcRunway.IntegrationTests.Sheets
{
    public class SheetsUnitData_RefreshUnitsTests : IClassFixture<SheetsUnitData_RefreshFixture>
    {
        public SheetsUnitData_RefreshUnitsTests(SheetsUnitData_RefreshFixture fixture)
        {
            this.sut = fixture.sut;
        }

        private SheetsUnitData sut;

        [Fact]
        public void RefreshUnits_ShouldRetrieveUnits()
        {
            this.sut.Units.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public void RefreshUnits_ShouldSetName()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);

            phalanx.Name.ShouldBe("Phalanx");
        }

        [Fact]
        public void RefreshUnits_ShouldSetIdentifier()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);

            phalanx.Identifier.ShouldBe("UNIT_VEHICLE_PHALANX");
        }

        [Fact]
        public void RefreshUnits_ShouldSetDescription()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);

            phalanx.Description.ShouldBe("This durable APC has a mounted Machine Gun and carries with it a horde of Corpus Spartans who deploy as the unit takes damage to provide cover for it.");
        }

        [Fact]
        public void ShouldSetLevels()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);

            phalanx.Levels.Count.ShouldBe(20);
        }

        [Fact]
        public void ShouldSetLevelMetalUpgradeCost()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level7 = phalanx.Levels.First(l => l.Number == 7);

            level7.UpgradeCostMetal.ShouldBe(90000000);
        }

        [Fact]
        public void ShouldSetLevelOilUpgradeCost()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level7 = phalanx.Levels.First(l => l.Number == 7);

            level7.UpgradeCostOil.ShouldBe(90000000);
        }

        [Fact]
        public void ShouldSetLevelThoriumUpgradeCost()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level12 = phalanx.Levels.First(l => l.Number == 12);

            level12.UpgradeCostThorium.ShouldBe(2000000);
        }

        [Fact]
        public void ShouldSetUpgradeSkuWhenRelevant()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level14 = phalanx.Levels.First(l => l.Number == 14);

            level14.UpgradeSku.ShouldBe("unit_upgrade_cor_phalanx_14");
        }

        [Fact]
        public void ShouldNotSetCostSkuWhenIrrelevant()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level7 = phalanx.Levels.First(l => l.Number == 7);

            level7.UpgradeSku.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public void ShouldSetUpgradeSkuCostWhenRelevant()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level14 = phalanx.Levels.First(l => l.Number == 14);
            UpgradeSkuCost cost = level14.UpgradeSkuCosts.First();

            cost.Sku.ShouldBe("unit_upgrade_cor_phalanx");
            cost.Quantity.ShouldBe(4);
        }

        [Fact]
        public void ShouldNotSetUpgradeSkuCostWhenIrrelevant()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level7 = phalanx.Levels.First(l => l.Number == 7);

            level7.UpgradeSkuCosts.Count.ShouldBe(0);

        }

        [Fact]
        public void ShouldSetUpgradeCostGoldToNegativeValueWhenDisallowed()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level12 = phalanx.Levels.First(l => l.Number == 12);

            level12.UpgradeCostGold.ShouldBe(-1);
        }

        [Fact]
        public void ShouldSetUpgradeCostGold()
        {
            Unit warrig = this.sut.Units.First(u => u.Id == 257);
            Level level8 = warrig.Levels.First(l => l.Number == 8);

            level8.UpgradeCostGold.ShouldBe(50);
        }

    }
}
