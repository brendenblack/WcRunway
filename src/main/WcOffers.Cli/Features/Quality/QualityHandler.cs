﻿using Atlassian.Jira;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcData.GameContext;

namespace WcOffers.Cli.Features.Quality
{
    public class QualityHandler
    {
        private readonly ILogger<QualityHandler> logger;
        private readonly OfferJiraTicketManager jira;
        private readonly ISandbox2Context sb2;

        public QualityHandler(ILogger<QualityHandler> logger, OfferJiraTicketManager jira, ISandbox2Context sb2)
        {
            this.logger = logger;
            this.jira = jira;
            this.sb2 = sb2;
        }

        public int Execute(QualityOptions opts)
        {
            foreach (var offerCode in opts.OfferCodes)
            {
                var offer = sb2.Offers.FirstOrDefault(o => o.OfferCode == offerCode);

                if (offer != null)
                {
                    logger.LogInformation($"Creating JIRA issue for offer {offer.OfferCode}");

                    Task.Run(async () => 
                    {
                        var issue = await jira.CreateIssueForOffer(offer);
                        logger.LogInformation("Issue created with key {}", issue.Key.Value);
                        if (!string.IsNullOrWhiteSpace(opts.Assignee))
                        {
                            await jira.SubmitToQa(issue, opts.Assignee);
                            logger.LogInformation("Issue {} submitted for QA and assigned to {}", issue.Key.Value, opts.Assignee);
                        }
                    }).Wait();

                    
                }
                else
                {
                    logger.LogError($"Unable to find an offer with code {offerCode}");
                    return -1;
                }
            }

            return 0;
        }
    }
}
