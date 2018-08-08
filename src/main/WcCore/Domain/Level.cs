using System;
using System.Collections.Generic;
using System.Text;

namespace WcCore.Domain
{
    public class Level
    {
        public int Number { get; set; }

        /// <summary>
        /// Differentiates different "stages" of leveling, e.g. for faction units 1-10 is normal, thereafter an increasing token cost scheme is applied
        /// </summary>
        public int Grade { get; set; }

        public int UpgradeCostMetal { get; set; } = -1;
        public int UpgradeCostOil { get; set; } = -1;
        public int UpgradeCostThorium { get; set; } = -1;
        public int UpgradeCostGold { get; set; } = -1;

        public int UpgradeTimeSeconds { get; set; } = 0;

        public TimeSpan UpgradeTime
        {
            get
            {
                return TimeSpan.FromSeconds(UpgradeTimeSeconds);
            }
            set
            {
                UpgradeTimeSeconds = Convert.ToInt32(value.TotalSeconds);
            }
        }

        public LevelUpgradeCost UpgradeCost { get; set; }



        public string UpgradeSku { get; set; }
        public List<UpgradeSkuCost> UpgradeSkuCosts { get; set; } = new List<UpgradeSkuCost>();

        public void AddUpgradeSkuCost(string sku, int quantity)
        {
            this.UpgradeSkuCosts.Add(new UpgradeSkuCost(sku, quantity));
        }
        
    }
}
