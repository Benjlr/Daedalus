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
            var upperLimit = 1000;
            var lowerLimit = 1;

            _test = TestFactory.GenerateFixedBarExitTest(ModelSingleton.Instance.MyStrategy, ModelSingleton.Instance.Mymarket, new FixedBarExitTestOptions(lowerLimit, upperLimit, 1));
            
            base.InitialiseData();
        }

    }
}
