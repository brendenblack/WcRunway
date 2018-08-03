using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Implementation.Sheets
{
    public abstract class BaseSheetsData
    {
        public DateTime LastUpdate { get; protected set; } = DateTime.MinValue;
        public TimeSpan Validity { get; protected set; } = TimeSpan.FromMinutes(30);
        public bool IsStale
        {
            get
            {
                return DateTime.Now > (LastUpdate + Validity);
            }
        }
    }
}
