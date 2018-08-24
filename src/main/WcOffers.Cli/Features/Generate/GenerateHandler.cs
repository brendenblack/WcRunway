using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcData.GameContext;
using WcData.Sheets;

namespace WcOffers.Cli.Features.Generate
{
    public class GenerateHandler
    {
        private readonly ILogger<GenerateHandler> logger;
        private readonly IOfferData offerData;
        private readonly ISandbox2Context sb2;
        private readonly TemplateOfferGenerator gen;

        public GenerateHandler(ILogger<GenerateHandler> logger, IOfferData offerData, ISandbox2Context sb2, TemplateOfferGenerator gen)
        {
            this.logger = logger;
            this.offerData = offerData;
            this.sb2 = sb2;
            this.gen = gen;
        }

        public int Execute(GenerateOptions opts)
        {

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

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (opts.Parameters != null && opts.Parameters.Count > 0)
            {
                foreach (var pair in opts.Parameters)
                {
                    if (pair.Contains('='))
                    {
                        var split = pair.Split('=');
                        parameters.Add(split[0], split[1]);
                    }
                }
            }

            var template = offerData.Templates.FirstOrDefault(t => t.Id == opts.TemplateId);
            if (template == null)
            {
                logger.LogError("Unable to find an offer template with ID {}", opts.TemplateId);
                return -1;
            }

            var offer = gen.GenerateOfferFromTemplate(template, opts.OfferCode, parameters);
            sb2.Offers.Add(offer);
            sb2.SaveChanges();
            return 0;
            
        }
    }
}
