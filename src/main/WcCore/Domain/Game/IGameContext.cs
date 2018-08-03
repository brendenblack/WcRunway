using System.Collections.Generic;

namespace WcRunway.Core.Domain.Game
{
    public interface IGameContext
    {
        IEnumerable<Unit> Units { get; }
    }
}
