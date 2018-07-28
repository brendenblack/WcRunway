using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Cli.Features.Generate;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Game;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;
using Xunit;

namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
{
    public class Execute_Should
    {
        // ILogger<GenerateHandler> log, IGameContext gameContext, UniqueOfferGenerator gen, Sandbox2Context sb2

        // ILogger<UniqueOfferGenerator> log, IOfferData offerData)
        public Execute_Should()
        {
            var offerGeneratorLogger = TestHelpers.CreateLogger<UniqueOfferGenerator>();
            var mockOfferData = new Mock<IOfferData>();
            var skeletonJuggernaut = new OfferSkeleton()
            {
                UnitId = 217,
                Title = "The Juggernaut",
                Description = "Unlock the Juggernaut",
                IconTitle = "Juggernaut unlock",
                IconDescription = "Offer includes a Juggernaut unlock",
                Cost = 99,
                FullCost = 1000,
                CostSku = "gold"
            };
            mockOfferData.Setup(od => od.Skeletons).Returns(new List<OfferSkeleton> { skeletonJuggernaut });

            // TODO: setup offer data

            var uniqueOfferGenerator = new UniqueOfferGenerator(offerGeneratorLogger, mockOfferData.Object);

            var logger = TestHelpers.CreateLogger<GenerateHandler>();
            var mockGameContext = new Mock<IGameContext>();
            var phalanx = new Unit(251) { Name = "Phalanx" };
            var juggernaut = new Unit(217) { Name = "Juggernaut" };
            mockGameContext.Setup(gc => gc.Units).Returns(new List<Unit> { phalanx, juggernaut });

            var options = new DbContextOptionsBuilder<Sandbox2Context>()
                .UseInMemoryDatabase(databaseName: "GenerateHandlerExecute")
                .Options;
            var sb2 = new Sandbox2Context(options);
            sb2.Database.EnsureCreated();

            this.sut = new GenerateHandler(logger, mockGameContext.Object, uniqueOfferGenerator, sb2);
        }

        private readonly GenerateHandler sut;

        [Fact]
        public void ThrowExceptionWhenUnitIdNotFound()
        {
            var opts = new GenerateOptions();
            opts.UnitId = 101;

            Should.Throw<ArgumentException>(() => sut.Execute(opts));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ThrowExceptionWhenPrefixInvalid(string prefix)
        {
            var opts = new GenerateOptions
            {
                UnitId = 217,
                OfferCodePrefix = prefix
            };

            Should.Throw<ArgumentException>(() => sut.Execute(opts));
        }

        #region Unlock tests
        [Fact]
        public void AddOfferToDatabase()
        {
            var opts = new GenerateOptions()
            {
                UnitId = 217,
                OfferCodePrefix = "Jul18Test"
            };

        }
        #endregion
    }
}
