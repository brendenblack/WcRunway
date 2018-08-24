using Shouldly;
using System.Linq;
using WcData.Sheets.Implementation;
using WcData.Sheets.Models;
using Xunit;

namespace WcDataTests.Sheets.Implementation.SheetsUnitDataTests
{
    public class RefreshUnits_Should : IClassFixture<RefreshUnitsFixture>
    {
        public RefreshUnits_Should(RefreshUnitsFixture fixture)
        {
            this.sut = fixture.sut;
        }

        private SheetsUnitData sut;

        [Fact]
        public void RetrieveUnits()
        {
            this.sut.Units.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public void SetName()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);

            phalanx.Name.ShouldBe("Phalanx");
        }

        [Fact]
        public void SetIdentifier()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);

            phalanx.Identifier.ShouldBe("UNIT_VEHICLE_PHALANX");
        }

        [Fact]
        public void SetDescription()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);

            phalanx.Description.ShouldBe("This durable APC has a mounted Machine Gun and carries with it a horde of Corpus Spartans who deploy as the unit takes damage to provide cover for it.");
        }

        [Fact]
        public void SetLevels()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);

            phalanx.Levels.Count.ShouldBe(20);
        }

        [Fact]
        public void SetLevelMetalUpgradeCost()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level7 = phalanx.Levels.First(l => l.Number == 7);

            level7.UpgradeCostMetal.ShouldBe(90000000);
        }

        [Fact]
        public void SetLevelOilUpgradeCost()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level7 = phalanx.Levels.First(l => l.Number == 7);

            level7.UpgradeCostOil.ShouldBe(90000000);
        }

        [Fact]
        public void SetLevelThoriumUpgradeCost()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level12 = phalanx.Levels.First(l => l.Number == 12);

            level12.UpgradeCostThorium.ShouldBe(2000000);
        }

        [Fact]
        public void SetUpgradeSkuWhenRelevant()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level14 = phalanx.Levels.First(l => l.Number == 14);

            level14.UpgradeSku.ShouldBe("unit_upgrade_cor_phalanx_14");
        }

        [Fact]
        public void NotSetCostSkuWhenIrrelevant()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level7 = phalanx.Levels.First(l => l.Number == 7);

            level7.UpgradeSku.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public void SetUpgradeSkuCostWhenRelevant()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level14 = phalanx.Levels.First(l => l.Number == 14);
            UpgradeSkuCost cost = level14.UpgradeSkuCosts.First();

            cost.Sku.ShouldBe("unit_upgrade_cor_phalanx");
            cost.Quantity.ShouldBe(4);
        }

        [Fact]
        public void NotSetUpgradeSkuCostWhenIrrelevant()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level7 = phalanx.Levels.First(l => l.Number == 7);

            level7.UpgradeSkuCosts.Count.ShouldBe(0);

        }

        [Fact]
        public void SetUpgradeCostGoldToNegativeValueWhenDisallowed()
        {
            Unit phalanx = this.sut.Units.First(u => u.Id == 251);
            Level level12 = phalanx.Levels.First(l => l.Number == 12);

            level12.UpgradeCostGold.ShouldBe(-1);
        }

        [Fact]
        public void SetUpgradeCostGold()
        {
            Unit warrig = this.sut.Units.First(u => u.Id == 257);
            Level level8 = warrig.Levels.First(l => l.Number == 8);

            level8.UpgradeCostGold.ShouldBe(50);
        }

    }
}
