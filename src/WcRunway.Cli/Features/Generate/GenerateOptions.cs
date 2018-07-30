using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Cli.Features.Generate
{
    [Verb("token", HelpText = "Generate offers and their related components")]
    public class GenerateOptions
    {
        [Option('u', "unit", Required = true, HelpText = "Generates the indicated offers based on the unit")]
        public int UnitId { get; set; }

        [Option('c', "code", Required = true, HelpText = "The prefix for the generated offers, will be trimmed to a maximum of 17 characters")]
        public string OfferCodePrefix { get; set; }

        [Option('t', "tech", Required = false, Default = false, HelpText = "Generate an offer comprised of tech for the indicated unit")]
        public bool IncludeTech { get; set; }

        [Option('l', "levels", Required = false, Default = false, HelpText = "Generate offers for level buckets for the indicated unit")]
        public bool IncludeLevels { get; set; }
        
        [Option('o', "omega", Default = true, HelpText = "Generate an offer for Omega parts for the indicated unit")]
        public bool IncludeOmegaParts { get; set; }

        [Option('e', "elite", Default = true, HelpText = "Generate an offer for Elite parts for the indicated unit")]
        public bool IncludeEliteParts { get; set; }

        [Option('d', "output", Required = false, HelpText = "Specify the output directory for cohort CSVs, by default this will be the current directory")]
        public string OutputDirectoryPath { get; set; }

    }
}
