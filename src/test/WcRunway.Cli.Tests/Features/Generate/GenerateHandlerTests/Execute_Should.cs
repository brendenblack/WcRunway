using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Cli.Features.Generate;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;
using Xunit;

namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
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
            var genLogger = TestHelpers.CreateLogger<GenerateHandler>();
            this.sut = new GenerateHandler(genLogger, fixture.GameContext, fixture.OfferGenerator, sb2);
        }

        private readonly ExecuteFixture fixture;
        private readonly Sandbox2Context sb2;
        private readonly GenerateHandler sut;

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ReturnNegative1WhenPrefixInvalid(string prefix)
        {
            var opts = new GenerateOptions { OfferCodePrefix = prefix };

            var exitCode = this.sut.Execute(opts);

            exitCode.ShouldBe(-1);
        }

        [Fact]
        public void ReturnNegative1WhenPrefixInUse()
        {
            var existingOffer = new Offer() { OfferCode = "InUseTest123" };
            this.sb2.Offers.Add(existingOffer);
            this.sb2.SaveChanges();
            var opts = new GenerateOptions { OfferCodePrefix = "InUseTest", UnitId = 217 };

            var exitCode = this.sut.Execute(opts);

            exitCode.ShouldBe(-1);
        }

        [Fact]
        public void ReturnNegative1WhenUnitNotFound()
        {
            var opts = new GenerateOptions { OfferCodePrefix = "AvailPrefix", UnitId = 1908237 };

            var exitCode = this.sut.Execute(opts);

            exitCode.ShouldBe(-1);
        }
    }
}
