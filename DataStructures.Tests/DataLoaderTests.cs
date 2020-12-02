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
            if (bundleAssembly?.CodeBase == null) return "";
            var asmPath = new Uri(bundleAssembly.CodeBase).LocalPath;
            return Path.Combine(Path.GetDirectoryName(asmPath) ?? "", data);
        }

        [Fact]
        private void ShouldLoadMarketFromBidAsktxt() {
            var myData = File.ReadAllLines(GetData("TextData\\TestMarketBidask.txt"));
            BidAskData[] myMarket = DataLoader.LoadBidAskData(GetData("TextData\\TestMarketBidask.txt"));

            for (int i = 0; i < myMarket.Length; i++) {
                var row = myData[i].Split(',');
                Assert.Equal(DateTime.ParseExact(row[0], "yyyy/MM/dd hh:mm:ss", null), myMarket[i].Time);
                Assert.Equal(double.Parse(row[1]), myMarket[i].Open_Ask);
                Assert.Equal(double.Parse(row[2]), myMarket[i].Open_Bid);
                Assert.Equal(double.Parse(row[3]), myMarket[i].High_Ask);
                Assert.Equal(double.Parse(row[4]), myMarket[i].High_Bid);
                Assert.Equal(double.Parse(row[5]), myMarket[i].Low_Ask);
                Assert.Equal(double.Parse(row[6]), myMarket[i].Low_Bid);
                Assert.Equal(double.Parse(row[7]), myMarket[i].Close_Ask);
                Assert.Equal(double.Parse(row[8]), myMarket[i].Close_Bid);
                Assert.Equal(double.Parse(row[9]), myMarket[i].volume);
            }
        }

        [Fact]
        private void ShouldLoadMarketFromSessiontxt() {
            var myData = File.ReadAllLines(GetData("TextData\\TestMarketBidSession.txt"));
            SessionData[] myMarket = DataLoader.LoadConsolidatedData(GetData("TextData\\TestMarketBidSession.txt"));
            for (int i = 0; i < myMarket.Length; i++) {
                var row = myData[i].Split(',');
                Assert.Equal(DateTime.ParseExact(row[0], "yyyy/MM/dd", null), myMarket[i].CloseDate);
                Assert.Equal(double.Parse(row[1]), myMarket[i].Open);
                Assert.Equal(double.Parse(row[2]), myMarket[i].High);
                Assert.Equal(double.Parse(row[3]), myMarket[i].Low);
                Assert.Equal(double.Parse(row[4]), myMarket[i].Close);
                Assert.Equal(double.Parse(row[5]), myMarket[i].Volume);
            }
        }

        [Fact]
        private void ShouldReturnDataType() {
            Assert.IsType(DataLoader.CheckDataType(GetData("TextData\\TestMarketBidSession.txt")), new SessionData());
            Assert.IsType(DataLoader.CheckDataType(GetData("TextData\\TestMarketBidask.txt")), new BidAskData());
        }

        [Fact]
        private void ShouldThrowForWrongData() {
            Assert.Throws<Exception>(() => DataLoader.CheckDataType(GetData("TextData\\InvalidMarketData.txt")));
        }
    }
}
