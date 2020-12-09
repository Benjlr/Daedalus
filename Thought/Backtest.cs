using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace Thought
{
    public class Backtest
    {
        public Universe Markets { get; set; }
        private List<DatedResult> Results { get; set; }

        public Backtest(Universe markets) {

        }


        private void GetResults() {
            
        }


        private void CollateTradesAcrossMarkets() {

            foreach (var market in Markets.Elements) {
                market.
            }

                if (returnsToAdd[i] != 0) {
                    var date = dataToReference[i].CloseDate;
                    var relevantDatumItem = dataDatum.OrderBy(x => Math.Abs(x.CloseDate.Ticks - date.Ticks)).First();
                    for (int j = 0; j < dataDatum.Length; j++)
                        if (dataDatum[j].CloseDate == relevantDatumItem.CloseDate)
                            returnsDatum[j] += returnsToAdd[i];
                }
        }


        private void PrintTradesToReturnTimeLine(List<Trade> test, double[] market) {
            foreach (var t in test)
                for (int j = 0; j < t.Results.Length; j++)
                    market[t.MarketStart + j] += t.Results[j];
        }


    }

    public struct DatedResult
    {
        public DateTime Date { get; set; }
        public double Result { get; set; }

        public DatedResult(DateTime date, double result) {
            Date = date;
            Result = result;
        }
    }
}
