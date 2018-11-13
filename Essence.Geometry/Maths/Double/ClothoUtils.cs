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
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Maths.Double
{
    public static class ClothoUtils
    {
        #region Fresnel integrals

        /// <summary>
        /// Integral del Fresnel: Integral de Fresnel S(z) = integrate(sin((%pi / 2)*t^2), t, 0, z).
        /// </summary>
        public static double FresnelS(double z)
        {
            double sz, cz;
            Fresnel(z, out sz, out cz);
            return sz;
        }

        /// <summary>
        /// Integral del Fresnel: Integral de Fresnel C(z) = integrate(cos((%pi / 2)*t^2), t, 0, z).
        /// </summary>
        public static double FresnelC(double z)
        {
            double sz, cz;
            Fresnel(z, out sz, out cz);
            return cz;
        }

        /// <summary>
        /// Derivada iesima de la integral del Fresnel (i = 1..4).
        /// </summary>
        public static double DFresnelS(double z, int i = 1)
        {
            double zz = (SysMath.PI / 2) * z * z;
            switch (i)
            {
                case 1:
                    return SysMath.Sin(zz);
                case 2:
                    return SysMath.PI * z * SysMath.Cos(zz);
                case 3:
                    return SysMath.PI * SysMath.Cos(zz) - PI_e2 * z * z * SysMath.Sin(zz);
                case 4:
                    return -3 * PI_e2 * z * SysMath.Sin(zz) - PI_e3 * z * z * z * SysMath.Cos(zz);
                case 5:
                    return -3 * PI_e2 * SysMath.Sin(zz) - 6 * PI_e3 * z * z * SysMath.Cos(zz) + PI_e4 * z * z * z * z * SysMath.Sin(zz);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Derivada iesima de la integral del Fresnel (i = 1..4).
        /// </summary>
        public static double DFresnelC(double z, int i = 1)
        {
            double zz = (SysMath.PI / 2) * z * z;
            switch (i)
            {
                case 1:
                    return SysMath.Cos(zz);
                case 2:
                    return -SysMath.PI * z * SysMath.Sin(zz);
                case 3:
                    return -SysMath.PI * SysMath.Sin(zz) - PI_e2 * z * z * SysMath.Cos(zz);
                case 4:
                    return -3 * PI_e2 * z * SysMath.Cos(zz) + PI_e3 * z * z * z * SysMath.Sin(zz);
                case 5:
                    return -3 * PI_e2 * SysMath.Cos(zz) + 6 * PI_e3 * z * z * SysMath.Sin(zz) + PI_e4 * z * z * z * z * SysMath.Cos(zz);
            }
            throw new NotImplementedException();
        }

        #endregion

        public static void Clotho(double s, bool invertY, double a, out double x, out double y)
        {
            double a_sqrtpi = a * sqrtpi;

            double fs, fc;
            Fresnel(s / a_sqrtpi, out fs, out fc);

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

        /*public static double ClothoLeftNormal(double s, bool invertY, double a)
        {
            return ClothoTangent(s, invertY, a) + SysMath.PI / 2;
        }

        public static double ClothoRightNormal(double s, bool invertY, double a)
        {
            return ClothoTangent(s, invertY, a) - SysMath.PI / 2;
        }*/

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

        /* S(x) for small x */

        private static readonly double[] sn =
        {
            -2.99181919401019853726E3,
            7.08840045257738576863E5,
            -6.29741486205862506537E7,
            2.54890880573376359104E9,
            -4.42979518059697779103E10,
            3.18016297876567817986E11,
        };

        private static readonly double[] sd =
        {
            /* 1.00000000000000000000E0,*/
            2.81376268889994315696E2,
            4.55847810806532581675E4,
            5.17343888770096400730E6,
            4.19320245898111231129E8,
            2.24411795645340920940E10,
            6.07366389490084639049E11,
        };

        /* C(x) for small x */

        private static readonly double[] cn =
        {
            -4.98843114573573548651E-8,
            9.50428062829859605134E-6,
            -6.45191435683965050962E-4,
            1.88843319396703850064E-2,
            -2.05525900955013891793E-1,
            9.99999999999999998822E-1,
        };

        private static readonly double[] cd =
        {
            3.99982968972495980367E-12,
            9.15439215774657478799E-10,
            1.25001862479598821474E-7,
            1.22262789024179030997E-5,
            8.68029542941784300606E-4,
            4.12142090722199792936E-2,
            1.00000000000000000118E0,
        };

        /* Auxiliary function f(x) */

        private static readonly double[] fn =
        {
            4.21543555043677546506E-1,
            1.43407919780758885261E-1,
            1.15220955073585758835E-2,
            3.45017939782574027900E-4,
            4.63613749287867322088E-6,
            3.05568983790257605827E-8,
            1.02304514164907233465E-10,
            1.72010743268161828879E-13,
            1.34283276233062758925E-16,
            3.76329711269987889006E-20,
        };

        private static readonly double[] fd =
        {
            /*  1.00000000000000000000E0,*/
            7.51586398353378947175E-1,
            1.16888925859191382142E-1,
            6.44051526508858611005E-3,
            1.55934409164153020873E-4,
            1.84627567348930545870E-6,
            1.12699224763999035261E-8,
            3.60140029589371370404E-11,
            5.88754533621578410010E-14,
            4.52001434074129701496E-17,
            1.25443237090011264384E-20,
        };

        /* Auxiliary function g(x) */

        private static readonly double[] gn =
        {
            5.04442073643383265887E-1,
            1.97102833525523411709E-1,
            1.87648584092575249293E-2,
            6.84079380915393090172E-4,
            1.15138826111884280931E-5,
            9.82852443688422223854E-8,
            4.45344415861750144738E-10,
            1.08268041139020870318E-12,
            1.37555460633261799868E-15,
            8.36354435630677421531E-19,
            1.86958710162783235106E-22,
        };

        private static readonly double[] gd =
        {
            /*  1.00000000000000000000E0,*/
            1.47495759925128324529E0,
            3.37748989120019970451E-1,
            2.53603741420338795122E-2,
            8.14679107184306179049E-4,
            1.27545075667729118702E-5,
            1.04314589657571990585E-7,
            4.60680728146520428211E-10,
            1.10273215066240270757E-12,
            1.38796531259578871258E-15,
            8.39158816283118707363E-19,
            1.86958710162783236342E-22,
        };

        private static readonly double sqrtpi = SysMath.Sqrt(SysMath.PI);
        private const double PI_e2 = SysMath.PI * SysMath.PI;
        private const double PI_e3 = PI_e2 * SysMath.PI;
        private const double PI_e4 = PI_e3 * SysMath.PI;

        private static double polevl(double x, double[] coef, int n)
        {
            double ans;
            //double p = coef;
            int icoef = 0;
            int i;

            ans = coef[icoef];
            icoef++;
            i = n;

            do
            {
                ans = ans * x + coef[icoef];
                icoef++;
                i--;
            }
            while (i > 0);

            return ans;
        }

        private static double p1evl(double x, double[] coef, int n)
        {
            double ans;
            //double p = coef;
            int icoef = 0;
            int i;

            ans = x + coef[icoef];
            icoef++;
            i = n - 1;

            do
            {
                ans = ans * x + coef[icoef];
                icoef++;
                i--;
            }
            while (i > 0);

            return ans;
        }

        /// <summary>
        /// Calcula las integrales de Fresnel.
        /// </summary>
        /// <param name="z">Desarrollo de la integral/longitud arco.</param>
        /// <param name="sz">S(z)</param>
        /// <param name="cz">C(z)</param>
        internal static void Fresnel(double z, out double sz, out double cz)
        {
            double f, g, cc, ss, c, s, t, u;
            double x, x2;

            x = SysMath.Abs(z);
            x2 = x * x;

            if (x2 < 2.5625)
            {
                t = x2 * x2;
                ss = x * x2 * polevl(t, sn, 5) / p1evl(t, sd, 6);
                cc = x * polevl(t, cn, 5) / polevl(t, cd, 6);
            }
            else if (x > 36974.0)
            {
                cc = 0.5;
                ss = 0.5;
            }
            else
            {
                x2 = x * x;
                t = SysMath.PI * x2;
                u = 1.0 / (t * t);
                t = 1.0 / t;
                f = 1.0 - u * polevl(u, fn, 9) / p1evl(u, fd, 10);
                g = t * polevl(u, gn, 10) / p1evl(u, gd, 11);

                t = SysMath.PI * 0.5 * x2;
                c = SysMath.Cos(t);
                s = SysMath.Sin(t);
                t = SysMath.PI * x;
                cc = 0.5 + (f * s - g * c) / t;
                ss = 0.5 - (f * c + g * s) / t;
            }

            if (z < 0.0)
            {
                cc = -cc;
                ss = -ss;
            }

            cz = cc;
            sz = ss;
        }

        #endregion
    }
}