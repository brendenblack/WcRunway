﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WcData.Implementation.Snowflake.Converters
{
    public class EmbeddedJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(new StringReader((string)reader.Value), objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
