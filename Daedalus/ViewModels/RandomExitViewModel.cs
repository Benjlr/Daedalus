using System.Linq;
using Daedalus.Models;
using Daedalus.Utils;
using LinqStatistics;
using Logic;
using Logic.Metrics;
using Logic.Rules;
using Logic.Rules.Entry;
using Logic.Rules.Exit;
using Logic.Utils;

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
            _test = TestFactory.GenerateRandomExitTests(ModelSingleton.Instance.MyStarrtegy.Durations.Average(), ModelSingleton.Instance.MyStarrtegy.Durations.StandardDeviation(),50, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);
            
            base.InitialiseData();
        }

    }
}
