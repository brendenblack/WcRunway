using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets;

namespace WcRunway.Core.Domain.Offers
{
    public interface IOfferData
    {
        List<OfferSkeleton> Skeletons { get; }
    }
}
