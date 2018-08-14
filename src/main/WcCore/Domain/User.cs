using System;
using System.Collections.Generic;
using System.Text;

namespace WcCore.Domain
{
    public class User
    {
        public int Id { get; set; }

        public long? FacebookId { get; set; }
        public string KixeyeId { get; set; }

        public string EmailAddress { get; set; }
        public List<UserUnit> UnlockedUnits { get; set; } = new List<UserUnit>();

        public long AddTimeEpochSeconds { get; set; }

        public DateTimeOffset AddTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(AddTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
        }

        public long LastSeenEpochSeconds { get; set; }
        public DateTimeOffset LastSeen
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(LastSeenEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
        }

        public string Country { get; set; }

        public string Gender { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
