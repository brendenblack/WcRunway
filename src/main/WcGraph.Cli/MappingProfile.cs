using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.Models;

namespace WcGraph.Cli
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WcCore.Domain.User, WcGraph.Models.User>();

            CreateMap<WcData.Snowflake.Models.Attack.AttackBlob, Base>()
                .ForMember(b => b.Level, o => o.MapFrom(a => a.DefenderLevel))
                .ForMember(b => b.Type, o => o.MapFrom(a => a.EnemyType));

            CreateMap<WcData.Snowflake.Models.Attack.AttackBlob, BaseInstance>()
                .ForMember(b => b.Sector, o => o.MapFrom(s => s.Sector))
                .ForMember(b => b.XCoordinate, o => o.MapFrom(s => s.DefenderX))
                .ForMember(b => b.YCoordinate, o => o.MapFrom(s => s.DefenderY))
                .ForMember(b => b.Base, o => o.MapFrom(s => s));


            CreateMap<WcData.Snowflake.Models.Attack.AttackBlob, PveBattle>()
                .ForMember(b => b.Target, o => o.MapFrom(s => s));
        }
    }
}
