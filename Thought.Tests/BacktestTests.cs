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
        public Universe UniverseData { get; private set; }

    }

    public class BacktestTests
    {
        
        [Fact]
        private void GeneratesResults() {
            var myUnivers = new Universe(new IRuleSet[1] { new DummyEntries(5, 40) });
            myUnivers.AddMarket(TradeFlatteningData.longMarket, "longTest");
            myUnivers.AddMarket(TradeFlatteningData.mediumMarket, "mediumMarket");
            myUnivers.AddMarket(TradeFlatteningData.shortMarket, "shortMarket");

            var strat = new StrategyExecuter(MarketSide.Bull, true);

            var Backtest = new Backtest(myUnivers, strat);

            Assert.True(Backtest.ParseResults(TimeSpan.FromDays(1)).Count >0 );
        }

        [Fact]
        private void GeneratesResultsForDays() {
            var myUnivers = new Universe(new IRuleSet[1] { new DummyEntries(5, 40) });
            myUnivers.AddMarket(TradeFlatteningData.longMarket, "longTest");
            myUnivers.AddMarket(TradeFlatteningData.mediumMarket, "mediumMarket");
            myUnivers.AddMarket(TradeFlatteningData.shortMarket, "shortMarket");

            var strat = new StrategyExecuter(MarketSide.Bull, true);

            var Backtest = new Backtest(myUnivers, strat);

            Assert.True(Backtest.ParseResults(TimeSpan.FromDays(1)).Count > 0);
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
            public List<DatedResult> ParseResults(TimeSpan time)
            {
                return _results.SelectMany(x => x.Results.ToList()).ToList();

                var orderedResults = _results.OrderBy(x => x.Results.Last().Date).ToList();
                var iters = (orderedResults.Last().Results.Last().Date.Ticks - orderedResults.First().Results.First().Date.Ticks) / time.Ticks;
                for (long i = 0; i < iters; i += time.Ticks)
                {
                    
                }
            }
        }

 
    }
}
