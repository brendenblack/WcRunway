using System;
using System.Collections.Generic;
using System.Text;

namespace WcData.GameContext.Models
{
    public class UserTopup
    {
        public int Id { get; set; }
        public long Code { get; set; }
        public string Type { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public long AddTimeEpochSeconds { get; set; }

        public DateTimeOffset AddTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(AddTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
        }

        public long UpdateTimeEpochSeconds { get; set; }

        public DateTimeOffset UpdateTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(UpdateTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
        }

        public string Stage { get; set; }

        public double Amount { get; set; }
    }
}
