using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain;
using WcRunway.Core.Domain.Offers;

namespace WcRunway.Core.Tests.Domain.Offers.UniqueOfferGeneratorTests
{
    public class StandardUniqueLevelOffersFixture : UniqueOfferGeneratorFixture
    {
        public StandardUniqueLevelOffersFixture() : base()
        {
            this.juggernaut = new Unit(217) { Name = "Juggernaut" };

            for (int i = 1; i <= 20; i++)
            {
                juggernaut.Levels.Add(new Level { Grade = 1, Number = i });
            }

            for (int i = 21; i <= 30; i++)
            {
                juggernaut.Levels.Add(new Level { Grade = 2, Number = i });
            }

            for (int i = 31; i <= 40; i++)
            {
                juggernaut.Levels.Add(new Level { Grade = 3, Number = i });

            }

            Offers = OfferGenerator.CreateLevelOffers(juggernaut, "Jul18Test");
        }

        private readonly Unit juggernaut;
        public List<Offer> Offers { get; private set; }
    }
}
