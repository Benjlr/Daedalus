using Logic.Analysis.StrategyRunners;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Logic.Utils;
using System.Reflection;
using RuleSets;

namespace Logic.Tests
{
    public class StrategyOptionsTests
    {
        [Fact]
        public void ShouldFallOutsideNoTradeWindow()
        {
            var stratOpt = new StrategyOptions();
            stratOpt.NoTradePeriods = new CashPeriods[]
            {
                new CashPeriods() {
                    StartCutoff = new DateBoundary() {DayStart = DayOfWeek.Friday, HourStart = 16, MinuteStart = 35,},
                    EndCutoff = new DateBoundary() {DayStart = DayOfWeek.Monday, HourStart = 11, MinuteStart = 05}
                }
            };

            DrillDownStats myStats = new DrillDownStats(new List<double>());
            MarketData market = new MarketData(new DateTime(2020, 11, 5, 16, 34, 0),0,0,0,0,0,0,0,0,0);
            MarketData marketTwo = new MarketData(new DateTime(2020, 11, 2, 11, 6, 0), 0,0,0,0,0,0,0,0,0);
            MarketData marketThree = new MarketData(new DateTime(2020, 11, 4, 12, 0, 0), 0,0,0,0,0,0,0,0,0);

            Assert.True(stratOpt.GoodToEnter(myStats, market));
            Assert.True(stratOpt.GoodToEnter(myStats, marketTwo));
            Assert.True(stratOpt.GoodToEnter(myStats, marketThree));
        }

