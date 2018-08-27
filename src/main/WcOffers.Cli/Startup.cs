using Atlassian.Jira;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Microsoft.Extensions.DependencyInjection;
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
        public void ConfigureServices(IServiceCollection services, IConfiguration config)
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

            services.AddTransient<TemplatedOfferGenerator>();
            services.AddTransient<TokenRunway>();
            services.AddTransient<GenerateUniqueHandler>();
            services.AddTransient<QualityHandler>();
            services.AddTransient<TestHandler>();
            services.AddTransient<ListTemplatesHandler>();
            services.AddTransient<GenerateHandler>();
        }
    }
}
