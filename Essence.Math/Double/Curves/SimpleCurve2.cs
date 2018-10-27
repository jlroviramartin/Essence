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

using System;
using System.Diagnostics.Contracts;
using Essence.Util.Math.Double;
using SysMath = System.Math;
using UnaryFunction = System.Func<double, double>;

namespace Essence.Maths.Double.Curves
{
    public abstract class SimpleCurve2 : ICurve2
    {
        public Vec2d Point0
        {
            get { return this.GetPosition(this.TMin); }
        }

        public Vec2d Point1
        {
            get { return this.GetPosition(this.TMax); }
        }

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

        public abstract Vec2d GetPosition(double t);

        public virtual Vec2d GetFirstDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 1, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 1, 5);
            return new Vec2d(fdx(t), fdy(t));
        }

        public virtual Vec2d GetSecondDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 2, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 2, 5);
            return new Vec2d(fdx(t), fdy(t));
        }

        public virtual Vec2d GetThirdDerivative(double t)
        {
            throw new NotImplementedException();
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 3, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 3, 5);
            return new Vec2d(fdx(t), fdy(t));
        }

        #endregion

        #region Differential geometric quantities

        public virtual double TotalLength
        {
            get { return this.GetLength(this.TMin, this.TMax); }
        }

        public virtual Vec2d GetTangent(double t)
        {
            return this.GetFirstDerivative(t).Norm();
        }

        public virtual double GetSpeed(double t)
        {
            return this.GetFirstDerivative(t).Length;
        }

        public virtual double GetCurvature(double t)
        {
            Vec2d der1 = this.GetFirstDerivative(t);
            double speed2 = der1.Length2;

            if (speed2.EpsilonEquals(0))
            {
                // Curvature is indeterminate, just return 0.
                return 0;
            }

            Vec2d der2 = this.GetSecondDerivative(t);

            double numer = der1.Dot(der2);
            double denom = SysMath.Pow(speed2, 1.5);
            return numer / denom;
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

        public virtual Vec2d GetLeftNormal(double t)
        {
            return vecMath.PerpLeft(this.GetTangent(t));
        }

        public virtual void GetFrame(double t, ref Vec2d position, ref Vec2d tangent, ref Vec2d leftNormal)
        {
            position = this.GetPosition(t);
            tangent = this.GetTangent(t);
            leftNormal = vecMath.PerpLeft(tangent);
        }

        #endregion

        #endregion

        #region private

        /// <summary>Número máximo de evaluaciones para el cálculo de la integral.</summary>
        private const int IntegralMaxEval = 1000;

        private static readonly DoubleMath math = DoubleMath.Instance;
        private static readonly VecMath<double, DoubleMath, Vec2d, Vec2dFactory> vecMath = VecMath<double, DoubleMath, Vec2d, Vec2dFactory>.Instance;

        #endregion
    }
}