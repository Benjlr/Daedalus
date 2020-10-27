using Logic;
using Logic.Rules;
using Logic.Rules.Entry;
using Logic.Utils;
using Logic.Utils.Calculations;
using PriceSeriesCore.FinancialSeries;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daedalus.Utils;
using Logic.Rules.Exit;

namespace Daedalus.Models
{
    public class ModelSingleton
    {

        public static ModelSingleton Instance => _instance ?? (_instance = new ModelSingleton());
        private static ModelSingleton _instance { get; set; }
        public Market Mymarket { get; set; }
        public Strategy MyStrategy { get; set; }


        private ModelSingleton()
        {
            Mymarket =MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);
            
            MyStrategy = StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                //new MAViolation(), 
                new ThreeLowerLows(),
                //new ThreeHigherHighs(), 

                //new BullishMATag(),
                //new PriceContractionFromLow(),
                //new KeltnerOverSold(),
                //new KeltnerOverBought(),
                
                //new Sigma(), 
                //new PivotExit(), 

                //new TrendDay(),

                new ATRContraction(),
                //new ATRExpansion(),

                //new PriceContraction(),
                //new InvestorBotEntry(),

                //new BearishMATage(), 

                //new FiftyFiftyEntry(), 
                //new FiftyFiftyExit(), 

