using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WcCore.Domain.Offers;
using WcData.Implementation.MySql;

namespace WcData.GameContext
{
    public class LiveSlaveContext : DbContext
    {
        public LiveSlaveContext(DbContextOptions<LiveSlaveContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            MySqlModelBuilder.DefineModel(builder);
        }

        public DbSet<Offer> Offers { get; set; }
    }
}
