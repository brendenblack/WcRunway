using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WcCore.Domain.Offers;

namespace WcData.Implementation.MySql
{
    public class MySqlModelBuilder
    {
        public static void DefineModel(ModelBuilder builder)
        {
            builder.Entity<Offer>().ToTable("offers");

            builder.Entity<Offer>().HasKey(o => o.Id);

            builder.Entity<Offer>().Ignore(o => o.StartTime);
            builder.Entity<Offer>().Ignore(o => o.EndTime);
            builder.Entity<Offer>().Ignore(o => o.ModifiedTime);
            builder.Entity<Offer>().Ignore(o => o.CreatedTime);
            builder.Entity<Offer>().Ignore(o => o.IsEnabled);
            builder.Entity<Offer>().Ignore(o => o.IsDeleted);

            builder.Entity<Offer>().Property(o => o.OfferCode).HasColumnName("offer_code").IsRequired();
            builder.Entity<Offer>().Property(o => o.StartTimeEpochSeconds).HasColumnName("start_time");
            builder.Entity<Offer>().Property(o => o.EndTimeEpochSeconds).HasColumnName("end_time");
            builder.Entity<Offer>().Property(o => o.CreatedTimeEpochSeconds).HasColumnName("created_time");
            builder.Entity<Offer>().Property(o => o.ModifiedTimeEpochSeconds).HasColumnName("mod_time");
            builder.Entity<Offer>().Property(o => o.Duration).HasColumnName("duration");
            builder.Entity<Offer>().Property(o => o.Cooldown).HasColumnName("cooldown");
            builder.Entity<Offer>().Property(o => o.CooldownType).HasColumnName("cooldown_type").HasDefaultValue(1);
            builder.Entity<Offer>().Property(o => o.FullCost).HasColumnName("full_cost").HasDefaultValue(0);
            builder.Entity<Offer>().Property(o => o.MaxQuantity).HasColumnName("max_qty").HasDefaultValue(1);
            builder.Entity<Offer>().Property(o => o.Description).HasColumnName("desc");
            builder.Entity<Offer>().Property(o => o.ContentJson).HasColumnName("content");
            builder.Entity<Offer>().Property(o => o.DisplayedItemsJson).HasColumnName("displayed_items");
            builder.Entity<Offer>().Property(o => o.IconTitle).HasColumnName("icon_title");
            builder.Entity<Offer>().Property(o => o.IconDescription).HasColumnName("icon_desc");
            builder.Entity<Offer>().Property(o => o.CostSku).HasColumnName("cost_sku");
            builder.Entity<Offer>().Property(o => o.DisplayOptionsJson).HasColumnName("display_options");
            builder.Entity<Offer>().Property(o => o.TemplateId).HasColumnName("template_id");
            builder.Entity<Offer>().Property(o => o.Deleted).HasColumnName("is_deleted");
            builder.Entity<Offer>().Property(o => o.Enabled).HasColumnName("is_enabled");
            builder.Entity<Offer>().Property(o => o.Prerequisite).HasColumnName("pre_req");
        }
    }
}
