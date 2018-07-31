using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;

namespace WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets.Extension
{
    public static class RowObjectExtensions
    {
        [Obsolete("Use ReadColumnAsString instead")]
        public static String AsString(this Object column)
        {
            return column.ToString();
        }

        [Obsolete("Use ReadColumnAsInteger instead")]
        public static int AsInteger(this Object column, int defaultValue = 0)
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

        [Obsolete("Use ReadColumnAsOfferType instead")]
        public static OfferType AsOfferType(this Object col, OfferType defaultValue = OfferType.UNIT_UNLOCK)
        {
            if (col == null)
            {
                return defaultValue;
            }

            switch (col.ToString().ToUpper())
            {
                case "UNLOCK":
                    return OfferType.UNIT_UNLOCK;
                case "LEVELS":
                    return OfferType.LEVELS;
                case "OMEGA PARTS":
                case "OMEGA":
                    return OfferType.OMEGA_PARTS;
                case "ELITE PARTS":
                case "ELITE":
                    return OfferType.ELITE_PARTS;
                case "TECH":
                    return OfferType.TECH;
                default:
                    return defaultValue;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static OfferType ReadColumnAsOfferType(this IList<object> row, int columnIndex, OfferType defaultValue = OfferType.UNIT_UNLOCK)
        {
            string columnValue;
            try
            {
                columnValue = row[columnIndex].ToString() ?? "";
            }
            catch (IndexOutOfRangeException)
            {
                return defaultValue;
            }

            if (string.IsNullOrWhiteSpace(columnValue))
            {
                return defaultValue;
            }

            switch (columnValue.ToUpper())
            {
                case "UNLOCK":
                    return OfferType.UNIT_UNLOCK;
                case "LEVELS":
                    return OfferType.LEVELS;
                case "OMEGA PARTS":
                case "OMEGA":
                    return OfferType.OMEGA_PARTS;
                case "ELITE PARTS":
                case "ELITE":
                    return OfferType.ELITE_PARTS;
                case "TECH":
                    return OfferType.TECH;
                default:
                    return defaultValue;
            }
        }

        public static int ReadColumnAsInteger(this IList<object> row, int columnIndex, int defaultValue = 0)
        {
            try
            {
                int result = Int32.Parse(row[columnIndex].ToString()); // .ToString() ?? "";
                return result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Attemps to read the specified column as a string value, returning a default value if it cannot or if it is empty.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="defaultValue">Optional. The value to return if the column cannot be read or is empty</param>
        /// <returns></returns>
        public static string ReadColumnAsString(this IList<object> row, int columnIndex, string defaultValue = "")
        {
            try
            {
                string result = row[columnIndex].ToString();
                return result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
