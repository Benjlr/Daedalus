using System.Collections.Generic;
using Logic.Metrics;
using Logic.Rules.Entry;
using System.IO;
using Logic.Analysis.Metrics;
using Logic.Strategies;
using Xunit;
using RuleSets;

namespace Logic.Tests
{
    public class TestBaseTests
    {
        private List<ITest[]> myTests { get; set; }
        private ITest myInvalidTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";

        public TestBaseTests()
        {
            var market = MarketBuilder.CreateMarket(marketData);
            var invalidstrat = StrategyBuilder.CreateStrategy(new IRuleSet[] {
                new DummyEntries(65, 98)
            }, market);
            myInvalidTests = TestFactory.GenerateFixedBarExitTest(invalidstrat, market, new FixedBarExitTestOptions(10, 14, 1))[0][0];
            
            var strat = StrategyBuilder.CreateStrategy(new IRuleSet[] {
                new DummyEntries(1, 98)
            }, market);

            myTests = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1));
        }

        [Fact]
        public void ShouldGenerateCorrectLongAverages()
        {
            var avgGainsLong = new List<double>() { 0.0024375522, 0.0023488751, 0.0023916688, 0.0026418145 };
            var avgLossLong = new List<double>() { -0.0028625861, -0.0032561979, -0.0035221397, -0.0034242300 };
            var avgddLong = new List<double>() { -0.0033349619, -0.0034472730, -0.0035416383, -0.0036113720 };
            var avgddLongWinners = new List<double>() { -0.0020590124, -0.0023014395, -0.0022003207, -0.0023539033 };
            for (var i = 0; i < myTests.Count; i++)
            {
                Assert.Equal(TestUtils._round(myTests[i][0].AverageGain), TestUtils._round(avgGainsLong[i]));
                Assert.Equal(TestUtils._round(myTests[i][0].AverageLoss), TestUtils._round(avgLossLong[i]));
                Assert.Equal(TestUtils._round(myTests[i][0].AverageDrawdown), TestUtils._round(avgddLong[i]));
                Assert.Equal(TestUtils._round(myTests[i][0].AverageDrawdownWinners), TestUtils._round(avgddLongWinners[i]));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectLongMedians()
        {
            var medianGainsLong = new List<double>() { 0.0023664331, 0.0021832078, 0.0020914795, 0.0024704914 };
            var medianLossLong = new List<double>() { -0.0015575196, -0.0016302871, -0.0021896131, -0.0018975332 };
            var medianddLong = new List<double>() { -0.0022238882, -0.0023574050, -0.0026090518, -0.0027335419 };
            var medianddLongWinners = new List<double>() { -0.0019952839, -0.0019974602, -0.0019996364, -0.0020903390 };
            for (var i = 0; i < myTests.Count; i++)
            {
                Assert.Equal(TestUtils._round(myTests[i][0].MedianGain), TestUtils._round(medianGainsLong[i]));
                Assert.Equal(TestUtils._round(myTests[i][0].MedianLoss), TestUtils._round(medianLossLong[i]));
                Assert.Equal(TestUtils._round(myTests[i][0].MedianDrawDown), TestUtils._round(medianddLong[i]));
                Assert.Equal(TestUtils._round(myTests[i][0].MedianDrawDownWinners), TestUtils._round(medianddLongWinners[i]));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortAverages()
        {
            var avgGainsShort = new List<double>() { 0.0035051675, 0.0036991027, 0.0037803156, 0.0036420248 };
            var avgLossShort = new List<double>() { -0.0025857625, -0.0027376288, -0.0029556374, -0.0031197632 };
            var avgddShort = new List<double>() { -0.0032079160, -0.0033267906, -0.0034457517, -0.0035647277 };
            var avgddShortWinners = new List<double>() { -0.0021220591, -0.0020364951, -0.0019552167, -0.0019802343 };
            for (var i = 0; i < myTests.Count; i++)
            {
                Assert.Equal(TestUtils._round(myTests[i][1].AverageGain), TestUtils._round(avgGainsShort[i]));
                Assert.Equal(TestUtils._round(myTests[i][1].AverageLoss), TestUtils._round(avgLossShort[i]));
                Assert.Equal(TestUtils._round(myTests[i][1].AverageDrawdown), TestUtils._round(avgddShort[i]));
                Assert.Equal(TestUtils._round(myTests[i][1].AverageDrawdownWinners), TestUtils._round(avgddShortWinners[i]));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortMedians()
        {
            var medianGainsShort = new List<double>() { 0.0014436524, 0.0018273267, 0.0020181597, 0.0014134532 };
            var medianLossShort = new List<double>() { -0.0019305426, -0.0023617041, -0.0027282648, -0.0029048668 };
            var medianddShort = new List<double>() { -0.0026817853, -0.0028116652, -0.0032658992, -0.0034468231 };
            var medianddShortWinners = new List<double>() { -0.0013752420, -0.0013752420, -0.0012938823, -0.0013027666 };
            for (var i = 0; i < myTests.Count; i++)
            {
                Assert.Equal(TestUtils._round(myTests[i][1].MedianGain), TestUtils._round(medianGainsShort[i]));
                Assert.Equal(TestUtils._round(myTests[i][1].MedianLoss), TestUtils._round(medianLossShort[i]));
                Assert.Equal(TestUtils._round(myTests[i][1].MedianDrawDown), TestUtils._round(medianddShort[i]));
                Assert.Equal(TestUtils._round(myTests[i][1].MedianDrawDownWinners), TestUtils._round(medianddShortWinners[i]));
            }
        }

        [Fact]
        public void ShouldGenerateWinRatios()
        {
            var longRatios = new List<double>() { 0.4183673469, 0.4742268041, 0.5051546392, 0.4895833333 };
            var shortRatios = new List<double>() { 0.2783505155, 0.2783505155, 0.2886597938, 0.2989690722 };
            for (var i = 0; i < myTests.Count; i++)
            {
                Assert.Equal(TestUtils._round(myTests[i][0].WinPercentage), TestUtils._round(longRatios[i]));
                Assert.Equal(TestUtils._round(myTests[i][1].WinPercentage), TestUtils._round(shortRatios[i]));
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyLong()
        {
            var medianExp = new List<double>() { 1.0928730468, 1.2078656525, 0.9750818373, 1.2488081700 };
            var avgExp = new List<double>() { 0.6124975585, 0.6506340453, 0.6931852388, 0.7400161033 };
            for (var i = 0; i < myTests.Count; i++)
            {
                Assert.Equal(TestUtils._round(myTests[i][0].ExpectancyAverage), TestUtils._round(avgExp[i]));
                Assert.Equal(TestUtils._round(myTests[i][0].ExpectancyMedian), TestUtils._round(medianExp[i]));
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyShort()
        {
            var medianExp = new List<double>() { 0.2884356869, 0.2984395950, 0.3001773677, 0.2075125104 };
            var avgExp = new List<double>() { 0.5228605260, 0.5211797687, 0.5190220998, 0.4978635718 };
            for (var i = 0; i < myTests.Count; i++)
            {
                Assert.Equal(TestUtils._round(myTests[i][1].ExpectancyAverage), TestUtils._round(avgExp[i]));
                Assert.Equal(TestUtils._round(myTests[i][1].ExpectancyMedian), TestUtils._round(medianExp[i]));
            }
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
