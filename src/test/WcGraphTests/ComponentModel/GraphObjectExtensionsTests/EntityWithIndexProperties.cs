using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.ComponentModel.DataAnnotations.Schema;

namespace WcGraphTests.ComponentModel.GraphObjectExtensionsTests
{
    [GraphNode]
    public class EntityWithIndexProperties
    {
        [GraphIndex]
        public int Id { get; set; }

        [GraphIndex]
        [GraphLabel("my_second_index")]
        public int OtherId { get; set; }

        public string NonIndexProperty { get; set; }
        

    }
}
