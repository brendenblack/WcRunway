using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;

namespace WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets
{
    internal static class RowObjectExtensions
    {
        internal static String AsString(this Object column)
        {
            return column.ToString();
        }

        internal static int AsInteger(this Object column, int defaultValue = 0)
        {
            int i;

            bool parse = Int32.TryParse(
                column.ToString(), 
                System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowLeadingSign,
                System.Globalization.CultureInfo.InvariantCulture, 
                out i);
            if (!parse)
            {
                i = defaultValue;
            }

            return i;
        }

        internal static OfferType AsOfferType(this Object col, OfferType defaultValue = OfferType.UNLOCK)
        {
            OfferType ot;
            if (Enum.TryParse(col.ToString(), out ot))
            {
                return ot;
            }
            else;
            {
                return defaultValue;
            }
        }
    }
}
