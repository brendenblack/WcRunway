using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.GenerateUnique
{
    [Verb("generate-unique", HelpText = "Generate standard offers for Unique units")]
    public class GenerateUniqueOptions : CommandLineOptions
    {
        [Value(0)]
        public int UnitId { get; set; }

        [Option('c', "code", Required = true, HelpText = "The prefix for the generated offers, will be trimmed to a maximum of 15 characters")]
        public string OfferCodePrefix { get; set; }

        [Option('u', "unlock", Required = false, Default = false, HelpText = "Generate an unlock offer for the indicated unit")]
        public bool IncludeUnlock { get; set; }

        [Option('t', "tech", Required = false, Default = false, HelpText = "Generate an offer comprised of tech for the indicated unit. If multiple tech offers exist for the given unit, they will all be created")]
        public bool IncludeTech { get; set; }

        [Option('l', "levels", Required = false, Default = false, HelpText = "Generate level bucket offers for the indicated unit")]
        public bool IncludeLevels { get; set; }

        [Option('L', "level-step", Required = false, Default = false, HelpText = "Generate offers for individual levels for the indicated unit. Mutually exclusive with -l/--level")]
        public bool IncludeLevelSteps { get; set; }

        [Option('o', "omega", Required = false, Default = false, HelpText = "Generate an offer of Omega parts for the indicated unit")]
        public bool IncludeOmegaParts { get; set; }

        [Option('e', "elite", Required = false, Default = false, HelpText = "Generate an offer of Elite parts for the indicated unit")]
        public bool IncludeEliteParts { get; set; }

        [Option('a', "all", Required = false, Default = false, HelpText = "Generate all offers for the specified unit. Overrides all other options")]
        public bool IncludeAllOffers { get; set; }

    }
}
