using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;
using Xunit;

namespace WcRunway.CoreTests.Domain.Offers
{
    public class Offer_StartTimeShould
    {
        [Theory]
        [InlineData(1514793600)]
        public void ReturnExpectedTimeWhenValid(int epochSeconds)
        {
            var expectedTime = DateTimeOffset.FromUnixTimeSeconds(1514793600).ToOffset(TimeSpan.FromHours(-7));

            var offer = new Offer { StartTimeEpochSeconds = epochSeconds };
            
            offer.StartTime.ShouldBe(expectedTime);
        }
    }
}
