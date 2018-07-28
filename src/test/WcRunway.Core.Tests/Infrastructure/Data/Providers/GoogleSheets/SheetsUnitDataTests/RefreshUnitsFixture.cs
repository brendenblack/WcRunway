using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets;

namespace WcRunway.Core.Tests.Infrastructure.Data.Providers.GoogleSheets.SheetsUnitDataTests
{
    public class RefreshUnitsFixture : IDisposable
    {
        public RefreshUnitsFixture()
        {
            var credential = EmbeddedJsonServiceCredential.CreateCredentialFromFile(); // lol
            var connector = new SheetsConnectorService(TestHelpers.CreateLogger<SheetsConnectorService>(), credential);
            this.sut = new SheetsUnitData(TestHelpers.CreateLogger<SheetsUnitData>(), connector);
            Task.Run(() => this.sut.RefreshUnits()).Wait();
        }

        public SheetsUnitData sut { get; private set; }

        public void Dispose()
        {
            // no-op
        }
    }
}
