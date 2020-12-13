using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using DataStructures.StatsTools;

namespace Logic.StrategyRunners
{
    public class StrategyOptions
    {
        public double ExpectancyCutOff { get; set; } = -1;
        public double WinPercentCutOff { get; set; } = -1;
        public double SpreadCutOff { get; set; } = -1;
        public CashPeriods[] NoTradePeriods { get; set; }

        public bool GoodToEnter(TradeStatistics stats, BidAskData data) {
            var boolOne = ExpectancyCutOff == -1 || stats.MedianExpectancy > ExpectancyCutOff; 
            var boolTwo = WinPercentCutOff == -1 ||  stats.WinPercent > WinPercentCutOff; 
            var boolThree = SpreadCutOff == -1 || data.Open.Ask - data.Open.Bid <= SpreadCutOff ;
            var boolFour = NoTradePeriods == null || NoTradePeriods.All(x => WithinTradeablePeriod(new DateBoundary(data.Close.Time), x));
            return boolOne && boolTwo && boolThree && boolFour;
        }
        public bool GoodToEnter(double expectncy, double winPercent, int spread, DateTime time) {
            var boolOne = ExpectancyCutOff == -1 || expectncy > ExpectancyCutOff;
            var boolTwo = WinPercentCutOff == -1 || winPercent > WinPercentCutOff;
            var boolThree = SpreadCutOff == -1 || SpreadCutOff > spread;
            var boolFour = NoTradePeriods == null || NoTradePeriods.All(x => WithinTradeablePeriod(new DateBoundary(time), x));
            return boolOne && boolTwo && boolThree && boolFour;
        }


        private bool WithinTradeablePeriod(DateBoundary time, CashPeriods period) {
            if (!DayIsWithinRange(period.StartCutoff.DayStart, time.DayStart, period.EndCutoff.DayStart)) return true;
            if (time.HourStart < period.StartCutoff.HourStart && period.StartCutoff.DayStart == time.DayStart){ return true;}
            else if (time.HourStart > period.EndCutoff.HourStart && period.EndCutoff.DayStart == time.DayStart) return true;
            if (time.MinuteStart < period.StartCutoff.MinuteStart && period.StartCutoff.DayStart == time.DayStart) return true;
            else if (time.MinuteStart > period.EndCutoff.MinuteStart && period.EndCutoff.DayStart == time.DayStart) return true;
            return false;
        }

        private bool DayIsWithinRange(DayOfWeek start, DayOfWeek myDay, DayOfWeek end) {
            List<DayOfWeek> myDays = new List<DayOfWeek>() {DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,DayOfWeek.Thursday, DayOfWeek.Friday,DayOfWeek.Saturday};
            var currentDay = myDays.IndexOf(start);
            do {
                if (CheckDay(myDay, myDays, ref currentDay)) return true;
            } while (currentDay != myDays.IndexOf(end)+1);
                
            return false;
        }



        private static bool CheckDay(DayOfWeek myDay, List<DayOfWeek> myDays, ref int currentDay) {
            if (currentDay == myDays.IndexOf(myDay)) return true;
            currentDay++;
            if (currentDay >= myDays.Count) currentDay = 0;
            return false;
        }

        public bool ShouldExit(TradeStateGenerator previousState, BidAskData data){
            if(!NoTradePeriods?.All(x => WithinTradeablePeriod(new DateBoundary(data.Close.Time), x)) ?? false) 
                //investedState.Returns.Add((data.Open.Bid - previousState.EntryPrice) / previousState.EntryPrice);           
                return false;
            return false;
        }
    }


    public class CashPeriods
    {
        public DateBoundary StartCutoff { get; set; }
        public DateBoundary EndCutoff { get; set; }
    }

    public class DateBoundary
    {
        public DayOfWeek DayStart { get; set; }
        public int HourStart { get; set; }
        public int MinuteStart { get; set; }

        public DateBoundary(DateTime time)
        {
            DayStart = time.DayOfWeek;
            HourStart = time.Hour;
            MinuteStart = time.Minute;
        }
        public DateBoundary() { }
    }

}
