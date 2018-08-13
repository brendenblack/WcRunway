using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Models.Attack;
using Xunit;

namespace WcGraph.CliTests.MappingProfileTests
{
    public class AttackBlobToPveAttackShould : IClassFixture<MappingProfileFixture>
    {
        public AttackBlobToPveAttackShould(MappingProfileFixture fixture)
        {
            blob = fixture.Blob;
            mapper = fixture.mapper;
        }

        private readonly AttackBlob blob;
        private readonly IMapper mapper;
    }
}
