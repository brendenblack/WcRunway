using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WcData.GameContext;
using WcData.Sheets;
using WcData.Snowflake;
using WcOffers.Cli;
using WcOffers.Cli.Features.Generate;
using Xunit;

namespace WcOffers.CliTests.StartupTests
{
    public class ConfigureServices_Should
    {
        [Fact]
        public void AddSandbox2Context()
        {
            var opts = new GenerateOptions { ConfigurationFile = Path.GetFullPath("StartupTests/validconfig.json") };
            IConfiguration config = Startup.LoadConfiguration(opts);
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection, config);
            ServiceProvider container = serviceCollection.BuildServiceProvider();

            container.GetRequiredService<ISandbox2Context>().ShouldNotBeNull();
        }

        [Fact]
        public void AddSnowflakeContext()
        {
            var opts = new GenerateOptions { ConfigurationFile = Path.GetFullPath("StartupTests/validconfig.json") };
            IConfiguration config = Startup.LoadConfiguration(opts);
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection, config);
            ServiceProvider container = serviceCollection.BuildServiceProvider();

            container.GetRequiredService<IPveBattles>().ShouldNotBeNull();
        }

        [Fact]
        public void AddSheetsContext()
        {
            var opts = new GenerateOptions { ConfigurationFile = Path.GetFullPath("StartupTests/validconfig.json") };
            IConfiguration config = Startup.LoadConfiguration(opts);
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection, config);
            ServiceProvider container = serviceCollection.BuildServiceProvider();

            container.GetRequiredService<IUnitData>().ShouldNotBeNull();
            container.GetRequiredService<IOfferData>().ShouldNotBeNull();
            container.GetRequiredService<IGameData>().ShouldNotBeNull();
        }
    }
}
