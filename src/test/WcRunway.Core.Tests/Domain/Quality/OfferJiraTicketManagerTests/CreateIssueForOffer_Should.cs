using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Domain.Quality;
using Xunit;

namespace WcRunway.Core.Tests.Domain.Quality.OfferJiraTicketManagerTests
{
    public class CreateIssueForOffer_Should
    {
        public CreateIssueForOffer_Should()
        {
            sut = new OfferJiraTicketManager("https://jira.sjc.kixeye.com", "", "");
        }

        private readonly OfferJiraTicketManager sut;

        [Fact]
        public async Task Test()
        {
            var offer = new Offer { OfferCode = "Aug18TestJira" };

            await sut.CreateIssueForOffer(offer);

            offer.OfferCode.ShouldBe("Aug18TestJira");

        }

        [Fact]
        public async Task ThrowWhenTicketSummaryExists()
        {
            var offer = new Offer { OfferCode = "Aug18TestJira" };

            await sut.CreateIssueForOffer(offer);

            offer.OfferCode.ShouldBe("Aug18TestJira");
        }
    }
}
