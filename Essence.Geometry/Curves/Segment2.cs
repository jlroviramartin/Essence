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
using Essence.Maths.Double;
using Essence.Util.Math.Double;
using java.lang;

namespace Essence.Geometry.Curves
{
    /**
     *
     * @author joseluis
     */
    public class Segment2 : SimpleCurve2
    {
        public Segment2(Point2d p0, Point2d p1)
            : this(p0, 0, p1, p0.DistanceTo(p1))
        {
        }

        public Segment2(Point2d p0, double t0, Point2d p1, double t1)
        {
            this.p0 = p0;
            this.p1 = p1;

            this.v01 = this.p1.Sub(this.p0);
            this.length = this.v01.Length;
            this.directionUnit = this.v01.Div(this.length);

            this.SetTInterval(t0, t1);
        }

        public Vector2d Direction
        {
            get { return this.directionUnit; }
        }

        public double Length
        {
            get { return this.length; }
        }

        /**
         * This method projects the point <code>p</code> on the segment <code>p0:tMin to p1:tMax</code> and return the parameter t.
         *
         * @param p Point.
         * @param clamp This parameter indicates if the method clamps the return parameter to [tMin, tMax].
         * @return Parameter.
         */
        public double Project(Point2d p, bool clamp)
        {
            double t01 = this.Project01(p);
            double t = this.ttransform.GetInverse(t01);
            if (clamp)
            {
                t = MathUtils.Clamp(t, this.TMin, this.TMax);
            }
            return t;
        }

        /**
         * This method projects the point <code>p</code> on the segment <code>p0:0 to p1:length</code> and return the parameter t.
         *
         * @param p Point.
         * @return Parameter.
         */
        public double Project0L(Point2d p)
        {
            Vector2d diff = p.Sub(this.p0);
            return this.directionUnit.Dot(diff);
        }

        /**
         * This methods evaluates the parameter <code>t0L</code> on the segment <code>p0:0 to p1:length</code> and returns the point.
         *
         * @param t0L Parameter.
         * @return Point.
         */
        public Point2d Evaluate0L(double t0L)
        {
            return this.p0.Add(this.directionUnit.Mul(t0L));
        }

        /**
         * This method projects the point <code>p</code> on the segment <code>p0:0 to p1:1</code> and return the parameter t.
         *
         * @param p Point.
         * @return Parameter.
         */
        public double Project01(Point2d p)
        {
            return this.Project0L(p) / this.Length;
        }

        /**
         * This methods evaluates the parameter <code>t01</code> on the segment <code>p0:0 to p1:1</code> and returns the point.
         *
         * @param t0L Parameter.
         * @return Point.
         */
        public Point2d Evaluate01(double t01)
        {
            return this.p0.Add(this.v01.Mul(t01));
        }

        #region private

        private double GetT01(double t)
        {
            t = MathUtils.Clamp(t, this.TMin, this.TMax);
            double t01 = this.ttransform.Get(t);
            return t01;
        }

        private readonly Point2d p0;
        private readonly Point2d p1;

        private double tmin;
        private double tmax;

        private readonly Vector2d v01; // p1 - p0
        private readonly double length;
        private readonly Vector2d directionUnit;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        private Transform1 ttransform;

        #endregion

        #region Curve2

        public override double TMin
        {
            get { return this.tmin; }
        }

        public override double TMax
        {
            get { return this.tmax; }
        }

        public override void SetTInterval(double tmin, double tmax)
        {
            this.tmin = tmin;
            this.tmax = tmax;
            this.ttransform = new Transform1(this.tmin, this.tmax, 0, 1);
        }

        public override Point2d GetPosition(double t)
        {
            /*
             *  s:      t * m + n;
             *  x:      p0_x +(p1_x - p0_x)*s;
             *  y:      p0_y +(p1_y - p0_y)*s;
             *  z:      1;
             *  result: [ x, y, z ];
             *
             *  result: [(p1_x-p0_x)*(m*t+n)+p0_x,(p1_y-p0_y)*(m*t+n)+p0_y,1]
             */
            double t01 = this.GetT01(t);
            return this.Evaluate01(t01);
        }

        public override Vector2d GetFirstDerivative(double t)
        {
            /*
             *  s:      t * m + n;
             *  x:      p0_x +(p1_x - p0_x)*s;
             *  y:      p0_y +(p1_y - p0_y)*s;
             *  z:      1;
             *  result: diff([ x, y, z ], t, 1);
             *
             *  result: [m*(p1_x-p0_x),m*(p1_y-p0_y),0]
             */
            double m = this.ttransform.A;

            Vector2d v = this.v01;
            v = v.Mul(m);
            return v;
        }

        public override Vector2d GetSecondDerivative(double t)
        {
            /*
             *  s:      t * m + n;
             *  x:      p0_x +(p1_x - p0_x)*s;
             *  y:      p0_y +(p1_y - p0_y)*s;
             *  z:      1;
             *  result: diff([ x, y, z ], t, 2);
             *
             *  result: [0,0,0]
             */
            return Vector2d.Zero;
        }

        public override Vector2d GetThirdDerivative(double t)
        {
            /*
             *  s:      t * m + n;
             *  x:      p0_x +(p1_x - p0_x)*s;
             *  y:      p0_y +(p1_y - p0_y)*s;
             *  z:      1;
             *  result: diff([ x, y, z ], t, 3);
             *
             *  result: [0,0,0]
             */
            return Vector2d.Zero;
        }

        public override double TotalLength
        {
            get { return this.length; }
        }

        public override double GetLength(double t0, double t1)
        {
            double t01_0 = this.GetT01(t0);
            double t01_1 = this.GetT01(t1);
            return Math.abs(t01_1 - t01_0) * this.TotalLength;
        }

        #endregion
    }
}