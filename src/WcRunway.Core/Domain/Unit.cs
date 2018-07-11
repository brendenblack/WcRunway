using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcRunway.Core.Domain
{
    public class Unit
    {
        public Unit(int id)
        {
            this.Id = id;
        }

        public int Id { get; private set; }
        public string Key { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string UnlockSku { get; set; }

        public List<Level> Levels { get; set; } = new List<Level>();

        public int MaxLevel
        {
            get
            {
                return Levels.Select(l => l.Number).Max();
            }
        }

        public IEnumerable<string> LevelUpSkus
        {
            get
            {
                return Levels.SelectMany(l => l.UpgradeSkuCosts.Select(s => s.Sku));
            }
        }
    }
}
