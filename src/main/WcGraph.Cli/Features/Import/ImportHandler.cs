using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using WcData.GameContext;
using WcData.Sheets;
using WcData.Snowflake;
using WcGraph.Data;
using WcGraph.Infrastructure;
using WcGraph.Models;

namespace WcGraph.Cli.Features.Import
{
    public class ImportHandler
    {
        private readonly ILogger<ImportHandler> logger;
        private readonly IMapper mapper;
        private readonly IGameData gameData;
        private readonly LiveSlaveContext db;
        private readonly IPveBattles pve;
        private readonly long LAST_SEEN_CUTOFF = (DateTimeOffset.Now - TimeSpan.FromDays(7)).ToUnixTimeSeconds();

        public ImportHandler(ILogger<ImportHandler> logger, IMapper mapper, IGameData gameData, LiveSlaveContext db, IPveBattles pve)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.gameData = gameData;
            this.db = db;
            this.pve = pve;
        }

        public int Execute(ImportOptions opts)
        {
            if (opts.ShouldImportUnits)
            {
                try
                {
                    ImportUnits(opts.ShouldImportUnitLevels);
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return -1;
                }
            }

            if (opts.ShouldImportUsers)
            {
                try
                {
                    ImportUsers();
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return -1;
                }
            }

            if (opts.ShouldImportUnitOwnership)
            {
                try
                {
                    ImportUnitOwnership();
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return -1;
                }
            }

            if (opts.ShouldImportPveAttacks)
            {
                ImportBattles();
            }

            return 0;

        }

