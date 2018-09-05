using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.Generate
{
    [Verb("generate", HelpText = "Generate offers based on a template. Template IDs can be discovered by executing the list-templates command")]
    public class GenerateOptions : CommandLineOptions
    {
        [Value(0, Required = true, HelpText = "The ID of the template to generate. IDs can be discovered by executing the list-templates command")]
        public int TemplateId { get; set; }

        [Option('o', "offer-code", Required = true, HelpText = "The offer code to assign to the generated offer. Maximum of 20 characters")]
        public string OfferCode { get; set; }

        [Option('p', "prereq", Required = false, HelpText = "The code of an offer to use as a prerequisite")]
        public string Prerequisite { get; set; }

        [Option('D', "params", Required = false, HelpText = "A list of key/value pairs (in the form of key1=value1 key2=\"my value 2\") that will be substitutded in the template")]
        public IEnumerable<string> Parameters { get; set; }

        [Option('c', "cost", Required = false, HelpText = "Allows overriding of the templated offer's cost")]
        public int? Cost { get; set; }

        [Option('C', "full-cost", Required = false, HelpText = "Allows overriding of the templated offer's full cost")]
        public int? FullCost { get; set; }

        [Option('q', "quantity", Required = false, HelpText = "Allows overriding of the templated offer's max quantity")]
        public int? MaxQuantity { get; set; }

        [Option('i', "ignore-warn", Required = false, Default = false, HelpText = "Any warnings generated will cause the process to fail to prevent writing invalid data to the database. Specify this option to ignore any warnings and generate an offer regardless")]
        public bool IgnoreWarnings { get; set; }
    }
}
