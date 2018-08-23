using Neo4j.Driver.V1;
using Neo4jClient;
using System;
using System.Threading.Tasks;

namespace WcEconomy.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {



            //Console.WriteLine("Hello World!");

            var uri = "bolt://localhost:7687/db/data";
            var user = "neo4j";
            var password = "password";


            using (IDriver driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password)))
            {
                string[] queries = {
                    "CREATE INDEX ON :Movie(title)",
                    "CREATE INDEX ON :Movie(id)",
                    "CREATE INDEX ON :Person(id)",
                    "CREATE INDEX ON :Person(name)",
                    "CREATE INDEX ON :Genre(name)"
                };

                using (var session = driver.Session())
                {
                    foreach (var query in queries)
                    {
                        await session.RunAsync(query);
                    }
                }
            }
        }
    }
}
