using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.Validate
{
    /// <summary>
    /// This handler is meant to test the configuration values required to generate services. Because of that, it is atypical and 
    /// bypasses the standard framework. It does not (and must not) depend on the container to locate required services. 
    /// </summary>
    public class ValidateHandler
    {
        public int Execute(ValidateOptions opts)
        {

            IConfiguration config = Startup.LoadConfiguration(opts);
            var wellFormed = CheckIfConfigIsWellFormed(config);

            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection, config);
            ServiceProvider container = serviceCollection.BuildServiceProvider();

            return -1;
        }

        /// <summary>
        /// Validates whether or not the provided configuration files have all of the required fields set or not
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool CheckIfConfigIsWellFormed(IConfiguration config)
        {
            var sb2Url = config["data:sandbox2:url"];
            var sb2Name = config["data:sandbox2:name"];
            var sb2Username = config["data:sandbox2:username"];
            var sb2Password = config["data:sandbox2:password"];


            var sfAccount = config["data:snowflake:account"];
            var sfUsername = config["data:snowflake:username"];
            var sfPassword = config["data:snowflake:password"];
            var sfDatabase = config["data:snowflake:database"] ?? "";
            var sfSchema = config["data:snowflake:schema"] ?? "";


            return true; // TODO
        }

        public bool CheckIfSb2ConfigIsValid(ServiceProvider container)
        {
            return false;
        }
    }
}
