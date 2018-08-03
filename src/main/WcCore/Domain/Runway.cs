using System;
using System.Collections.Generic;
using System.Text;

namespace WcCore.Domain
{
    public class Runway
    {
        public Unit Unit { get; set; }

        public IEnumerable<string> Skus
        {
            get
            {
                return Unit.LevelUpSkus;
            }
        }

        public int GetRunwayForSku(string sku)
        {
            return 0;
        }

    }
}
