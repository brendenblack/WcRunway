using System.Collections.Generic;
using WcData.Sheets.Models;

namespace WcData.Sheets
{
    public interface IOfferData
    {
        List<OfferSkeleton> Skeletons { get; }

        List<OfferTemplate> Templates { get; }
    }
}
