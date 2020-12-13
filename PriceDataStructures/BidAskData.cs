using System;
using Microsoft.VisualBasic;

namespace DataStructures
{
    public struct BidAskData
    {
        public BidAsk Open { get; set; }
        public BidAsk High { get; set; }
        public BidAsk Low { get; set; }
        public BidAsk Close { get; set; }
        public double Volume { get; set; }

        public BidAskData(
            BidAsk open,
            BidAsk high,
            BidAsk low,
            BidAsk close,
            double volume) {

            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public BidAskData(DateTime time, double o_a, double o_b, double h_a, double h_b, double l_a, double l_b, double c_a, double c_b, double vol)  {
            Open = new BidAsk(o_b,o_a, time);
            High = new BidAsk(h_b, h_a, time);
            Low = new BidAsk(l_b, l_a, time);
            Close = new BidAsk(c_b, c_a, time);
            Volume = vol;
        }

        public BidAskData(DateTime time, int vol, double open, double high, double low, double close ) {
            Open = new BidAsk(open, open, time);
            High = new BidAsk(high, high, time);
            Low = new BidAsk(low, low, time);
            Close = new BidAsk(close, close, time);
            Volume = vol;
        }
    }

    public struct BidAsk
    {
        public double Bid { get; set; }
        public double Ask { get; set; }
        public DateTime Time { get; set; }

        public double Mid => ((Ask - Bid) / 2.0) + Bid;

        public BidAsk(double bid, double ask, DateTime date) {
            Bid = bid;
            Ask = ask;
            Time = date;
        }
    }


}