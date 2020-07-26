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

            var entryOne = new PriceContraction();
            var exitOne = new ThreeLowerLows();

            MyStarrtegy = StrategyBuilder.CreateStrategy(new IRuleSet[] { entryOne, exitOne }, Mymarket);
        }

        

    }
}
