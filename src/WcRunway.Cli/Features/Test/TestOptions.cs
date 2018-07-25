using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Cli.Features.Test
{
    [Verb("test", HelpText = "Perform arbitrary development tests")]
    public class TestOptions
    {
        [Option("sandbox2", Required = false, Default = false)]
        public bool TestSandbox2 { get; set; }

        [Option("copybible", Required = false, Default = false)]
        public bool TestOfferBible { get; set; }
    }
}
