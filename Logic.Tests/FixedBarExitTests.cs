using Logic.Metrics;
using Logic.Rules.Entry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Logic.Metrics.EntryTests.TestsDrillDown;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTests
    {
        private int _precision = 6;
        private List<ITest[]> myTests { get; set; }
        private ITest myInvalidTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";        
        private double _round(double x) => Math.Round(x, _precision);
        

        public FixedBarExitTests() {
            var market = MarketBuilder.CreateMarket(marketData);
            var strat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[] {
                new DummyEntries(1, 98)
            }, market);

            myTests = TestFactory.GenerateFixedBarExitTest(10, 14, strat, market);

            var invalidstrat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[] {
                new DummyEntries(65, 98)
            }, market);
            myInvalidTests = TestFactory.GenerateFixedBarExitTest(10, 11, invalidstrat, market)[0][0];
        }

        private List<List<double>> LoadData(string path) {
            var myLists = InitList();
            var files = File.ReadAllLines(path);
            for (var i = 0; i < files.Length; i++) {
                var row = files[i].Split(',').ToList();
                myLists[0].Add(double.Parse(row[0]));
                myLists[1].Add(double.Parse(row[1]));
                myLists[2].Add(double.Parse(row[2]));
                myLists[3].Add(double.Parse(row[3]));
            }

            return myLists;
        }
        private List<double> LoadDataSingleColumn(string path)
        {
            var myLists = new List<double>();
            var files = File.ReadAllLines(path);
            for (var i = 0; i < files.Length; i++) {
                myLists.Add(double.Parse(files[i]));
            }

            return myLists;
        }


        private List<List<double>> InitList() {
            var results = new List<List<double>> {new List<double>(), new List<double>(), new List<double>(), new List<double>()};
            return results;
        }

        [Fact]
        public void ShouldGenerateLongResults() {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\LongData.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(_round(myTests[i][0].FBEResults[j]), _round(resultsLong[i][j]));
        }

        [Fact]
        public void ShouldGenerateShortResults() {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\ShortData.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(_round(myTests[i][1].FBEResults[j]), _round(resultsLong[i][j]));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults() {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownData.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(_round(myTests[i][0].FBEDrawdown[j]), _round(resultsLong[i][j]));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults() {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownShort.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(_round(myTests[i][1].FBEDrawdown[j]), _round(resultsLong[i][j]));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongWinnersResults() {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownDataWinners.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(_round(myTests[i][0].FBEDrawdownWinners[j]), _round(resultsLong[i][j]));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortWinnersResults() {
            var resultsLong = LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownShortWinners.txt");
            for (var i = 0; i < resultsLong.Count; i++)
            for (var j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(_round(myTests[i][1].FBEDrawdownWinners[j]), _round(resultsLong[i][j]));
        }

        [Fact]
        public void ShouldGenerateCorrectLongAverages() {
            var avgGainsLong = new List<double>() {0.002438,0.002349,0.002392,0.002642};
            var avgLossLong = new List<double>() {-0.002863,-0.003256,-0.003522,-0.003424};
            var avgddLong = new List<double>() {-0.003335,-0.003447,-0.003542,-0.003611};
            var avgddLongWinners = new List<double>() {-0.002059,-0.002301,-0.002200,-0.002354};
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(_round(myTests[i][0].AverageGain), _round(avgGainsLong[i]));
                Assert.Equal(_round(myTests[i][0].AverageLoss), _round(avgLossLong[i]));
                Assert.Equal(_round(myTests[i][0].AverageDrawdown), _round(avgddLong[i]));
                Assert.Equal(_round(myTests[i][0].AverageDrawdownWinners), _round(avgddLongWinners[i]));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectLongMedians() {
            var medianGainsLong = new List<double>() {0.002366,0.002183,0.002091,0.002470};
            var medianLossLong = new List<double>() {-0.001558,-0.001630,-0.002190,-0.001898};
            var medianddLong = new List<double>() {-0.002224,-0.002357,-0.002609,-0.002734};
            var medianddLongWinners = new List<double>() {-0.001995,-0.001997,-0.002000,-0.002090};
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(_round(myTests[i][0].MedianGain), _round(medianGainsLong[i]));
                Assert.Equal(_round(myTests[i][0].MedianLoss), _round(medianLossLong[i]));
                Assert.Equal(_round(myTests[i][0].MedianDrawDown), _round(medianddLong[i]));
                Assert.Equal(_round(myTests[i][0].MedianDrawDownWinners), _round(medianddLongWinners[i]));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortAverages() {
            var avgGainsShort = new List<double>() {0.003505,0.003699,0.003780,0.003642};
            var avgLossShort = new List<double>() {-0.002586,-0.002738,-0.002956,-0.003120};
            var avgddShort = new List<double>() {-0.003208,-0.003327,-0.003446,-0.003565};
            var avgddShortWinners = new List<double>() {-0.002122,-0.002036,-0.001955,-0.001980};
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(_round(myTests[i][1].AverageGain), _round(avgGainsShort[i]));
                Assert.Equal(_round(myTests[i][1].AverageLoss), _round(avgLossShort[i]));
                Assert.Equal(_round(myTests[i][1].AverageDrawdown), _round(avgddShort[i]));
                Assert.Equal(_round(myTests[i][1].AverageDrawdownWinners), _round(avgddShortWinners[i]));
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortMedians() {
            var medianGainsShort = new List<double>() {0.001444,0.001827,0.002018,0.001413};
            var medianLossShort = new List<double>() {-0.001931,-0.002362,-0.002728,-0.002905};
            var medianddShort = new List<double>() {-0.002682,-0.002812,-0.003266,-0.003447};
            var medianddShortWinners = new List<double>() {-0.001375,-0.001375,-0.001294,-0.001303};
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(_round(myTests[i][1].MedianGain), _round(medianGainsShort[i]));
                Assert.Equal(_round(myTests[i][1].MedianLoss), _round(medianLossShort[i]));
                Assert.Equal(_round(myTests[i][1].MedianDrawDown), _round(medianddShort[i]));
                Assert.Equal(_round(myTests[i][1].MedianDrawDownWinners), _round(medianddShortWinners[i]));
            }
        }

        [Fact]
        public void ShouldGenerateWinRatios() {
            var longRatios = new List<double>() {0.418367,0.474227,0.505155,0.489583};
            var shortRatios = new List<double>() {0.278351,0.278351,0.288660,0.298969};
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(_round(myTests[i][0].WinPercentage), _round(longRatios[i]));
                Assert.Equal(_round(myTests[i][1].WinPercentage), _round(shortRatios[i]));
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyLong() {
            var medianExp = new List<double>() {1.092873,1.207866,0.975082,1.248808};
            var avgExp = new List<double>() {0.612498,0.650634,0.693185,0.740016};
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(_round(myTests[i][0].ExpectancyAverage), _round(avgExp[i]));
                Assert.Equal(_round(myTests[i][0].ExpectancyMedian), _round(medianExp[i]));
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyShort() {
            var medianExp = new List<double>() {0.288436,0.298440,0.300177,0.207513};
            var avgExp = new List<double>() {0.522861,0.521180,0.519022,0.497864};
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(_round(myTests[i][1].ExpectancyAverage), _round(avgExp[i]));
                Assert.Equal(_round(myTests[i][1].ExpectancyMedian), _round(medianExp[i]));
            }
        }

        [Fact]
        public void ShouldNotGenerateExpectancies() {
            Assert.Equal(0, myInvalidTests.ExpectancyAverage);
            Assert.Equal(0, myInvalidTests.ExpectancyMedian);
        }

        [Fact]
        private void ShouldNotGenerateRatios() {
            Assert.Equal(0, myInvalidTests.WinPercentage);
        }

        [Fact]
        private void ShouldNotGenerateAveragesLong() {
            Assert.Equal(0, myInvalidTests.AverageGain);
            Assert.Equal(0, myInvalidTests.AverageLoss);
            Assert.Equal(0, myInvalidTests.AverageDrawdown);
            Assert.Equal(0, myInvalidTests.AverageDrawdownWinners);
        }

        [Fact]
        private void ShouldNotGenerateMediansLong() {
            Assert.Equal(0, myInvalidTests.MedianGain);
            Assert.Equal(0, myInvalidTests.MedianLoss);
            Assert.Equal(0, myInvalidTests.MedianDrawDown);
            Assert.Equal(0, myInvalidTests.MedianDrawDownWinners);
        }

        [Fact]
        private void ShouldNotGenerateAveragesShort() {
            Assert.Equal(0, myInvalidTests.AverageGain);
            Assert.Equal(0, myInvalidTests.AverageLoss);
            Assert.Equal(0, myInvalidTests.AverageDrawdown);
            Assert.Equal(0, myInvalidTests.AverageDrawdownWinners);
        }

        [Fact]
        private void ShouldNotGenerateMediansShort() {
            Assert.Equal(0, myInvalidTests.MedianGain);
            Assert.Equal(0, myInvalidTests.MedianLoss);
            Assert.Equal(0, myInvalidTests.MedianDrawDown);
            Assert.Equal(0, myInvalidTests.MedianDrawDownWinners);
        }

        [Fact]
        private void ShouldGenerateLongRunHistory() {
            for (int i = 0; i < myTests.Count; i++)
            for (int j = 0; j < myTests[i][0].RunIndices.Count; j++) {
                var myindt = new int[i + 1 + 10];
                for (int k = 0; k < myindt.Length; k++) myindt[k] = 1 + j + k;
                Assert.Equal(myTests[i][0].RunIndices[j], myindt);
            }
        }

        [Fact]
        private void ShouldGenerateShortRunHistory() {
            for (int i = 0; i < myTests.Count; i++)
                for (int j = 0; j < myTests[i][1].RunIndices.Count; j++) {
                    var myindt = new int[i + 1 + 10];
                    for (int k = 0; k < myindt.Length; k++) myindt[k] = 1 + j + k;
                    Assert.Equal(myTests[i][1].RunIndices[j], myindt);
                }
        }

        [Fact]
        private void ShouldGenerateRollingExpectancy() {
            var resultsLong = LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\FBEData\\Rolling8Exp.txt");
            var resultsLongSeventeen = LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\FBEData\\Rolling18Exp.txt");
            var eight = EntryTestDrilldown.GetRollingExpectancy(myTests[0][0].FBEResults.ToList(), 8);
            var Seventeen = EntryTestDrilldown.GetRollingExpectancy(myTests[0][0].FBEResults.ToList(), 18);
            Assert.Equal(eight.Select(_round).ToList(), resultsLong.Select(_round).ToList());
            Assert.Equal(Seventeen.Select(_round).ToList(), resultsLongSeventeen.Select(_round).ToList());
        }
        
        [Fact]
        private void ShouldGenerateExpectancyByPeriod() {
            var resultsTenPeriod = LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\FBEData\\10PeriodExp.txt");
            var resultsThirtyPeriod = LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\FBEData\\30PeriodExp.txt");
            var ten = EntryTestDrilldown.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 10);
            var thirty = EntryTestDrilldown.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 30);
            Assert.Equal(resultsTenPeriod.Select(_round).ToList(), ten.Select(_round).ToList());
            Assert.Equal(resultsThirtyPeriod.Select(_round).ToList(), thirty.Select(_round).ToList());
        }
    }
}