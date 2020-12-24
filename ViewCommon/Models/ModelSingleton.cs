using DataStructures;
using Logic;
using RuleSets;
using RuleSets.Entry;
using RuleSets.Exit;
using System.IO;
using DataStructures.StatsTools;

namespace ViewCommon.Models
{
    public class ModelSingleton
    {

        public static ModelSingleton Instance => _instance ??= new ModelSingleton();
        private static ModelSingleton _instance { get; set; }
        public Market Mymarket { get; set; }
        public StaticStrategy MyStrategy { get; set; }
        private object _lock = new object() ;

        private ModelSingleton()
        {
            lock (_lock)
            {
                try {
                    Mymarket = new Market(DataLoader.LoadData(Markets.asx200_cash_5), "asx200_cash_5");
                }
                catch {
                    var marketData = Directory.GetCurrentDirectory() + "\\Utils\\LocalData\\asx200cash";
                    Mymarket = new Market(DataLoader.LoadData(marketData), "asx200cash");
                }




                MyStrategy = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[]
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

                }, Mymarket , new StaticStopTarget(new ExitPrices(0.8,1.2)));

            }


        }

    }
}
