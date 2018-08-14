using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.Models
{
    public class PveBattle
    {

        public string Id { get; set; }
    
        public int AttackerId { get; set; }

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

        public Dictionary<int, Platoon> PlatoonStaging { get; } = new Dictionary<int, Platoon>(); // { { 0, null }, { 1, null }, { 2, null }, { 3, null }, { 4, null }, { 5, null }, };

        /// <summary>
        /// Returns the platoon that is staged for this attack on the specified hex position, 0-5
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public Platoon GetPlatoonOnHex(int hex)
        {
            return PlatoonStaging.GetValueOrDefault(hex);
        }


    }
}
