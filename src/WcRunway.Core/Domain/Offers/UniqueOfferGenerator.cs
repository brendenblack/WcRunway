using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;

namespace WcRunway.Core.Domain.Offers
{
    public class UniqueOfferGenerator
    {
        private readonly ILogger<UniqueOfferGenerator> log;
        private readonly IOfferData offerData;

        public UniqueOfferGenerator(ILogger<UniqueOfferGenerator> log, IOfferData offerData)
        {
            this.log = log;
            this.offerData = offerData;
        }


        public Offer CreateUnlockOffer(Unit unit, string prefix, int priority = 0)
        {
            if (unit == null || string.IsNullOrWhiteSpace(prefix))
            {
                // TODO
            }

            var code = $"{prefix}Unl";
            log.LogInformation("Creating unlock offer for {0} - {1} using offer code {2}", unit.Id, unit.Name, code);

            // Fetch data from the offer data dictionary
            var skeleton = this.offerData.Skeletons.FirstOrDefault(c => c.UnitId == unit.Id && c.OfferType == OfferType.UNIT_UNLOCK);
            // If nothing was found, use generic alternative with functionally empty content & displayed item blocks
            if (skeleton == null)
            {
                log.LogWarning("No custom copy exists for an unlock of {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
                skeleton = new OfferSkeleton
                {
                    Title = $"Unlock the {unit.Name}!",
                    Description = $"Offer includes an UNLOCK of the powerful {unit.Name.ToUpper()}!",
                    IconTitle = $"{unit.Name} UNLOCK!",
                    IconDescription = $"Offer includes an UNLOCK of the powerful {unit.Name.ToUpper()}!",
                    Duration = 86400,
                    Cost = 99,
                    FullCost = -1,
                    CostSku = "gold",
                    Content = "{ \"skus\": { \"gold\": 0 } }",
                    DisplayedItems = "[]"
                };                
            }

            var offer = new Offer
            {
                OfferCode = code,
                Title = skeleton.Title,
                Description = skeleton.Description,
                IconTitle = skeleton.IconTitle,
                IconDescription = skeleton.IconDescription,
                Cost = skeleton.Cost,
                FullCost = skeleton.FullCost,
                CostSku = skeleton.CostSku,
                Duration = skeleton.Duration,
                ContentJson = skeleton.Content,
                DisplayedItemsJson = skeleton.DisplayedItems,
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now + TimeSpan.FromDays(3),
                Priority = priority,
                MaxQuantity = 1,
                Cooldown = 0,
                CooldownType = 1
                
            };

            return offer;
        }
    }
}
