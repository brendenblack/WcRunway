using System;
using System.Collections.Generic;
using System.Text;

namespace WcOffers.Cli.Features
{
    /// <summary>
    /// The preferred way to indicate that a class is a command line handler, but as of yet does not work for
    /// auto-registration with the DI container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandLineHandler<T> where T : CommandLineOptions
    {
        int Execute(T opts);
    }

    /// <summary>
    /// Indicates that a class is a command line handler, and required for auto-registration with the DI container at startup.
    /// </summary>
    public abstract class CommandLineHandler
    {

    }
}
