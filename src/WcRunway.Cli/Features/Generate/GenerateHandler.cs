using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Game;

namespace WcRunway.Cli.Features.Generate
{
    public class GenerateHandler
    {
        private readonly ILogger<GenerateHandler> log;
        private readonly IGameContext gameContext;
        private readonly UniqueOfferGenerator gen;

        public GenerateHandler(ILogger<GenerateHandler> log, IGameContext gameContext, UniqueOfferGenerator gen)
        {
            this.log = log;
            this.gameContext = gameContext;
            this.gen = gen;
        }

        public int Execute(GenerateOptions o)
        {
            if (String.IsNullOrWhiteSpace(o.OfferCodePrefix))
            {
                log.LogError("Invalid offer code prefix provided");
                throw new ArgumentException("Invalid offer code prefix provided");
            }

            var prefix = (o.OfferCodePrefix.Length > 17) ? o.OfferCodePrefix.Substring(0, 17) : o.OfferCodePrefix;
            log.LogInformation("Launching Generate Offer handler for unit id {0} with offer code prefix {1}", o.UnitId, prefix);

            var unit = this.gameContext.Units.FirstOrDefault(u => u.Id == o.UnitId);

            if (unit == null)
            {
                log.LogError("Unable to find a unit with id {0}", o.UnitId);
                throw new ArgumentException($"A unit with id {o.UnitId} was not found");
            }

            // generate unlock


            if (o.IncludeLevels)
            {
                // generate levels
            }

            if (o.IncludeTech)
            {
                // generate tech
            }

            if (o.IncludeEliteParts)
            {
                // elite parts
            }

            if (o.IncludeOmegaParts)
            {
                // omega parts
            }



            return 0;
        }
    }
}
