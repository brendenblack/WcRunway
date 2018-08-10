using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.GameContext.Models;

namespace WcData.GameContext.Implementation
{
    public class GameDbContext : DbContext, ISandbox2Context, ILiveSlaveContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        { 
        }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<UserUnit> UserAcademy { get; set; }
    }
}
