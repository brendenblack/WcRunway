﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core.Domain.Offers;

namespace WcRunway.Core.Infrastructure.Data.Providers.MySql
{
    public class Sandbox2Context : DbContext
    {
        public Sandbox2Context(DbContextOptions<Sandbox2Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Offer>().ToTable("offers");

            builder.Entity<Offer>().HasKey(o => o.Id);

            builder.Entity<Offer>().Ignore(o => o.StartTime);
            builder.Entity<Offer>().Ignore(o => o.EndTime);

            builder.Entity<Offer>().Property(o => o.OfferCode).HasColumnName("offer_code").IsRequired();
            builder.Entity<Offer>().Property(o => o.StartTimeEpochSeconds).HasColumnName("start_time");
            builder.Entity<Offer>().Property(o => o.EndTimeEpochSeconds).HasColumnName("end_time");
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

        }


        public DbSet<Offer> Offers { get; set; }
    }
}
