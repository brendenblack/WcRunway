using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WcGraph.ComponentModel.DataAnnotations.Schema;
using WcGraph.Data;

namespace WcGraph.ComponentModel
{
    public static class PropertyInfoExtensions
    {
        public static string GetLabelName(this PropertyInfo property)
        {
            if (property == null)
            {
                return "";
            }

            var nodeAttr = property.GetCustomAttributes(typeof(GraphLabelAttribute), true).FirstOrDefault() as GraphLabelAttribute;
            if (nodeAttr != null)
            {
                if (!string.IsNullOrWhiteSpace(nodeAttr.Name))
                {
                    return nodeAttr.Name;
                }
            }


            return property.Name.ToSnakeCase();
            // return (string.IsNullOrWhiteSpace(nodeAttr?.Name ?? "")) ? nodeAttr.Name : nameof(obj).ToSnakeCase();
        }
    }
}
