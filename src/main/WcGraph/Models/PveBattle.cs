using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class PveBattle
    {
        public virtual User LaunchedBy { get; set; }

        public virtual BaseInstance Target { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public int RubiSessions { get; set; }

        public int RubiDuration { get; set; }

        public int Duration { get; set; }

        /// <summary>
        /// Total amount of damage dealt by the attacker
        /// </summary>
        public int DamageDealt { get; set; }

        /// <summary>
        /// Total amount of damage received by the attacker
        /// </summary>
        public int DamageReceived { get; set; }

        public int MissilesUsed { get; set; }

        public int MissilesShotDown { get; set; }
    }
}
