using Logic.Analysis.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Logic.Tests.FBEData;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTests
    {

        private List<ITest[]> myTests { get; set; }
        //private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";


        public FixedBarExitTests() 
        {
            var market = Market.MarketBuilder.CreateMarket(TestBars.DataLong);
            var strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(2, TestBars.DataLong.Length)
            }, market);

            var longSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(2, 4, 2, MarketSide.Bull));
            var shortSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(2, 4, 2, MarketSide.Bear));


            myTests = new List<ITest[]>();
            for (int i = 0; i < longSide.Count; i++)
                myTests.Add(new[] { longSide[i], shortSide[i] });
        }



        [Fact]
        public void ShouldGenerateLongResults() {
            var arrayOne = new double[]
            {
                0,
                0,
                (12 - 9) / 9.0,
                (13 - 9) / 9.0,
                (15 - 13) / 13.0,
                (7.0 - 15.0) / 15.0,
                (6 - 15.0) / 15.0,
                (9 - 6) / 6.0,
                (13 - 6) / 6.0,
                (14.0 - 13.0) / 13.0
            };

            Assert.Equal(arrayOne, myTests[0][0].FBEResults);
            Assert.Equal(new double[] { }, myTests[1][0].FBEResults);
        }

        [Fact]
        public void ShouldGenerateShortResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\ShortData.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].FBEResults.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownData.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][0].FBEDrawdown.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownShort.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].FBEDrawdown.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateLongDurations()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DurationLong.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][0].Durations, resultsLong[i].Select(x=>(int)x));
        }

        [Fact]
        public void ShouldGenerateShortDurations()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DurationShort.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].Durations, resultsLong[i].Select(x => (int)x));
        }
    }
}