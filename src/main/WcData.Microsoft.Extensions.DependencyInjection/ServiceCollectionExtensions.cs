using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WcData.GameContext;
using WcData.Implementation;
using WcData.Implementation.MySql;
using WcData.Implementation.Sheets;
using WcData.Sheets;

namespace WcData.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds connections to the various Google Sheets used by design to store and manipulate data in the game. This information is not guaranteed to 
        /// be the current in-game state.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
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

            services.AddSingleton(credential);
            services.AddSingleton<SheetsConnectorService>();

            services.AddSingleton<IUnitData, SheetsUnitData>();
            services.AddSingleton<IOfferData, SheetsOfferData>();

            services.AddTransient<IGameData, SheetsGameData>();

            return services;
        }

        /// <summary>
        /// Adds a connection to a MySql game database. Can be added more than once to add different environments
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddGameContext(this IServiceCollection services, Action<GameContextOptions> optionsAction)
        {
            var opts = new GameContextOptions();
            optionsAction.Invoke(opts);

            var connectionString = $"server={opts.Url};database={opts.Name};uid={opts.Username};pwd={opts.Password};ssl-mode=none";

            switch (opts.Environment)
            {//.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).
                case GameContexts.LIVE_MAIN:
                    throw new NotImplementedException("A connection to live is not yet supported");
                case GameContexts.LIVE_SLAVE:
                    services.AddDbContext<LiveSlaveContext>(opt => opt.UseMySQL(connectionString));
                    break;
                case GameContexts.SANDBOX2:
                    services.AddDbContext<Sandbox2Context>(opt => opt.UseMySQL(connectionString));
                    break;
                default:
                    break;
            }
            
            return services;
        }

        public static IServiceCollection AddSnowflake(this IServiceCollection services, Action<SnowflakeOptions> optionsAction)
        {
            var opts = new SnowflakeOptions();
            optionsAction.Invoke(opts);

            return services;
        }
    }
}
