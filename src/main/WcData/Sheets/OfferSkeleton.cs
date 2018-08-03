using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;

namespace WcData.Sheets
{
    public class OfferSkeleton
    {
        public int UnitId { get; set; }
        public OfferType OfferType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconTitle { get; set; }
        public string IconDescription { get; set; }
        public int Cost { get; set; }
        public int FullCost { get; set; }
        public string CostSku { get; set; }
        public int Duration { get; set; }
        public string Content { get; set; }
        public string DisplayedItems { get; set; }
        public int MaximumQuanity { get; set; } = 1;
        public int TemplateId { get; set; } = 6;
    }
}
