namespace Viewer
{
    public readonly struct Model
    {
        public Model(double[][] movingAverages, double[][] prices, int entryIndex, int exitIndex, double entryPrice, double exitPrice) {
            MovingAverages = movingAverages;
            Prices = prices;
            EntryIndex = entryIndex;
            ExitIndex = exitIndex;
            EntryPrice = entryPrice;
            ExitPrice = exitPrice;
        }

        public double[][] MovingAverages { get;  }
        public double[][] Prices { get;  }
        public int EntryIndex { get;  }
        public int ExitIndex { get;  }

        public double EntryPrice { get; }
        public double ExitPrice { get; }

    }
}
