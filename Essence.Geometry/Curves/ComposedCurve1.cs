using System;
using System.Collections.Generic;
using REAL = System.Double;

namespace Essence.Maths.Double.Curves
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
                REAL tmin = last.TMax;
                REAL tlen = curve.TMax - curve.TMin;
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

        public override REAL GetTMin(int indice)
        {
            return this.segments[indice].TMin;
        }

        public override REAL GetTMax(int indice)
        {
            return this.segments[indice].TMax;
        }

        #region Position and derivatives

        protected override REAL GetPosition(int index, REAL tInSegment)
        {
            return this.segments[index].GetPosition(tInSegment);
        }

        protected override REAL GetFirstDerivative(int index, REAL tInSegment)
        {
            return this.segments[index].GetFirstDerivative(tInSegment);
        }

        protected override REAL GetSecondDerivative(int index, REAL tInSegment)
        {
            return this.segments[index].GetSecondDerivative(tInSegment);
        }

        protected override REAL GetThirdDerivative(int index, REAL tInSegment)
        {
            return this.segments[index].GetThirdDerivative(tInSegment);
        }

        #endregion

        #region Differential geometric quantities

        protected override REAL GetLength(int index, REAL tInSegment0, REAL tInSegment1)
        {
            return this.segments[index].GetLength(tInSegment0, tInSegment1);
        }

        protected override REAL GetSpeed(int index, REAL tInSegment)
        {
            return this.segments[index].GetSpeed(tInSegment);
        }

        #endregion

        protected override void FindIndex(REAL t, out int index, out REAL tInSegment)
        {
            index = this.segments.BinarySearch(new CurveForSearch(t), CurveComparer.Instance);
            if (index < 0)
            {
                index = ~index;
            }
            index = Essence.Util.Math.Int.MathUtils.Clamp(index, 0, this.segments.Count - 1);

            tInSegment = t;
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
            public CurveForSearch(REAL t)
            {
                this.TMin = t;
            }

            public REAL TMin { get; private set; }

            REAL ICurve1.TMax
            {
                get { throw new NotImplementedException(); }
            }

            void ICurve1.SetTInterval(REAL tmin, REAL tmax)
            {
                throw new NotImplementedException();
            }

            REAL ICurve1.GetPosition(REAL t)
            {
                throw new NotImplementedException();
            }

            REAL ICurve1.GetFirstDerivative(REAL t)
            {
                throw new NotImplementedException();
            }

            REAL ICurve1.GetSecondDerivative(REAL t)
            {
                throw new NotImplementedException();
            }

            REAL ICurve1.GetThirdDerivative(REAL t)
            {
                throw new NotImplementedException();
            }

            REAL ICurve1.TotalLength
            {
                get { throw new NotImplementedException(); }
            }

            REAL ICurve1.GetLength(REAL t0, REAL t1)
            {
                throw new NotImplementedException();
            }

            REAL ICurve1.GetSpeed(REAL t)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}