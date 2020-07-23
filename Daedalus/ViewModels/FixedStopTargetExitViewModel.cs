using Daedalus.Models;
using Daedalus.Utils;
using Logic;
using Logic.Metrics;
using Logic.Rules;
using Logic.Rules.Entry;
using Logic.Rules.Exit;
using Logic.Utils;

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
            _test = TestFactory.GenerateFixedStopTargetExitTest(20, 5,2,50, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);
            
            base.InitialiseData();
        }

    }
}
