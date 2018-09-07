using Atlassian.Jira;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WcData.Microsoft.Extensions.DependencyInjection;
using WcOffers.Cli.Features;
using WcOffers.Cli.Features.Generate;
using WcOffers.Cli.Features.GenerateUnique;
using WcOffers.Cli.Features.ListTemplates;
using WcOffers.Cli.Features.Quality;
using WcOffers.Cli.Features.Test;
using WcOffers.Cli.Features.Token;

namespace WcOffers.Cli
{
    public class Startup
    {

        /// <summary>
        /// Loads in any external configuration found, based on an order of precedence: 1. config file specified on 
        /// the command line 2. config file specified in the environment variable WC_OFFER_CONFIG 3. (default) config.ini.
        /// 
        /// In a development environment, appsettings.dev.json will be loaded and take priority over config.ini
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static IConfiguration LoadConfiguration(CommandLineOptions opts)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

            if (!string.IsNullOrWhiteSpace(opts.ConfigurationFile))
            {
                if (!File.Exists(opts.ConfigurationFile))
                {
                    throw new FileNotFoundException($"Unable to find configuration file specified on the command line at {opts.ConfigurationFile}");
                }
                
                // TODO: allow for .ini, .json and .xml files to be passed in and handled appropriately
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1#file-configuration-provider
                if (opts.ConfigurationFile.EndsWith(".ini"))
                {
                    builder.AddIniFile(opts.ConfigurationFile);
                }
                else if (opts.ConfigurationFile.EndsWith(".json"))
                {
                    builder.AddJsonFile(opts.ConfigurationFile);
                }
                else
                {
                    throw new ArgumentException($"Unable to load specified configuration file {opts.ConfigurationFile}. Files must be in INI or JSON format.");
                }
            }
            else if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("WC_OFFER_CONFIG")))
            {
                // TODO
            }
            else
            {
                builder.AddIniFile("config.ini", true)
                    .AddJsonFile("appsettings.dev.json", true);
            }


            return builder.Build();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
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

            var accountname = config["data:snowflake:account"];

            services.AddSnowflake(opt =>
            {
                opt.Account = config["data:snowflake:account"];
                opt.Username = config["data:snowflake:username"];
                opt.Password = config["data:snowflake:password"];
                opt.Database = config["data:snowflake:database"] ?? "";
                opt.Schema = config["data:snowflake:schema"] ?? "";
            });

            services.AddSingleton(s => Jira.CreateRestClient(config["data:jira:url"], config["data:jira:username"], config["data:jira:password"]));
            services.AddTransient<OfferJiraTicketManager>();

            // Add all handlers that derive from CommandLineHandler to the container (note the TODO below)
            var handlers = Assembly.GetAssembly(typeof(Startup)).DefinedTypes
                .Where(t => typeof(CommandLineHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToArray();

            // TODO: make this work
            // IsAssignableFrom always returns false because of the <T> required by the interface
            //Assembly.GetAssembly(typeof(Startup)).DefinedTypes
            //   .Where(t => typeof(ICommandLineHandler<CommandLineOptions>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            //   .ToArray();

            foreach (var handlerType in handlers.Select(h => h.AsType()))
            {
                services.AddTransient(handlerType);
            }

            //services.AddTransient<TemplatedOfferGenerator>();
            //services.AddTransient<TokenRunway>();
            //services.AddTransient<GenerateUniqueHandler>();
            //services.AddTransient<QualityHandler>();
            //services.AddTransient<ListTemplatesHandler>();
            //services.AddTransient<GenerateTemplateHandler>();
            //services.AddTransient<TestHandler>();
        }

        /// <summary>
        /// Logging is used to replace console output during operation of the program, as well as optional logging to file.
        /// The optional logging is configured by the user in the provided IConfiguration object
        /// </summary>
        /// <param name="container"></param>
        /// <param name="config"></param>
        /// <param name="consoleVerbosity"></param>
        public static void ConfigureLogging(ServiceProvider container, IConfiguration config, NLog.LogLevel consoleVerbosity = null)
        {
            var c = new LoggingConfiguration();
            // Console "logging" is actually the output of the program, and so it is controlled
            // without the user configuring it
            var consoleTarget = new ColoredConsoleTarget("target1")
            {
                Layout = @"${message} ${exception}"
            };

            c.AddTarget(consoleTarget);
            //c.AddRuleForOneLevel(NLog.LogLevel.Off, consoleTarget, "Microsoft.EntityFrameworkCore.*");
            c.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Info, consoleTarget, "Microsoft.*", true); // Only write errors from Microsoft.* classes to the console
            c.AddRule(consoleVerbosity ?? NLog.LogLevel.Info, NLog.LogLevel.Fatal, consoleTarget, "*");




            var logfile = config["logging:file"];
            if (!string.IsNullOrWhiteSpace(logfile))
            {
                var logfilePattern = config["logging:pattern"];
                if (string.IsNullOrWhiteSpace(logfilePattern))
                {
                    logfilePattern = @"${longdate} [${level:uppercase=true}] [${callsite}:${callsite-linenumber}] ${message} ${exception} ${all-event-properties}";
                }

                var level = NLog.LogLevel.FromString(config["logging:level"]) ?? NLog.LogLevel.Debug;

                var fileTarget = new FileTarget("logfileTarget")
                {
                    FileName = logfile,
                    Layout = logfilePattern                    
                };
                c.AddTarget(fileTarget);
                c.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Info, consoleTarget, "Microsoft.*", true); // Only write errors from Microsoft.* classes to the console
                c.AddRule(level, NLog.LogLevel.Fatal, fileTarget, "WcOffers.*");
            }

            container.GetRequiredService<ILoggerFactory>()
                .AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });

            NLog.LogManager.Configuration = c;
        }
    }
}
