using LinqStatistics;
using Logic;
using Logic.Metrics;
using Logic.Rules;
using Logic.Rules.Entry;
using Logic.Rules.Exit;
using Logic.Utils;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Logic.Metrics.CoreTests;
using Logic.Metrics.EntryTests;
using Logic.Metrics.ExitTests;

namespace UtilConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var asx_200_5_min = MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);

            var entryOne = new BullishMATag();
            var exitOne = new ThreeLowerLows();

            Strategy myStrat = StrategyBuilder.CreateStrategy(new IRuleSet[] { entryOne, exitOne }, asx_200_5_min);




            double[] FBEtestExps = new double[1000];
            double[] FixedStopExitExps = new double[1000];
            double[] RandomExitExps = new double[1000];
            double[] RandomEntryExps = new double[1000];
            StringBuilder stringo = new StringBuilder();


            RangeTest RT = new RangeTest(20);
            RT.Run(asx_200_5_min.RawData, myStrat);

            for (int i = 1; i < 1000; i++)
            {
                //Console.WriteLine(i);
                FixedBarExitTest t = new FixedBarExitTest(i);
                FixedStopTargetExitTest u = new FixedStopTargetExitTest(i, i, false);
                t.Run(asx_200_5_min.RawData, myStrat.Entries);
                u.Run(asx_200_5_min.RawData, myStrat.Entries);

                RandomExitTest v = new RandomExitTest(myStrat.Durations.Where(x=>x!=0).Average(), myStrat.Durations.Where(x => x != 0).StandardDeviation());
                RandomEntryTests w = new RandomEntryTests(myStrat.Durations.Where(x => x != 0).Average(), myStrat.Durations.Where(x => x != 0).StandardDeviation());
                v.Run(asx_200_5_min.RawData, myStrat.Entries);
                w.RunRE(asx_200_5_min.RawData, myStrat.Exits);

                stringo.AppendLine($"{t.ExpectancyLong:0.00},{u.ExpectancyLong:0.00},{v.ExpectancyLong:0.00},{w.ExpectancyLong:0.00}");
                Console.WriteLine($"{i} - - - {t.ExpectancyLong:0.00},{u.ExpectancyLong:0.00},{v.ExpectancyLong:0.00},{w.ExpectancyLong:0.00}");

            }

            File.WriteAllText(@"C:\Temp\myfile.csv", stringo.ToString());


        }
    }
}
