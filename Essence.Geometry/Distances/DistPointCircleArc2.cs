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

namespace Essence.Maths.Double.Distances
{
    public sealed class DistPointCircleArc2
    {
        public DistPointCircleArc2()
        {
        }

        public Point2d Point { get; set; }

        public CircleArc2 Arc { get; set; }

        public Point2d ClosestPoint { get; private set; }

        public double ClosestAngle { get; private set; }

        public double CalcDistance()
        {
            return System.Math.Sqrt(this.CalcDistance2());
        }

        public double CalcDistance2()
        {
            // Se calcula la distancia al circulo.
            Circle2 circulo = new Circle2(this.Arc.Center, this.Arc.Radius);
            DistPointCircle2 dist = new DistPointCircle2()
            {
                Point = this.Point,
                Circle = circulo
            };
            double d = dist.CalcDistance();
            Point2d p = dist.ClosestPoint;
            double ca = AngleUtils.Ensure0To2Pi(dist.ClosestAngle); // [0, 2PI)

            // Se comprueba si el punto cae dentro del arco.
            double ai = this.Arc.Angle1;
            double aa = this.Arc.AdvAngle;
            if (AngleUtils.InArc(ai, aa, ca))
            {
                this.ClosestPoint = p;
                this.ClosestAngle = ca;
                return d * d;
            }

            // Se prueban los extremos.
            double d2_0 = this.Arc.Point0.Distance2To(this.Point);
            double d2_1 = this.Arc.Point1.Distance2To(this.Point);
            if (d2_0 <= d2_1)
            {
                this.ClosestPoint = this.Arc.Point0;
                this.ClosestAngle = AngleUtils.Ensure0To2Pi(this.Arc.Angle1, true);
                return d2_0;
            }
            else
            {
                this.ClosestPoint = this.Arc.Point1;
                this.ClosestAngle = AngleUtils.Ensure0To2Pi(this.Arc.Angle2, true);
                return d2_1;
            }
        }
    }
}