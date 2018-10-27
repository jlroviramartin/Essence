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
using MathUtils = Essence.Util.Math.Double.MathUtils;
using SysMath = System.Math;
using UnaryFunction = System.Func<double, double>;

namespace Essence.Geometry.Curves
{
    /// <summary>
    /// Curva desplazada.
    /// </summary>
    public class DisplacedCurve2 : ICurve2
    {
        public DisplacedCurve2(ICurve2 curve, ICurve1 displacement)
        {
            this.curve = curve;
            this.displacement = displacement;
        }

        public ICurve2 Curve
        {
            get { return this.curve; }
        }

        public ICurve1 Displacement
        {
            get { return this.displacement; }
        }

        #region private

        private readonly ICurve2 curve;
        private readonly ICurve1 displacement;

        #endregion

        /// <summary>Número máximo de evaluaciones para el cálculo de la integral.</summary>
        private const int IntegralMaxEval = 1000;

        #region ICurve2

        public virtual bool IsClosed
        {
            get { return this.curve.IsClosed; }
        }

        public virtual double TMin
        {
            get { return MathUtils.Max(this.curve.TMin, this.displacement.TMin); }
        }

        public virtual double TMax
        {
            get { return MathUtils.Min(this.curve.TMax, this.displacement.TMax); }
        }

        public virtual void SetTInterval(double tmin, double tmax)
        {
            this.curve.SetTInterval(tmin, tmax);
            this.displacement.SetTInterval(tmin, tmax);
        }

        #region Position and derivatives

        public virtual Point2d GetPosition(double t)
        {
            double d = this.displacement.GetPosition(t);
            Point2d p = this.curve.GetPosition(t);
            Vector2d n = this.curve.GetLeftNormal(t);
            return p + n * d;
        }

        public virtual Vector2d GetFirstDerivative(double t)
        {
            Vector2d df = this.curve.GetFirstDerivative(t);
            double df_x = df.X;
            double df_y = df.Y;

            Vector2d d2f = this.curve.GetSecondDerivative(t);
            double d2f_x = d2f.X;
            double d2f_y = d2f.Y;

            double n = this.displacement.GetPosition(t);
            double dn = this.displacement.GetFirstDerivative(t);

            /*double a = df_y * df_y + df_x * df_x;
            double sqrt_a = SysMath.Sqrt(a);
            double b = (df_x * d2f_x + df_y * d2f_y) / a;

            double c = (n * b - dn);

            double x = df_x + (c * df_y - n * d2f_y) / sqrt_a;
            double y = df_y - (c * df_x - n * d2f_x) / sqrt_a;

            return new Vector2d(x, y);*/

            /*UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 1, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 1, 5);
            return new Vector2d(fdx(t), fdy(t));*/

            double df2 = ((df_y * df_y) + (df_x * df_x));

            double x = -((df_y) * (dn)) / SysMath.Sqrt(df2) + (n * (df_y) * (2 * (df_y) * (d2f_y) + 2 * (df_x) * (d2f_x))) / (2 * (SysMath.Sqrt(df2) * df2)) - (n * (d2f_y)) / SysMath.Sqrt(df2) + df_x;
            double y = +((df_x) * (dn)) / SysMath.Sqrt(df2) - (n * (df_x) * (2 * (df_y) * (d2f_y) + 2 * (df_x) * (d2f_x))) / (2 * (SysMath.Sqrt(df2) * df2)) + (n * (d2f_x)) / SysMath.Sqrt(df2) + df_y;
            return new Vector2d(x, y);
        }

        public virtual Vector2d GetSecondDerivative(double t)
        {
            Vector2d df = this.curve.GetFirstDerivative(t);
            double df_x = df.X;
            double df_y = df.Y;

            Vector2d d2f = this.curve.GetSecondDerivative(t);
            double d2f_x = d2f.X;
            double d2f_y = d2f.Y;

            Vector2d d3f = this.curve.GetThirdDerivative(t);
            double d3f_x = d3f.X;
            double d3f_y = d3f.Y;

            double n = this.displacement.GetPosition(t);
            double dn = this.displacement.GetFirstDerivative(t);
            double d2n = this.displacement.GetThirdDerivative(t);

            double a = df_y * df_y + df_x * df_x;
            double sqrt_a = SysMath.Sqrt(a);

            double b = (df_x * d2f_x + df_y * d2f_y) / a;
            double bb = (df_x * d3f_x + d2f_x * d2f_x + df_y * d3f_y + d2f_y * d2f_y) / a;

            double c = 2 * (n * b - dn);
            double d = (n * (bb - 3 * b * b) + dn * 2 * b - d2n);

            double x = d2f_x + (d * df_y + c * d2f_y - n * d3f_y) / sqrt_a;
            double y = d2f_y - (d * df_x + c * d2f_x - n * d3f_x) / sqrt_a;

            return new Vector2d(x, y);

            /*UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 2, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 2, 5);
            return new Vector2d(fdx(t), fdy(t));*/
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