using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcGraph.Data;

namespace WcGraph.ComponentModel.Design
{
    public class MergeStatement
    {
        public MergeStatement(string variableName, string nodeType, Dictionary<string,string> indexProperties = null, Dictionary<string, string> nonIndexProperties = null)
        {
            VariableName = variableName;

            NodeType = nodeType;

            if (indexProperties == null)
            {
                indexProperties = new Dictionary<string, string>();
            }

            IndexProperties = indexProperties;
            NonIndexProperties = nonIndexProperties;
        }

        public string VariableName { get; }

        public string NodeType { get; set; }

        public Dictionary<string, string> IndexProperties { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> NonIndexProperties { get; set; } = new Dictionary<string, string>();

        public override string ToString()
        {
            var indexProperties = String.Join(',', IndexProperties.Select(x => x.Key + ": " + x.Value).ToArray());

            return $"({VariableName}:{NodeType} {{ {indexProperties} }})";
        }

        public static MergeStatement For(Object obj, string variableName = "a")
        {
            var nodeName = obj.GetNodeName();

            var indexProperties = obj.GetIndexProperties();

            var merge = new MergeStatement(variableName, nodeName);
            foreach (var p in indexProperties)
            {
                var indexKey = p.GetLabelName();
                var indexValue = p.GetValue(obj) as string;
                merge.IndexProperties.Add(indexKey, indexValue);
            }

            return merge;
        }
    }
}
