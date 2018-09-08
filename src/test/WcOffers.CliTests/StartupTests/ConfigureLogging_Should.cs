using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WcOffers.Cli;
using WcOffers.Cli.Features.Generate;

namespace WcOffers.CliTests.StartupTests
{
    public class ConfigureLogging_Should
    {
        public void AddConsoleTarget()
        {
            var opts = new GenerateTemplateOptions { ConfigurationFile = Path.GetFullPath("StartupTests/validconfig.ini") };
            IConfiguration config = Startup.LoadConfiguration(opts);
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection, config);
            var container = serviceCollection.BuildServiceProvider();
            Startup.ConfigureLogging(container, config);

            var loggerFactory = container.GetRequiredService<ILoggerFactory>();

            LogManager.LogFactory.ta
        }
    }
}
