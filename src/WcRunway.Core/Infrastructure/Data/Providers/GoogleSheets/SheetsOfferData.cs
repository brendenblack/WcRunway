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
    public class SheetsOfferData : IOfferData
    {
        #region column mapping
        private static readonly int COL_UNIT_ID = 0;
        private static readonly int COL_OFFER_TYPE = 1;
        private static readonly int COL_TITLE = 2;
        private static readonly int COL_DESCRIPTION = 3;
        private static readonly int COL_ICON_TITLE = 4;
        private static readonly int COL_ICON_DESCRIPTION = 5;
        private static readonly int COL_COST = 6;
        private static readonly int COL_FULL_COST = 7;
        private static readonly int COL_COST_SKU = 8;
        private static readonly int COL_DURATION = 9;
        private static readonly int COL_CONTENT = 10;
        private static readonly int COL_DISPLAY = 11;
        #endregion

        private readonly string sheetId = "1x3nlFmcPUNzJT6wwkqxtGBnxcWALenR5ZnBI5wZjxvw";
        private readonly SheetsService sheets;
        private readonly ILogger<SheetsOfferData> log;

        public DateTime LastUpdate { get; private set; }
        public TimeSpan Validity { get; private set; }
        public bool IsStale
        {
            get
            {
                return DateTime.Now > (LastUpdate + Validity);
            }
        }

        public SheetsOfferData(ILogger<SheetsOfferData> logger, SheetsConnectorService sheets)
        {
            this.Validity = TimeSpan.FromMinutes(30);
            this.sheets = sheets.Service;
            this.log = logger;
        }


        private List<OfferSkeleton> skeletons = new List<OfferSkeleton>();

        public List<OfferSkeleton> Skeletons
        {
            get
            {
                if (IsStale)
                {
                    Update();
                }

                return skeletons;
            }
        }

        public async Task Update()
        {
            log.LogDebug("Clearing {} offer skeleton(s)", skeletons?.Count ?? 0);
            this.skeletons.Clear();

            var range = "Uniques!A2:L";
            log.LogDebug("Retrieving records from range {0}", range);
            
            SpreadsheetsResource.ValuesResource.GetRequest request = sheets.Spreadsheets.Values.Get(sheetId, range);
            ValueRange response = await request.ExecuteAsync();

            log.LogDebug("Received response of {0} from range {0} (major dimension: {1})", response.Values?.Count ?? 0, response.Range, response.MajorDimension);
            var values = response.Values;

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    var skeleton = new OfferSkeleton
                    {
                        UnitId = row[COL_UNIT_ID].AsInteger(),
                        OfferType = row[COL_OFFER_TYPE].AsOfferType(),
                        Title = row[COL_TITLE].ToString(),
                        Description = row[COL_DESCRIPTION].ToString(),
                        IconTitle = row[COL_ICON_TITLE].ToString(),
                        IconDescription = row[COL_ICON_DESCRIPTION].ToString(),
                        Cost = row[COL_COST].AsInteger(),
                        FullCost = row[COL_FULL_COST].AsInteger(),
                        CostSku = row[COL_COST_SKU].AsString() ?? "gold",
                        Duration = row[COL_DURATION].AsInteger(),
                        Content = row[COL_CONTENT].AsString(),
                        DisplayedItems = row[COL_DISPLAY].AsString()
                    };

                    this.skeletons.Add(skeleton);
                }
            }
            else
            {
                log.LogWarning("No values returned from sheet {0}", sheetId);
            }

        }
    }
}
