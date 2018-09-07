using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.GameContext;
using WcData.Sheets;
using WcOffers.Cli.Features.Generate;
using WcOffers.Cli.Tests;
using Xunit;

namespace WcOffers.CliTests.Features.Generate.GenerateTemplateHandlerTests
{
    public class ReadParameters_Should
    {
        public ReadParameters_Should()
        {
            var mockOfferData = new Mock<IOfferData>();
            var mockSb2 = new Mock<ISandbox2Context>();
            var generatorLogger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var gen = new TemplatedOfferGenerator(generatorLogger, mockOfferData.Object);
            var handlerLogger = TestHelpers.CreateLogger<GenerateTemplateHandler>();
            sut = new GenerateTemplateHandler(handlerLogger, mockOfferData.Object, mockSb2.Object, gen);
        }

        private readonly GenerateTemplateHandler sut;


        [Fact]
        public void ReturnExpectedNumberOfItems()
        {
            var opts = new GenerateTemplateOptions
            {
                Parameters =
                new List<string> {
                    "param1=value 1",
                    "param2=value 2",
                    "param3=value 3",
                    "param4=value 4"
                }
            };

            var parameters = sut.ReadParameters(opts);

            parameters.Count.ShouldBe(4);
        }

        [Fact]
        public void ReturnValuesWithSpaces()
        {
            var opts = new GenerateTemplateOptions
            {
                Parameters =
                new List<string> {
                    "param1=value 1",
                }
            };

            var parameters = sut.ReadParameters(opts);

            parameters.GetValueOrDefault("param1").ShouldBe("value 1");
        }

        [Fact]
        public void IgnoreKeysWithSpaces()
        {
            var opts = new GenerateTemplateOptions
            {
                Parameters =
                new List<string> {
                    "param 1=value 1",
                }
            };

            var parameters = sut.ReadParameters(opts);

            parameters.Count.ShouldBe(0);
        }

        [Fact]
        public void ReturnValidParametersWhenInvalidOnesAreIncluded()
        {
            var opts = new GenerateTemplateOptions
            {
                Parameters =
                new List<string> {
                    "param 1=illegal parameter",
                    "param2=valid"
                }
            };

            var parameters = sut.ReadParameters(opts);

            parameters.GetValueOrDefault("param2").ShouldBe("valid");
        }

        [Fact]
        public void IgnoreInvalidParametersWhenValidParametersAreIncluded()
        {
            var opts = new GenerateTemplateOptions
            {
                Parameters =
                new List<string> {
                    "param 1=illegal parameter",
                    "param2=valid",
                    "param3=also valid"
                }
            };

            var parameters = sut.ReadParameters(opts);

            parameters.Count.ShouldBe(2);
        }

    }
}
