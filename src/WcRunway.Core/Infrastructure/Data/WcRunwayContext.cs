using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Game;

namespace WcRunway.Core.Infrastructure.Data
{
    public class WcRunwayContext
    {
        public WcRunwayContext(IUnitData units)
        {
            this.Units = units;
        }

        public IUnitData Units { get; private set; }
    }
}
