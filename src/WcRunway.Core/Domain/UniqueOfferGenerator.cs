using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;

namespace WcRunway.Core.Domain
{
    public class UniqueOfferGenerator
    {
        private readonly ILogger<UniqueOfferGenerator> log;

        public UniqueOfferGenerator(ILogger<UniqueOfferGenerator> log)
        {
            this.log = log;
        }


        public Offer CreateUnlockOffer(Unit unit, string prefix)
        {
            var code = $"{prefix}Unl";
            log.LogInformation("Creating unlock offer for {0} {1} using offer code {2}", unit.Id, unit.Name, code);

            var offer = new Offer
            {
                OfferCode = code,
                Title = ""
            };
        }
    }
}
