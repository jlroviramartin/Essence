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
using System.Diagnostics;
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using SysMath = System.Math;

/// <summary>
/// Clothoid:
/// 
/// </summary>
namespace Essence.Maths.Double
{
    public static class ClothoUtils
    {
        /// <summary>
        /// Evaluates the A parameter of the clothoid equation: A^2 = R*l.
        /// This method uses an aproximation.
        /// </summary>
        /// <param name="len">Length of the clothoid arc between points 1 and 2.</param>
        /// <param name="r1">Radious ot the clothoid arc at point 1.</param>
        /// <param name="r2">Radious ot the clothoid arc at point 2.</param>
        /// <returns>Clothoid parameter.</returns>
        public static double SolveParam_2(double len, double r1, double r2)
        {
            Func<double, double> f = (a) =>
            {
                double fs1, fc1;
                FresnelUtils.Fresnel(a / (r1 * sqrtpi), out fs1, out fc1);

                double fs2, fc2;
                FresnelUtils.Fresnel(a / (r2 * sqrtpi), out fs2, out fc2);

                double fc21 = (fc2 - fc1);
                double fs21 = (fs2 - fs1);

                return a * a * SysMath.PI * (fc21 * fc21 + fs21 * fs21) - len * len;
            };

            int maxEval = 50; // 30

            try
            {
                double min = 0;
                double max = SysMath.Min(SysMath.Abs(r1), SysMath.Abs(r2)) * MAX_L;
                return Solver.Solve(f, min, max, Solver.Type.Brent, Solver.DEFAULT_ABSOLUTE_ACCURACY, maxEval);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Evaluates the A parameter of the clothoid equation: A^2 = R*l.
        /// This method gets the exact value.
        /// Solve the equation system:
        /// <![CDATA[
        /// if r1 = inf
        ///     A^2 = r2 * len (l2 = len)
        /// if r2 = inf
        ///     A^2 = r1 * len (l1 = len)
        /// else
        ///     A^2 = r1 * l1
        ///     A^2 = r2 * l2
        ///     if l2 > l1
        ///         l2 - l1 = len
        ///     else (l2 < l1)
        ///         l1 - l2 = len
        /// ]]>
        /// </summary>
        /// <param name="len">Length of the clothoid arc between points 1 and 2.</param>
        /// <param name="r1">Radious ot the clothoid arc at point 1.</param>
        /// <param name="r2">Radious ot the clothoid arc at point 2.</param>
        /// <returns>Clothoid parameter.</returns>
        public static double SolveParam(double len, double r1, double r2)
        {
            // A^2
            double a2;
            if (double.IsInfinity(r1))
            {
                a2 = r2 * len;
            }
            else if (double.IsInfinity(r2))
            {
                a2 = r1 * len;
            }
            else
            {
                a2 = (r1 * r2 * len) / (r1 - r2);
            }
            return Math.Sqrt(Math.Abs(a2));
        }

        /// <summary>
        /// Evaluates the length of the clothoid equation: A^2 / R = l.
        /// This method gets the exact value.
        /// Solve the equation system:
        /// <![CDATA[
        /// if r1 = inf
        ///     len = A^2 / r2 (l2 = len)
        /// if r2 = inf
        ///     len = A^2 / r1 (l1 = len)
        /// else
        ///     l1 = A^2 / r1
        ///     l2 = A^2 / r2
        ///     if l2 > l1
        ///         len = l2 - l1
        ///     else (l2 < l1)
        ///         len = l1 - l2
        /// ]]>
        /// </summary>
        /// <param name="len">Length of the clothoid arc between points 1 and 2.</param>
        /// <param name="r1">Radious ot the clothoid arc at point 1.</param>
        /// <param name="r2">Radious ot the clothoid arc at point 2.</param>
        /// <returns>Clothoid parameter.</returns>
        public static double SolveLength(double a, double r1, double r2)
        {
            double a2 = a * a;
            double len;
            if (double.IsInfinity(r1))
            {
                len = a2 / r2;
            }
            else if (double.IsInfinity(r2))
            {
                len = a2 / r1;
            }
            else
            {
                len = Math.Abs((a2 / r2) - (a2 / r1));
            }
            return len;
        }

        public static void Clotho(double s, bool invertY, double a, out double x, out double y)
        {
            double a_sqrtpi = a * sqrtpi;

            double fs, fc;
            FresnelUtils.Fresnel(s / a_sqrtpi, out fs, out fc);

            x = a_sqrtpi * fc;
            y = a_sqrtpi * fs;

            if (invertY)
            {
                y = -y;
            }
        }

        public static Point2d Clotho(double s, bool invertY, double a)
        {
            double x, y;
            Clotho(s, invertY, a, out x, out y);
            return new Point2d(x, y);
        }

        /// <summary>
        /// Calcula el radios de la curva.
        /// </summary>
        public static double ClothoRadious(double s, bool invertY, double a)
        {
            double radius;
            if (s.EpsilonEquals(0))
            {
                radius = MathUtils.SafeSign(s) * double.PositiveInfinity; // Punto de inflexion, puede ser (+) o (-).
            }
            else
            {
                radius = (a * a / s);

                if (SysMath.Abs(radius) >= MAX_RADIUS)
                {
                    radius = SysMath.Sign(radius) * double.PositiveInfinity;
                }
            }

            if (invertY)
            {
                radius = -radius;
            }
            return radius;
        }

        /// <summary>
        /// Calcula el desarrollo de la curva para el radio indicado.
        /// </summary>
        public static double ClothoL(double r, bool invertY, double a)
        {
            double l;
            if (SysMath.Abs(r) >= MAX_RADIUS)
            {
                //r = SysMath.Sign(r) * double.PositiveInfinity;
                l = 0;
            }
            else
            {
                l = (a * a) / r;
            }

            if (invertY)
            {
                l = -l;
            }
            return l;
        }

        /// <summary>
        /// Busca el desarrollo de la clotoide cuya tangente es <c>v</c>: DClotho x v = 0.
        /// Por simetria, existen 2 soluciones, el desarrollo positivo y el negativo.
        /// </summary>
        public static double FindTangent(bool invertY, double a, Vector2d v)
        {
            int ysign = (invertY ? -1 : 1);
            return SysMath.Sqrt(2 * a * a * SysMath.Atan2(ysign * v.Y, v.X));
        }

        /// <summary>
        /// Busca el desarrollo de la clotoide cuya tangente (en radianes) es <c>tg</c>.
        /// Por simetria, existen 2 soluciones, el desarrollo positivo y el negativo.
        /// </summary>
        public static double FindTangent(bool invertY, double a, double tg)
        {
            if (invertY)
            {
                tg = 2 * SysMath.PI - tg;
            }

            tg = AngleUtils.Ensure0To2Pi(tg, true);

            double dl = SysMath.Sqrt(2 * a * a * tg);
            return dl;
        }

        /// <summary>
        /// Calcula el maximo desarrollo de la curva (pasado dicho desarrollo, aumenta el error).
        /// </summary>
        public static double GetMaxL(double a)
        {
            return MAX_L * a;
        }

        /// <summary>
        /// Calcula el mínimo rádio de la curva (por debajo de dicho desarrollo, aumenta el error).
        /// </summary>
        public static double GetMinRadius(double a)
        {
            return a / MAX_L;
        }

        public static double ClothoTangent(double s, bool invertY, double a)
        {
            double dir = ((s * s) / (2 * a * a));

            if (invertY)
            {
                dir = -dir;
            }
            return dir;
        }

        public static Vector2d ClothoDir(double s, bool invertY, double a)
        {
            double tg = ClothoTangent(s, invertY, a);
            return Vector2d.NewRotate(tg);
        }

        public static Vector2d ClothoLeftNormal(double s, bool invertY, double a)
        {
            return ClothoDir(s, invertY, a).PerpLeft;
        }

        public static Vector2d ClothoRightNormal(double s, bool invertY, double a)
        {
            return ClothoDir(s, invertY, a).PerpRight;
        }

        #region Derivadas

        public static void DClotho(double s, bool invertY, double a, out double x, out double y)
        {
            double s2_2a2 = (s * s) / (2 * a * a);

            x = SysMath.Cos(s2_2a2);
            y = SysMath.Sin(s2_2a2);

            if (invertY)
            {
                y = -y;
            }
        }

        public static Vector2d DClotho(double s, bool invertY, double a)
        {
            double x, y;
            DClotho(s, invertY, a, out x, out y);
            return new Vector2d(x, y);
        }

        public static void DClotho2(double s, bool invertY, double a, out double x, out double y)
        {
            double s2_2a2 = (s * s) / (2 * a * a);

            double s_s2 = s / (a * a);

            x = -SysMath.Sin(s2_2a2) * s_s2;
            y = SysMath.Cos(s2_2a2) * s_s2;

            if (invertY)
            {
                y = -y;
            }
        }

        public static Vector2d DClotho2(double s, bool invertY, double a)
        {
            double x, y;
            DClotho2(s, invertY, a, out x, out y);
            return new Vector2d(x, y);
        }

        public static void DClotho3(double s, bool invertY, double a, out double x, out double y)
        {
            double s2_2a2 = (s * s) / (2 * a * a);
            double sin = SysMath.Sin(s2_2a2);
            double cos = SysMath.Cos(s2_2a2);

            double a2 = a * a;
            double one_a2 = 1 / a2;
            double s2_a4 = (s * s) / (a2 * a2);

            x = -sin * one_a2 - cos * s2_a4;
            y = cos * one_a2 - sin * s2_a4;

            if (invertY)
            {
                y = -y;
            }
        }

        public static Vector2d DClotho3(double s, bool invertY, double a)
        {
            double x, y;
            DClotho3(s, invertY, a, out x, out y);
            return new Vector2d(x, y);
        }

        #endregion

        #region private

        /// <summary>Radio apartir del cual se considera una recta.</summary>
        public const double MAX_RADIUS = 1e20;

        /// <summary>Valor de corte para el desarrollo de la clotoide.</summary>
        internal const double MAX_L = 2.23; // sqrt(5)

        private static readonly double sqrtpi = SysMath.Sqrt(SysMath.PI);

        #endregion
    }
}