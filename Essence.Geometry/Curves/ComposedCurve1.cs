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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Curves
{
    public class ComposedCurve1 : MultiCurve1
    {
        public IEnumerable<ICurve1> GetSegments()
        {
            return this.segments;
        }

        public void Add(ICurve1 curve)
        {
            if (this.segments.Count == 0)
            {
                this.segments.Add(curve);
            }
            else
            {
                ICurve1 last = this.segments[this.segments.Count - 1];
                double tmin = last.TMax;
                double tlen = curve.TMax - curve.TMin;
                curve.SetTInterval(tmin, tmin + tlen);
                this.segments.Add(curve);
            }
        }

        public void SetClosed(bool closed)
        {
            this.closed = closed;
        }

        #region MultiCurve1

        public override int SegmentsCount
        {
            get { return this.segments.Count; }
        }

        public override double GetTMin(int indice)
        {
            return this.segments[indice].TMin;
        }

        public override double GetTMax(int indice)
        {
            return this.segments[indice].TMax;
        }

        #region Position and derivatives

        protected override double GetPosition(int index, double tInSegment)
        {
            return this.segments[index].GetPosition(tInSegment);
        }

        protected override double GetFirstDerivative(int index, double tInSegment)
        {
            return this.segments[index].GetFirstDerivative(tInSegment);
        }

        protected override double GetSecondDerivative(int index, double tInSegment)
        {
            return this.segments[index].GetSecondDerivative(tInSegment);
        }

        protected override double GetThirdDerivative(int index, double tInSegment)
        {
            return this.segments[index].GetThirdDerivative(tInSegment);
        }

        #endregion

        #region Differential geometric quantities

        protected override double GetLength(int index, double tInSegment0, double tInSegment1)
        {
            return this.segments[index].GetLength(tInSegment0, tInSegment1);
        }

        protected override double GetSpeed(int index, double tInSegment)
        {
            return this.segments[index].GetSpeed(tInSegment);
        }

        #endregion

        protected override void FindIndex(double t, out int index, out double tInSegment)
        {
            index = this.segments.BinarySearch(new CurveForSearch(t), CurveComparer.Instance);
            if (index < 0)
            {
                index = ~index;
                index--;
            }
            index = Essence.Util.Math.Int.MathUtils.Clamp(index, 0, this.segments.Count - 1);
            Contract.Assert(t.EpsilonL(this.TMin) || t.EpsilonG(this.TMax) || (t.EpsilonGE(this.GetTMin(index)) && t.EpsilonLE(this.GetTMax(index))));

            tInSegment = t;
        }

        protected override BoundingBox1d GetBoundingBox(int indice)
        {
            return this.segments[indice].BoundingBox;
        }

        #endregion

        #region ICurve1

        #endregion

        #region private

        private bool closed = false;
        private readonly List<ICurve1> segments = new List<ICurve1>();

        #endregion

        #region Inner clases

        private sealed class CurveComparer : IComparer<ICurve1>
        {
            public static readonly CurveComparer Instance = new CurveComparer();

            public int Compare(ICurve1 x, ICurve1 y)
            {
                return x.TMin.CompareTo(y.TMin);
            }
        }

        private sealed class CurveForSearch : ICurve1
        {
            public CurveForSearch(double t)
            {
                this.TMin = t;
            }

            public double TMin { get; private set; }

            double ICurve1.TMax
            {
                get { throw new NotImplementedException(); }
            }

            void ICurve1.SetTInterval(double tmin, double tmax)
            {
                throw new NotImplementedException();
            }

            double ICurve1.GetPosition(double t)
            {
                throw new NotImplementedException();
            }

            double ICurve1.GetFirstDerivative(double t)
            {
                throw new NotImplementedException();
            }

            double ICurve1.GetSecondDerivative(double t)
            {
                throw new NotImplementedException();
            }

            double ICurve1.GetThirdDerivative(double t)
            {
                throw new NotImplementedException();
            }

            double ICurve1.TotalLength
            {
                get { throw new NotImplementedException(); }
            }

            double ICurve1.GetLength(double t0, double t1)
            {
                throw new NotImplementedException();
            }

            double ICurve1.GetSpeed(double t)
            {
                throw new NotImplementedException();
            }

            BoundingBox1d ICurve1.BoundingBox
            {
                get { return BoundingBox1d.Empty; }
            }
        }

        #endregion
    }
}