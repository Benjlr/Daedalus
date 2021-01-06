using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class SwingTests
    {
        [Fact]
        private void ShouldCalculateSwing() {

        }


    }

    //public sealed class Swings 
    //{

    //    private List<Session> sessionList { get; set; } = new List<Session>();
    //    private Pivot lastPivot { get; set; } = Pivot.None;
    //    private int lastIndex { get; set; }
    //    private double lastCost => lastPivot.Equals(Pivot.High) ? sessionList[lastIndex].High : sessionList[lastIndex].Low;

    //    //lastIndex IS BASED ON ARRAY POSITION NOT OXYPLOT POSITION
    //    protected override void Calculate(List<Session> tlist) {
    //        //Initialise
    //        sessionList = tlist;
    //        var StartPoint = (int)Params.Parameters[0] > tlist.Count - 1 ? 0 : tlist.Count - (int)Params.Parameters[0];
    //        var ReversalLimit = (double)Params.Parameters[1];
    //        var e = sessionList.First().Eval;
    //        var ReversalList = new List<Tuple<int, Reversal>>();
    //        var Pivs = Parent.RetrieveList<Pivot>(IndicatorName.Pivots, e);
    //        var baseAtrs = Parent.RetrieveList<double>(IndicatorName.Atr, e);
    //        var Atr = new List<double>();
    //        for (var i = 0; i < sessionList.Count; i++) Atr.Add(baseAtrs[i] * ReversalLimit);

    //        Parent.RetrieveList<Pivot>(IndicatorName.Pivots, e);

    //        //Calculate
    //        for (var i = StartPoint; i < Pivs.Count; i++)
    //            if (Pivs[i].Equals(Pivot.High) || Pivs[i].Equals(Pivot.Low)) {
    //                lastIndex = i;
    //                lastPivot = Pivs[i];
    //                break;
    //            }


    //        for (var i = lastIndex + 1; i < sessionList.Count; i++) {
    //            var currPiv = Pivs[i];

    //            if (currPiv.Equals(Pivot.Both)) {
    //                if (lastPivot.Equals(Pivot.High)) {
    //                    if (lastCost < sessionList[i].High) lastIndex = i;
    //                    if (sessionList[i].Low < lastCost - Atr[i]) {
    //                        ReversalList.Add(new Tuple<int, Reversal>(lastIndex, Reversal.Peak));
    //                        lastIndex = i;
    //                        lastPivot = Pivot.Low;
    //                    }
    //                }
    //                else if (lastPivot.Equals(Pivot.Low)) {
    //                    if (lastCost > sessionList[i].Low) lastIndex = i;
    //                    if (sessionList[i].High > Atr[i] + lastCost) {
    //                        ReversalList.Add(new Tuple<int, Reversal>(lastIndex, Reversal.Trough));
    //                        lastIndex = i;
    //                        lastPivot = Pivot.High;
    //                    }
    //                }
    //            }
    //            else if (currPiv.Equals(Pivot.High)) {
    //                if (lastPivot.Equals(Pivot.High)) {
    //                    if (lastCost < sessionList[i].High) lastIndex = i;
    //                }
    //                else if (lastPivot.Equals(Pivot.Low)) {
    //                    if (sessionList[i].High > Atr[i] + lastCost) {
    //                        ReversalList.Add(new Tuple<int, Reversal>(lastIndex, Reversal.Trough));
    //                        lastIndex = i;
    //                        lastPivot = Pivot.High;
    //                    }
    //                }
    //            }
    //            else if (currPiv.Equals(Pivot.Low)) {
    //                if (lastPivot.Equals(Pivot.High)) {
    //                    if (sessionList[i].Low < lastCost - Atr[i]) {
    //                        ReversalList.Add(new Tuple<int, Reversal>(lastIndex, Reversal.Peak));
    //                        lastIndex = i;
    //                        lastPivot = Pivot.Low;
    //                    }
    //                }
    //                else if (lastPivot.Equals(Pivot.Low)) {
    //                    if (lastCost > sessionList[i].Low) lastIndex = i;
    //                }
    //            }
    //        }

    //        if (ReversalList.Count > 0 && !ReversalList.Any(x => x.Item1.Equals(lastIndex))) ReversalList.Add(new Tuple<int, Reversal>(lastIndex, ReversalList.Last().Item2 == Reversal.Peak ? Reversal.Trough : Reversal.Peak));


    //        for (var i = 0; i < sessionList.Count; i++)
    //            if (ReversalList.FirstOrDefault(x => x.Item1.Equals(i)) == null)
    //                ReversalList.Add(new Tuple<int, Reversal>(i, Reversal.NotDefined));

    //        Parent.Update(EnumName, e, ReversalList);
    //    }


    //    public static void CalculateAlgoSwings(List<Session> input, int startPoint = 1000, double reversalLimit = 2.1, bool lookBack = true) {
    //        //if (StartPoint > Input.Count) StartPoint = Input.Count - 1;
    //        //var Templist = Input.Skip(Input.Count - StartPoint);

    //        //var firstOrDefault = Templist.FirstOrDefault(x => x.HighPivot > 0 || x.LowPivot > 0);

    //        //if (firstOrDefault != null && LookBack)
    //        //    {
    //        //        int z = firstOrDefault.Index - 1;
    //        //        if (z > -1)
    //        //        {
    //        //            if (Input[z].HighPivot > 0 && Input[z].LowPivot > 0)
    //        //                Input[z].ReversalPoint = Reversal.PeakTrough;
    //        //            else if (Input[z].HighPivot > 0) Input[z].ReversalPoint = Reversal.Peak;
    //        //            else if (Input[z].LowPivot > 0) Input[z].ReversalPoint = Reversal.Trough;


    //        //            while (z < Input.Count)
    //        //            {


    //        //            var last = Input.LastOrDefault(
    //        //                    x => (x.ReversalPoint.Equals(Reversal.Trough) ||
    //        //                          x.ReversalPoint.Equals(Reversal.Peak) ||
    //        //                          x.ReversalPoint.Equals(Reversal.PeakTrough)) && x.Index - 1 <= z);
    //        //                int direction = 0;
    //        //                if (last.ReversalPoint.Equals(Reversal.PeakTrough))
    //        //                {
    //        //                    var Lastlast =
    //        //                        Input.LastOrDefault(
    //        //                            x => (x.ReversalPoint.Equals(Reversal.Trough) ||
    //        //                                  x.ReversalPoint.Equals(Reversal.Peak)) && x.Index <= last.Index);
    //        //                    if (Lastlast != null && Lastlast.ReversalPoint.Equals(Reversal.Peak))
    //        //                        direction = 1;
    //        //                    else if (Lastlast != null && Lastlast.ReversalPoint.Equals(Reversal.Trough))
    //        //                        direction = -1;
    //        //                    else if (Lastlast == null)
    //        //                    {
    //        //                        if (last.UpBar) direction = 1;
    //        //                        else direction = -1;
    //        //                    }
    //        //                }
    //        //                else if (last.ReversalPoint.Equals(Reversal.Peak)) direction = 1;
    //        //                else if (last.ReversalPoint.Equals(Reversal.Trough)) direction = -1;


    //        //                var next = Input.FirstOrDefault(
    //        //                    x => (x.HighPivot > 0 || x.LowPivot > 0) && x.Index - 1 > z);
    //        //                if (next != null)
    //        //                {


    //        //                if (direction > 0)
    //        //                    {
    //        //                    if (next.LowPivot > 0)
    //        //                    {
    //        //                        if (last.High - next.Low > ReversalLimit * next.ATR) next.ReversalPoint = Reversal.Trough;
    //        //                    }

    //        //                        if (next.HighPivot > 0)
    //        //                        {


    //        //                        if (next.ReversalPoint.Equals(Reversal.Trough) &&
    //        //                                (next.High - next.Low > ReversalLimit * next.ATR))
    //        //                                next.ReversalPoint = Reversal.PeakTrough;
    //        //                            else
    //        //                            {
    //        //                                if (next.High >= last.High)
    //        //                                {
    //        //                                    if (last.ReversalPoint == Reversal.PeakTrough) last.ReversalPoint = Reversal.Trough;
    //        //                                    else last.ReversalPoint = Reversal.notDefined;

    //        //                                    next.ReversalPoint = Reversal.Peak;
    //        //                                }
    //        //                            }
    //        //                        }
    //        //                    }
    //        //                    if (direction < 0)
    //        //                    {
    //        //                        if (next.HighPivot > 0)
    //        //                        {
    //        //                        if (next.High - last.Low > ReversalLimit * next.ATR)
    //        //                                next.ReversalPoint = Reversal.Peak;
    //        //                        }

    //        //                        if (next.LowPivot > 0)
    //        //                        {

    //        //                        if (next.ReversalPoint.Equals(Reversal.Peak) &&
    //        //                                (next.High - next.Low > ReversalLimit * next.ATR))
    //        //                                next.ReversalPoint = Reversal.PeakTrough;
    //        //                            else
    //        //                            {
    //        //                                if (next.Low <= last.Low)
    //        //                                {
    //        //                                    if(last.ReversalPoint.Equals(Reversal.PeakTrough)) last.ReversalPoint = Reversal.Peak;
    //        //                                  else  last.ReversalPoint = Reversal.notDefined;
    //        //                                    next.ReversalPoint = Reversal.Trough;
    //        //                                }
    //        //                            }
    //        //                        }
    //        //                    }

    //        //                next.PotentialReversal = next.ReversalPoint;
    //        //                    z = next.Index - 1;
    //        //                }
    //        //                else z = Input.Count;
    //        //            }
    //        //        }
    //        //    }         
    //        //else if (firstOrDefault != null && !LookBack)
    //        //{
    //        //    int z = firstOrDefault.Index - 1;
    //        //    if (z > -1)
    //        //    {
    //        //        if (Input[z].HighPivot > 0 && Input[z].LowPivot > 0)
    //        //            Input[z].ReversalPoint = Reversal.PeakTrough;
    //        //        else if (Input[z].HighPivot > 0) Input[z].ReversalPoint = Reversal.Peak;
    //        //        else if (Input[z].LowPivot > 0) Input[z].ReversalPoint = Reversal.Trough;


    //        //        while (z < Input.Count)
    //        //        {
    //        //            var last = Input.LastOrDefault(
    //        //                x => (x.ReversalPoint.Equals(Reversal.Trough) ||
    //        //                      x.ReversalPoint.Equals(Reversal.Peak) ||
    //        //                      x.ReversalPoint.Equals(Reversal.PeakTrough)) && x.Index - 1 <= z);


    //        //            int direction = 0;
    //        //            if (last.ReversalPoint.Equals(Reversal.PeakTrough))
    //        //            {
    //        //                var Lastlast =
    //        //                    Input.LastOrDefault(x => (x.ReversalPoint.Equals(Reversal.Trough) ||
    //        //                                              x.ReversalPoint.Equals(Reversal.Peak)) &&
    //        //                                             x.Index <= last.Index);
    //        //                if (Lastlast != null && Lastlast.ReversalPoint.Equals(Reversal.Peak)) direction = 1;
    //        //                else if (Lastlast != null && Lastlast.ReversalPoint.Equals(Reversal.Trough))
    //        //                    direction = -1;
    //        //                else if (Lastlast == null)
    //        //                {
    //        //                    if (last.UpBar) direction = 1;
    //        //                    else direction = -1;
    //        //                }
    //        //            }
    //        //            else if (last.ReversalPoint.Equals(Reversal.Peak)) direction = 1;
    //        //            else if (last.ReversalPoint.Equals(Reversal.Trough)) direction = -1;


    //        //            var next = Input.FirstOrDefault(x => (x.HighPivot > 0 || x.LowPivot > 0) && x.Index - 1 > z);
    //        //            if (next != null)
    //        //            {
    //        //                if (direction > 0)
    //        //                {
    //        //                    if (next.LowPivot > 0)
    //        //                    {
    //        //                        if (last.High - next.Low > ReversalLimit * last.ATR)
    //        //                            next.ReversalPoint = Reversal.Trough;
    //        //                    }

    //        //                    if (next.HighPivot > 0)
    //        //                    {

    //        //                        if (next.ReversalPoint.Equals(Reversal.Trough) &&
    //        //                            (next.High - next.Low > ReversalLimit * last.ATR))
    //        //                            next.ReversalPoint = Reversal.PeakTrough;
    //        //                        else
    //        //                        {
    //        //                            if (next.High >= last.High)
    //        //                            {
    //        //                                last.ReversalPoint = last.ReversalPoint.Equals(Reversal.PeakTrough) ? Reversal.Trough : Reversal.notDefined;
    //        //                                next.ReversalPoint = Reversal.Peak;
    //        //                            }
    //        //                        }
    //        //                    }
    //        //                }
    //        //                if (direction < 0)
    //        //                {
    //        //                    if (next.HighPivot > 0)
    //        //                    {

    //        //                        if (next.High - last.Low > ReversalLimit * last.ATR)
    //        //                            next.ReversalPoint = Reversal.Peak;
    //        //                    }

    //        //                    if (next.LowPivot > 0)
    //        //                    {


    //        //                        if (next.ReversalPoint.Equals(Reversal.Peak) &&
    //        //                            (next.High - next.Low > ReversalLimit * last.ATR))
    //        //                            next.ReversalPoint = Reversal.PeakTrough;
    //        //                        else
    //        //                        {
    //        //                            if (next.Low <= last.Low)
    //        //                            {
    //        //                                last.ReversalPoint = last.ReversalPoint.Equals(Reversal.PeakTrough) ? Reversal.Peak : Reversal.notDefined;
    //        //                                next.ReversalPoint = Reversal.Trough;
    //        //                            }
    //        //                        }
    //        //                    }
    //        //                }
    //        //                next.PotentialReversal = next.ReversalPoint;

    //        //                z = next.Index - 1;
    //        //            }
    //        //            else z = Input.Count;
    //        //        }
    //        //    }
    //        //}
    //    }


    //    public static void DefineTrends(List<Session> input) {
    //        //List<Session> Swingers = DeepCopySwings(Input);
    //        //if (Swingers.Count < 4) return ;

    //        //TrendState CurrentState  = TrendState.notDefined;
    //        //int LastChange = 0;

    //        //for (int i = 3; i < Swingers.Count; i++)
    //        //{
    //        //    if (i == Swingers.Count - 1)
    //        //    {
    //        //        string a = "";
    //        //    }
    //        //    var First = Swingers[i - 3];
    //        //    var Second = Swingers[i - 2];
    //        //    var Third = Swingers[i - 1];
    //        //    var Fourth = Swingers[i];

    //        //    if (First.ReversalPoint.Equals(Reversal.Peak))
    //        //    {
    //        //        if (Third.High > First.High)
    //        //        {
    //        //            if (CurrentState.Equals(TrendState.DownTrend))
    //        //            {
    //        //                var Cuurr = Input.First(x => x.Index >= Second.Index && x.Index <= Third.Index &&
    //        //                                             x.High > First.High);
    //        //                Input.Where(x=>x.Index - 1 >= LastChange && x.Index <= Cuurr.Index ).ToList().ForEach(x => x.Trend = CurrentState);

    //        //                LastChange = Cuurr.Index - 1;
    //        //                CurrentState = TrendState.Range;
    //        //            }

    //        //            if (Second.Low < Fourth.Low)
    //        //            {
    //        //                if (CurrentState.Equals(TrendState.Range) || CurrentState.Equals(TrendState.notDefined))
    //        //                {
    //        //                    var Cuurr = Fourth;
    //        //                    Input.Where(x => x.Index - 1 >= LastChange && x.Index <= Cuurr.Index).ToList()
    //        //                        .ForEach(x => x.Trend = CurrentState);

    //        //                    LastChange = Cuurr.Index - 1;
    //        //                    CurrentState = TrendState.UpTrend;
    //        //                }
    //        //            }

    //        //        }

    //        //        if (Third.High < First.High)
    //        //        {
    //        //            if (CurrentState.Equals(TrendState.UpTrend))
    //        //            {
    //        //                var Cuurr = Third;
    //        //                Input.Where(x => x.Index - 1 >= LastChange && x.Index <= Cuurr.Index).ToList().ForEach(x => x.Trend = CurrentState);

    //        //                LastChange = Cuurr.Index - 1;
    //        //                CurrentState = TrendState.Range;
    //        //            }

    //        //            if (Second.Low > Fourth.Low)
    //        //            {
    //        //                if (CurrentState.Equals(TrendState.Range) || CurrentState.Equals(TrendState.notDefined))
    //        //                {
    //        //                    var Cuurr = Input.First(x => x.Index >= Third.Index && x.Index <= Fourth.Index && 
    //        //                             x.Low < Second.Low);
    //        //                    Input.Where(x => x.Index - 1 >= LastChange && x.Index <= Cuurr.Index).ToList().ForEach(x => x.Trend = CurrentState);

    //        //                    LastChange = Cuurr.Index - 1;
    //        //                    CurrentState = TrendState.DownTrend;
    //        //                }
    //        //            }
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        if (Third.Low > First.Low)
    //        //        {
    //        //            if (CurrentState.Equals(TrendState.DownTrend))
    //        //            {
    //        //                var Cuurr = Third;
    //        //                Input.Where(x => x.Index - 1 >= LastChange && x.Index <= Cuurr.Index).ToList().ForEach(x => x.Trend = CurrentState);

    //        //                LastChange = Cuurr.Index - 1;
    //        //                CurrentState = TrendState.Range;
    //        //            }

    //        //            if (Second.High < Fourth.High)
    //        //            {
    //        //                if (CurrentState.Equals(TrendState.Range) || CurrentState.Equals(TrendState.notDefined))
    //        //                {
    //        //                    var Cuurr = Input.First(x => x.Index >= Third.Index && x.Index <= Fourth.Index &&
    //        //                                                 x.High > Second.High);
    //        //                    Input.Where(x => x.Index - 1 >= LastChange && x.Index <= Cuurr.Index).ToList().ForEach(x => x.Trend = CurrentState);

    //        //                    LastChange = Cuurr.Index - 1;
    //        //                    CurrentState = TrendState.UpTrend;
    //        //                }
    //        //            }
    //        //        }

    //        //        if (Third.Low < First.Low)
    //        //        {
    //        //            if (CurrentState.Equals(TrendState.UpTrend))
    //        //            {
    //        //                var Cuurr = Input.First(x => x.Index >= Second.Index && x.Index <= Third.Index &&
    //        //                                             x.Low < First.Low);
    //        //                Input.Where(x => x.Index - 1 >= LastChange && x.Index <= Cuurr.Index).ToList().ForEach(x => x.Trend = CurrentState);

    //        //                LastChange = Cuurr.Index - 1;
    //        //                CurrentState = TrendState.Range;
    //        //            }

    //        //            if (Second.High > Fourth.High)
    //        //            {
    //        //                if (CurrentState.Equals(TrendState.Range) || CurrentState.Equals(TrendState.notDefined))
    //        //                {
    //        //                    var Cuurr = Fourth;
    //        //                    Input.Where(x => x.Index - 1 >= LastChange && x.Index <= Cuurr.Index).ToList().ForEach(x => x.Trend = CurrentState);

    //        //                    LastChange = Cuurr.Index - 1;
    //        //                    CurrentState = TrendState.DownTrend;
    //        //                }
    //        //            }

    //        //        }
    //        //    }

    //        //}

    //        //Input.Where(x => x.Index - 1 >= LastChange && x.Index <= Swingers.Last().Index).ToList().ForEach(x => x.Trend = CurrentState);
    //    }


    //    public static TrendState DefineRegion(List<Session> inputs) {
    //        //if(!Inputs.Any(x => x.ReversalPoint.Equals(Reversal.Peak) ||
    //        //                    x.ReversalPoint.Equals(Reversal.Trough) ||
    //        //                    x.ReversalPoint.Equals(Reversal.PeakTrough))) return TrendState.Range;

    //        //var t = Inputs.Where(x => x.ReversalPoint.Equals(Reversal.Peak) ||
    //        //                          x.ReversalPoint.Equals(Reversal.Trough) ||
    //        //                          x.ReversalPoint.Equals(Reversal.PeakTrough)).ToList();

    //        //int last = t.Count - 1;

    //        //if(last < 3) return TrendState.Range;
    //        //if (Inputs[last].ReversalPoint.Equals(Reversal.Peak))
    //        //{
    //        //    if (Inputs[last].High > Inputs[last-2].High && Inputs[last - 1].Low > Inputs[last - 3].Low) return TrendState.UpTrend;
    //        //    if (Inputs[last].High < Inputs[last-2].High && Inputs[last - 1].Low < Inputs[last - 3].Low ) return TrendState.DownTrend;
    //        //    else return TrendState.Range;
    //        //}
    //        //else
    //        //{
    //        //    if (Inputs[last].Low > Inputs[last - 2].Low  && Inputs[last-1].High > Inputs[last - 3].High) return TrendState.UpTrend;
    //        //    if (Inputs[last].Low < Inputs[last-2].Low && Inputs[last-1].High < Inputs[last - 3].High) return TrendState.DownTrend;
    //        //    else return TrendState.Range;
    //        //}

    //        return TrendState.NotDefined;
    //    }

    //    public enum TrendState
    //    {
    //        NotDefined,
    //        UpTrend,
    //        DownTrend,
    //        Range
    //    }
    //}

}
