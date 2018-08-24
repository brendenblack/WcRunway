using AutoMapper;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Models.Attack;
using WcGraph.Cli;
using WcGraph.Models;
using Xunit;

namespace WcGraph.CliTests.MappingProfileTests
{
    public class AttackBlobToBase_Should : IClassFixture<MappingProfileFixture>
    {
        public AttackBlobToBase_Should(MappingProfileFixture fixture)
        {
            blob = fixture.Blob;
            mapper = fixture.mapper;
        }

        private readonly AttackBlob blob;
        private readonly IMapper mapper;

        [Fact]
        public void MapLevel()
        {
            var result = mapper.Map<Base>(blob);

            result.Level.ShouldBe(blob.DefenderLevel);
        }

        [Fact]
        public void MapType()
        {
            var result = mapper.Map<Base>(blob);

            result.Type.ShouldBe(blob.EnemyType);
        }
    }
}
