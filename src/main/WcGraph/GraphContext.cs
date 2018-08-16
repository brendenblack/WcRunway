using Neo4jClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using WcGraph.ComponentModel;
using WcGraph.ComponentModel.DataAnnotations.Schema;
using WcGraph.ComponentModel.Design;

namespace WcGraph.Data
{
    public class GraphContext
    {
        public GraphContext()
        {
            var g = new BoltGraphClient("bolt://127.0.0.1:7687/graph/db", "neo4j", "password");
            g.Connect();
            graph = g;
            labelBehaviour = LabelBehaviour.OPT_IN;
            mergeBehaviour = MergeBehaviour.ON_CREATE;
        }

        public GraphContext(GraphConfiguration config)
        {
            var g = new BoltGraphClient(config.Address, config.Username, config.Password);
            g.Connect();
            graph = g;
            labelBehaviour = config.LabelBehaviour;
        }

        IGraphClient graph;
        private readonly LabelBehaviour labelBehaviour;
        private readonly MergeBehaviour mergeBehaviour;

        /// <summary>
        /// Adds a node to the graph, including its entire relationship tree. All non-null navigation properties will be MERGE'd in to the graph and only if
        /// they are newly created (i.e. ON CREATE) will properties be added.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Write<T>(T obj, bool checkIndices = false)
        {
            var isGraphNode = typeof(T).GetCustomAttributes(typeof(GraphNodeAttribute), true).Any();
            if (!isGraphNode)
            {
                throw new InvalidOperationException($"Unable to persist an object of type {typeof(T)}. Entities must be annotated with the GraphNode attribute");
            }

            var variableNameIndex = 1;

            var indexProperties = typeof(T)
                .GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(GraphIndexAttribute)));

            if (indexProperties.Count() <= 0)
            {
                throw new NotImplementedException("Unable to persist an object that does not have any indices defined. Add the GraphIndex attribute to one or more appropriate, unique properties of " + typeof(T));
            }

            // Ensure that the index exists for each property declared as an index for this 
            if (checkIndices)
            {
                foreach (var index in indexProperties)
                {
                    var indexName = index.GetLabelName();
                    if (!graph.CheckIndexExists(indexName, IndexFor.Node))
                    {
                        var cql = graph.Cypher
                            .Create($"INDEX ON :{obj.GetNodeName()}({indexName})")
                            .CreateUniqueConstraint($"a:{obj.GetNodeName()}", $"a.{indexName}");

                        var q = graph.Cypher.Query;

                        cql.ExecuteWithoutResults();
                    }
                }
            }


            var merge = MergeStatement.For(obj, "a1");
            variableNameIndex++;

            var cypher = graph.Cypher
                .Merge(merge.ToString());

            Dictionary<string, object> labelDictionary = new Dictionary<string, object>();
            var nonIndexProperties = typeof(T).GetProperties().Where(p => !Attribute.IsDefined(p, typeof(GraphIndexAttribute)));
            if (nonIndexProperties.Count() > 0)
            {
                foreach (var p in nonIndexProperties)
                {
                    if (p.PropertyType.IsPrimitive)
                    {
                        labelDictionary.Add(p.GetLabelName(), p.GetValue(obj));
                    }
                }

                if (mergeBehaviour != MergeBehaviour.NEVER)
                {
                    if (mergeBehaviour == MergeBehaviour.ON_CREATE)
                    {
                        cypher = cypher.OnCreate();
                    }
                    foreach (var label in labelDictionary)
                    {
                        switch (System.Type.GetTypeCode(label.Value.GetType()))
                        {
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                                cypher = cypher.Set($"{merge.VariableName}.{label.Key} = {label.Value}");
                                break;
                            case TypeCode.String:
                                cypher = cypher.Set($"{merge.VariableName}.{label.Key} = '{label.Value}'");
                                break;
                            case TypeCode.Object:
                                // TODO
                                break;
                        }
                    }

                    
                }
                //cypher.OnCreate();
                // TODO: add properties
            }


            var navigationProperties = obj
                .GetType()
                .GetProperties()
                .Where(prop => !prop.PropertyType.IsPrimitive && prop.IsDefined(typeof(GraphRelationshipAttribute), false));

