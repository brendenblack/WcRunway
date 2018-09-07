using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features.Test
{
    public class TestHandler : CommandLineHandler // ICommandLineHandler<TestOptions>
    {
        private readonly ILogger<TestHandler> logger;

        public TestHandler(ILogger<TestHandler> logger)
        {
            this.logger = logger;
        }

        public int Execute(TestOptions opts)
        {
            throw new NotImplementedException();
        }

        public int TestLogging(TestOptions opts)
        {
            logger.LogInformation("this is an info log");

            var subs = 5;
            logger.LogInformation("This is an info log with {} substitution(s)", subs);

            logger.LogDebug("this is a debug message");

            logger.LogWarning("warning warning warning");

            logger.LogTrace("this is a trace message");

            return 0;
        }
    }
}
