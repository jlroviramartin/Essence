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
using SysMath = System.Math;

namespace Essence.Geometry.Curves
{
    public class Const1 : SimpleCurve1
    {
        public Const1(double p, double t0, double t1)
        {
            this.p = p;

            this.len = SysMath.Abs(t1 - t0);

            this.SetTInterval(t0, t1);
        }

        #region private

        private double GetT01(double t)
        {
            t = t.Clamp(this.TMin, this.TMax);
            double t01 = this.ttransform.Get(t);
            return t01;
        }

        private readonly double p;

        private double tmin;
        private double tmax;

        private readonly double len;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        private Transform1 ttransform;

        #endregion

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

        public override double GetPosition(double t)
        {
            return this.p;
        }

        public override double GetFirstDerivative(double t)
        {
            return 0;
        }

        public override double GetSecondDerivative(double t)
        {
            return 0;
        }

        public override double GetThirdDerivative(double t)
        {
            return 0;
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

        public override BoundingBox1d BoundingBox
        {
            get { return new BoundingBox1d(this.p, this.p); }
        }

        #endregion
    }
}