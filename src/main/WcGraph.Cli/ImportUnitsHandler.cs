using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcData.Sheets;
using WcGraph.Infrastructure;

namespace WcGraph.Cli
{
    public class ImportUnitsHandler
    {
        private readonly IGameData gameData;

        public ImportUnitsHandler(IGameData gameData)
        {
            this.gameData = gameData;
        }

        public int Execute(ImportUnitsOptions opts)
        {
            // warm up the units list before creating a connection to Neo4j

            var units = gameData.Units
                .Select(u => new Models.Unit
                {
                    Id = u.Id,
                    Identifier = u.Identifier,
                    Name = u.Name,
                    Key = u.Key
                })
                .ToList();

            //var units = new List<Models.Unit>
            //{
            //    new Models.Unit
            //    {
            //        Id = 1,
            //        Identifier = "identifier",
            //        Name = "the unit",
            //        Key = "the_unit"
            //    }
            //};

            string cypher = new StringBuilder()
                    .AppendLine("UNWIND {units} as unit")
                    .AppendLine("MERGE (u:Unit { id: unit.id })")
                    .AppendLine("SET u = unit")
                    .ToString();



            IDriver driver = GraphDatabase.Driver("bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "password"));
            using (var session  = driver.Session())
            {
                session.Run(cypher, new Dictionary<string, object>() { { "units", ParameterSerializer.ToDictionary(units) } });
            }

            return 0;
        }

    }
}
