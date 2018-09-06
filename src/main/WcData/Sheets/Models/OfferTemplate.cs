﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Sheets.Models
{
    public class OfferTemplate
    {
        public int Id { get; set; }

        /// <summary>
        /// A description of the template; what sort of offer the template represents
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Descriptive tags that aid in searching for a family of templates
        /// </summary>
        public List<string> Tags { get; set; }

        public string OfferTitle { get; set; }

        public string OfferDescription { get; set; }

        public string OfferIconTitle { get; set; }

        public string OfferIconDescription { get; set; }

        public int OfferCost { get; set; }

        public int OfferFullCost { get; set; }

        public string OfferCostSku { get; set; }

        public int OfferDuration { get; set; }

        public string OfferContent { get; set; }

        public string OfferDisplay { get; set; }

        public int OfferTemplateId { get; set; }

        public int OfferMaxQuantity { get; set; } = 1;

    }
}
                                         