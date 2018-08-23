using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using WcData.Sheets;
using WcData.GameContext;
using WcData.Sheets.Models;

namespace WcOffers.Cli.Tests.Features.Generate.GenerateUniqueHandlerTests
{
    public class ExecuteFixture
    {
        public OfferSkeleton UnlockSkeleton { get; private set; }
        public OfferSkeleton OmegaSkeleton { get; private set; }
        public OfferSkeleton EliteSkeleton { get; private set; }
        public List<OfferSkeleton> TechSkeletons { get; private set; }
        public List<OfferSkeleton> LevelSkeletons { get; private set; }

        public IOfferData OfferData { get; private set; }
        /// <summary>
        /// Available units for testing include 217 Juggernaut with levels and 257 War Rig withoiut levels
        /// </summary>
        public IGameData GameContext { get; private set; }
        public IUnitOwnership UnitOwnership { get; private set; }

        public UniqueOfferGenerator OfferGenerator { get; private set; }

        public ExecuteFixture()
        {
            var skeletons = SetupOfferSkeletons();
            OfferData = SetupOfferData(skeletons);
            GameContext = SetupGameContext();
            this.OfferGenerator = SetupOfferGenerator(OfferData);
        }

        public IGameData SetupGameContext()
        {
            var mockGameCtx = new Mock<IGameData>();

            var warrig = new Unit(257) { Name = "War Rig" };
            var juggernaut = new Unit(217) { Name = "Juggernaut" };
            for (int i = 1; i <= 20; i++)
            {
                juggernaut.Levels.Add(new Level { Grade = 1, Number = i });
            }

            for (int i = 21; i <= 30; i++)
            {
                juggernaut.Levels.Add(new Level { Grade = 2, Number = i });
            }

            for (int i = 31; i <= 40; i++)
            {
                juggernaut.Levels.Add(new Level { Grade = 3, Number = i });
            }



            var units = new List<Unit> { juggernaut, warrig };
            mockGameCtx.Setup(ctx => ctx.Units).Returns(units);

            return mockGameCtx.Object;
        }

        public IOfferData SetupOfferData(List<OfferSkeleton> skeletons)
        {
            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Skeletons).Returns(skeletons);
            return mockOfferData.Object;
        }

        public Sandbox2Context SetupSandbox2(string name)
        {
            var options = new DbContextOptionsBuilder<Sandbox2Context>()
               .UseInMemoryDatabase(databaseName: name)
               .Options;

            var sb2 = new Sandbox2Context(options);

            return sb2;
        }

        private IUnitOwnership SetupUnitOwnership()
        {
            var mockUnitOwnership = new Mock<IUnitOwnership>();
            mockUnitOwnership.Setup(uo => uo.FetchUnitOwnerUserIds(251)).Returns(new List<int>()
            {
                123457,
                70917529,
                812903,
                41289015,
                1023908
            });
            mockUnitOwnership.Setup(uo => uo.FetchUnitNonOwnerUserIds(217)).Returns(new List<int> {71259071, 17923, 1231, 5616, 89182, 12498714, 129, 081258 });

            return mockUnitOwnership.Object;
        }

        public UniqueOfferGenerator SetupOfferGenerator(IOfferData offerData = null)
        {
            if (offerData == null)
            {
                offerData = OfferData;
            }

            ILogger<UniqueOfferGenerator> logger = TestHelpers.CreateLogger<UniqueOfferGenerator>();
            var gen = new UniqueOfferGenerator(logger, offerData);

            return gen;
        }

