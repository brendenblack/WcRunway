using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.Infrastructure;
using WcGraph.Models;

namespace WcGraph.Data
{
    public class PveBattleRepository
    {
        public PveBattleRepository(IDriver driver)
        {
            this.driver = driver;
            // driver = GraphDatabase.Driver("bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "password"));
        }

        private readonly IDriver driver;

        public void AddBattle(PveBattle battle)
        {
            string cypher = new StringBuilder()
                    .AppendLine("UNWIND {units} as unit")
                    .AppendLine("MERGE (u:Unit { id: unit.id })")
                    .AppendLine("SET u = unit")
                    .ToString();

            string cypher2 = new StringBuilder()
                .AppendLine("MERGE (a:Attack { id: battle. ")
                .ToString();


            var dictionary = new Dictionary<string, object>() { { "battle", ParameterSerializer.ToDictionary(battle) } };

            


            using (var session = driver.Session())
            {
                
            }
        }
    }
}
