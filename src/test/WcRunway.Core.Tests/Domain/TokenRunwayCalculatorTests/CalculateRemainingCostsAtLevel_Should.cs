//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using WcCore.Domain;
//using Xunit;

//namespace WcCore.Tests.Domain.TokenRunwayCalculatorTests
//{
//    public class CalculateRemainingCostsAtLevel_Should
//    {
//        public CalculateRemainingCostsAtLevel_Should()
//        {
//            this.units = new List<Unit>
//            {
//                new Unit(251)
//                {
//                    Name = "Phalanx",
//                    Levels = new List<Level>
//                    {
//                        new Level { Number = 1 },
//                        new Level { Number = 2 },
//                        new Level { Number = 3 },
//                        new Level { Number = 4 },
//                        new Level { Number = 5 },
//                        new Level { Number = 6 },
//                        new Level { Number = 7 },
//                        new Level { Number = 8 },
//                        new Level { Number = 9 },
//                        new Level { Number = 10 },
//                        new Level { Number = 11, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 1) } },
//                        new Level { Number = 12, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 2) } },
//                        new Level { Number = 13, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 3) } },
//                        new Level { Number = 14, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 4) } },
//                        new Level { Number = 15, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 5) } },
//                        new Level { Number = 16, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 6) } },
//                        new Level { Number = 17, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 7) } },
//                        new Level { Number = 18, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 8) } },
//                        new Level { Number = 19, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 9) } },
//                        new Level { Number = 20, UpgradeSkuCosts = new List<UpgradeSkuCost> { new UpgradeSkuCost("unit_upgrade_cor_phalanx", 10) } },
//                    }
//                }
//            };

//            this.sut = new TokenRunwayCalculator();
//        }

//        private IEnumerable<Unit> units;
//        private TokenRunwayCalculator sut;

//        [Fact]
//        public void TrackSkuUsedAsCost()
//        {
//            var phalanx = this.units.First(u => u.Id == 251);

//            var result = sut.CalculateRemainingCostsAtLevel(phalanx, 10);


//            result.SkuCosts.ShouldContainKey("unit_upgrade_cor_phalanx");
//        }

//        [Theory]
//        [InlineData(1, 55)]
//        [InlineData(2, 55)]
//        [InlineData(3, 55)]
//        [InlineData(4, 55)]
//        [InlineData(5, 55)]
//        [InlineData(6, 55)]
//        [InlineData(7, 55)]
//        [InlineData(8, 55)]
//        [InlineData(9, 55)]
//        [InlineData(10, 55)]
//        [InlineData(11, 54)]
//        [InlineData(12, 52)]
//        [InlineData(13, 49)]
//        [InlineData(14, 45)]
//        [InlineData(15, 40)]
//        [InlineData(16, 34)]
//        [InlineData(17, 27)]
//        [InlineData(18, 19)]
//        [InlineData(19, 10)]
//        [InlineData(20, 0)]
//        public void CalculateCorrectTokenCount(int level, int expectedCount)
//        {
//            var phalanx = this.units.First(u => u.Id == 251);

//            var result = sut.CalculateRemainingCostsAtLevel(phalanx, level);
//            result.SkuCosts.TryGetValue("unit_upgrade_cor_phalanx", out int tokenCount);

//            tokenCount.ShouldBe(expectedCount);
//        }
//    }
//}
