using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Cli.Features.Generate;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;
using Xunit;

namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
{
    public class ValidatePrefix_Should : IClassFixture<ExecuteFixture> //, IDisposable
    {
        public ValidatePrefix_Should(ExecuteFixture fixture)
        {
            this.fixture = fixture;
            this.sb2 = fixture.SetupSandbox2($"Execute_{new Guid().ToString()}");
            var genLogger = TestHelpers.CreateLogger<GenerateHandler>();
            this.sut = new GenerateHandler(genLogger, fixture.GameContext, fixture.OfferGenerator, sb2);
        }

        private readonly ExecuteFixture fixture;
        private readonly Sandbox2Context sb2;
        private readonly GenerateHandler sut;

        [Fact]
        public void ReturnExpectedPrefix()
        {
            var result = this.sut.ValidatePrefix("TestPrefix");

            result.ShouldBe("TestPrefix");
        }

        [Theory]
        [InlineData("Abcdefghijklmnopqrstuvwxyz")]
        [InlineData("Abcdefghijklmnopq")]
        public void ReturnTruncatedPrefixWhenLongerThan16Characters(string prefix)
        {
            var result = this.sut.ValidatePrefix(prefix);

            result.ShouldBe("Abcdefghijklmnop");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ThrowWhenNullOrWhitespace(string prefix)
        {
            Should.Throw<ArgumentException>(() => this.sut.ValidatePrefix(prefix));
        }

        [Theory]
        [InlineData("Test123abc")]
        [InlineData("Test123")]
        public void ThrowWhenInUse(string existingOfferCode)
        {
            var existingOffer = TestHelpers.CreateTestOffer(existingOfferCode);
            this.sb2.Offers.Add(existingOffer);
            this.sb2.SaveChanges();

            Should.Throw<InvalidOperationException>(() => this.sut.ValidatePrefix("Test123"));
        }

        //public void Dispose()
        //{
        //    this.fixture.Sandbox2.Offers.RemoveRange(this.fixture.Sandbox2.Offers);
        //    this.fixture.Sandbox2.SaveChanges();
        //}
    }
}
