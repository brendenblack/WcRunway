using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Logging;
using System.IO;

namespace WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets
{
    public class SheetsConnectorService
    {
        private readonly ILogger<SheetsConnectorService> log;

        public SheetsConnectorService(ILogger<SheetsConnectorService> logger, ServiceAccountCredential credential)
        {
            log = logger;
            // TODO: inject this secret
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(new string[] { SheetsService.Scope.SpreadsheetsReadonly })
                    .UnderlyingCredential as ServiceAccountCredential;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            this.Service = service;
        }

        public SheetsService Service { get; private set; }



        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "War Commander Runway";
    }
}
