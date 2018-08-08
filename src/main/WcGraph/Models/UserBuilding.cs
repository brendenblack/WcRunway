using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class UserBuilding
    {
        public User User { get; set; }

        public Building Building { get; set; }

        public int Id { get; set; }

        public List<BuildingLevel> LevelsObtained { get; set; } = new List<BuildingLevel>();

    }
}
