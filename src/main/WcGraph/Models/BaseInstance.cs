using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class BaseInstance
    {
        public Base Base { get; set; }

        public int Sector { get; set; }

        public int XCoordinate { get; set; }
        
        public int YCoordinate { get; set; }

        public string Id
        {
            get
            {
                return $"{Base.Type}-{Base.Level}-{Sector}-{XCoordinate}-{YCoordinate}";
            }
        }
    }
}
