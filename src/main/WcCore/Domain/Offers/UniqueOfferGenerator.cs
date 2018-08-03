//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using WcCore.Domain.Game;
//using WcCore.Domain.Offers;
//using WcCore.Domain.Users;
//using WcCore.Infrastructure.Data.Providers.MySql;
//using WcCore.Infrastructure.Data.Snowflake;

//namespace WcCore.Domain.Offers
//{
//    public class UniqueOfferGenerator
//    {
//        private readonly ILogger<UniqueOfferGenerator> log;
//        private readonly IOfferData offerData;

//        public readonly int MAX_PREFIX_LENGTH = 16;

//        public UniqueOfferGenerator(ILogger<UniqueOfferGenerator> log, IOfferData offerData)
//        {
//            this.log = log;
//            this.offerData = offerData;
//        }

//        /// <summary>
//        /// Creates an offer that will unlock the specified unit
//        /// </summary>
//        /// <param name="unit">The unit to unlock</param>
//        /// <param name="prefix">What prefix to apply to the offer code</param>
//        /// <param name="priority">Optional. The priority to assign to the offer, higher priorit offers will display before lower priority ones</param>
//        /// <returns></returns>
//        public Offer CreateUnlockOffer(Unit unit, string prefix, int priority = 0)
//        {
//            if (unit == null || string.IsNullOrWhiteSpace(prefix))
//            {
//                throw new ArgumentException("Unit must not be null");
//            }

//            if (priority < 0)
//            {
//                priority = 0;
//            }

//            var code = (prefix.Length > 17) ? $"{prefix.Substring(0, 17)}Unl" : $"{prefix}Unl";
//            log.LogInformation("Creating unlock offer for {0} - {1} using offer code {2}", unit.Id, unit.Name, code);

//            // Fetch data from the offer data dictionary
//            var skeleton = this.offerData.Skeletons.FirstOrDefault(c => c.UnitId == unit.Id && c.OfferType == OfferType.STANDARD_UNLOCK);
//            // If nothing was found, use generic alternative with functionally empty content & displayed item blocks
//            if (skeleton == null)
//            {
//                log.LogWarning("No custom copy exists for an unlock of {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
//                skeleton = new OfferSkeleton
//                {
//                    Title = $"Unlock the {unit.Name}!",
//                    Description = $"Offer includes an UNLOCK of the powerful {unit.Name.ToUpper()}!",
//                    IconTitle = $"{unit.Name} UNLOCK!",
//                    IconDescription = $"Offer includes an UNLOCK of the powerful {unit.Name.ToUpper()}!",
//                    Duration = 86400,
//                    Cost = 99,
//                    FullCost = -1,
//                    CostSku = "gold",
//                    Content = "{ \"skus\": { \"gold\": 0 } }",
//                    DisplayedItems = "[ {} ]"
//                };                
//            }

//            var offer = new Offer
//            {
//                OfferCode = code,
//                Title = skeleton.Title,
//                Description = skeleton.Description,
//                IconTitle = skeleton.IconTitle,
//                IconDescription = skeleton.IconDescription,
//                Cost = skeleton.Cost,
//                FullCost = skeleton.FullCost,
//                CostSku = skeleton.CostSku,
//                Duration = skeleton.Duration,
//                ContentJson = skeleton.Content,
//                DisplayedItemsJson = skeleton.DisplayedItems,
//                StartTime = DateTimeOffset.Now,
//                EndTime = DateTimeOffset.Now + TimeSpan.FromDays(3),
//                Priority = priority,
//                MaxQuantity = 1,
//                Cooldown = 0,
//                CooldownType = 0,
//                TemplateId = 6
                
//            };

//            return offer;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="unit"></param>
//        /// <param name="prefix"></param>
//        /// <param name="unlockAsPrereq">Optional. For the first level offer, specifies whether a generated unlock offer should be a prerequisite</param>
//        /// <param name="priority">Optional. The priority to assign to the offer, higher priorit offers will display before lower priority ones</param>
//        /// <returns></returns>
//        public List<Offer> CreateLevelOffers(Unit unit, string prefix, bool unlockAsPrereq = true, int priority = 0)
//        {
//            var offers = new List<Offer>();

