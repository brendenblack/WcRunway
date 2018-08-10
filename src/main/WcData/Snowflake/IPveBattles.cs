using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Models;

namespace WcData.Snowflake
{
    public interface IPveBattles
    {
        IEnumerable<PveAttack> FetchAttacks(string baseType, int baseLevel);

        IEnumerable<PveAttack> FetchAttacksByUser(int userId, DateTimeOffset from, DateTimeOffset to);
    }
}
