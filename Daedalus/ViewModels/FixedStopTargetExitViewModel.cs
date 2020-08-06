using Daedalus.Models;
using Daedalus.Utils;
using Logic.Metrics;

namespace Daedalus.ViewModels
{
    public class FixedStopTargetExitViewModel : TestViewModelBase
    {


        public FixedStopTargetExitViewModel() : base()
        {
            this.InitialiseData();
        }


        protected sealed override void InitialiseData()
        {
            _test = TestFactory.GenerateFixedStopTargetExitTest(50, 5,5,50, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);
            
            base.InitialiseData();
        }

    }
}
