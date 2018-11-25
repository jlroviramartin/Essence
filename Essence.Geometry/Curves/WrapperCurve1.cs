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

namespace Essence.Geometry.Curves
{
    public class WrapperCurve1 : ICurve1
    {
        public WrapperCurve1(ICurve1 curve)
        {
            this.curve = curve;
        }

        protected readonly ICurve1 curve;

        public virtual double TMin
        {
            get { return this.curve.TMin; }
        }

        public virtual double TMax
        {
            get { return this.curve.TMax; }
        }

        public virtual void SetTInterval(double tmin, double tmax)
        {
            this.curve.SetTInterval(tmin, tmax);
        }

        public virtual double GetPosition(double t)
        {
            return this.curve.GetPosition(t);
        }

        public virtual double GetFirstDerivative(double t)
        {
            return this.curve.GetFirstDerivative(t);
        }

        public virtual double GetSecondDerivative(double t)
        {
            return this.curve.GetSecondDerivative(t);
        }

        public virtual double GetThirdDerivative(double t)
        {
            return this.curve.GetThirdDerivative(t);
        }

        public virtual double TotalLength
        {
            get { return this.curve.TotalLength; }
        }

        public virtual double GetLength(double t0, double t1)
        {
            return this.curve.GetLength(t0, t1);
        }

        public virtual double GetSpeed(double t)
        {
            return this.curve.GetSpeed(t);
        }

        public virtual BoundingBox1d BoundingBox
        {
            get { return this.BoundingBox; }
        }
    }
}