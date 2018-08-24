using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WcData.GameContext.Models
{
    [Table("offers")]
    public class Offer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("offer_code")]
        public string OfferCode { get; set; }

        [Column("start_time")]
        public long StartTimeEpochSeconds { get; set; }

        [NotMapped]
        public DateTimeOffset StartTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(StartTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
            set
            {
                this.StartTimeEpochSeconds = value.ToUnixTimeSeconds();
                this.ModifiedTimeEpochSeconds = StartTimeEpochSeconds;
            }
        }

        [Column("end_time")]
        public long EndTimeEpochSeconds { get; set; }

        [NotMapped]
        public DateTimeOffset EndTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(EndTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
            set
            {
                this.EndTimeEpochSeconds = value.ToUnixTimeSeconds();
            }
        }

        [Column("mod_time")]
        public long ModifiedTimeEpochSeconds { get; set; }

        [NotMapped]
        public DateTimeOffset ModifiedTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(ModifiedTimeEpochSeconds).ToOffset(TimeSpan.FromHours(-7));
            }
            set
            {
                this.ModifiedTimeEpochSeconds = value.ToUnixTimeSeconds();
            }
        }

        [Column("created_time")]
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

        [Column("duration")]
        public int? Duration { get; set; } = 86400;

        [Column("priority")]
        public int? Priority { get; set; } = 0;

        [Column("max_qty")]
        public int MaxQuantity { get; set; } = 1;

        [Column("cooldown")]
        public int Cooldown { get; set; } = 0;

        [Column("cooldown_type")]
        public int CooldownType { get; set; } = 1;

        [Column("cost")]
        public int Cost { get; set; } = 0;

        [Column("full_cost")]
        public int FullCost { get; set; } = -1;

        [Column("cost_sku")]
        public string CostSku { get; set; } = "gold";

        [Column("title")]
        public string Title { get; set; }

        [Column("desc")]
        public string Description { get; set; }

        [Column("content")]
        public string ContentJson { get; set; }

        [Column("displayed_items")]
        public string DisplayedItemsJson { get; set; }

        [Column("icon_title")]
        public string IconTitle { get; set; }

        [Column("icon_desc")]
        public string IconDescription { get; set; }

        [Column("display_options")]
        public string DisplayOptionsJson { get; set; } = "{ \"show_popup\": 2}";

        [Column("template_id")]
        public int TemplateId { get; set; }

        [Column("is_deleted")]
        private int deleted { get; set; } = 0;

        [NotMapped]
        public bool IsDeleted
        {
            get
            {
                return deleted == 1;
            }
            set
            {
                deleted = (value) ? 1 : 0;
            }
        }

        [Column("is_enabled")]
        private int enabled { get; set; } = 1;

        [NotMapped]
        public bool IsEnabled
        {
            get
            {
                return enabled == 1;
            }
            set
            {
                enabled = (value) ? 1 : 0;
            }
        }

        [Column("pre_req")]
        public string Prerequisite { get; set; }
    }
}
