using LinqStatistics;
using Logic;
using Logic.Metrics;
using Logic.Rules;
using Logic.Rules.Entry;
using Logic.Rules.Exit;
using Logic.Utils;
using Logic.Utils.Calculations;
using PriceSeries;
using PriceSeries.FinancialSeries;
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

            ManualFixedBarTestsDetiled();

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
                            List<Session> myREcentSessions = ListTools.GetNewList(collatedData, i - 5, i - 1);
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



        private static void ManualFixedBarTestsDetiled()
        {
            var start = 1;
            var end = 230;

            var sttock = Markets.Bitcoin;
            var myStocks = new List<string>();
            myStocks.Add(sttock);

            var allRetruns = ListTools.BinGenerator(-50, 50, 5);
            var myDrawdownBins = ListTools.BinGenerator(-50, 0, 5);
            var histogramReturns = new List<string>();
            var histogramDrawdowns = new List<string>();
            var categoriseDrawdown = ListTools.CategoryGenerator(-50, 50, 5);
            var myReturnBins = ListTools.BinGenerator(-50, 50, 5);

            double[][] generalResultsIteration = new double[end - start][];


            for (int z = 0; z < myStocks.Count; z++)
            {
                var asx_200_5_min = MarketBuilder.CreateMarket(myStocks[z]);

                var entryOne = new ATRContraction();
                var exitOne = new ThreeLowerLows();

                entryOne.Calc(asx_200_5_min.CostanzaData.ToList(), asx_200_5_min.RawData);

                Strategy myStrat = StrategyBuilder.CreateStrategy(new IRuleSet[] { entryOne, exitOne }, asx_200_5_min);
                var tests = TestFactory.GenerateFixedBarExitTest(start, end, myStrat, asx_200_5_min);
                double[][] SingleIter = new double[tests.Length][];

                for (int k = 0; k < tests.Length; k++)
                {


                    for (int i = 0; i < tests[k].FBELong.Length; i++)
                    {
                        if (tests[k].FBELong[i] != 0)
                        {
                            CategoriseItem(myReturnBins, tests[k].FBELong[i]);
                            CategoriseItem(categoriseDrawdown, tests[k].FBEDrawdownLong[i], tests[k].FBELong[i]);
                            CategoriseItem(allRetruns, tests[k].FBELong[i]);
                        }

                        if (tests[k].FBELong[i] > 0) CategoriseItem(myDrawdownBins, tests[k].FBEDrawdownLong[i]);
                    }

                    histogramReturns.Add(GenerateHistogramRow(myReturnBins));
                    histogramDrawdowns.Add(GenerateHistogramRow(myDrawdownBins));

                    SingleIter[k] = AddGeneralResultsArray(tests[k]);

                    ResetBin(myReturnBins);
                    ResetBin(myDrawdownBins);

                    Console.WriteLine($"{k}");
                }

                GenerateAverageResults(generalResultsIteration, SingleIter, end - start, z+1);

                ResetBin(myReturnBins);
                ResetBin(myDrawdownBins);

                Console.WriteLine($"{myStocks[z]}");
            }




            List<string> drawdownsByReturn = TransformCategorisedListsToHistograms(categoriseDrawdown);
            List<string> finalGeneralResults = new List<string>();

            var drawdownBindHeaders = "";
            var returnBinGeaders = "";
            myDrawdownBins.Keys.ToList().ForEach(x => drawdownBindHeaders += $"{x},");
            myReturnBins.Keys.ToList().ForEach(x => returnBinGeaders += $"{x},");
            generalResultsIteration.ToList().ForEach(x=>finalGeneralResults.Add($"{x[0]:0.00},{x[1]:0.00},{x[2]:0%},{x[3]:0.0},{x[4]:0.0},{x[5]:0.0},{x[6]:0.0}"));

            StringBuilder GeneralStats = WriteStats(finalGeneralResults, "AvgExpectancy,MedianExpectncy,Win%,AverageDrawdown,MedianDrawdown");
            StringBuilder DrawdownStats = WriteStats(histogramDrawdowns, drawdownBindHeaders);
            StringBuilder returnStats = WriteStats(histogramReturns, returnBinGeaders);
            StringBuilder allReturnStats = WriteStats(new List<string>(){ GenerateHistogramRow(allRetruns) }, returnBinGeaders);
            StringBuilder AllDrawdownByReturn = WriteStats(drawdownsByReturn, drawdownBindHeaders);

            var folder = Path.Combine(@"C:\Temp\Histograms", DateTime.Now.ToString("hh_mm_ss__dd_MM__yyyy"));
            Directory.CreateDirectory(folder);
            File.WriteAllText($"{folder}\\GeneralStats.csv", GeneralStats.ToString());
            File.WriteAllText($"{folder}\\drawdownHisto.csv", DrawdownStats.ToString());
            File.WriteAllText($"{folder}\\returnHisto.csv", returnStats.ToString());
            File.WriteAllText($"{folder}\\allReturns.csv", allReturnStats.ToString());
            File.WriteAllText($"{folder}\\DrawdownByReturn.csv", AllDrawdownByReturn.ToString());

        }

        public static void ResetBin(Dictionary<double, int> bin)
        {
            var keyList = bin.Keys.ToList();
            for (int i = 0; i < keyList.Count; i++) bin[keyList[i]] = 0;
        }

        public static List<string> TransformCategorisedListsToHistograms(Dictionary<double, List<double>> CategorisedLists)
        {
            List<string> results = new List<string>();
            var categoryKeys = CategorisedLists.Keys.ToList();

            for (int i = 0; i < categoryKeys.Count; i++)
            {
                results.Append($"{categoryKeys[i]},");
                var DrawddownBins = ListTools.BinGenerator(-50, 0, 5);
                for (int j = 0; j < CategorisedLists[categoryKeys[i]].Count; j++) CategoriseItem(DrawddownBins, CategorisedLists[categoryKeys[i]][j]);
                results.Add(GenerateHistogramRowCumulative(DrawddownBins));
            }

            return results;
        }

        private static string GenerateHistogramRowCumulative(Dictionary<double, int> bin)
        {
            var totalItems = bin.Values.Sum();
            var allBins = bin.Keys.ToList();

            string resultsRow = "";
            var lastVal = 0.0;
            for (int i = 0; i < allBins.Count; i++)
            {
                var myRes = (double) bin[allBins[i]] / (double) totalItems;
                lastVal += myRes;
                resultsRow += $"{lastVal:0.0%},";
            }

            return resultsRow;
        }

        private static string GenerateHistogramRow(Dictionary<double, int> bin)
        {
            var totalItems = bin.Values.Sum();
            var allBins = bin.Keys.ToList();

            string resultsRow = "";
            for (int i = 0; i < allBins.Count; i++) resultsRow += $"{(double)bin[allBins[i]] / (double)totalItems:0.0%},";

            return resultsRow;
        }


        private static void GenerateAverageResults(double[][] results, double[][] newResults, int testsLength, int currentIteration)
        {
            
            for (int i = 0; i < testsLength; i++)
            {
                if(results[i] == null) results[i]= new double[7];
                for (int j = 0; j < results[i].Length; j++)
                {
                    results[i][j] = (results[i][j] * (currentIteration - 1.00) + newResults[i][j]) / currentIteration;

                }
            }

        }

        private static double[] AddGeneralResultsArray(ITest test)
        {
            var myMedian = test.MedianDrawDownLong;
            return new double[5]
            {
                test.ExpectancyLongAverage,
                test.ExpectancyLongMedian,
                test.WinPercentageLong,
                test.AverageDrawdownLong,
                myMedian,

            };



        }

        public static StringBuilder WriteStats(List<string> resultsLong, string header)
        {
            StringBuilder longs = new StringBuilder().AppendLine(header);
            resultsLong.ForEach(x => longs.AppendLine($"{x},"));
            return longs.AppendLine();
        }

        public static void CategoriseItem(Dictionary<double, int> myBins, double item)
        {
            if (myBins.All(x => x.Key < item)) myBins[myBins.Max(x => x.Key)]++;
            else
            {
                for (int j = 1; j < myBins.Count; j++)
                {
                    if (myBins.Keys.ToList()[j] > item)
                    {
                        myBins[myBins.Keys.ToList()[j-1]]++;
                        break;
                    }
                }
            }
        }
        public static void CategoriseItem(Dictionary<double, List<double>> myBins, double item, double bin)
        {
            if (myBins.All(x => x.Key < bin)) myBins[myBins.Max(x => x.Key)].Add(item);
            else
            {
                for (int j = 1; j < myBins.Count; j++)
                {
                    if (myBins.Keys.ToList()[j] > bin)
                    {
                        myBins[myBins.Keys.ToList()[j-1]].Add(item);
                        break;
                    }
                }
            }
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
