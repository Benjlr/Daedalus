using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriceAlgorithms
{
    public class SessionCollate
    {
        public static List<SessionData> CollateToDaily(List<SessionData> input) {
            List<SessionData> returnValue = new List<SessionData>();
            for (int i = 0; i < input.Count; i++) {
                if (input[i].OpenDate.TimeOfDay == new TimeSpan(10, 0, 0))
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(i, 72)));
            }

            return returnValue;
        }

        public static List<SessionData> CollateTo24HrDaily(List<SessionData> input) {
            List<SessionData> returnValue = new List<SessionData>();
            DayOfWeek day = input[0].OpenDate.DayOfWeek;
            int start = 0;
            for (int i = 0; i < input.Count; i++) {
                if (input[i].OpenDate.DayOfWeek != day) {
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, i - start)));
                    day = input[i].OpenDate.DayOfWeek;
                    start = i;
                }

            }

            return returnValue;
        }

        public static List<SessionData> CollateToHourly(List<SessionData> input) {
            List<SessionData> returnValue = new List<SessionData>();
            int day = input[0].OpenDate.Hour;
            int start = 0;
            for (int i = 0; i < input.Count; i++) {
                if (input[i].OpenDate.Hour != day) {
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, i - start)));
                    day = input[i].OpenDate.Hour;
                    start = i;
                }

            }

            return returnValue;
        }

        public static List<BidAskData> CollateToHourly(List<BidAskData> input) {
            List<BidAskData> returnValue = new List<BidAskData>();
            int day = input[0].Time.Hour;
            int start = 0;
            for (int i = 0; i < input.Count; i++) {
                if (input[i].Time.Hour != day) {
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, i - start)));
                    day = input[i].Time.Hour;
                    start = i;
                }

            }

            return returnValue;
        }

        public static List<BidAskData> CollateToHalfHourly(List<BidAskData> input) {
            List<BidAskData> returnValue = new List<BidAskData>();
            int start = 0;
            for (int i = 1; i < input.Count; i++) {
                var lastmin = input[i].Time.Minute;

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
        //        group inp by inp.CloseDate.AddDays(-(int)inp.CloseDate.DayOfWeek);


        //    var yearWeekGroups = input.GroupBy(d => new { d.CloseDate.Year, WeekNum = Calendar.ReadOnly().GetWeekOfYear(d.CloseDate, CalendarWeekRule.FirstFullWeek,DayOfWeek.Sunday) });

        //    var grouped = input.GroupBy(x=>x.CloseDate.AddDays(-(int)x.CloseDate.DayOfWeek)).ToList();
        //    var retVal = new List<Session>();

        //    foreach (var t in grouped)
        //    {
        //        var high = t.Max(x => x.High);
        //        var low = t.Min(x => x.Low);
        //        retVal.Add(new Session(t.First().OpenDate, 
        //                                t.First(x => x.Low == low).LowDate, 
        //                                t.First(x=>x.High == high).HighDate, 
        //                                t.Last().CloseDate, 
        //                                t.Sum(x=>x.Volume), 
        //                                t.First().Open,
        //                                high,
        //                                low,
        //                                t.Last().Close));
        //    }



        //    return retVal;
        //}

        private static SessionData BuildSingleSessionFromList(List<SessionData> input) {
            var highBar = input.IndexOf(input.First(x => x.High.Equals(input.Max(y => y.High))));
            var lowBar = input.IndexOf(input.First(x => x.Low.Equals(input.Min(y => y.Low))));
            return new SessionData(
                od: input[0].OpenDate,
                hd: input[highBar].HighDate,
                ld: input[lowBar].LowDate,
                cd: input.Last().CloseDate,
                v: input.Sum(x => x.Volume),
                o: input[0].Open,
                h: input[highBar].High,
                l: input[lowBar].Low,
                c: input.Last().Close);
        }

        private static BidAskData BuildSingleSessionFromList(List<BidAskData> input) {
            var highBar = input.IndexOf(input.First(x => x.High_Ask.Equals(input.Max(y => y.High_Ask))));
            var lowBar = input.IndexOf(input.First(x => x.Low_Bid.Equals(input.Min(y => y.Low_Bid))));
            return new BidAskData(
                time: input[0].Time,
                o_a: input[0].Open_Ask,
                o_b: input[0].Open_Bid,
                h_a: input[highBar].High_Ask,
                h_b: input[highBar].High_Bid,
                l_a: input[lowBar].Low_Ask,
                l_b: input[lowBar].Low_Bid,
                c_a: input.Last().Close_Ask,
                c_b: input.Last().Close_Bid,
                vol: input.Sum(x => x.volume));
        }
    }
}
