using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.Token
{
    [Verb("token", HelpText = "Calculate token runway")]
    public class TokenOptions
    {
        [Option('u', "unit", Required = false, HelpText = "Calculates the token runway for the given unit ID")]
        public int? UnitId { get; set; }
    }
}
