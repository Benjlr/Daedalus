using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructures;
using Logic.Metrics;
using Logic.Metrics.EntryTests;
using RuleSets;
using RuleSets.Entry;
using TestUtils;
using Xunit;

namespace Logic.Tests
{
    public class TestFactoryTests
    {
        private Market _market { get; }
        private Strategy _strat{ get; }
        public TestFactoryTests() {
            _market = Market.MarketBuilder.CreateMarket(FSTETestsBars.DataLong);
            _strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(2, FSTETestsBars.DataLong.Length),
            }, _market);
        }

        [Fact]
        private void ShouldGenerateOneFixedStopTestLong() {
            var longSide = TestFactory.GenerateFixedStopTargetExitTest
                (_strat, _market, new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0, 0, MarketSide.Bull));

            Assert.Single(longSide);
        }

        [Fact]
        private void ShouldGenerateCorrectStopTargetsLong() {
            var longSide = TestFactory.GenerateFixedStopTargetExitTest
                (_strat, _market, new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0, 0, MarketSide.Bull));

            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide.First()).TargetDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide.First()).StopDistance);
        }

        [Fact]
        private void ShouldGenerateOneFixedStopTestShort() {
            var shorts = TestFactory.GenerateFixedStopTargetExitTest
                (_strat, _market, new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.15, 0, MarketSide.Bear));

            Assert.Single(shorts);
        }

        [Fact]
        private void ShouldGenerateCorrectStopTargetsShort() {
            var shorts = TestFactory.GenerateFixedStopTargetExitTest
                (_strat, _market, new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0, 0, MarketSide.Bear));

            Assert.Equal(0.85, ((FixedStopTargetExitTest)shorts.First()).TargetDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)shorts.First()).StopDistance);
        }

        [Fact]
        private void ShouldGenerateFourFixedStopTestLong() {
            var longSide = TestFactory.GenerateFixedStopTargetExitTest
                (_strat, _market, new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.15, 1, MarketSide.Bull));

            Assert.Equal(4,longSide.Count);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[0]).TargetDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[0]).StopDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[1]).TargetDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[1]).StopDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[2]).TargetDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[2]).StopDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[3]).TargetDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[3]).StopDistance);
        }

        [Fact]
        private void ShouldGenerateFourFixedStopTestShort() {
            var longSide = TestFactory.GenerateFixedStopTargetExitTest
                (_strat, _market, new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.15, 1, MarketSide.Bear));

            Assert.Equal(4, longSide.Count);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[0]).TargetDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[0]).StopDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[1]).TargetDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[1]).StopDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[2]).TargetDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[2]).StopDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[3]).TargetDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[3]).StopDistance);
        }

        [Fact]
        private void ShouldGenerateTwelveFixedStopTestLong() {
            var longSide = TestFactory.GenerateFixedStopTargetExitTest
                (_strat, _market, new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.6, 4, MarketSide.Bull));

            Assert.Equal(25, longSide.Count);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[0]).TargetDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[0]).StopDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[1]).TargetDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[1]).StopDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)longSide[2]).TargetDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[2]).StopDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)longSide[3]).TargetDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[3]).StopDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)longSide[4]).TargetDistance);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)longSide[4]).StopDistance);

            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[5]).TargetDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[5]).StopDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[6]).TargetDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[6]).StopDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)longSide[7]).TargetDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[7]).StopDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)longSide[8]).TargetDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[8]).StopDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)longSide[9]).TargetDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)longSide[9]).StopDistance);

            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[10]).TargetDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)longSide[10]).StopDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[11]).TargetDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)longSide[11]).StopDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)longSide[12]).TargetDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)longSide[12]).StopDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)longSide[13]).TargetDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)longSide[13]).StopDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)longSide[14]).TargetDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)longSide[14]).StopDistance);

            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[15]).TargetDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)longSide[15]).StopDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[16]).TargetDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)longSide[16]).StopDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)longSide[17]).TargetDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)longSide[17]).StopDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)longSide[18]).TargetDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)longSide[18]).StopDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)longSide[19]).TargetDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)longSide[19]).StopDistance);

            Assert.Equal(1.15, ((FixedStopTargetExitTest)longSide[20]).TargetDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)longSide[20]).StopDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)longSide[21]).TargetDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)longSide[21]).StopDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)longSide[22]).TargetDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)longSide[22]).StopDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)longSide[23]).TargetDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)longSide[23]).StopDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)longSide[24]).TargetDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)longSide[24]).StopDistance);
        }

        [Fact]
        private void ShouldGenerateTwelveFixedStopTestShort() {
            var shortSide = new  TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.6, 4));
            shortSide.Run()

            Assert.Equal(25, shortSide.Count);
            Assert.Equal(0.85, ((FixedStopTargetExitTest)shortSide[0]).TargetDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)shortSide[0]).StopDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)shortSide[1]).TargetDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)shortSide[1]).StopDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)shortSide[2]).TargetDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)shortSide[2]).StopDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)shortSide[3]).TargetDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)shortSide[3]).StopDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)shortSide[4]).TargetDistance);
            Assert.Equal(1.15, ((FixedStopTargetExitTest)shortSide[4]).StopDistance);

            Assert.Equal(0.85, ((FixedStopTargetExitTest)shortSide[5]).TargetDistance);
            Assert.Equal(1.3,((FixedStopTargetExitTest)shortSide[5]).StopDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)shortSide[6]).TargetDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)shortSide[6]).StopDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)shortSide[7]).TargetDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)shortSide[7]).StopDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)shortSide[8]).TargetDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)shortSide[8]).StopDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)shortSide[9]).TargetDistance);
            Assert.Equal(1.3, ((FixedStopTargetExitTest)shortSide[9]).StopDistance);

            Assert.Equal(0.85, ((FixedStopTargetExitTest)shortSide[10]).TargetDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)shortSide[10]).StopDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)shortSide[11]).TargetDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)shortSide[11]).StopDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)shortSide[12]).TargetDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)shortSide[12]).StopDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)shortSide[13]).TargetDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)shortSide[13]).StopDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)shortSide[14]).TargetDistance);
            Assert.Equal(1.45, ((FixedStopTargetExitTest)shortSide[14]).StopDistance);

            Assert.Equal(0.85, ((FixedStopTargetExitTest)shortSide[15]).TargetDistance);
            Assert.Equal(1.6,((FixedStopTargetExitTest)shortSide[15]).StopDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)shortSide[16]).TargetDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)shortSide[16]).StopDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)shortSide[17]).TargetDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)shortSide[17]).StopDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)shortSide[18]).TargetDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)shortSide[18]).StopDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)shortSide[19]).TargetDistance);
            Assert.Equal(1.6, ((FixedStopTargetExitTest)shortSide[19]).StopDistance);

            Assert.Equal(0.85, ((FixedStopTargetExitTest)shortSide[20]).TargetDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)shortSide[20]).StopDistance);
            Assert.Equal(0.7, ((FixedStopTargetExitTest)shortSide[21]).TargetDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)shortSide[21]).StopDistance);
            Assert.Equal(0.55, ((FixedStopTargetExitTest)shortSide[22]).TargetDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)shortSide[22]).StopDistance);
            Assert.Equal(0.4, ((FixedStopTargetExitTest)shortSide[23]).TargetDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)shortSide[23]).StopDistance);
            Assert.Equal(0.25, ((FixedStopTargetExitTest)shortSide[24]).TargetDistance);
            Assert.Equal(1.75, ((FixedStopTargetExitTest)shortSide[24]).StopDistance);
        }


    }
}
