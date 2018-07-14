using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain;

namespace WcRunway.Core.Infrastructure.Data.Game
{
    public interface IGameContext
    {
        IEnumerable<Unit> Units { get; }
    }
}
