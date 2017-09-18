// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Curves
{
    public class CurveEvaluator2
    {
        public CurveEvaluator2(ICurve2 curve, double tinc)
        {
            this.curve = curve;
            this.tinc = tinc;
        }

        private readonly ICurve2 curve;
        private readonly double tinc;

        public IEnumerable<double> EvaluateTimes()
        {
            return GetTimes(this.curve, this.tinc);
        }

        public IEnumerable<Point2d> Evaluate()
        {
            return GetTimes(this.curve, this.tinc).Select(time => this.curve.GetPosition(time));

            /*using (IEnumerator<double> enumerator = this.EvaluateTimes().GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    yield break;
                }

                Point2d p = this.curve.GetPosition(enumerator.Current);
                p = new Point2d(MathUtils.Round(p.X, 3), MathUtils.Round(p.Y, 3));
                yield return p;
                Point2d pprev = p;

                while (enumerator.MoveNext())
                {
                    p = this.curve.GetPosition(enumerator.Current);
                    p = new Point2d(MathUtils.Round(p.X, 3), MathUtils.Round(p.Y, 3));
                    if (!p.EpsilonEquals(pprev))
                    {
                        yield return p;
                        pprev = p;
                    }
                }
            }*/
        }

        private static IEnumerable<double> GetTimes(ICurve2 curve, double tinc)
        {
            if (curve is MultiCurve2)
            {
                TimeList times = new TimeList();
                MultiCurve2 multi = (MultiCurve2)curve;
                for (int i = 0; i < multi.SegmentsCount; i++)
                {
                    double tmin = multi.GetTMin(i);
                    double tmax = multi.GetTMax(i);
                    double t = tmin;
                    while (t < tmax)
                    {
                        times.Add(t);
                        t += tinc;
                    }
                    times.Add(tmax);
                }
                return times.GetTimes();
            }
            else if (curve is DisplacedCurve2)
            {
                DisplacedCurve2 displaced = (DisplacedCurve2)curve;
                return MathUtils.ConcatSorted(GetTimes(displaced.Curve, tinc), GetTimes(displaced.Displacement, tinc));
            }
            else
            {
                TimeList times = new TimeList();
                double tmin = curve.TMin;
                double tmax = curve.TMax;
                double t = tmin;
                while (t < tmax)
                {
                    times.Add(t);
                    t += tinc;
                }
                times.Add(tmax);
                return times.GetTimes();
            }
        }

        private static IEnumerable<double> GetTimes(ICurve1 curve, double tinc)
        {
            if (curve is MultiCurve1)
            {
                TimeList times = new TimeList();
                MultiCurve1 multi = (MultiCurve1)curve;
                for (int i = 0; i < multi.SegmentsCount; i++)
                {
                    double tmin = multi.GetTMin(i);
                    double tmax = multi.GetTMax(i);
                    double t = tmin;
                    while (t < tmax)
                    {
                        times.Add(t);
                        t += tinc;
                    }
                    times.Add(tmax);
                }
                return times.GetTimes();
            }
            else
            {
                TimeList times = new TimeList();
                double tmin = curve.TMin;
                double tmax = curve.TMax;
                double t = tmin;
                while (t < tmax)
                {
                    times.Add(t);
                    t += tinc;
                }
                times.Add(tmax);
                return times.GetTimes();
            }
        }

        private class TimeList
        {
            public void Add(double t)
            {
                //t = MathUtils.Round(t, 3);
                t = SysMath.Round(t, 3);
                if (this.times.Count == 0 || this.times[this.times.Count - 1].EpsilonL(t))
                {
                    this.times.Add(t);
                }
            }

            public IEnumerable<double> GetTimes()
            {
                return this.times;
            }

            private readonly List<double> times = new List<double>();
        }
    }
}