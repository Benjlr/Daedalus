using Daedalus.Models;
using Daedalus.Utils;
using Logic.Metrics;

namespace Daedalus.ViewModels
{
    public class FixedBarExitViewModel : TestViewModelBase
    {

        public FixedBarExitViewModel() : base()
        {
            this.InitialiseData();
        }


        protected sealed override void InitialiseData()
        {
            var upperLimit = 865;
            var lowerLimit = 1;
            
            _test = TestFactory.GenerateFixedBarExitTest(lowerLimit, upperLimit, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);
            
            base.InitialiseData();
        }

    }
}
