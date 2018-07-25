using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Infrastructure.Data.Providers.MySql
{
    public class Sandbox2Context : DbContext
    {
        public Sandbox2Context(DbContextOptions<Sandbox2Context> options) : base(options) { }

        public DbSet<Offer> Offers { get; set; }
    }
}
