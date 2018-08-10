using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.Snowflake.Models
{
    public class Location
    {
        public Location(int sector, int x, int y)
        {
            Sector = sector;
            X = x;
            Y = y;
        }
        public int Sector { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
