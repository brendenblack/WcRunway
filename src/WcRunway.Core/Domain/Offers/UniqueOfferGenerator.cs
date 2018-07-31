using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcRunway.Core.Domain.Offers;
using WcRunway.Core.Domain.Users;
using WcRunway.Core.Infrastructure.Data.Providers.MySql;
using WcRunway.Core.Infrastructure.Data.Snowflake;

namespace WcRunway.Core.Domain.Offers
{
    public class UniqueOfferGenerator
    {
        private readonly ILogger<UniqueOfferGenerator> log;
        private readonly IOfferData offerData;
        private readonly IUnitOwnership unitOwnership;

        public readonly int MAX_PREFIX_LENGTH = 16;

        public UniqueOfferGenerator(ILogger<UniqueOfferGenerator> log, IOfferData offerData, IUnitOwnership unitOwnership)
        {
            this.log = log;
            this.offerData = offerData;
            this.unitOwnership = unitOwnership;
        }

        /// <summary>
        /// Creates an offer that will unlock the specified unit
        /// </summary>
        /// <param name="unit">The unit to unlock</param>
        /// <param name="prefix">What prefix to apply to the offer code</param>
        /// <param name="priority">Optional. The priority to assign to the offer, higher priorit offers will display before lower priority ones</param>
        /// <returns></returns>
        public Offer CreateUnlockOffer(Unit unit, string prefix, int priority = 0)
        {
            if (unit == null || string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Unit must not be null");
            }

            if (priority < 0)
            {
                priority = 0;
            }

            var code = (prefix.Length > 17) ? $"{prefix.Substring(0, 17)}Unl" : $"{prefix}Unl";
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
                    DisplayedItems = "[ {} ]"
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
                CooldownType = 0,
                TemplateId = 6
                
            };

            return offer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="prefix"></param>
        /// <param name="unlockAsPrereq">Optional. For the first level offer, specifies whether a generated unlock offer should be a prerequisite</param>
        /// <param name="priority">Optional. The priority to assign to the offer, higher priorit offers will display before lower priority ones</param>
        /// <returns></returns>
        public List<Offer> CreateLevelOffers(Unit unit, string prefix, bool unlockAsPrereq = true, int priority = 0)
        {
            if (unit.Id == 311)
            {
                // TODO: handle Siege Squadron differently
            }
            else
            {

            }
            return new List<Offer>();
        }


        public Offer CreateOmegaPartsOffer(Unit unit, string prefix, int priority = 0)
        {
            if (unit == null || string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Unit must not be null");
            }

            if (priority < 0)
            {
                priority = 0;
            }

            var skeleton = this.offerData.Skeletons.FirstOrDefault(c => c.UnitId == unit.Id && c.OfferType == OfferType.OMEGA_PARTS);
            // If nothing was found, use generic alternative with functionally empty content & displayed item blocks
            if (skeleton == null)
            {
                log.LogWarning("No custom copy exists for an unlock of {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
                skeleton = new OfferSkeleton
                {
                    Title = $"Omega {unit.Name} Parts!",
                    Description = $"Offer includes OMEGA PARTS for the {unit.Name.ToUpper()}!",
                    IconTitle = $"{unit.Name} Omega Parts!",
                    IconDescription = $"Offer includes {unit.Name.ToUpper()} Omega Parts!",
                    Duration = 86400,
                    Cost = 99,
                    FullCost = -1,
                    CostSku = "gold",
                    Content = "{ \"skus\": { \"gold\": 0 } }",
                    DisplayedItems = "[ {} ]",
                    MaximumQuanity = 1
                };
            }

            var offer = new Offer
            {
                OfferCode = $"{prefix}OPts",
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
                MaxQuantity = skeleton.MaximumQuanity,
                Cooldown = 0,
                CooldownType = 0,
                TemplateId = 6

            };

            return offer;
        }

        public Offer CreateElitePartsOffer(Unit unit, string prefix, int priority = 0)
        {
            if (unit == null || string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Unit must not be null");
            }

            if (priority < 0)
            {
                priority = 0;
            }

            var skeleton = this.offerData.Skeletons.FirstOrDefault(c => c.UnitId == unit.Id && c.OfferType == OfferType.ELITE_PARTS);
            // If nothing was found, use generic alternative with functionally empty content & displayed item blocks
            if (skeleton == null)
            {
                log.LogWarning("No skeleton was found for an elite parts offer for {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
                skeleton = new OfferSkeleton
                {
                    Title = $"Elite {unit.Name} Parts!",
                    Description = $"Offer includes ELITE PARTS for the {unit.Name.ToUpper()}!",
                    IconTitle = $"{unit.Name} Elite Parts!",
                    IconDescription = $"Offer includes {unit.Name.ToUpper()} Elite Parts!",
                    Duration = 86400,
                    Cost = 99,
                    FullCost = -1,
                    CostSku = "gold",
                    Content = "{ \"skus\": { \"gold\": 0 } }",
                    DisplayedItems = "[ {} ]",
                    MaximumQuanity = 1
                };
            }

            var offer = new Offer
            {
                OfferCode = $"{prefix}EPts",
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
                MaxQuantity = skeleton.MaximumQuanity,
                Cooldown = 0,
                CooldownType = 0,
                TemplateId = 6
            };

            return offer;
        }

        public List<Offer> CreateTechOffers(Unit unit, string prefix, int priority = 0)
        {
            if (unit == null || string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Unit must not be null");
            }

            if (priority < 0)
            {
                priority = 0;
            }

            var skeletons = this.offerData.Skeletons
                .Where(c => c.UnitId == unit.Id && c.OfferType == OfferType.TECH)
                .ToList();

            if (skeletons.Count == 0)
            {
                log.LogWarning("No skeletons were found for a tech offer for {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
                skeletons.Add(new OfferSkeleton
                {
                    Title = $"Tech out your {unit.Name}!",
                    Description = $"Offer includes TECH for the {unit.Name.ToUpper()}!",
                    IconTitle = $"{unit.Name} Tech!",
                    IconDescription = $"Offer includes {unit.Name.ToUpper()} Tech!",
                    Duration = 86400,
                    Cost = 99,
                    FullCost = -1,
                    CostSku = "gold",
                    Content = "{ \"skus\": { \"gold\": 0 } }",
                    DisplayedItems = "[ {} ]",
                    MaximumQuanity = 1
                });
            }

            var offers = new List<Offer>();
            for(int i = 0; i < skeletons.Count; i++)
            {
                var skeleton = skeletons[i];

                var offer = new Offer
                {
                    OfferCode = $"{prefix}tek{i}",
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
                    MaxQuantity = skeleton.MaximumQuanity,
                    Cooldown = 0,
                    CooldownType = 0,
                    TemplateId = 6,
                    Prerequisite = $"{prefix}Unl"
                };
                offers.Add(offer);
            }

            return offers;
        }



        /// <summary>
        /// Fetches a cohort of users who do not have the specified unit unlocked and are thus eligible to receive the unlock offer
        /// </summary>
        /// <param name="unit"></param>
        public List<int> FetchUnlockCohort(Unit unit)
        {
            return this.unitOwnership.FetchUnitOwnerUserIds(unit.Id);
        }
    }
}
