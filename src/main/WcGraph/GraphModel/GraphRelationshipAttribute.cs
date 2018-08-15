using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.GraphModel
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class GraphRelationshipAttribute : Attribute
    {
        public GraphRelationshipAttribute(string name)
        {
            this.Name = name;
            this.Direction = Relationships.OUTGOING;
        }

        public string Name { get; private set; }
        public Relationships Direction { get; set; }
    }
}
