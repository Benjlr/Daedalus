using DataStructures;
using DataStructures.StatsTools;
using System;
using System.Collections.Generic;
using System.Windows;
using ViewCommon.Utils;

namespace Icarus.ViewModels
{
    public class StatsViewModel : ViewModelBase
    {
        private ExtendedStats _stats { get; set; }
        private List<Trade> _trades { get; set; }

        public StatsViewModel() {
            _trades = new List<Trade>();
        }

        public string WinPercent => $"Win Percent: {Environment.NewLine}{_stats?.WinPercent:0.0%}";

        public string AverageGain => $"Avg. gain: {Environment.NewLine}{_stats?.AvgGain:0.000}";
        public string AverageLoss => $"Avg. loss: {Environment.NewLine}{_stats?.AvgLoss:0.000}";

        public string AverageTitWin=> $"Avg. Time: {Environment.NewLine}{_stats?.AverageTimeWinners:0.000}";
        public string MedianTitWin => $"Mdn. Time: {Environment.NewLine}{_stats?.MedianTimeWinners:0.000}";
        public string AverageTitLose => $"Avg. Time: {Environment.NewLine}{_stats?.AverageTimeLosers:0.000}";
        public string MedianTitLose => $"Mdn. Time: {Environment.NewLine}{_stats?.MedianTimeLosers:0.000}";

        public string MedianGain => $"Mdn. gain: {Environment.NewLine}{_stats?.MedianGain:0.000}";
        public string MedianLoss => $"Mdn. loss: {Environment.NewLine}{_stats?.MedianLoss:0.000}";

        public string AverageExpectancy => $"Avg. expectancy:{Environment.NewLine} {_stats?.AverageExpectancy:0.000}";
        public string MedianExpectancy => $"Mdn. expectancy: {Environment.NewLine}{_stats?.MedianExpectancy:0.000}";

        public string SortinoRatio => $"Sortino ratio: {Environment.NewLine}{_stats?.Sortino:0.000}";

        public string AverageDrawdown => $"Avg. drawdown: {Environment.NewLine}{_stats?.AverageDrawdown:0.000}";
        public string AverageDrawdownWinners => $"Avg. drawdown winners: {Environment.NewLine}{_stats?.AverageDrawdownWinners:0.000}";

        public string MedianDrawdown => $"Mdn. drawdown:{Environment.NewLine} {_stats?.MedianDrawDown:0.000}";
        public string MedianDrawdownWinners => $"Mdn. drawdown winners: {Environment.NewLine}{_stats?.MedianDrawDownWinners:0.000}";


        public void UpdateStats(Trade newTarde) {
            _trades.Add(newTarde);
            _stats = new ExtendedStats(_trades);

            Application.Current.Dispatcher.Invoke(() => {
                NotifyPropertyChanged($"WinPercent");
            NotifyPropertyChanged($"AverageGain");
            NotifyPropertyChanged($"AverageLoss");
            NotifyPropertyChanged($"MedianGain");
            NotifyPropertyChanged($"MedianLoss");
            NotifyPropertyChanged($"AverageExpectancy");
            NotifyPropertyChanged($"MedianExpectancy");
            NotifyPropertyChanged($"SortinoRatio");
            NotifyPropertyChanged($"AverageDrawdown");
            NotifyPropertyChanged($"AverageDrawdownWinners");
            NotifyPropertyChanged($"MedianDrawdown");
            NotifyPropertyChanged($"MedianDrawdownWinners");
            NotifyPropertyChanged($"AverageTitWin");
            NotifyPropertyChanged($"MedianTitWin");
            NotifyPropertyChanged($"AverageTitLose");
            NotifyPropertyChanged($"MedianTitLose");
            });
            //ThreadPool.QueueUserWorkItem(Dowork);
        }

        private void Dowork(object callback) {
            _stats = new ExtendedStats(_trades);

            //Application.Current.Dispatcher.Invoke(() => {
                NotifyPropertyChanged($"WinPercent");
                NotifyPropertyChanged($"AverageGain");
                NotifyPropertyChanged($"AverageLoss");
                NotifyPropertyChanged($"MedianGain");
                NotifyPropertyChanged($"MedianLoss");
                NotifyPropertyChanged($"AverageExpectancy");
                NotifyPropertyChanged($"MedianExpectancy");
                NotifyPropertyChanged($"SortinoRatio");
                NotifyPropertyChanged($"AverageDrawdown");
                NotifyPropertyChanged($"AverageDrawdownWinners");
                NotifyPropertyChanged($"MedianDrawdown");
                NotifyPropertyChanged($"MedianDrawdownWinners");
            //});


        }

    }
}
