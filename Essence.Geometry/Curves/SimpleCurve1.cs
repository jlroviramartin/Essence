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
using UnaryFunction = System.Func<double, double>;

namespace Essence.Geometry.Curves
{
    public abstract class SimpleCurve1 : ICurve1
    {
        public double Point0
        {
            get { return this.GetPosition(this.TMin); }
        }

        public double Point1
        {
            get { return this.GetPosition(this.TMax); }
        }

        #region private

        /// <summary>Número máximo de evaluaciones para el cálculo de la integral.</summary>
        private const int IntegralMaxEval = 1000;

        #endregion

        #region ICurve1

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

        public abstract double GetPosition(double t);

        public virtual double GetFirstDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt), 1, 5);
            return fdx(t);
        }

        public virtual double GetSecondDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt), 2, 5);
            return fdx(t);
        }

        public virtual double GetThirdDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt), 3, 5);
            return fdx(t);
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
            return this.GetFirstDerivative(t);
        }

        #endregion

        public abstract BoundingBox1d BoundingBox { get; }

        #endregion
    }
}