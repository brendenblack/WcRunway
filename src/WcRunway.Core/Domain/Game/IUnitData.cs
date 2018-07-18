using System.Collections.Generic;

namespace WcRunway.Core.Domain.Game
{
    public interface IUnitData
    {
        IEnumerable<Unit> Units { get; }
    }
}
