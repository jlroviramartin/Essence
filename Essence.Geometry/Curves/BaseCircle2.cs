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
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Maths.Double.Curves
{
    public abstract class BaseCircle2 : SimpleCurve2
    {
        public BaseCircle2(Point2d center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public Point2d Center
        {
            get { return this.center; }
        }

        public double Radius
        {
            get { return this.radius; }
        }

        public double GetAngle(double t)
        {
            t = t.Clamp(this.TMin, this.TMax);
            double a = this.ttransform.Get(t);
            return a;
        }

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
            this.ttransform = new Transform1(this.TMin, this.TMax, 0, 2 * SysMath.PI);
        }

        public override Point2d GetPosition(double t)
        {
            /*
             *  Maxima:
             *  s:      t * m + n;
             *  x:      r*cos(r);
             *  y:      r*sin(r);
             *  z:      1;
             *  result: [ x, y, z ];
             *  
             *  result: [r*cos(r),r*sin(r),1]
             */
            double angle = this.GetAngle(t);
            double x = this.Radius * SysMath.Cos(angle);
            double y = this.Radius * SysMath.Sin(angle);
            return this.Center.Add(new Vector2d(x, y));
        }

        public override Vector2d GetFirstDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:      t * m + n;
             *  x:      r*cos(r);
             *  y:      r*sin(r);
             *  z:      1;
             *  result: diff([ x, y, z ], t, 1);
             *  
             *  result: [-m*r*sin(m*t+n),m*r*cos(m*t+n),0]
             */
            double m = this.ttransform.A;

            double angle = this.GetAngle(t);
            double x = -this.Radius * SysMath.Sin(angle);
            double y = this.Radius * SysMath.Cos(angle);
            return new Vector2d(m * x, m * y);
        }

        public override Vector2d GetSecondDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:      t * m + n;
             *  x:      r*cos(r);
             *  y:      r*sin(r);
             *  z:      1;
             *  result: diff([ x, y, z ], t, 2);
             *  
             *  result: [-m^2*r*cos(m*t+n),-m^2*r*sin(m*t+n),0]
             */
            double m = this.ttransform.A;
            double m2 = m * m;

            double angle = this.GetAngle(t);
            double x = -this.Radius * SysMath.Cos(angle);
            double y = -this.Radius * SysMath.Sin(angle);
            return new Vector2d(m2 * x, m2 * y);
        }

        public override Vector2d GetThirdDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:      t * m + n;
             *  x:      r*cos(r);
             *  y:      r*sin(r);
             *  z:      1;
             *  result: diff([ x, y, z ], t, 3);
             *  
             *  result: [m^3*r*sin(m*t+n),-m^3*r*cos(m*t+n),0]
             */
            double m = this.ttransform.A;
            double m3 = m * m * m;

            double angle = this.GetAngle(t);
            double x = this.Radius * SysMath.Sin(angle);
            double y = -this.Radius * SysMath.Cos(angle);
            return new Vector2d(m3 * x, m3 * y);
        }

        public override double GetLength(double t0, double t1)
        {
            double a0 = this.GetAngle(t0);
            double a1 = this.GetAngle(t1);
            return AngleUtils.Diff(a0, a1) * this.Radius;
        }

        #endregion

        #region private

        private readonly Point2d center;
        private readonly double radius;

        protected double tmin;
        protected double tmax;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        protected Transform1 ttransform;

        #endregion
    }
}