//            if (unit == null || string.IsNullOrWhiteSpace(prefix))
//            {
//                throw new ArgumentException("Unit must not be null");
//            }

//            if (priority < 0)
//            {
//                priority = 0;
//            }

//            if (unit.Id == 311)
//            {
//                throw new NotImplementedException("Level offers for the Siege Squadron are not currently supported");
//            }
//            else
//            {
//                var skeletons = this.offerData.Skeletons
//                    .Where(c => c.UnitId == unit.Id && c.OfferType == OfferType.LEVELS)
//                    .ToList();

//                if (skeletons.Count == 0)
//                {
//                    // log.LogWarning("No skeletons were found for level offers for {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
//                    throw new NotImplementedException("Generating levels without a skeleton is not yet supported. Add a skeleton to the Offer Data Google Sheet to generate this offer");
//                }

//                var skeletonLevelDictionary = new Dictionary<int, OfferSkeleton>();
//                foreach (var skeleton in skeletons)
//                {
//                    try
//                    {
//                        var blob = JsonConvert.DeserializeObject<ContentBlob>(skeleton.Content);
//                        int level = blob.UnitUnlocks
//                            .Where(u => u.UnitId == unit.Id)
//                            .Select(u => u.Level)
//                            .FirstOrDefault();

//                        if (level > 0)
//                        {
//                            skeletonLevelDictionary.Add(level, skeleton);
//                        }
//                        else
//                        {
//                            log.LogError("Unable to determine a valid level for skeleton {0}", skeleton.Content.ToString());
//                        }
//                    }
//                    catch (JsonSerializationException)
//                    {
//                        log.LogError("Unable to parse offer content JSON for level up offer");
//                    }
//                }

//                var grades = unit.Levels.Select(l => l.Grade).Distinct().OrderBy(g => g);

//                foreach (var grade in unit.Levels.Select(l => l.Grade).Distinct().OrderBy(g => g))
//                {
//                    // all offers in this "grade" will be generated first, then ran through again in 
//                    // an ordered fashion to establish prerequisites
//                    var offerLevelDictionary = new Dictionary<int, Offer>();

//                    foreach (var levelInGrade in unit.Levels.Where(l => l.Grade == grade).Select(l => l.Number))
//                    {
//                        if (skeletonLevelDictionary.TryGetValue(levelInGrade, out OfferSkeleton skeleton))
//                        {
//                            var offer = new Offer
//                            {
//                                OfferCode = $"{prefix}Lv{levelInGrade}",
//                                Title = skeleton.Title,
//                                Description = skeleton.Description,
//                                IconTitle = skeleton.IconTitle,
//                                IconDescription = skeleton.IconDescription,
//                                Cost = skeleton.Cost,
//                                FullCost = skeleton.FullCost,
//                                CostSku = skeleton.CostSku,
//                                Duration = skeleton.Duration,
//                                ContentJson = skeleton.Content,
//                                DisplayedItemsJson = skeleton.DisplayedItems,
//                                StartTime = DateTimeOffset.Now,
//                                EndTime = DateTimeOffset.Now + TimeSpan.FromDays(3),
//                                Priority = priority,
//                                MaxQuantity = skeleton.MaximumQuanity,
//                                Cooldown = 0,
//                                CooldownType = 0,
//                                TemplateId = skeleton.TemplateId,
//                            };
//                            offerLevelDictionary.Add(levelInGrade, offer);
//                        }
//                    }

//                    var levels = offerLevelDictionary.Keys
//                        .OrderBy(k => k);

