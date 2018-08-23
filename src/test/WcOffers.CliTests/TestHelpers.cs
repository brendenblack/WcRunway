using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.GameContext.Models;

namespace WcOffers.Cli.Tests
{
    public class TestHelpers
    {
        public static ILogger<T> CreateLogger<T>()
        {
            var factory = new LoggerFactory().AddDebug();
            var logger = factory.CreateLogger<T>();
            return logger;
        }

        public static Offer CreateTestOffer(string code)
        {
            var offer = new Offer
            {
                OfferCode = code,
                ContentJson = "{ \"gold\": 0 }",
                DisplayedItemsJson = "[]",
                TemplateId = 6,
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now,
                Title = $"{code} offer",
                IconTitle = $"{code} offer",
                Description = $"Test offer for {code}",
                IconDescription = $"Test offer for {code}"
            };

            return offer;
        }
    }
}
