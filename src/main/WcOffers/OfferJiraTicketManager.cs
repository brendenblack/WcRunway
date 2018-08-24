using Atlassian.Jira;
using Microsoft.Extensions.Logging;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WcData.GameContext.Models;

namespace WcOffers
{
    public class OfferJiraTicketManager
    {
        /*
         * Issue creation metadata can be found at
         * https://jira.sjc.kixeye.com/rest/api/2/issue/createmeta?projectKeys=WC&issuetypeNames=Offer&expand=projects.issuetypes.fields
         * 
         * SDK documentation can be found at
         * https://bitbucket.org/farmas/atlassian.net-sdk/wiki/Home
         */

        private readonly Jira jira;
        private readonly ILogger<OfferJiraTicketManager> logger;

        public OfferJiraTicketManager(ILogger<OfferJiraTicketManager> logger, Jira jira)
        {
            this.logger = logger;
            this.jira = jira;
        }


        public async Task<Issue> CreateIssueForOffer(Offer offer)
        {
            logger.LogDebug("Creating issue for offer {}", offer.OfferCode);
            var issue = jira.CreateIssue("WC");
            issue.Type = "Offer";
            issue.Summary = offer.OfferCode;
            issue.Description = CreateIssueDescription(offer);
            issue.FixVersions.Add("Offers");
            issue["Severity"] = "3 - Normal";

            var createdIssue = await issue.SaveChangesAsync();
            return issue;


        }

        public async Task<bool> CheckForExistingTicket(Offer offer)
        {
            return await new Task<bool>(() => { return true;  });
        }

        public string CreateIssueDescription(Offer offer, string comment = "")
        {
            string description = "";

            if (!string.IsNullOrWhiteSpace(comment))
            {
                description += comment + "\n\n\n";
            }

            description += $"|*Offer Code:* {offer.OfferCode}|\n"
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

            return description;

        }

        public async Task SubmitToQa(Issue issue, string user = "")
        {
            issue.Resolution = "Resolved";
            new WorkflowTransitionUpdates();
            await issue.WorkflowTransitionAsync("Send to QA");

            if (!string.IsNullOrWhiteSpace(user))
            {
                issue.Assignee = user;
                await issue.SaveChangesAsync();
            }
        }
    }
}
