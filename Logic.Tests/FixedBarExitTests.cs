using Logic.Metrics;
using Logic.Rules.Entry;
using Logic.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTests
    {
        private ITest[] myTests { get; set; }
        private ITest myInvalidTests { get; set; }

        public FixedBarExitTests()
        {
            var market = MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);
            var strat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[]
            {
                new DummyEntries(1, 98)
            }, market);

            myTests = TestFactory.GenerateFixedBarExitTest(10, 14, strat, market);

            var invalidstrat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[]
            {
                new DummyEntries(65, 98)
            }, market);
            myInvalidTests = TestFactory.GenerateFixedBarExitTest(10, 11, invalidstrat, market)[0];
        }

        private List<List<double>> LoadData(string path)
        {
            var myLists = InitList();
            var files = File.ReadAllLines(path);
            for (var i = 0; i < files.Length; i++)
            {
                var row = files[i].Split(',').ToList();
                myLists[0].Add(double.Parse(row[0]));
                myLists[1].Add(double.Parse(row[1]));
                myLists[2].Add(double.Parse(row[2]));
                myLists[3].Add(double.Parse(row[3]));
            }

            return myLists;
        }

        private List<List<double>> InitList()
        {
            var results = new List<List<double>> {new List<double>(), new List<double>(), new List<double>(), new List<double>()};
            return results;
        }

        [Fact]
        public void ShouldGenerateLongResults()
        {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\LongData.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(Math.Round(myTests[i].FBELong[j], 4), Math.Round(resultsLong[i][j], 4));
        }

        [Fact]
        public void ShouldGenerateShortResults()
        {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\ShortData.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(Math.Round(myTests[i].FBEShort[j], 4), Math.Round(resultsLong[i][j], 4));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults()
        {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownData.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(Math.Round(myTests[i].FBEDrawdownLong[j], 4), Math.Round(resultsLong[i][j], 4));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults()
        {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownShort.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(Math.Round(myTests[i].FBEDrawdownShort[j], 4), Math.Round(resultsLong[i][j], 4));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongWinnersResults()
        {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownDataWinners.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(Math.Round(myTests[i].FBEDrawdownLongWinners[j], 4), Math.Round(resultsLong[i][j], 4));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortWinnersResults()
        {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownShortWinners.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(Math.Round(myTests[i].FBEDrawdownShortWinners[j], 4), Math.Round(resultsLong[i][j], 4));
        }

        [Fact]
        public void ShouldGenerateCorrectLongAverages()
        {
            var avgGainsLong = new List<double>() {13.37317073, 12.88478261, 13.11836735, 14.4893617};
            var avgLossLong = new List<double>() {-15.80175439, -17.9745098, -19.44375, -18.90612245};
            var avgddLong = new List<double>() {-18.38571429, -19.00510204, -19.5255102, -19.91020408};
            var avgddLongWinners = new List<double>() {-11.31707317, -12.65217391, -12.09183673, -12.93617021};
            for (var i = 0; i < myTests.Length; i++)
            {
                Assert.Equal(Math.Round(myTests[i].AverageGainLong, 4), Math.Round(avgGainsLong[i], 4));
                Assert.Equal(Math.Round(myTests[i].AverageLossLong, 4), Math.Round(avgLossLong[i], 4));
                Assert.Equal(Math.Round(myTests[i].AverageDrawdownLong, 4), Math.Round(avgddLong[i], 4));
                Assert.Equal(Math.Round(myTests[i].AverageDrawdownWinnersLong, 4), Math.Round(avgddLongWinners[i], 4));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectLongMedians()
        {
            var medianGainsLong = new List<double>() {13, 12, 11.5, 13.5};
            var medianLossLong = new List<double>() {-8.6, -9, -12.1, -10.5};
            var medianddLong = new List<double>() {-12.25, -13, -14.25, -15};
            var medianddLongWinners = new List<double>() {-11, -11, -11, -11.5};
            for (var i = 0; i < myTests.Length; i++)
            {
                Assert.Equal(Math.Round(myTests[i].MedianGainLong, 4), Math.Round(medianGainsLong[i], 4));
                Assert.Equal(Math.Round(myTests[i].MedianLossLong, 4), Math.Round(medianLossLong[i], 4));
                Assert.Equal(Math.Round(myTests[i].MedianDrawDownLong, 4), Math.Round(medianddLong[i], 4));
                Assert.Equal(Math.Round(myTests[i].MedianDrawDownWinnersLong, 4), Math.Round(medianddLongWinners[i], 4));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortAverages()
        {
            var avgGainsShort = new List<double>() {19.33333333, 20.4, 20.84642857, 20.0862069};
            var avgLossShort = new List<double>() {-14.19428571, -15.02571429, -16.22028986, -17.11911765};
            var avgddShort = new List<double>() {-17.63265306, -18.28571429, -18.93877551, -19.59183673};
            var avgddShortWinners = new List<double>() {-11.68888889, -11.22592593, -10.77857143, -10.92068966};
            for (var i = 0; i < myTests.Length; i++)
            {
                Assert.Equal(Math.Round(myTests[i].AverageGainShort, 4), Math.Round(avgGainsShort[i], 4));
                Assert.Equal(Math.Round(myTests[i].AverageLossShort, 4), Math.Round(avgLossShort[i], 4));
                Assert.Equal(Math.Round(myTests[i].AverageDrawdownShort, 4), Math.Round(avgddShort[i], 4));
                Assert.Equal(Math.Round(myTests[i].AverageDrawdownWinnersShort, 4), Math.Round(avgddShortWinners[i], 4));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortMedians()
        {
            var medianGainsShort = new List<double>() {8, 10.1, 11.1, 7.8};
            var medianLossShort = new List<double>() {-10.65, -13, -15, -16};
            var medianddShort = new List<double>() {-14.75, -15.5, -18, -19};
            var medianddShortWinners = new List<double>() {-7.6, -7.6, -7.15, -7.2};
            for (var i = 0; i < myTests.Length; i++)
            {
                Assert.Equal(Math.Round(myTests[i].MedianGainShort, 4), Math.Round(medianGainsShort[i], 4));
                Assert.Equal(Math.Round(myTests[i].MedianLossShort, 4), Math.Round(medianLossShort[i], 4));
                Assert.Equal(Math.Round(myTests[i].MedianDrawDownShort, 4), Math.Round(medianddShort[i], 4));
                Assert.Equal(Math.Round(myTests[i].MedianDrawDownWinnersShort, 4), Math.Round(medianddShortWinners[i], 4));
            }
        }

        [Fact]
        public void ShouldGenerateWinRatios()
        {
            var longRatios = new List<double>() {0.418367347,0.474226804,0.505154639,0.489583333};
            var shortRatios = new List<double>() {0.278350515,0.278350515,0.288659794,0.298969072
            };
            for (var i = 0; i < myTests.Length; i++)
            {
                Assert.Equal(Math.Round(myTests[i].WinPercentageLong, 4), Math.Round(longRatios[i], 4));
                Assert.Equal(Math.Round(myTests[i].WinPercentageShort, 4), Math.Round(shortRatios[i], 4));
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyLong()
        {
            var medianExp = new List<double>() {1.087311302,1.202614379,0.970213499,1.233236152};
            var avgExp = new List<double>() {0.608748751,0.646558307,0.688738884,0.735103627};
            for (var i = 0; i < myTests.Length; i++)
            {
                Assert.Equal(Math.Round(myTests[i].ExpectancyLongAverage, 4), Math.Round(avgExp[i], 4));
                Assert.Equal(Math.Round(myTests[i].ExpectancyLongMedian, 4), Math.Round(medianExp[i], 4));
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyShort()
        {
            var medianExp = new List<double>() {0.289738431,0.29967033,0.300289855,0.207904412};
            var avgExp = new List<double>() {0.525362319,0.523673702,0.521533238,0.500386565};
            for (var i = 0; i < myTests.Length; i++)
            {
                Assert.Equal(Math.Round(myTests[i].ExpectancyShortAverage, 4), Math.Round(avgExp[i], 4));
                Assert.Equal(Math.Round(myTests[i].ExpectancyShortMedian, 4), Math.Round(medianExp[i], 4));
            }
        }

        [Fact]
        public void ShouldNotGenerateExpectancies()
        {
            Assert.Equal(myInvalidTests.ExpectancyLongAverage, 0);
            Assert.Equal(myInvalidTests.ExpectancyLongMedian, 0);
            Assert.Equal(myInvalidTests.ExpectancyShortAverage, 0);
            Assert.Equal(myInvalidTests.ExpectancyShortMedian, 0);
        }

        [Fact]
        private void ShouldNotGenerateRatios()
        {
            Assert.Equal(myInvalidTests.WinPercentageShort, 0);
            Assert.Equal(myInvalidTests.WinPercentageLong, 0);
        }

        [Fact]
        private void ShouldNotGenerateAveragesLong()
        {
            Assert.Equal(myInvalidTests.AverageGainLong, 0);
            Assert.Equal(myInvalidTests.AverageLossLong, 0);
            Assert.Equal(myInvalidTests.AverageDrawdownLong, 0);
            Assert.Equal(myInvalidTests.AverageDrawdownWinnersLong, 0);
        }

        [Fact]
        private void ShouldNotGenerateMediansLong()
        {
            Assert.Equal(myInvalidTests.MedianGainLong, 0);
            Assert.Equal(myInvalidTests.MedianLossLong, 0);
            Assert.Equal(myInvalidTests.MedianDrawDownLong, 0);
            Assert.Equal(myInvalidTests.MedianDrawDownWinnersLong, 0);
        }

        [Fact]
        private void ShouldNotGenerateAveragesShort()
        {
            Assert.Equal(myInvalidTests.AverageGainShort, 0);
            Assert.Equal(myInvalidTests.AverageLossShort, 0);
            Assert.Equal(myInvalidTests.AverageDrawdownShort, 0);
            Assert.Equal(myInvalidTests.AverageDrawdownWinnersShort, 0);
        }

        [Fact]
        private void ShouldNotGenerateMediansShort()
        {
            Assert.Equal(myInvalidTests.MedianGainShort, 0);
            Assert.Equal(myInvalidTests.MedianLossShort, 0);
            Assert.Equal(myInvalidTests.MedianDrawDownShort, 0);
            Assert.Equal(myInvalidTests.MedianDrawDownWinnersShort, 0);
        }
    }
}