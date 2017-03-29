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
using org.apache.commons.math3.exception;
using org.apache.commons.math3.analysis.solvers;
using SysMath = System.Math;
using Essence.Geometry.Core;

namespace Essence.Maths.Double.Curves
{
    /// <summary>
    ///     Arco de clotoide.
    /// </summary>
    public class ClothoidArc2 : SimpleCurve2
    {
        public static ClothoidArc2[] Split(double l0,
                                           Point2d point0, Point2d point1,
                                           double radius0, double radius1,
                                           double a)
        {
            bool invertY = (radius1 < 0);

            double l0_n = ClothoUtils.ClothoL(radius0, invertY, a);
            double l1_n = ClothoUtils.ClothoL(radius1, invertY, a);
            Contract.Assert(l0_n < 0 && l1_n > 0);

            // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
            Vector2d p0_n = ClothoUtils.Clotho(l0_n, invertY, a);
            Vector2d p1_n = ClothoUtils.Clotho(l1_n, invertY, a);

            // Diferencia de puntos en coordenadas reales.
            Vector2d v01 = point1.Sub(point0);

            Vector2d v01_n = p1_n.Sub(p0_n);

            // Rotacion de v01_n -> v01.
            double r = v01_n.AngleTo(v01);

            // Transformacion a aplicar.
            ITransform2D transform = Transform2D.Translate(point1.X - p1_n.X, point1.Y - p1_n.Y)
                                                .Concat(Transform2D.Rotate(p1_n.X, p1_n.Y, r));

            ClothoidArc2 left = new ClothoidArc2(transform, l0_n, 0, invertY, a);
            ClothoidArc2 right = new ClothoidArc2(transform, 0, l1_n, invertY, a);

            left.SetTInterval(l0, l0 + (-l0_n)); // l0_n < 0
            right.SetTInterval(l0 + (-l0_n), l0 + (-l0_n) + l1_n);

            return new[] { left, right };
        }

        public ClothoidArc2(double l0,
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
                    // Se deja tal cual.
                    //// Se toma el radio inicio como infinito.
                    ////radius0 = SysMath.Sign(radius1) * double.PositiveInfinity;
                }
            }

            if (SysMath.Abs(radius0) > SysMath.Abs(radius1))
            {
                // t positivas
                this.invertY = radius1 < 0;
            }
            else
            {
                // t negativa
                this.invertY = radius1 > 0;
            }

            // Diferencia de puntos en coordenadas reales.
            Vector2d v01 = point1.Sub(point0);

            this.a = a;

            // Desarrollo segun el radio en el punto (0) y (1).
            double l0_n = ClothoUtils.ClothoL(radius0, this.invertY, this.a);
            double l1_n = ClothoUtils.ClothoL(radius1, this.invertY, this.a);

            // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
            Point2d p0_n = ClothoUtils.Clotho(l0_n, this.invertY, this.a);
            Point2d p1_n = ClothoUtils.Clotho(l1_n, this.invertY, this.a);

            Vector2d v01_n = p1_n.Sub(p0_n);

            // Rotacion de v01_n -> v01.
            double r = v01_n.AngleTo(v01);

            // Transformacion a aplicar.
            this.transform = Transform2D.Translate(point1.X - p1_n.X, point1.Y - p1_n.Y)
                                        .Concat(Transform2D.Rotate(p1_n.X, p1_n.Y, r));

            this.l0 = l0_n;
            this.l1 = l1_n;

