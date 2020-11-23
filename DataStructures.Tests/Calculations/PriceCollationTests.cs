using System;
using System.Collections.Generic;
using System.Text;
using DataStructures.PriceAlgorithms;
using Xunit;
using Xunit.Sdk;

namespace DataStructures.Tests.Calculations
{
    public class PriceCollationTests
    {
        [Fact]
        private void ShouldCollateTo24hrCorrectly() {
            List<SessionData> myData = new List<SessionData>();
            var myDate = new DateTime(01,01,01,00,00,00);
            while (myDate.Day < 5) {
                if (myDate.Hour == 0 && myDate.Minute == 0) myData.Add(new SessionData(myDate, 0, 10, 5, 5, 5));
                else if (myDate.Hour == 12 && myDate.Minute ==5) myData.Add(new SessionData(myDate, 0, 5, 36, 5, 5));
                else if (myDate.Hour == 13 && myDate.Minute ==20) myData.Add(new SessionData(myDate, 0, 5, 5, 3, 5));
                else if (myDate.Hour == 23 && myDate.Minute ==55) myData.Add(new SessionData(myDate, 0, 5, 5, 5, 7));
                else myData.Add(new SessionData(myDate, 0, 5, 5, 5, 5));
                myDate = myDate.AddMinutes(5);
            }

            var newList = SessionCollate.CollateTo24HrDaily(myData);
            
            Assert.True(newList.Count == 4);
            for (int i = 0; i < newList.Count; i++) {
                Assert.Equal(i+1, newList[i].OpenDate.Day);
                Assert.Equal(i+1, newList[i].CloseDate.Day);

                Assert.Equal(00, newList[i].OpenDate.Hour);
                Assert.Equal(00, newList[i].OpenDate.Minute);
                Assert.Equal(23, newList[i].CloseDate.Hour);
                Assert.Equal(55, newList[i].CloseDate.Minute);

                Assert.Equal(12, newList[i].HighDate.Hour);
                Assert.Equal(5, newList[i].HighDate.Minute);
                Assert.Equal(13, newList[i].LowDate.Hour);
                Assert.Equal(20, newList[i].LowDate.Minute);

                Assert.Equal(10, newList[i].Open);
                Assert.Equal(36, newList[i].High);
                Assert.Equal(3, newList[i].Low);
                Assert.Equal(7, newList[i].Close);
            }
        }

        [Fact]
        private void ShouldCollateToMarketHoursCorrectly() {

            List<SessionData> myData = new List<SessionData>();
            var myDate = new DateTime(01, 01, 01, 00, 00, 00);
            while (myDate.Day < 5) {
                if (myDate.Hour == 10 && myDate.Minute == 0) myData.Add(new SessionData(myDate, 0, 10, 5, 5, 5));
                else if (myDate.Hour == 12 && myDate.Minute == 5) myData.Add(new SessionData(myDate, 0, 5, 36, 5, 5));
                else if (myDate.Hour == 13 && myDate.Minute == 20) myData.Add(new SessionData(myDate, 0, 5, 5, 3, 5));
                else if (myDate.Hour == 15 && myDate.Minute == 55) myData.Add(new SessionData(myDate, 0, 5, 5, 5, 7));
                else myData.Add(new SessionData(myDate, 0, 5, 5, 5, 5));
                myDate = myDate.AddMinutes(5);
            }

            var newList = SessionCollate.CollateToDaily(myData);

            Assert.True(newList.Count == 4);
            for (int i = 0; i < newList.Count; i++) {
                Assert.Equal(i + 1, newList[i].OpenDate.Day);
                Assert.Equal(i + 1, newList[i].CloseDate.Day);

                Assert.Equal(10, newList[i].OpenDate.Hour);
                Assert.Equal(00, newList[i].OpenDate.Minute);
                Assert.Equal(15, newList[i].CloseDate.Hour);
                Assert.Equal(55, newList[i].CloseDate.Minute);

                Assert.Equal(12, newList[i].HighDate.Hour);
                Assert.Equal(5, newList[i].HighDate.Minute);
                Assert.Equal(13, newList[i].LowDate.Hour);
                Assert.Equal(20, newList[i].LowDate.Minute);

                Assert.Equal(10, newList[i].Open);
                Assert.Equal(36, newList[i].High);
                Assert.Equal(3, newList[i].Low);
                Assert.Equal(7, newList[i].Close);
            }
        }


        [Fact]
        private void ShouldCollateToHourlyCorrectly() {

            List<SessionData> myData = new List<SessionData>();
            var myDate = new DateTime(01, 01, 01, 00, 00, 00);
            while (myDate.Day < 3) {
                if (myDate.Minute == 0) myData.Add(new SessionData(myDate, 0, 10, 5, 5, 5));
                else if (myDate.Minute == 15) myData.Add(new SessionData(myDate, 0, 5, 36, 5, 5));
                else if (myDate.Minute == 20) myData.Add(new SessionData(myDate, 0, 5, 5, 3, 5));
                else if (myDate.Minute == 55) myData.Add(new SessionData(myDate, 0, 5, 5, 5, 7));
                else myData.Add(new SessionData(myDate, 0, 5, 5, 5, 5));
                myDate = myDate.AddMinutes(5);
            }

            var newList = SessionCollate.CollateToHourly(myData);

            Assert.True(newList.Count == 48);
            for (int i = 0; i < newList.Count; i++) {
                Assert.Equal(00, newList[i].OpenDate.Minute);
                Assert.Equal(55, newList[i].CloseDate.Minute);

                Assert.Equal(15, newList[i].HighDate.Minute);
                Assert.Equal(20, newList[i].LowDate.Minute);

                Assert.Equal(10, newList[i].Open);
                Assert.Equal(36, newList[i].High);
                Assert.Equal(3, newList[i].Low);
                Assert.Equal(7, newList[i].Close);
            }
        }
    }
}
