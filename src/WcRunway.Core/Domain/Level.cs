﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain
{
    public class Level
    {
        public int Number { get; set; }

        /// <summary>
        /// Differentiates different "stages" of leveling, e.g. 1-20 is normal, then an increasing token cost
        /// </summary>
        public int Grade { get; set; }

        public int UpgradeCostMetal { get; set; } = -1;
        public int UpgradeCostOil { get; set; } = -1;
        public int UpgradeCostThorium { get; set; } = -1;
        public int UpgradeCostGold { get; set; } = -1;

        public LevelUpgradeCost UpgradeCost { get; set; }


        public int UpgradeAcademyLevel { get; set; }


        public string UpgradeSku { get; set; }
        public List<UpgradeSkuCost> UpgradeSkuCosts { get; set; } = new List<UpgradeSkuCost>();

        public void AddUpgradeSkuCost(string sku, int quantity)
        {
            this.UpgradeSkuCosts.Add(new UpgradeSkuCost(sku, quantity));
        }

        private int UpgradeTime { get; set; }
        
    }
}
