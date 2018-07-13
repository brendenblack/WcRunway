using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Units;

namespace WcRunway.Core.Infrastructure.Data
{
    public class GameBible : IGameBible
    {
        public GameBible(IUnitRepository unitRepository)
        {
            this.Units = unitRepository;
        }

        public IUnitRepository Units { get; }
    }
}
