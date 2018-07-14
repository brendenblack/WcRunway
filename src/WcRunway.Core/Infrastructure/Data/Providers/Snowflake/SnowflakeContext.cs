using Snowflake.Data.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace WcRunway.Core.Infrastructure.Data.Providers.Snowflake
{
    public class SnowflakeContext
    {
        private readonly string connectionString;
        private readonly static string WAREHOUSE_WC = "WC";
        private readonly static string SCHEMA_MYSQL = "MYSQL";
        private readonly static string SCHEMA_PUBLIC = "PUBLIC";

        private readonly static string TABLE_USER_ACADEMY = $"{WAREHOUSE_WC}.{SCHEMA_MYSQL}.USER_ACADEMY_WC";
        private readonly static string TABLE_USERS = $"{WAREHOUSE_WC}.{SCHEMA_MYSQL}.USERS_WC";

        public SnowflakeContext(string account, string user, string password, string db = "", string schema = "")
        {
            this.connectionString = $"account={account};user={user};password={password}";

            if (!string.IsNullOrWhiteSpace(db))
            {
                this.connectionString += $";db={db}";
            }

            if (!string.IsNullOrWhiteSpace(schema))
            {
                this.connectionString += $"schema={schema}";
            }
        }

        public Dictionary<int, int> GetUnitOwnership(int unitId)
        {
            var ownership = new Dictionary<int, int>();
            using (IDbConnection conn = new SnowflakeDbConnection())
            {
                conn.ConnectionString = this.connectionString;
                conn.Open();

                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"select ua.USERID, ua.LEVEL from {TABLE_USER_ACADEMY} ua join {TABLE_USERS} u on u.USERID = ua.USERID where u.SEENTIME >= DATE_PART('epoch_second', dateadd('day', -30, current_date())) and ua.type = {unitId};";
                IDataReader reader = cmd.ExecuteReader();
                var records = 0;
                try
                {
                    while (reader.Read())
                    {
                        records++;
                        var userId = reader.GetInt32(0);
                        var level = reader.GetInt32(1);
                        if (records % 10 == 0)
                        {
                            Console.WriteLine("[{0}] Adding user {1} at level {2}", records, userId, level);
                        }
                        ownership.Add(userId, level);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception occurred at record {0}", records);
                }

            }
            return ownership;
        }

    }
}
