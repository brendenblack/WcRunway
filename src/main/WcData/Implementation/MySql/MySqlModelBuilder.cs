using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WcCore.Domain;
using WcCore.Domain.Offers;

namespace WcData.Implementation.MySql
{
    public class MySqlModelBuilder
    {
        public static void DefineModel(ModelBuilder builder)
        {
            #region Offer
            //builder.Entity<Offer>().ToTable("offers");
            //builder.Entity<Offer>().HasKey(o => o.Id);

            //builder.Entity<Offer>().Ignore(o => o.StartTime);
            //builder.Entity<Offer>().Ignore(o => o.EndTime);
            //builder.Entity<Offer>().Ignore(o => o.ModifiedTime);
            //builder.Entity<Offer>().Ignore(o => o.CreatedTime);
            //builder.Entity<Offer>().Ignore(o => o.IsEnabled);
            //builder.Entity<Offer>().Ignore(o => o.IsDeleted);

            //builder.Entity<Offer>().Property(o => o.OfferCode).HasColumnName("offer_code").IsRequired();
            //builder.Entity<Offer>().Property(o => o.StartTimeEpochSeconds).HasColumnName("start_time");
            //builder.Entity<Offer>().Property(o => o.EndTimeEpochSeconds).HasColumnName("end_time");
            //builder.Entity<Offer>().Property(o => o.CreatedTimeEpochSeconds).HasColumnName("created_time");
            //builder.Entity<Offer>().Property(o => o.ModifiedTimeEpochSeconds).HasColumnName("mod_time");
            //builder.Entity<Offer>().Property(o => o.Duration).HasColumnName("duration");
            //builder.Entity<Offer>().Property(o => o.Cooldown).HasColumnName("cooldown");
            //builder.Entity<Offer>().Property(o => o.CooldownType).HasColumnName("cooldown_type").HasDefaultValue(1);
            //builder.Entity<Offer>().Property(o => o.FullCost).HasColumnName("full_cost").HasDefaultValue(0);
            //builder.Entity<Offer>().Property(o => o.MaxQuantity).HasColumnName("max_qty").HasDefaultValue(1);
            //builder.Entity<Offer>().Property(o => o.Description).HasColumnName("desc");
            //builder.Entity<Offer>().Property(o => o.ContentJson).HasColumnName("content");
            //builder.Entity<Offer>().Property(o => o.DisplayedItemsJson).HasColumnName("displayed_items");
            //builder.Entity<Offer>().Property(o => o.IconTitle).HasColumnName("icon_title");
            //builder.Entity<Offer>().Property(o => o.IconDescription).HasColumnName("icon_desc");
            //builder.Entity<Offer>().Property(o => o.CostSku).HasColumnName("cost_sku");
            //builder.Entity<Offer>().Property(o => o.DisplayOptionsJson).HasColumnName("display_options");
            //builder.Entity<Offer>().Property(o => o.TemplateId).HasColumnName("template_id");
            //builder.Entity<Offer>().Property(o => o.Deleted).HasColumnName("is_deleted");
            //builder.Entity<Offer>().Property(o => o.Enabled).HasColumnName("is_enabled");
            //builder.Entity<Offer>().Property(o => o.Prerequisite).HasColumnName("pre_req");
            #endregion

            #region User
            builder.Entity<User>().ToTable("users");
            builder.Entity<User>().HasKey(u => u.Id);

            builder.Entity<User>().Ignore(u => u.AddTime);
            builder.Entity<User>().Ignore(u => u.LastSeen);
            builder.Entity<User>().Ignore(u => u.UnlockedUnits);
            builder.Entity<User>().Ignore(u => u.Country);
            builder.Entity<User>().Ignore(u => u.Gender);

            builder.Entity<User>().Property(u => u.Id).HasColumnName("userid");
            builder.Entity<User>().Property(u => u.KixeyeId).HasColumnName("kxid");
            builder.Entity<User>().Property(u => u.FacebookId).HasColumnName("fbid");
            builder.Entity<User>().Property(u => u.AddTimeEpochSeconds).HasColumnName("addtime");
            builder.Entity<User>().Property(u => u.LastSeenEpochSeconds).HasColumnName("seentime");
            builder.Entity<User>().Property(u => u.FirstName).HasColumnName("first_name");
            builder.Entity<User>().Property(u => u.LastName).HasColumnName("last_name");
            builder.Entity<User>().Property(u => u.EmailAddress).HasColumnName("email");
            #endregion

            #region Unit
            builder.Entity<Unit>().Ignore(u => u.Levels);
            #endregion


            #region UserUnit
            builder.Entity<UserUnit>().ToTable("user_academy");
            builder.Entity<UserUnit>().HasKey(u => u.Id);
            builder.Entity<UserUnit>()
                .HasOne(u => u.User)
                .WithMany(u => u.UnlockedUnits)
                .HasForeignKey(u => u.UserId);
            builder.Entity<UserUnit>()
                .HasOne(u => u.Unit)
                .WithMany()
                .HasForeignKey(u => u.UnitId);

            builder.Entity<UserUnit>().Ignore(u => u.Created);

            builder.Entity<UserUnit>().Property(u => u.UserId).HasColumnName("userid");
            builder.Entity<UserUnit>().Property(u => u.Level).HasColumnName("level");
            builder.Entity<UserUnit>().Property(u => u.Skin).HasColumnName("skin");
            builder.Entity<UserUnit>().Property(u => u.Status).HasColumnName("status");
            builder.Entity<UserUnit>().Property(u => u.UnitId).HasColumnName("type");
            builder.Entity<UserUnit>().Property(u => u.CreatedEpochSeconds).HasColumnName("created");
            #endregion

            #region UserTopup
            builder.Entity<UserTopup>().ToTable("user_topups");
            builder.Entity<UserTopup>().HasKey(ut => ut.Id);
            builder.Entity<UserTopup>()
                .HasOne(ut => ut.User)
                .WithMany()
                .HasForeignKey(ut => ut.UserId);

            builder.Entity<UserTopup>().Ignore(ut => ut.UpdateTime);
            builder.Entity<UserTopup>().Ignore(ut => ut.AddTime);

            builder.Entity<UserTopup>().Property(ut => ut.AddTimeEpochSeconds).HasColumnName("addtime");
            builder.Entity<UserTopup>().Property(ut => ut.UpdateTimeEpochSeconds).HasColumnName("updatetime");
            builder.Entity<UserTopup>().Property(ut => ut.Stage).HasColumnName("stage");
            builder.Entity<UserTopup>().Property(ut => ut.Amount).HasColumnName("procamount");
            //builder.Entity<UserTopup>().Property(ut => ut.AddTimeEpochSeconds).HasColumnName("add_time");
            #endregion
        }
    }
}
