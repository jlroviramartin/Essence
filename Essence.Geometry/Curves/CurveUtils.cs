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

namespace Essence.Geometry.Curves
{
    public static class CurveUtils
    {
        public static ICurve1 Split(ICurve1 curve, double tmin, double tmax)
        {
            return new SplitCurve1(curve, tmin, tmax);
        }

        public static ICurve2 Split(ICurve2 curve, double tmin, double tmax)
        {
            return new SplitCurve2(curve, tmin, tmax);
        }

        private sealed class SplitCurve1 : WrapperCurve1
        {
            public SplitCurve1(ICurve1 curve, double tmin, double tmax)
                : base(curve)
            {
                this.tmin = tmin;
                this.tmax = tmax;
            }

            private readonly double tmin;
            private readonly double tmax;

            public override double TMin
            {
                get { return this.tmin; }
            }

            public override double TMax
            {
                get { return this.tmax; }
            }

            public override double TotalLength
            {
                get { return this.curve.GetLength(this.tmin, this.tmax); }
            }
        }

        private sealed class SplitCurve2 : WrapperCurve2
        {
            public SplitCurve2(ICurve2 curve, double tmin, double tmax)
                : base(curve)
            {
                this.tmin = tmin;
                this.tmax = tmax;
            }

            private readonly double tmin;
            private readonly double tmax;

            public override double TMin
            {
                get { return this.tmin; }
            }

            public override double TMax
            {
                get { return this.tmax; }
            }

            public override double TotalLength
            {
                get { return this.curve.GetLength(this.tmin, this.tmax); }
            }
        }
    }
}