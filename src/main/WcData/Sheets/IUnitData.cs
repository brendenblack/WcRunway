using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain;

namespace WcData.Sheets
{
    public interface IUnitData
    {
        IEnumerable<Unit> Units { get; }
    }
}
