using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Units;

namespace WcRunway.Core.Infrastructure.Data
{
    public class WcRunwayContext
    {
        public WcRunwayContext(IUnitRepository units)
        {
            this.Units = units;
        }

        public IUnitRepository Units { get; private set; }
    }
}
