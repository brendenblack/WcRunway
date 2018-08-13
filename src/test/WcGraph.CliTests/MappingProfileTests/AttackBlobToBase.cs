using AutoMapper;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Models.Attack;
using WcGraph.Models;
using Xunit;

namespace WcGraph.CliTests.MappingProfileTests
{
    public class AttackBlobToBase : IClassFixture<MappingProfileFixture>
    {
        public AttackBlobToBase(MappingProfileFixture fixture)
        {
            blob = fixture.Blob;
            mapper = fixture.mapper;
        }

        private readonly AttackBlob blob;
        private readonly IMapper mapper;

        [Fact]
        public void MapSector()
        {
            var result = mapper.Map<BaseInstance>(blob);

            result.Sector.ShouldBe(blob.Sector);
        }

        [Fact]
        public void MapXCoord()
        {
            var result = mapper.Map<BaseInstance>(blob);

            result.XCoordinate.ShouldBe(blob.DefenderX);
        }

        [Fact]
        public void MapYCoord()
        {
            var result = mapper.Map<BaseInstance>(blob);

            result.YCoordinate.ShouldBe(blob.DefenderY);
        }

        [Fact]
        public void MapBase()
        {
            var result = mapper.Map<BaseInstance>(blob);

            result.Base.ShouldNotBeNull();
        }

    }
}
