using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.GameContext.Models;
using WcData.Sheets;
using WcOffers;
using Xunit;

namespace WcOffersTests.TemplatedOfferGeneratorTests
{
    public class CheckOfferForPlaceholder_Should
    {
        public CheckOfferForPlaceholder_Should()
        {
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var mockOfferData = new Mock<IOfferData>().Object;

            sut = new TemplatedOfferGenerator(logger, mockOfferData);
        }

        private readonly TemplatedOfferGenerator sut;

        [Theory]
        [InlineData("My %description% with %2% placeholders", 2)]
        [InlineData("My %description% with 1 placeholders", 1)]
        [InlineData("My description with 0 placeholders", 0)]
        [InlineData("%My% %description% %with% %4% placeholders", 4)]
        [InlineData("%My description with a long but invalid placeholder%", 0)]
        public void ReturnNumberOfDescriptionPlaceholders(string description, int placeholderCount)
        {
            var offer = new Offer
            {
                Description = description
            };

            var result= sut.CheckOfferForPlaceholder(offer);

            result.ShouldBe(placeholderCount);
        }

        [Theory]
        [InlineData("My %title% with %2% placeholders", 2)]
        [InlineData("My %title% with 1 placeholders", 1)]
        [InlineData("My title with 0 placeholders", 0)]
        [InlineData("%My% %title% %with% %4% placeholders", 4)]
        [InlineData("%My title with a long but invalid placeholder%", 0)]
        public void ReturnNumberOfTitlePlaceholders(string title, int placeholderCount)
        {
            var offer = new Offer
            {
                Title = title
            };

            var result = sut.CheckOfferForPlaceholder(offer);

            result.ShouldBe(placeholderCount);
        }

        [Theory]
        [InlineData("My %icontitle% with %2% placeholders", 2)]
        [InlineData("My %icontitle% with 1 placeholders", 1)]
        [InlineData("My icontitle with 0 placeholders", 0)]
        [InlineData("%My% %icontitle% %with% %4% placeholders", 4)]
        [InlineData("%My icontitle with a long but invalid placeholder%", 0)]
        public void ReturnNumberOfIconTitlePlaceholders(string iconTitle, int placeholderCount)
        {
            var offer = new Offer
            {
                IconTitle = iconTitle
            };

            var result = sut.CheckOfferForPlaceholder(offer);

            result.ShouldBe(placeholderCount);
        }

        [Theory]
        [InlineData("My %icondesc% with %2% placeholders", 2)]
        [InlineData("My %icondesc% with 1 placeholders", 1)]
        [InlineData("My icondesc with 0 placeholders", 0)]
        [InlineData("%My% %icondesc% %with% %4% placeholders", 4)]
        [InlineData("%My icondesc with a long but invalid placeholder%", 0)]
        public void ReturnNumberOfIconDescriptionPlaceholders(string iconDescription, int placeholderCount)
        {
            var offer = new Offer
            {
                IconDescription = iconDescription
            };

            var result = sut.CheckOfferForPlaceholder(offer);

            result.ShouldBe(placeholderCount);
        }

        [Theory]
        [InlineData("{ \"gold\":0, \"units\": [ { \"type\": 244, \"level\": %level% } ]", 1)]
        [InlineData("{ \"gold\":0, \"units\": [ { \"type\": 244, \"level\": 2 } ]", 0)]
        [InlineData("{ \"gold\":0, \"units\": [ { \"type\": 244, \"level\": %level1% }, { \"type\": 251, \"level\": %level2% } ]", 2)]
        [InlineData("{ \"gold\":0, \"units\": [ { \"type\": 244, \"level\": %invalid placeholder% } ]", 0)]
        public void ReturnNumberOfContentPlaceholders(string contentJson, int placeholderCount)
        {
            var offer = new Offer
            {
                ContentJson = contentJson
            };

            var result = sut.CheckOfferForPlaceholder(offer);

            result.ShouldBe(placeholderCount);
        }



    }
}
