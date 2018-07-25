using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Offers
{
    public interface IOfferCopyBible
    {
        List<OfferCopy> Copies { get;  }
        string GetTitleFor(int unitId, OfferType offerType);
    }
}
