using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcData.GameContext;
using WcData.GameContext.Implementation;
using WcData.GameContext.Models;
using WcData.Sheets;
using WcData.Sheets.Models;
using WcOffers.Cli.Features.Generate;
using WcOffers.Cli.Tests;
using Xunit;

namespace WcOffers.CliTests.Features.Generate.GenerateHandlerTests
{
    public class Handle_Should
    {
        public Handle_Should()
        { }
        // ILogger<GenerateHandler> logger, IOfferData offerData, ISandbox2Context sb2, TemplatedOfferGenerator gen)
        [Fact]
        public void ReturnErrorCode_WhenOfferCodeIsNullOrWhitespace()
        {
            
            var mockOfferData = new Mock<IOfferData>();
            var mockSb2 = new Mock<ISandbox2Context>();
            var generatorLogger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var gen = new TemplatedOfferGenerator(generatorLogger, mockOfferData.Object);
            var handlerLogger = TestHelpers.CreateLogger<GenerateHandler>();
            var sut = new GenerateHandler(handlerLogger, mockOfferData.Object, mockSb2.Object, gen);
            var opts = new GenerateOptions { OfferCode = null };

            var result = sut.Execute(opts);

            result.ShouldBe(-1);
        }

        [Fact]
        public void ReturnErrorCode_WhenOfferCodeIsInUse()
        {
            var mockOfferData = new Mock<IOfferData>();
            var generatorLogger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var gen = new TemplatedOfferGenerator(generatorLogger, mockOfferData.Object);
            var options = new DbContextOptionsBuilder<GameDbContext>()
               .UseInMemoryDatabase(databaseName: "ReturnErrorCodeWhenOfferCodeIsInUse")
               .Options;
            ISandbox2Context sb2 = new GameDbContext(options);
            var existingOffer = new Offer { OfferCode = "TestCode1" };
            sb2.Offers.Add(existingOffer);
            sb2.SaveChanges();
            var generateLogger = TestHelpers.CreateLogger<GenerateHandler>();
            var sut = new GenerateHandler(generateLogger, mockOfferData.Object, sb2, gen);
            var opts = new GenerateOptions { OfferCode = "TestCode1" };

            var result = sut.Execute(opts);

            result.ShouldBe(-1);
        }
        
        [Fact]
        public void ReturnErrorCode_WhenTemplateNotFound()
        {
            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Templates).Returns(new List<OfferTemplate>());
            var generatorLogger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var gen = new TemplatedOfferGenerator(generatorLogger, mockOfferData.Object);
            var options = new DbContextOptionsBuilder<GameDbContext>()
               .UseInMemoryDatabase(databaseName: "ReturnErrorCodeWhenOfferCodeIsInUse")
               .Options;
            ISandbox2Context sb2 = new GameDbContext(options);
            var generateLogger = TestHelpers.CreateLogger<GenerateHandler>();
            var sut = new GenerateHandler(generateLogger, mockOfferData.Object, sb2, gen);
            var opts = new GenerateOptions { OfferCode = "TestCode1", TemplateId = 1 };

            var result = sut.Execute(opts);

            result.ShouldBe(-1);
        }

        [Fact]
        public void ReturnErrorCode_WhenIgnoreWarningsIsFalse_AndMissingParametersRaiseWarnings()
        {
            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Templates).Returns(new List<OfferTemplate>
            {
                new OfferTemplate
                {
                    Id = 1,
                    OfferTitle = "title",
                    OfferDescription = "description with %paramcount% parameters",
                    OfferContent = "{ \"skus\": { \"gold\": 0 } }",
                    OfferCost = 0,
                    OfferCostSku = "gold",
                    OfferFullCost = -1,
                    OfferDisplay = "[{}]",
                    OfferDuration = 1000,
                    OfferIconDescription = "icon description",
                    OfferIconTitle = "icon title",
                    OfferMaxQuantity = 1,
                    OfferTemplateId = 6
                }
            });
            var generatorLogger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var gen = new TemplatedOfferGenerator(generatorLogger, mockOfferData.Object);
            var options = new DbContextOptionsBuilder<GameDbContext>()
               .UseInMemoryDatabase(databaseName: "ReturnErrorCode_WhenIgnoreWarningsIsFalse_AndMissingParametersRaiseWarnings")
               .Options;
            ISandbox2Context sb2 = new GameDbContext(options);
            var generateLogger = TestHelpers.CreateLogger<GenerateHandler>();
            var sut = new GenerateHandler(generateLogger, mockOfferData.Object, sb2, gen);
            var opts = new GenerateOptions
            {
                OfferCode = "TestCode2",
                TemplateId = 1,
                Parameters = new List<string> { "param1=valid param", "param 2=invalid" },
                IgnoreWarnings = false };

            var result = sut.Execute(opts);

