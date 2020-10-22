using Logic.Metrics;
using Logic.Rules.Entry;
using Logic.Utils;
using System;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTests
    {
        ITest[] myTests { get; set; }

        public FixedBarExitTests()
        {
            var market = MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);
            var strat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[]
                {
                    new ATRContraction(),
                }, market);

            myTests = TestFactory.GenerateFixedBarExitTest(10, 100, strat, market);
        }


        [Fact]
        public void Test1()
        {

        }
    }
}
