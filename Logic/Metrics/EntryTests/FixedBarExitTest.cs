namespace Logic.Metrics.EntryTests
{
    public class FixedBarExitTest : TestBase
    {

        // Fixed Bar Exit
        private int Fixed_Bar_exit { get; }

        public FixedBarExitTest(int bars_to_wait)
        {
            Fixed_Bar_exit = bars_to_wait;
        }


        public void RunFBE(MarketData[] data,bool[] entries)
        {
            FBELong = new double[data.Length];
            FBEShort= new double[data.Length];

            for (int i = 0; i < entries.Length - 1; i++)
            {
                if (entries[i])
                {
                    int x = i + 1;
                    if (x + Fixed_Bar_exit >= data.Length) return;

                    double entryPriceBull = data[x].Open_Ask;
                    double entryPriceBear = data[x].Open_Bid;

                    FBELong[i] = data[x + Fixed_Bar_exit].Open_Bid - entryPriceBull;
                    FBEShort[i] = entryPriceBear - data[x + Fixed_Bar_exit].Open_Ask;
                }
            }
        }
    }
}
