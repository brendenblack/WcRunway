using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WcRunway.Cli.Features.Generate;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Game;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;
using Xunit;

namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
{
    public class Execute_Should : IClassFixture<GenerateHandlerFixture>
    {
        public Execute_Should(GenerateHandlerFixture fixture)
        {
            this.fixture = fixture;
            this.sut = fixture.Handler;
        }

        private readonly GenerateHandlerFixture fixture;
        private readonly GenerateHandler sut;

        [Fact]
        public void ThrowExceptionWhenUnitIdNotFound()
        {
            var opts = new GenerateOptions();
            opts.UnitId = 101;

            Should.Throw<ArgumentException>(() => sut.Execute(opts));
        }

        //[Theory]
        //[InlineData(null)]
        //[InlineData("")]
        //public void ThrowExceptionWhenPrefixInvalid(string prefix)
        //{
        //    var opts = new GenerateOptions
        //    {
        //        UnitId = 217,
        //        OfferCodePrefix = prefix
        //    };

        //    Should.Throw<ArgumentException>(() => sut.Execute(opts));
        //}

        //[Fact]
        //public void ThrowExceptWhenOfferCodeExists()
        //{
        //    var opts = new GenerateOptions
        //    {
        //        UnitId = 217,
        //        OfferCodePrefix = "Jul18Exists"
        //    };

        //    this.sut.Execute(opts);

        //    Should.Throw<InvalidOperationException>(() => this.sut.Execute(opts));
        //}

        #region Unlock tests
        [Fact]
        public void Return0WhenSuccessful()
        {
            var opts = new GenerateOptions()
            {
                UnitId = 217,
                OfferCodePrefix = "Jul18Test"
            };

            var result = this.sut.Execute(opts);

            result.ShouldBe(0);
        }


        [Fact]
        public void AddUnlockOfferWithExpectedOfferCodeToDatabase()
        {
            var opts = new GenerateOptions()
            {
                UnitId = 217,
                OfferCodePrefix = "Test123",
                IncludeEliteParts = false,
                IncludeLevels = false,
                IncludeOmegaParts = false,
                IncludeTech = false
            };

            this.sut.Execute(opts);
            var offer = this.fixture.Sandbox2.Offers.FirstOrDefaultAsync(o => o.OfferCode == "Test123Unl");

            offer.ShouldNotBeNull();
        }

        [Fact]
        public void CreateUnlockCohortCsvInDefaultDirectory()
        {
            var opts = new GenerateOptions()
            {
                UnitId = 217,
                OfferCodePrefix = "Jul18Test"
            };
            var csv = Path.Combine(Environment.CurrentDirectory, "Jul18Test", "Jul18TestUnl.csv");

            this.sut.Execute(opts);

            File.Exists(csv).ShouldBe(true);
        }
        #endregion
    }
}
