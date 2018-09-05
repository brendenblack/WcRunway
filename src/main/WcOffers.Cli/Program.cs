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
using WcOffers.Cli.Features;
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
        static void Main(string[] args)
        {
            // Setup input parser
            var parser = new Parser(with => {
                with.EnableDashDash = true;
                with.CaseSensitive = true;
                });

            // Handle arguments
            int exitCode = Parser.Default.ParseArguments<TokenOptions, DataOptions, GenerateUniqueOptions, QualityOptions, TestOptions, ListTemplatesOptions, GenerateOptions>(args)
                .MapResult(
                    (CommandLineOptions o) =>
                    {
                        IConfiguration config = Startup.LoadConfiguration(o);
                        var serviceCollection = new ServiceCollection();
                        Startup.ConfigureServices(serviceCollection, config);
                        ServiceProvider container = serviceCollection.BuildServiceProvider();

                        return Execute(container, o);
                    },
                    //(TokenOptions o) => container.GetService<TokenRunway>().Execute(o),
                    //(GenerateUniqueOptions o) => container.GetService<GenerateUniqueHandler>().Execute(o),
                    //(QualityOptions o) => container.GetService<QualityHandler>().Execute(o),
                    //(TestOptions o) => container.GetService<TestHandler>().Execute(o),
                    //(GenerateOptions o) => container.GetService<GenerateHandler>().Execute(o),
                    //(ListTemplatesOptions o) => container.GetService<ListTemplatesHandler>().Execute(o),
                    (errs) => HandleParseError(errs));


            Environment.Exit(exitCode);
        }

        public static int Execute(ServiceProvider container, CommandLineOptions opts)
        {  

            // TODO: how to configure console logging?
            

            // Create logger
            var log = container.GetService<ILoggerFactory>().CreateLogger<Program>();

            try
            {
                switch (opts)
                {
                    case GenerateOptions generateOptions:
                        return container.GetService<GenerateHandler>().Execute(generateOptions);
                    case TestOptions testOptions:
                        return container.GetService<TestHandler>().Execute(testOptions);
                    default:
                        return -1;
                }
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

       

        public static int HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error.ToString());
            }

            return -1;
        }

        
    }
}