                //new CrossoverTag(),

            }, Mymarket);

            //var ListofSess = Mymarket.CostanzaData.ToList();
            //var hourly = SessionCollate.CollateToHourly(ListofSess);
            //var six = MovingAverage.ExponentialMovingAverage(ListofSess.Select(x => x.Close).ToList(), 6);
            //var ten = MovingAverage.SimpleMovingAverage(ListofSess.Select(x => x.Close).ToList(), 10);
            //var twenty = MovingAverage.ExponentialMovingAverage(ListofSess.Select(x => x.Close).ToList(), 20);
            //var fifty = MovingAverage.SimpleMovingAverage(ListofSess.Select(x => x.Close).ToList(), 50);
            //var twohun = MovingAverage.ExponentialMovingAverage(ListofSess.Select(x => x.Close).ToList(), 200);
            //var trueRange = AverageTrueRange.Calculate(ListofSess, 1);
            //var trueRangePC = AverageTrueRange.CalculateATRPC(ListofSess);
            //var nrwr = NRWRBars.Calculate(ListofSess);

            //var sixTenRnage = GetRangePositions(MovingAverage.GetRMSE(new List<List<double>>() { six, ten }), 50);
            //var sixTenTwentyRnage = GetRangePositions(MovingAverage.GetRMSE(new List<List<double>>() { six, ten, twenty }), 50);

            //StringBuilder text = new StringBuilder();
            //StringBuilder SignalData = new StringBuilder();
            //for (int i = 200; i < ListofSess.Count; i++)
            //{
            //    ListTools.AppendBar(hourly[ListTools.ReturnHourlyIndex(hourly, ListofSess[i])], text);
            //    ListTools.AppendBar(ListofSess[i], text);

            //    text.Append($"{six[i]:0.00},{ten[i]:0.00},{twenty[i]:0.00},{fifty[i]:0.00},{twohun[i]:0.00},{sixTenRnage[i]},{sixTenTwentyRnage[i]},{nrwr[i]},{trueRange[i]},{trueRangePC[i]},");

            //    if (trueRangePC[i] == 0.0)
            //    {
            //        List<double> mylist = ListTools.GetNewList(trueRangePC, ListTools.GetFirstOfPeriod(trueRangePC, i), ListTools.GetLastOfPeriod(trueRangePC, i));
            //        List<Session> mysessions = ListTools.GetNewList(ListofSess, ListTools.GetFirstOfPeriod(trueRangePC, i) - 20, ListTools.GetFirstOfPeriod(trueRangePC, i));
            //        var movePC = (mylist.Max()) / (mylist.Min());
            //        var position = ListTools.GetPositionRange(mysessions, mysessions.Last().Close);

            //        text.Append($"{movePC:0.00%},{position:0.00%}");
            //    }

            //    text.AppendLine();
            //}

            //List<IndexedDoubles> indexedAtrPcs = new List<IndexedDoubles>();
            //for (int i = 0; i < trueRangePC.Count; i++) indexedAtrPcs.Add(new IndexedDoubles() { index = i, value = trueRangePC[i] });

            //for (int i = 200; i < indexedAtrPcs.Count; i++)
            //{
            //    if (indexedAtrPcs[i].value == 0.0)
            //    {
            //        var prevIndex = indexedAtrPcs.LastOrDefault(x => x.value == 0.0 && x.index < i).index;
            //        var firstIndex = i;
            //        var lastIndex = ListTools.GetLastOfPeriod(indexedAtrPcs.Select(x => x.value).ToList(), i);
            //        var range = lastIndex - firstIndex + 1;

            //        List<double> mylist = ListTools.GetNewList(trueRangePC, firstIndex, lastIndex);
            //        List<Session> mysessionsfirst = ListTools.GetNewList(ListofSess, prevIndex, firstIndex);
            //        List<Session> mysessionsduring = ListTools.GetNewList(ListofSess, firstIndex, lastIndex);

            //        var movePC = ListTools.GetPositionRange(mysessionsduring, ListofSess[lastIndex].Close);
            //        var movePCMag = mysessionsduring.Max(x => x.High) - mysessionsduring.Min(x => x.Low);
            //        var position = ListTools.GetPositionRange(mysessionsfirst, mysessionsfirst.Last().Close);
            //        var FiveBarHigh = ListTools.GetNewList(ListofSess, lastIndex, lastIndex + 5).Max(x => x.High) - ListofSess[i].Close;
            //        var FiveBarLow = ListofSess[i].Close - ListTools.GetNewList(ListofSess, lastIndex, lastIndex + 5).Min(x => x.Low);
            //        var TenBarHigh = ListTools.GetNewList(ListofSess, lastIndex, lastIndex + 10).Max(x => x.High) - ListofSess[i].Close;
            //        var TenBarLow = ListofSess[i].Close - ListTools.GetNewList(ListofSess, lastIndex, lastIndex + 10).Min(x => x.Low);
            //        var TwentyBarHigh = ListTools.GetNewList(ListofSess, lastIndex, lastIndex + 20).Max(x => x.High) - ListofSess[i].Close;
            //        var TwentyBarLow = ListofSess[i].Close - ListTools.GetNewList(ListofSess, lastIndex, lastIndex + 20).Min(x => x.Low);
            //        var fortyBarHigh = ListTools.GetNewList(ListofSess, lastIndex, lastIndex + 40).Max(x => x.High) - ListofSess[i].Close;
            //        var fortyBarLow = ListofSess[i].Close - ListTools.GetNewList(ListofSess, lastIndex, lastIndex + 40).Min(x => x.Low);
            //        var spread = Mymarket.RawData[i].Close_Ask - Mymarket.RawData[i].Close_Bid;

            //        var lastSixTen = ListTools.GetNewList(sixTenRnage, i - 8, i).All(x => x < 0.1);
            //        var lastSixTenTwen = ListTools.GetNewList(sixTenTwentyRnage, i - 8, i).All(x => x < 0.1);

            //        SignalData.AppendLine($"{spread},{firstIndex},{lastIndex},{range},{movePC:0.###%},{movePCMag:0.0},{position:0.###%},{lastSixTen},{lastSixTenTwen}," +
            //                              $"{FiveBarHigh:0.0},{FiveBarLow:0.0},{TenBarHigh:0.0},{TenBarLow:0.0},{TwentyBarHigh:0.0},{TwentyBarLow:0.0},{fortyBarHigh:0.0},{fortyBarLow:0.0}");

            //        i = lastIndex;
            //    }
            //}

            //File.WriteAllText(@"C:\Temp\stats.csv", text.ToString());
            //File.WriteAllText(@"C:\Temp\statsIndividual5.csv", SignalData.ToString());
        }

        private List<double> GetRangePositions(List<double> range, int lookback)
        {
            var retval = new List<double>();
            for (int i = 0; i < range.Count; i++)
            {
                if(i < lookback) retval.Add(0.5);
                else
                {
                    var range2 = ListTools.GetNewListByIndex(range, i - lookback, i);
                    retval.Add( ListTools.GetPositionRange(range2, range2.Last()));
                }
            }

            return retval;
        }


        public class IndexedDoubles
        {
            public int index { get; set; }
            public double value { get; set; }
        }


    }
}
