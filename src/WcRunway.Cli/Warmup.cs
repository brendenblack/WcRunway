using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WcRunway.Core.Domain.Game;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets;

namespace WcRunway.Cli
{
    public class Warmup
    {
        private readonly ILogger<Warmup> log;
        private readonly IUnitData unitData;

        public Warmup(ILogger<Warmup> log, IUnitData unitData)
        {
            this.log = log;
            this.unitData = unitData;
        }

        public async Task Run()
        {
            log.LogInformation("Beginning warmup");

            if (unitData.GetType().IsAssignableFrom(typeof(SheetsUnitData)))
            {
                log.LogInformation("Warming up unit data from Google Sheets");

                var units = unitData as SheetsUnitData;

                await units.RefreshUnits();

                log.LogInformation("Tracking {0} units", units.Units.Count());
            }
            else
            {
                log.LogTrace("Skipping warmup of unknown type {0}", unitData.GetType());
            }

            log.LogInformation("Warmup complete");
        }
    }
}
