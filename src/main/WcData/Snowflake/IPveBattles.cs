using System;
using System.Collections.Generic;
using System.Text;
using WcData.Snowflake.Models;
using WcData.Snowflake.Models.Attack;

namespace WcData.Snowflake
{
    public interface IPveBattles
    {
        IEnumerable<PveAttack> FetchAttacks(string baseType, int baseLevel);

        IEnumerable<AttackBlob> FetchAttacksByUser(int userId, DateTimeOffset from, DateTimeOffset to);
    }
}
