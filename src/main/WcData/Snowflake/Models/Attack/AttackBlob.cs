using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.Implementation.Snowflake;
using WcData.Implementation.Snowflake.Converters;

namespace WcData.Snowflake.Models.Attack
{
    public class AttackBlob
    {
        [JsonProperty("alliance_data")]
        [JsonConverter(typeof(EmbeddedJsonConverter))]
        public Alliance AllianceData { get; set; }

        [JsonProperty("analytics_tag")]
        public string AnalyticsTag { get; set; }

        [JsonProperty("attack_id")]
        public string AttackId { get; set; }

        [JsonProperty("attack_location")]
        public string AttackLocation { get; set; }

        [JsonProperty("attacker_buff_used")]
        public int AttackerBuffUsed { get; set; }

        [JsonProperty("attacker_details")]
        public string AttackerDetails { get; set; }

        [JsonProperty("attacker_fireteams")]
        public string AttackerFireteams { get; set; }

        [JsonProperty("attacker_platoon_metadata")]
        [JsonConverter(typeof(EmbeddedJsonConverter))]
        public List<PlatoonAttackStagingLocation> AttackerPlatoonStagingLocations { get; set; }

        [JsonProperty("attacker_rubi_duration")]
        public int AttackerRubiDuration { get; set; }

        [JsonProperty("attacker_rubi_sessions")]
        public int AttackerRubiSessions { get; set; }

        [JsonProperty("attacker_thorium_total")]
        public int AttackerThoriumTotal { get; set; }

        [JsonProperty("attacker_units_killed")]
        public string AttackerUnitsKilled { get; set; }

        [JsonProperty("attacker_x")]
        public int AttackerX { get; set; }

        [JsonProperty("attacker_y")]
        public int AttackerY { get; set; }

        [JsonProperty("base_defender_units")]
        public string BaseDefenderUnits { get; set; }

        [JsonProperty("battle_duration")]
        public int BattleDuration { get; set; }

        [JsonProperty("battle_mode")]
        public string BattleMode { get; set; }

        [JsonProperty("damage_to_attacker")]
        public int DamageToAttacker { get; set; }

        [JsonProperty("damage_to_defender")]
        public int DamageToDefender { get; set; }

        [JsonProperty("defender_bastions")]
        public string DefenderBastions { get; set; }

        [JsonProperty("defender_buff_used")]
        public int DefenderBuffUsed { get; set; }

        [JsonProperty("defender_details")]
        public string DefenderDetails { get; set; }

        [JsonProperty("defender_id")]
        public int DefenderId { get; set; }

        [JsonProperty("defender_level")]
        public int DefenderLevel { get; set; }

        [JsonProperty("defender_rubi_duration")]
        public int DefenderRubiDuration { get; set; }

        [JsonProperty("defender_rubi_sessions")]
        public int DefenderRubiSessions { get; set; }

        [JsonProperty("defender_units_killed")]
        public string DefenderUnitsKilled { get; set; }

        [JsonProperty("defender_x")]
        public int DefenderX { get; set; }

        [JsonProperty("defender_y")]
        public int DefenderY { get; set; }

        [JsonProperty("deposit_size")]
        public int DepositSize { get; set; }

        [JsonProperty("deposit_type")]
        public int DepositType { get; set; }

        [JsonProperty("dt_pst")]
        public DateTime DtPst { get; set; }

        [JsonProperty("enemy_type")]
        public string EnemyType { get; set; }

        [JsonProperty("env")]
        public string Env { get; set; }

        [JsonProperty("faction_id")]
        public int FactionId { get; set; }

        [JsonProperty("g")]
        public string GameId { get; set; }

        [JsonProperty("hr_pst")]
        public int HrPst { get; set; }

        [JsonProperty("http_referer")]
        public string HttpReferer { get; set; }

        [JsonProperty("k")]
        public string K { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("l")]
        public string L { get; set; }

        [JsonProperty("mercenaries_used")]
        public int MercenariesUsed { get; set; }

        [JsonProperty("metal")]
        public int Metal { get; set; }

        [JsonProperty("missiles_shot_down")]
        public int MissilesShotDown { get; set; }

        [JsonProperty("missiles_used")]
        public int MissilesUsed { get; set; }

        [JsonProperty("number_platoons")]
        public int NumberPlatoons { get; set; }

        [JsonProperty("number_squads_deployed")]
        public int NumberSquadsDeployed { get; set; }

        [JsonProperty("number_squads_retreated")]
        public int NumberSquadsRetreated { get; set; }

        [JsonProperty("oil")]
        public int Oil { get; set; }

        [JsonProperty("p")]
        public string P { get; set; }

        [JsonProperty("retreat")]
        public int Retreat { get; set; }

        [JsonProperty("rx_ts")]
        [JsonConverter(typeof(EpochToDateTimeOffsetConverter))]
        public DateTimeOffset RxTs { get; set; }

        [JsonProperty("s")]
        public string S { get; set; }

        [JsonProperty("sector")]
        public int Sector { get; set; }

        [JsonProperty("synchronous")]
        public int Synchronous { get; set; }

        [JsonProperty("t")]
        [JsonConverter(typeof(EpochToDateTimeOffsetConverter))]
        public DateTimeOffset T { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("tag_set")]
        public string TagSet { get; set; }

        [JsonProperty("thorium")]
        public int Thorium { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("u")]
        public string U { get; set; }

        [JsonProperty("units_deployed")]
        [JsonConverter(typeof(EmbeddedJsonConverter))]
        public List<DeployedUnit> UnitsDeployed { get; set; }

