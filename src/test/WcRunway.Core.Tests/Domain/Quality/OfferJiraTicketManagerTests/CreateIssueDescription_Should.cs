using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Domain.Quality;
using Xunit;

namespace WcRunway.Core.Tests.Domain.Quality.OfferJiraTicketManagerTests
{
    public class CreateIssueDescription_Should
    {
        public CreateIssueDescription_Should()
        {
            sut = new OfferJiraTicketManager("https://jira.sjc.kixeye.com", "", "");
        }

        private readonly OfferJiraTicketManager sut;

        [Fact]
        public void CreateExpectedDescriptionWhenNoComments()
        {
            var offer = new Offer
            {
                OfferCode = "Test123",
                Id = 100,
                Title = "Test Offer",
                Description = "This is only a test",
                IconTitle = "Test Icon",
                IconDescription = "Icon description",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddDays(3),
                Duration = 700,
                Cost = 128,
                FullCost = -1,
                TemplateId = 6,
                ContentJson = "{\"gold\": 0 }",
                DisplayedItemsJson = "[{}]"
            };
            var expectedDescription = $"|*Offer Code:* {offer.OfferCode}|\n"
                + $"|*Offer Id:* {offer.Id}|\n"
                + $"|*Title:* {offer.Title}|\n"
                + $"|*Description:* {offer.Description}|\n"
                + $"|*Icon Title:* {offer.IconTitle}|\n"
                + $"|*Icon Description:* {offer.IconDescription}|\n"
                + $"|*Start Time:* {offer.StartTime}|\n"
                + $"|*End Time:* {offer.EndTime}|\n"
                + $"|*Duration:* {offer.Duration}|\n"
                + $"|*Cost:* {offer.Cost}|\n"
                + $"|*Full Cost:* {offer.FullCost}|\n"
                + $"|*Template ID:* {offer.TemplateId}|\n"
                + $"|*Content:* {offer.ContentJson}|\n"
                + $"|*Displayed Items:* {offer.DisplayedItemsJson}|\n"
                + $"|*Display Options:* {offer.DisplayOptionsJson}|\n"
                + $"|*Enabled:* {offer.IsEnabled}|\n";

            var result = sut.CreateIssueDescription(offer);

            result.ShouldBe(expectedDescription);
        }

        [Fact]
        public void CreateExpectedDescriptioWithComments()
        {
            var offer = new Offer
            {
                OfferCode = "Test123",
                Id = 100,
                Title = "Test Offer",
                Description = "This is only a test",
                IconTitle = "Test Icon",
                IconDescription = "Icon description",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddDays(3),
                Duration = 700,
                Cost = 128,
                FullCost = -1,
                TemplateId = 6,
                ContentJson = "{\"gold\": 0 }",
                DisplayedItemsJson = "[{}]"
            };
            var expectedDescription = $"These are my comments\n\n\n|*Offer Code:* {offer.OfferCode}\n"
                + $"|*Offer Id:* {offer.Id}\n"
                + $"|*Title:* {offer.Title}\n"
                + $"|*Description:* {offer.Description}\n"
                + $"|*Icon Title:* {offer.IconTitle}\n"
                + $"|*Icon Description:* {offer.IconDescription}\n"
                + $"|*Start Time:* {offer.StartTime}\n"
                + $"|*End Time:* {offer.EndTime}\n"
                + $"|*Duration:* {offer.Duration}\n"
                + $"|*Cost:* {offer.Cost}\n"
                + $"|*Full Cost:* {offer.FullCost}\n"
                + $"|*Template ID:* {offer.TemplateId}\n"
                + $"|*Content:* {offer.ContentJson}\n"
                + $"|*Displayed Items:* {offer.DisplayedItemsJson}\n"
                + $"|*Display Options:* {offer.DisplayOptionsJson}\n"
                + $"|*Enabled:* {offer.IsEnabled}\n";

            var result = sut.CreateIssueDescription(offer, "These are my comments");

            result.ShouldBe(expectedDescription);
        }
    }
}
