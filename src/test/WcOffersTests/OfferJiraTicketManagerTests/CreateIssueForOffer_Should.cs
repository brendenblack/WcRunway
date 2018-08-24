using Atlassian.Jira;
using WcOffers;

namespace WcOffersTests.OfferJiraTicketManagerTests
{
    public class CreateIssueForOffer_Should
    {
        public CreateIssueForOffer_Should()
        {
            var jira = Jira.CreateRestClient("https://jira.sjc.kixeye.com", "", "");
            var logger = TestHelpers.CreateLogger<OfferJiraTicketManager>();
            sut = new OfferJiraTicketManager(logger, jira);
        }

        private readonly OfferJiraTicketManager sut;

        //[Fact]
        //public async Task Test()
        //{
        //    var offer = new Offer { OfferCode = "Aug18TestJira" };

        //    await sut.CreateIssueForOffer(offer);

        //    offer.OfferCode.ShouldBe("Aug18TestJira");

        //}

        //[Fact]
        //public async Task ThrowWhenTicketSummaryExists()
        //{
        //    var offer = new Offer { OfferCode = "Aug18TestJira" };

        //    await sut.CreateIssueForOffer(offer);

        //    offer.OfferCode.ShouldBe("Aug18TestJira");
        //}
    }
}
