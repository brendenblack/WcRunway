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
        public void SetStartTimeToNowish()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var template = new OfferTemplate();
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);

            var result = sut.GenerateOfferFromTemplate(template, "MyCode");

            result.StartTime.ShouldBe(DateTimeOffset.Now, TimeSpan.FromSeconds(30));
        }

        [Fact]
        public void SetEndTimeTo3DaysFromNowish()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var template = new OfferTemplate();
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);

            var result = sut.GenerateOfferFromTemplate(template, "MyCode");

            result.EndTime.ShouldBe(DateTimeOffset.Now.AddDays(3), TimeSpan.FromSeconds(30));
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

        [Fact]
        public void NotFailWhenTitleParametersNotProvided()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);
            var template = new OfferTemplate
            {
                OfferTitle = "My %parameterized% title"
            };

            var result = sut.GenerateOfferFromTemplate(template, "MyCode");

            result.ShouldNotBeNull();
        }

        [Fact]
        public void ReplaceParametersInDescription()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var template = new OfferTemplate
            {
                OfferDescription = "My %adjective% description"
            };
            var parameters = new Dictionary<string, string>
            {
                { "adjective", "special" }
            };
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);

            var result = sut.GenerateOfferFromTemplate(template, "MyCode", parameters);

            result.Description.ShouldBe("My special description");
        }

        [Fact]
        public void ReplaceMultipleParametersInDescription()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var template = new OfferTemplate
            {
                OfferDescription = "My %adjective% and %length% description"
            };
            var parameters = new Dictionary<string, string>
            {
                { "adjective", "special" },
                { "length", "long" }
            };
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);

            var result = sut.GenerateOfferFromTemplate(template, "MyCode", parameters);

            result.Description.ShouldBe("My special and long description");
        }

        [Fact]
        public void NotFailWhenDescriptionParametersNotProvided()
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);
            var template = new OfferTemplate
            {
                OfferDescription = "My %parameterized% description"
            };

            var result = sut.GenerateOfferFromTemplate(template, "MyCode");

            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(9075)]
        public void SetPriorityWhenValid(int priority)
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);
            var template = new OfferTemplate();

            var result = sut.GenerateOfferFromTemplate(template, "MyCode", priority: priority);

            result.Priority.ShouldBe(priority);
        }

        [Theory]
        [InlineData(-901274)]
        [InlineData(-1)]
        public void SetPriorityTo0WhenInvalid(int invalidPriority)
        {
            var mockOfferData = new Mock<IOfferData>();
            var logger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var sut = new TemplatedOfferGenerator(logger, mockOfferData.Object);
            var template = new OfferTemplate();

            var result = sut.GenerateOfferFromTemplate(template, "MyCode", priority: invalidPriority);

            result.Priority.ShouldBe(invalidPriority);
        }
    }
}
