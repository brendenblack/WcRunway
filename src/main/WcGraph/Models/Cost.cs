using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class Cost
    {
        public int Metal { get; set; }
        public int Oil { get; set; }
        public int Thorium { get; set; }

        public long DurationSeconds { get; set; }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds(DurationSeconds);
            }
            //set
            //{
            //    DurationSeconds = value.TotalSeconds;
            //}
        }

        public int Gold { get; set; }

        public int Medals { get; set; }

        public int BloodThorium { get; set; }
    }
}
