using System.Collections.Generic;

namespace WcCore.Domain.Game
{
    public interface IUnitData
    {
        IEnumerable<Unit> Units { get; }
    }
}
