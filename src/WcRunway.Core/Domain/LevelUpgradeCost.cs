using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain
{
    public class LevelUpgradeCost
    {
        public int FromLevel { get; set; }
        public int ToLevel { get; set; }

        public int Metal { get; set; }

        public int Oil { get; set; }

        public int Gold { get; set; }

        public int Thorium { get; set; }

        public Dictionary<string, int> SkuCosts { get; set; } = new Dictionary<string, int>();
    }
}
