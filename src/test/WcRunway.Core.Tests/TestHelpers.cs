using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcCore.Tests
{
    public class TestHelpers
    {
        public static ILogger<T> CreateLogger<T>()
        {
            var factory = new LoggerFactory().AddDebug();
            var logger = factory.CreateLogger<T>();
            return logger;
        }
    }
}
