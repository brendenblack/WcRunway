using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WcData.Implementation.Sheets
{
    public class EmbeddedJsonServiceCredential
    {
        public static ServiceAccountCredential CreateCredentialFromFile()
        {
            ServiceAccountCredential credential;
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

            return credential;
        }
    }
}
