using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Logic.Metrics;
using Logic.Rules.Entry;
using Xunit;

namespace Logic.Tests
{
    public class TestBaseTests
    {
        private ITest myInvalidTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";

        public TestBaseTests()
        {
            var market = MarketBuilder.CreateMarket(marketData);

            var invalidstrat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[] {
                new DummyEntries(65, 98)
            }, market);
            myInvalidTests = TestFactory.GenerateFixedBarExitTest(invalidstrat, market, new FixedBarExitTestOptions(10, 14, 1))[0][0];

        }

        [Fact]
        public void ShouldNotGenerateExpectancies()
        {
            Assert.Equal(0, myInvalidTests.ExpectancyAverage);
            Assert.Equal(0, myInvalidTests.ExpectancyMedian);
        }

        [Fact]
        private void ShouldNotGenerateRatios()
        {
            Assert.Equal(0, myInvalidTests.WinPercentage);
        }

        [Fact]
        private void ShouldNotGenerateAveragesLong()
        {
            Assert.Equal(0, myInvalidTests.AverageGain);
            Assert.Equal(0, myInvalidTests.AverageLoss);
            Assert.Equal(0, myInvalidTests.AverageDrawdown);
            Assert.Equal(0, myInvalidTests.AverageDrawdownWinners);
        }

        [Fact]
        private void ShouldNotGenerateMediansLong()
        {
            Assert.Equal(0, myInvalidTests.MedianGain);
            Assert.Equal(0, myInvalidTests.MedianLoss);
            Assert.Equal(0, myInvalidTests.MedianDrawDown);
            Assert.Equal(0, myInvalidTests.MedianDrawDownWinners);
        }

        [Fact]
        private void ShouldNotGenerateAveragesShort()
        {
            Assert.Equal(0, myInvalidTests.AverageGain);
            Assert.Equal(0, myInvalidTests.AverageLoss);
            Assert.Equal(0, myInvalidTests.AverageDrawdown);
            Assert.Equal(0, myInvalidTests.AverageDrawdownWinners);
        }

        [Fact]
        private void ShouldNotGenerateMediansShort()
        {
            Assert.Equal(0, myInvalidTests.MedianGain);
            Assert.Equal(0, myInvalidTests.MedianLoss);
            Assert.Equal(0, myInvalidTests.MedianDrawDown);
            Assert.Equal(0, myInvalidTests.MedianDrawDownWinners);
        }
    }
}
