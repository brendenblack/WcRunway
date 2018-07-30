using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Offers;
using Xunit;

namespace WcRunway.Core.Tests.Domain.Offers.UniqueOfferGeneratorTests
{
    public class FetchUnlockCohort_Should : IClassFixture<UniqueOfferGeneratorFixture>
    {

        public FetchUnlockCohort_Should(UniqueOfferGeneratorFixture fixture)
        {
            sut = fixture.OfferGenerator;
        }

        private readonly UniqueOfferGenerator sut;

        [Fact]
        public void ReturnListOfUserIds()
        {
            var unit = new Unit(251);

            var cohort = sut.FetchUnlockCohort(unit);

            cohort.Count.ShouldBe(5);

        }
    }
}
