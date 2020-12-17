using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriceAlgorithms
{
    public class SessionCollate
    {
        public static List<BidAskData> CollateToDaily(List<BidAskData> input) {
            List<BidAskData> returnValue = new List<BidAskData>();
            for (int i = 0; i < input.Count; i++) {
                if (input[i].Open.TicksToTime.TimeOfDay == new TimeSpan(10, 0, 0))
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(i, 72)));
            }

            return returnValue;
        }

        public static List<BidAskData> CollateTo24HrDaily(List<BidAskData> input) {
            List<BidAskData> returnValue = new List<BidAskData>();
            DayOfWeek day = input[0].Open.TicksToTime.DayOfWeek;
            int start = 0;
            for (int i = 0; i < input.Count; i++) {
                if (input[i].Open.TicksToTime.DayOfWeek != day) {
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, i - start)));
                    day = input[i].Open.TicksToTime.DayOfWeek;
                    start = i;
                }

            }
            if(input.Count > start) returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, input.Count - start)));
            return returnValue;
        }

        public static List<BidAskData> CollateToHourly(List<BidAskData> input) {
            List<BidAskData> returnValue = new List<BidAskData>();
            int day = input[0].Open.TicksToTime.Hour;
            int start = 0;
            for (int i = 0; i < input.Count; i++) {
                if (input[i].Open.TicksToTime.Hour != day) {
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, i - start)));
                    day = input[i].Open.TicksToTime.Hour;
                    start = i;
                }
            }
            if (input.Count > start) returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, input.Count - start)));

            return returnValue;
        }

        public static List<BidAskData> CollateToHalfHourly(List<BidAskData> input) {
            List<BidAskData> returnValue = new List<BidAskData>();
            int start = 0;
            for (int i = 1; i < input.Count; i++) {
                var lastmin = input[i].Open.TicksToTime.Minute;

                if (lastmin == 30 || lastmin == 0) {
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, i - start)));
                    start = i;
                }

            }

            return returnValue;
        }

        //public static List<Session> CollateToTradingWeek(List<Session> input)
        //{
        //    var consignmentsByWeek = from inp in input
        //        group inp by inp.Close.Time.AddDays(-(int)inp.Close.Time.DayOfWeek);


        //    var yearWeekGroups = input.GroupBy(d => new { d.Close.Time.Year, WeekNum = Calendar.ReadOnly().GetWeekOfYear(d.Close.Time, CalendarWeekRule.FirstFullWeek,DayOfWeek.Sunday) });

        //    var grouped = input.GroupBy(x=>x.Close.Time.AddDays(-(int)x.Close.Time.DayOfWeek)).ToList();
        //    var retVal = new List<Session>();

        //    foreach (var t in grouped)
        //    {
        //        var high = t.Max(x => x.High);
        //        var low = t.Min(x => x.Low);
        //        retVal.Add(new Session(t.First().Open.Time, 
        //                                t.First(x => x.Low == low).Low.Time, 
        //                                t.First(x=>x.High == high).High.Time, 
        //                                t.Last().Close.Time, 
        //                                t.Sum(x=>x.Volume), 
        //                                t.First().Open,
        //                                high,
        //                                low,
        //                                t.Last().Close));
        //    }



        //    return retVal;
        //}

        private static BidAskData BuildSingleSessionFromList(List<BidAskData> input) {
            var highBar = input.IndexOf(input.First(x => x.High.Ask.Equals(input.Max(y => y.High.Ask))));
            var lowBar = input.IndexOf(input.First(x => x.Low.Bid.Equals(input.Min(y => y.Low.Bid))));

            var open = input[0].Open;
            var high = input[highBar].High;
            var low = input[lowBar].Low;
            var close = input.Last().Close;


            return new BidAskData(open,high,low,close,
                 input.Sum(x => x.Volume));
        }
    }
}