            foreach (var property in navigationProperties)
            {
                var attr = property.GetCustomAttribute(typeof(GraphRelationshipAttribute), true) as GraphRelationshipAttribute;
                if (attr != null)
                {
                    // Get the navigation property object, ensuring it isn't null
                    var val = property.GetValue(obj);
                    if (val != null)
                    {
                        // ensure that the object is annotated with the GraphNode attribute
                        if (val.GetType().GetCustomAttributes(typeof(GraphNodeAttribute), true).Any())
                        {
                            var merge2 = MergeStatement.For(val);
                            if (attr.Direction == RelationshipDirection.Incoming)
                            {
                                cypher = cypher.Merge(merge2.ToString())
                                    .Merge($"{merge.VariableName}<-[:{attr.Name}]-[{merge2.VariableName})");
                            }
                            else
                            {
                                cypher = cypher.Merge(merge2.ToString())
                                    .Merge($"{merge.VariableName}-[:{attr.Name}]->[{merge2.VariableName})");
                            }
                        }


                    }

                    if (attr.Direction == RelationshipDirection.Outgoing)
                    {
                        // Ensure that the other side of the relationship is not null and marked as a graph node

                    }
                }
                
            }




            var query = cypher.Query;

            return;
        }

        public void WriteAll<T>(List<T> objs)
        {
            var indexProperties = typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(GraphIndexAttribute)));

            if (indexProperties.Count() <= 0)
            {
                throw new NotImplementedException("Unable to persist an object that does not have any indices defined. Add the GraphIndex attribute on an appropriate, unique column");
            }

            // Ensure that the index exists for each property declared as an index for this 
            foreach (var index in indexProperties)
            {
                var indexName = index.GetLabelName();
                if (!graph.CheckIndexExists(indexName, IndexFor.Node))
                {
                    var config = new IndexConfiguration()
                    {
                        Provider = IndexProvider.lucene,
                        Type = IndexType.exact
                    };
                    graph.CreateIndex(indexName, config, IndexFor.Node);
                }
            }

        }


            //graph.Cypher
            //    .Merge()
            //g.Cypher
            //    .Merge("(a:PveAttack { id: {attackid} })")
            //    .OnCreate()
            //    .Set("a = {battle}")
            //    .Merge("(u:User { id: {userid} })")
            //    .Merge("(bt:BaseTemplate { level: {baselevel}, type: {basetype} })")
            //    .Merge("(b:Base { targetid: {targetid} })")
            //    .OnCreate()
            //    .Set("b = {target}")
            //    .Merge("(b)-[:INSTANCE_OF]->(bt)")
            //    .Merge("(u)-[:LAUNCHED_ATTACK]->(a)")
            //    .Merge("(a)-[:ATTACK_ON]->(b)")
            //    .WithParams(new

       
        [Obsolete]
        public string GetNodeNameFromObject(Object obj)
        {
            var nodeAttr = obj.GetType().GetCustomAttributes(typeof(GraphNodeAttribute), true).FirstOrDefault() as GraphNodeAttribute;
            return (string.IsNullOrWhiteSpace(nodeAttr.Name)) ? nodeAttr.Name : nameof(obj);
        }
        
        [Obsolete]
        public string GetLabelNameFromPropertyInfo(PropertyInfo info)
        {
            var attr = info.GetCustomAttributes(typeof(GraphLabelAttribute), true).FirstOrDefault() as GraphLabelAttribute;

            if (attr != null && string.IsNullOrWhiteSpace(attr.Name))
            {
                return attr.Name;
            }
            else
            {
                return info.Name.ToSnakeCase();
            }
        }


        public void IdentifyRelationships(Object obj)
        {
            var navigationProperties = obj.GetType().GetProperties().Where(prop => !prop.PropertyType.IsPrimitive && prop.IsDefined(typeof(GraphRelationshipAttribute), false));
            foreach (var property in navigationProperties)
            {
                var attr = property.GetType().GetCustomAttribute(typeof(GraphRelationshipAttribute), true) as GraphRelationshipAttribute;
                if (attr.Direction == RelationshipDirection.Outgoing)
                {
                    var val = property.GetValue(obj);
                    // Ensure that the other side of the relationship is not null and marked as a graph node
                    if (val != null)
                    {
                        var attr2 = val.GetType().GetCustomAttributes(typeof(GraphNodeAttribute), true).FirstOrDefault() as GraphNodeAttribute;

                        IdentifyRelationships(obj);
                    }
                }

               
            }
        }
    }
}
