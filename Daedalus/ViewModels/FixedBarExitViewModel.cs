using Daedalus.Models;
using Daedalus.Utils;
using Logic.Metrics;

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

    

            _test = TestFactory.GenerateFixedBarExitTest(lowerLimit, upperLimit, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);
            
            base.InitialiseData();
        }

    }
}
