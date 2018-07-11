﻿using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcRunway.Core;
using Xunit;

namespace WcRunway.CoreTests
{
    public class StandardLevelSchemeTests
    {
        public StandardLevelSchemeTests()
        {
            this.sut = new StandardLevelScheme();
        }

        private readonly StandardLevelScheme sut;

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void LevelsLessThanOrEqualTo10ShouldReturn55(int level)
        {
            int tokens = this.sut.CalculateTokensForLevel(level);

            tokens.ShouldBe(55);
        }

        [Theory]
        [InlineData(11, 54)]
        [InlineData(12, 52)]
        [InlineData(13, 49)]
        [InlineData(14, 45)]
        [InlineData(15, 40)]
        [InlineData(16, 34)]
        [InlineData(17, 27)]
        [InlineData(18, 19)]
        [InlineData(19, 10)]
        [InlineData(20, 0)]
        public void LevelsGreaterThan10ShouldReturnExpectedValue(int level, int expected)
        {
            int tokens = this.sut.CalculateTokensForLevel(level);

            tokens.ShouldBe(expected);
        }
    }
}
