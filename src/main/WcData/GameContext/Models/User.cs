using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WcData.GameContext.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("userid")]
        public int Id { get; set; }

        [Column("fbid")]
        public long? FacebookId { get; set; }

        [Column("kxid")]
        public string KixeyeId { get; set; }

        [Column("email")]
        public string EmailAddress { get; set; }
        public List<UserUnit> UnlockedUnits { get; set; } = new List<UserUnit>();

        [Column("addtime")]
        public long AddTimeEpochSeconds { get; set; }

        [NotMapped]
        public DateTimeOffset AddTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(AddTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
        }

        [Column("seentime")]
        public long LastSeenEpochSeconds { get; set; }

        [NotMapped]
        public DateTimeOffset LastSeen
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(LastSeenEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
        }
        
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

     






    }
}
