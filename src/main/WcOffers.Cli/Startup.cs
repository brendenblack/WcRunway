using Atlassian.Jira;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WcData.Microsoft.Extensions.DependencyInjection;
using WcOffers.Cli.Features;
using WcOffers.Cli.Features.Generate;
using WcOffers.Cli.Features.GenerateUnique;
using WcOffers.Cli.Features.ListTemplates;
using WcOffers.Cli.Features.Quality;
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

            // TODO: load all CommandLineHandler-derived classes
            services.AddTransient<TemplatedOfferGenerator>();
            services.AddTransient<TokenRunway>();
            services.AddTransient<GenerateUniqueHandler>();
            services.AddTransient<QualityHandler>();
            services.AddTransient<ListTemplatesHandler>();
            services.AddTransient<GenerateHandler>();
        }

        public static void ConfigureLogging(ServiceProvider container, IConfiguration config)
        {
            var configfile = config["logging:nlog:config-file"];
            if (string.IsNullOrWhiteSpace(configfile))
            {
                // TODO: how should this be handled?
            }

            container.GetRequiredService<ILoggerFactory>()
                .AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });

            NLog.LogManager.LoadConfiguration(configfile);
        }
    }
}
