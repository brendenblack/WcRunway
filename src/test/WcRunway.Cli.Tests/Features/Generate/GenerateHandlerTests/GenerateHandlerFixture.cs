using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcRunway.Cli.Features.Generate;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Game;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Domain.Users;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;

namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
{
    public class GenerateHandlerFixture
    {
        public GenerateHandler Handler { get; private set; }
        public UniqueOfferGenerator OfferGenerator { get; private set; }
        public Sandbox2Context Sandbox2 { get; private set; }
        public OfferSkeleton Skeleton { get; private set; }

        public GenerateHandlerFixture()
        {
            this.Sandbox2 = SetupSandbox2();

            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Skeletons).Returns(SetupSkeletons());

            this.Skeleton = mockOfferData.Object.Skeletons.First(); // TODO

            var unitOwnership = SetupUnitOwnership();
            this.OfferGenerator = SetupOfferGenerator(mockOfferData.Object, unitOwnership);

            var mockGameContext = SetupGameContext();

            var genLogger = TestHelpers.CreateLogger<GenerateHandler>();
            this.Handler = new GenerateHandler(genLogger, mockGameContext, unitOwnership, OfferGenerator, Sandbox2);
        }

        private IGameContext SetupGameContext()
        {
            var mockGameCtx = new Mock<IGameContext>();
            var units = new List<Unit>
            {
                new Unit(217)
                {
                    Name = "Juggernaut"
                }
            };
            mockGameCtx.Setup(ctx => ctx.Units).Returns(units);

            return mockGameCtx.Object;
        }

        private Sandbox2Context SetupSandbox2()
        {
            var options = new DbContextOptionsBuilder<Sandbox2Context>()
               .UseInMemoryDatabase(databaseName: "CreateUnlockOffer")
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

        private UniqueOfferGenerator SetupOfferGenerator(IOfferData offerData, IUnitOwnership unitOwnership)
        {
            ILogger<UniqueOfferGenerator> logger = TestHelpers.CreateLogger<UniqueOfferGenerator>();
            var gen = new UniqueOfferGenerator(logger, offerData, unitOwnership);

            return gen;
        }

        private List<OfferSkeleton> SetupSkeletons()
        {
            var skeleton = new OfferSkeleton
            {
                UnitId = 217,
                OfferType = OfferType.UNIT_UNLOCK,
                Title = "The death machine",
                Description = "Death on wheels! This Offer includes an UNLOCKED Standard Juggernaut.",
                IconTitle = "Death Machine!",
                IconDescription = "Offer includes an UNLOCKED Standard Juggernaut.",
                Cost = 99,
                FullCost = 1000,
                CostSku = "gold",
                Duration = 8200,
                Content = "",
                DisplayedItems = ""
            };

            return new List<OfferSkeleton> { skeleton };
        }
    }
}
