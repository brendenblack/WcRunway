using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class UnitLevel
    {
        public Unit Unit { get; set; }

        public int Number { get; set; }

        public int Grade { get; set; }

        public int UpgradeCostMetal { get; set; } = -1;
        public int UpgradeCostOil { get; set; } = -1;
        public int UpgradeCostThorium { get; set; } = -1;
        public int UpgradeCostGold { get; set; } = -1;
    }
}
