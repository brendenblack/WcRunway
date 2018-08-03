using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.Cohort
{
    [Verb("cohort")]
    public class CohortOptions
    {
        [Option('d', "output", Required = false, HelpText = "Specify the output directory for cohort CSVs, by default this will be the current directory")]
        public string OutputDirectoryPath { get; set; }

        [Value(0)]
        public List<string> OfferCodes { get; set; }
    }
}
