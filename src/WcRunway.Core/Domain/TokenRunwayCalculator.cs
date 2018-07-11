using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcRunway.Core.Domain
{
    public class TokenRunwayCalculator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="ownership">A dictionary where the key is a <see cref="User"/> ID, and the value is the level that they own</param>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, int>> GetRunwayForUnit(Unit unit, Dictionary<int, int> ownership)
        {
            var result = new Dictionary<int, Dictionary<string,int>>();
            var costIndex = GetUpgradeCostDictionary(unit);
            
            foreach (var level in unit.Levels)
            {
                var ownersAtLevel = ownership
                    .Where(o => o.Value == level.Number)
                    .Select(o => o.Key)
                    .ToList();

                foreach (var owner in ownersAtLevel)
                {
                    var cost = costIndex.First(c => c.Key == level.Number).Value;
                    result.Add(owner, cost.SkuCosts);
                }
            }

            return result;
        }




        /// <summary>
        /// For a given <see cref="Unit"/>, calculate the remaining upgrade cost for each level
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public Dictionary<int, LevelUpgradeCost> GetUpgradeCostDictionary(Unit unit)
        {
            var result = new Dictionary<int, LevelUpgradeCost>();

            foreach (var level in unit.Levels)
            {
                var cost = CalculateRemainingCostsAtLevel(unit, level.Number);
                result.Add(level.Number, cost);
            }

            return result;
        }

        /// <summary>
        /// Computes the total remaining cost per resource type of a <see cref="Unit"/> for the given level
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public LevelUpgradeCost CalculateRemainingCostsAtLevel(Unit unit, int level)
        {
            var levelsToGo = unit.Levels
                .Where(l => l.Number > level)
                .ToList();
            
            var totalMetalCost = levelsToGo.Select(l => l.UpgradeCostMetal).Sum();
            var totalOilCost = levelsToGo.Select(l => l.UpgradeCostOil).Sum();
            var totalThoriumCost = levelsToGo.Select(l => l.UpgradeCostThorium).Sum();
            var totalGoldCost = levelsToGo.Where(l => l.UpgradeCostGold > 0).Select(l => l.UpgradeCostGold).Sum();


            var skus = levelsToGo.SelectMany(l => l.UpgradeSkuCosts.Select(c => c.Sku)).Distinct();
            var result = new Dictionary<string, int>();
            foreach (var sku in skus)
            {
                var skuTotal = levelsToGo.SelectMany(l => l.UpgradeSkuCosts.Where(c => c.Sku == sku).Select(c => c.Quantity)).Sum();

                if (result.ContainsKey(sku))
                {
                    result.TryGetValue(sku, out int currentSkuTotal);
                    result[sku] = currentSkuTotal + skuTotal;
                }
                else
                {
                    result.Add(sku, skuTotal);
                }
            }

            var totalCost = new LevelUpgradeCost();
            totalCost.FromLevel = level;
            totalCost.ToLevel = unit.Levels.Select(l => l.Number).Max();
            totalCost.Metal = totalMetalCost;
            totalCost.Oil = totalOilCost;
            totalCost.Thorium = totalThoriumCost;
            totalCost.SkuCosts = result;

            return totalCost;
        }

    }
}
