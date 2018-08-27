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
using WcOffers.Cli.Features.Generate;
using WcOffers.Cli.Features.GenerateUnique;
using WcOffers.Cli.Features.ListTemplates;
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
            new Startup().ConfigureServices(serviceCollection, config);
            container = serviceCollection.BuildServiceProvider();

            // Configure console logging
            container.GetRequiredService<ILoggerFactory>()
                .AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            NLog.LogManager.LoadConfiguration("nlog.config");
           
            // Create logger
            var log = container.GetService<ILoggerFactory>().CreateLogger<Program>();
          
            // Handle input
            try
            {
                var parser = new Parser(with => {
                    with.EnableDashDash = true;
                    with.CaseSensitive = true;
                    });

                Parser.Default.ParseArguments<TokenOptions, DataOptions, GenerateUniqueOptions, QualityOptions, TestOptions, ListTemplatesOptions, GenerateOptions>(args)
                   .MapResult(
                        (TokenOptions o) => container.GetService<TokenRunway>().Execute(o),
                        (GenerateUniqueOptions o) => container.GetService<GenerateUniqueHandler>().Execute(o),
                        (QualityOptions o) => container.GetService<QualityHandler>().Execute(o),
                        (TestOptions o) => container.GetService<TestHandler>().Execute(o),
                        (GenerateOptions o) => container.GetService<GenerateHandler>().Execute(o),
                        (ListTemplatesOptions o) => container.GetService<ListTemplatesHandler>().Execute(o),
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
        }


        public static int ExecuteToken(TokenOptions opts)
        {
            Console.WriteLine("Running token runway for {0}", opts.UnitId);
            Console.ReadKey();
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

        
    }


}
