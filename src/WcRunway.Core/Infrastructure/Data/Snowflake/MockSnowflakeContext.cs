using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WcRunway.Core.Infrastructure.Data.Snowflake
{
    public class MockSnowflakeContext
    {
        public Dictionary<int, int> GetUnitOwnership(int unitId)
        {
            var results = new Dictionary<int, int>();
            using (var reader = File.OpenText($"Resources/owners_{unitId}.csv"))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.PrepareHeaderForMatch = (header) => header.Trim().ToLower();
                var records = csv.GetRecords(new { UserId = default(int), Level = default(int) });

                foreach (var record in records)
                {
                    results.Add(record.UserId, record.Level);
                }
            }

            return results;
        }

        public Dictionary<int, double> GetMonetizedUsers()
        {
            var results = new Dictionary<int, double>();
            using (var reader = File.OpenText($"Resources/monetized.csv"))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.PrepareHeaderForMatch = (header) => header.Trim().ToLower();
                var records = csv.GetRecords(new { UserId = default(int), Ltv = default(double) });

                foreach (var record in records)
                {
                    results.Add(record.UserId, record.Ltv);
                }
            }

            return results;
        }
    }
}
