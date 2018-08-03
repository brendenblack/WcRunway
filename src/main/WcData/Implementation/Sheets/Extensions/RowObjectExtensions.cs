using System;
using System.Collections.Generic;
using WcRunway.Core.Domain.Offers;

namespace WcData.Implementation.Sheets.Extensions
{
    public static class RowObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static OfferType ReadColumnAsOfferType(this IList<object> row, int columnIndex, OfferType defaultValue = OfferType.STANDARD_UNLOCK)
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
                    return OfferType.STANDARD_UNLOCK;
                case "ELITE UNLOCK":
                    return OfferType.ELITE_UNLOCK;
                case "OMEGA UNLOCK":
                    return OfferType.OMEGA_UNLOCK;
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
