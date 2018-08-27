using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WcData.GameContext.Models;
using WcData.Sheets;
using WcData.Sheets.Models;

namespace WcOffers
{
    public class TemplatedOfferGenerator
    {
        private readonly ILogger<TemplatedOfferGenerator> logger;
        private readonly IOfferData offerData;

        public TemplatedOfferGenerator(ILogger<TemplatedOfferGenerator> logger, IOfferData offerData)
        {
            this.logger = logger;
            this.offerData = offerData;
        }

        public Offer GenerateOfferFromTemplate(OfferTemplate template, string offerCode, Dictionary<string,string> parameters = null, string prerequisite = "", int priority = 0)
        {

            var offer = new Offer
            {
                OfferCode = offerCode,
                Priority = priority,
                Title = template.OfferTitle,
                Description = template.OfferDescription,
                IconTitle = template.OfferIconTitle,
                IconDescription = template.OfferIconDescription,
                ContentJson = template.OfferContent,
                DisplayedItemsJson = template.OfferDisplay,
                Duration = template.OfferDuration,
                Cost = template.OfferCost,
                FullCost = template.OfferFullCost,
                CostSku = template.OfferCostSku,
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now + TimeSpan.FromDays(3),
                MaxQuantity = template.OfferMaxQuantity,
                Cooldown = 0,
                CooldownType = 0,
                TemplateId = template.OfferTemplateId,
                Prerequisite = prerequisite
            };

            foreach (var key in parameters.Keys)
            {
                offer.Title = offer.Title.Replace($"%{key}", parameters.GetValueOrDefault(key));
                offer.Description = offer.Description.Replace($"%{key}", parameters.GetValueOrDefault(key));
                offer.IconTitle = offer.IconTitle.Replace($"%{key}", parameters.GetValueOrDefault(key));
                offer.IconDescription = offer.IconDescription.Replace($"%{key}", parameters.GetValueOrDefault(key));
                offer.ContentJson = offer.ContentJson.Replace($"%{key}", parameters.GetValueOrDefault(key));
                offer.DisplayedItemsJson = offer.DisplayedItemsJson.Replace($"%{key}", parameters.GetValueOrDefault(key));
            }


            return offer;
        }


        /// <summary>
        /// Checks an offer for any placeholders that exist in the Title, Description, Icon Title, Icon Description, Content JSON or Display JSON. Returns the number of placeholders found.
        /// </summary>
        /// <param name="offer"></param>
        /// <returns>The number of placeholders found. A 0 indicates no placeholders remain</returns>
        public int CheckOfferForPlaceholder(Offer offer)
        {
            var checks = new Dictionary<string, string> {
                { "title", offer.Title },
                { "description", offer.Description},
                { "icon title", offer.IconTitle },
                { "icon description", offer.IconDescription },
                { "content", offer.ContentJson },
                { "display items", offer.DisplayedItemsJson }
            };


            Regex pattern = new Regex(@"%\w+%", RegexOptions.Compiled);
            int placeholdersFound = 0;


            foreach (var field in checks)
            {
                if (!string.IsNullOrWhiteSpace(field.Value))
                {
                    var matches = pattern.Matches(field.Value);
                    foreach (Match match in matches)
                    {
                        foreach (Capture capture in match.Captures)
                        {
                            logger.LogWarning($"Offer {field.Key} contains the placeholder '{capture.Value}'");
                            placeholdersFound++;
                        }
                    }
                }
            }

            return placeholdersFound;
        }
    }
}
