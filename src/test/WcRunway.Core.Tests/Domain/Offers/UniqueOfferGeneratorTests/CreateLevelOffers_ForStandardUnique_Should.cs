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
    public class CreateLevelOffers_ForStandardUnique_Should : IClassFixture<StandardUniqueLevelOffersFixture>, IDisposable
    {
        private readonly UniqueOfferGeneratorFixture fixture;
        private readonly UniqueOfferGenerator sut;

        private readonly List<Offer> offers;
        

        public CreateLevelOffers_ForStandardUnique_Should(StandardUniqueLevelOffersFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.Sandbox2.Database.EnsureCreated();
            this.sut = fixture.OfferGenerator;
            this.offers = fixture.Offers;
        }

        public void Dispose()
        {
            this.fixture.Sandbox2.Offers.RemoveRange(this.fixture.Sandbox2.Offers);
            this.fixture.Sandbox2.SaveChanges();
        }

        [Fact]
        public void Return8Offers()
        {
            offers.Count.ShouldBe(8);
        }

        [Fact]
        public void SetLvl10PrereqToLvl5()
        {
            offers.First(o => o.OfferCode == "Jul18TestLv10").Prerequisite.ShouldBe("Jul18TestLv5");
        }

        [Fact]
        public void SetLvl15PrereqToLvl10()
        {
            offers.First(o => o.OfferCode == "Jul18TestLv15").Prerequisite.ShouldBe("Jul18TestLv10");
        }

        [Fact]
        public void SetLvl20PrereqToLvl15()
        {
            offers.First(o => o.OfferCode == "Jul18TestLv20").Prerequisite.ShouldBe("Jul18TestLv15");
        }

        [Fact]
        public void SetNoPrereqForLvl25()
        {
            offers.First(o => o.OfferCode == "Jul18TestLv25").Prerequisite.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public void SetLvl30PrereqToLvl25()
        {
            offers.First(o => o.OfferCode == "Jul18TestLv30").Prerequisite.ShouldBe("Jul18TestLv25");
        }

        [Fact]
        public void SetNoPrereqForLvl35()
        {
            offers.First(o => o.OfferCode == "Jul18TestLv35").Prerequisite.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public void SetLvl40PrereqToLvl35()
        {
            offers.First(o => o.OfferCode == "Jul18TestLv40").Prerequisite.ShouldBe("Jul18TestLv35");
        }

    }
}
