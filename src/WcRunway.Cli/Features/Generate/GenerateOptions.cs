using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Cli.Features.Generate
{
    [Verb("generate", HelpText = "Generate offers and their related components")]
    public class GenerateOptions
    {

        //[Option('U', "unit", Required = true, HelpText = "Generates the indicated offers based on the unit")]
        [Value(0)]
        public int UnitId { get; set; }

        [Option('c', "code", Required = true, HelpText = "The prefix for the generated offers, will be trimmed to a maximum of 16 characters")]
        public string OfferCodePrefix { get; set; }

        [Option('u', "unlock", Required = false, Default = false, HelpText = "Generate an unlock offer for the indicated unit")]
        public bool IncludeUnlock { get; set; }

        [Option('t', "tech", Required = false, Default = false, HelpText = "Generate an offer comprised of tech for the indicated unit")]
        public bool IncludeTech { get; set; }

        [Option('l', "levels", Required = false, Default = false, HelpText = "Generate offers for level buckets for the indicated unit")]
        public bool IncludeLevels { get; set; }
        
        [Option('o', "omega", Required = false, Default = false, HelpText = "Generate an offer of Omega parts for the indicated unit")]
        public bool IncludeOmegaParts { get; set; }

        [Option('e', "elite", Required = false, Default = true, HelpText = "Generate an offer of Elite parts for the indicated unit")]
        public bool IncludeEliteParts { get; set; }

        [Option("all", Required = false, Default = false, HelpText = "Generate all offers for the specified unit")]
        public bool IncludeAllOffers { get; set; }

    }
}
