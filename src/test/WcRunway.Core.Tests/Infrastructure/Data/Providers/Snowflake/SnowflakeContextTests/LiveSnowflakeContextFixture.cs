using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Providers.Snowflake;

namespace WcRunway.Core.Tests.Infrastructure.Data.Providers.Snowflake.SnowflakeContextTests
{
    /// <summary>
    /// Utilizes a live connection to the actual Snowflake instance to perform integration tests<para/>
    /// Specify the connection details by adding the following environment variables:
    /// <list type="bullet">
    ///     <item>
    ///         <description>SNOWFLAKE_ACCOUNT</description>
    ///     </item>
    ///     <item>
    ///         <description>SNOWFLAKE_USERNAME</description>
    ///     </item>
    ///     <item>
    ///         <description>SNOWFLAKE_PASSWORD</description>
    ///     </item>
    ///     </list>
    /// </summary>
    public class LiveSnowflakeContextFixture : IDisposable
    {
        public LiveSnowflakeContextFixture()
        {
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }

            var account = Environment.GetEnvironmentVariable("SNOWFLAKE_ACCOUNT");
            var username = Environment.GetEnvironmentVariable("SNOWFLAKE_USERNAME");
            var password = Environment.GetEnvironmentVariable("SNOWFLAKE_PASSWORD");

            var conn = new SnowflakeConnectionDetails(account, username, password);
            var logger = TestHelpers.CreateLogger<SnowflakeContext>();

            Snowflake = new SnowflakeContext(logger, conn);
        }

        public SnowflakeContext Snowflake { get; private set; }

        public void Dispose()
        {
            Snowflake = null;
        }
    }
}
