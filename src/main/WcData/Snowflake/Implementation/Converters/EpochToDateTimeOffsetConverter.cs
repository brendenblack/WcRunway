using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Snowflake.Implementation.Converters
{
    public class EpochToDateTimeOffsetConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long ticks;
            if (reader.TokenType == JsonToken.String)
            {
                Int64.TryParse(reader.Value.ToString(), out ticks);
            }
            else if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(String.Format("Unexpected token parsing date. Expected Integer, got {0}.", reader.TokenType));
            }
            else
            {
                ticks = (long)reader.Value;
            }

            var date = DateTimeOffset.FromUnixTimeSeconds(ticks).ToOffset(TimeSpan.FromHours(-7));
            // date = date.AddSeconds(ticks);

            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var epoc = new DateTime(1970, 1, 1);
                var delta = ((DateTime)value) - epoc;
                if (delta.TotalSeconds < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "Unix epoc starts January 1st, 1970");
                }
                ticks = (long)delta.TotalSeconds;
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
            writer.WriteValue(ticks);
        }
    }
}
