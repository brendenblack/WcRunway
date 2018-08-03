using Shouldly;
using System;
using System.Linq;
using WcData.GameContext;
using WcOffers.Cli.Features.Generate;
using Xunit;

namespace WcOffers.Cli.Tests.Features.Generate.GenerateHandlerTests
{
    public class Execute_Unlock_Should : IClassFixture<ExecuteFixture>
    {
        public Execute_Unlock_Should(ExecuteFixture fixture)
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
                OfferCodePrefix = "Jul18Test",
                IncludeUnlock = true,
                IncludeEliteParts = false,
                IncludeLevels = false,
                IncludeOmegaParts = false,
                IncludeTech = false
            };

            var result = this.sut.Execute(opts);

            result.ShouldBe(0);
        }
        
        [Fact]
        public void AddUnlockOfferWithExpectedOfferCodeToDatabase()
        {
            var opts = new GenerateOptions()
            {
                UnitId = 217,
                OfferCodePrefix = "Test123",
                IncludeUnlock = true,
                IncludeEliteParts = false,
                IncludeLevels = false,
                IncludeOmegaParts = false,
                IncludeTech = false
            };

            this.sut.Execute(opts);
            var offer = this.sb2.Offers.FirstOrDefault(o => o.OfferCode == "Test123Unl");

            offer.ShouldNotBeNull();
        }        
        #endregion
    }
}
