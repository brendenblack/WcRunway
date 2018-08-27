using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Sheets;
using WcData.Sheets.Models;
using WcOffers;
using Xunit;

namespace WcOffersTests.TemplatedOfferGeneratorTests
{
    public class GenerateOfferFromTemplate_Should
    {
        public GenerateOfferFromTemplate_Should()
        {

        }

        [Fact]
        public void SetOfferCode()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var template = new OfferTemplate
            {

            };
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);

            var result = sut.GenerateOfferFromTemplate(template, "MyCode");

            result.OfferCode.ShouldBe("MyCode");
        }

        [Fact]
        public void ReplaceParametersInTitle()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var template = new OfferTemplate
            {
                OfferTitle = "My %lame% title"
            };
            var parameters = new Dictionary<string, string>
            {
                { "lame", "special" }
            };
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);

            var result = sut.GenerateOfferFromTemplate(template, "MyCode", parameters);

            result.Title.ShouldBe("My special title");
        }

        [Fact]
        public void ReplaceMultipleParametersInTitle()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var template = new OfferTemplate
            {
                OfferTitle = "My %adjective% and %length% title"
            };
            var parameters = new Dictionary<string, string>
            {
                { "adjective", "special" },
                { "length", "long" }
            };
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);

            var result = sut.GenerateOfferFromTemplate(template, "MyCode", parameters);

            result.Title.ShouldBe("My special and long title");
        }

    }
}
