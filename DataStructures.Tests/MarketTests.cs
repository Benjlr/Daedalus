using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TestUtils;
using Xunit;

namespace DataStructures.Tests
{
    public class MarketTests
    {
        static string GetData(string data) {
            var bundleAssembly = AppDomain.CurrentDomain?.GetAssemblies().FirstOrDefault(x => x.FullName != null && x.FullName.Contains("TestUtils"));
            if(bundleAssembly?.CodeBase == null) return "";
            var asmPath = new Uri(bundleAssembly.CodeBase).LocalPath;
            return Path.Combine(Path.GetDirectoryName(asmPath) ?? "", data);
        }

        [Fact]
        private void ShouldLoadMarketFromBidAsktxt() {
            var myData = File.ReadAllLines(GetData("TestMarketBidask.txt"));
            Market myMarket = Market.MarketBuilder.CreateMarket(GetData("TestMarketBidask.txt"));

            for (int i = 0; i < myMarket.RawData.Length; i++) {
                var row = myData[i].Split(',');
                Assert.Equal(DateTime.ParseExact(row[0], "yyyy/MM/dd hh:mm:ss", null), myMarket.RawData[i].Time );
                Assert.Equal(double.Parse(row[1]), myMarket.RawData[i].Open_Ask);
                Assert.Equal(double.Parse(row[2]), myMarket.RawData[i].Open_Bid);
                Assert.Equal(double.Parse(row[3]), myMarket.RawData[i].High_Ask);
                Assert.Equal(double.Parse(row[4]), myMarket.RawData[i].High_Bid);
                Assert.Equal(double.Parse(row[5]), myMarket.RawData[i].Low_Ask);
                Assert.Equal(double.Parse(row[6]), myMarket.RawData[i].Low_Bid);
                Assert.Equal(double.Parse(row[7]), myMarket.RawData[i].Close_Ask);
                Assert.Equal(double.Parse(row[8]), myMarket.RawData[i].Close_Bid);
                Assert.Equal(double.Parse(row[9]), myMarket.RawData[i].volume);
            }
        }
        [Fact]
        private void ShouldLoadMarketFromSessiontxt() {
            var myData = File.ReadAllLines(GetData("TestMarketBidSession.txt"));
            Market myMarket = Market.MarketBuilder.CreateMarket(GetData("TestMarketBidSession.txt"));
            for (int i = 0; i < myMarket.CostanzaData.Length; i++) {
                var row = myData[i].Split(',');
                Assert.Equal(DateTime.ParseExact(row[0], "yyyy/MM/dd", null), myMarket.CostanzaData[i].CloseDate);
                Assert.Equal(double.Parse(row[1]), myMarket.CostanzaData[i].Open);
                Assert.Equal(double.Parse(row[2]), myMarket.CostanzaData[i].High);
                Assert.Equal(double.Parse(row[3]), myMarket.CostanzaData[i].Low);
                Assert.Equal(double.Parse(row[4]), myMarket.CostanzaData[i].Close);
                Assert.Equal(double.Parse(row[5]), myMarket.CostanzaData[i].Volume);
            }
        }

        [Fact]
        private void ShouldSliceMarket() {
            Market myMarket = Market.MarketBuilder.CreateMarket(GetData("TestMarketBidSession.txt"));
            var newMarket = myMarket.Slice(20, 40);

            for (int i = 20; i <= 40; i++) {
                Assert.Equal(myMarket.RawData[i], newMarket.RawData[i-20]);  
                Assert.Equal(myMarket.CostanzaData[i], newMarket.CostanzaData[i-20]);
            }

            Assert.Equal(21, newMarket.RawData.Length);
            Assert.Equal(21, newMarket.CostanzaData.Length);
        }

        [Fact]
        private void ShouldCollateBidAskToSession() {
            Market myMarket = Market.MarketBuilder.CreateMarket(new BidAskData[]{new BidAskData(new DateTime(2020,01,01), 8,7,12,10,4,1,10,10,123 ) } );
            Assert.Equal(new SessionData(new DateTime(2020, 01, 01), 123,7.5,11,2.5,10), myMarket.CostanzaData[0] );
        }


        [Fact]
        private void ShouldCollateSessionToBidAsk() {
            Market myMarket = Market.MarketBuilder.CreateMarket(new SessionData[] { new SessionData(new DateTime(2020, 01, 01), 123,  8,  10,  1, 10) });
            Assert.Equal(new BidAskData(new DateTime(2020, 01, 01), 8, 8,10,10,1, 1, 10,10,123), myMarket.RawData[0]);
        }
    }
}
