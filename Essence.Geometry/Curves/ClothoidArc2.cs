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

using System;
using System.Diagnostics.Contracts;
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using Essence.Geometry.Core;
using Essence.Maths.Double;
using SysMath = System.Math;
using Transform2 = Essence.Geometry.Core.Transform2;

namespace Essence.Geometry.Curves
{
    /// <summary>
    /// Arco de clotoide.
    /// </summary>
    public class ClothoidArc2 : SimpleCurve2
    {
        public static ClothoidArc2[] Split(double tmin,
                                           Point2d point0, Point2d point1,
                                           double radius0, double radius1,
                                           double a)
        {
            bool invertY = (radius1 < 0);

            double l0_n = ClothoUtils.ClothoL(radius0, invertY, a);
            double l1_n = ClothoUtils.ClothoL(radius1, invertY, a);
            Contract.Assert(l0_n < 0 && l1_n > 0);

            // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
            Point2d p0_n = ClothoUtils.Clotho(l0_n, invertY, a);
            Point2d p1_n = ClothoUtils.Clotho(l1_n, invertY, a);

            // Diferencia de puntos en coordenadas reales.
            Vector2d v01 = point1.Sub(point0);

            Vector2d v01_n = p1_n.Sub(p0_n);

            // Rotacion de v01_n -> v01.
            double r = v01_n.AngleTo(v01);

            // Transformacion a aplicar.
            ITransform2 transform = Transform2.Translate(point1.X - p1_n.X, point1.Y - p1_n.Y)
                                              .Concat(Transform2.Rotate(p1_n.X, p1_n.Y, r));

            ClothoidArc2 left = new ClothoidArc2(transform, l0_n, 0, invertY, a);
            ClothoidArc2 right = new ClothoidArc2(transform, 0, l1_n, invertY, a);

            left.SetTInterval(tmin, tmin + (-l0_n)); // l0_n < 0
            right.SetTInterval(tmin + (-l0_n), tmin + (-l0_n) + l1_n);

            return new[] { left, right };
        }

        [Obsolete]
        public ClothoidArc2(double tmin, // It is only needed as start station
                            Point2d point0, Point2d point1,
                            double radius0, double radius1,
                            double a)
        {
            // Correccion sobre los radios.
            if (SysMath.Sign(radius1) != SysMath.Sign(radius0))
            {
                if (double.IsInfinity(radius0))
                {
                    radius0 = SysMath.Sign(radius1) * double.PositiveInfinity;
                }
                else if (double.IsInfinity(radius1))
                {
                    radius1 = SysMath.Sign(radius0) * double.PositiveInfinity;
                }
                else
                {
                    // No se permite cambio de signo en el radio. Funcionaria???
                    Contract.Assert(false);
                }
            }

            if (SysMath.Abs(radius0) > SysMath.Abs(radius1))
            {
                // t positivas
                this.InvertY = radius1 < 0;
            }
            else
            {
                // t negativa
                this.InvertY = radius1 > 0;
            }

            this.A = a;

            // Desarrollo segun el radio en el punto (0) y (1).
            double l0_n = ClothoUtils.ClothoL(radius0, this.InvertY, this.A);
            double l1_n = ClothoUtils.ClothoL(radius1, this.InvertY, this.A);

            // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
            Point2d p0_n = ClothoUtils.Clotho(l0_n, this.InvertY, this.A);
            Point2d p1_n = ClothoUtils.Clotho(l1_n, this.InvertY, this.A);

            Vector2d v01_n = p1_n.Sub(p0_n);
            Vector2d v01 = point1.Sub(point0);

            // Rotacion de v01_n -> v01.
            double r = v01_n.AngleTo(v01);

            // Transformacion a aplicar.
            this.transform = Transform2.Translate(point1.X - p1_n.X, point1.Y - p1_n.Y)
                                       .Concat(Transform2.Rotate(p1_n.X, p1_n.Y, r));

            this.l0 = l0_n;
            this.l1 = l1_n;

            this.SetTInterval(tmin, tmin + Math.Abs(this.l1 - this.l0));
        }

