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
    public class CreateUnlockOffer_Should : IClassFixture<UniqueOfferGeneratorFixture>, IDisposable
    {
        //private readonly UniqueOfferGenerator sut;
        //private readonly OfferSkeleton _skeleton;
        private readonly UniqueOfferGeneratorFixture fixture;
        private readonly UniqueOfferGenerator sut;

        public CreateUnlockOffer_Should(UniqueOfferGeneratorFixture fixture)
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
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.OfferCode.ShouldBe("Jul18TestUnl");
        }

        [Theory]
        [InlineData("Jul18TestTestTestTest")]
        [InlineData("Jul18TestTestTestUnlock")]
        [InlineData("Jul18TestTestTestLocked")]
        [InlineData("Jul18TestTestTestT")]
        public void TrimOfferCodeWhenLongerThan17Characters(string offerCodePrefix)
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, offerCodePrefix);

            offer.OfferCode.ShouldBe("Jul18TestTestTestUnl");
        }

        [Theory]
        [InlineData(7)]
        [InlineData(1000)]
        [InlineData(245)]
        public void SetPriorityWhenPriorityValid(int priority)
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

            offer.Title.ShouldBe(this.fixture.UnlockSkeleton.Title);
        }

        [Fact]
        public void SetDescriptionAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Description.ShouldBe(this.fixture.UnlockSkeleton.Description);
        }

        [Fact]
        public void SetIconTitleAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.IconTitle.ShouldBe(this.fixture.UnlockSkeleton.IconTitle);
        }

        [Fact]
        public void SetIconDescriptionAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.IconDescription.ShouldBe(this.fixture.UnlockSkeleton.IconDescription);
        }

        [Fact]
        public void SetCostAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Cost.ShouldBe(this.fixture.UnlockSkeleton.Cost);
        }

        [Fact]
        public void SetFullCostAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.FullCost.ShouldBe(this.fixture.UnlockSkeleton.FullCost);
        }

        [Fact]
        public void SetCostSkuAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.CostSku.ShouldBe(this.fixture.UnlockSkeleton.CostSku);
        }

        [Fact]
        public void SetDurationAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.Duration.ShouldBe(this.fixture.UnlockSkeleton.Duration);
        }

        [Fact]
        public void SetContentAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.ContentJson.ShouldBe(this.fixture.UnlockSkeleton.Content);
        }

        [Fact]
        public void SetDisplayedItemsAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateUnlockOffer(unit, "Jul18Test");

            offer.DisplayedItemsJson.ShouldBe(this.fixture.UnlockSkeleton.DisplayedItems);
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

            offer.DisplayedItemsJson.ShouldBe("[ {} ]");
        }
        #endregion

    }
}
