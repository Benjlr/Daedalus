

namespace DataStructures
{
    public struct TradePrices
    {
        public double EntryPrice { get; set; }
        public double StopPrice { get; set; }
        public double TargetPrice { get; set; }
        public double Return { get; set; }

        public TradePrices(ExitPrices exits, double entryPrice)
        {

        }
    }

    public abstract class TradeStateGenerator
    {
        private ArrayBuilder _tradeBuilder { get; set; }
        public TradePrices TradeLimits { get; private set; }
        
        protected TradeStateGenerator(int marketIndex) {
            _tradeBuilder = new ArrayBuilder();
            _tradeBuilder.Init(marketIndex);

        }

        public static TradeStateGenerator Invest(MarketSide longShort, int marketIndex, double entryPrice, double closePrice) {
            TradeStateGenerator generator = new TradeStateGenerator(longShort, marketIndex);
            generator.TradeLimits = new TradePrices() {
                EntryPrice = entryPrice,
                StopPrice = entryPrice * stopTarget.StopPercentage,
                TargetPrice = entryPrice * stopTarget.TargetPercentage,
                Return = generator.CalculateReturn(closePrice, entryPrice)
            };
            generator._tradeBuilder.AddResult(generator.TradeLimits.Return);
            return generator;
        }

        public void Continue(double closeData) {
            TradeLimits = new TradePrices() {
                EntryPrice = TradeLimits.EntryPrice,
                StopPrice = TradeLimits.StopPrice,
                TargetPrice = TradeLimits.TargetPrice,
                Return = CalculateReturn(closeData, TradeLimits.EntryPrice)
            };
            _tradeBuilder.AddResult(TradeLimits.Return);
        }

        public void ContinueUpdateExits(double closeData, ExitPrices exitPrices) {
            TradeLimits = new TradePrices()
            {
                EntryPrice = TradeLimits.EntryPrice,
                StopPrice = TradeLimits.EntryPrice * exitPrices.StopPercentage,
                TargetPrice = TradeLimits.EntryPrice * exitPrices.TargetPercentage,
                Return = CalculateReturn(closeData, TradeLimits.EntryPrice)
            };
            _tradeBuilder.AddResult(TradeLimits.Return);
        }

        public Trade Exit(double exitPrice) {
            _tradeBuilder.AddResult(CalculateReturn(exitPrice, TradeLimits.EntryPrice));
            return _tradeBuilder.CompileTrade();
        }

        protected abstract double calculateReturn(BidAskData data) {
            if (_isLong.Equals(MarketSide.Bull)) return (current - entry) / entry;
            else return -(current - entry) / entry;

        }

        protected abstract TradePrices initTradePrices(BidAskData data);
    }

    public class LongTradeGenerator : TradeStateGenerator
    {
        protected LongTradeGenerator(int marketIndex) : base(marketIndex)
        { }

        protected override double calculateReturn(BidAskData data)
        {
            return 
        }
    }

    public class ShortTradeGenerator : TradeStateGenerator
    {

    }
}
