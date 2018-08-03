using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WcData.Units;
using WcRunway.Core.Domain;

namespace WcData.Implementation.Sheets
{
    public class SheetsUnitData : IUnitData
        {
            #region Column mapping
            private readonly int COL_IDENTIFIER = 0;
            private readonly int COL_BRANCHES = 1;
            private readonly int COL_UNLOCK_SKU = 2;
            private readonly int COL_AVAILABLE = 3;
            private readonly int COL_LEVEL_BLOCKED = 4;
            private readonly int COL_ID = 5;
            private readonly int COL_LEVEL = 6;
            private readonly int COL_GRADE = 7;
            private readonly int COL_STORAGE = 8;
            private readonly int COL_VIEW_RANGE = 9;
            private readonly int COL_ENGAGEMENT_RANGE = 10;
            private readonly int COL_AOE_ID = 11;
            private readonly int COL_STATUS_TYPE = 12;
            private readonly int COL_STATUS_LEVEL = 13;
            private readonly int COL_PROJECTILE_TYPE = 14;
            private readonly int COL_ATTACK_AIR = 15;
            private readonly int COL_ATTACK_LAND = 16;
            private readonly int COL_ATTACK_MISSILES = 17;
            private readonly int COL_SPLASH = 18;
            private readonly int COL_ATTACK_RANGE = 19;
            private readonly int COL_MAX_RANGE = 20;
            private readonly int COL_ARMOR_TYPE = 21;
            private readonly int COL_RESIST_TYPE = 22;
            private readonly int COL_RESIST_MODIFIER = 23;
            private readonly int COL_CLIP_SIZE = 24;
            private readonly int COL_FIRE_RATE = 25;
            private readonly int COL_SHOT_COUNT = 26;
            private readonly int COL_SHOT_DEST_SPREAD = 27; // what is this?
            private readonly int COL_RELOAD_TIME = 28;
            private readonly int COL_CHARGE_TIME = 29;
            private readonly int COL_DISCHARGE_DELAY = 30;
            private readonly int COL_DAMAGE = 31;
            private readonly int COL_DPS = 32;
            private readonly int COL_MAX_HEALTH = 38;
            private readonly int COL_MAX_SPEED = 39;
            private readonly int COL_ACCELERATION = 40;
            private readonly int COL_BRAKES = 41;
            private readonly int COL_AIR = 42;
            private readonly int COL_TURNING_SLOWDOWN = 43;
            private readonly int COL_PRODUCTION_COST_METAL = 46;
            private readonly int COL_PRODUCTION_COST_OIL = 47;
            private readonly int COL_PRODUCTION_COST_THORIUM = 48;
            private readonly int COL_PRODUCTION_TIME = 49;
            private readonly int COL_MAP_SPEED = 50;
            // skip
            private readonly int COL_ACADEMY_LEVEL = 56;
            private readonly int COL_UPGRADE_COST_GOLD = 62;
            private readonly int COL_UPGRADE_COST_METAL = 63;
            private readonly int COL_UPGRADE_COST_OIL = 64;
            private readonly int COL_UPGRADE_COST_THORIUM = 65;
            // skip
            private readonly int COL_UPGRADE_SKU = 67;
            private readonly int COL_UPGRADE_SKU_COST = 68;
            private readonly int COL_GRADE_TEXT_LEVEL_VALUE = 64;
            private readonly int COL_GRADE_TEXT_LOCALE_KEY = 65;
            private readonly int COL_UPGRADE_TIME = 66;
            private readonly int COL_NAME = 73;
            private readonly int COL_KEY = 74;
            private readonly int COL_PLAYER_OWNABLE = 75;
            private readonly int COL_DESCRIPTION = 76;
            private readonly int COL_DESCRIPTION_KEY = 77;
            private readonly int COL_THORIUM_DESCRIPTION = 78;
            private readonly int COL_THORIUM_DESCRIPTION_KEY = 79;
            private readonly int COL_GROUP = 80;
            private readonly int COL_STRAFE = 81;
            private readonly int COL_SUICIDE = 82;
            private readonly int COL_REQUIRES = 84;
            private readonly int COL_MAX_BUILDABLE_WITHOUT_SKU = 92;
            private readonly int COL_MAX_BUILDABLE_SKU = 93;
            private readonly int COL_ATTACK_PRIORITY = 94;
            private readonly int COL_STATUS_IMMUNITIES = 95;
            private readonly int COL_MIN_ATTACK_RANGE = 96;
            private readonly int COL_GRANTABLE = 98;

