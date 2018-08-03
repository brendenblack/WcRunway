using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WcData.Implementation.Sheets.Extensions;
using WcData.Offers;

namespace WcData.Implementation.Sheets
{
    public class SheetsOfferData : BaseSheetsData, IOfferData
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
        private static readonly int COL_TEMPLATE_ID = 12;
        private static readonly int COL_MAX_QUANTITY = 13;
        #endregion

        private readonly string sheetId = SheetIds.OFFER_DATA;
        private readonly SheetsService sheets;
        private readonly ILogger<SheetsOfferData> log;

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
                    log.LogDebug("Offer skeleton data is stale, refetching...");
                    Task.WaitAll(Update());
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
                    try
                    {
                        var skeleton = new OfferSkeleton
                        {
                            UnitId = row.ReadColumnAsInteger(COL_UNIT_ID),
                            OfferType = row.ReadColumnAsOfferType(COL_OFFER_TYPE), // .[COL_OFFER_TYPE].AsOfferType(),
                            Title = row.ReadColumnAsString(COL_TITLE),
                            Description = row.ReadColumnAsString(COL_DESCRIPTION), // ?: row[COL_DESCRIPTION].ToString(),
                            IconTitle = row.ReadColumnAsString(COL_ICON_TITLE),
                            IconDescription = row.ReadColumnAsString(COL_ICON_DESCRIPTION),
                            Cost = row.ReadColumnAsInteger(COL_COST),
                            FullCost = row.ReadColumnAsInteger(COL_FULL_COST),
                            CostSku = row.ReadColumnAsString(COL_COST_SKU, "gold"),
                            Duration = row.ReadColumnAsInteger(COL_DURATION, 28800),
                            Content = row.ReadColumnAsString(COL_CONTENT, "{\"gold\": 0 }"),
                            DisplayedItems = row.ReadColumnAsString(COL_DISPLAY, "[]"),
                            MaximumQuanity = row.ReadColumnAsInteger(COL_MAX_QUANTITY, 1)
                        };

                        this.skeletons.Add(skeleton);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        log.LogError("Unable to read row as an offer skeleton, skipping");
                    }
                }
            }
            else
            {
                log.LogWarning("No values returned from sheet {0}", sheetId);
            }
        }
    }
}