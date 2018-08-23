using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WcData.GameContext;
using WcData.Microsoft.Extensions.DependencyInjection;
using WcOffers.Cli.Features.Data;
using WcOffers.Cli.Features.GenerateUnique;
using WcOffers.Cli.Features.Quality;
using WcOffers.Cli.Features.Test;
using WcOffers.Cli.Features.Token;

namespace WcOffers.Cli
{
    class Program
    {
        private static ServiceProvider container;

        static async Task Main(string[] args)
        {
            var exitCode = 0;

            // load configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.dev.json", true)
                .AddEnvironmentVariables();

            IConfiguration config = builder.Build();
            

            // Build the container
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, config);
            container = serviceCollection.BuildServiceProvider();

            // Configure console logging
            container.GetRequiredService<ILoggerFactory>()
                .AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            NLog.LogManager.LoadConfiguration("nlog.config");
           
            // Warm up the data source
            var log = container.GetService<ILoggerFactory>().CreateLogger<Program>();
            //log.LogTrace("Triggering warmup");
            //await container.GetService<Warmup>().Run();
            //log.LogTrace("Warmup complete");

            // Handle input
            try
            {
                var parser = new Parser(with => {
                    with.EnableDashDash = true;
                    with.CaseSensitive = true;
                    });

                Parser.Default.ParseArguments<TokenOptions, DataOptions, GenerateUniqueOptions, QualityOptions, TestOptions>(args)
                   .MapResult(
                        (TokenOptions o) => container.GetService<TokenRunway>().Execute(o),
                        (DataOptions o) => ExecuteData(o),
                        (GenerateUniqueOptions o) => container.GetService<GenerateUniqueHandler>().Execute(o),
                        (QualityOptions o) => container.GetService<QualityHandler>().Execute(o),
                        (TestOptions o) => container.GetService<TestHandler>().Execute(o),
                        (errs) => HandleParseError(errs));
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                exitCode = 1;
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            NLog.LogManager.Shutdown();

            Environment.Exit(exitCode);
            

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

        private static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            // Add SB2 database connection
            services.AddSandbox2(opt =>
            {
                opt.Url = config["data:sandbox2:url"];
                opt.Name = config["data:sandbox2:name"];
                opt.Username = config["data:sandbox2:username"];
                opt.Password = config["data:sandbox2:password"];
            });

            // Add connection to game data stored in Google Sheets
            services.AddSheets(opts => { opts.ClientSecretPath = "client_secret.json"; });

            services.AddSnowflake(opt =>
            {
                opt.Account = config["data:snowflake:account"];
                opt.Username = config["data:snowflake:username"];
                opt.Password = config["data:snowflake:password"];
                opt.Database = config["data:snowflake:database"] ?? "";
                opt.Schema = config["data:snowflake:schema"] ?? "";
            });

            services.AddTransient(s => new OfferJiraTicketManager(config["data:jira:url"], config["data:jira:username"], config["data:jira:password"]));

            //services.AddTransient<SnowflakeConnectionDetails>(o => new SnowflakeConnectionDetails(sfAccount, sfUsername, sfPassword, sfDatabase, sfSchema));

            //services.AddSingleton(EmbeddedJsonServiceCredential.CreateCredentialFromFile()); 
            //services.AddSingleton<SheetsConnectorService>();
            //services.AddTransient<Warmup>();
            //services.AddTransient<IUnitOwnership, SnowflakeContext>();
            //services.AddTransient<ISnowflakeContext, MockSnowflakeContext>();// TODO: refactor this out
            //services.AddSingleton<IUnitData, SheetsUnitData>();
            //services.AddTransient<IGameContext, GameContext>();
            //services.AddTransient<IOfferCopyBible, SheetsOfferCopyBible>();
            //services.AddTransient<UniqueOfferGenerator>();
            //services.AddTransient<IOfferData, SheetsOfferData>();

            services.AddTransient<TokenRunway>();
            services.AddTransient<GenerateUniqueHandler>();
            services.AddTransient<QualityHandler>();
            services.AddTransient<TestHandler>();
        }
    }


}
