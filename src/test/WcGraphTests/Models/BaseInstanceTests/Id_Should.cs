using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.Models;
using Xunit;

namespace WcGraphTests.Models.BaseInstanceTests
{
    public class Id_Should
    {
        [Fact]
        public void BeEqualWhenXYSectorLevelAndTypeAreTheSame()
        {
            var base1 = new BaseInstance
            {
                Base = new Base
                {
                    Level = 10,
                    Type = "test_base"
                },
                Sector = 2,
                XCoordinate = 123,
                YCoordinate = 254
            };
            var base2 = new BaseInstance
            {
                Base = new Base
                {
                    Level = 10,
                    Type = "test_base"
                },
                Sector = 2,
                XCoordinate = 123,
                YCoordinate = 254
            };

            base1.Id.ShouldBe(base2.Id);
        }

        [Theory]
        [InlineData(123, 321)]
        public void NotBeEqualWhenXIsDifferent(int x1, int x2)
        {
            var base1 = new BaseInstance
            {
                Base = new Base
                {
                    Level = 10,
                    Type = "test_base"
                },
                Sector = 2,
                XCoordinate = x1,
                YCoordinate = 254
            };
            var base2 = new BaseInstance
            {
                Base = new Base
                {
                    Level = 10,
                    Type = "test_base"
                },
                Sector = 2,
                XCoordinate = x2,
                YCoordinate = 254
            };

            base1.Id.ShouldNotBe(base2.Id);
        }

        [Theory]
        [InlineData(123, 321)]
        public void NotBeEqualWhenYIsDifferent(int y1, int y2)
        {
            var base1 = new BaseInstance
            {
                Base = new Base
                {
                    Level = 10,
                    Type = "test_base"
                },
                Sector = 2,
                XCoordinate = 254,
                YCoordinate = y1
            };
            var base2 = new BaseInstance
            {
                Base = new Base
                {
                    Level = 10,
                    Type = "test_base"
                },
                Sector = 2,
                XCoordinate = 254,
                YCoordinate = y2
            };

            base1.Id.ShouldNotBe(base2.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void NotBeEqualWhenSectorIsDifferent(int sector)
        {
            var base1 = new BaseInstance
            {
                Base = new Base
                {
                    Level = 10,
                    Type = "test_base"
                },
                Sector = 2,
                XCoordinate = 254,
                YCoordinate = 123
            };
            var base2 = new BaseInstance
            {
                Base = new Base
                {
                    Level = 10,
                    Type = "test_base"
                },
                Sector = sector,
                XCoordinate = 254,
                YCoordinate = 123
            };

            base1.Id.ShouldNotBe(base2.Id);
        }

    }
}
