using System.Collections.Generic;

namespace WcCore.Domain.Game
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
