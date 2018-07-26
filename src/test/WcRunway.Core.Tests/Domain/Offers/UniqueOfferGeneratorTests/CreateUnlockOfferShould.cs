using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;
using Xunit;

namespace WcRunway.Core.Tests.Domain.Offers.UniqueOfferGeneratorTests
{
    public class CreateUnlockOfferShould
    {
        private readonly UniqueOfferGenerator sut;
        private readonly OfferSkeleton _skeleton;

        public CreateUnlockOfferShould()
        {
            var options = new DbContextOptionsBuilder<Sandbox2Context>()
                .UseInMemoryDatabase(databaseName: "CreateUnlockOffer")
                .Options;

            var sb2 = new Sandbox2Context(options);
            ILogger<UniqueOfferGenerator> logger = TestHelpers.CreateLogger<UniqueOfferGenerator>();

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

            this._skeleton = skeleton;

            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Skeletons).Returns(new List<OfferSkeleton> { skeleton });

            var gen = new UniqueOfferGenerator(logger, mockOfferData.Object);

            this.sut = gen;
        }

        [Fact]
        public void SetOfferCode()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.OfferCode.ShouldBe("Jul18TestUnl");
        }

        [Theory]
        [InlineData(7)]
        [InlineData(1000)]
        [InlineData(245)]
        public void SetPriorityWhenProvidedValid(int priority)
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test", priority);

            offer.Priority.ShouldBe(priority);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2415)]
        public void SetPriorityTo0WhenProvidedInvalid(int invalidPriority)
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test", invalidPriority);

            offer.Priority.ShouldBe(0);
        }

        [Fact]
        public void SetStartTimeToNow()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.StartTime.ShouldBe(DateTimeOffset.Now, TimeSpan.FromSeconds(10));

        }

        [Fact]
        public void SetEndTimeTo3DaysFromNow()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.EndTime.ShouldBe(DateTimeOffset.Now.AddDays(3), TimeSpan.FromSeconds(10));

        }

        #region when skeleton exists
        [Fact]
        public void SetTitleAccordingToSkeletonWhenSkeletonFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Title.ShouldBe(this._skeleton.Title);
        }

        [Fact]
        public void SetDescriptionAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Description.ShouldBe(this._skeleton.Description);
        }

        [Fact]
        public void SetIconTitleAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.IconTitle.ShouldBe(this._skeleton.IconTitle);
        }

        [Fact]
        public void SetIconDescriptionAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.IconDescription.ShouldBe(this._skeleton.IconDescription);
        }

        [Fact]
        public void SetCostAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Cost.ShouldBe(this._skeleton.Cost);
        }

        [Fact]
        public void SetFullCostAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.FullCost.ShouldBe(this._skeleton.FullCost);
        }

        [Fact]
        public void SetCostSkuAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.CostSku.ShouldBe(this._skeleton.CostSku);
        }

        [Fact]
        public void SetDurationAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Duration.ShouldBe(this._skeleton.Duration);
        }

        [Fact]
        public void SetContentAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.ContentJson.ShouldBe(this._skeleton.Content);
        }

        [Fact]
        public void SetDisplayedItemsAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.DisplayedItemsJson.ShouldBe(this._skeleton.DisplayedItems);
        }

        #endregion


        #region when skeleton does not exist
        [Fact]
        public void SetGenericTitleWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Title.ShouldBe("Unlock the War Rig!");
        }

        [Fact]
        public void SetGenericDescriptionWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Description.ShouldBe("Offer includes an UNLOCK of the powerful WAR RIG!");
        }

        [Fact]
        public void SetGenericIconTitleWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.IconTitle.ShouldBe("War Rig UNLOCK!");
        }

        [Fact]
        public void SetGenericIconDescriptionWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.IconDescription.ShouldBe("Offer includes an UNLOCK of the powerful WAR RIG!");
        }

        [Fact]
        public void SetDefaultDurationWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Duration.ShouldBe(86400);
        }

        [Fact]
        public void SetDefaultCostWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Cost.ShouldBe(99);
        }

        [Fact]
        public void SetDefaultFullCostWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.FullCost.ShouldBe(-1);
        }

        [Fact]
        public void SetDefaultCostSkuWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.CostSku.ShouldBe("gold");
        }

        [Fact]
        public void SetEmptyContentWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.ContentJson.ShouldBe("{ \"skus\": { \"gold\": 0 } }");
        }

        [Fact]
        public void SetEmptyDisplayedItemsWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.DisplayedItemsJson.ShouldBe("[]");
        }
        #endregion

    }
}
