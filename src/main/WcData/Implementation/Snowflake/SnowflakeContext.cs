using Microsoft.Extensions.Logging;
using Snowflake.Data.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WcData.Snowflake;
using WcData.Snowflake.Models;

namespace WcData.Implementation.Snowflake
{
    public class SnowflakeContext : IPveBattles
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

        public IEnumerable<PveAttack> FetchAttacksByUser(int userId, DateTimeOffset from, DateTimeOffset to)
        {
            var attacks = new List<PveAttack>();
            var query = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("    SRC:attack_id,")
                .AppendLine("    SRC:sector,")
                .AppendLine("    SRC:attack_location,") // weird name, but this is the type/category of base
                .AppendLine("    SRC:attacker_x,")
                .AppendLine("    SRC:attacker_y,")
                .AppendLine("    SRC:defender_x,")
                .AppendLine("    SRC:defender_y,")
                .AppendLine("    SRC:damage_to_attacker,")
                .AppendLine("    SRC:damage_to_defender,")
                .AppendLine("    SRC:missiles_used,")
                .AppendLine("    SRC:missiles_shot_down,")
                .AppendLine("    SRC:defender_level,")
                .AppendLine("    SRC:defender_location,")
                .AppendLine("  FROM wc.public.raw_para_json")
                .AppendLine("  WHERE game_id = 'WC'")
                .AppendLine("    AND env = 'prod'")
                .AppendLine("    AND game_user_id = (?)")
                .AppendLine("    AND dt_pst BETWEEN '(?)' AND '(?)'")
                .AppendLine("    AND tag = attack")
                .AppendLine("    AND SRC:enemy_type = 'PVE';");

            using (IDbConnection conn = new SnowflakeDbConnection())
            {
                conn.ConnectionString = this.connection.ConnectionString;
                conn.Open();

                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query.ToString();

                var userIdParameter = cmd.CreateParameter();
                userIdParameter.ParameterName = "1";
                userIdParameter.Value = userId;
                userIdParameter.DbType = DbType.Int32;
                cmd.Parameters.Add(userIdParameter);

                var fromParameter = cmd.CreateParameter();
                fromParameter.ParameterName = "2";
                fromParameter.Value = from.ToString("yyyy-mm-dd");
                fromParameter.DbType = DbType.Date;
                cmd.Parameters.Add(fromParameter);

                var toParameter = cmd.CreateParameter();
                toParameter.ParameterName = "2";
                toParameter.Value = to.ToString("yyyy-mm-dd");
                toParameter.DbType = DbType.Date;
                cmd.Parameters.Add(toParameter);

                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    log.LogTrace("Consuming line");

                    var sector = reader.GetInt32(1);
                    var attack = new PveAttack
                    {
                        Id = reader.GetString(0),
                        AttackerId = userId,
                        AttackerLocation = new Location(sector, reader.GetInt32(3), reader.GetInt32(4)),
                        DefenderLocation = new Location(sector, reader.GetInt32(5), reader.GetInt32(6)),
                        DamageToAttacker = reader.GetInt64(7),
                        DamageToDefender = reader.GetInt64(8),
                        MissilesUsed = reader.GetInt32(9),
                        MissilesShotDown = reader.GetInt32(10),
                        DefenderLevel = reader.GetInt32(11),
                        DefenderType = reader.GetString(12)
                    };
                    attacks.Add(attack);
                }
            }

            return attacks;
        }

        public IEnumerable<PveAttack> FetchAttacks(string baseType, int baseLevel)
        {
            throw new NotImplementedException();
        }



        //public IEnumerable<PveAttack> FetchAttacksByBase(string baseType, int baseLevel)
        //{
        //    using (IDbConnection conn = new SnowflakeDbConnection())
        //    {
        //        conn.ConnectionString = this.connection.ConnectionString;
        //        conn.Open();

        //        IDbCommand cmd = conn.CreateCommand();
        //        cmd.CommandText = $"select ua.USERID from {TABLE_USER_ACADEMY} ua join {TABLE_USERS} u on u.USERID = ua.USERID where u.SEENTIME >= DATE_PART('epoch_second', dateadd('day', -30, current_date())) and ua.type = {unitId};";
        //        IDataReader reader = cmd.ExecuteReader();
        //        var records = 0;
        //        try
        //        {
        //            while (reader.Read())
        //            {
        //                records++;
        //                var userId = reader.GetInt32(0);
        //                ownership.Add(userId);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            log.LogError("An exception occurred at record {0}", records);
        //        }

        //    }
        //}
    }
}
