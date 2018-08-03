using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WcData.Implementation;
using WcData.Implementation.Sheets;

namespace WcData.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddSheets(this IServiceCollection services, Action<SheetsOptions> optionsAction)
        {
            var opts = new SheetsOptions();
            optionsAction.Invoke(opts);

            // services.AddSingleton(EmbeddedJsonServiceCredential.CreateCredentialFromFile());

            ServiceAccountCredential credential;
            using (var stream = new FileStream(opts.ClientSecretPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(new string[] { SheetsService.Scope.SpreadsheetsReadonly })
                    .UnderlyingCredential as ServiceAccountCredential;
            }

            services.AddSingleton<SheetsConnectorService>();

            services.AddSingleton<IUnitData, SheetsUnitData>();
            services.AddTransient<IOfferData, SheetsOfferData>();

            return services;
        }

        public static IServiceCollection AddMySql(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddSnowflake(this IServiceCollection services)
        {
            return services;
        }
    }
}
