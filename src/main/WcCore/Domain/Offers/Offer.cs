using System;
using System.Collections.Generic;
using System.Text;

namespace WcCore.Domain.Offers
{
    /// <summary>
    /// Models an offer as represented inside the War Commander offer tool
    /// </summary>
    public class Offer
    {
        public Offer()
        {
            this.CreatedTime = DateTimeOffset.Now;
            this.ModifiedTime = DateTimeOffset.Now;
        }

        public int Id { get; set; }

        public string OfferCode { get; set; }

        public long StartTimeEpochSeconds { get; set; }

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

        public long EndTimeEpochSeconds { get; set; }

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

        public long ModifiedTimeEpochSeconds { get; set; }

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

        public long CreatedTimeEpochSeconds { get; set; }

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

        public int? Duration { get; set; } = 86400;

        public int? Priority { get; set; } = 0;

        public int MaxQuantity { get; set; } = 1;

        public int Cooldown { get; set; } = 0;

        public int CooldownType { get; set; } = 1;

        public int Cost { get; set; } = 0;

        public int FullCost { get; set; } = -1;

        public string CostSku { get; set; } = "gold";

        public string Title { get; set; }

        public string Description { get; set; }

        public string ContentJson { get; set; }

        public string DisplayedItemsJson { get; set; }

        public string IconTitle { get; set; }

        public string IconDescription { get; set; }

        public string DisplayOptionsJson { get; set; } = "{ \"show_popup\": 2}";

        public int TemplateId { get; set; }

        public int Deleted { get; set; } = 0;

        public bool IsDeleted
        {
            get
            {
                return Deleted == 1;
            }
            set
            {
                Deleted = (value) ? 1 : 0;
            }
        }

        public int Enabled { get; set; } = 1;

        public bool IsEnabled
        {
            get
            {
                return Enabled == 1;
            }
            set
            {
                Enabled = (value) ? 1 : 0;
            }
        }

        public string Prerequisite { get; set; }

        #region Builder
        public static IOfferBuilderSetStartTime WithCode(string offerCode)
        {
            return new Offer.Builder(offerCode);
        }


        public interface IOfferBuilderSetStartTime
        {
            IOfferBuilderSetEndTime StartingAt(DateTimeOffset dto);
            IOfferBuilderSetEndTime StartingAt(long epochSeconds);
        }

        public interface IOfferBuilderSetEndTime
        {

        }

        public class Builder : IOfferBuilderSetStartTime, IOfferBuilderSetEndTime
        {
            private readonly string offerCode;
            private long startTime;
            private long endTime;

            internal Builder(string offerCode)
            {
                this.offerCode = offerCode;
            }

            public IOfferBuilderSetEndTime StartingAt(DateTimeOffset dto)
            {
                throw new NotImplementedException();
            }

            public IOfferBuilderSetEndTime StartingAt(long epochSeconds)
            {
                this.startTime = epochSeconds;
                return this;
            }
        }
        #endregion

    }

}
