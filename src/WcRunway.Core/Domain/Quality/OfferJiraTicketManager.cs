using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WcRunway.Core.Domain.Offers;

namespace WcRunway.Core.Domain.Quality
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

        public OfferJiraTicketManager(string url, string username, string password)
        {
            jira = Jira.CreateRestClient(url, username, password);
        }


        public async Task CreateIssueForOffer(Offer offer)
        {
            var issue = jira.CreateIssue("WC");
            issue.Type = "Offer";
            issue.Summary = offer.OfferCode;
            issue.Description = "This is a test description"; // TODO: make table
            issue.FixVersions.Add("Offers");
            issue["Severity"] = "3 - Normal";

            await issue.SaveChangesAsync();
        }
    }
}
