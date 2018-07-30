using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Infrastructure.Data.Providers.Snowflake
{
    public class SnowflakeConnectionDetails
    {
        public SnowflakeConnectionDetails(string account, string username, string password, string db = "", string schema = "")
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Account name, username and password must all be provided");
            }

            this.Account = account;
            this.Username = username;
            this.Password = password;
            this.Database = db;
            this.Schema = schema;
            
            var conn = $"account={account};user={username};password={password}";

            if (!string.IsNullOrWhiteSpace(db))
            {
                conn += $";db={db}";
            }

            if (!string.IsNullOrWhiteSpace(schema))
            {
                conn += $"schema={schema}";
            }

            this.ConnectionString = conn;
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Account { get; private set; }
        public string ConnectionString { get; private set; }

        public string Database { get; private set; }
        public string Schema { get; private set; }
    }
}
