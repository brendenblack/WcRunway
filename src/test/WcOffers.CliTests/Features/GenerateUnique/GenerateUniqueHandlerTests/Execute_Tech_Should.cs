using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcData.GameContext;
using WcOffers.Cli.Features.GenerateUnique;
using Xunit;

namespace WcOffers.Cli.Tests.Features.Generate.GenerateUniqueHandlerTests
{
    public class Execute_Tech_Should : IClassFixture<ExecuteFixture>
    { 
        public Execute_Tech_Should(ExecuteFixture fixture)
        {
            this.fixture = fixture;
            this.sb2 = fixture.SetupSandbox2($"Execute_{new Guid().ToString()}");
            var genLogger = TestHelpers.CreateLogger<GenerateUniqueHandler>();
            this.sut = new GenerateUniqueHandler(genLogger, fixture.GameContext, fixture.OfferGenerator, sb2);
        }

        private readonly ExecuteFixture fixture;
        private readonly Sandbox2Context sb2;
        private readonly GenerateUniqueHandler sut;


        #region Unlock tests
        [Fact]
        public void Return0WhenSuccessful()
        {
            var opts = new GenerateUniqueOptions()
            {
                UnitId = 217,
                OfferCodePrefix = "TechTest",
                IncludeUnlock = false,
                IncludeEliteParts = false,
                IncludeLevels = false,
                IncludeOmegaParts = false,
                IncludeTech = true
            };

            var result = this.sut.Execute(opts);

            result.ShouldBe(0);
        }


        [Fact]
        public void AddTechOffersWithExpectedOfferCodesToDatabase()
        {
            var opts = new GenerateUniqueOptions()
            {
                UnitId = 257,
                OfferCodePrefix = "Test123",
                IncludeUnlock = false,
                IncludeEliteParts = false,
                IncludeLevels = false,
                IncludeOmegaParts = false,
                IncludeTech = true
            };

            this.sut.Execute(opts);
            var offers = this.sb2.Offers.Where(o => o.OfferCode.StartsWith("Test123Tec")).ToList();

            offers.Count.ShouldBe(2); //.ShouldNotBeNull("No offer was found in the database with the code 'Test123Tec1'");
        }
        #endregion

    }
}
