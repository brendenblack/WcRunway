using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Sheets
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

            bool parse = Int32.TryParse(column.ToString(), System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out i);
            if (!parse)
            {
                i = defaultValue;
            }

            return i;
        }
    }
}