//                    for (int i = 0; i < levels.Count(); i++)
//                    {
//                        if (i == 0)
//                        {
//                            if (grade == 1 && unlockAsPrereq)
//                            {
//                                if (offerLevelDictionary.TryGetValue(levels.ElementAt(i), out Offer offer))
//                                {
//                                    // If this is the first offer of the first level grade, and "unlock as prereq"
//                                    // is true then add the expected unlock offer code as a prerequisite
//                                    offer.Prerequisite = $"{prefix}Unl";
//                                }
//                                else
//                                {
//                                    // TODO
//                                    log.LogWarning("What happened? Must be bad data, user error");
//                                }
//                            }
//                        }
//                        else
//                        {
//                            if (offerLevelDictionary.TryGetValue(levels.ElementAt(i), out Offer offer))
//                            {
//                                if (offerLevelDictionary.TryGetValue(levels.ElementAt(i-1), out Offer prereqOffer))
//                                {
//                                    offer.Prerequisite = prereqOffer.OfferCode;
//                                }
//                            }
//                        }
//                    }

//                    offers.AddRange(offerLevelDictionary.Values);
//                }



//                //var levelGradeDictionary = unit.Levels.ToDictionary(l => l.Number, l => l.Grade);

//                //if (skeletons.Count == 0)
//                //{
//                //    log.LogWarning("No skeletons were found for level offers for {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
//                //    throw new NotImplementedException("Generating levels without a skeleton is not yet supported. Add a skeleton to the Offer Data Google Sheet to generate this offer");
//                //    var grades = unit.Levels.Select(l => l.Grade).Distinct().OrderBy(g => g);
//                //    foreach (var grade in grades)
//                //    {
//                //        var gradeLevels = unit.Levels.Where(l => l.Grade == grade);
//                //        if (grade == 1)
//                //        {

//                //        }
//                //        else if (grade == 2)
//                //        {

//                //        }
//                //        else if (grade == 3)
//                //        {
//                //            log.LogWarning("Unsupported grade ({0}) found in unit data", grade);
//                //        }
//                //    }

//                //    return new List<Offer>();


//                //}
//                //else
//                //{
//                //    var offers = new List<Offer>();
//                //    var levelOffers = new Dictionary<int, Offer>();
//                //    for (int i = 0; i < skeletons.Count; i++)
//                //    {
//                //        var skeleton = skeletons[i];

//                //        int level = 0;
//                //        try
//                //        {
//                //            var unlocks = JsonConvert.DeserializeObject<List<UnitUnlock>>(skeleton.Content);
//                //            level = unlocks.Where(u => u.UnitId == unit.Id).Select(u => u.Level).FirstOrDefault();
//                //        }
//                //        catch (JsonSerializationException)
//                //        {
//                //            // TODO: more details
//                //            log.LogError("Unable to deserialize content blob from skeleton");
//                //            continue;
//                //        }

//                //        if (level <= 0)
//                //        {
//                //            // TODO: more clarity in error message
//                //            log.LogWarning("Unable to determine the level this skeleton is granting");
//                //            continue;
//                //        }

//                //        var offer = new Offer
//                //        {
//                //            OfferCode = $"{prefix}Lv{level}",
//                //            Title = skeleton.Title,
//                //            Description = skeleton.Description,
//                //            IconTitle = skeleton.IconTitle,
//                //            IconDescription = skeleton.IconDescription,
//                //            Cost = skeleton.Cost,
//                //            FullCost = skeleton.FullCost,
//                //            CostSku = skeleton.CostSku,
//                //            Duration = skeleton.Duration,
//                //            ContentJson = skeleton.Content,
//                //            DisplayedItemsJson = skeleton.DisplayedItems,
//                //            StartTime = DateTimeOffset.Now,
//                //            EndTime = DateTimeOffset.Now + TimeSpan.FromDays(3),
//                //            Priority = priority,
//                //            MaxQuantity = skeleton.MaximumQuanity,
//                //            Cooldown = 0,
//                //            CooldownType = 0,
//                //            TemplateId = skeleton.TemplateId,
//                //        };

//                //        levelOffers.Add(level, offer);
//                //    }

//                //    foreach (var grade in levelGradeDictionary.Values)
//                //    {
//                //        var levelsInGrade = levelGradeDictionary
//                //            .Where(d => d.Value == grade).Select(d => d.Key)
//                //            .OrderBy(l => l)
//                //            .ToList();

