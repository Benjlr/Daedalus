using System;
using System.Collections.Generic;
using System.Text;
using DataStructures.PriceAlgorithms;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class MovingAverageTests
    {
        private List<double> myValues = new List<double>()
        {
            9.355409848, 8.411808685, 5.599313938, 0.414904245, 5.256302681,
            6.31540909, 1.497067101, 8.838435337, 5.662222594, 3.726458634,
            2.79255853, 9.489498002, 5.726134449, 6.330258231,
            1.066416182, 6.24062087, 8.904408531, 8.504210806, 1.769079426,

        };

        [Fact]
        private void ShouldCalculateSimpleMovingAverageCorrectlySevenPeriod() {
            var result = MovingAverage.SimpleMovingAverage(myValues, 7);

            Assert.Equal(new List<double>()
            {
                9.355409848, 8.8836092665, 7.788844157, 5.9453591789999995, 5.8075478793999995, 5.8921914145,
                5.264316512571428,
                5.1904630109999994,
                4.797664998,
                4.5301142402857142,
                4.8697791381428575,
                5.4745213268571433,
                5.3903392352857145,
                6.0807951110000005,
                4.9705066602857144,
                5.0531349854285708,
                5.792842113571429,
                6.6087924387142847,
                5.5058754992857146
            }, result);
        }

        [Fact]
        private void ShouldCalculateSimpleMovingAverageCorrectlyFourPeriod() {
            var result = MovingAverage.SimpleMovingAverage(myValues, 4);

            Assert.Equal(new List<double>()
            {
                9.355409848,
                8.8836092665,
                7.788844157,
                5.9453591789999995,
                4.9205823872500005,
                4.3964824885,
                3.37092077925,
                5.4768035522500007,
                5.5782835305,
                4.9310459165000005,
                5.25491877375,
                5.41768444,
                5.43366240375,
                6.084612303,
                5.653076716,
                4.840857433,
                5.6354259535,
                6.17891409725,
                6.35457990825
            }, result);
        }

        [Fact]
        private void ShouldCalculateExpMovingAverageCorrectlyThreePeriod() {
            var result = MovingAverage.ExponentialMovingAverage(myValues, 3);

            Assert.Equal(new List<double>()
            {
                9.355409848,
                8.8836092665,
                7.24146160225,
                3.828182923625,
                4.5422428023125,
                5.42882594615625,
                3.4629465235781249,
                6.1506909302890627,
                5.906456762144531 ,
                4.8164576980722655,
                3.8045081140361328,
                6.6470030580180666,
                6.1865687535090332,
                6.2584134922545172,
                3.6624148371272587,
                4.9515178535636295,
                6.9279631922818146,
                7.7160869991409076,
                4.7425832125704535
            }, result);
        }

        [Fact]
        private void ShouldCalculateExpMovingAverageCorrectlyNinePeriod() {
            var result = MovingAverage.ExponentialMovingAverage(myValues, 9);

            Assert.Equal(new List<double>()
            {
                9.355409848,
                9.166689615400001 ,
                8.4532144799200015,
                6.8455524329360014,
                6.5277024825488015,
                6.4852438040390412,
                5.4876084634312328,
                6.157773838144986,
                6.058663589315989,
                5.5922225982527909,
                5.0322897846022325,
                5.9237314280817861,
                5.8842120322654292,
                5.9734212720123434,
                4.9920202540098746,
                5.2417403772079,
                5.9742740079663195,
                6.4802613675730552,
                5.5380249792584442
            }, result);
        }
    }
}
