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
using Microsoft.Win32;

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
            OpenFileDialog t = new OpenFileDialog();
            t.ShowDialog();
            string getData = t.FileName;

            Mymarket =MarketBuilder.CreateMarket(getData);
            
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
