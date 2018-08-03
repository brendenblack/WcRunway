using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using WcData.Microsoft.Extensions.DependencyInjection;

namespace WcData.Microsoft.Extensions.DependencyInjection.Tests
{
    public class Class1
    {
        [Fact]
        public void test()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSheets(opts => { opts.ClientSecretPath = "client_secret.json"; });

            var provider = services.BuildServiceProvider();
            // provider.GetRequiredService<>
        }
    }
}
