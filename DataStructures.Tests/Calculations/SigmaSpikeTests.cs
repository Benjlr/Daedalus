using System;
using System.Collections.Generic;
using System.Text;
using DataStructures.PriceAlgorithms;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class SigmaSpikeTests
    {
        [Fact]
        private void ShouldCalculateSigmaCorrectly() {
            var sigResult = SigmaSpike.Calculate(data,5);
            Assert.Equal(new List<double>()
            {
                0                       ,
                0                       ,
                0                       ,
                0                       ,
                0.3839999999999999      ,
                -0.28610197815443505    ,
                1.6131903102174843      ,
                0.32952945852666371     ,
                -0.57949710080731409    ,
                -2.1104356911667339     ,
                71.746295422875235      ,
                -0.039104579956593742   ,
                -0.014930024518812597   ,
                -0.022377124801225692   ,
                0.022437173425243578    ,
                43.25323770611957       ,
            }, sigResult);
        }


        public List<double> data = new List<double>()
        {
           2,
           6,
           4,
           5,
           7,
           5,
           8,
           9,
           7,
           1,
           45,
           6,
           4,
           2,
           3,
           78,
        };
    }
}