        [JsonProperty("win")]
        public int Win { get; set; }
    }


    //[JsonProperty("alliance_data")]
    //public Alliance Alliance { get; set; }

    //[JsonProperty("attack_id")]
    //public string AttackId { get; set; }

    //[JsonProperty("attacker_buff_used")]
    //public int AttackerBuff { get; set; }

    //[JsonProperty("analytics_tag")]
    //public string AnalyticsTag { get; set; }

    //[JsonProperty("attack_location")]
    //public string AttackLocation { get; set; } // TODO: what should this actually be called?

    //[JsonProperty("attacker_details")]
    //public string attacker_details { get; set; }

    //[JsonProperty("attacker_fireteams")]
    //public string attacker_fireteams { get; set; }

    //[JsonProperty("attacker_platoon_metadata")]
    //public List<PlatoonAttackStagingLocation> PlatoonStaging { get; set; }

    //[JsonProperty("attacker_rubi_duration")]
    //public string attacker_rubi_duration { get; set; }

    //[JsonProperty("attacker_rubi_sessions")]
    //public string attacker_rubi_sessions { get; set; }

    //[JsonProperty("attacker_thorium_total")]
    //public string attacker_thorium_total { get; set; }

    //[JsonProperty("attacker_units_killed")]
    //public string attacker_units_killed { get; set; }

    //[JsonProperty("attacker_x")]
    //public string attacker_x { get; set; }

    //[JsonProperty("attacker_y")]
    //public string attacker_y { get; set; }

    //[JsonProperty("base_defender_units")]
    //public string base_defender_units { get; set; }

    //[JsonProperty("battle_duration")]
    //public string battle_duration { get; set; }

    //[JsonProperty("battle_mode")]
    //public string battle_mode { get; set; }

    //[JsonProperty("damage_to_attacker")]
    //public string damage_to_attacker { get; set; }

    //[JsonProperty("damage_to_defender")]
    //public string damage_to_defender { get; set; }

    //[JsonProperty("defender_bastions")]
    //public string defender_bastions { get; set; }

    //[JsonProperty("defender_buff_used")]
    //public string defender_buff_used { get; set; }

    //[JsonProperty("defender_details")]
    //public string defender_details { get; set; }

    //[JsonProperty("defender_id")]
    //public string defender_id { get; set; }

    //[JsonProperty("defender_level")]
    //public string defender_level { get; set; }

    //[JsonProperty("defender_rubi_duration")]
    //public string defender_rubi_duration { get; set; }

    //[JsonProperty("defender_rubi_sessions")]
    //public string defender_rubi_sessions { get; set; }

    //[JsonProperty("defender_units_killed")]
    //public string defender_units_killed { get; set; }

    //[JsonProperty("defender_x")]
    //public string defender_x { get; set; }

    //[JsonProperty("defender_y")]
    //public string defender_y { get; set; }

    //[JsonProperty("deposit_size")]
    //public string deposit_size { get; set; }

    //[JsonProperty("deposit_type")]
    //public string deposit_type { get; set; }

    //[JsonProperty("dt_pst")]
    //public string dt_pst { get; set; }

    //[JsonProperty("enemy_type")]
    //public string enemy_type { get; set; }

    //[JsonProperty("env")]
    //public string env { get; set; }

    //[JsonProperty("faction_id")]
    //public string faction_id { get; set; }

    //[JsonProperty("g")]
    //public string g { get; set; }

    //[JsonProperty("hr_pst")]
    //public string hr_pst { get; set; }

    //[JsonProperty("http_referer")]
    //public string http_referer { get; set; }

    //[JsonProperty("k")]
    //public string k { get; set; }

    //[JsonProperty("key")]
    //public string key { get; set; }

    //[JsonProperty("l")]
    //public string l { get; set; }

    //[JsonProperty("mercenaries_used")]
    //public string mercenaries_used { get; set; }

    //[JsonProperty("metal")]
    //public string metal { get; set; }

    //[JsonProperty("missiles_shot_down")]
    //public string missiles_shot_down { get; set; }

    //[JsonProperty("missiles_used")]
    //public string missiles_used { get; set; }

    //[JsonProperty("number_platoons")]
    //public string number_platoons { get; set; }

    //[JsonProperty("number_squads_deployed")]
    //public string number_squads_deployed { get; set; }

    //[JsonProperty("number_squads_retreated")]
    //public string number_squads_retreated { get; set; }

    //[JsonProperty("oil")]
    //public string oil { get; set; }

    //[JsonProperty("p")]
    //public string p { get; set; }

    //[JsonProperty("retreat")]
    //public string retreat { get; set; }

    //[JsonProperty("rx_ts")]
    //public string rx_ts { get; set; }

    //[JsonProperty("s")]
    //public string s { get; set; }

    //[JsonProperty("sector")]
    //public string sector { get; set; }

    //[JsonProperty("synchronous")]
    //public string synchronous { get; set; }

    //[JsonProperty("t")]
    //public string t { get; set; }

    //[JsonProperty("tag")]
    //public string tag { get; set; }

    //[JsonProperty("tag_set")]
    //public string tag_set { get; set; }

    //[JsonProperty("thorium")]
    //public string thorium { get; set; }

    //[JsonProperty("type")]
    //public string type { get; set; }

    //[JsonProperty("u")]
    //public string u { get; set; }

    //[JsonProperty("units_deployed")]
    //public string units_deployed { get; set; }

    //[JsonProperty("win")]
    //public string win { get; set; }

}
