using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using SysMath = System.Math;
using REAL = System.Double;

namespace Essence.Maths.Double.Curves
{
    public class Line1 : SimpleCurve1
    {
        public Line1(REAL p0, REAL p1, REAL t0, REAL t1)
        {
            this.p0 = p0;
            this.p1 = p1;

            this.len = new Point2d(t0, p0).DistanceTo(new Point2d(t1, p1));

            this.SetTInterval(t0, t1);
        }

        #region ICurve2

        public override REAL TMin
        {
            get { return this.tmin; }
        }

        public override REAL TMax
        {
            get { return this.tmax; }
        }

        public override void SetTInterval(REAL tmin, REAL tmax)
        {
            this.tmin = tmin;
            this.tmax = tmax;
            this.ttransform = new Transform1(this.tmin, this.tmax, 0, 1);
        }

        public override REAL GetPosition(REAL t)
        {
            /*
             *  s:      t * m + n;
             *  x:      p0_x + (p1_x - p0_x) * s;
             *  result: [ x ];
             *
             *  result: (p1_x - p0_x) * (m * t+n) + p0_x
             */
            double t01 = this.GetT01(t);
            return this.Evaluate01(t01);
        }

        public override REAL GetFirstDerivative(REAL t)
        {
            /*
             *  s:      t * m + n;
             *  x:      p0_x + (p1_x - p0_x) * s;
             *  result: diff([ x ], t, 1);
             *
             *  result: [ m * (p1_x - p0_x) ]
             */
            REAL m = this.ttransform.A;

            return (this.p1 - this.p0) * m;
        }

        public override REAL GetSecondDerivative(REAL t)
        {
            /*
             *  s:      t * m + n;
             *  x:      p0_x + (p1_x - p0_x)*s;
             *  result: diff([ x ], t, 2);
             *
             *  result: [ 0 ]
             */
            return 0;
        }

        public override REAL GetThirdDerivative(REAL t)
        {
            /*
             *  s:      t * m + n;
             *  x:      p0_x +(p1_x - p0_x)*s;
             *  result: diff([ x ], t, 3);
             *
             *  result: [ 0 ]
             */
            return 0;
        }

        public override REAL TotalLength
        {
            get { return this.len; }
        }

        public override REAL GetLength(REAL t0, REAL t1)
        {
            REAL t01_0 = this.GetT01(t0);
            REAL t01_1 = this.GetT01(t1);
            return SysMath.Abs(t01_1 - t01_0) * this.TotalLength;
        }

        #endregion

        #region private

        private REAL GetT01(REAL t)
        {
            t = t.Clamp(this.TMin, this.TMax);
            REAL t01 = this.ttransform.Get(t);
            return t01;
        }

        private REAL Evaluate01(REAL t01)
        {
            return this.p0 + (this.p1 - this.p0) * t01;
        }

        private readonly REAL p0;
        private readonly REAL p1;

        private REAL tmin;
        private REAL tmax;

        private readonly REAL len;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        private Transform1 ttransform;

        #endregion
    }
}