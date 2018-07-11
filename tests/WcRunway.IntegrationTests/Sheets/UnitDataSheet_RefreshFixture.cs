using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WcRunway.Core.Infrastructure;
using WcRunway.Core.Sheets;

namespace WcRunway.IntegrationTests.Sheets
{
    public class UnitDataSheet_RefreshFixture : IDisposable
    {
        public UnitDataSheet_RefreshFixture()
        {
            var connector = new SheetsConnectorService();
            this.sut = new SheetsUnitRepository(connector);
            Task.Run(() => this.sut.RefreshUnits()).Wait();

        }

        public SheetsUnitRepository sut { get; private set; }

        public void Dispose()
        {
            // no-op
        }
    }
}
