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
using Essence.Maths.Double.Curves;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Maths.Double.Distances
{
    public sealed class DistPointCircle2
    {
        public DistPointCircle2()
        {
        }

        public Point2d Point { get; set; }

        public Circle2 Circle { get; set; }

        public bool Solid { get; set; }

        public Point2d ClosestPoint { get; private set; }

        public double ClosestAngle
        {
            get { return AngleUtils.Ensure0To2Pi(this.ClosestPoint.Sub(this.Circle.Center).Angle); }
        }

        public double CalcDistance()
        {
            double radio = System.Math.Abs(this.Circle.Radius);

            Vector2d diff = this.Point.Sub(this.Circle.Center);
            double len = diff.Length;

            if (this.Solid && (len < radio))
            {
                this.ClosestPoint = this.Point;
                return 0;
            }

            if (len.EpsilonEquals(0))
            {
                // Justo en el centro del circulo..
                this.ClosestPoint = this.Circle.GetPosition(0);
                return radio;
            }

            this.ClosestPoint = this.Circle.Center.Add(diff.Div(len).Mul(radio));
            return SysMath.Abs(len - radio);
        }
    }
}