using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Domain.Users;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;
using WcRunway.Core.Infrastructure.Data.Providers.Snowflake;

namespace WcRunway.Core.Tests.Domain.Offers.UniqueOfferGeneratorTests
{
    public class UniqueOfferGeneratorFixture : IDisposable
    {
        public UniqueOfferGenerator OfferGenerator { get; private set; }
        public Sandbox2Context Sandbox2 { get; private set; }
        public OfferSkeleton Skeleton { get; private set; }

        public UniqueOfferGeneratorFixture()
        {

            this.Sandbox2 = SetupSandbox2();

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
            this.Skeleton = skeleton;

            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Skeletons).Returns(new List<OfferSkeleton> { skeleton });

            var mockUnitOwnership = SetupUnitOwnership();


            ILogger<UniqueOfferGenerator> logger = TestHelpers.CreateLogger<UniqueOfferGenerator>();
            var gen = new UniqueOfferGenerator(logger, mockOfferData.Object, mockUnitOwnership);

            this.OfferGenerator = gen;
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

            return mockUnitOwnership.Object;
        }


        public void Dispose()
        {
            this.Sandbox2 = null;
        }
    }
}
