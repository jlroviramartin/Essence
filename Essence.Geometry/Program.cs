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
using Essence.Maths.Double.Curves;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Maths
{
    internal class Program
    {
        private static void Main(string[] args)
        {
        }
    }

#if false
    public class Line
    {
        public Line(Point2d p0, double t0, Point2d p1, double t1, double normal, double radio)
        {
            this.p0 = p0;
            this.t0 = t0;
            this.p1 = p1;
            this.t1 = t1;
            this.normal = normal;
            this.radio = radio;

            this.dir = this.p1.Sub(this.p0);
            this.len = this.dir.Length;
            this.dirN = this.dir.Div(this.len);
        }

        public double Evaluate(Point2d pnt)
        {
            double t01 = this.Project01(pnt);
            double t = (t01.EpsilonEquals(0)
                ? this.t0
                : (t01.EpsilonEquals(1)
                    ? this.t1
                    : this.t0 + (this.t1 - this.t0) * t01));
            return t;
        }

        public void PuntoEje(double t, out Point2d pnt, out double normal, out double radio)
        {
            // Se normaliza la estacion.
            double t01 = (t.EpsilonEquals(this.t0)
                ? 0
                : (t.EpsilonEquals(this.t1)
                    ? 1
                    : (t - this.t0) / (this.t1 - this.t0)));

            pnt = this.Evaluate01(t01);
            normal = this.normal;
            radio = this.radio;
        }

        public BoundingBox2d GetEnvelope(double t0, double t1)
        {
            if (t0.EpsilonEquals(this.t0) && t1.EpsilonEquals(this.t1))
            {
                return BoundingBox2d.Union(this.p0, this.p1);
            }

            if (this.t0.EpsilonEquals(this.t1))
            {
                return BoundingBox2d.Union(this.p0, this.p0);
            }

            // Se normaliza la estacion.
            double t01_0 = (t0 - this.t0) / (this.t1 - this.t0);
            double t01_1 = (t1 - this.t0) / (this.t1 - this.t0);

            return BoundingBox2d.Union(
                this.Evaluate01(t01_0),
                this.Evaluate01(t01_1));
        }

        public BoundingBox2d GetEnvelope(double d, double estacion1, double estacion2)
        {
            double t0 = this.t0;
            double t1 = this.t1;

            Vector2d v = this.dirN.PerpRight.Mul(d);

            if (estacion1.EpsilonEquals(this.t0) && estacion2.EpsilonEquals(this.t1))
            {
                return BoundingBox2d.Union(this.p0.Add(v), this.p1.Add(v));
            }

            if (this.t0.EpsilonEquals(this.t1))
            {
                return BoundingBox2d.Union(this.p0.Add(v), this.p0.Add(v));
            }

            // Se normaliza la estacion.
            double t01_0 = (estacion1 - this.t0) / (this.t1 - this.t0);
            double t01_1 = (estacion2 - this.t0) / (this.t1 - this.t0);

            return BoundingBox2d.Union(
                this.Evaluate01(t01_0).Add(v),
                this.Evaluate01(t01_1).Add(v));
        }

        private readonly Point2d p0;
        private readonly double t0;

        private readonly Point2d p1;
        private readonly double t1;

        private readonly Vector2d dir;
        private readonly Vector2d dirN;
        private readonly double len;

        private readonly double normal;
        private readonly double radio;

        public double Project01(Point2d p)
        {
            Vector2d diff = p.Sub(this.p0);
            //return this.dir.Dot(diff) / this.len;
            return this.dirN.Dot(diff);
        }

        private Point2d Evaluate01(double t01)
        {
            //return this.p0.Add(this.dirN.Mul(t01 * this.len));
            return this.p0.Add(this.dir.Mul(t01));
        }
    }

    public class Arc
    {
        public double Evaluate(Point2d pnt)
        {
            CircleArc2 arc = new CircleArc2(this.center, this.radius, this.angle0, this.angle1);
            double d2;
            double t = arc.Project(pnt, out d2);

            // Se desnormaliza la estacion.
            return this.t0 + (this.t1 - this.t0) * t;
        }

        public void PuntoEje(double t, out Point2d pnt, out double normal, out double radio)
        {
            CircleArc2 arc = new CircleArc2(this.center, this.radius, this.angle0, this.angle1);

            if (this.t0.EpsilonEquals(this.t1))
            {
                pnt = arc.GetPosition(this.t0);
                radio = this.radius;
                normal = this.normal1;
                return;
            }

            // Se normaliza la estacion.
            double t01 = (t - this.t0) / (this.t1 - this.t0);

            pnt = arc.GetPosition(t01);
            radio = this.radius;
            normal = this.normal1 + (this.t1 - t) / this.radius;
        }

        private Point2d center;
        private double radius;

        private double angle0;
        private readonly double t0;

        private double angle1; // Si angle1 < angle0 es CW.
        private readonly double t1;

        private readonly double normal1;
    }

    public class Clothoid
    {
    }

    public class EllipseArc2d
    {
        private double angle0;
        private double angle1;

        private double angleW;

        private Vec2d center;

        private readonly double avAngle;

        private double xRadius;
        private double yRadius;
    }

    public class Ellipse2d
    {
        private double angleW;

        private Vec2d center;

        private double xRadius;
        private double yRadius;
    }
#endif
}