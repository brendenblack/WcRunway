using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WcRunway.Core.Infrastructure;
using WcRunway.Core.Sheets;

namespace WcRunway.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWcSheets(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<SheetsConnectorService>();
            services.AddScoped<SheetsUnitRepository>();

            return services;
        }
    }
}
