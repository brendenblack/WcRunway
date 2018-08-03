using System.Collections.Generic;
using WcCore.Domain.Offers;

namespace WcData.Sheets
{
    public interface IOfferData
    {
        List<OfferSkeleton> Skeletons { get; }
    }
}
