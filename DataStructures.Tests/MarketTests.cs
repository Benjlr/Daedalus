using System;
using System.IO;
using System.Linq;
using Xunit;

namespace DataStructures.Tests
{
    public class MarketTests
    {
        static string GetData(string data) {
            var bundleAssembly = AppDomain.CurrentDomain?.GetAssemblies().FirstOrDefault(x => x.FullName != null && x.FullName.Contains("TestUtils"));
            if (bundleAssembly?.Location == null) return "";
            var asmPath = new Uri(bundleAssembly.Location).LocalPath;
            return Path.Combine(Path.GetDirectoryName(asmPath) ?? "", data);
        }
        [Fact]
        private void ShouldSliceMarket() {
            Market myMarket = Market.MarketBuilder.CreateMarket(DataLoader.LoadData(GetData("TextData\\TestMarketBidSession.txt")));
            var newMarket = myMarket.Slice(20, 40);
            for (int i = 20; i <= 40; i++) {
                Assert.Equal(myMarket.RawData[i], newMarket.RawData[i-20]);  
                Assert.Equal(myMarket.RawData[i], newMarket.RawData[i-20]);
            }

            Assert.Equal(21, newMarket.RawData.Length);
            Assert.Equal(21, newMarket.RawData.Length);
        }

        [Fact]
        private void ShouldCollateSessionToBidAsk() {
            Market myMarket = Market.MarketBuilder.CreateMarket(new BidAskData[] { new BidAskData(new DateTime(2020, 01, 01), 123,  8,  10,  1, 10) });
            Assert.Equal(new BidAskData(new DateTime(2020, 01, 01), 8, 8,10,10,1, 1, 10,10,123), myMarket.RawData[0]);
        }
    }
}
