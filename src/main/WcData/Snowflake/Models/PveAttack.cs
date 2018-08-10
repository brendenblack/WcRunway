using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcData.Snowflake.Models.Attack;

namespace WcData.Snowflake.Models
{
    public class PveAttack
    {
        public string Id { get; set; }

        public int AttackerId { get; set; }

        public Location AttackerLocation { get; set; }

        public string DefenderType { get; set; }

        public int DefenderLevel { get; set; }

        public Location DefenderLocation { get; set; }

        public long Duration { get; set; }

        public long DamageToAttacker { get; set; }

        public long DamageToDefender { get; set; }

        public int MissilesUsed { get; set; }

        public int MissilesShotDown { get; set; }

        public int NumberOfSquadsDeployed { get; set; }

        public int NumberOfSquadsRetreated { get; set; }

        public int MetalClaimed { get; set; }
        public int OilClaimed { get; set; }
        public int ThoriumClaimed { get; set; }

        public Dictionary<int, int> BaseDefenders { get; } = new Dictionary<int, int>();

        public List<PlatoonAttackStagingLocation> AttackingPlatoonLocations { get; } = new List<PlatoonAttackStagingLocation>();

        public string GetPlatoonIdAtHex(int hex)
        {
            return AttackingPlatoonLocations
                .Where(pl => pl.Hex == hex)
                .Select(pl => pl.PlatoonId)
                .FirstOrDefault();
        }

        public List<DeployedUnit> UnitsDeployed { get; set; } = new List<DeployedUnit>();
    }
}
