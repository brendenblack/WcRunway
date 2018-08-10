using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WcData.Implementation.Snowflake.Extensions
{
    public static class IDataReaderExtensions
    {
        public static int ReadColumnAsInt(this IDataReader reader, int column, int defaultValue = 0)
        {
            if (Int32.TryParse(reader.GetString(column)?.Replace("\\", "").Replace("\"",""), out int result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static long ReadColumnAsLong(this IDataReader reader, int column, long defaultValue = 0)
        {
            if (Int64.TryParse(reader.GetString(column), out long result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string ReadColumnAsString(this IDataReader reader, int column, string defaultValue = "")
        {
            return reader.GetString(column)?.Replace("\\", "").Replace("\"", "") ?? defaultValue;
        }
    }
}
