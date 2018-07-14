using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain;

namespace WcRunway.Core.Infrastructure.Data.Game
{
    public class GameContext : IGameContext
    {
        private readonly IUnitData unitData;

        public GameContext(IUnitData unitData)
        {
            this.unitData = unitData;
        }

        IEnumerable<Unit> IGameContext.Units => this.unitData.Units;
    }
}
