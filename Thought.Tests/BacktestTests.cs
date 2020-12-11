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
        private void CanGenerateBackTest() {
            var myUnivers = new Universe(new IRuleSet[1]{new DummyEntries(5,40)});
            myUnivers.AddMarket(TradeFlatteningData.longMarket, "longTest");
            myUnivers.AddMarket(TradeFlatteningData.mediumMarket, "mediumMarket");
            myUnivers.AddMarket(TradeFlatteningData.shortMarket, "shortMarket");

            var strat = new StrategyExecuter(MarketSide.Bull, true);

            var Backtest = new Backtest(myUnivers, strat);


            Assert.True(true);
        }


        [Fact]
        private void GeneratesResults() {
            var myUnivers = new Universe(new IRuleSet[1] { new DummyEntries(5, 40) });
            myUnivers.AddMarket(TradeFlatteningData.longMarket, "longTest");
            myUnivers.AddMarket(TradeFlatteningData.mediumMarket, "mediumMarket");
            myUnivers.AddMarket(TradeFlatteningData.shortMarket, "shortMarket");

            var strat = new StrategyExecuter(MarketSide.Bull, true);

            var Backtest = new Backtest(myUnivers, strat);

            Assert.True(Backtest.GetResults().Count >0 );
        }

        public class Backtest
        {
            public Universe Markets { get; set; }
            private List<DatedResult> _results { get; set; }



            public object Results { get; set; }

            public Backtest(Universe markets, StrategyExecuter exec) {

                foreach (var element in markets.Elements) {
                    var trades = exec.Execute(element);
                    foreach (var trade in trades) {
                        //_results.Add(dated);
                    }
                }    
                
            }
            public List<DatedResult> GetResults() {
                return null;
            }
        }

 
    }
}
