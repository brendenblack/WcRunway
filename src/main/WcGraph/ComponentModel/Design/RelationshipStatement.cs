using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.ComponentModel.Design
{
    public class RelationshipStatement
    {
        public string FromNode { get; set; }

        public string RelationshipName { get; set; }

        public string ToNode { get; set; }

        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
    }
}