//                //        var offersInGrade = levelOffers
//                //            .Where(o => levelsInGrade.Contains(o.Key));

//                //        foreach (var level in levelsInGrade)
//                //        {
//                //            if (grade == 1 && unlockAsPrereq && level == levelsInGrade.Min())
//                //            {

//                //            }
//                //            else
//                //            {

//                //            }
//                //        }
//                //    }
//                //}

//                //return offers;
//            }

//            return offers;
//        }

//        public Offer CreateOmegaPartsOffer(Unit unit, string prefix, int priority = 0)
//        {
//            if (unit == null || string.IsNullOrWhiteSpace(prefix))
//            {
//                throw new ArgumentException("Unit must not be null");
//            }

//            if (priority < 0)
//            {
//                priority = 0;
//            }

//            var skeleton = this.offerData.Skeletons.FirstOrDefault(c => c.UnitId == unit.Id && c.OfferType == OfferType.OMEGA_PARTS);
//            // If nothing was found, use generic alternative with functionally empty content & displayed item blocks
//            if (skeleton == null)
//            {
//                log.LogWarning("No custom copy exists for an unlock of {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
//                skeleton = new OfferSkeleton
//                {
//                    Title = $"Omega {unit.Name} Parts!",
//                    Description = $"Offer includes OMEGA PARTS for the {unit.Name.ToUpper()}!",
//                    IconTitle = $"{unit.Name} Omega Parts!",
//                    IconDescription = $"Offer includes {unit.Name.ToUpper()} Omega Parts!",
//                    Duration = 86400,
//                    Cost = 99,
//                    FullCost = -1,
//                    CostSku = "gold",
//                    Content = "{ \"skus\": { \"gold\": 0 } }",
//                    DisplayedItems = "[ {} ]",
//                    MaximumQuanity = 1
//                };
//            }

//            var offer = new Offer
//            {
//                OfferCode = $"{prefix}OPts",
//                Title = skeleton.Title,
//                Description = skeleton.Description,
//                IconTitle = skeleton.IconTitle,
//                IconDescription = skeleton.IconDescription,
//                Cost = skeleton.Cost,
//                FullCost = skeleton.FullCost,
//                CostSku = skeleton.CostSku,
//                Duration = skeleton.Duration,
//                ContentJson = skeleton.Content,
//                DisplayedItemsJson = skeleton.DisplayedItems,
//                StartTime = DateTimeOffset.Now,
//                EndTime = DateTimeOffset.Now + TimeSpan.FromDays(3),
//                Priority = priority,
//                MaxQuantity = skeleton.MaximumQuanity,
//                Cooldown = 0,
//                CooldownType = 0,
//                TemplateId = 6

//            };

//            return offer;
//        }

//        public Offer CreateElitePartsOffer(Unit unit, string prefix, int priority = 0)
//        {
//            if (unit == null || string.IsNullOrWhiteSpace(prefix))
//            {
//                throw new ArgumentException("Unit must not be null");
//            }

//            if (priority < 0)
//            {
//                priority = 0;
//            }

//            var skeleton = this.offerData.Skeletons.FirstOrDefault(c => c.UnitId == unit.Id && c.OfferType == OfferType.ELITE_PARTS);
//            // If nothing was found, use generic alternative with functionally empty content & displayed item blocks
//            if (skeleton == null)
//            {
//                log.LogWarning("No skeleton was found for an elite parts offer for {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
//                skeleton = new OfferSkeleton
//                {
//                    Title = $"Elite {unit.Name} Parts!",
//                    Description = $"Offer includes ELITE PARTS for the {unit.Name.ToUpper()}!",
//                    IconTitle = $"{unit.Name} Elite Parts!",
//                    IconDescription = $"Offer includes {unit.Name.ToUpper()} Elite Parts!",
//                    Duration = 86400,
//                    Cost = 99,
//                    FullCost = -1,
//                    CostSku = "gold",
//                    Content = "{ \"skus\": { \"gold\": 0 } }",
//                    DisplayedItems = "[ {} ]",
//                    MaximumQuanity = 1
//                };
//            }

