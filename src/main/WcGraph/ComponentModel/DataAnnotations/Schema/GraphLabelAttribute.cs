using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.ComponentModel.DataAnnotations.Schema
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class GraphLabelAttribute : Attribute
    {
        public GraphLabelAttribute() : this("") { }

        public GraphLabelAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
