//using Snowflake.Data.Client;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;

//namespace WcCore.Infrastructure.Data.Users
//{
//    public class SnowflakeUserRepository
//    {
//        private readonly string connectionString;

//        public SnowflakeUserRepository(string account, string user, string password, string db = "", string schema = "")
//        {
//            this.connectionString = $"account={account};user={user};password={password}";

//            if (!string.IsNullOrWhiteSpace(db))
//            {
//                this.connectionString += $";db={db}";
//            }

//            if (!string.IsNullOrWhiteSpace(schema))
//            {
//                this.connectionString += $"schema={schema}";
//            }
//        }

//        public List<User> GetAllActive()
//        {
//            List<User> users = new List<User>();
//            using (IDbConnection conn = new SnowflakeDbConnection())
//            {
//                conn.ConnectionString = this.connectionString;
//                conn.Open();

//                IDbCommand cmd = conn.CreateCommand();
//                cmd.CommandText = "select USERID, ADDTIME, SEENTIME, EMAIL, TIMEPLAYED, COUNTRY, GENDER from wc.mysql.users_wc where seentime >= DATE_PART('epoch_second', dateadd('day', -30, current_date())) limit 70;";
//                IDataReader reader = cmd.ExecuteReader();

//                while (reader.Read())
//                {
//                    User user = new User();
//                    user.Id = reader.GetInt32(0);
//                    user.EmailAddress = reader.GetString(3);

//                    user.AddTimeEpochSeconds = reader.GetInt32(1);
//                    user.LastSeenEpochSeconds = reader.GetInt32(2);
//                    //user.TimePlayed = new TimeSpan(reader.GetInt32(4));

//                    user.Country = reader.GetString(5) ?? "unknown";
//                    user.Gender = reader.GetString(6) ?? "unknown";

//                    users.Add(user);
//                }

//                conn.Close();
//            }

//            return users;
//        }

//        public User GetUser(int id)
//        {
//            User user = null;
//            using (IDbConnection conn = new SnowflakeDbConnection())
//            {
//                conn.ConnectionString = this.connectionString;
//                conn.Open();

//                IDbCommand cmd = conn.CreateCommand();
//                cmd.CommandText = $"select USERID, ADDTIME, SEENTIME, EMAIL, TIMEPLAYED, COUNTRY, GENDER from wc.mysql.users_wc where USERID = {id};";
//                IDataReader reader = cmd.ExecuteReader();

//                while (reader.Read())
//                {
//                    user = new User();
//                    user.Id = reader.GetInt32(0);
//                    user.EmailAddress = reader.GetString(3);

//                    user.AddTimeEpochSeconds = reader.GetInt32(1);
//                    user.LastSeenEpochSeconds = reader.GetInt32(2);
//                    //user.TimePlayed = new TimeSpan(reader.GetInt32(4));

//                    user.Country = reader.GetString(5) ?? "unknown";
//                    user.Gender = reader.GetString(6) ?? "unknown";
//                }

//                conn.Close();
//            }

//            return user;
//        }

//    }
//}
