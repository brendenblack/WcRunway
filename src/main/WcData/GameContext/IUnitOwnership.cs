using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.GameContext
{
    public interface IUnitOwnership
    {
        /// <summary>
        /// Designates what is considered a recent user, in days; e.g. a value of 90 will limit all results to users active within the past 90 days.
        /// </summary>
        int RecentDays { get; set; }

        /// <summary>
        /// Fetches the ownership spread of a given unit for recent users, returning a dictionary of users that own the unit and at what level
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        Dictionary<int, int> FetchUnitOwnership(int unitId);

        /// <summary>
        /// Fetches a list of recent user IDs that own the specified unit
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        List<int> FetchUnitOwnerUserIds(int unitId);

        /// <summary>
        /// Fetches a list of recent user IDs that own the specified unit within a given level range
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="fromLevel"></param>
        /// <param name="toLevel"></param>
        /// <returns></returns>
        List<int> FetchUnitOwnerUserIds(int unitId, int fromLevel, int toLevel);

        /// <summary>
        /// Fetches a list of recent user IDs that do not own the specified unit
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        List<int> FetchUnitNonOwnerUserIds(int unitId);
    }
}
