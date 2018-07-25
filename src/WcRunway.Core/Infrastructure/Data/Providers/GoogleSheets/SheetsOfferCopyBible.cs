using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WcRunway.Core.Domain.Offers;

namespace WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets
{
    public class SheetsOfferCopyBible
    {
        #region column mapping
        private static readonly int COL_UNIT_ID = 0;
        private static readonly int COL_OFFER_TYPE = 1;
        private static readonly int COL_TITLE = 2;
        private static readonly int COL_DESCRIPTION = 3;
        private static readonly int COL_ICON_TITLE = 4;
        private static readonly int COL_ICON_DESCRIPTION = 5;

        #endregion

        private readonly string sheetId = "1x3nlFmcPUNzJT6wwkqxtGBnxcWALenR5ZnBI5wZjxvw";
        private readonly SheetsService sheets;
        private readonly ILogger<SheetsOfferCopyBible> log;

        public DateTime LastUpdate { get; private set; }
        public TimeSpan Validity { get; private set; }
        public bool IsStale
        {
            get
            {
                return DateTime.Now > (LastUpdate + Validity);
            }
        }

        public SheetsOfferCopyBible(ILogger<SheetsOfferCopyBible> logger, SheetsConnectorService sheets)
        {
            this.Validity = TimeSpan.FromMinutes(30);
            this.sheets = sheets.Service;
            this.log = logger;
        }

        private List<OfferCopy> copies = new List<OfferCopy>();
        public List<OfferCopy> Copies
        {
            get
            {
                if (IsStale)
                {
                    log.LogInformation("Offer copy values are stale, refetching");
                    Update();
                }

                return this.copies;
            }
        }


        public async Task Update()
        {
            this.copies.Clear();

            var range = "Copy!A:F";
            SpreadsheetsResource.ValuesResource.GetRequest request = sheets.Spreadsheets.Values.Get(sheetId, range);
            ValueRange response = await request.ExecuteAsync();
            log.LogDebug("Received response of range {0} (major dimension: {1})", response.Range, response.MajorDimension);
            var values = response.Values;

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    var copy = new OfferCopy
                    {
                        UnitId = row[COL_UNIT_ID].AsInteger(),
                        OfferType = (OfferType)Enum.Parse(typeof(OfferType), row[1].ToString()),
                        Title = row[COL_TITLE].ToString(),
                        Description = row[COL_DESCRIPTION].ToString(),
                        IconTitle = row[COL_ICON_TITLE].ToString(),
                        IconDescription = row[COL_ICON_DESCRIPTION].ToString()
                    };

                    this.copies.Add(copy);
                }
            }

        }
    }
}
