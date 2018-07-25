using Microsoft.Extensions.Logging;
using System.Linq;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Infrastructure.Data.Providers.GoogleSheets;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;

namespace WcRunway.Cli.Features.Test
{
    public class TestHandler
    {
        private readonly ILogger<TestHandler> log;
        private readonly Sandbox2Context sb2;
        private readonly IOfferCopyBible offerCopyBible;

        public TestHandler(ILogger<TestHandler> log, Sandbox2Context sb2, IOfferCopyBible offerCopyBible)
        {
            this.log = log;
            this.sb2 = sb2;
            this.offerCopyBible = offerCopyBible;
        }

        public int Execute(TestOptions o)
        {
            if (o.TestSandbox2)
            {
                DoSb2Test();
            }
            else if (o.TestOfferBible)
            {
                DoOfferBibleTest();
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

        public void DoOfferBibleTest()
        {
            foreach(var copy in this.offerCopyBible.Copies)
            {
                log.LogInformation(copy.Title);
            }
        }
    }
}
