using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.Models;

namespace WcGraph.Data
{
    public class ClientPveBattleRepository
    {
        public ClientPveBattleRepository()
        {
            
        }

        public void AddBattle(PveBattle battle)
        {
            var g = new Neo4jClient.BoltGraphClient("bolt://127.0.0.1:7687/graph/db", "neo4j", "password");
            g.Connect();
            //var config = NeoServerConfiguration.GetConfiguration(new Uri("bolt://127.0.0.1:7687"), "neo4j", "password");
            //var factory = new GraphClientFactory(config);
            
            
            g.Cypher
                .Merge("(a:PveAttack { id: {attackid} })")
                .OnCreate()
                .Set("a = {battle}")
                .Merge("(u:User { id: {userid} })")
                .Merge("(bt:BaseTemplate { level: {baselevel}, type: {basetype} })")
                .Merge("(b:Base { targetid: {targetid} })")
                .OnCreate()
                .Set("b = {target}")
                .Merge("(b)-[:INSTANCE_OF]->(bt)")
                .Merge("(u)-[:LAUNCHED_ATTACK]->(a)")
                .Merge("(a)-[:ATTACK_ON]->(b)")
                .WithParams(new
                {
                    attackid = battle.Id,
                    battle = new
                    {
                        damage_dealt = battle.DamageDealt,
                        damage_received = battle.DamageReceived,
                        duration = battle.Duration,
                        missiles_shot_down = battle.MissilesShotDown,
                        missiles_used = battle.MissilesUsed,
                        rubi_sessions = battle.RubiSessions,
                        rubi_duration = battle.RubiDuration
                    },
                    userid = battle.AttackerId,
                    basetype = battle.Target.Base.Type,
                    baselevel = battle.Target.Base.Level,
                    targetid = battle.Target.Id,
                    target = new
                    {
                        targetid = battle.Target.Id,
                        sector = battle.Target.Sector,
                        x = battle.Target.XCoordinate,
                        y = battle.Target.YCoordinate
                    },
                    hex0 = ""
                    
                })
                .ExecuteWithoutResults();

            foreach (var pl in battle.PlatoonStaging)
            {
                g.Cypher.Match("(u:User { id: {userid} })")
                    .Merge("(p:Platoon { id: {platoonid} })")
                    .Merge("(u)-[:COMMANDS]->(p)")
                    .Merge("(p)-[:DEPLOYED {hex: {hex}}]->(a:PveAttack { id: {attackid} })")
                    .WithParams(new
                     {
                         attackid = battle.Id,
                         userid = battle.AttackerId,
                         platoonid = pl.Value.Id,
                         hex = pl.Key
                    })
                    .ExecuteWithoutResults();
            }

        }
    }
}