        public void ImportUnits(bool importLevels = false)
        {
            if (importLevels)
            {
                // TODO
            }


            var units = gameData.Units
                .Select(u => new Models.Unit
                {
                    Id = u.Id,
                    Identifier = u.Identifier,
                    Name = u.Name,
                    Key = u.Key
                })
                .ToList();

            string cypher = new StringBuilder()
                    .AppendLine("UNWIND {units} as unit")
                    .AppendLine("MERGE (u:Unit { id: unit.id })")
                    .AppendLine("SET u = unit")
                    .ToString();



            IDriver driver = GraphDatabase.Driver("bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "password"));
            using (var session = driver.Session())
            {
                session.Run(cypher, new Dictionary<string, object>() { { "units", ParameterSerializer.ToDictionary(units) } });
            }
        }

        public void ImportUsers()
        {
            var timer = new Stopwatch();
            timer.Start();

            Console.WriteLine("Beginning query");
            var users = this.db.Users
                .Where(u => u.LastSeenEpochSeconds >= 1525882590)
                .Select(u => new Models.User
                {
                    Id = u.Id,
                    //FBID = u.FacebookId,
                    //KXID = u.KixeyeId,
                    //AddTime = u.AddTimeEpochSeconds,
                    //EmailAddress = u.EmailAddress
                })
                .AsNoTracking()
                .ToList();
            timer.Stop();

            Console.WriteLine("Query finished after {0}", timer.Elapsed);


            string cypher = new StringBuilder()
                .AppendLine("UNWIND {users} as user")
                .AppendLine("MERGE (p:User { id: user.id })")
                .AppendLine("SET p = user")
                .ToString();

            timer.Start();
            Console.WriteLine("Beginning cypher insert");

            IDriver driver = GraphDatabase.Driver("bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "password"));
            using (var session = driver.Session())
            {
                Console.WriteLine("Creating index");
                session.Run("CREATE INDEX ON :User(id)");
                Console.WriteLine("Index created");

                var result = session.Run(cypher, new Dictionary<string, object>() { { "users", ParameterSerializer.ToDictionary(users) } });

                var summary = result.Consume();

                Console.WriteLine($"[{DateTime.Now}] [Users] #NodesCreated: {summary.Counters.NodesCreated}, #RelationshipsCreated: {summary.Counters.RelationshipsCreated}");
            }

            timer.Stop();
            Console.WriteLine("Insert finished after {0}", timer.Elapsed);

        }

        public void ImportBases()
        {

        }

        // TODO: how to parameterize this
        public void ImportBattles()
        {
            var userId = 34359485; // TODO: test value

           var user = db.Users.FirstOrDefault(u => u.Id == userId);

            var user2 = db.Users
                .Where(u => u.Id == userId)
                .ProjectTo<User>(mapper.ConfigurationProvider)
                .FirstOrDefault();


            if (user == null || user2 == null)
            {
                throw new KeyNotFoundException($"Unable to find a user with id {userId}");
            }

            var attacks = pve.FetchAttacksByUser(user.Id, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now);
            var test = new UnitRepository();
            foreach (var attack in attacks)
            {
                var battle = mapper.Map<PveBattle>(attack);



                //foreach (var staging in attack.AttackerPlatoonStagingLocations)
                //{
                //    battle.PlatoonStaging[staging.Hex] = new Platoon { Owner = user2, Id = staging.PlatoonId };
                //}

                test.Write(battle);

                //var repo = new ClientPveBattleRepository();
                //repo.AddBattle(battle);


            }
        }

        // TODO: break ownership
        public void ImportUnitOwnership()
        {
            var timer = new Stopwatch();

            var totalSet = this.db.UserAcademy
                .AsNoTracking()
                .Where(u => u.User.LastSeenEpochSeconds >= LAST_SEEN_CUTOFF)
                .Select(u => new { u.UserId, u.Id})
                .GroupBy(u => u.UserId)
                .ToDictionary(u => u.Key, u => u.ToList());

            foreach (var unit in this.gameData.Units.Reverse())
            {
                timer.Start();
                Console.WriteLine("Beginning query for owners of {0}", unit.Name);
                this.db.Database.SetCommandTimeout(180);
                var userUnits = this.db.UserAcademy
                    .AsNoTracking()
                    .Where(u => u.Id == unit.Id && u.User.LastSeenEpochSeconds >= LAST_SEEN_CUTOFF)
                    .Select(u => new UserUnit
                    {
                        Id = u.Id,
                        Created = u.CreatedEpochSeconds,
                        Status = u.Status,
                        UnitId = u.Id,
                        UserId = u.UserId
                    })
                    .ToList();

                timer.Stop();
                Console.WriteLine("Query returned {0} results in {1}", userUnits.Count, timer.Elapsed);
                timer.Restart();

                IDriver driver = GraphDatabase.Driver("bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "password"));
                using (var session = driver.Session())
                {
                    Console.WriteLine("Creating indices");
                    session.Run("CREATE INDEX ON :UserUnit(id)");
                    session.Run("CREATE INDEX ON :Unit(id)");
                    session.Run("CREATE INDEX ON :User(id)");

                    var dictionary = new Dictionary<string, object>() { { "userunits", ParameterSerializer.ToDictionary(userUnits) } };
                    Console.WriteLine("Inserting ownership for {0}", unit.Name);
                    string cypher = new StringBuilder()
                    .AppendLine("UNWIND {userunits} as row")
                    // Ensure the unit node exists
                    .AppendLine("MERGE (u:Unit { id: row.unitId })")
                    // Ensure the user node exists
                    .AppendLine("Merge (p:User { id: row.userId })")
                    // Add the UserUnit node and set its properties
                    .AppendLine("MERGE (x:UserUnit { id: row.id })")
                    .AppendLine("SET x.created = row.created")
                    .AppendLine("SET x.status = row.status")
                    // Create an INSTANCE_OF relationship
                    .AppendLine("MERGE (x)-[r1:INSTANCE_OF]->(u)")
                    .AppendLine("MERGE (x)<-[r2:OWNS]-(p)")
                    .ToString();

                    var result = session.WriteTransaction(tx => tx.Run(cypher, dictionary));
                    var summary = result.Consume();
                    Console.WriteLine($"[{DateTime.Now}] [User Units] [{unit.Name}] #NodesCreated: {summary.Counters.NodesCreated}, #RelationshipsCreated: {summary.Counters.RelationshipsCreated}");
                }

                timer.Stop();
                Console.WriteLine("Insert of {0} owners of {1} finished after {2}", userUnits.Count, unit.Name, timer.Elapsed);
                timer.Reset();
            }
        }
    }
}