        public static ClothoidArc2 BuildSimple(double radius0, double radius1, double a, ITransform2 transform)
        {
            // Correccion sobre los radios.
            if (SysMath.Sign(radius1) != SysMath.Sign(radius0))
            {
                if (double.IsInfinity(radius0))
                {
                    radius0 = SysMath.Sign(radius1) * double.PositiveInfinity;
                }
                else if (double.IsInfinity(radius1))
                {
                    radius1 = SysMath.Sign(radius0) * double.PositiveInfinity;
                }
                else
                {
                    // No se permite cambio de signo en el radio. Funcionaria???
                    Contract.Assert(false);
                }
            }

            // Desarrollo segun el radio en el punto (0) y (1).
            double l0 = ClothoUtils.ClothoL(radius0, false, a);
            double l1 = ClothoUtils.ClothoL(radius1, false, a);

            ClothoidArc2 clotho = new ClothoidArc2(transform, l0, l1, false, a);
            clotho.SetTInterval(0, Math.Abs(l1 - l0));
            return clotho;
        }

        public static ClothoidArc2 BuildSimple(double radius0, double radius1, double a)
        {
            return BuildSimple(radius0, radius1, a, Transform2.Identity());
        }

#if false
        public static ClothoidArc2 BuildSimple(double tmin,
                                               Point2d point0, Point2d point1,
                                               double radius0, double radius1,
                                               double a)
        {
            // Correccion sobre los radios.
            if (SysMath.Sign(radius1) != SysMath.Sign(radius0))
            {
                if (double.IsInfinity(radius0))
                {
                    radius0 = SysMath.Sign(radius1) * double.PositiveInfinity;
                }
                else if (double.IsInfinity(radius1))
                {
                    radius1 = SysMath.Sign(radius0) * double.PositiveInfinity;
                }
                else
                {
                    // No se permite cambio de signo en el radio. Funcionaria???
                    Contract.Assert(false);
                }
            }

            bool invertY;
            if (SysMath.Abs(radius0) > SysMath.Abs(radius1))
            {
                // t positivas
                invertY = radius1 < 0;
            }
            else
            {
                // t negativa
                invertY = radius1 > 0;
            }

            // Diferencia de puntos en coordenadas reales.
            Vector2d v01 = point1.Sub(point0);

            // Desarrollo segun el radio en el punto (0) y (1).
            double l0_n = ClothoUtils.ClothoL(radius0, invertY, a);
            double l1_n = ClothoUtils.ClothoL(radius1, invertY, a);

            // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
            Point2d p0_n = ClothoUtils.Clotho(l0_n, invertY, a);
            Point2d p1_n = ClothoUtils.Clotho(l1_n, invertY, a);

            Vector2d v01_n = p1_n.Sub(p0_n);

            // Rotacion de v01_n -> v01.
            double r = v01_n.AngleTo(v01);

            // Transformacion a aplicar.
            ITransform2 transform = Transform2.Translate(point1.X - p1_n.X, point1.Y - p1_n.Y)
                                              .Concat(Transform2.Rotate(p1_n.X, p1_n.Y, r));

            double l0 = l0_n;
            double l1 = l1_n;

            ClothoidArc2 clotho = new ClothoidArc2(transform, l0, l1, invertY, a);
            clotho.SetTInterval(tmin, tmin + Math.Abs(l1 - l0));
            return clotho;
        }

        public ClothoidArc2(Vector2d point0, Vector2d point1,
                            double radius0, double radius1)
            : this(0, (Point2d)point0, (Point2d)point1, radius0, radius1)
        {
        }

