using System;
using System.Diagnostics.Contracts;
using org.apache.commons.math3.exception;
using org.apache.commons.math3.analysis.solvers;
using SysMath = System.Math;

namespace Essence.Math.Double.Curves
{
    /// <summary>
    ///     Arco de clotoide.
    /// </summary>
    public class ClothoidArc2 : SimpleCurve2
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public ClothoidArc2(double l0,
                            Vec2d point0, Vec2d point1,
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
                    // No se permite cambio de signo en el radio.
                    Contract.Assert(false);
                }
            }

            if (SysMath.Abs(radius0) > SysMath.Abs(radius1))
            { // t positivas
                this.invertY = radius1 < 0;
            }
            else
            { // t negativa
                this.invertY = radius1 > 0;
            }

            // Diferencia de puntos en coordenadas reales.
            Vec2d v01 = point1.Sub(point0);

            this.a = SolveParam(v01.Length, radius0, radius1);

            // Desarrollo segun el radio en el punto (0) y (1).
            double l0_n = MathUtils.ClothoL(radius0, this.invertY, this.a);
            double l1_n = MathUtils.ClothoL(radius1, this.invertY, this.a);

            // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
            Vec2d p0_n = MathUtils.Clotho(l0_n, this.invertY, this.a);
            Vec2d p1_n = MathUtils.Clotho(l1_n, this.invertY, this.a);

            Vec2d v01_n = p1_n.Sub(p0_n);

            // Rotacion de v01_n -> v01.
            double r = vecMath.Angle(v01_n, v01);

            // Transformacion a aplicar.
            this.transform = Transform2.Translate(point1.X - p1_n.X, point1.Y - p1_n.Y)
                                       .Mult(Transform2.Rotate(p1_n.X, p1_n.Y, r));

            this.l0 = l0_n;
            this.l1 = l1_n;

            //this.tmin = this.l0;
            //this.tmax = this.l0 + (this.l1 - this.l0);
            //this.ttransform = new Transform1(this.l0, this.l1, this.tmin, this.tmax);

            this.SetTInterval(l0, l0 + (this.l1 - this.l0));
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
            return MathUtils.ClothoRadious(t, this.InvertY, this.A);
        }

        public new double GetTangent(double t)
        {
            return MathUtils.ClothoTangent(t, this.InvertY, this.A);
        }

        #region private

        private const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;
        private static readonly double sqrtpi = SysMath.Sqrt(SysMath.PI);
        private static readonly DoubleMath math = DoubleMath.Instance;
        private static readonly VecMath<double, DoubleMath, Vec2d, Vec2dFactory> vecMath = VecMath<double, DoubleMath, Vec2d, Vec2dFactory>.Instance;

        /// <summary>
        /// Resuelve el parametro de la clotoide dado los radios <c>r0</c> y <c>r1</c> Con una distancia <c>d</c> entre los puntos.
        /// </summary>
        private static double SolveParam(double d, double r0, double r1)
        {
            Func<double, double> f = (a) =>
            {
                double fs0, fc0;
                MathUtils.Fresnel(a / (r0 * sqrtpi), out fs0, out fc0);

                double fs1, fc1;
                MathUtils.Fresnel(a / (r1 * sqrtpi), out fs1, out fc1);

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
                double v = solver.solve(maxEval, new DelegateUnivariateFunction(f), 0, SysMath.Min(SysMath.Abs(r0), SysMath.Abs(r1)) * MathUtils.MAX_L);
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
            Contract.Assert(SysMath.Abs(dl) <= MathUtils.GetMaxL(this.a));
            return dl;
        }

        private readonly double l0;
        private readonly double l1;
        private readonly double a;
        private readonly bool invertY;

        /// <summary>Transformacion que se aplica sobre la posicion.</summary>
        private readonly Transform2 transform;

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

        public override Vec2d GetPosition(double t)
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
            Vec2d pt = MathUtils.Clotho(dl, this.InvertY, this.A);
            return this.transform.TransformPoint(pt);
        }

        public override Vec2d GetFirstDerivative(double t)
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
            Vec2d v = MathUtils.DClotho(dl, this.InvertY, this.A);
            v = v.Mul(m);
            return this.transform.TransformVector(v);
        }

        public override Vec2d GetSecondDerivative(double t)
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
            Vec2d v = MathUtils.DClotho2(dl, this.InvertY, this.A);
            v = v.Mul(m2);
            return this.transform.TransformVector(v);
        }

        public override Vec2d GetThirdDerivative(double t)
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
            Vec2d v = MathUtils.DClotho3(dl, this.InvertY, this.A);
            v = v.Mul(m3);
            return this.transform.TransformVector(v);
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

