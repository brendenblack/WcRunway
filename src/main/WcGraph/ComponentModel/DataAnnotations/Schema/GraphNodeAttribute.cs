using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.ComponentModel.DataAnnotations.Schema
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GraphNodeAttribute : Attribute
    {
        public GraphNodeAttribute() : this("") { }

        /// <summary>
        /// Marks this object as a node in the graph, with the specified name
        /// </summary>
        /// <param name="name">The type of node</param>
        public GraphNodeAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
