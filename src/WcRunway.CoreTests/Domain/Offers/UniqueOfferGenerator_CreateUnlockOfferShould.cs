using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Offers;
using Xunit;

namespace WcRunway.CoreTests.Domain.Offers
{
    public class UniqueOfferGenerator_CreateUnlockOfferShould
    {
        private readonly IOfferData offerData;

        private readonly UniqueOfferGenerator sut;

        public UniqueOfferGenerator_CreateUnlockOfferShould()
        {
            var skeleton = new OfferSkeleton
            {
                UnitId = 217,
                OfferType = OfferType.UNIT_UNLOCK,
                Title = "The death machine",
                Description = "Death on wheels! This Offer includes an UNLOCKED Standard Juggernaut.",
                IconTitle = "",
                IconDescription = "",
                Cost = 99,
                FullCost = 1000,
                CostSku = "gold",
                Duration = 8200,
                Content = "",
                DisplayedItems = ""
            };

            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Skeletons).Returns(new List<OfferSkeleton> { skeleton });

            this.offerData = mockOfferData.Object;

            this.sut = new UniqueOfferGenerator(TestLogging.CreateLogger<UniqueOfferGenerator>(), offerData);
        }

        [Fact]
        public void AddTitleAccordingToSkeletonWhenSkeletonFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Title.ShouldBe("The death machine");
        }

        [Fact]
        public void AddGenericTitleWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Title.ShouldBe("Unlock the War Rig!");
        }
    }
}
