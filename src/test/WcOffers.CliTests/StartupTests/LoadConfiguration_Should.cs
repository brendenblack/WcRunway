using Microsoft.Extensions.Configuration;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WcOffers.Cli;
using WcOffers.Cli.Features;
using WcOffers.Cli.Features.Generate;
using Xunit;

namespace WcOffers.CliTests.StartupTests
{
    public class LoadConfiguration_Should
    {
        [Fact]
        public void ThrowWhenOptionsSpecifiedFileDoesNotExist()
        {
            var opts = new GenerateOptions
            {
               ConfigurationFile = "doesnotexist.ini"
            };

            Should.Throw<FileNotFoundException>(() => Startup.LoadConfiguration(opts));
        }

        [Fact]
        public void ReadInRequiredSettingsFromValidIniFile()
        {
            var opts = new GenerateOptions
            {
                ConfigurationFile = Path.GetFullPath("StartupTests/validconfig.ini")
            };

            IConfiguration config = Startup.LoadConfiguration(opts);

            var sb2Url = config["data:sandbox2:url"];
            var sb2Name = config["data:sandbox2:name"];
            var sb2Username = config["data:sandbox2:username"];
            var sb2Password = config["data:sandbox2:password"];
            var sfAccount = config["data:snowflake:account"];
            var sfUsername = config["data:snowflake:username"];
            var sfPassword = config["data:snowflake:password"];

            sb2Url.ShouldBe("wc-dev-sandbox2.mysql.com");
            sb2Name.ShouldBe("wc_dev_sandbox");
            sb2Username.ShouldBe("sb2username");
            sb2Password.ShouldBe("sb2password");
            sfAccount.ShouldBe("warcommander");
            sfUsername.ShouldBe("sfusername");
            sfPassword.ShouldBe("sfpassword");
        }

        [Fact]
        public void ReadInRequiredSettingsFromValidJsonFile()
        {
            var opts = new GenerateOptions { ConfigurationFile = Path.GetFullPath("StartupTests/validconfig.json") };
            IConfiguration config = Startup.LoadConfiguration(opts);
            var sb2Url = config["data:sandbox2:url"];
            var sb2Name = config["data:sandbox2:name"];
            var sb2Username = config["data:sandbox2:username"];
            var sb2Password = config["data:sandbox2:password"];
            var sfAccount = config["data:snowflake:account"];
            var sfUsername = config["data:snowflake:username"];
            var sfPassword = config["data:snowflake:password"];

            sb2Url.ShouldBe("wc-dev-sandbox2.mysql.com");
            sb2Name.ShouldBe("wc_dev_sandbox");
            sb2Username.ShouldBe("sb2username");
            sb2Password.ShouldBe("sb2password");
            sfAccount.ShouldBe("warcommander");
            sfUsername.ShouldBe("sfusername");
            sfPassword.ShouldBe("sfpassword");
        }


    }
}
