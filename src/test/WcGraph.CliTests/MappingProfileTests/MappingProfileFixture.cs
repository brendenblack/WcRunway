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
                EnemyType = "test_enemy",
                AttackLocation = "test_enemy",
                AttackerRubiSessions = 2,
                AttackerRubiDuration = 12345,
                BattleDuration = 123456,
                RxTs = DateTimeOffset.FromUnixTimeSeconds(1533759906).ToOffset(TimeSpan.FromHours(-7))
            };

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            mapper = new Mapper(mapperConfig);
        }


        public IMapper mapper { get; private set; }

        public AttackBlob Blob { get; private set; }

        public void Dispose()
        {
        }
    }
}
