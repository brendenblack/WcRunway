using WcRunway.Core.Domain.Game;

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
