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

        [Option('V', "extra-verbose", Required = false, Default = false, HelpText = "Display a much more verbose output during runtime")]
        public bool IsExtraVerbose { get; set; }

        [Option('f', "config-file", Required = false, HelpText = "Optional. Specifies a configuration file to use, otherwise will default to config.ini located in the application's installation directory")]
        public string ConfigurationFile { get; set; }
    }
}
