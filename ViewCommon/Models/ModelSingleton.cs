using Logic;
using Logic.Utils;
using RuleSets;
using RuleSets.Entry;
using RuleSets.Exit;
using System.IO;

namespace ViewCommon.Models
{
    public class ModelSingleton
    {

        public static ModelSingleton Instance => _instance ??= new ModelSingleton();
        private static ModelSingleton _instance { get; set; }
        public Market Mymarket { get; set; }
        public Strategy MyStrategy { get; set; }
        private object _lock = new object() ;

        private ModelSingleton()
        {
            lock (_lock)
            {
                try {
                    Mymarket = Market.MarketBuilder.CreateMarket(Markets.SP500_Cash_5_Min);
                }
                catch {
                    var marketData = Directory.GetCurrentDirectory() + "\\Utils\\LocalData\\asx200cash";
                    Mymarket = Market.MarketBuilder.CreateMarket(marketData);
                }




                MyStrategy = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
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


        }

    }
}
