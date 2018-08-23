using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Models.Attack;
using WcGraph.Models;

namespace WcGraph.Cli
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<WcData.Snowflake.Models.User, User>();

            CreateMap<WcData.GameContext.Models.User, User>();
           
            CreateMap<AttackBlob, Base>()
                .ForMember(b => b.Level, o => o.MapFrom(a => a.DefenderLevel))
                .ForMember(b => b.Type, o => o.MapFrom(a => a.AttackLocation));

            CreateMap<AttackBlob, BaseInstance>()
                .ForMember(b => b.Sector, o => o.MapFrom(s => s.Sector))
                .ForMember(b => b.XCoordinate, o => o.MapFrom(s => s.DefenderX))
                .ForMember(b => b.YCoordinate, o => o.MapFrom(s => s.DefenderY))
                .ForMember(b => b.Base, o => o.MapFrom(s => s));

            CreateMap<AttackBlob, PveBattle>()
                .ForMember(b => b.Id, o => o.MapFrom(s => s.AttackId))
                .ForMember(b => b.AttackerId, o => o.MapFrom(s => s.UserId))
                .ForMember(b => b.Target, o => o.MapFrom(s => s))
                .ForMember(b => b.Timestamp, o => o.MapFrom(s => s.RxTs))
                .ForMember(b => b.RubiDuration, o => o.MapFrom(s => s.AttackerRubiDuration))
                .ForMember(b => b.RubiSessions, o => o.MapFrom(s => s.AttackerRubiSessions))
                .ForMember(b => b.Duration, o => o.MapFrom(s => s.BattleDuration))
                .ForMember(b => b.DamageReceived, o => o.MapFrom(s => s.DamageToAttacker))
                .ForMember(b => b.DamageDealt, o => o.MapFrom(s => s.DamageToDefender))
                .ForMember(b => b.MissilesUsed, o => o.MapFrom(s => s.MissilesUsed))
                .ForMember(b => b.MissilesShotDown, o => o.MapFrom(s => s.MissilesShotDown));
        }
    }
}
