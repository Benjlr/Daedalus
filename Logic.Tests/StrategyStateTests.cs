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
        private void ShouldExitLong() {
            var testData = new TestData();
            var newState = _factory.BuildNextState(testData.data, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data2, new DrillDownStats(new List<double>()), true);
            newState = _factory.BuildNextState(testData.data4, new DrillDownStats(new List<double>()), true);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(newState.Returns, new List<double>() { -0.005 });

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
            newState = _factory.BuildNextState(testData.data4, new DrillDownStats(new List<double>()), true);
            Assert.False(newState.InvestedState.Invested);
            Assert.Equal(newState.Returns, new List<double>() { -0.005 });
        }

        [Fact]
        private void ShouldRemainInCash() {


            Assert.Equal(true, false);

        }

        [Fact]
        private void ShouldRemainInvested() {


            Assert.Equal(true, false);

        }


        [Fact]
        private void ShouldCalculateReturns() {


            Assert.Equal(true, false);

        }
    }
}
