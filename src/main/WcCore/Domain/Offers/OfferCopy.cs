using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Offers
{
    public class OfferCopy
    {
        public int UnitId { get; set; }
        public OfferType OfferType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconTitle { get; set; }
        public string IconDescription { get; set; }
    }
}
