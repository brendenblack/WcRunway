using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Models.Attack;
using WcGraph.Cli;
using Xunit;

namespace WcGraph.CliTests.MappingProfileTests
{
    public class MappingProfileFixture : IDisposable
    {
        public MappingProfileFixture()
        {
            Blob = new AttackBlob
            {
                DefenderLevel = 100,
                EnemyType = "test_enemy"
            };

            Mapper.Initialize(cfg => cfg.AddProfile(new MappingProfile()));
            mapper = Mapper.Instance;
        }


        public IMapper mapper { get; private set; }

        public AttackBlob Blob { get; private set; }

        public void Dispose()
        {
        }
    }
}
