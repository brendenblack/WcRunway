//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text;

//namespace WcRunway.Core.Infrastructure.Data.Providers.MySql
//{
//    [Table("offers")]
//    public class Offer
//    {
//        [Key]
//        [Column("id")]
//        public int Id { get; set; }

//        [Column("offer_code")]
//        public string OfferCode { get; set; }

//        [Column("start_time")]
//        public long StartTimeEpochSeconds { get; set; }

//        [NotMapped]
//        public DateTime StartTime
//        {
//            get
//            {
//                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
//                var timespan = TimeSpan.FromSeconds(StartTimeEpochSeconds);
//                return epoch.Add(timespan).ToLocalTime();
//            }
//            set
//            {
//                this.StartTimeEpochSeconds = 0; // value.ToUniversalTime();
//            }
//        }

//        [Column("end_time")]
//        public long EndTimeEpochSeconds { get; set; }

//        [NotMapped]
//        public DateTime EndTime
//        {
//            get
//            {
//                return DateTime.Now;
//            }
//        }

//        public int? Duration { get; set; }

//        public int? Priority { get; set; }

//        [Column("max_qty")]
//        public int? MaxQuantity { get; set; }

//        public int? Cooldown { get; set; }

//        [Column("cooldown_type")]
//        public int? CooldownType { get; set; }

//        [Column("full_cost")]
//        public int? FullCost { get; set; }

//        public string Title { get; set; }

//        [Column("desc")]
//        public string Description { get; set; }

//        [Column("content")]
//        public string ContentJson { get; set; }

//        [Column("displayed_items")]
//        public string DisplayedItemsJson { get; set; }

//        [Column("icon_title")]
//        public string IconTitle { get; set; }

//        [Column("icon_desc")]
//        public string IconDescription { get; set; }




//    }
//}
