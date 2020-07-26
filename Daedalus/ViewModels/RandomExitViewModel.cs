using Daedalus.Models;
using Daedalus.Utils;
using LinqStatistics;
using Logic.Metrics;
using System.Linq;

namespace Daedalus.ViewModels
{
    public class RandomExitViewModel : TestViewModelBase
    {


        public RandomExitViewModel() : base()
        {
            this.InitialiseData();
        }


        protected sealed override void InitialiseData()
        {
            _test = TestFactory.GenerateRandomExitTests(ModelSingleton.Instance.MyStarrtegy.Durations.Average(), ModelSingleton.Instance.MyStarrtegy.Durations.StandardDeviation(),250, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);
            
            base.InitialiseData();
        }

    }
}
