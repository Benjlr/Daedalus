using Logic.Analysis.StrategyRunners;
using Logic.Strategies;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Logic.Tests
{
    public class StrategyRunnerTests
    {
        private string returnItemData => Directory.GetCurrentDirectory() + "\\StrategyRunnerData\\ReturnItems.txt";
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";
        private Market myMarket { get; set; }
        private Strategy myStrategy { get; set; }

        public StrategyRunnerTests() {
            myMarket = MarketBuilder.CreateMarket(marketData);
            myStrategy = StrategyBuilder.CreateStrategy(new IRuleSet[] {
                new DummyEntries(3, 100)
            }, myMarket);
        }


        [Fact]
        private void ShouldGenerateReturns() {
            var fste = new FixedStopTargetExitStrategyRunner(myMarket, myStrategy);
        fste.ExecuteRunner();
            var results = fste.Runner.Select(x => x.Market.Return).ToList();

            Assert.Equal(TestUtils.LoadDataSingleColumn(returnItemData), results);
               }
    }
}