            result.ShouldBe(-1);
        }

        [Fact]
        public void NotSaveOffer_WhenIgnoreWarningsIsFalse_AndMissingParametersRaiseWarnings()
        {
            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Templates).Returns(new List<OfferTemplate>
            {
                new OfferTemplate
                {
                    Id = 1,
                    OfferTitle = "title",
                    OfferDescription = "description with %paramcount% parameters",
                    OfferContent = "{ \"skus\": { \"gold\": 0 } }",
                    OfferCost = 0,
                    OfferCostSku = "gold",
                    OfferFullCost = -1,
                    OfferDisplay = "[{}]",
                    OfferDuration = 1000,
                    OfferIconDescription = "icon description",
                    OfferIconTitle = "icon title",
                    OfferMaxQuantity = 1,
                    OfferTemplateId = 6
                }
            });
            var generatorLogger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var gen = new TemplatedOfferGenerator(generatorLogger, mockOfferData.Object);
            var options = new DbContextOptionsBuilder<GameDbContext>()
               .UseInMemoryDatabase(databaseName: "NotSaveOffer_WhenIgnoreWarningsIsFalse_AndMissingParametersRaiseWarnings")
               .Options;
            ISandbox2Context sb2 = new GameDbContext(options);
            var generateLogger = TestHelpers.CreateLogger<GenerateHandler>();
            var sut = new GenerateHandler(generateLogger, mockOfferData.Object, sb2, gen);
            var opts = new GenerateOptions
            {
                OfferCode = "TestCode3",
                Parameters = new List<string>
                {
                    "param1=valid param",
                    "param 2=invalid"
                },
                IgnoreWarnings = false
            };

            var result = sut.Execute(opts);

            sb2.Offers.Any(o => o.OfferCode == "TestCode3").ShouldBeFalse();
        }

        [Fact]
        public void AddOfferToDatabase_WhenSimpleOptionsAreValid()
        {
            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Templates).Returns(new List<OfferTemplate>
            {
                new OfferTemplate
                {
                    Id = 1,
                    OfferTitle = "title",
                    OfferDescription = "description with no parameters",
                    OfferContent = "{ \"skus\": { \"gold\": 0 } }",
                    OfferCost = 0,
                    OfferCostSku = "gold",
                    OfferFullCost = -1,
                    OfferDisplay = "[{}]",
                    OfferDuration = 1000,
                    OfferIconDescription = "icon description",
                    OfferIconTitle = "icon title",
                    OfferMaxQuantity = 1,
                    OfferTemplateId = 6
                }
            });
            var generatorLogger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var gen = new TemplatedOfferGenerator(generatorLogger, mockOfferData.Object);
            var options = new DbContextOptionsBuilder<GameDbContext>()
               .UseInMemoryDatabase(databaseName: "dbAbc")
               .Options;
            ISandbox2Context sb2 = new GameDbContext(options);
            var generateLogger = TestHelpers.CreateLogger<GenerateHandler>();
            var sut = new GenerateHandler(generateLogger, mockOfferData.Object, sb2, gen);
            var opts = new GenerateOptions
            {
                OfferCode = "TestCode2",
                TemplateId = 1,
                IgnoreWarnings = false
            };

            var result = sut.Execute(opts);

            sb2.Offers.Any(o => o.OfferCode == "TestCode2").ShouldBeTrue();
        }

        [Fact]
        public void CreateOffer_WithSubstitutedValues_WhenOptionsAreValid()
        {
            var mockOfferData = new Mock<IOfferData>();
            mockOfferData.Setup(o => o.Templates).Returns(new List<OfferTemplate>
            {
                new OfferTemplate
                {
                    Id = 1,
                    OfferTitle = "title",
                    OfferDescription = "description with no parameters",
                    OfferContent = "{ \"skus\": { \"gold\": 0 } }",
                    OfferCost = 0,
                    OfferCostSku = "gold",
                    OfferFullCost = -1,
                    OfferDisplay = "[{}]",
                    OfferDuration = 1000,
                    OfferIconDescription = "icon description",
                    OfferIconTitle = "icon title",
                    OfferMaxQuantity = 1,
                    OfferTemplateId = 6
                }
            });
            var generatorLogger = TestHelpers.CreateLogger<TemplatedOfferGenerator>();
            var gen = new TemplatedOfferGenerator(generatorLogger, mockOfferData.Object);
            var options = new DbContextOptionsBuilder<GameDbContext>()
               .UseInMemoryDatabase(databaseName: "dbAbc")
               .Options;
            ISandbox2Context sb2 = new GameDbContext(options);
            var generateLogger = TestHelpers.CreateLogger<GenerateHandler>();
            var sut = new GenerateHandler(generateLogger, mockOfferData.Object, sb2, gen);
            var opts = new GenerateOptions
            {
                OfferCode = "TestCode2",
                TemplateId = 1,
                IgnoreWarnings = false,
                Cost = 500,
                FullCost = 5200,
                MaxQuantity = 14,
                Prerequisite = "TestPrereq"
            };

            sut.Execute(opts);
            var result = sb2.Offers.First(o => o.OfferCode == "TestCode2");

            result.Cost.ShouldBe(500);
            result.FullCost.ShouldBe(5200);
            result.MaxQuantity.ShouldBe(14);
            result.Prerequisite.ShouldBe("TestPrereq");
        }
    }
}
