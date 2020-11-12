using Logic.Analysis.StrategyRunners;
using Logic.Tests.StrategyRunnerData;
using Logic.Utils;
using System.Collections.Generic;
using Xunit;

namespace Logic.Tests
{
    public class StrategyStateTests
    {
        StrategyState.StrategyStateFactory _factory = new StrategyState.StrategyStateFactory(new StrategyOptions());

        [Fact]
        private void ShouldInvestLong() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            Assert.True(newState.InvestedState.Invested);
            Assert.Equal(0,newState.InvestedState.Return);
        }

        [Fact]
        private void ShouldExitLongStop() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data4, new DrillDownStats(new List<double>()), true);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(newState.Return, -0.005 );
        }

        [Fact]
        private void ShouldExitLongTarget() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data3, new DrillDownStats(new List<double>()), true);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(newState.Return,  0.0049999999999998865 );
        }

        [Fact]
        private void ShouldExitLongOpenBelowStop() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data7, new DrillDownStats(new List<double>()), true);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(newState.Return,  -0.02 );
        }

        [Fact]
        private void ShouldExitLongOpenAboveTarget() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data6, new DrillDownStats(new List<double>()), true);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(newState.Return, 0.01);
        }

        [Fact]
        private void ShouldNotInvestLongIfUnderExpectancy()
        {
            StrategyState.StrategyStateFactory myFactory = 
                new StrategyState.StrategyStateFactory(new StrategyOptions()
                {
                    ExpectancyCutOff = 1
                });

            var testData = new TestData();
            var newState = myFactory.BuildNextState(testData.data, new DrillDownStats(new List<double>(){-0.5,0.2,0.3,-3}), true);
            newState = myFactory.BuildNextState(testData.data, new DrillDownStats(new List<double>(){-0.5,0.2,0.3,-3}), false);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(0, newState.InvestedState.Return);
        }

        [Fact]
        private void ShouldInvestShort() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), true);
            Assert.True(newState.InvestedState.Invested);
            Assert.Equal(-0.002, newState.InvestedState.Return);
        }

        [Fact]
        private void ShouldExitShort() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data3, new DrillDownStats(new List<double>()), true);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(newState.Return, -0.005);
        }

        [Fact]
        private void ShouldRemainInCash() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), false);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), false);
            newState = _factory.BuildNextState(testData.data7, new DrillDownStats(new List<double>()), false);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(newState.Return, 0);

        }

        [Fact]
        private void ShouldRemainInvested() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), false);
            newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), false);
            Assert.True(newState.InvestedState.Invested);
            Assert.Equal(newState.Return, 0);
        }


    }
}
