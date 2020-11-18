using System;
using System.Collections.Generic;
using System.Text;
using Logic.Analysis.Metrics;
using Xunit;

namespace Logic.Tests
{
    class TradeStructTests
    {
        [Fact]
        public void ShouldGenerateTrade() {

            Assert.Equal(new Trade(new []{-0.2,0.2,0.4}, 0), );
        }
    }
}
