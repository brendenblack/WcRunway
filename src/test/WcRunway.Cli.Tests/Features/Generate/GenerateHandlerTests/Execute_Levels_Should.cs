using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcRunway.Cli.Features.Generate;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;
using Xunit;

namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
{
    public class Execute_Levels_Should : IClassFixture<ExecuteFixture>
    {
        public Execute_Levels_Should(ExecuteFixture fixture)
        {
            this.fixture = fixture;
            this.sb2 = fixture.SetupSandbox2($"Execute_{new Guid().ToString()}");
            var genLogger = TestHelpers.CreateLogger<GenerateHandler>();
            this.sut = new GenerateHandler(genLogger, fixture.GameContext, fixture.OfferGenerator, sb2);
        }

        private readonly ExecuteFixture fixture;
        private readonly Sandbox2Context sb2;
        private readonly GenerateHandler sut;

        #region Unlock tests
        [Fact]
        public void Return0WhenSuccessful()
        {
            var opts = new GenerateOptions()
            {
                UnitId = 217,
                OfferCodePrefix = "LvlReturnCode",
                IncludeUnlock = false,
                IncludeEliteParts = false,
                IncludeLevels = true,
                IncludeOmegaParts = false,
                IncludeTech = false
            };

            var result = this.sut.Execute(opts);

            result.ShouldBe(0);
        }


        [Fact]
        public void AddLevelOffersWithExpectedOfferCodesToDatabase()
        {
            var opts = new GenerateOptions()
            {
                UnitId = 217,
                OfferCodePrefix = "LvlTest123",
                IncludeUnlock = false,
                IncludeEliteParts = false,
                IncludeLevels = true,
                IncludeOmegaParts = false,
                IncludeTech = false
            };

            this.sut.Execute(opts);
            var offers = this.sb2.Offers.Where(o => o.OfferCode.StartsWith("LvlTest123Lv")).ToList();

            offers.Count.ShouldBe(8); //.ShouldNotBeNull("No offer was found in the database with the code 'Test123Tec1'");
        }
        #endregion
    }
}
