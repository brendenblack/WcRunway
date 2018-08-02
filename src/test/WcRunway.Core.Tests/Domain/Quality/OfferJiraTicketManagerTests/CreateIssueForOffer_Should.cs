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
        [Fact]
        public async Task Test()
        {
            var jira = new OfferJiraTicketManager("", "", "");
            var offer = new Offer { OfferCode = "Aug18TestJira" };
            await jira.CreateIssueForOffer(offer);

            offer.OfferCode.ShouldBe("Aug18TestJira");

        }
    }
}
