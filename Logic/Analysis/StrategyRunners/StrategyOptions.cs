using System;
using System.Linq;

namespace Logic.Analysis.StrategyRunners
{
    public class StrategyOptions
    {
        public double ExpectancyCutOff { get; set; }
        public double WinPercentCutOff { get; set; }
        public double SpreadCutOff { get; set; }
        public CashPeriods[] NoTradePeriods { get; set; }

        public bool GoodToEnter(double expectancy, double winPercent, double spread, DateTime date)
        {
            var boolOne = expectancy < ExpectancyCutOff || ExpectancyCutOff == 0; 
            var boolTwo = winPercent > WinPercentCutOff || WinPercentCutOff == 0; 
            var boolThree = SpreadCutOff > spread || SpreadCutOff == 0;
            var boolFour = NoTradePeriods.All(x => !CheckTradePeriod(new DateBoundary(date), x));
            return boolOne && boolTwo && boolThree;
        }
        
        

        private bool CheckTradePeriod(DateBoundary time, CashPeriods period)
        {
            var boolOne = time.DayStart > period.StartCutoff.DayStart && time.DayStart < period.EndCutoff.DayStart;
            var boolTwo = time.HourStart > period.StartCutoff.HourStart && time.HourStart < period.EndCutoff.HourStart;
            var boolThree = time.MinuteStart > period.StartCutoff.MinuteStart && time.MinuteStart < period.EndCutoff.MinuteStart;

            return boolOne && boolTwo && boolThree;
        }


        public bool ShouldExit()
        {
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
