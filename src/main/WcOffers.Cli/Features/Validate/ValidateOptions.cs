using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.Validate
{
    [Verb("validate", HelpText = "Validates the configuration settings for the application")]
    public class ValidateOptions : CommandLineOptions
    {
    }
}
