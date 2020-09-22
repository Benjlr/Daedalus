using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
