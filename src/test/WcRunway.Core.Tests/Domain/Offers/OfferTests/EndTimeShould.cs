using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;
using Xunit;

namespace WcRunway.Core.Tests.Domain.Offers.OfferTests
{
    public class EndTimeShould
    {
        [Theory]
        [InlineData(1514793600)]
        [InlineData(1532415600)]
        public void ReturnExpectedTimeWhenValid(int epochSeconds)
        {
            var expectedTime = DateTimeOffset.FromUnixTimeSeconds(epochSeconds).ToOffset(TimeSpan.FromHours(-7));

            var offer = new Offer { EndTimeEpochSeconds = epochSeconds };

            offer.EndTime.ShouldBe(expectedTime);
        }
    }
}
