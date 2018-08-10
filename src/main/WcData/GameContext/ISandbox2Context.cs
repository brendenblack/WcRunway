using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WcData.GameContext.Models;

namespace WcData.GameContext
{
    public interface ISandbox2Context
    {
        DbSet<Offer> Offers
        {
            get;
        }
    }
}
