using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Offers
{
    public class MockOfferCopyBible : IOfferCopyBible
    {



        public string GetTitleFor(int unitId, OfferType offerType)
        {
            throw new NotImplementedException();
        }
    }
}
