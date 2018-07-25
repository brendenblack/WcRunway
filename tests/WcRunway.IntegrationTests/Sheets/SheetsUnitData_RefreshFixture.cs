using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets;

namespace WcRunway.IntegrationTests.Sheets
{
    public class SheetsUnitData_RefreshFixture : IDisposable
    {
        public SheetsUnitData_RefreshFixture()
        {
            var logger = new Logger<SheetsConnectorService>(new NullLoggerFactory());
            var connector = new SheetsConnectorService(logger);
            var logger2 = new Logger<SheetsUnitData>(new NullLoggerFactory());
            this.sut = new SheetsUnitData(logger2, connector);
            Task.Run(() => this.sut.RefreshUnits()).Wait();

        }

        public SheetsUnitData sut { get; private set; }

        public void Dispose()
        {
            // no-op
        }
    }
}
