using System;
using System.Collections.Generic;
using System.Text;
using Daedalus.Utils;
using Logic.Metrics;
using Logic.Metrics.EntryTests;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Daedalus.ViewModels
{
    public class FixedBarExitViewModel : TestViewModelBase
    {
        private int upperLimit { get; set; } 
        private int lowerLimit { get; set; } 



        public FixedBarExitViewModel() : base()
        {
            this.InitialiseData();
        }


        protected sealed override void InitialiseData()
        {
            upperLimit = 300;
            lowerLimit = 1;

            _test = TestFactory.GenerateFixedBarExitTest(lowerLimit, upperLimit);
            
            base.InitialiseData();
        }

    }
}
