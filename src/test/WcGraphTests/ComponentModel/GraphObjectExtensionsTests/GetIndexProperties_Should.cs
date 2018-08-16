using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcGraph.ComponentModel;
using Xunit;

namespace WcGraphTests.ComponentModel.GraphObjectExtensionsTests
{
    public class GetIndexProperties_Should
    {
        [Fact]
        public void ReturnIndexProperties()
        {
            var entity = new EntityWithIndexProperties();

            var indexProperties = entity.GetIndexProperties();

            indexProperties.Count().ShouldBe(2);
        }

        [Fact]
        public void ReturnEmptyListWhenNoIndexPropertiesArePresent()
        {
            var entity = new EntityWithoutIndexProperties();

            var indexProperties = entity.GetIndexProperties();

            indexProperties.Count().ShouldBe(0);
        }
    }
}