#if false
/// <summary>
///     Constructor.
/// </summary>
        public ClothoidArc2d(double l0, double l1,
                             Vec2d point0, Vec2d point1,
                             double tg1,
                             double radius0, double radius1,
                             double a)
        {
            Contract.Requires(a > 0);

            this.l0 = l0;
            this.l1 = l1;
            this.point0 = point0;
            this.point1 = point1;
            this.tg1 = tg1;
            this.radius0 = radius0;
            this.radius1 = radius1;
            this.a = a;

            if (double.IsInfinity(this.radius1))
            {
                // Rotacion.
                double r = this.tg1;

                // Transformacion a aplicar.
                this.transform = Transform2.Rotate(0, 0, r).Mult(Transform2.Translate(this.point1.X - 0, this.point1.Y - 0));

                // Estación del mango.
                this.diff = this.l1;
            }
            else
            {
                // Desarrollo segun el radio en el punto (1).
                double lf = MathUtils.ClothoL(this.radius1, this.radius1 < 0, this.a);

                // Coordenadas en el punto (1) para una clotoide normalizada.
                Vec2d pf = MathUtils.Clotho(lf, this.radius1 < 0, this.a);

                // Tangente en el punto (1).
                double tf = MathUtils.ClothoTangent(lf, this.radius1 < 0, this.a);

                // Rotacion.
                double r = this.tg1 - tf;

                // Transformacion a aplicar.
                this.transform = Transform2.Rotate(pf.X, pf.Y, r).Mult(Transform2.Translate(this.point1.X - pf.X, this.point1.Y - pf.Y));

                // Estación del mango.
                this.diff = this.l1 - lf;
            }
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public ClothoidArc2d(double l0,
                             Vec2d point0, Vec2d point1,
                             double radius0, double radius1,
                             double a)
        {
            Contract.Requires(a > 0);

            //if (double.IsInfinity(radius1))
            {
                /*// Rotacion.
                double r = this.tg1;

                // Transformacion a aplicar.
                this.transform = Transform2.Rotate(0, 0, r).Mult(Transform2.Translate(this.point1.X - 0, this.point1.Y - 0));

                // Estación del mango.
                this.diff = this.l1;*/
            }
            //else
            {
                this.a = a;
                this.invertY = radius1 < 0;

                // Desarrollo segun el radio en el punto (0) y (1).
                double l0_n = MathUtils.ClothoL(radius0, this.invertY, this.a);
                double l1_n = MathUtils.ClothoL(radius1, this.invertY, this.a);

                // Coordenadas en el punto (0) y (1) para una clotoide normalizada.
                Vec2d p0_n = MathUtils.Clotho(l0_n, this.invertY, this.a);
                Vec2d p1_n = MathUtils.Clotho(l1_n, this.invertY, this.a);

                Vec2d v01_n = p1_n.Sub(p0_n);

                // Tangente en el punto (0) y (1).
                //double tg0_n = MathUtils.ClothoTangent(l0_n, this.invertY, this.a);
                //double tg1_n = MathUtils.ClothoTangent(l1_n, this.invertY, this.a);

                // Rotacion.
                Vec2d v01 = point1.Sub(point0);

                // Rotacion de v01_n -> v01.
                double r = vecMath.Angle(v01_n, v01);

                // Transformacion a aplicar.
                this.transform = Transform2.Translate(point1.Sub(p1_n))
                                           .Mult(Transform2.Rotate(p1_n, r));

                // Estación del mango.
                this.diff = l0_n - l0;

                this.l0 = l0;
                this.l1 = this.l0 + (l1_n - l0_n);
            }
        }
#endif
