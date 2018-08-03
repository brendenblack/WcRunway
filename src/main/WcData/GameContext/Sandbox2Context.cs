using Microsoft.EntityFrameworkCore;
using WcData.Implementation.MySql;
using WcCore.Domain.Offers;

namespace WcData.GameContext
{
    public class Sandbox2Context : DbContext
    {
        public Sandbox2Context(DbContextOptions<Sandbox2Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            MySqlModelBuilder.DefineModel(builder);
        }

        public DbSet<Offer> Offers { get; set; }
    }
}
