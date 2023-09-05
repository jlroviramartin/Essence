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
using SysMath = System.Math;
using static System.Math;

namespace Essence.Maths.Double
{
    /// <summary>
    /// Fresnel integrals.
    /// <c><![CDATA[
    ///           x
    ///           -
    ///          | |
    /// C(x) =   |   cos(pi/2 t**2) dt,
    ///        | |
    ///         -
    ///          0
    ///
    ///           x
    ///           -
    ///          | |
    /// S(x) =   |   sin(pi/2 t**2) dt.
    ///        | |
    ///         -
    ///          0
    /// ]]></c>
    /// NOTE: maxima fresnel_s(z) and fresnel_c(z) uses the same integral.
    /// http://maxima.sourceforge.net/docs/manual/es/maxima_15.html
    /// 
    /// http://www.netlib.org/cephes/misc.tgz
    /// http://www.opendrive.org/tools/odrSpiral.zip
    /// </summary>
    public static class FresnelUtils
    {
        /// <summary>
        /// Evaluates the Fresnel integrals.
        /// <c><![CDATA[
        ///           x
        ///           -
        ///          | |
        /// C(x) =   |   cos(pi/2 t**2) dt,
        ///        | |
        ///         -
        ///          0
        /// 
        ///           x
        ///           -
        ///          | |
        /// S(x) =   |   sin(pi/2 t**2) dt.
        ///        | |
        ///         -
        ///          0
        /// ]]></c>
        /// The integrals are evaluated by a power series for <![CDATA[x < 1]]>.
        /// For <![CDATA[x >= 1]]> auxiliary functions f(x) and g(x) are employed
        /// such that
        /// C(x) = 0.5 + f(x) sin( pi/2 x**2 ) - g(x) cos( pi/2 x**2 )
        /// S(x) = 0.5 - f(x) cos( pi/2 x**2 ) - g(x) sin( pi/2 x**2 )
        /// </summary>
        /// <param name="z">Desarrollo de la integral/longitud arco.</param>
        /// <param name="sz">S(z)</param>
        /// <param name="cz">C(z)</param>
        public static void Fresnel(double z, out double sz, out double cz)
        {
            double f, g, cc, ss, c, s, t, u;
            double x, x2;

            x = Abs(z);
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
                // Asymptotic power series auxiliary functions
                // for large argument
                x2 = x * x;
                t = PI * x2;
                u = 1.0 / (t * t);
                t = 1.0 / t;
                f = 1.0 - u * polevl(u, fn, 9) / p1evl(u, fd, 10);
                g = t * polevl(u, gn, 10) / p1evl(u, gd, 11);

                t = PI * 0.5 * x2;
                c = Cos(t);
                s = Sin(t);
                t = PI * x;
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

        /// <summary>
        /// Derivada iesima de la integral del Fresnel (i = 1..5).
        /// </summary>
        public static void DFresnel(double z, out double sz, out double cz, int i = 1)
        {
            double zz = (PI / 2) * z * z;
            double s = Sin(zz);
            double c = Cos(zz);

            switch (i)
            {
                case 1:
                    sz = s;
                    cz = c;
                    return;
                case 2:
                {
                    double PI_z = PI * z;

                    sz = PI_z * c;
                    cz = -PI_z * s;
                    return;
                }
                case 3:
                {
                    double PI_e2_z2 = PI_e2 * z * z;

                    sz = PI * c - PI_e2_z2 * s;
                    cz = -PI * s - PI_e2_z2 * c;
                    return;
                }
                case 4:
                {
                    double PI_e2_3_z = 3 * PI_e2 * z;
                    double PI_e3_z3 = PI_e3 * z * z * z;

                    sz = -PI_e2_3_z * s - PI_e3_z3 * c;
                    cz = -PI_e2_3_z * c + PI_e3_z3 * s;
                    return;
                }
                case 5:
                {
                    double PI_e2_3 = 3 * PI_e2;
                    double PI_e3_6_z2 = 6 * PI_e3 * z * z;
                    double PI_e4_z4 = PI_e4 * z * z * z * z;

                    sz = -PI_e2_3 * s - PI_e3_6_z2 * c + PI_e4_z4 * s;
                    cz = -PI_e2_3 * c + PI_e3_6_z2 * s + PI_e4_z4 * c;
                    return;
                }
            }
            throw new NotImplementedException();
        }

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
        /// Derivada iesima de la integral del Fresnel (i = 1..5).
        /// </summary>
        public static double DFresnelS(double z, int i = 1)
        {
            double zz = (PI / 2) * z * z;
            switch (i)
            {
                case 1:
                    return Sin(zz);
                case 2:
                    return PI * z * Cos(zz);
                case 3:
                    return PI * Cos(zz) - PI_e2 * z * z * Sin(zz);
                case 4:
                    return -3 * PI_e2 * z * Sin(zz) - PI_e3 * z * z * z * Cos(zz);
                case 5:
                    return -3 * PI_e2 * Sin(zz) - 6 * PI_e3 * z * z * Cos(zz) + PI_e4 * z * z * z * z * Sin(zz);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Derivada iesima de la integral del Fresnel (i = 1..5).
        /// </summary>
        public static double DFresnelC(double z, int i = 1)
        {
            double zz = (PI / 2) * z * z;
            switch (i)
            {
                case 1:
                    return Cos(zz);
                case 2:
                    return -PI * z * Sin(zz);
                case 3:
                    return -PI * Sin(zz) - PI_e2 * z * z * Cos(zz);
                case 4:
                    return -3 * PI_e2 * z * Cos(zz) + PI_e3 * z * z * z * Sin(zz);
                case 5:
                    return -3 * PI_e2 * Cos(zz) + 6 * PI_e3 * z * z * Sin(zz) + PI_e4 * z * z * z * z * Cos(zz);
            }
            throw new NotImplementedException();
        }

        #region private

        /* S(x) for small x */

        /** S(x) for small x numerator. */
        private static readonly double[] sn =
        {
            -2.99181919401019853726E3,
            7.08840045257738576863E5,
            -6.29741486205862506537E7,
            2.54890880573376359104E9,
            -4.42979518059697779103E10,
            3.18016297876567817986E11,
        };

        /** S(x) for small x denominator. */
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

        /** C(x) for small x numerator. */
        private static readonly double[] cn =
        {
            -4.98843114573573548651E-8,
            9.50428062829859605134E-6,
            -6.45191435683965050962E-4,
            1.88843319396703850064E-2,
            -2.05525900955013891793E-1,
            9.99999999999999998822E-1,
        };

        /** C(x) for small x denominator. */
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

        /** Auxiliary function f(x) numerator. */
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

        /** Auxiliary function f(x) denominator. */
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

        /** Auxiliary function g(x) numerator. */
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

        /** Auxiliary function g(x) denominator. */
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

        private const double PI_e2 = PI * PI;
        private const double PI_e3 = PI_e2 * PI;
        private const double PI_e4 = PI_e3 * PI;

        /**
         * Compute a polynomial in x.
         * @param x double; x
         * @param coef double[]; coefficients
         * @return polynomial in x
         */
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

        /**
         * Compute a polynomial in x.
         * @param x double; x
         * @param coef double[]; coefficients
         * @return polynomial in x
         */
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

        #endregion
    }
}