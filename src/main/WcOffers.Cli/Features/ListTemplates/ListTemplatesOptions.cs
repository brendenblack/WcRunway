using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.ListTemplates
{
    [Verb("list-templates", HelpText = "Shows a listing of offer templates and their IDs")]
    public class ListTemplatesOptions : CommandLineOptions
    {
        [Option('t', "tag", Required = false, HelpText = "Look up offer templates that have the specified tag (case insensitive)")]
        public string Tag { get; set; }
    }
}
