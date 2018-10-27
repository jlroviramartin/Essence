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

using System.Diagnostics.Contracts;
using Essence.Geometry.Core.Double;
using Essence.Maths;
using Essence.Maths.Double;
using Essence.Util.Math.Double;
using SysMath = System.Math;
using UnaryFunction = System.Func<double, double>;

namespace Essence.Geometry.Curves
{
    public abstract class SimpleCurve2 : ICurve2
    {
        public Point2d Point0
        {
            get { return this.GetPosition(this.TMin); }
        }

        public Point2d Point1
        {
            get { return this.GetPosition(this.TMax); }
        }

        #region private

        /// <summary>Número máximo de evaluaciones para el cálculo de la integral.</summary>
        private const int IntegralMaxEval = 1000;

        #endregion

        #region ICurve2

        public virtual bool IsClosed
        {
            get { return false; }
        }

        public virtual double TMin
        {
            get { return 0; }
        }

        public virtual double TMax
        {
            get { return 1; }
        }

        public virtual void SetTInterval(double tmin, double tmax)
        {
        }

        #region Position and derivatives

        public abstract Point2d GetPosition(double t);

        public virtual Vector2d GetFirstDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 1, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 1, 5);
            return new Vector2d(fdx(t), fdy(t));
        }

        public virtual Vector2d GetSecondDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 2, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 2, 5);
            return new Vector2d(fdx(t), fdy(t));
        }

        public virtual Vector2d GetThirdDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 3, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 3, 5);
            return new Vector2d(fdx(t), fdy(t));
        }

        #endregion

        #region Differential geometric quantities

        public virtual double TotalLength
        {
            get { return this.GetLength(this.TMin, this.TMax); }
        }

        public virtual double GetLength(double t0, double t1)
        {
            Contract.Assert(t0 <= t1);

            if (t0 < this.TMin)
            {
                t0 = this.TMin;
            }

            if (t1 > this.TMax)
            {
                t1 = this.TMax;
            }

            return Integrator.Integrate(this.GetSpeed, t0, t1, Integrator.Type.RombergIntegrator, IntegralMaxEval);
        }

        public virtual double GetSpeed(double t)
        {
            return this.GetFirstDerivative(t).Length;
        }

        public virtual double GetCurvature(double t)
        {
            // https://en.wikipedia.org/wiki/Curvature
            Vector2d der1 = this.GetFirstDerivative(t);
            double speed2 = der1.LengthSquared;

            if (speed2.EpsilonEquals(0))
            {
                // Curvature is indeterminate, just return 0.
                return 0;
            }

            Vector2d der2 = this.GetSecondDerivative(t);

            double numer = der1.DotPerpRight(der2);
            double denom = SysMath.Pow(speed2, 3d / 2);
            return numer / denom;
        }

        public virtual Vector2d GetTangent(double t)
        {
            return this.GetFirstDerivative(t).Unit;
        }

        public virtual Vector2d GetLeftNormal(double t)
        {
            return this.GetTangent(t).PerpLeft;
        }

        public virtual void GetFrame(double t, ref Point2d position, ref Vector2d tangent, ref Vector2d leftNormal)
        {
            position = this.GetPosition(t);
            tangent = this.GetTangent(t);
            leftNormal = tangent.PerpLeft;
        }

        #endregion

        #endregion
    }
}