namespace Logic
{
    public enum MarketSide {
        Bull,
        Bear
    }

    public enum Action {
        Entry,
        Exit,
    }

    public enum StrategyMode
    {        
        UsingStopTargets,
        UsingExits,
        ExitsRaiseStops
    }

}
