using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Core.Domain.Offers
{
    public abstract class OfferGenerator
    {
        
        // TODO: validate characters as acceptable OS filenames
        /// <summary>
        /// Ensures the prefix is valid. To be considered viable, the provided prefix must not be null or whitespace, and not already in use.
        /// If the specified prefix is longer than 16 characters, it will be truncated to allow for offer type suffixes to be added.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        //public string ValidatePrefix(string prefix)
        //{
        //    if (String.IsNullOrWhiteSpace(prefix))
        //    {
        //        throw new ArgumentException("Invalid offer code prefix provided");
        //    }

        //    prefix = (prefix.Length > 16) ? prefix.Substring(0, 16) : prefix;

        //    if (this.sb2.Offers.Any(o => o.OfferCode.StartsWith(prefix)))
        //    {
        //        throw new InvalidOperationException($"Unable to create offers because the prefix {prefix} already exist");
        //    }

        //    return prefix;
        //}

    }
}
