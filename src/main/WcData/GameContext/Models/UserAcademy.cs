using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WcData.GameContext.Models
{
    [Table("user_academy")]
    public class UserAcademyEntry
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [NotMapped]
        public User User { get; set; }

        [Column("type")]
        public int UnitId { get; set; }

        [Column("level")]
        public int Level { get; set; }

        [Column("skin")]
        public int Skin { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("updated")]
        public long UpdatedTimeEpochSeconds { get; set; }

        [NotMapped]
        public DateTimeOffset UpdatedTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(UpdatedTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
            set
            {
                this.UpdatedTimeEpochSeconds = value.ToUnixTimeSeconds();
            }
        }

        [Column("created")]
        public long CreatedTimeEpochSeconds { get; set; }

        [NotMapped]
        public DateTimeOffset CreatedTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(CreatedTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
            set
            {
                this.CreatedTimeEpochSeconds = value.ToUnixTimeSeconds();
            }
        }
    }
}
