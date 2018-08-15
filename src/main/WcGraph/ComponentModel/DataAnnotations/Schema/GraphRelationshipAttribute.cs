using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.ComponentModel.DataAnnotations.Schema
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class GraphRelationshipAttribute : Attribute
    {
        public GraphRelationshipAttribute(string name)
        {
            this.Name = name;
            this.Direction = RelationshipDirection.Outgoing;
        }

        public string Name { get; private set; }
        public RelationshipDirection Direction { get; set; }
    }
}
