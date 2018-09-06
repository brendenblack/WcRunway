using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcData.GameContext;
using WcData.GameContext.Models;
using WcData.Sheets;

namespace WcOffers.Cli.Features.Generate
{
    public class GenerateHandler
    {
        private readonly ILogger<GenerateHandler> logger;
        private readonly IOfferData offerData;
        private readonly ISandbox2Context sb2;
        private readonly TemplatedOfferGenerator gen;

        public GenerateHandler(ILogger<GenerateHandler> logger, IOfferData offerData, ISandbox2Context sb2, TemplatedOfferGenerator gen)
        {
            this.logger = logger;
            this.offerData = offerData;
            this.sb2 = sb2;
            this.gen = gen;
        }

        public int Execute(GenerateOptions opts)
        {
            // Validate the provided offer code
            if (string.IsNullOrWhiteSpace(opts.OfferCode))
            {
                logger.LogError("An offer code must be provided");
                return -1;
            }

            if (sb2.Offers.Any(o => o.OfferCode == opts.OfferCode))
            {
                logger.LogError("The provided offer code of {} is already in use", opts.OfferCode);
                return -1;
            }
            logger.LogDebug("Creating offer with code {}", opts.OfferCode);
            // TODO: trim offer code to <= 20 characters

            // Clean parameters
            logger.LogDebug("Checking provided options for parameters... ");
            var parameters = ReadParameters(opts);
            logger.LogDebug("Found {} parameters", parameters.Count);

            var template = offerData.Templates.FirstOrDefault(t => t.Id == opts.TemplateId);
            if (template == null)
            {
                logger.LogError("Unable to find an offer template with ID {}", opts.TemplateId);
                return -1;
            }
            else
            {
                logger.LogDebug("Using offer template {} - {}", template.Id, template.Description);
            }

            if (opts.Cost.HasValue)
            {
                logger.LogInformation("Overriding template's cost value of {} with {}", template.OfferCost, opts.Cost.HasValue);
                template.OfferCost = opts.Cost.Value;
            }

            if (opts.FullCost.HasValue)
            {
                logger.LogInformation("Overriding template's full cost value of {} with {}", template.OfferFullCost, opts.FullCost.HasValue);
                template.OfferFullCost = opts.FullCost.Value;
            }

            if (opts.MaxQuantity.HasValue)
            {
                logger.LogInformation("Overriding template's max quantity value of {} with {}", template.OfferMaxQuantity, opts.MaxQuantity.HasValue);
                template.OfferMaxQuantity = opts.MaxQuantity.Value;
            }

            var offer = gen.GenerateOfferFromTemplate(template, opts.OfferCode, parameters);

            // Check offer for any remaining placeholder values
            int placeholders = gen.CheckOfferForPlaceholder(offer);


            if (placeholders > 0 && !opts.IgnoreWarnings)
            {
                logger.LogError("Cancelling offer generation because warning were raised. No database changes have been made.");
                return -1;
            }

            var missingFields = 0;
            if (missingFields > 0 || placeholders > 0)
            {
                logger.LogWarning("Generating offer despite {} warning(s) because the ignore warnings flag was set");
            }

            if (!string.IsNullOrWhiteSpace(opts.Prerequisite))
            {
                offer.Prerequisite = opts.Prerequisite;
            }

            sb2.Offers.Add(offer);
            sb2.SaveChanges();
            return 0;
            
        }


        public Dictionary<string, string> ReadParameters(GenerateOptions opts)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (opts.Parameters != null && opts.Parameters.Count() > 0)
            {
                foreach (var pair in opts.Parameters)
                {
                    logger.LogTrace("Examining provided key/value pair {}", pair);
                    if (pair.Contains('='))
                    {
                        var split = pair.Split('=');
                        if (split.Count() == 2)
                        {
                            if (!split[0].Trim().Contains(" "))
                            {
                                parameters.Add(split[0].Trim(), split[1].Trim());
                            }
                            else
                            {
                                logger.LogInformation("Key '{}' is illegal and will be ignored. Parameter keys cannot have white space.", split[0].Trim());
                            }
                        }
                        else
                        {
                            logger.LogDebug("Splitting '{}' yielded an unexpected {} item(s), skipping", pair, split.Count());
                        }
                    }
                    else
                    {
                        logger.LogDebug("Provided key/value pair '{}' does not contain an '=' character, skipping it", pair);
                    }
                }
            }
            else
            {
                logger.LogTrace("No parameters have been provided");
            }

            return parameters;
        }

    }
}
