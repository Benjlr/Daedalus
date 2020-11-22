using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using DataStructures.PriceAlgorithms;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class LiquidityFilterTests
    {
        [Fact]
        private void ShouldBeLiquid() {

            var myCloses = new List<double>();
            var myVols = new List<double>();
            for (int i = 0; i < 100; i++) {
                myCloses.Add(new Random().Next(1, 5));
                myVols.Add(new Random().Next(20000, 400000));
            }

            var liq = LiquidityFilter.IsLiquid(myCloses, myVols);
            Assert.True(liq);
        }

        [Fact]
        private void ShouldNotBeLiquid() {
            var myCloses = new List<double>();
            var myVols = new List<double>();
            for (int i = 0; i < 100; i++) {
                myCloses.Add(new Random().Next(1, 5) / 10.0);
                myVols.Add(new Random().Next(20000, 400000));
            }

            var liq = LiquidityFilter.IsLiquid(myCloses, myVols);
            Assert.False(liq);
        }
    }
}