//            var offer = new Offer
//            {
//                OfferCode = $"{prefix}EPts",
//                Title = skeleton.Title,
//                Description = skeleton.Description,
//                IconTitle = skeleton.IconTitle,
//                IconDescription = skeleton.IconDescription,
//                Cost = skeleton.Cost,
//                FullCost = skeleton.FullCost,
//                CostSku = skeleton.CostSku,
//                Duration = skeleton.Duration,
//                ContentJson = skeleton.Content,
//                DisplayedItemsJson = skeleton.DisplayedItems,
//                StartTime = DateTimeOffset.Now,
//                EndTime = DateTimeOffset.Now + TimeSpan.FromDays(3),
//                Priority = priority,
//                MaxQuantity = skeleton.MaximumQuanity,
//                Cooldown = 0,
//                CooldownType = 0,
//                TemplateId = 6
//            };

//            return offer;
//        }

//        public List<Offer> CreateTechOffers(Unit unit, string prefix, int priority = 0)
//        {
//            if (unit == null || string.IsNullOrWhiteSpace(prefix))
//            {
//                throw new ArgumentException("Unit must not be null");
//            }

//            if (priority < 0)
//            {
//                priority = 0;
//            }

//            var skeletons = this.offerData.Skeletons
//                .Where(c => c.UnitId == unit.Id && c.OfferType == OfferType.TECH)
//                .ToList();

//            if (skeletons.Count == 0)
//            {
//                log.LogWarning("No skeletons were found for a tech offer for {0} ({1}); a generic offer will be created with no content or displayed item values", unit.Name, unit.Id);
//                skeletons.Add(new OfferSkeleton
//                {
//                    Title = $"Tech out your {unit.Name}!",
//                    Description = $"Offer includes TECH for the {unit.Name.ToUpper()}!",
//                    IconTitle = $"{unit.Name} Tech!",
//                    IconDescription = $"Offer includes {unit.Name.ToUpper()} Tech!",
//                    Duration = 86400,
//                    Cost = 99,
//                    FullCost = -1,
//                    CostSku = "gold",
//                    Content = "{ \"skus\": { \"gold\": 0 } }",
//                    DisplayedItems = "[ {} ]",
//                    MaximumQuanity = 1
//                });
//            }

//            var offers = new List<Offer>();
//            for(int i = 0; i < skeletons.Count; i++)
//            {
//                var skeleton = skeletons[i];

//                var offer = new Offer
//                {
//                    OfferCode = $"{prefix}Tec{i+1}",
//                    Title = skeleton.Title,
//                    Description = skeleton.Description,
//                    IconTitle = skeleton.IconTitle,
//                    IconDescription = skeleton.IconDescription,
//                    Cost = skeleton.Cost,
//                    FullCost = skeleton.FullCost,
//                    CostSku = skeleton.CostSku,
//                    Duration = skeleton.Duration,
//                    ContentJson = skeleton.Content,
//                    DisplayedItemsJson = skeleton.DisplayedItems,
//                    StartTime = DateTimeOffset.Now,
//                    EndTime = DateTimeOffset.Now + TimeSpan.FromDays(3),
//                    Priority = priority,
//                    MaxQuantity = skeleton.MaximumQuanity,
//                    Cooldown = 0,
//                    CooldownType = 0,
//                    TemplateId = 6,
//                    Prerequisite = $"{prefix}Unl"
//                };
//                offers.Add(offer);
//            }

//            return offers;
//        }



//        /// <summary>
//        /// Fetches a cohort of users who do not have the specified unit unlocked and are thus eligible to receive the unlock offer
//        /// </summary>
//        /// <param name="unit"></param>
//        //public List<int> FetchUnlockCohort(Unit unit)
//        //{
//        //    return this.unitOwnership.FetchUnitOwnerUserIds(unit.Id);
//        //}
//    }

//}
