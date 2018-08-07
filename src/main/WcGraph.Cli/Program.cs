using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WcData.GameContext;
using WcData.Microsoft.Extensions.DependencyInjection;

namespace WcGraph.Cli
{
    public class Program
    {
        static void Main(string[] args)
        {

            // Build the container
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var container = serviceCollection.BuildServiceProvider();

            try
            {


                Parser.Default.ParseArguments<ImportUnitsOptions>(args)
                       .MapResult(
                            (ImportUnitsOptions o) => container.GetService<ImportUnitsHandler>().Execute(o),
                            (errs) => HandleParseError(errs));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Task.Run(async () => { await callWebApi(); }).GetAwaiter().GetResult();
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

            // Add SB2 database connection
            //services.AddGameContext(opt =>
            //{
            //    opt.Environment = GameContexts.SANDBOX2;
            //    opt.Url = config["data:sandbox2:url"];
            //    opt.Name = config["data:sandbox2:name"];
            //    opt.Username = config["data:sandbox2:username"];
            //    opt.Password = config["data:sandbox2:password"];
            //});

            // Add connection to game data stored in Google Sheets
            services.AddSheets(opts => { opts.ClientSecretPath = Path.GetFullPath("client_secret.json"); });


            services.AddTransient<ImportUnitsHandler>();
            //services.AddSnowflake(opt =>
            //{
            //    opt.Account = config["data:snowflake:account"];
            //    opt.Username = config["data:snowflake:username"];
            //    opt.Password = config["data:snowflake:password"];
            //    opt.Database = config["data:snowflake:database"] ?? "";
            //    opt.Schema = config["data:snowflake:schema"] ?? "";
            //});

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


        }
    }
}
