using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using WcData.GameContext;

namespace WcOffers.Cli.Features.Test
{
    public class TestHandler
    {
        private readonly ILogger<TestHandler> log;
        private readonly ISandbox2Context sb2;

        public TestHandler(ILogger<TestHandler> log, ISandbox2Context sb2)
        {
            this.log = log;
            this.sb2 = sb2;
        }

        public int Execute(TestOptions opts)
        {

            var csvList = sb2.Offers.Where(o => o.Id > 800).Select(o => new CsvRepresentation
            {
                Title = o.Title,
                Description = o.Description,
                IconTitle = o.IconTitle,
                IconDescription = o.IconDescription,
                Cost = o.Cost,
                FullCost = o.FullCost,
                CostSku = o.CostSku,
                Content = o.ContentJson.Replace("\n", " ").Replace("\r", " "),
                Display = o.DisplayedItemsJson.Replace("\n", " ").Replace("\r", " "),
                TemplateId = o.TemplateId,
                Duration = (o.Duration.HasValue) ? o.Duration.Value : 0,
                MaxQuantity  = o.MaxQuantity
            })
            .ToList();

            using (StreamWriter writer = File.CreateText("offers.csv"))
            {
                var csvWriter = new CsvWriter(writer);
                csvWriter.WriteRecords(csvList);
            }

            return 0;
        }

        public void DoSb2Test()
        {
            var id = 903;
            var offer = this.sb2.Offers.FirstOrDefault(o => o.Id == id);
            if (offer == null)
            {
                log.LogWarning("Unable to find offer with id {0}", id);
            }
            else
            {
                log.LogInformation("Retrieved offer with code {0}", offer.OfferCode);
            }
        }

        public class CsvRepresentation
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string IconTitle { get; set; }
            public string IconDescription { get; set; }
            public int Cost { get; set; }
            public int FullCost { get; set; }
            public string CostSku { get; set; }
            public int Duration { get; set; }
            public string Content { get; set; }
            public string Display { get; set; }
            public int TemplateId { get; set; }
            public int MaxQuantity { get; set; }
        }
    }
}
