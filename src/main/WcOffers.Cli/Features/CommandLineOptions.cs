using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features
{
    public abstract class CommandLineOptions
    {
        [Option('v', "verbose", Required = false, Default = false, HelpText = "Display more verbose output during runtime")]
        public bool IsVerbose { get; set; }

        [Option('f', "config-file", Required = false, Default = "", HelpText = "")]
        public string ConfigurationFile { get; set; }
    }
}
