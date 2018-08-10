using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Cli.Features.Import
{
    [Verb("import")]
    public class ImportOptions
    {
        [Option('u', Required = false, Default = false)]
        public bool ShouldImportUnits { get; set; }

        [Option('l', Required = false, Default = false)]
        public bool ShouldImportUnitLevels { get; set; }

        [Option('p', Required = false, Default = false)]
        public bool ShouldImportUsers { get; set; }

        [Option('b', Required = false, Default = false)]
        public bool ShouldImportBuildings { get; set; }
        
        [Option('o', Required = false, Default = false)]
        public bool ShouldImportUnitOwnership { get; set; }

        [Option('a', Required = false, Default = false)]
        public bool ShouldImportPveAttacks { get; set; }
    }
}
