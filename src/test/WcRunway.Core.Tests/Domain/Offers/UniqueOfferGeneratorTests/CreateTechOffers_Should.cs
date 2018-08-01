using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Offers;
using Xunit;

namespace WcRunway.Core.Tests.Domain.Offers.UniqueOfferGeneratorTests
{
    public class CreateTechOffers_Should : IClassFixture<UniqueOfferGeneratorFixture>, IDisposable
    {
        private readonly UniqueOfferGeneratorFixture fixture;
        private readonly UniqueOfferGenerator sut;
        private readonly OfferSkeleton skeleton1;
        private readonly OfferSkeleton skeleton2;

        public CreateTechOffers_Should(UniqueOfferGeneratorFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.Sandbox2.Database.EnsureCreated();
            this.sut = fixture.OfferGenerator;

            skeleton1 = this.fixture.TechSkeletons.First(s => s.Title == "Steering KNUCKLE");
            skeleton2 = this.fixture.TechSkeletons.First(s => s.Title == "Mounted FLAMETHROWER");
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
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            foreach (var offer in offers)
            {
                offer.OfferCode.ShouldStartWith("Jul18TestTek");
            }
        }

        [Theory]
        [InlineData(7)]
        [InlineData(1000)]
        [InlineData(245)]
        public void SetPriorityWhenPriorityValid(int priority)
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test", priority);

            offers.First().Priority.ShouldBe(priority);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2415)]
        public void SetPriorityTo0WhenProvidedInvalid(int invalidPriority)
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test", invalidPriority);

            offers.First().Priority.ShouldBe(0);
        }

        [Fact]
        public void SetStartTimeToNow()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            offers.First().StartTime.ShouldBe(DateTimeOffset.Now, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void SetEndTimeTo3DaysFromNow()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            offers.First().EndTime.ShouldBe(DateTimeOffset.Now.AddDays(3), TimeSpan.FromSeconds(10));
        }

        #region when skeleton exists
        [Fact]
        public void SetTitleAccordingToSkeletonWhenSkeletonFound()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            foreach (var offer in offers)
            {
                this.fixture.TechSkeletons.Select(s => s.Title).Contains(offer.Title).ShouldBeTrue($"Title was {offer.Title}");
            }
        }

        [Fact]
        public void SetDescriptionAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            foreach (var offer in offers)
            {
                this.fixture.TechSkeletons.Select(s => s.Description).Contains(offer.Description).ShouldBeTrue($"Description was {offer.Description}");
            }
        }

        [Fact]
        public void SetIconTitleAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            foreach (var offer in offers)
            {
                this.fixture.TechSkeletons.Select(s => s.IconTitle).Contains(offer.IconTitle).ShouldBeTrue($"Icon title was {offer.IconTitle}");
            }
        }

        [Fact]
        public void SetIconDescriptionAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            foreach (var offer in offers)
            {
                this.fixture.TechSkeletons.Select(s => s.IconDescription).Contains(offer.IconDescription).ShouldBeTrue($"Icon description was {offer.IconDescription}");
            }
        }

        [Fact]
        public void SetCostAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            offers.First(o => o.Title == skeleton1.Title).Cost.ShouldBe(skeleton1.Cost);
            offers.First(o => o.Title == skeleton2.Title).Cost.ShouldBe(skeleton2.Cost);
        }

        [Fact]
        public void SetFullCostAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            offers.First(o => o.Title == skeleton1.Title).FullCost.ShouldBe(skeleton1.FullCost);
            offers.First(o => o.Title == skeleton2.Title).FullCost.ShouldBe(skeleton2.FullCost);
        }

        [Fact]
        public void SetCostSkuAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            offers.First(o => o.Title == skeleton1.Title).CostSku.ShouldBe(skeleton1.CostSku);
            offers.First(o => o.Title == skeleton2.Title).CostSku.ShouldBe(skeleton2.CostSku);
        }

        [Fact]
        public void SetDurationAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            offers.First(o => o.Title == skeleton1.Title).Duration.ShouldBe(skeleton1.Duration);
            offers.First(o => o.Title == skeleton2.Title).Duration.ShouldBe(skeleton2.Duration);
        }

        [Fact]
        public void SetContentAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            offers.First(o => o.Title == skeleton1.Title).ContentJson.ShouldBe(skeleton1.Content);
            offers.First(o => o.Title == skeleton2.Title).ContentJson.ShouldBe(skeleton2.Content);
        }

        [Fact]
        public void SetDisplayedItemsAccordingToSkeletonWhenSkeletonExists()
        {
            var unit = new Unit(257) { Name = "War Rig" };
            var offers = this.sut.CreateTechOffers(unit, "Jul18Test");

            offers.First(o => o.Title == skeleton1.Title).DisplayedItemsJson.ShouldBe(skeleton1.DisplayedItems);
            offers.First(o => o.Title == skeleton2.Title).DisplayedItemsJson.ShouldBe(skeleton2.DisplayedItems);
        }

        #endregion


        #region when skeletons do not exist
        [Fact]
        public void SetGenericTitleWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().Title.ShouldBe("Tech out your Juggernaut!");
        }

        [Fact]
        public void SetGenericDescriptionWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().Description.ShouldBe("Offer includes TECH for the JUGGERNAUT!");
        }

        [Fact]
        public void SetGenericIconTitleWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().IconTitle.ShouldBe("Juggernaut Tech!");
        }

        [Fact]
        public void SetGenericIconDescriptionWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().IconDescription.ShouldBe("Offer includes JUGGERNAUT Tech!");
        }

        [Fact]
        public void SetDefaultDurationWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().Duration.ShouldBe(86400);
        }

        [Fact]
        public void SetDefaultCostWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().Cost.ShouldBe(99);
        }

        [Fact]
        public void SetDefaultFullCostWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().FullCost.ShouldBe(-1);
        }

        [Fact]
        public void SetDefaultCostSkuWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().CostSku.ShouldBe("gold");
        }

        [Fact]
        public void SetEmptyContentWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().ContentJson.ShouldBe("{ \"skus\": { \"gold\": 0 } }");
        }

        [Fact]
        public void SetEmptyDisplayedItemsWhenSkeletonNotFound()
        {
            var unit = new Unit(217) { Name = "Juggernaut" };
            var offer = this.sut.CreateTechOffers(unit, "Jul18Test");

            offer.First().DisplayedItemsJson.ShouldBe("[ {} ]");
        }
        #endregion
    }
}
