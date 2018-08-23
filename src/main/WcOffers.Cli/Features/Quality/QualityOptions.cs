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
    }
}
