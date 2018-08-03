using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Microsoft.Extensions.DependencyInjection
{
    public class SnowflakeOptions
    {
        public string Account { get; set; } = "kixeye";

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string Database { get; set; }

        public string Schema { get; set; }
    }
}
