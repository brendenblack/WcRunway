using Shouldly;
using System;
using WcOffers.Cli.Features.GenerateUnique;
using WcCore.Domain.Offers;
using Xunit;
using WcData.GameContext;

namespace WcOffers.Cli.Tests.Features.Generate.GenerateUniqueHandlerTests
{
    /*
     * This class specifically tests the generic features of Execute() that aren't tied to the option switches
     */
    public class Execute_Should : IClassFixture<ExecuteFixture>
    { 
        public Execute_Should(ExecuteFixture fixture)
        {
            this.fixture = fixture;
            this.sb2 = fixture.SetupSandbox2($"Execute_{new Guid().ToString()}");
            var genLogger = TestHelpers.CreateLogger<GenerateUniqueHandler>();
            this.sut = new GenerateUniqueHandler(genLogger, fixture.GameContext, fixture.OfferGenerator, sb2);
        }

        private readonly ExecuteFixture fixture;
        private readonly Sandbox2Context sb2;
        private readonly GenerateUniqueHandler sut;

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ReturnNegative1WhenPrefixInvalid(string prefix)
        {
            var opts = new GenerateUniqueOptions { OfferCodePrefix = prefix };

            var exitCode = this.sut.Execute(opts);

            exitCode.ShouldBe(-1);
        }

        [Fact]
        public void ReturnNegative1WhenPrefixInUse()
        {
            var existingOffer = new Offer() { OfferCode = "InUseTest123" };
            this.sb2.Offers.Add(existingOffer);
            this.sb2.SaveChanges();
            var opts = new GenerateUniqueOptions { OfferCodePrefix = "InUseTest", UnitId = 217 };

            var exitCode = this.sut.Execute(opts);

            exitCode.ShouldBe(-1);
        }

        [Fact]
        public void ReturnNegative1WhenUnitNotFound()
        {
            var opts = new GenerateUniqueOptions { OfferCodePrefix = "AvailPrefix", UnitId = 1908237 };

            var exitCode = this.sut.Execute(opts);

            exitCode.ShouldBe(-1);
        }
    }
}
