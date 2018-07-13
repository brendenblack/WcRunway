using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Cli
{
    [Verb("data", HelpText = "Fetch and cache data required for runway calculations")]
    public class DataOptions
    {

        [Option('l', "lastUpdated", Required = false)]
        public bool GetLastUpdated { get; set; }
    }
}
