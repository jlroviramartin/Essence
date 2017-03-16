#region License

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

#endregion

using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Maths.Double.Curves
{
    public class Line2 : SimpleCurve2
    {
        public Line2(Point2d p0, Point2d p1)
            : this(p0, 0, p1, p0.DistanceTo(p1))
        {
        }

        public Line2(Point2d p0, double t0, Point2d p1, double t1)
        {
            this.p0 = p0;
            this.p1 = p1;

            this.v01 = this.p1.Sub(this.p0);
            this.len = this.v01.Length;
            this.dirN = this.v01.Div(this.len);

            this.SetTInterval(t0, t1);
        }

        public double Project01(Point2d p)
        {
            Vector2d diff = p.Sub(this.p0);
            //return this.dir.Dot(diff) / this.len;
            return this.dirN.Dot(diff);
        }

        #region ICurve2

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
            get { return this.len; }
        }

        public override double GetLength(double t0, double t1)
        {
            double t01_0 = this.GetT01(t0);
            double t01_1 = this.GetT01(t1);
            return SysMath.Abs(t01_1 - t01_0) * this.TotalLength;
        }

        #endregion

        #region private

        private double GetT01(double t)
        {
            t = t.Clamp(this.TMin, this.TMax);
            double t01 = this.ttransform.Get(t);
            return t01;
        }

        private Point2d Evaluate01(double t01)
        {
            //return this.p0.Add(this.dirN.Mul(t01 * this.len));
            return this.p0.Add(this.v01.Mul(t01));
        }

        private readonly Point2d p0;
        private readonly Point2d p1;

        private double tmin;
        private double tmax;

        private readonly Vector2d v01; // p1 - p0
        private readonly double len;
        private readonly Vector2d dirN;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        private Transform1 ttransform;

        #endregion
    }
}