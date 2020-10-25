using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using PriceSeries.FinancialSeries;

namespace Logic.Utils.Calculations
{
    public class SessionCollate
    {
        public static List<Session> CollateToDaily(List<Session> input)
        {
            List<Session> returnValue = new List<Session>();
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].OpenDate.TimeOfDay == new TimeSpan(10, 0, 0))
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(i,72)));
            }

            return returnValue;
        }

        public static List<Session> CollateTo24HrDaily(List<Session> input)
        {
            List<Session> returnValue = new List<Session>();
            DayOfWeek day = input[0].OpenDate.DayOfWeek;
            int start = 0;
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].OpenDate.DayOfWeek != day)
                {
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, i-start)));
                    day = input[i].OpenDate.DayOfWeek;
                    start = i;
                }

            }

            return returnValue;
        }

        public static List<Session> CollateToHourly(List<Session> input)
        {
            List<Session> returnValue = new List<Session>();
            int day = input[0].OpenDate.Hour;
            int start = 0;
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].OpenDate.Hour != day)
                {
                    returnValue.Add(BuildSingleSessionFromList(input.GetRange(start, i - start)));
                    day = input[i].OpenDate.Hour;
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

        private static Session BuildSingleSessionFromList(List<Session> input)
        {
            var highBar = input.IndexOf(input.First(x=>x.High.Equals(input.Max(y => y.High))));
            var lowBar = input.IndexOf(input.First(x=>x.Low.Equals(input.Min(y => y.Low))));
            return new Session(
                od: input[0].OpenDate,
                hd: input[highBar].HighDate,
                ld: input[lowBar].LowDate,
                cd: input.Last().CloseDate,
                v: input.Sum(x=>x.Volume),
                o: input[0].Open,
                h: input[highBar].High,
                l: input[lowBar].Low,
                c: input.Last().Close);
        }
    }
}
