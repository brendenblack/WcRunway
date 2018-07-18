using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WcRunway.Cli.Verbs;
using WcRunway.Core.Domain.Game;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets;
using WcRunway.Core.Infrastructure.Data.Snowflake;

namespace WcRunway.Cli
{
    class Program
    {
        private static ServiceProvider container;

        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            container = serviceCollection.BuildServiceProvider();

            // configure console logging
            container.GetRequiredService<ILoggerFactory>()
                .AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            NLog.LogManager.LoadConfiguration("nlog.config");
            
            var log = container.GetService<ILoggerFactory>().CreateLogger<Program>();
            log.LogTrace("Triggering warmup");
            await container.GetService<Warmup>().Run();
            log.LogTrace("Warmup complete");

            Parser.Default.ParseArguments<TokenOptions, DataOptions>(args)
               .MapResult(
                    (TokenOptions o) => container.GetService<TokenRunway>().Execute(o),
                    (DataOptions o) => ExecuteData(o),
                    (errs) => HandleParseError(errs));

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            NLog.LogManager.Shutdown();




            //Console.WriteLine("Loading unit data...");
            //var sheets = new SheetsConnectorService();
            //await uds.RefreshUnits();
            //Console.WriteLine("Tracking {0} units", uds.Units.Count());

            //// select the phalanx as the test case
            //var phalanx = uds.Units.First(u => u.Id == 251);


            //Console.WriteLine("Connecting to Snowflake to retrieve owners of the Phalanx");
            //var snowflake = new MockSnowflakeContext();
            //var owners = snowflake.GetUnitOwnership(phalanx.Id);

            //var monetized = snowflake.Spenders();

            //var monetizedCount = owners.Keys.Where(o => monetized.ContainsKey(o)).Count();

            //Console.WriteLine("There are {0} owners of the {1}, {2} of which are monetized", owners.Count, phalanx.Name, monetizedCount);

            //Console.WriteLine("Calculating runway for {0}", phalanx.Name);
            //var calc = new TokenRunwayCalculator();

            //var runwayByUser = new Dictionary<int, int>();
            //int maxLevel = phalanx.MaxLevel;

            //foreach (var level in phalanx.Levels)
            //{
            //    var ownersAtLevel = owners
            //        .Where(o => o.Value == level.Number)
            //        .Select(o => o.Key)
            //        .ToList();

            //    int monetizedOwners = ownersAtLevel.Where(id => monetized.ContainsKey(id)).Count();

            //    var costs = calc.CalculateRemainingCostsAtLevel(phalanx, level.Number);
            //    costs.SkuCosts.TryGetValue("unit_upgrade_cor_phalanx", out int tokens);

            //    Console.WriteLine("Level {0} has {1} ({5}) owners with {2} tokens remaining to level {3} ({1} * {2} = {4})", 
            //        level.Number, 
            //        ownersAtLevel.Count,
            //        tokens,
            //        maxLevel,
            //        ownersAtLevel.Count * tokens,
            //        monetizedOwners);

            //    foreach (var owner in ownersAtLevel)
            //    {
            //        runwayByUser.Add(owner, tokens);
            //    }
            //}

            //var total = runwayByUser.Values.Sum();
            //var totalMonetized = runwayByUser.Where(r => monetized.Keys.Contains(r.Key)).Select(r => r.Value).Sum();
            //Console.WriteLine("A total of {0} ({1}) tokens are remaining to be sold for the {2}", total, totalMonetized, phalanx.Name);

        }


        public static int ExecuteToken(TokenOptions opts)
        {
            
            Console.WriteLine("Running token runway for {0}", opts.UnitId);
            Console.ReadKey();
            return 0;
        }

        public static int ExecuteData(DataOptions opts)
        {
            return 0;
        }

        public static int HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error.ToString());
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            return -1;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            services.AddSingleton<SheetsConnectorService>();
            services.AddTransient<Warmup>();
            services.AddTransient<ISnowflakeContext, MockSnowflakeContext>();
            services.AddSingleton<IUnitData, SheetsUnitData>();
            services.AddTransient<IGameContext, GameContext>();

            services.AddTransient<TokenRunway>();
        }
    }


}
