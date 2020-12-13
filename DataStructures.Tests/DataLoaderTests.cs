using System;
using System.IO;
using System.Linq;
using Xunit;

namespace DataStructures.Tests
{
    public class DataLoaderTests
    {
        static string GetData(string data) {
            var bundleAssembly = AppDomain.CurrentDomain?.GetAssemblies().FirstOrDefault(x => x.FullName != null && x.FullName.Contains("TestUtils"));
            if (bundleAssembly?.Location == null) return "";
            var asmPath = new Uri(bundleAssembly.Location).LocalPath;
            return Path.Combine(Path.GetDirectoryName(asmPath) ?? "", data);
        }

        [Fact]
        private void ShouldLoadMarketFromBidAsktxt() {
            var myData = File.ReadAllLines(GetData("TextData\\TestMarketBidask.txt"));
            BidAskData[] myMarket = DataLoader.LoadData(GetData("TextData\\TestMarketBidask.txt"));

            for (int i = 0; i < myMarket.Length; i++) {
                var row = myData[i].Split(',');
                Assert.Equal(DateTime.ParseExact(row[0], "yyyy/MM/dd hh:mm:ss", null), myMarket[i].Close.Time);
                Assert.Equal(double.Parse(row[1]), myMarket[i].Open.Ask);
                Assert.Equal(double.Parse(row[2]), myMarket[i].Open.Bid);
                Assert.Equal(double.Parse(row[3]), myMarket[i].High.Ask);
                Assert.Equal(double.Parse(row[4]), myMarket[i].High.Bid);
                Assert.Equal(double.Parse(row[5]), myMarket[i].Low.Ask);
                Assert.Equal(double.Parse(row[6]), myMarket[i].Low.Bid);
                Assert.Equal(double.Parse(row[7]), myMarket[i].Close.Ask);
                Assert.Equal(double.Parse(row[8]), myMarket[i].Close.Bid);
                Assert.Equal(double.Parse(row[9]), myMarket[i].Volume);
            }
        }

        [Fact]
        private void ShouldLoadMarketFromSessiontxt() {
            var myData = File.ReadAllLines(GetData("TextData\\TestMarketBidSession.txt"));
            BidAskData[] myMarket = DataLoader.LoadData(GetData("TextData\\TestMarketBidSession.txt"));
            for (int i = 0; i < myMarket.Length; i++) {
                var row = myData[i].Split(',');
                Assert.Equal(DateTime.ParseExact(row[0], "yyyy/MM/dd", null), myMarket[i].Close.Time);
                Assert.Equal(double.Parse(row[1]), myMarket[i].Open.Mid);
                Assert.Equal(double.Parse(row[2]), myMarket[i].High.Mid);
                Assert.Equal(double.Parse(row[3]), myMarket[i].Low.Mid);
                Assert.Equal(double.Parse(row[4]), myMarket[i].Close.Mid);
                Assert.Equal(double.Parse(row[5]), myMarket[i].Volume);
            }
        }


        [Fact]
        private void ShouldThrowForWrongData() {
            Assert.Throws<Exception>(() => DataLoader.LoadData(GetData("TextData\\InvalidMarketData.txt")));
        }
    }
}
