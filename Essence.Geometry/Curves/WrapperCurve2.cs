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
    public class WrapperCurve2 : ICurve2
    {
        public WrapperCurve2(ICurve2 curve)
        {
            this.curve = curve;
        }

        protected readonly ICurve2 curve;

        public virtual bool IsClosed
        {
            get { return this.curve.IsClosed; }
        }

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

        public virtual Point2d GetPosition(double t)
        {
            return this.curve.GetPosition(t);
        }

        public virtual Vector2d GetFirstDerivative(double t)
        {
            return this.curve.GetFirstDerivative(t);
        }

        public virtual Vector2d GetSecondDerivative(double t)
        {
            return this.curve.GetSecondDerivative(t);
        }

        public virtual Vector2d GetThirdDerivative(double t)
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

        public virtual double GetCurvature(double t)
        {
            return this.curve.GetCurvature(t);
        }

        public virtual Vector2d GetTangent(double t)
        {
            return this.curve.GetTangent(t);
        }

        public virtual Vector2d GetLeftNormal(double t)
        {
            return this.curve.GetLeftNormal(t);
        }

        public virtual void GetFrame(double t, ref Point2d position, ref Vector2d tangent, ref Vector2d leftNormal)
        {
            this.curve.GetFrame(t, ref position, ref tangent, ref leftNormal);
        }

        public virtual BoundingBox2d BoundingBox
        {
            get { return this.BoundingBox; }
        }
    }
}