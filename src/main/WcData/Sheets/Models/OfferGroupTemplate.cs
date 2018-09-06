using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Sheets.Models
{
    /// <summary>
    /// Represents an offer template that is meant to be as part of a collection that will chain together,
    /// and one offer has a prerequisite on the prior one. E.g. base expansion and resource compression
    /// </summary>
    public class OfferGroupTemplate
    {
        /// <summary>
        /// The group that this template belongs to
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// What order this particular template should be within the group
        /// </summary>
        public int Ordinal { get; set; }

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
