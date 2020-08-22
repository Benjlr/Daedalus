using Logic;
using Logic.Rules;
using Logic.Rules.Entry;
using Logic.Rules.Exit;
using Logic.Utils;

namespace Daedalus.Models
{
    public class ModelSingleton
    {

        public static ModelSingleton Instance => _instance ?? (_instance = new ModelSingleton());
        private static ModelSingleton _instance { get; set; }

        public Market Mymarket { get; set; }
        public Strategy MyStarrtegy { get; set; }


        private ModelSingleton()
        {
            Mymarket = MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);

            MyStarrtegy = StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                //new ThreeLowerLows(),

                //new BullishMATag(),
                //new PriceContractionFromLow(),
                //new KeltnerOverSold(),
                //new KeltnerOverBought(),

                //new TrendDay(),


                new PriceContraction(),
                new InvestorBotEntry(),

                //new BearishMATage(), 
                //new ThreeHigherHighs(), 

                //new FiftyFiftyEntry(), 
                //new FiftyFiftyExit(), 
            }, Mymarket);
        }

        

    }
}
