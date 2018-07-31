using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets.Extension;
using Xunit;

namespace WcRunway.Core.Tests.Infrastructure.Data.Providers.GoogleSheets.RowObjectExtensionsTests
{
    public class AsOfferType_Should
    {
        [Theory]
        [InlineData("omega parts")]
        [InlineData("Omega parts")]
        [InlineData("Omega Parts")]
        [InlineData("OMEGA PARTS")]
        public void ReturnOmegaParts(string input)
        {
            Object value = input;

            value.AsOfferType().ShouldBe(OfferType.OMEGA_PARTS);
        }

        [Theory]
        [InlineData("omega parts", OfferType.OMEGA_PARTS)]
        [InlineData("Omega parts", OfferType.OMEGA_PARTS)]
        [InlineData("Omega Parts", OfferType.OMEGA_PARTS)]
        [InlineData("OMEGA PARTS", OfferType.OMEGA_PARTS)]
        [InlineData("elite parts", OfferType.ELITE_PARTS)]
        [InlineData("Elite Parts", OfferType.ELITE_PARTS)]
        [InlineData("Elite parts", OfferType.ELITE_PARTS)]
        [InlineData("ELITE PARTS", OfferType.ELITE_PARTS)]
        [InlineData("Levels", OfferType.LEVELS)]
        [InlineData("levels", OfferType.LEVELS)]
        [InlineData("unlock", OfferType.UNIT_UNLOCK)]
        [InlineData("Unlock", OfferType.UNIT_UNLOCK)]
        [InlineData("UNLOCK", OfferType.UNIT_UNLOCK)]
        public void ReturnExpectedOfferType(string input, OfferType expectedOutput)
        {
            Object value = input;

            value.AsOfferType().ShouldBe(expectedOutput);
        }
    }
}