        private List<OfferSkeleton> SetupOfferSkeletons()
        {
            this.UnlockSkeleton = new OfferSkeleton
            {
                UnitId = 217,
                OfferType = OfferType.STANDARD_UNLOCK,
                Title = "The death machine",
                Description = "Death on wheels! This Offer includes an UNLOCKED Standard Juggernaut.",
                IconTitle = "Death Machine!",
                IconDescription = "Offer includes an UNLOCKED Standard Juggernaut.",
                Cost = 99,
                FullCost = 1000,
                CostSku = "gold",
                Duration = 8200,
                Content = "{ \"skus\": { \"juggernautunlocked\": 1 } }",
                DisplayedItems = "[ { \"item\": \"juggernautunlocked\", \"amount\": 1, \"order\": 1 } ]"
            };

            this.OmegaSkeleton = new OfferSkeleton
            {
                UnitId = 217,
                OfferType = OfferType.OMEGA_PARTS,
                Title = "Omega Juggernaut Parts!",
                Description = "Crush your enemies from afar with the Omega Juggernaut's terrifying firepower! This Offer contains 10 of the 80 parts you need, and can be purchased up to 8 times.",
                IconTitle = "Omega Juggernaut",
                IconDescription = "Offer contains 10 Omega Juggernaut parts",
                Cost = 99,
                FullCost = 1167,
                CostSku = "gold",
                Duration = 8200,
                Content = "{ \"skus\": {\"omegajuggernautpartcheck\": 10}}",
                DisplayedItems = "[{\"item\":\"juggernautcampaign_part\" , \"amount\": 10, \"order\":1} ]",
                MaximumQuanity = 8
            };

            this.EliteSkeleton = new OfferSkeleton
            {
                UnitId = 217,
                OfferType = OfferType.ELITE_PARTS,
                Title = "Elite Juggernaut Parts!",
                Description = "Crush your enemies from afar with the Elite Juggernaut's terrifying firepower! This Offer contains 10 of the 40 parts you need, and can be purchased up to 4 times.",
                IconTitle = "Elite Juggernaut",
                IconDescription = "Offer contains 10 Elite Juggernaut parts",
                Cost = 99,
                FullCost = 1167,
                CostSku = "gold",
                Duration = 86400,
                Content = "{ \"skus\": {\"elitejuggernaut_part\": 10}}",
                DisplayedItems = "[{ \"item\": \"elitejuggernaut_part\" , \"amount\": 10, \"order\": 1} ]",
                MaximumQuanity = 4
            };

            this.TechSkeletons = new List<OfferSkeleton>
            {
                new OfferSkeleton
                {
                    UnitId = 257,
                    OfferType = OfferType.TECH,
                    Title = "Steering KNUCKLE",
                    Description = "Make your War Rig bigger and badder as you crush enemies. This offer includes Steering Knuckle.",
                    IconTitle = "Steering KNUCKLE",
                    IconDescription = "This offer includes Steering Knuckle.",
                    Cost = 99,
                    FullCost = 1167,
                    CostSku = "gold",
                    Duration = 86400,
                    Content = "{ \"skus\": { \"warrig-steeringknuckle-componentunlocked\": 1 }}",
                    DisplayedItems = "[ {\"item\":\"warrig-steeringknuckle-componentunlocked\" , \"amount\": 1, \"order\":1} ]",
                    MaximumQuanity = 1
                },
                new OfferSkeleton
                {
                    UnitId = 257,
                    OfferType = OfferType.TECH,
                    Title = "Mounted FLAMETHROWER",
                    Description = "Make your War Rig bigger and badder as you crush enemies. This offer includes Mounted Flamethrower.",
                    IconTitle = "FLAMETHROWER",
                    IconDescription = "This offer includes Mounted Flamethrower.",
                    Cost = 99,
                    FullCost = 1167,
                    CostSku = "gold",
                    Duration = 86400,
                    Content = "{ \"skus\": { \"warrig-mountedflamethrower-componentunlocked\": 1 }}",
                    DisplayedItems = "[ {\"item\":\"warrig-mountedflamethrower-componentunlocked\" , \"amount\": 1, \"order\":1} ]",
                    MaximumQuanity = 1
                }
            };

            this.LevelSkeletons = new List<OfferSkeleton>
            {
                new OfferSkeleton
                {
                    UnitId = 217,
                    OfferType = OfferType.LEVELS,
                    Title = "Juggernaut Upgrades!",
                    Description = "Make your Juggernaut last longer and hit harder than ever before! This offer will upgrade your existing Juggernaut to level 5!",
                    IconTitle = "Juggernaut Upgrades!",
                    IconDescription = "This offer will upgrade your existing Juggernaut to level 5",
                    Cost = 39,
                    FullCost = 250,
                    CostSku = "gold",
                    Duration = 28800,
                    Content = "{\"unit_unlocks\":[ {\"type\":217, \"level\": 5} ]}",
                    DisplayedItems = "[ {\"item\":\"juggernautunlocked\" , \"amount\": 1, \"order\":1}]",
                    MaximumQuanity = 1
                },
                new OfferSkeleton
                {
                    UnitId = 217,
                    OfferType = OfferType.LEVELS,
                    Title = "Juggernaut Upgrades!",
                    Description = "Make your Juggernaut last longer and hit harder than ever before! This offer will upgrade your existing Juggernaut to level 10!",
                    IconTitle = "Juggernaut Upgrades!",
                    IconDescription = "This offer will upgrade your existing Juggernaut to level 10",
                    Cost = 39,
                    FullCost = 250,
                    CostSku = "gold",
                    Duration = 28800,
                    Content = "{\"unit_unlocks\":[ {\"type\":217, \"level\": 10} ]}",
                    DisplayedItems = "[ {\"item\":\"juggernautunlocked\" , \"amount\": 1, \"order\":1}]",
                    MaximumQuanity = 1
                },
                new OfferSkeleton
                {
                    UnitId = 217,
                    OfferType = OfferType.LEVELS,
                    Title = "Juggernaut Upgrades!",
                    Description = "Make your Juggernaut last longer and hit harder than ever before! This offer will upgrade your existing Juggernaut to level 15!",
                    IconTitle = "Juggernaut Upgrades!",
                    IconDescription = "This offer will upgrade your existing Juggernaut to level 15",
                    Cost = 39,
                    FullCost = 250,
                    CostSku = "gold",
                    Duration = 28800,
                    Content = "{\"unit_unlocks\":[ {\"type\":217, \"level\": 15} ]}",
                    DisplayedItems = "[ {\"item\":\"juggernautunlocked\" , \"amount\": 1, \"order\":1}]",
                    MaximumQuanity = 1
                },
                new OfferSkeleton
                {
                    UnitId = 217,
                    OfferType = OfferType.LEVELS,
                    Title = "Juggernaut Upgrades!",
                    Description = "Make your Juggernaut last longer and hit harder than ever before! This offer will upgrade your existing Juggernaut to level 20!",
                    IconTitle = "Juggernaut Upgrades!",
                    IconDescription = "This offer will upgrade your existing Juggernaut to level 20",
                    Cost = 39,
                    FullCost = 250,
                    CostSku = "gold",
                    Duration = 28800,
                    Content = "{\"unit_unlocks\":[ {\"type\":217, \"level\": 20} ]}",
                    DisplayedItems = "[ {\"item\":\"juggernautunlocked\" , \"amount\": 1, \"order\":1}]",
                    MaximumQuanity = 1
                },
                new OfferSkeleton
                {
                    UnitId = 217,
                    OfferType = OfferType.LEVELS,
                    Title = "Juggernaut Upgrades!",
                    Description = "Make your Juggernaut last longer and hit harder than ever before! This offer will upgrade your existing Juggernaut to level 25!",
                    IconTitle = "Juggernaut Upgrades!",
                    IconDescription = "This offer will upgrade your existing Juggernaut to level 25",
                    Cost = 39,
                    FullCost = 250,
                    CostSku = "gold",
                    Duration = 28800,
                    Content = "{\"unit_unlocks\":[ {\"type\":217, \"level\": 25} ]}",
                    DisplayedItems = "[ {\"item\":\"juggernautunlocked\" , \"amount\": 1, \"order\":1}]",
                    MaximumQuanity = 1
                },
                new OfferSkeleton
                {
                    UnitId = 217,
                    OfferType = OfferType.LEVELS,
                    Title = "Juggernaut Upgrades!",
                    Description = "Make your Juggernaut last longer and hit harder than ever before! This offer will upgrade your existing Juggernaut to level 30!",
                    IconTitle = "Juggernaut Upgrades!",
                    IconDescription = "This offer will upgrade your existing Juggernaut to level 30",
                    Cost = 39,
                    FullCost = 250,
                    CostSku = "gold",
                    Duration = 28800,
                    Content = "{\"unit_unlocks\":[ {\"type\":217, \"level\": 30} ]}",
                    DisplayedItems = "[ {\"item\":\"juggernautunlocked\" , \"amount\": 1, \"order\":1}]",
                    MaximumQuanity = 1
                },
                new OfferSkeleton
                {
                    UnitId = 217,
                    OfferType = OfferType.LEVELS,
                    Title = "Juggernaut Upgrades!",
                    Description = "Make your Juggernaut last longer and hit harder than ever before! This offer will upgrade your existing Juggernaut to level 35!",
                    IconTitle = "Juggernaut Upgrades!",
                    IconDescription = "This offer will upgrade your existing Juggernaut to level 35",
                    Cost = 39,
                    FullCost = 250,
                    CostSku = "gold",
                    Duration = 28800,
                    Content = "{\"unit_unlocks\":[ {\"type\":217, \"level\": 35} ]}",
                    DisplayedItems = "[ {\"item\":\"juggernautunlocked\" , \"amount\": 1, \"order\":1}]",
                    MaximumQuanity = 1
                },
                new OfferSkeleton
                {
                    UnitId = 217,
                    OfferType = OfferType.LEVELS,
                    Title = "Juggernaut Upgrades!",
                    Description = "Make your Juggernaut last longer and hit harder than ever before! This offer will upgrade your existing Juggernaut to level 40!",
                    IconTitle = "Juggernaut Upgrades!",
                    IconDescription = "This offer will upgrade your existing Juggernaut to level 40",
                    Cost = 39,
                    FullCost = 250,
                    CostSku = "gold",
                    Duration = 28800,
                    Content = "{\"unit_unlocks\":[ {\"type\":217, \"level\": 40 } ]}",
                    DisplayedItems = "[ {\"item\":\"juggernautunlocked\" , \"amount\": 1, \"order\":1}]",
                    MaximumQuanity = 1
                },

            };

            var result = new List<OfferSkeleton>
            {
                UnlockSkeleton, OmegaSkeleton, EliteSkeleton
            };
            result.AddRange(TechSkeletons);
            result.AddRange(LevelSkeletons);

            return result;
        }
    }
}
