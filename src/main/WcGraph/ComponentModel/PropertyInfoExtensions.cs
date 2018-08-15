using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcGraph.ComponentModel.DataAnnotations.Schema;
using WcGraph.Data;

namespace WcGraph.ComponentModel
{
    public static class PropertyInfoExtensions
    {
        public static string GetLabelName(this Object obj)
        {
            var nodeAttr = obj.GetType().GetCustomAttributes(typeof(GraphLabelAttribute), true).FirstOrDefault() as GraphLabelAttribute;
            return (string.IsNullOrWhiteSpace(nodeAttr.Name)) ? nodeAttr.Name : nameof(obj).ToSnakeCase();
        }
    }
}
