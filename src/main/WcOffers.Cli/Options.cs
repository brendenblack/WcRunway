using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli
{
    class Options
    {
        [Option('t', "token", Required = false, HelpText = "Calculates the token runway for the given unit ID")]
        public int UnitId { get; set; }
    }
}
