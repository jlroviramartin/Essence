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

using Essence.Geometry.Core.Double;
using System;
using System.Collections.Generic;

namespace Essence.Geometry.Curves
{
    public class ComposedCurve2 : MultiCurve2
    {
        public ComposedCurve2(IEnumerable<ICurve2> segments)
            : base(EvaluateTimes(segments))
        {
            foreach (ICurve2 segment in segments)
            {
                segment.SetTInterval(0, segment.TMax - segment.TMin);
                this.segments.Add(segment);
            }
        }

        public IEnumerable<ICurve2> GetSegments()
        {
            return this.segments;
        }

        /*public void Add(ICurve2 curve)
        {
            if (this.segments.Count == 0)
            {
                this.segments.Add(curve);
            }
            else
            {
                ICurve2 last = this.segments[this.segments.Count - 1];
                double tmin = last.TMax;
                double tlen = curve.TMax - curve.TMin;
                curve.SetTInterval(tmin, tmin + tlen);
                this.segments.Add(curve);
            }
        }*/

        public void SetClosed(bool closed)
        {
            this.closed = closed;
        }

        #region MultiCurve2

        /*public override int NumSegments
        {
            get { return this.segments.Count; }
        }

        public override double GetTMin(int key)
        {
            if (this.segments.Count == 0)
            {
                return 0;
            }
            return this.segments[key].TMin;
        }

        public override double GetTMax(int key)
        {
            if (this.segments.Count == 0)
            {
                return 0;
            }
            return this.segments[key].TMax;
        }*/

        #region Position and derivatives

        protected override Point2d GetPosition(int key, double dt)
        {
            return this.segments[key].GetPosition(dt);
        }

        protected override Vector2d GetFirstDerivative(int key, double dt)
        {
            return this.segments[key].GetFirstDerivative(dt);
        }

        protected override Vector2d GetSecondDerivative(int key, double dt)
        {
            return this.segments[key].GetSecondDerivative(dt);
        }

        protected override Vector2d GetThirdDerivative(int key, double dt)
        {
            return this.segments[key].GetThirdDerivative(dt);
        }

        #endregion

        #region Differential geometric quantities

        protected override double GetLength(int key, double dt0, double dt1)
        {
            return this.segments[key].GetLength(dt0, dt1);
        }

        protected override double GetSpeed(int key, double dt)
        {
            return this.segments[key].GetSpeed(dt);
        }

        protected override Vector2d GetTangent(int key, double dt)
        {
            return this.segments[key].GetTangent(dt);
        }

        protected override Vector2d GetLeftNormal(int key, double dt)
        {
            return this.segments[key].GetLeftNormal(dt);
        }

        protected override void GetFrame(int key, double dt,
                                         out Point2d position, out Vector2d tangent, out Vector2d normal)
        {
            this.segments[key].GetFrame(dt, out position, out tangent, out normal);
        }

        protected override double GetCurvature(int key, double dt)
        {
            return this.segments[key].GetCurvature(dt);
        }

        #endregion

        /*protected override void GetKeyInfo(double t, out int key, out double dt)
        {
            key = this.segments.BinarySearch(new CurveForSearch(t), CurveComparer.Instance);
            if (key < 0)
            {
                key = ~key;
                key--;
            }
            key = Essence.Util.Math.Int.MathUtils.Clamp(key, 0, this.segments.Count - 1);
            Contract.Assert(t.EpsilonL(this.TMin) || t.EpsilonG(this.TMax) || (t.EpsilonGE(this.GetTMin(key)) && t.EpsilonLE(this.GetTMax(key))));

            dt = t;
        }*/

        protected override BoundingBox2d GetBoundingBox(int key)
        {
            return this.segments[key].BoundingBox;
        }

        #endregion

        #region ICurve2

        public override bool IsClosed
        {
            get { return this.closed; }
        }

        #endregion

        #region private

        private static IEnumerable<double> EvaluateTimes(IEnumerable<ICurve2> segments)
        {
            double t = 0;
            yield return t;

            foreach (ICurve2 curve in segments)
            {
                t += curve.TMax - curve.TMin;
                yield return t;
            }
        }

        private bool closed = false;
        private readonly List<ICurve2> segments = new List<ICurve2>();

        #endregion

        #region Inner clases

        private sealed class CurveComparer : IComparer<ICurve2>
        {
            public static readonly CurveComparer Instance = new CurveComparer();

            public int Compare(ICurve2 x, ICurve2 y)
            {
                return x.TMin.CompareTo(y.TMin);
            }
        }

        private sealed class CurveForSearch : ICurve2
        {
            public CurveForSearch(double t)
            {
                this.TMin = t;
            }

            bool ICurve2.IsClosed
            {
                get { throw new NotImplementedException(); }
            }

            public double TMin { get; }

            double ICurve2.TMax
            {
                get { throw new NotImplementedException(); }
            }

            void ICurve2.SetTInterval(double tmin, double tmax)
            {
                throw new NotImplementedException();
            }

            double ICurve2.GetT(double length, int iterations, double tolerance)
            {
                throw new NotImplementedException();
            }

            Point2d ICurve2.GetPosition(double t)
            {
                throw new NotImplementedException();
            }

            Vector2d ICurve2.GetFirstDerivative(double t)
            {
                throw new NotImplementedException();
            }

            Vector2d ICurve2.GetSecondDerivative(double t)
            {
                throw new NotImplementedException();
            }

            Vector2d ICurve2.GetThirdDerivative(double t)
            {
                throw new NotImplementedException();
            }

            double ICurve2.TotalLength
            {
                get { throw new NotImplementedException(); }
            }

            double ICurve2.GetLength(double t0, double t1)
            {
                throw new NotImplementedException();
            }

            double ICurve2.GetSpeed(double t)
            {
                throw new NotImplementedException();
            }

            double ICurve2.GetCurvature(double t)
            {
                throw new NotImplementedException();
            }

            Vector2d ICurve2.GetLeftNormal(double t)
            {
                throw new NotImplementedException();
            }

            Vector2d ICurve2.GetTangent(double t)
            {
                throw new NotImplementedException();
            }

            void ICurve2.GetFrame(double t, out Point2d position, out Vector2d tangent, out Vector2d normal)
            {
                throw new NotImplementedException();
            }

            BoundingBox2d ICurve2.BoundingBox
            {
                get { return BoundingBox2d.Empty; }
            }
        }

        #endregion
    }
}