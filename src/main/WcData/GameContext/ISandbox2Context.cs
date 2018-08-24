using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WcData.GameContext.Models;

namespace WcData.GameContext
{
    public interface ISandbox2Context
    {
        DbSet<Offer> Offers
        {
            get;
        }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
