﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Infrastructure
{
    public static class ParameterSerializer
    {
        public static IList<Dictionary<string, object>> ToDictionary<TSourceType>(IList<TSourceType> source)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(source, settings);

            return JsonConvert.DeserializeObject<IList<Dictionary<string, object>>>(json, new CustomDictionaryConverter());
        }
    }
}
