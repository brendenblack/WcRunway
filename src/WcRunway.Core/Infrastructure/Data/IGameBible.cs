using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain;
using WcRunway.Core.Infrastructure.Data.Units;

namespace WcRunway.Core.Infrastructure.Data
{
    public interface IGameBible
    {
        IUnitRepository Units { get; }
    }
}
