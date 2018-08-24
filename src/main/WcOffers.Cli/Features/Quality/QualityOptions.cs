using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.Quality
{
    [Verb("qa", HelpText = "Adds a generated offer to the QA pipeline")]
    public class QualityOptions
    {
        [Value(0)]
        public IEnumerable<string> OfferCodes { get; set; }

        [Option('a', "assign", Required = false, HelpText = "If specified, will attempt to assign the created JIRA issue(s) to the provided user")]
        public string Assignee { get; set; }
    }
}
