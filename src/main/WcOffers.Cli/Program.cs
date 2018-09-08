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
            int exitCode = Parser.Default.ParseArguments<ValidateOptions, QualityOptions, GenerateTemplateOptions, GenerateUniqueOptions, ListTemplatesOptions, TestOptions>(args)
                .MapResult(
                    (ValidateOptions o) => new ValidateHandler().Execute(o),
                    (CommandLineOptions o) =>
                    {
                        // Read in application configuration
                        IConfiguration config = Startup.LoadConfiguration(o);

                        // Create the container
                        var serviceCollection = new ServiceCollection();
                        Startup.ConfigureServices(serviceCollection, config);
                        ServiceProvider container = serviceCollection.BuildServiceProvider();

                        // Configure logging
                        var outputLevel = (o.IsExtraVerbose) ? NLog.LogLevel.Trace : (o.IsVerbose) ? NLog.LogLevel.Debug : NLog.LogLevel.Info;
                        Startup.ConfigureLogging(container, config, outputLevel);

                        // Execute the request
                        return Execute(container, o);
                    },
                    (errs) =>
                    {
                        // Simply return an (error) exit code here; the help subsystem of the CommandLine library
                        // will print an error message and the help screen automatically at this point
                        // https://github.com/commandlineparser/commandline/wiki#parsing
                        return -11;
                    });

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
                    case ListTemplatesOptions listTemplateOptions:
                        return container.GetService<ListTemplatesHandler>().Execute(listTemplateOptions);
                    case GenerateTemplateOptions generateOptions:
                        return container.GetService<GenerateTemplateHandler>().Execute(generateOptions);
                    case QualityOptions qualityOptions:
                        return container.GetService<QualityHandler>().Execute(qualityOptions);
                    case TestOptions testOptions:
                        return container.GetService<TestHandler>().TestLogging(testOptions);
                    case GenerateUniqueOptions generateUniqueOptions:
                        return container.GetService<GenerateUniqueHandler>().Execute(generateUniqueOptions);
                    default:
                        log.LogError("Unable to locate a handler for {}", opts.GetType().Name);
                        return -1;
                }
            }
            catch (Exception e)
            {
                log.LogError(e, "An exception ocurred while attempting to execute this request");
                return -1;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }        
    }
}
