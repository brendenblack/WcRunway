using Microsoft.EntityFrameworkCore;
using WcData.GameContext.Models;

namespace WcData.GameContext
{
    public interface ILiveSlaveContext
    {
        DbSet<Offer> Offers { get; }

        DbSet<User> Users { get; }

        DbSet<Unit> Units { get; }

        DbSet<UserUnit> UserAcademy { get; }
    }
}
