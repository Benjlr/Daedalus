using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using RuleSets;
using RuleSets.Entry;
using TestUtils;
using Xunit;

namespace Thought.Tests
{

    public class BackTestFixture
    {
        private Universe _universeData { get; }
        public BacktestTests.Backtest TestinObject { get; }

        public BackTestFixture()
        {
            _universeData  = new Universe(new IRuleSet[1] { new DummyEntries(5, 10000) });
            _universeData.AddMarket(RandomBars.GenerateRandomMarket(1500), "longTest");
            _universeData.AddMarket(RandomBars.GenerateRandomMarket(1000), "mediumMarket");
            _universeData.AddMarket(RandomBars.GenerateRandomMarket(800), "shortMarket");

            var strat = new StrategyExecuter(MarketSide.Bull, true);
            TestinObject = new BacktestTests.Backtest(_universeData, strat);
        }

    }

    public class BacktestTests : IClassFixture<BackTestFixture>
    {
        private BackTestFixture _fixture;

        public BacktestTests(BackTestFixture fixt) {
            _fixture = fixt;
        }

        [Fact]
        private void GeneratesResults() {


            Assert.True(_fixture.TestinObject.ParseResults(TimeSpan.FromDays(1)).Count > 0);
        }

        [Fact]
        private void GeneratesResultsForDays() {
            var results = _fixture.TestinObject.ParseResults(TimeSpan.FromDays(1));

            Assert.True(results.Count > 0);
            for (int i = 1; i < results.Count; i++) {
                Assert.Equal(results[i].Date - results[i-1].Date, TimeSpan.FromDays(1));
            }
        }


        public class Backtest
        {
            public Universe Markets { get; set; }
            private List<Trade> _results { get; set; }

            public Backtest(Universe markets, StrategyExecuter exec) {
                _results = new List<Trade>();
                foreach (var element in markets.Elements) {
                    var trades = exec.Execute(element);
                    foreach (var trade in trades)
                        _results.Add(trade);
                }
            }

            public List<DatedResult> ParseResults(TimeSpan time) {
                //return _results.SelectMany(x => x.Results.ToList()).ToList();
                var retVal = new List<DatedResult>();
                var orderedResults = _results.OrderBy(x => x.Results.Last().Date).ToList();
                var totalSpan = (orderedResults.Last().Results.Last().Date.Ticks -
                                 orderedResults.First().Results.First().Date.Ticks);
                
                var iters =  totalSpan / (double)time.Ticks;
                for (long i = 0; i < totalSpan; i += time.Ticks)
                {
                    var resultIter = _results.Where(x => x.Results.Last().Date.Ticks > i && x.Results.Last().Date.Ticks < i + iters).ToList();
                    var avgDD = resultIter.Any(x => x.FinalDrawdown < 0)
                        ? resultIter.Where(x => x.FinalDrawdown < 0).Average(x => x.FinalDrawdown)
                        : 0;
                    retVal.Add(new DatedResult(new DateTime((long)iters+i), resultIter.Sum(x=>x.FinalResult), avgDD));
                }

                return retVal;
            }
        }
    }
}