using DataStructures;
using Logic.StrategyRunners;
using RuleSets;
using TestUtils;
using Xunit;

namespace Logic.Tests
{
    public class StrategyStateTests
    {
        readonly StrategyState.StrategyStateFactory _longfactory = new StrategyState.StrategyStateFactory(new StrategyOptions()){LongShort = MarketSide.Bull, stop = 0.995, target = 1.005};
        readonly StrategyState.StrategyStateFactory _shortfactory = new StrategyState.StrategyStateFactory(new StrategyOptions()){ LongShort = MarketSide.Bear, stop = 1.005, target = 0.995 };

        [Fact]
        private void ShouldInvestLong() {
            var testData = new StratRunnerTestData();
            var newState = _longfactory.BuildNextState(testData.data,  true, false);
            //Assert.True(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(0,newState.InvestedState.TradeState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldExitLongStop() {
            var testData = new StratRunnerTestData();
            var newState = _longfactory.BuildNextState(testData.data, true, false);
            newState = _longfactory.BuildNextState(testData.data2, true, false);
            newState = _longfactory.BuildNextState(testData.data4, false, true);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(-0.005, newState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldExitLongTarget() {
            var testData = new StratRunnerTestData();
            var newState = _longfactory.BuildNextState(testData.data, true, true);
            newState = _longfactory.BuildNextState(testData.data2, true, true);
            newState = _longfactory.BuildNextState(testData.data3, true, true);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(0.0049999999999998865 ,  newState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldExitLongOpenBelowStop() {
            var testData = new StratRunnerTestData();
            var newState = _longfactory.BuildNextState(testData.data, true, true);
            newState = _longfactory.BuildNextState(testData.data2, false, false);
            newState = _longfactory.BuildNextState(testData.data7, false, false);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(newState.Return,  -0.02 );
            Assert.True(false);
        }

        [Fact]
        private void ShouldExitLongOpenAboveTarget() {
            var testData = new StratRunnerTestData();
            var newState = _longfactory.BuildNextState(testData.data, true, true);
            newState = _longfactory.BuildNextState(testData.data2, false, false);
            newState = _longfactory.BuildNextState(testData.data6, false, false);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(0.01, newState.Return);
            Assert.True(false);
        }

        [Fact(Skip = "build optimiser first")]
        private void ShouldNotInvestLongIfUnderExpectancy()
        {
            StrategyState.StrategyStateFactory myFactory = 
                new StrategyState.StrategyStateFactory(new StrategyOptions()
                {
                    ExpectancyCutOff = 1
                });

            var testData = new StratRunnerTestData();
            var newState = myFactory.BuildNextState(testData.data, true, true);
            newState = myFactory.BuildNextState(testData.data, false, false);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(0, newState.InvestedState.TradeState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldInvestShort() {
            var testData = new StratRunnerTestData();
            var newState = _shortfactory.BuildNextState(testData.data, false, true);
            newState = _shortfactory.BuildNextState(testData.data2, false, false);
            //Assert.True(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(-0.002, newState.InvestedState.TradeState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldExitShortStop() {
            var testData = new StratRunnerTestData();
            var newState = _shortfactory.BuildNextState(testData.data, false, true);
            newState = _shortfactory.BuildNextState(testData.data2, false, false);
            newState = _shortfactory.BuildNextState(testData.data8, false, false);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(-0.0049999999999998865, newState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldExitShortTarget() {
            var testData = new StratRunnerTestData();
            var newState = _shortfactory.BuildNextState(testData.data, false, true);
            newState = _shortfactory.BuildNextState(testData.data2, false, false);
            newState = _shortfactory.BuildNextState(testData.data4, false, false);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(0.005, newState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldExitShortOpenAboveStop() {
            var testData = new StratRunnerTestData();
            var newState = _shortfactory.BuildNextState(testData.data, false, true);
            newState = _shortfactory.BuildNextState(testData.data2, false, false);
            newState = _shortfactory.BuildNextState(testData.data6, false, false);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(-0.01, newState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldExitShortOpenBelowTarget() {
            var testData = new StratRunnerTestData();
            var newState = _shortfactory.BuildNextState(testData.data, false, true);
            newState = _shortfactory.BuildNextState(testData.data2, false, false);
            newState = _shortfactory.BuildNextState(testData.data7, false, false);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(0.02, newState.Return);
            Assert.True(false);
        }

        [Fact]
        private void ShouldRemainInCash() {
            var testData = new StratRunnerTestData();
            var newState = _shortfactory.BuildNextState(testData.data, false, false);
            newState = _shortfactory.BuildNextState(testData.data2, false, false);
            newState = _shortfactory.BuildNextState(testData.data7, false, false);
            //Assert.False(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(0, newState.Return);

            Assert.True(false);
        }

        [Fact]
        private void ShouldRemainInvested() {
            var testData = new StratRunnerTestData();
            var newState = _longfactory.BuildNextState(testData.data, true, true);
            newState = _longfactory.BuildNextState(testData.data2, true, true);
            newState = _longfactory.BuildNextState(testData.data, true, true);
            //Assert.True(newState.InvestedState.TradeState.Invested);
            //Assert.Equal(0, newState.Return);
            Assert.True(false);
        }


    }
}
