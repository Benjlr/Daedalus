using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceSeries.FinancialSeries;

namespace Logic.Utils
{
    public class ListTools
    {
       public static int GetFirstOfPeriod(List<double> inouts, int currentINdex)
        {
            for (int i = currentINdex - 1; i >= 0; i--)
            {
                if (inouts[i] != 0.0) return i + 1;
            }

            return 0;
        }

       public static int GetLastOfPeriod(List<double> inouts, int currentINdex)
        {
            for (int i = currentINdex + 1; i < inouts.Count; i++)
            {
                if (inouts[i] != 0.0) return i - 1;
            }

            return inouts.Count - 1;
        }

       public static List<T> GetNewList<T>(List<T> input, int start, int end)
        {
            var listofVals = new List<T>();
            if (end > input.Count - 1) end = input.Count - 1;
            for (int j = start; j <= end; j++)
            {
                listofVals.Add(input[j]);
            }

            return listofVals;
        }
       public static T[] GetNewList<T>(T[] input, int start, int end)
       {
           var listofVals = new T[end-start+1];
           if (end > input.Length - 1) end = input.Length - 1;
           for (int j = start; j <= end; j++)
           {
               listofVals[j-start] = input[j];
           }

           return listofVals;
       }
        public static void AppendBar(Session bar, StringBuilder text)
        {
            text.Append($"{bar.CloseDate:g}{bar.Open:0.00},{bar.High:0.00},{bar.Low:0.00},{bar.Close:0.00},");
        }

       public static int ReturnHourlyIndex(List<Session> Hourly, Session FiveMinute)
        {
            return Hourly.IndexOf(Hourly.First(x => x.CloseDate.Hour == FiveMinute.OpenDate.AddHours(-1).Hour));
        }

       public static double GetPositionRange(List<Session> myInput, double value)
        {
            var Min = myInput.Min(x => x.Low);
            var Max = myInput.Max(x => x.High);
            return (value - Min) / (Max - Min);
        }

       public static double GetPositionRange(List<double> myInput, double value)
       {
           var Min = myInput.Min();
           var Max = myInput.Max();
           return (value - Min) / (Max - Min);
       }

       public static Dictionary<double, int> BinGenerator(int min, int max, int width)
       {
           if ((max - min) % width != 0) throw new Exception();

           var count = (max - min) / width;
           var myDict = new Dictionary<double, int>();

           for (int i = 0; i < count; i++) myDict.Add(min + width * i, 0);
           return myDict;
       }

       public static Dictionary<double, List<double>> CategoryGenerator(int min, int max, int width)
       {
           if ((max - min) % width != 0) throw new Exception();

           var count = (max - min) / width;
           var myDict = new Dictionary<double, List<double>>();

           for (int i = 0; i < count; i++) myDict.Add(min + width * i, new List<double>());
           return myDict;
       }
    }
}
