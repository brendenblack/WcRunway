using Microsoft.Extensions.Logging;
using Snowflake.Data.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using WcRunway.Core.Domain.Users;

namespace WcRunway.Core.Infrastructure.Data.Providers.Snowflake
{
    public class SnowflakeContext : IUnitOwnership
    {
        private readonly ILogger<SnowflakeContext> log;
        private readonly SnowflakeConnectionDetails connection;
        private readonly static string WAREHOUSE_WC = "WC";
        private readonly static string SCHEMA_MYSQL = "MYSQL";
        private readonly static string SCHEMA_PUBLIC = "PUBLIC";

        private readonly static string TABLE_USER_ACADEMY = $"{WAREHOUSE_WC}.{SCHEMA_MYSQL}.USER_ACADEMY_WC";
        private readonly static string TABLE_USERS = $"{WAREHOUSE_WC}.{SCHEMA_MYSQL}.USERS_WC";

        private int _recent = 90;
        public int RecentDays
        {
            get
            {
                return _recent;
            }
            set
            {
                _recent = (value < 1) ? 90 : value;
            }
        }

        public SnowflakeContext(ILogger<SnowflakeContext> logger, SnowflakeConnectionDetails connection)
        {
            this.log = logger;
            this.connection = connection;
          
        }

        public Dictionary<int, int> FetchUnitOwnership(int unitId)
        {
            var ownership = new Dictionary<int, int>();
            using (IDbConnection conn = new SnowflakeDbConnection())
            {
                conn.ConnectionString = this.connection.ConnectionString;
                conn.Open();

                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"select ua.USERID, ua.LEVEL from {TABLE_USER_ACADEMY} ua join {TABLE_USERS} u on u.USERID = ua.USERID where u.SEENTIME >= DATE_PART('epoch_second', dateadd('day', -30, current_date())) and ua.type = {unitId};";
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                var records = 0;
                try
                {
                    while (reader.Read())
                    {
                        records++;
                        var userId = reader.GetInt32(0);
                        var level = reader.GetInt32(1);
                        log.LogDebug("Reading unit ownership, logging every 10th entry");
                        if (records % 10 == 0)
                        {
                            log.LogDebug("[{0}] Adding user {1} at level {2}", records, userId, level);
                        }
                        ownership.Add(userId, level);
                    }
                }
                catch (Exception e)
                {
                    log.LogError("An exception occurred at record {0}", records);
                }

            }
            return ownership;
        }

        public List<int> FetchUnitOwnerUserIds(int unitId)
        {
            var ownership = new List<int>();
            using (IDbConnection conn = new SnowflakeDbConnection())
            {
                conn.ConnectionString = this.connection.ConnectionString;
                conn.Open();

                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"select ua.USERID from {TABLE_USER_ACADEMY} ua join {TABLE_USERS} u on u.USERID = ua.USERID where u.SEENTIME >= DATE_PART('epoch_second', dateadd('day', -30, current_date())) and ua.type = {unitId};";
                IDataReader reader = cmd.ExecuteReader();
                var records = 0;
                try
                {
                    while (reader.Read())
                    {
                        records++;
                        var userId = reader.GetInt32(0);
                        ownership.Add(userId);
                    }
                }
                catch (Exception e)
                {
                    log.LogError("An exception occurred at record {0}", records);
                }

            }
            return ownership;
        }

        public List<int> FetchUnitOwnerUserIds(int unitId, int fromLevel, int toLevel)
        {
            throw new NotImplementedException();
        }

        public List<int> FetchUnitNonOwnerUserIds(int unitId)
        {
            throw new NotImplementedException();
        }
    }
}
