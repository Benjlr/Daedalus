using System.Collections.Generic;
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

        //public List<Market> Mymarket { get; set; }
        //public List<Strategy> MyStrategy { get; set; }


        public Market Mymarket { get; set; }
        public Strategy MyStrategy { get; set; }


        private ModelSingleton()
        {
            //Mymarket = new List<Market>();

            //Mymarket = MarketBuilder.CreateStockMarkets();

            Mymarket =MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);

            //MyStrategy =new List<Strategy>();

            //for (int i = 0; i < Mymarket.Count; i++)
            //{
            //    MyStrategy.Add(StrategyBuilder.CreateStrategy(new IRuleSet[]
            //    {
            //        //new MAViolation(), 
            //        //new ThreeLowerLows(),
            //        //new ThreeHigherHighs(), 

            //        //new BullishMATag(),
            //        //new PriceContractionFromLow(),
            //        //new KeltnerOverSold(),
            //        //new KeltnerOverBought(),


            //        new PivotAndNREntry(), 
            //        //new PivotExit(), 

            //        //new TrendDay(),


            //        new PriceContraction(),
            //        //new InvestorBotEntry(),

            //        //new BearishMATage(), 

            //        //new FiftyFiftyEntry(), 
            //        //new FiftyFiftyExit(), 

            //        //new CrossoverTag(),
            //    }, Mymarket[i]));
            //}



            MyStrategy = StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                //new MAViolation(), 
                //new ThreeLowerLows(),
                //new ThreeHigherHighs(), 

                //new BullishMATag(),
                //new PriceContractionFromLow(),
                //new KeltnerOverSold(),
                //new KeltnerOverBought(),


                //new Sigma(), 
                //new PivotExit(), 

                //new TrendDay(),

                new ATRContraction(),
                new PriceContraction(),
               // new InvestorBotEntry(),

                //new BearishMATage(), 

                //new FiftyFiftyEntry(), 
                //new FiftyFiftyExit(), 

                //new CrossoverTag(),

            }, Mymarket);
        }

        

    }
}
