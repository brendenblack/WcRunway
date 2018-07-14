﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcRunway.Cli.Verbs;
using WcRunway.Core.Infrastructure.Data.Game;

namespace WcRunway.Cli
{
    public class TokenRunway
    {
        private readonly IGameContext game;
        private readonly ILogger<TokenRunway> log;

        public TokenRunway(ILogger<TokenRunway> logger, IGameContext gameBible)
        {
            this.game = gameBible;
            this.log = logger;
        }

        public int Execute(TokenOptions options)
        {
            log.LogInformation("Beginning token runway calculator...");

            if (options.UnitId.HasValue && options.UnitId.Value > 0)
            {
                var id = options.UnitId.Value;

                var unit = game.Units.FirstOrDefault(u => u.Id == id);
                if (unit == null)
                {
                    log.LogError("No unit found with id {0}", id);
                    return -1;
                }

                log.LogInformation("Calculating runway for {0} ({1})", unit.Name, unit.Id);
            }
            else
            {

            }
            return 0;
        }




    }
}