            private readonly int COL_GENERATION = 1;
            #endregion

            private readonly string sheetId = "1oB2tzdftGTXeOnWr_aIlZ0A48FM15bfyz406Ldn_7hk";
            private readonly SheetsService sheets;
            private readonly ILogger<SheetsUnitData> log;

            public SheetsUnitData(ILogger<SheetsUnitData> logger, SheetsConnectorService sheets)
            {
                this.Validity = TimeSpan.FromMinutes(30);
                this.sheets = sheets.Service;
                this.log = logger;
            }

            private List<Unit> _units = new List<Unit>();

            public DateTime LastUpdate { get; private set; } = DateTime.MinValue;
            public TimeSpan Validity { get; private set; }
            public bool IsStale
            {
                get
                {
                    return DateTime.Now > (LastUpdate + Validity);
                }
            }

            public IEnumerable<Unit> Units
            {
                get
                {
                    if (IsStale)
                    {
                        log.LogDebug("Units data is stale, refetching...");
                        Task.WaitAll(RefreshUnits());
                    }

                    return _units;
                }
            }

            public async Task RefreshUnits()
            {
                var range = "Unit Data!A2:EJ";
                SpreadsheetsResource.ValuesResource.GetRequest request = sheets.Spreadsheets.Values.Get(sheetId, range);
                ValueRange response = await request.ExecuteAsync();
                log.LogDebug("Received response of range {0} (major dimension: {1})", response.Range, response.MajorDimension);
                var values = response.Values;


                if (values != null && values.Count > 0)
                {
                    var units = new Dictionary<int, Unit>();

                    foreach (var row in values)
                    {
                        var id = Int32.Parse(row[COL_ID].ToString());
                        var exists = units.TryGetValue(id, out Unit unit);
                        if (!exists)
                        {
                            unit = new Unit(id);
                            units.Add(id, unit);
                        }

                        var name = row[COL_NAME].AsString();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            unit.Name = name;
                        }

                        unit.Key = row[COL_KEY].AsString();
                        unit.Identifier = row[COL_IDENTIFIER].AsString();
                        var description = row[COL_DESCRIPTION].AsString();
                        if (!string.IsNullOrWhiteSpace(description))
                        {
                            unit.Description = description;
                        }

                        var unlockSku = row[COL_UNLOCK_SKU].AsString();

                        Level level = new Level();
                        level.Number = row[COL_LEVEL].AsInteger();
                        level.Grade = row[COL_GRADE].AsInteger();
                        level.UpgradeCostMetal = row[COL_UPGRADE_COST_METAL].AsInteger();
                        level.UpgradeCostOil = row[COL_UPGRADE_COST_OIL].AsInteger();
                        level.UpgradeCostThorium = row[COL_UPGRADE_COST_THORIUM].AsInteger();
                        level.UpgradeCostGold = row[COL_UPGRADE_COST_GOLD].AsInteger();

                        var upgradeSku = row[COL_UPGRADE_SKU].AsString();
                        var upgradeSkuCost = row[COL_UPGRADE_SKU_COST].AsString();
                        if (!string.IsNullOrWhiteSpace(upgradeSku))
                        {
                            level.UpgradeSku = upgradeSku;
                        }

                        if (!string.IsNullOrWhiteSpace(upgradeSkuCost))
                        {
                            try
                            {
                                var skuCost = JObject.Parse(upgradeSkuCost).First.ToObject<JProperty>();
                                Int32.TryParse(skuCost.Value.ToString(), out int qty);
                                level.AddUpgradeSkuCost(skuCost.Name, qty);
                            }
                            catch (JsonSerializationException e)
                            {
                                log.LogError("There was an error parsing upgradeSkuCost column as JSON, value: {0}", upgradeSkuCost);
                            }
                        }

                        unit.Levels.Add(level);




                    }

                    this._units = units.Select(u => u.Value).ToList();
                }
                else
                {
                    log.LogWarning("No data found");
                }

                this.LastUpdate = DateTime.Now;


            }
        }
}
