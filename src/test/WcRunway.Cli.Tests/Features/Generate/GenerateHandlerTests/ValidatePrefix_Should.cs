using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Cli.Features.Generate;
using Xunit;

namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
{
    public class ValidatePrefix_Should : IClassFixture<GenerateHandlerFixture>, IDisposable
    {
        public ValidatePrefix_Should(GenerateHandlerFixture fixture)
        {
            this.sut = fixture.Handler;
            this.fixture = fixture;
        }

        private readonly GenerateHandler sut;
        private readonly GenerateHandlerFixture fixture;

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
            this.fixture.Sandbox2.Offers.Add(existingOffer);
            this.fixture.Sandbox2.SaveChanges();

            Should.Throw<InvalidOperationException>(() => this.sut.ValidatePrefix("Test123"));
        }

        public void Dispose()
        {
            this.fixture.Sandbox2.Offers.RemoveRange(this.fixture.Sandbox2.Offers);
            this.fixture.Sandbox2.SaveChanges();
        }
    }
}
