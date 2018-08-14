using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WcData.GameContext.Models
{
    [Table("units_v2")]
    public class UserUnit
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

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
