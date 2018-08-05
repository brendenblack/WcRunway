using System;
using System.Collections.Generic;
using System.Text;
using WcCore.Domain;

namespace WcData.Sheets
{
    public interface IGameData
    {
        IEnumerable<Unit> Units { get; }
    }
}
