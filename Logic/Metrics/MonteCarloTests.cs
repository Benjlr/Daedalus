using System;
using System.Collections.Generic;
using System.Text;


namespace Logic.Metrics
{ 
    public class MonteCarloTests
    {
        public void Run(Strategy strat, Market market, double initCapital, double dollarsPerPoint)
        {
            var myCapitalLong = initCapital;
            var myCapitalShort = initCapital;
            var myReturnsLong = new double[market.RawData.Length];
            var myReturnsShort = new double[market.RawData.Length];
            var data = market.RawData;

            for (int i = 0; i < strat.Entries.Length; i++)
            {
                myReturnsLong[i] = myCapitalLong; 
                myReturnsShort[i] = myCapitalShort;
                if (strat.Entries[i])
                {
                    i++;
                    double entryPriceBull = data[i].Open_Ask;
                    double entryPriceBear = data[i].Open_Bid;


                    while (i < data.Length)
                    {
                        myCapitalLong = (data[i].Open_Bid - entryPriceBull) * dollarsPerPoint; 
                        myCapitalShort = (entryPriceBear - data[i].Open_Ask) * dollarsPerPoint;
                        myReturnsLong[i] = myCapitalLong;
                        myReturnsShort[i] = myCapitalShort;
                        i++;
                        if (strat.Exits[i]) break;

                    }
                    
                    myCapitalLong = (data[i].Open_Bid - entryPriceBull) * dollarsPerPoint;
                    myCapitalShort = (entryPriceBear - data[i].Open_Ask) * dollarsPerPoint;
                    myReturnsLong[i] = myCapitalLong;
                    myReturnsShort[i] = myCapitalShort;

                    if (i >= data.Length - 1) break;
                    
                }
            }
            MonteCarloCalculator T = new MonteCarloCalculator(200);
            

        }
    }
}
