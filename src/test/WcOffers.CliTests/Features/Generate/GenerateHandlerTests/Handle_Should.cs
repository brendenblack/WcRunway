using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.GameContext;
using WcData.GameContext.Implementation;
using WcData.GameContext.Models;
using WcData.Sheets;
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
        public void ReturnErrorCodeWhenOfferCodeIsNullOrWhitespace()
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
        public void ReturnErrorCodeWhenOfferCodeIsInUse()
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
    }
}
