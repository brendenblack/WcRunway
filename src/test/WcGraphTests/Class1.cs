using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.Models;
using Xunit;

namespace WcGraphTests
{
    public class Class1
    {
        [Fact]
        public void NameOfType()
        {
            var name = nameof(PveBattle);

            name.ShouldBe("PveBattle");
        }
    }
}