        public static ITransform2 EvaluateTransform(Point2d point0, Point2d point1,
                                                    double radius0, double radius1,
                                                    double a)
        {
            // Correccion sobre los radios.
            if (SysMath.Sign(radius1) != SysMath.Sign(radius0))
            {
                if (double.IsInfinity(radius0))
                {
                    radius0 = SysMath.Sign(radius1) * double.PositiveInfinity;
                }
                else if (double.IsInfinity(radius1))
                {
                    radius1 = SysMath.Sign(radius0) * double.PositiveInfinity;
                }
                else
                {
                    // No se permite cambio de signo en el radio. Funcionaria???
                    Contract.Assert(false);
                }
            }

            bool invertY;
            if (SysMath.Abs(radius0) > SysMath.Abs(radius1))
            {
                // t positivas
                invertY = radius1 < 0;
            }
            else
            {
                // t negativa
                invertY = radius1 > 0;
            }

            // Diferencia de puntos en coordenadas reales.
            Vector2d v01 = point1.Sub(point0);

            // Desarrollo segun el radio en el punto (0) y (1).
            double l0_n = ClothoUtils.ClothoL(radius0, invertY, a);
            double l1_n = ClothoUtils.ClothoL(radius1, invertY, a);

            // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
            Point2d p0_n = ClothoUtils.Clotho(l0_n, invertY, a);
            Point2d p1_n = ClothoUtils.Clotho(l1_n, invertY, a);

            Vector2d v01_n = p1_n.Sub(p0_n);

            // Rotacion de v01_n -> v01.
            double r = v01_n.AngleTo(v01);

            // Transformacion a aplicar.
            return Transform2.Translate(point1.X - p1_n.X, point1.Y - p1_n.Y)
                             .Concat(Transform2.Rotate(p1_n.X, p1_n.Y, r));
        }
#endif

        public ClothoidArc2(ITransform2 transform,
                            double l0, double l1,
                            bool invertY, double a)
        {
            this.l0 = l0;
            this.l1 = l1;
            this.A = a;
            this.InvertY = invertY;

            this.transform = transform;

            this.SetTInterval(this.l0, this.l1);
        }

        public double A { get; }

        public bool InvertY { get; }

        public double GetRadius(double t)
        {
            double dl = this.GetL(t);
            return ClothoUtils.ClothoRadius(dl, this.InvertY, this.A);
        }

        public new double GetTangent(double t)
        {
            double dl = this.GetL(t);
            return ClothoUtils.ClothoTangent(dl, this.InvertY, this.A);
        }

        #region private

        private double GetL(double t)
        {
            t = t.Clamp(this.tmin, this.tmax);
            double dl = this.ttransform.Get(t);
            if (SysMath.Abs(dl) > ClothoUtils.GetMaxL(this.A))
            {
                throw new Exception("Longitud del arco por encima del m�ximo permitido.");
            }
            return dl;
        }

        private readonly double l0;
        private readonly double l1;

        /// <summary>Transformacion que se aplica sobre la posicion.</summary>
        private readonly ITransform2 transform;

        private double tmin;
        private double tmax;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        private Transform1 ttransform;

        #endregion

        #region Curve2

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
            this.ttransform = new Transform1(this.tmin, this.tmax, this.l0, this.l1);
        }

        public override Point2d GetPosition(double t)
        {
            /*
             *  Maxima:
             *  s:�     t * m + n;
             *  x:      a*sqrt(%pi)*fresnel_c(s/(a*sqrt(%pi)));
             *  y:      a*sqrt(%pi)*fresnel_s(s/(a*sqrt(%pi)));
             *  z:�     1;
             *  result: [ x, y, z ]; 
             *  
             *  result: [sqrt(%pi)*a*fresnel_c((m*t+n)/(sqrt(%pi)*a)),sqrt(%pi)*a*fresnel_s((m*t+n)/(sqrt(%pi)*a)),1]
             */
            double dl = this.GetL(t);
            Point2d pt = ClothoUtils.Clotho(dl, this.InvertY, this.A);
            return this.transform.DoTransform(pt);
        }

        public override Vector2d GetFirstDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:�     t * m + n;
             *  x:      a*sqrt(%pi)*fresnel_c(s/(a*sqrt(%pi)));
             *  y:      a*sqrt(%pi)*fresnel_s(s/(a*sqrt(%pi)));
             *  z:�     1;
             *  result: diff([ x, y, z ], t, 1); 
             *  
             *  result: [m*cos((m*t+n)^2 / (2*a^2)),
             *           m*sin((m*t+n)^2 / (2*a^2)),
             *           0]
             *
             *          m*[cos((m*t+n)^2 / (2*a^2)),
             *             sin((m*t+n)^2 / (2*a^2)),
             *             0]
             *
             *          m*[cos(s^2 / (2*a^2)),
             *             sin(s^2 / (2*a^2)),
             *             0]
             */
            double m = this.ttransform.A;

