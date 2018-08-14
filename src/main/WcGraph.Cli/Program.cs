﻿using AutoMapper;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.IO;
using WcData.Microsoft.Extensions.DependencyInjection;
using WcGraph.Cli.Features.Import;
using WcGraph.Data;

namespace WcGraph.Cli
{
    public class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.dev.json", true)
                .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            // Build the container
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, config);
            var container = serviceCollection.BuildServiceProvider();

            try
            {
                Parser.Default.ParseArguments<ImportOptions>(args)
                    .MapResult(
                        (ImportOptions o) => container.GetService<ImportHandler>().Execute(o),
                        (errs) => HandleParseError(errs));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
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
            services.AddLogging((builder) => builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace));

            services.AddAutoMapper(typeof(Program));

            // Add Live - Slave database connection
            services.AddLiveSlave(opt =>
            {
                opt.Url = config["data:liveslave:url"];
                opt.Name = config["data:liveslave:name"];
                opt.Username = config["data:liveslave:username"];
                opt.Password = config["data:liveslave:password"];
            });

            // Add connection to game data stored in Google Sheets
            services.AddSheets(opts => { opts.ClientSecretPath = Path.GetFullPath("client_secret.json"); });

            services.AddSnowflake(opts =>
            {
                opts.Account = config["data:snowflake:account"];
                opts.Username = config["data:snowflake:username"];
                opts.Password = config["data:snowflake:password"];
            });


            var driver = GraphDatabase.Driver("bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "password"));
            services.AddTransient<IDriver>(d => driver);
            services.AddTransient<PveBattleRepository>();

            services.AddTransient<ImportHandler>();
        }
    }
}
