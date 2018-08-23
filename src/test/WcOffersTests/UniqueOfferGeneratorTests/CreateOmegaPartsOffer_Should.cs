using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Sheets.Models;
using WcOffers;
using Xunit;

namespace WcOffersTests.UniqueOfferGeneratorTests
{
    public class CreateOmegaPartsOffer_Should : IClassFixture<UniqueOfferGeneratorFixture>, IDisposable
    {
        private readonly UniqueOfferGeneratorFixture fixture;
        private readonly UniqueOfferGenerator sut;

        public CreateOmegaPartsOffer_Should(UniqueOfferGeneratorFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.Sandbox2.Database.EnsureCreated();
            this.sut = fixture.OfferGenerator;
        }

        public void Dispose()
        {
            this.fixture.Sandbox2.Offers.RemoveRange(this.fixture.Sandbox2.Offers);
            this.fixture.Sandbox2.SaveChanges();
        }

        [Fact]
        public void SetOfferCode()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.OfferCode.ShouldBe("Jul18TestOPts");
        }

        [Theory]
        [InlineData(7)]
        [InlineData(1000)]
        [InlineData(245)]
        public void SetPriorityWhenPriorityValid(int priority)
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test", priority);

            offer.Priority.ShouldBe(priority);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2415)]
        public void SetPriorityTo0WhenProvidedInvalid(int invalidPriority)
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test", invalidPriority);

            offer.Priority.ShouldBe(0);
        }

        [Fact]
        public void SetStartTimeToNow()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.StartTime.ShouldBe(DateTimeOffset.Now, TimeSpan.FromSeconds(10));

        }

        [Fact]
        public void SetEndTimeTo3DaysFromNow()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.EndTime.ShouldBe(DateTimeOffset.Now.AddDays(3), TimeSpan.FromSeconds(10));

        }

        #region when skeleton exists
        [Fact]
        public void SetTitleAccordingToSkeletonWhenSkeletonFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.Title.ShouldBe(this.fixture.OmegaSkeleton.Title);
        }

        [Fact]
        public void SetDescriptionAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.Description.ShouldBe(this.fixture.OmegaSkeleton.Description);
        }

        [Fact]
        public void SetIconTitleAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.IconTitle.ShouldBe(this.fixture.OmegaSkeleton.IconTitle);
        }

        [Fact]
        public void SetIconDescriptionAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.IconDescription.ShouldBe(this.fixture.OmegaSkeleton.IconDescription);
        }

        [Fact]
        public void SetCostAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.Cost.ShouldBe(this.fixture.OmegaSkeleton.Cost);
        }

        [Fact]
        public void SetFullCostAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.FullCost.ShouldBe(this.fixture.OmegaSkeleton.FullCost);
        }

        [Fact]
        public void SetCostSkuAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.CostSku.ShouldBe(this.fixture.OmegaSkeleton.CostSku);
        }

        [Fact]
        public void SetDurationAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.Duration.ShouldBe(this.fixture.OmegaSkeleton.Duration);
        }

        [Fact]
        public void SetContentAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.ContentJson.ShouldBe(this.fixture.OmegaSkeleton.Content);
        }

        [Fact]
        public void SetDisplayedItemsAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.DisplayedItemsJson.ShouldBe(this.fixture.OmegaSkeleton.DisplayedItems);
        }

        #endregion


        #region when skeleton does not exist
        [Fact]
        public void SetGenericTitleWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.Title.ShouldBe("Omega War Rig Parts!");
        }

        [Fact]
        public void SetGenericDescriptionWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.Description.ShouldBe("Offer includes OMEGA PARTS for the WAR RIG!");
        }

        [Fact]
        public void SetGenericIconTitleWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.IconTitle.ShouldBe("War Rig Omega Parts!");
        }

        [Fact]
        public void SetGenericIconDescriptionWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.IconDescription.ShouldBe("Offer includes WAR RIG Omega Parts!");
        }

        [Fact]
        public void SetDefaultDurationWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.Duration.ShouldBe(86400);
        }

        [Fact]
        public void SetDefaultCostWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.Cost.ShouldBe(99);
        }

        [Fact]
        public void SetDefaultFullCostWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.FullCost.ShouldBe(-1);
        }

        [Fact]
        public void SetDefaultCostSkuWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.CostSku.ShouldBe("gold");
        }

        [Fact]
        public void SetEmptyContentWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.ContentJson.ShouldBe("{ \"skus\": { \"gold\": 0 } }");
        }

        [Fact]
        public void SetEmptyDisplayedItemsWhenSkeletonNotFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offer = this.sut.CreateOmegaPartsOffer(unit, "Jul18Test");

            offer.DisplayedItemsJson.ShouldBe("[ {} ]");
        }
        #endregion
    }
}
