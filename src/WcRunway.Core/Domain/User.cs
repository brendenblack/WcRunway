using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string EmailAddress { get; set; }
        public List<UserUnit> UserAcademy { get; set; } = new List<UserUnit>();

        public DateTime AddTime { get; set; }

        public DateTime LastSeenTime { get; set; }

        public TimeSpan TimePlayed { get; set; }

        public String Country { get; set; }

        public String Gender { get; set; }
    }
}
