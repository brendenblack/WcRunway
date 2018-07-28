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
                //string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-offer-tool.json");

                //credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                //    GoogleClientSecrets.Load(stream).Secrets,
                //    Scopes,
                //    "bblack",
                //    CancellationToken.None,
                //    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
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

        //public void doConnect()
        //{
        //    ServiceAccountCredential credential;

        //    using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
        //    {
        //        credential = GoogleCredential.FromStream(stream)
        //            .CreateScoped(new string[] { SheetsService.Scope.SpreadsheetsReadonly })
        //            .UnderlyingCredential as ServiceAccountCredential;
        //        //string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //        //credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-offer-tool.json");

        //        //credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //        //    GoogleClientSecrets.Load(stream).Secrets,
        //        //    Scopes,
        //        //    "bblack",
        //        //    CancellationToken.None,
        //        //    new FileDataStore(credPath, true)).Result;
        //        //Console.WriteLine("Credential file saved to: " + credPath);
        //    }

        //    // Create Google Sheets API service.
        //    var service = new SheetsService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = ApplicationName,
        //    });

        //    var spreadsheetId = "1oB2tzdftGTXeOnWr_aIlZ0A48FM15bfyz406Ldn_7hk";
        //    var range = "Unit Data!A1:EJ5";
        //    SpreadsheetsResource.ValuesResource.GetRequest request =
        //            service.Spreadsheets.Values.Get(spreadsheetId, range);

        //    // Prints the names and majors of students in a sample spreadsheet:
        //    // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
        //    ValueRange response = request.Execute();
        //    IList<IList<Object>> values = response.Values;
        //    if (values != null && values.Count > 0)
        //    {
        //        Console.WriteLine("Name, Major");
        //        foreach (var row in values)
        //        {
        //            // Print columns A and E, which correspond to indices 0 and 4.
        //            Console.WriteLine("{0} {1} {2}", row[0], row[1], row[9]);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("No data found.");
        //    }

        //}
    }
}
