using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;

namespace WcRunway.CoreTests.Domain.Offers
{
    public class UniqueOfferCreatorTests
    {
        private readonly IOfferData offerData;

        public UniqueOfferCreatorTests()
        {
            var skeleton = new OfferSkeleton
            {
                UnitId = 217,
                OfferType = OfferType.UNLOCK,
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
        }
    }
}
