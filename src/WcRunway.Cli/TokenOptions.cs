﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcRunway.Cli
{
    [Verb("token", HelpText = "Calculate token runway")]
    public class TokenOptions
    {
        [Option('u', "unit", Required = false, HelpText = "Calculates the token runway for the given unit ID")]
        public int UnitId { get; set; }
    }
}
