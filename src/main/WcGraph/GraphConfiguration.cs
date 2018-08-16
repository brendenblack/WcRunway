using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.ComponentModel;

namespace WcGraph
{
    public class GraphConfiguration
    {
        public string Address { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public LabelBehaviour LabelBehaviour { get; set; } = LabelBehaviour.OPT_IN;
    }
}
