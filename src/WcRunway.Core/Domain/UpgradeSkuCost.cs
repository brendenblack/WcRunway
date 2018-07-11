﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain
{
    public class UpgradeSkuCost
    {
        public UpgradeSkuCost(string sku, int quantity)
        {
            this.Sku = sku;
            this.Quantity = quantity;
        }

        public string Sku { get; private set; }
        public int Quantity { get; private set; }
    }
}
