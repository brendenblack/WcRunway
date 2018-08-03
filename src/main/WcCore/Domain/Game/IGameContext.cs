using System.Collections.Generic;

namespace WcCore.Domain.Game
{
    public interface IGameContext
    {
        IEnumerable<Unit> Units { get; }
    }
}
