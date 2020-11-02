using PriceSeriesCore.FinancialSeries;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Utils
{
    public class ListTools
    {
       public static int GetFirstOfPeriod(List<double> inouts, int currentINdex)        {
            for (int i = currentINdex - 1; i >= 0; i--)
                if (inouts[i] != 0.0) return i + 1;
            return 0;
        }

       public static int GetLastOfPeriod(List<double> inouts, int currentINdex) {
            for (int i = currentINdex + 1; i < inouts.Count; i++)
                if (inouts[i] != 0.0) return i - 1;            

            return inouts.Count - 1;
        }

       public static List<T> GetNewListByIndex<T>(List<T> input, int start, int end) {
            var listofVals = new List<T>();
            if (end > input.Count - 1) end = input.Count - 1;
            for (int j = start; j <= end; j++)
                listofVals.Add(input[j]);

            return listofVals;
        }

        public static T[] GetNewArrayByIndex<T>(T[] input, int start, int end)
        {
            if (end > input.Length - 1) end = input.Length - 1;
            var listofVals = new T[end - start + 1];
            for (int j = start; j <= end; j++)
                listofVals[j - start] = input[j];

            return listofVals;
        }
        public static List<T> GetNewListByStartIndexAndCount<T>(List<T> input, int start, int count) {
            var listofVals = new List<T>();
            if (start + count > input.Count - 1) count = input.Count - start;
            for (int j = start; j < count+start; j++)
                listofVals.Add(input[j]);

            return listofVals;
        }
        public static T[] GetNewArrayByStartIndexAndCount<T>(T[] input, int start, int count) {
            if (start + count > input.Length - 1) count = input.Length-start;
            var listofVals = new T[count];
            for (int j = start; j < count + start; j++)
                listofVals[j - start] = input[j];

            return listofVals;
        }
        public static List<T> GetNewListByEndIndexAndCount<T>(List<T> input, int end, int priorCount)
        {
            var listofVals = new List<T>();
            var start = end - priorCount+1;
            if (start < 0) start = 0;
            for (int j = start; j <= end; j++)
                listofVals.Add(input[j]);

            return listofVals;
        }
        public static T[] GetNewArrayByEndIndexAndCount<T>(T[] input, int end, int priorCount)
        {
            var start = end - priorCount+1;
            if (start < 0) start = 0;
            var listofVals = new T[end-start+1];
            for (int j = start; j <= end; j++)
                listofVals[j - start] = input[j];

            return listofVals;
        }

        public static int GetIndexOfLastNonZero(double[] array, int start)
        {
            for (int i = start; i >= 0; i--)
                if (array[i] != 0) return i;

            return 0;

        }

        public static void AppendBar(Session bar, StringBuilder text)
        {
            text.Append($"{bar.CloseDate:g}{bar.Open:0.00},{bar.High:0.00},{bar.Low:0.00},{bar.Close:0.00},");
        }

       public static int ReturnHourlyIndex(List<Session> Hourly, Session FiveMinute)
        {
            return Hourly.IndexOf(Hourly.First(x => x.CloseDate.Hour == FiveMinute.OpenDate.AddHours(-1).Hour));
        }

       public static double GetPositionRange(List<Session> myInput, double value) {
            var Min = myInput.Min(x => x.Low);
            var Max = myInput.Max(x => x.High);
            return (value - Min) / (Max - Min);
        }

       public static double GetPositionRange(List<double> myInput, double value) {
           var Min = myInput.Min();
           var Max = myInput.Max();
           return (value - Min) / (Max - Min);
       }
    }
}
