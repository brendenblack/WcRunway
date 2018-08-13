using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WcData.GameContext.Models
{
    public class UserPlatoon
    {
        [Column("")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User Owner { get; set; }

        [Key]
        public string Id { get; set; }
    }
}
