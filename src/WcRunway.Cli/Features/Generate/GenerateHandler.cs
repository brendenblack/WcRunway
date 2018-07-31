﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Game;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Domain.Users;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;

namespace WcRunway.Cli.Features.Generate
{
    public class GenerateHandler
    {
        private readonly ILogger<GenerateHandler> log;
        private readonly IGameContext gameContext;
        private readonly IUnitOwnership unitOwnership;
        private readonly UniqueOfferGenerator gen;
        private readonly Sandbox2Context sb2;
        
        public GenerateHandler(ILogger<GenerateHandler> log, IGameContext gameContext, IUnitOwnership unitOwnership, UniqueOfferGenerator gen, Sandbox2Context sb2)
        {
            this.log = log;
            this.gameContext = gameContext;
            this.unitOwnership = unitOwnership;
            this.gen = gen;
            this.sb2 = sb2;
        }

        public int Execute(GenerateOptions opts)
        {
            // Validate the prefix
            var prefix = ValidatePrefix(opts.OfferCodePrefix); // TODO: refactor this logic out and in to the Core project
            log.LogInformation("Launching Generate Offer handler for unit id {0} with offer code prefix {1}", opts.UnitId, prefix);

            // Retrieve the specified unit
            var unit = this.gameContext.Units.FirstOrDefault(u => u.Id == opts.UnitId);
            if (unit == null)
            {
                log.LogError($"A unit with id {opts.UnitId} was not found");
            }
            log.LogDebug($"Unit: {unit.ToString()}");

            List<Offer> generatedOffers = new List<Offer>();

            if (opts.IncludeAllOffers || opts.IncludeUnlock)
            {
                // Generate unlock offer
                var unlock = gen.CreateUnlockOffer(unit, prefix);
                generatedOffers.Add(unlock);
            }

            if (opts.IncludeAllOffers || opts.IncludeLevels)
            {
                // Generate level run offers
                var levels = gen.CreateLevelOffers(unit, prefix);
                generatedOffers.AddRange(levels);
            }

            if (opts.IncludeAllOffers || opts.IncludeTech)
            {
                // Generate tech offer
                var techs = gen.CreateTechOffers(unit, prefix);
                generatedOffers.AddRange(techs);
            }

            if (opts.IncludeAllOffers || opts.IncludeEliteParts)
            {
                // Generate elite parts offer
                var elite = gen.CreateElitePartsOffer(unit, prefix);
                generatedOffers.Add(elite);
            }

            if (opts.IncludeAllOffers || opts.IncludeOmegaParts)
            {
                // Generate omega parts
                var omega = gen.CreateOmegaPartsOffer(unit, prefix);
                generatedOffers.Add(omega);
            }


            try
            {
                this.sb2.Offers.AddRange(generatedOffers);
                this.sb2.SaveChanges();
                foreach (var offer in generatedOffers)
                {
                    log.LogInformation("Offer {0} has been generated with id {1}", offer.OfferCode, offer.Id);
                }
            }
            catch (Exception e)
            {
                // TODO
                log.LogError("");
                return -1;
            }


            return 0;
        }

        // TODO: validate characters as acceptable OS filenames
        /// <summary>
        /// Ensures the prefix is valid. To be considered viable, the provided prefix must not be null or whitespace, and not already in use.
        /// If the specified prefix is longer than 16 characters, it will be truncated to allow for offer type suffixes to be added.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string ValidatePrefix(string prefix)
        {
            if (String.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Invalid offer code prefix provided");
            }

            prefix = (prefix.Length > 16) ? prefix.Substring(0, 16) : prefix;

            if (this.sb2.Offers.Any(o => o.OfferCode.StartsWith(prefix)))
            {
                throw new InvalidOperationException($"Unable to create offers because the prefix {prefix} already exist");
            }

            return prefix;
        }
    }
}
