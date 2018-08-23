using System;
using System.Collections.Generic;
using System.Text;
using WcData.Sheets.Models;

namespace WcData.Sheets
{
    public interface IGameData
    {
        IEnumerable<Unit> Units { get; }
    }
}
