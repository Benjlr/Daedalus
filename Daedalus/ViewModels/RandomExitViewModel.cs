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
            _test = TestFactory.GenerateRandomExitTests(
                ModelSingleton.Instance.MyStrategy.Durations.Where(x=>x!=0).Average(), 
                ModelSingleton.Instance.MyStrategy.Durations.Where(x=>x!=0).StandardDeviation(),
                250, 
                ModelSingleton.Instance.MyStrategy,
                ModelSingleton.Instance.Mymarket);
            
            base.InitialiseData();
        }

    }
}
