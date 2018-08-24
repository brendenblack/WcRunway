using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcData.GameContext;
using WcData.Sheets;
using WcData.Sheets.Models;

namespace WcOffers.Cli.Features.ListTemplates
{
    public class ListTemplatesHandler
    {
        private readonly ILogger<ListTemplatesHandler> logger;
        private readonly IOfferData offerData;
        private readonly ISandbox2Context sb2;

        public ListTemplatesHandler(ILogger<ListTemplatesHandler> logger, IOfferData offerData, ISandbox2Context sb2)
        {
            this.logger = logger;
            this.offerData = offerData;
            this.sb2 = sb2;
        }


        public int Execute(ListTemplatesOptions opts)
        {
            List<OfferTemplate> templates = new List<OfferTemplate>();

            if (!string.IsNullOrWhiteSpace(opts.Tag))
            {
                templates = offerData.Templates
                    .Where(t => t.Tags.Contains(opts.Tag))
                    .ToList();

                logger.LogInformation("Showing {} offer templates with tag '{}'", templates.Count, opts.Tag);
            }
            else
            {
                templates = offerData.Templates;
                logger.LogInformation("Showing {} offer templates", templates.Count);
            }

            foreach (var template in templates)
            {
                logger.LogInformation("{} - {} (tags: {})", template.Id, template.Description, String.Join(", ", template.Tags));
            }


            return 0;
        }
    }
}
