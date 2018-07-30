using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Offers
{ 
    [Obsolete("Use IOfferData instead")]
    public interface IOfferCopyBible
    {
        [Obsolete("Use IOfferData instead")]
        List<OfferCopy> Copies { get;  }
        [Obsolete("Use IOfferData instead")]
        string GetTitleFor(int unitId, OfferType offerType);
    }
}
