using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Offers
{
    public interface IOfferCopyBible
    {
        string GetTitleFor(int unitId, OfferType offerType);
    }
}