            double dl = this.GetL(t);
            Vector2d v = ClothoUtils.DClotho(dl, this.InvertY, this.A);
            v = v.Mul(m);
            return this.transform.DoTransform(v);
        }

        public override Vector2d GetSecondDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:�     t * m + n;
             *  x:      a*sqrt(%pi)*fresnel_c(s/(a*sqrt(%pi)));
             *  y:      a*sqrt(%pi)*fresnel_s(s/(a*sqrt(%pi)));
             *  z:�     1;
             *  result: diff([ x, y, z ], t, 2); 
             *  
             *  result: [-(m^2*(m*t+n)*sin((m*t+n)^2 / (2*a^2))) / a^2,
             *            (m^2*(m*t+n)*cos((m*t+n)^2 / (2*a^2))) / a^2,
             *            0]
             *
             *          m^2*[-((m*t+n)*sin((m*t+n)^2 / (2*a^2))) / a^2,
             *                ((m*t+n)*cos((m*t+n)^2 / (2*a^2))) / a^2,
             *                0]
             *
             *          m^2*[-(s*sin(s^2 / (2*a^2))) / a^2,
             *                (s*cos(s^2 / (2*a^2))) / a^2,
             *                0]
             */
            double m = this.ttransform.A;
            double m2 = m * m;

            double dl = this.GetL(t);
            Vector2d v = ClothoUtils.DClotho2(dl, this.InvertY, this.A);
            v = v.Mul(m2);
            return this.transform.DoTransform(v);
        }

        public override Vector2d GetThirdDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:�     t * m + n;
             *  x:      a*sqrt(%pi)*fresnel_c(s/(a*sqrt(%pi)));
             *  y:      a*sqrt(%pi)*fresnel_s(s/(a*sqrt(%pi)));
             *  z:�     1;
             *  result: diff([ x, y, z ], t, 3); 
             *  
             *  result: [-(m^3*sin((m*t+n)^2 / (2*a^2))) / a^2 - (m^3*(m*t+n)^2*cos((m*t+n)^2 / (2*a^2))) / a^4,
             *            (m^3*cos((m*t+n)^2 / (2*a^2))) / a^2 - (m^3*(m*t+n)^2*sin((m*t+n)^2 / (2*a^2))) / a^4,
             *            0]
             *
             *          m^3*[-(sin((m*t+n)^2 / (2*a^2))) / a^2 - ((m*t+n)^2*cos((m*t+n)^2 / (2*a^2))) / a^4,
             *                (cos((m*t+n)^2 / (2*a^2))) / a^2 - ((m*t+n)^2*sin((m*t+n)^2 / (2*a^2))) / a^4,
             *                0]
             *
             *          m^3*[-(sin(s^2 / (2*a^2))) / a^2 - (s^2*cos(s^2 / (2*a^2))) / a^4,
             *                (cos(s^2 / (2*a^2))) / a^2 - (s^2*sin(s^2 / (2*a^2))) / a^4,
             *                0]
             */
            double m = this.ttransform.A;
            double m3 = m * m * m;

            double dl = this.GetL(t);
            Vector2d v = ClothoUtils.DClotho3(dl, this.InvertY, this.A);
            v = v.Mul(m3);
            return this.transform.DoTransform(v);
        }

        public override double TotalLength
        {
            get { return Math.Abs(this.l1 - this.l0); }
        }

        public override double GetLength(double t0, double t1)
        {
            double dl0 = this.GetL(t0);
            double dl1 = this.GetL(t1);
            return Math.Abs(dl1 - dl0);
        }

        public override BoundingBox2d BoundingBox
        {
            get
            {
                return BoundingBox2d.UnionOfPoints(
                    (Point2d)this.transform.Transform(new Point2d(0.0, 0.0)),
                    (Point2d)this.transform.Transform(new Point2d(1.0, 0.0)),
                    (Point2d)this.transform.Transform(new Point2d(1.0, 1.0)),
                    (Point2d)this.transform.Transform(new Point2d(0.0, 1.0)));
            }
        }

        #endregion
    }
}