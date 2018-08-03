using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Sheets
{
    public interface IOfferData
    {
        List<OfferSkeleton> Skeletons { get; }
    }
}
