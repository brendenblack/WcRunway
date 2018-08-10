using System;
using System.Collections.Generic;
using System.Text;

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


    }
}
