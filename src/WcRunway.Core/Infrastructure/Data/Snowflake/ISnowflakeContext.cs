using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Infrastructure.Data.Snowflake
{
    public interface ISnowflakeContext
    {
        /// <summary>
        /// Get a list of active spender IDs
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> SpenderIds();

        /// <summary>
        /// Get a list of active spender IDs along with their lifetime value
        /// </summary>
        /// <returns></returns>
        Dictionary<int, double> Spenders();

        /// <summary>
        /// Get a dictionary of users that own the given unit, and at what level.
        /// The number of keys present represents the total number of owners for the unit.
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns>A dictionary where the keys are User Ids and the values represent the level they are at.</returns>
        Dictionary<int, int> GetUnitOwnership(int unitId);
    }
}
