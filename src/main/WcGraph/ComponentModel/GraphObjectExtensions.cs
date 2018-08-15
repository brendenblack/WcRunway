using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WcGraph.ComponentModel.DataAnnotations.Schema;

namespace WcGraph.ComponentModel
{
    public static class GraphObjectExtensions
    {
        public static string GetNodeName(this Object obj)
        {
            var nodeAttr = obj.GetType().GetCustomAttributes(typeof(GraphNodeAttribute), true).FirstOrDefault() as GraphNodeAttribute;
            return (string.IsNullOrWhiteSpace(nodeAttr.Name)) ? nodeAttr.Name : nameof(obj);
        }

        public static IEnumerable<PropertyInfo> GetIndexProperties(this Object obj)
        {
            return obj.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(GraphIndexAttribute)));
        }
    }
}
