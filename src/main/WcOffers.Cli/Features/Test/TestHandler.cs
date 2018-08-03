using Microsoft.Extensions.Logging;
using System.Linq;
using WcData.GameContext;

namespace WcOffers.Cli.Features.Test
{
    public class TestHandler
    {
        private readonly ILogger<TestHandler> log;
        private readonly Sandbox2Context sb2;

        public TestHandler(ILogger<TestHandler> log, Sandbox2Context sb2)
        {
            this.log = log;
            this.sb2 = sb2;
        }

        public int Execute(TestOptions o)
        {
            if (o.TestSandbox2)
            {
                DoSb2Test();
            }


            return 0;
        }

        public void DoSb2Test()
        {
            var id = 903;
            var offer = this.sb2.Offers.FirstOrDefault(o => o.Id == id);
            if (offer == null)
            {
                log.LogWarning("Unable to find offer with id {0}", id);
            }
            else
            {
                log.LogInformation("Retrieved offer with code {0}", offer.OfferCode);
            }
        }
    }
}
