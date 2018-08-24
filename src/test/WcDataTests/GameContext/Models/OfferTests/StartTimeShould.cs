using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.GameContext.Models;
using Xunit;

namespace WcDataTests.GameContext.Models.OfferTests
{
    public class StartTimeShould
    {
        [Theory]
        [InlineData(1514793600)]
        [InlineData(1532415600)]
        public void ReturnExpectedTimeWhenValid(int epochSeconds)
        {
            var expectedTime = DateTimeOffset.FromUnixTimeSeconds(epochSeconds).ToOffset(TimeSpan.FromHours(-7));

            var offer = new Offer { StartTimeEpochSeconds = epochSeconds };

            offer.StartTime.ShouldBe(expectedTime);
        }
    }
}
