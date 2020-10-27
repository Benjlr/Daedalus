using PriceSeriesCore.FinancialSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqStatistics;

namespace Logic.Utils.Calculations
{
    public class SigmaSpike
    {
        public static List<double> Calculate(List<Session> input, int period = 20)
        {
            var retval = new List<double>();

            var returnSeries = input.Select(x => x.ReturnSeries).ToList();

            if (returnSeries.Count - 1 < period)
            {
                input.ForEach(x => retval.Add(0));
                return retval;
            }

            for (var i = 0; i < period + 1; i++) retval.Add(0);
            for (var i = period + 1; i < returnSeries.Count; i++)
            {
                var temp = returnSeries.GetRange(i - (period + 1), period).StandardDeviationP();
                retval.Add(returnSeries[i] / temp);

                //ALTERNATE
                //double temp = LinqStatistics.EnumerableStats.StandardDeviation(Inp.GetRange(i - 21, 20).Select(x => x.ReturnSeries)) * Inp[i - 1].Close;
                //double Value = (Inp[i].ReturnSeries) / temp;
            }

            return retval;
        }
    }
}