            this.SetTInterval(l0, l0 + (this.l1 - this.l0));
        }

        public ClothoidArc2(Vector2d point0, Vector2d point1,
                            double radius0, double radius1)
            : this(0, point0, point1, radius0, radius1)
        {
        }

        public ClothoidArc2(double l0,
                            Vector2d point0, Vector2d point1,
                            double radius0, double radius1)
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
                this.invertY = radius1 < 0;
            }
            else
            {
                // t negativa
                this.invertY = radius1 > 0;
            }

            // Diferencia de puntos en coordenadas reales.
            Vector2d v01 = point1.Sub(point0);

            this.a = SolveParam(v01.Length, radius0, radius1);

            // Desarrollo segun el radio en el punto (0) y (1).
            double l0_n = ClothoUtils.ClothoL(radius0, this.invertY, this.a);
            double l1_n = ClothoUtils.ClothoL(radius1, this.invertY, this.a);

            // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
            Point2d p0_n = ClothoUtils.Clotho(l0_n, this.invertY, this.a);
            Point2d p1_n = ClothoUtils.Clotho(l1_n, this.invertY, this.a);

            Vector2d v01_n = p1_n.Sub(p0_n);

            // Rotacion de v01_n -> v01.
            double r = v01_n.AngleTo(v01);

            // Transformacion a aplicar.
            this.transform = Transform2D.Translate(point1.X - p1_n.X, point1.Y - p1_n.Y)
                                        .Concat(Transform2D.Rotate(p1_n.X, p1_n.Y, r));

            this.l0 = l0_n;
            this.l1 = l1_n;

            this.SetTInterval(l0, l0 + (this.l1 - this.l0));
        }

        public ClothoidArc2(ITransform2D transform,
                            double l0, double l1,
                            bool invertY, double a)
        {
            this.l0 = l0;
            this.l1 = l1;
            this.a = a;
            this.invertY = invertY;

            this.transform = transform;

            this.SetTInterval(this.l0, this.l1);
        }

        public double A
        {
            get { return this.a; }
        }

        public bool InvertY
        {
            get { return this.invertY; }
        }

        public double GetRadius(double t)
        {
            double dl = this.GetL(t);
            return ClothoUtils.ClothoRadious(dl, this.InvertY, this.A);
        }

        public new double GetTangent(double t)
        {
            double dl = this.GetL(t);
            return ClothoUtils.ClothoTangent(dl, this.InvertY, this.A);
        }

        #region private

        private const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;
        private static readonly double sqrtpi = SysMath.Sqrt(SysMath.PI);

        /// <summary>
        ///     Resuelve el parametro de la clotoide dado los radios <c>r0</c> y <c>r1</c> Con una distancia <c>d</c> entre los
        ///     puntos.
        /// </summary>
        private static double SolveParam(double d, double r0, double r1)
        {
            Func<double, double> f = (a) =>
            {
                double fs0, fc0;
                ClothoUtils.Fresnel(a / (r0 * sqrtpi), out fs0, out fc0);

                double fs1, fc1;
                ClothoUtils.Fresnel(a / (r1 * sqrtpi), out fs1, out fc1);

                double fc10 = (fc1 - fc0);
                double fs10 = (fs1 - fs0);

                return a * a * SysMath.PI * (fc10 * fc10 + fs10 * fs10) - d * d;
            };

            //UnivariateSolver solver = new BisectionSolver(DEFAULT_ABSOLUTE_ACCURACY);
            //UnivariateSolver solver = new SecantSolver(DEFAULT_ABSOLUTE_ACCURACY);
            UnivariateSolver solver = new BrentSolver(DEFAULT_ABSOLUTE_ACCURACY);
            int maxEval = 50; // 30

            try
            {
                double v = solver.solve(maxEval, new DelegateUnivariateFunction(f), 0, SysMath.Min(SysMath.Abs(r0), SysMath.Abs(r1)) * ClothoUtils.MAX_L);
                return v;
            }
            catch (TooManyEvaluationsException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private double GetL(double t)
        {
            t = t.Clamp(this.tmin, this.tmax);
            double dl = this.ttransform.Get(t);
            if (SysMath.Abs(dl) > ClothoUtils.GetMaxL(this.a))
            {
                throw new Exception("Longitud del arco por encima del máximo permitido.");
            }
            return dl;
        }

        private readonly double l0;
        private readonly double l1;
        private readonly double a;
        private readonly bool invertY;

        /// <summary>Transformacion que se aplica sobre la posicion.</summary>
        private readonly ITransform2D transform;

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
             *  s:      t * m + n;
             *  x:      a*sqrt(%pi)*fresnel_c(s/(a*sqrt(%pi)));
             *  y:      a*sqrt(%pi)*fresnel_s(s/(a*sqrt(%pi)));
             *  z:      1;
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
             *  s:      t * m + n;
             *  x:      a*sqrt(%pi)*fresnel_c(s/(a*sqrt(%pi)));
             *  y:      a*sqrt(%pi)*fresnel_s(s/(a*sqrt(%pi)));
             *  z:      1;
             *  result: diff([ x, y, z ], t, 1); 
             *  
             *  result: [m*cos((m*t+n)^2/(2*a^2)),m*sin((m*t+n)^2/(2*a^2)),0]
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
             *  s:      t * m + n;
             *  x:      a*sqrt(%pi)*fresnel_c(s/(a*sqrt(%pi)));
             *  y:      a*sqrt(%pi)*fresnel_s(s/(a*sqrt(%pi)));
             *  z:      1;
             *  result: diff([ x, y, z ], t, 2); 
             *  
             *  result: [-(m^2*(m*t+n)*sin((m*t+n)^2/(2*a^2)))/a^2,(m^2*(m*t+n)*cos((m*t+n)^2/(2*a^2)))/a^2,0]
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
             *  s:      t * m + n;
             *  x:      a*sqrt(%pi)*fresnel_c(s/(a*sqrt(%pi)));
             *  y:      a*sqrt(%pi)*fresnel_s(s/(a*sqrt(%pi)));
             *  z:      1;
             *  result: diff([ x, y, z ], t, 3); 
             *  
             *  result: [-(m^3*sin((m*t+n)^2/(2*a^2)))/a^2-(m^3*(m*t+n)^2*cos((m*t+n)^2/(2*a^2)))/a^4,(m^3*cos((m*t+n)^2/(2*a^2)))/a^2-(m^3*(m*t+n)^2*sin((m*t+n)^2/(2*a^2)))/a^4,0]
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
            get { return this.l1 - this.l0; }
        }

        public override double GetLength(double t0, double t1)
        {
            double dl0 = this.GetL(t0);
            double dl1 = this.GetL(t1);
            return dl1 - dl0;
        }

        #endregion
    }
}