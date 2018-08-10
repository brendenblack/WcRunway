using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Snowflake.Data.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WcCore.Domain.Battles;
using WcData.Implementation.Snowflake.Extensions;
using WcData.Snowflake;
using WcData.Snowflake.Models;
using WcData.Snowflake.Models.Attack;

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

        public IEnumerable<AttackBlob> FetchAttacksByUser(int userId, DateTimeOffset from, DateTimeOffset to)
        {
            var attacks = new List<AttackBlob>();
            var query = new StringBuilder()
                .AppendLine("SELECT SRC FROM wc.public.raw_para_json")
                .AppendLine("  WHERE game_id = 'WC'")
                .AppendLine("    AND env = 'prod'")
                .AppendLine("    AND game_user_id = (?)")
                //.AppendLine("    AND dt_pst BETWEEN (?) AND (?)")
                .AppendLine("    AND dt_pst BETWEEN '2018-08-07' AND '2018-08-09'")
                .AppendLine("    AND tag = 'attack'")
                .AppendLine("    AND SRC:enemy_type = 'PVE';");

            using (IDbConnection conn = new SnowflakeDbConnection())
            {
                log.LogDebug("Opening connection to Snowflake");
                conn.ConnectionString = this.connection.ConnectionString;
                conn.Open();

                log.LogDebug("Creating command");
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query.ToString();

                var userIdParameter = cmd.CreateParameter();
                userIdParameter.ParameterName = "1";
                userIdParameter.Value = userId;
                userIdParameter.DbType = DbType.Int32;
                cmd.Parameters.Add(userIdParameter);

                //var fromParameter = cmd.CreateParameter();
                //fromParameter.ParameterName = "2";
                //fromParameter.Value = from.Date.Date;
                //fromParameter.DbType = DbType.Date;
                //cmd.Parameters.Add(fromParameter);

                //var toParameter = cmd.CreateParameter();
                //toParameter.ParameterName = "3";
                //toParameter.Value = to.Date.Date; // to.ToString("yyyy-mm-dd");
                //toParameter.DbType = DbType.Date;
                //cmd.Parameters.Add(toParameter);

                log.LogDebug("Executing command");
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var attack = JsonConvert.DeserializeObject<AttackBlob>(reader.GetString(0));

                    attacks.Add(attack);
                }
            }
            return attacks;
        }

        public IEnumerable<PveAttack> FetchAttacksByUser2(int userId, DateTimeOffset from, DateTimeOffset to)
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
                .AppendLine("    parse_json(SRC:attacker_platoon_metadata) as platoon_positioning,")
                .AppendLine("    parse_json(SRC:units_deployed) as units_deployed")
                .AppendLine("    SRC:metal,")
                .AppendLine("    SRC:oil,")
                .AppendLine("    SRC:thorium,")
                .AppendLine("    CASE WHEN SRC:win = '1' THEN 'win' WHEN SRC:retreat = '1' THEN 'retreat' ELSE 'defeat' END as result")
                .AppendLine("  FROM wc.public.raw_para_json")
                .AppendLine("  WHERE game_id = 'WC'")
                .AppendLine("    AND env = 'prod'")
                .AppendLine("    AND game_user_id = (?)")
                //.AppendLine("    AND dt_pst BETWEEN (?) AND (?)")
                .AppendLine("    AND dt_pst BETWEEN '2018-08-07' AND '2018-08-09'")
                .AppendLine("    AND tag = 'attack'")
                .AppendLine("    AND SRC:enemy_type = 'PVE';");

            using (IDbConnection conn = new SnowflakeDbConnection())
            {
                log.LogDebug("Opening connection to Snowflake");
                conn.ConnectionString = this.connection.ConnectionString;
                conn.Open();

                log.LogDebug("Creating command");
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query.ToString();

                var userIdParameter = cmd.CreateParameter();
                userIdParameter.ParameterName = "1";
                userIdParameter.Value = userId;
                userIdParameter.DbType = DbType.Int32;
                cmd.Parameters.Add(userIdParameter);

                //var fromParameter = cmd.CreateParameter();
                //fromParameter.ParameterName = "2";
                //fromParameter.Value = from.Date.Date;
                //fromParameter.DbType = DbType.Date;
                //cmd.Parameters.Add(fromParameter);

                //var toParameter = cmd.CreateParameter();
                //toParameter.ParameterName = "3";
                //toParameter.Value = to.Date.Date; // to.ToString("yyyy-mm-dd");
                //toParameter.DbType = DbType.Date;
                //cmd.Parameters.Add(toParameter);

                log.LogDebug("Executing command");
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    log.LogTrace("Consuming line");

                    var sector = reader.ReadColumnAsInt(1);
                    var baseType = reader.ReadColumnAsString(2);
                    var enemyBase = PveBase.OfType(baseType)
                        .InSector(sector)
                        .AtCoordinates(reader.ReadColumnAsInt(5), reader.ReadColumnAsInt(6))
                        .AtLevel(reader.ReadColumnAsInt(11))
                        .Build();

                    
                    var attack = new PveAttack
                    {
                        Id = reader.ReadColumnAsString(0),
                        AttackerId = userId,
                        AttackerLocation = new Location(sector, reader.ReadColumnAsInt(3), reader.ReadColumnAsInt(4)),
                        DefenderLocation = new Location(sector, reader.ReadColumnAsInt(5), reader.ReadColumnAsInt(6)),
                        DamageToAttacker = reader.ReadColumnAsLong(7),
                        DamageToDefender = reader.ReadColumnAsLong(8),
                        MissilesUsed = reader.ReadColumnAsInt(9),
                        MissilesShotDown = reader.ReadColumnAsInt(10),
                        DefenderLevel = reader.ReadColumnAsInt(11),
                        DefenderType = reader.ReadColumnAsString(2),
                        MetalClaimed = reader.ReadColumnAsInt(15),
                        OilClaimed = reader.ReadColumnAsInt(16),
                        ThoriumClaimed = reader.ReadColumnAsInt(17)
                    };


                    var platoonMetaJson = reader.GetString(13); //.Replace("\n", "");
                    var platoonPositions = JsonConvert.DeserializeObject<List<PlatoonAttackStagingLocation>>(platoonMetaJson);
                    //attack.AttackingPlatoonLocations.AddRange(platoonPositions);

                    var unitsDeployedJson = reader.GetString(14);
                    var unitsDeployed = JsonConvert.DeserializeObject<List<DeployedUnit>>(unitsDeployedJson);
                    attack.UnitsDeployed.AddRange(unitsDeployed);


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
