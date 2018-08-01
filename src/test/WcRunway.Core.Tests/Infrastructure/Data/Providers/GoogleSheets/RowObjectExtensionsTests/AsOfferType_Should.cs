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
        [InlineData("unlock", OfferType.STANDARD_UNLOCK)]
        [InlineData("Unlock", OfferType.STANDARD_UNLOCK)]
        [InlineData("UNLOCK", OfferType.STANDARD_UNLOCK)]
        [InlineData("Omega Unlock", OfferType.OMEGA_UNLOCK)]
        [InlineData("OMEGA UNLOCK", OfferType.OMEGA_UNLOCK)]
        [InlineData("omega unlock", OfferType.OMEGA_UNLOCK)]
        [InlineData("Elite Unlock", OfferType.ELITE_UNLOCK)]
        [InlineData("ELITE UNLOCK", OfferType.ELITE_UNLOCK)]
        [InlineData("elite unlock", OfferType.ELITE_UNLOCK)]
        public void ReturnExpectedOfferType(string input, OfferType expectedOutput)
        {
            Object[] row = new Object[20];
            row[1] = (Object)input;
            row.ReadColumnAsOfferType(1).ShouldBe(expectedOutput);
        }
    }
}
