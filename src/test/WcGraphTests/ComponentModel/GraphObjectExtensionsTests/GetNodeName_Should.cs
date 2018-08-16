using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.ComponentModel;
using WcGraph.Models;
using Xunit;

namespace WcGraphTests.ComponentModel.GraphObjectExtensionsTests
{
    public class GetNodeName_Should
    {
        [Fact]
        public void ReturnClassNameWhenNoNameSpecified()
        {
            var entity = new EntityWithoutSpecifiedName();

            var nodeName = entity.GetNodeName();

            nodeName.ShouldBe("EntityWithoutSpecifiedName");
        }

        [Fact]
        public void ReturnSpecifiedName()
        {
            var entity = new EntityWithSpecifiedName();

            var nodeName = entity.GetNodeName();

            nodeName.ShouldBe("Test");
        }

        [Fact]
        public void ReturnClassNameWhenNoabelPresent()
        {
            var entity = new EntityWithoutAttribute();

            var nodeName = entity.GetNodeName();

            nodeName.ShouldBe("EntityWithoutAttribute");
        }
    }
}
