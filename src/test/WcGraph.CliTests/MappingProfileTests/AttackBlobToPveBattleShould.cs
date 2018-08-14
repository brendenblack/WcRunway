using AutoMapper;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Models;
using WcData.Snowflake.Models.Attack;
using WcGraph.Models;
using Xunit;

namespace WcGraph.CliTests.MappingProfileTests
{
    public class AttackBlobToPveBattleShould : IClassFixture<MappingProfileFixture>
    {
        public AttackBlobToPveBattleShould(MappingProfileFixture fixture)
        {
            blob = fixture.Blob;
            mapper = fixture.mapper;

            sut = mapper.Map<PveBattle>(blob);
        }

        private readonly AttackBlob blob;
        private readonly IMapper mapper;
        private readonly PveBattle sut;

        [Fact]
        public void MapId()
        {
            var blob = new AttackBlob
            {
                AttackId = "4a117749-4ac2-4c86-b97a-eec99ee1c3af"
            };

            var result = mapper.Map<PveBattle>(blob);

            result.Id.ShouldBe("4a117749-4ac2-4c86-b97a-eec99ee1c3af");
        }

        [Fact]
        public void MapTarget()
        {
            sut.Target.ShouldNotBeNull();
        }

        [Fact]
        public void MapTimestamp()
        {
            sut.Timestamp.ShouldBe(DateTimeOffset.FromUnixTimeSeconds(1533759906).ToOffset(TimeSpan.FromHours(-7)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void MapRubiSessions(int sessions)
        {
            var blob = new AttackBlob
            {
                AttackerRubiSessions = sessions
            };

            var result = mapper.Map<PveBattle>(blob);

            result.RubiSessions.ShouldBe(sessions);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(200)]
        [InlineData(29375357)]
        public void MapRubiDuration(int duration)
        {
            var blob = new AttackBlob
            {
                AttackerRubiDuration = duration
            };

            var result = mapper.Map<PveBattle>(blob);

            result.RubiDuration.ShouldBe(duration);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(200)]
        [InlineData(29375357)]
        public void MapDuration(int duration)
        {
            var blob = new AttackBlob
            {
                BattleDuration = duration
            };

            var result = mapper.Map<PveBattle>(blob);

            result.Duration.ShouldBe(duration);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(200)]
        public void MapMissilesUsed(int missilesUsed)
        {
            var blob = new AttackBlob
            {
                MissilesUsed = missilesUsed
            };

            var result = mapper.Map<PveBattle>(blob);

            result.MissilesUsed.ShouldBe(missilesUsed);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(200)]
        public void MapMisslesShotDown(int missilesShotDown)
        {
            var blob = new AttackBlob
            {
                MissilesShotDown = missilesShotDown
            };

            var result = mapper.Map<PveBattle>(blob);

            result.MissilesShotDown.ShouldBe(missilesShotDown);
        }
    }
}
