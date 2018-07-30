using Microsoft.Extensions.Logging;
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
            var prefix = ValidatePrefix(opts.OfferCodePrefix);
            log.LogInformation("Launching Generate Offer handler for unit id {0} with offer code prefix {1}", opts.UnitId, prefix);

            DirectoryInfo outputDir = CreateOutputDirectory(opts.OutputDirectoryPath, prefix);
            log.LogInformation($"Setting output directory to {outputDir.FullName}");

            var unit = this.gameContext.Units.FirstOrDefault(u => u.Id == opts.UnitId);
            if (unit == null)
            {
                throw new ArgumentException($"A unit with id {opts.UnitId} was not found");
            }
            log.LogDebug($"Unit: {unit.ToString()}");

            // generate unlock
            var unlock = gen.CreateUnlockOffer(unit, prefix);
            this.sb2.Offers.Add(unlock);
            this.sb2.SaveChanges();
            log.LogInformation("Generated unlock offer has been created with id {0}", unlock.Id);

            log.LogTrace("Pulling non-owner cohort for offer {0} {1}", unlock.Id, unlock.OfferCode);
            var unlockCohort = this.unitOwnership.FetchUnitNonOwnerUserIds(unit.Id);
            log.LogDebug("Found {0} non-owner(s)", unlockCohort.Count);
            WriteCohort(Path.Combine(outputDir.FullName, unlock.OfferCode + ".csv"), unlockCohort);

            if (opts.IncludeLevels)
            {
                // generate levels
            }

            if (opts.IncludeTech)
            {
                // generate tech
            }

            if (opts.IncludeEliteParts)
            {
                // elite parts
            }

            if (opts.IncludeOmegaParts)
            {
                // omega parts
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

        public void WriteCohort(string filename, List<int> cohort)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (int user in cohort)
                {
                    writer.WriteLine(user);
                }
            }
        }

        public DirectoryInfo CreateOutputDirectory(string outputDirectoryPath, string prefix)
        {
            // TODO: taking in both arguments like this is gross when they are mutually exclusive options
            return (String.IsNullOrWhiteSpace(outputDirectoryPath))
                ? Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, prefix))
                : Directory.CreateDirectory(outputDirectoryPath);
        }

    }
}
