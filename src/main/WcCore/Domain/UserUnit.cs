using System;
using System.Collections.Generic;
using System.Text;

namespace WcCore.Domain
{
    /// <summary>
    /// Models unit ownership
    /// </summary>
    public class UserUnit
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public int Level { get; set; }

        public int Skin { get; set; }

        public int Status { get; set; }

        public long CreatedEpochSeconds { get; set; } = 0;

        public DateTimeOffset Created
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(CreatedEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
        }
    }
}
