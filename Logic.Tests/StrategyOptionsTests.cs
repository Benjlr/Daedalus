using Logic.Analysis.StrategyRunners;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Logic.Tests
{
    public class StrategyOptionsTests
    {
        [Fact]
        public void ShouldFallOutsideTradeWindow()
        {
            var stratOpt = new StrategyOptions();
            stratOpt.NoTradePeriods = new CashPeriods[]
            {
                new CashPeriods()
                {
                   StartCutoff = new DateBoundary()
                    {
                        DayStart = DayOfWeek.Friday,
                        HourStart =16,
                        MinuteStart = 35,
                    },
                   EndCutoff = new DateBoundary()
                   {
                       DayStart = DayOfWeek.Monday,
                       HourStart = 11,
                       MinuteStart = 05
                   }

                }
            };


            Assert.False(stratOpt.GoodToEnter(0,0,0, new DateTime(2020,11,7)));
            Assert.True(stratOpt.GoodToEnter(0,0,0, new DateTime(2020,11,6,16,36,0)));
        }
    }
}
