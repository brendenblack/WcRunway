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
using WcOffers.Cli.Features.Token;
using WcOffers.Cli.Features.Validate;

namespace WcOffers.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup argument parser
            var parser = new Parser(with => {
                with.EnableDashDash = true;
                with.CaseSensitive = true;
                });

            // Handle arguments, treating the Validation verb differently than the rest to bypass starting the container
            int exitCode = Parser.Default.ParseArguments<ValidateOptions, CommandLineOptions>(args)
                .MapResult(
                    (ValidateOptions o) => new ValidateHandler().Execute(o), 
                    (CommandLineOptions o) =>
                    {
                        IConfiguration config = Startup.LoadConfiguration(o);
                        var serviceCollection = new ServiceCollection();
                        Startup.ConfigureServices(serviceCollection, config);
                        ServiceProvider container = serviceCollection.BuildServiceProvider();

                        return Execute(container, o);
                    },
                    (errs) => HandleParseError(errs));


            var parsed = Parser.Default.ParseArguments<ValidateOptions, CommandLineOptions>(args);

            Environment.Exit(exitCode);
        }

        public static int Execute(ServiceProvider container, CommandLineOptions opts)
        {  
            // Create logger
            var log = container.GetService<ILoggerFactory>().CreateLogger<Program>();

            // TODO handlers
            // (TokenOptions o) => container.GetService<TokenRunway>().Execute(o),
            // (GenerateUniqueOptions o) => container.GetService<GenerateUniqueHandler>().Execute(o),
            // (ListTemplatesOptions o) => container.GetService<ListTemplatesHandler>().Execute(o),

            try
            {
                switch (opts)
                {
                    case GenerateOptions generateOptions:
                        return container.GetService<GenerateHandler>().Execute(generateOptions);
                    case QualityOptions qualityOptions:
                        return container.GetService<QualityHandler>().Execute(qualityOptions);
                    default:
                        return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                log.LogError("An exception ocurred while attempting to execute this request", e);
                return -1;
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
