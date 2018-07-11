using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WcRunway.Core.Domain;
using WcRunway.Core.Infrastructure;
using WcRunway.Core.Infrastructure.Data.Snowflake;
using WcRunway.Core.Sheets;

namespace WcRunway.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Loading unit data...");


            var sheets = new SheetsConnectorService();
            var uds = new SheetsUnitRepository(sheets);
            await uds.RefreshUnits();
            Console.WriteLine("Tracking {0} units", uds.Units.Count());
            var phalanx = uds.Units.First(u => u.Id == 251);

            Console.WriteLine("Connecting to Snowflake to retrieve owners of the Phalanx");

            var snowflake = new MockSnowflakeContext();
            var owners = snowflake.GetUnitOwnership(phalanx.Id);

            var monetized = snowflake.GetMonetizedUsers();

            var monetizedCount = owners.Keys.Where(o => monetized.ContainsKey(o)).Count();

            Console.WriteLine("There are {0} owners of the {1}, {2} of which are monetized", owners.Count, phalanx.Name, monetizedCount);

            Console.WriteLine("Calculating runway for {0}", phalanx.Name);
            var calc = new TokenRunwayCalculator();

            var runwayByUser = new Dictionary<int, int>();
            int maxLevel = phalanx.MaxLevel;

            foreach (var level in phalanx.Levels)
            {
                var ownersAtLevel = owners
                    .Where(o => o.Value == level.Number)
                    .Select(o => o.Key)
                    .ToList();

                int monetizedOwners = ownersAtLevel.Where(id => monetized.ContainsKey(id)).Count();

                var costs = calc.CalculateRemainingCostsAtLevel(phalanx, level.Number);
                costs.SkuCosts.TryGetValue("unit_upgrade_cor_phalanx", out int tokens);

                Console.WriteLine("Level {0} has {1} ({5}) owners with {2} tokens remaining to level {3} ({1} * {2} = {4})", 
                    level.Number, 
                    ownersAtLevel.Count,
                    tokens,
                    maxLevel,
                    ownersAtLevel.Count * tokens,
                    monetizedOwners);

                foreach (var owner in ownersAtLevel)
                {
                    runwayByUser.Add(owner, tokens);
                }
            }

            var total = runwayByUser.Values.Sum();
            var totalMonetized = runwayByUser.Where(r => monetized.Keys.Contains(r.Key)).Select(r => r.Value).Sum();
            Console.WriteLine("A total of {0} ({1}) tokens are remaining to be sold for the {2}", total, totalMonetized, phalanx.Name);
            
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
