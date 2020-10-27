using Logic;
using Logic.Utils;
using Logic.Utils.Calculations;
using PriceSeriesCore;
using PriceSeriesCore.FinancialSeries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Daedalus.Models.ModelSingleton;

namespace UtilConsole
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        private static void ManualRunthroughTests()
        {
            var asx_200_5_min = MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);
            var bidASkData = asx_200_5_min.RawData.ToList();
            var collatedData = asx_200_5_min.CostanzaData.ToList();
            var trueRangePC = AverageTrueRange.CalculateATRPC(collatedData);
            var twentyMa = MovingAverage.ExponentialMovingAverage(collatedData.Select(x => x.Close).ToList(), 20);
            var fifty = MovingAverage.SimpleMovingAverage(collatedData.Select(x => x.Close).ToList(), 50);
            var fissy = MovingAverage.SimpleMovingAverage(collatedData.Select(x => x.Close).ToList(), 50);
            var tenMA = MovingAverage.SimpleMovingAverage(collatedData.Select(x => x.Close).ToList(), 10);
            var SixMA = MovingAverage.ExponentialMovingAverage(collatedData.Select(x => x.Close).ToList(), 6);
            var atr = AverageTrueRange.Calculate(collatedData, 20);


            var contracts = 30;

            var startCpaital = 50000.0;
            var count = 0;
            StringBuilder tbo = new StringBuilder();

            List<IndexedDoubles> indexedAtrPcs = new List<IndexedDoubles>();
            for (int i = 0; i < trueRangePC.Count; i++) indexedAtrPcs.Add(new IndexedDoubles() {index = i, value = trueRangePC[i]});

            List<TradeResult> results = new List<TradeResult>();

            for (int i = 10; i < bidASkData.Count; i++)
            {
                var sixtoTen = Math.Abs(SixMA[i] - tenMA[i]);

                if (sixtoTen < atr[i] * 0.5
                )
                {
                    count++;
                    if (count > 9 && twentyMa[i] > fissy[i] && trueRangePC[i] == 0.0)
                    {
                        i++;

                        var t = new TradeResult();

                        t.startCapital = startCpaital;

                        t.longshort = Positioning.Long;
                        t.Entry = bidASkData[i].Open_Ask;
                        var myStop = t.Entry - 2 * atr[i];



                        while (myStop < bidASkData[i].Low_Bid)
                        {
                            var currentClose = bidASkData[i].Close_Bid;
                            var currentLow = bidASkData[i].Low_Bid;
                            List<Session> myREcentSessions = ListTools.GetNewListByIndex(collatedData, i - 5, i - 1);
                            var positionREcent = ListTools.GetPositionRange(myREcentSessions, myREcentSessions.Last().Close);
                            if (currentClose > t.Entry && myStop < currentLow - 5) myStop = currentLow - 5;
                            t.prices.Add(currentClose);
                            t.pricestops.Add(myStop);
                            i++;
                            if (i == bidASkData.Count) break;
                        }

                        if (i == bidASkData.Count) break;
                        if (bidASkData[i].Open_Bid < myStop) t.Exit = bidASkData[i].Open_Bid;
                        else t.Exit = myStop;



                        startCpaital += t.final * contracts;

                        results.Add(t);

                        tbo.AppendLine($"{startCpaital:0.0}");
                    }
                }
                else count = 0;

            }

            File.WriteAllText(@"C:\Temp\myREsultsYY.csv", tbo.ToString());

        }

        public class TradeResult
        {
            public Positioning longshort { get; set; }
            public double startCapital { get; set; }
            public double final => longshort.Equals(Positioning.Long) ? Exit - Entry : Entry - Exit;
            public double Entry { get; set; }
            public double Exit { get; set; }
            public List<double> prices { get; set; } = new List<double>();
            public List<double> pricestops { get; set; } = new List<double>();
        }
    }
}