        [Fact]
        public void ShouldFallInsideNoTradeWindow()
        {
            var stratOpt = new StrategyOptions();
            stratOpt.NoTradePeriods = new CashPeriods[] {
                new CashPeriods() {
                    StartCutoff = new DateBoundary() {
                        DayStart = DayOfWeek.Wednesday, HourStart = 12, MinuteStart = 45,
                    },
                    EndCutoff = new DateBoundary() {DayStart = DayOfWeek.Tuesday, HourStart = 03, MinuteStart = 4}
                }
            };

            DrillDownStats myStats = new DrillDownStats(new List<double>());
            MarketData market = new MarketData(new DateTime(2020, 11, 4, 12, 46, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0);
            MarketData marketTwo = new MarketData(new DateTime(2020, 11, 6, 17, 36, 5), 0, 0, 0, 0, 0, 0, 0, 0, 0);
            MarketData marketThree = new MarketData(new DateTime(2020, 11, 10, 3, 3, 1), 0, 0, 0, 0, 0, 0, 0, 0, 0);

            Assert.False(stratOpt.GoodToEnter(myStats, market));
            Assert.False(stratOpt.GoodToEnter(myStats, marketTwo));
            Assert.False(stratOpt.GoodToEnter(myStats, marketThree));
        }

        [Fact]
        public void ShouldFallOutsideNoTradeWindowSameDay()
        {
            var stratOpt = new StrategyOptions();
            stratOpt.NoTradePeriods = new CashPeriods[]
            {
                new CashPeriods() {
                    StartCutoff = new DateBoundary() {DayStart = DayOfWeek.Tuesday, HourStart = 16, MinuteStart = 35,},
                    EndCutoff = new DateBoundary() {DayStart = DayOfWeek.Tuesday, HourStart = 16, MinuteStart = 48}
                }
            };

            DrillDownStats myStats = new DrillDownStats(new List<double>());
            MarketData market = new MarketData(new DateTime(2020, 11, 2, 16, 36, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0);
            MarketData marketTwo = new MarketData(new DateTime(2020, 11, 9, 16, 40, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0);
            MarketData marketThree = new MarketData(new DateTime(2020, 11, 16, 16, 47, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0);

            Assert.True(stratOpt.GoodToEnter(myStats, market));
            Assert.True(stratOpt.GoodToEnter(myStats, marketTwo));
            Assert.True(stratOpt.GoodToEnter(myStats, marketThree));
        }

        [Fact]
        public void ShouldFallInsideNoTradeWindowSameDay()
        {
            var stratOpt = new StrategyOptions();
            stratOpt.NoTradePeriods = new CashPeriods[]
            {
                new CashPeriods() {
                    StartCutoff = new DateBoundary() {DayStart = DayOfWeek.Wednesday, HourStart = 16, MinuteStart = 35,},
                    EndCutoff = new DateBoundary() {DayStart = DayOfWeek.Wednesday, HourStart = 16, MinuteStart = 48}
                }
            };

            DrillDownStats myStats = new DrillDownStats(new List<double>());
            MarketData market = new MarketData(new DateTime(2020, 11, 4, 16, 36, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0);
            MarketData marketTwo = new MarketData(new DateTime(2020, 11, 11, 16, 40, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0);
            MarketData marketThree = new MarketData(new DateTime(2020, 11, 18, 16, 47, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0);

            Assert.False(stratOpt.GoodToEnter(myStats, market));
            Assert.False(stratOpt.GoodToEnter(myStats, marketTwo));
            Assert.False(stratOpt.GoodToEnter(myStats, marketThree));
        }

        [Fact]
        public void ShouldCutOffDueToExpectancy()
        {
            var stratOpt = new StrategyOptions(){ExpectancyCutOff = 1.2};

            Assert.False(stratOpt.GoodToEnter(0.5, 0, 0, new DateTime()));
            Assert.False(stratOpt.GoodToEnter(double.Epsilon, 0, 0, new DateTime()));
            Assert.False(stratOpt.GoodToEnter(1.199999, 0, 0, new DateTime()));
            Assert.False(stratOpt.GoodToEnter(0, 0, 0, new DateTime()));
        }

        [Fact]
        public void ShouldCutOffDueToWinPercent()
        {
            var stratOpt = new StrategyOptions() { WinPercentCutOff = 0.92 };

            Assert.False(stratOpt.GoodToEnter(0, 0.911111, 0, new DateTime()));
            Assert.False(stratOpt.GoodToEnter(0, double.Epsilon,  0, new DateTime()));
            Assert.False(stratOpt.GoodToEnter(0, 0, 0, new DateTime()));
        }

        [Fact]
        public void ShouldAllowExpectancy()
        {
            var stratOpt = new StrategyOptions() { ExpectancyCutOff = 2 };

            Assert.True(stratOpt.GoodToEnter(3, 0, 0, new DateTime()));
            Assert.True(stratOpt.GoodToEnter(double.MaxValue, 0, 0, new DateTime()));
            Assert.True(stratOpt.GoodToEnter(2.0001, 0, 0, new DateTime()));
        }

        [Fact]
        public void ShouldAllowWinPercent()
        {
            var stratOpt = new StrategyOptions() { WinPercentCutOff = 0.99 };

            Assert.True(stratOpt.GoodToEnter(0, 0.999,  0, new DateTime()));
            Assert.True(stratOpt.GoodToEnter(0, 1, 0,  new DateTime()));
            Assert.True(stratOpt.GoodToEnter(0, 0.991,  0, new DateTime()));
        }
        [Fact]
        public void ShouldCutOffDueSpread()
        {
            var stratOpt = new StrategyOptions() { SpreadCutOff = 2 };

            Assert.False(stratOpt.GoodToEnter(0, 0, 3, new DateTime()));
            Assert.False(stratOpt.GoodToEnter(0, 0, 5, new DateTime()));
            Assert.False(stratOpt.GoodToEnter(0, 0, 4, new DateTime()));
        }

        [Fact]
        public void ShouldAllowSpread()
        {
            var stratOpt = new StrategyOptions() { SpreadCutOff = 3 };

            Assert.True(stratOpt.GoodToEnter(0, 0, 0, new DateTime()));
            Assert.True(stratOpt.GoodToEnter(0, 0, 1, new DateTime()));
            Assert.True(stratOpt.GoodToEnter(0, 0, 2, new DateTime()));
        }

        [Fact]
        public void ShouldInitDateBoundary()
        {
            var dateBoundary = new DateBoundary(new DateTime(2020, 11, 11, 11, 11, 11, 11));
            var dateBoundary2 = new DateBoundary(new DateTime(2020, 11, 11, 23, 23, 23, 23));
            Assert.Equal(DayOfWeek.Wednesday, dateBoundary.DayStart);
            Assert.Equal(11, dateBoundary.HourStart);
            Assert.Equal(11, dateBoundary.MinuteStart);

            Assert.Equal(DayOfWeek.Wednesday, dateBoundary2.DayStart);
            Assert.Equal(23, dateBoundary2.HourStart);
            Assert.Equal(23, dateBoundary2.MinuteStart);
        }


    }
}
