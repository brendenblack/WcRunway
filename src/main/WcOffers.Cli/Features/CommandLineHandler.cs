using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features
{
    public abstract class CommandLineHandler
    {
        public abstract int Execute(CommandLineOptions opts);
    }
}
