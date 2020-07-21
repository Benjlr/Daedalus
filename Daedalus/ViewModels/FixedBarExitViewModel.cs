using System;
using System.Collections.Generic;
using System.Text;
using Daedalus.Utils;
using Logic.Metrics.EntryTests;
using OxyPlot;

namespace Daedalus.ViewModels
{
    public class FixedBarExitViewModel : ViewModelBase
    {
        private FixedBarExitTest _test { get; set; }

        public Model PlotModel { get; set; }
        public PlotController ControllerModel { get; set; }



        public FixedBarExitViewModel()
        {

        }


        private void InitialiseData()
        {
            _test = new FixedBarExitTest();
        }

    }
}
