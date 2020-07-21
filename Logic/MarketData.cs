using System;

namespace Logic
{
    public struct MarketData
    {
        public DateTime Time { get; set; }
        public double Open_Ask { get; set; }
        public double Open_Bid { get; set; }
        public double High_Ask { get; set; }
        public double High_Bid { get; set; }
        public double Low_Ask { get; set; }
        public double Low_Bid { get; set; }
        public double Close_Ask { get; set; }
        public double Close_Bid { get; set; }
        public long volume{ get; set; }

        public MarketData(DateTime time,
                            double o_a,
                            double o_b,
                            double h_a,
                            double h_b,
                            double l_a,
                            double l_b,
                            double c_a,
                            double c_b,
                            long vol)
        {
            Time = time;
            Open_Ask = o_a;
            Open_Bid = o_b;

            High_Ask = h_a;
            High_Bid = h_b;

            Low_Ask = l_a;
            Low_Bid = l_b;

            Close_Ask = c_a;
            Close_Bid = c_b;

            volume = vol;
        }
    }
}
