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
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantCast

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using REAL = System.Double;

namespace Essence.Util.Math.Double
{
    public static class MathUtils
    {
        /// <summary>Error.</summary>
        public const double EPSILON = (double)1e-09;

        /// <summary>Tolerancia a cero.</summary>
        public const double ZERO_TOLERANCE = (double)1e-08;

        // NOTA: mejor utilizar Math.Round(double value, int decimals)
        public static double Round(double value, int decimals)
        {
            Contract.Assert(decimals < round.Length);
            double r = round[decimals];
            return System.Math.Round(value * r) / r;
        }

        public static void MinMax(double a, double b, out double min, out double max)
        {
            if (a > b)
            {
                min = b;
                max = a;
            }
            else
            {
                min = a;
                max = b;
            }
        }

        public static double Reparam(double t, double a, double b, double c, double d)
        {
            double tt = ((t - a) / (b - a));
            return c + tt * (d - c);
        }

        internal static double InvSqrt(double value)
        {
            if (value.EpsilonZero())
            {
                throw new Exception("Division by zero in InvSqr");
                //return 0;
            }

            return (double)1.0 / (double)System.Math.Sqrt(value);
        }

        /// <summary>
        ///     Comprueba si el valor <c>v</c> no es NaN ni Infinito.
        /// </summary>
        /// <param name="v">Valor.</param>
        /// <returns>Indica si no es NaN ni Infinito.</returns>
        public static bool IsValid(this double v)
        {
            return (!double.IsNaN(v) && !double.IsInfinity(v));
        }

        public static double Max(params double[] values)
        {
            switch (values.Length)
            {
                case 0:
                    throw new Exception();
                case 1:
                    return values[0];
                case 2:
                    return System.Math.Max(values[0], values[1]);
                default:
                    double max = values[0];
                    for (int i = 1; i < values.Length; i++)
                    {
                        double v = values[i];
                        if (v > max)
                        {
                            max = v;
                        }
                    }
                    return max;
            }
        }

        public static double Min(params double[] values)
        {
            switch (values.Length)
            {
                case 0:
                    throw new Exception();
                case 1:
                    return values[0];
                case 2:
                    return System.Math.Min(values[0], values[1]);
                default:
                    double min = values[0];
                    for (int i = 1; i < values.Length; i++)
                    {
                        double v = values[i];
                        if (v < min)
                        {
                            min = v;
                        }
                    }
                    return min;
            }
        }

        public static double EpsilonClamp(this double v, double min, double max, double epsilon = ZERO_TOLERANCE)
        {
            if (v.EpsilonLE(min, epsilon))
            {
                return min;
            }
            if (v.EpsilonGE(max, epsilon))
            {
                return max;
            }
            return v;
        }

        public static double Clamp(this double v, double min, double max)
        {
            if (v < min)
            {
                return min;
            }
            if (v > max)
            {
                return max;
            }
            return v;
        }

        #region EpsilonZero, EpsilonEquals, EpsilonMayor, EpsilonMayorIgual, EpsilonMenor, EpsilonMenorIgual, EpsilonEntre

        public static int EpsilonSign(this double v, double epsilon = EPSILON)
        {
            return ((System.Math.Abs(v) <= epsilon) ? 0 : ((v < 0) ? -1 : 1));
        }

        /// <summary>
        ///     Comprueba si el valor <c>v</c> es igual a cero con un error.
        /// </summary>
        /// <param name="v">Valor.</param>
        /// <param name="epsilon">Error maximo.</param>
        /// <returns>Indica si son iguales.</returns>
        public static bool EpsilonZero(this double v, double epsilon = ZERO_TOLERANCE)
        {
            return (System.Math.Abs(v) <= epsilon);
        }

        /// <summary>
        ///     Comprueba si los valores <c>v1</c> y <c>v2</c> son iguales con un error.
        /// </summary>
        /// <param name="v1">Valor 1.</param>
        /// <param name="v2">Valor 2.</param>
        /// <param name="epsilon">Error maximo.</param>
        /// <returns>Indica si son iguales.</returns>
        public static bool EpsilonEquals(this double v1, double v2, double epsilon = EPSILON)
        {
            //return (System.Math.Abs(v2 - v1) <= epsilon);
            // Resuelve las comparaciones con PositiveInfinity / NegativeInfinity.
            if (v2 > v1)
            {
                return v2 <= v1 + epsilon;
            }
            else
            {
                return v1 <= v2 + epsilon;
            }
        }

        /// <summary>
        ///     Compara los valores <c>v1</c> y <c>v2</c>.
        /// </summary>
        /// <param name="v1">Valor 1.</param>
        /// <param name="v2">Valor 2.</param>
        /// <param name="epsilon">Error maximo.</param>
        /// <returns>Comparacion.</returns>
        public static int EpsilonCompareTo(this double v1, double v2, double epsilon = EPSILON)
        {
            if (v1.EpsilonEquals(v2, epsilon))
            {
                return 0;
            }
            return v1.CompareTo(v2);
        }

        /// <summary>
        ///     Comprueba si los valores <c>v1s</c> y <c>v2s</c> son iguales con un error.
        /// </summary>
        /// <param name="v1s">Valores 1.</param>
        /// <param name="v2s">Valores 2.</param>
        /// <param name="epsilon">Error maximo.</param>
        /// <returns>Indica si son iguales.</returns>
        public static bool EpsilonEquals(double[] v1s, double[] v2s, double epsilon = EPSILON)
        {
            Debug.Assert(v1s != null);
            Debug.Assert((v2s != null) && (v2s.Length == v1s.Length));

            for (int k = 0; k < v1s.Length; k++)
            {
                if (!v1s[k].EpsilonEquals(v2s[k], epsilon))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Comprueba si el valor <c>v</c> es mayor o igual que un minimo con un error.
        ///     <![CDATA[(min - error) <= v]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si el valor es mayor que el minimo.</returns>
        public static bool EpsilonGE(this double v, double min, double epsilon = EPSILON)
        {
            return ((min - epsilon) <= v);
        }

        /// <summary>
        ///     Comprueba si el valor <c>v</c> es extrictamente mayor que un minimo con un error.
        ///     <![CDATA[(min + error) < v]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si el valor es mayor que el minimo.</returns>
        public static bool EpsilonG(this double v, double min, double epsilon = EPSILON)
        {
            return ((min + epsilon) < v);
        }

        /// <summary>
        ///     Comprueba si el valor <c>v</c> es extrictamente menor que un maximo con un error.
        ///     <![CDATA[v < (max - error)]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="max">Maximo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si el valor es menor que el maximo.</returns>
        public static bool EpsilonL(this double v, double max, double epsilon = EPSILON)
        {
            return (v < (max - epsilon));
        }

        /// <summary>
        ///     Comprueba si el valor <c>v</c> es menor o igual que un maximo con un error.
        ///     <![CDATA[v <= (max + error)]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="max">Maximo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si el valor es menor que el maximo.</returns>
        public static bool EpsilonLE(this double v, double max, double epsilon = EPSILON)
        {
            return (v <= (max + epsilon));
        }

        /// <summary>
        ///     Comprueba si el valor <c>v</c> esta entre el minimo y maximo con un error.
        ///     <![CDATA[(min - error) <= v Y v <= (max + error)]]>
        ///     <![CDATA[ v en [ min - error,  max + error ] OPEN ]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="max">Maximo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si esta entre los valores minimo y maximo.</returns>
        public static bool EpsilonBetweenClosed(this double v, double min, double max, double epsilon = EPSILON)
        {
            return ((min - epsilon) <= v) && (v <= (max + epsilon));
        }

        /// <summary>
        ///     Comprueba si el valor <c>v</c> esta estrictamente comprendido entre el minimo y maximo con un error.
        ///     <![CDATA[(min - error) < v Y v < (max + error)]]>
        ///     <![CDATA[ v en ( min - error,  max + error ) CLOSED ]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="max">Maximo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si esta estrictamente entre los valores minimo y maximo.</returns>
        public static bool EpsilonBetweenOpen(this double v, double min, double max, double epsilon = EPSILON)
        {
            return ((min + epsilon) < v) && (v < (max - epsilon));
        }

        #endregion

        public static double Cuad(double d)
        {
            return d * d;
        }

        /// <summary>
        ///     Conversion de grados a radianes.
        /// </summary>
        /// <param name="grad">Grados.</param>
        /// <returns>Radianes.</returns>
        public static double Rad(double grad)
        {
            return (double)(grad * System.Math.PI / 180.0);
        }

        /// <summary>
        ///     Conversion de grados centesimales a radianes.
        /// </summary>
        /// <param name="grad">Grados centesimales.</param>
        /// <returns>Radianes.</returns>
        public static double CRad(double grad)
        {
            return (double)(grad * System.Math.PI / 200);
        }

        /// <summary>
        ///     Conversion de radianes a grados.
        /// </summary>
        /// <param name="rad">Radianes.</param>
        /// <returns>Grados.</returns>
        public static double Grad(double rad)
        {
            return (double)(rad * 180.0 / System.Math.PI);
        }

        /// <summary>
        ///     Une dos enumerados ordenados.
        /// </summary>
        public static IEnumerable<double> ConcatSorted(IEnumerable<double> t1, IEnumerable<double> t2, double error = EPSILON)
        {
            using (IEnumerator<double> enumer1 = t1.GetEnumerator())
            {
                using (IEnumerator<double> enumer2 = t2.GetEnumerator())
                {
                    bool next1 = enumer1.MoveNext();
                    bool next2 = enumer2.MoveNext();
                    while (next1 || next2)
                    {
                        if (next1 && next2)
                        {
                            if (enumer1.Current.EpsilonEquals(enumer2.Current, error))
                            {
                                yield return enumer1.Current;
                                next1 = enumer1.MoveNext();
                                next2 = enumer2.MoveNext();
                            }
                            else if (enumer1.Current < enumer2.Current)
                            {
                                yield return enumer1.Current;
                                next1 = enumer1.MoveNext();
                            }
                            else // if (enumer1.Current > enumer2.Current)
                            {
                                yield return enumer2.Current;
                                next2 = enumer2.MoveNext();
                            }
                        }
                        else if (next1)
                        {
                            yield return enumer1.Current;
                            next1 = enumer1.MoveNext();
                        }
                        else // if (next2)
                        {
                            yield return enumer2.Current;
                            next2 = enumer2.MoveNext();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Interpolación lineal.
        /// </summary>
        public static double Lerp(double d1, double d2, double alpha)
        {
            return (1 - alpha) * d1 + alpha * d2;
        }

        /// <summary>
        ///     Interpolación lineal.
        /// </summary>
        public static double Lerp(double x, double x1, double y1, double x2, double y2)
        {
            if (EpsilonEquals(x1, x2) || EpsilonEquals(y1, y2))
            {
                return y1;
            }
            return y1 + (y2 - y1) * (x - x1) / (x2 - x1);
        }

        public static IEnumerable<double> For(double min, double max, int c)
        {
            yield return min;

            double tt = (max - min) / (c + 1);

            double t = min;
            while (c > 0)
            {
                t += tt;
                yield return t;
                c--;
            }

            yield return max;
        }

        public static bool Between(this double v, double a, double b)
        {
            return v >= a && v <= b;
        }

        public static bool BetweenClosedOpen(this double v, double a, double b)
        {
            return v >= a && v < b;
        }

        public static int SafeSign(double v)
        {
            return ((v < 0) ? -1 : 1);
        }

        private static readonly double[] round;

        static MathUtils()
        {
            round = new double[16];
            round[0] = 1;
            for (int i = 1; i < round.Length; i++)
            {
                round[i] = round[i - 1] * 10;
            }
        }

        #region Inner clases

        /// <summary>
        ///     Compara los valores indicados.
        /// </summary>
        public sealed class EpsilonEqualityComparer : IEqualityComparer<double>, IEqualityComparer
        {
            public EpsilonEqualityComparer()
                : this(EPSILON)
            {
            }

            public EpsilonEqualityComparer(double epsilon)
            {
                this.epsilon = epsilon;
            }

            private readonly double epsilon;

            #region IComparer<REAL>

            public bool Equals(double x, double y)
            {
                return x.EpsilonEquals(y, this.epsilon);
            }

            public int GetHashCode(double x)
            {
                return x.GetHashCode();
            }

            #endregion

            #region IComparer

            bool IEqualityComparer.Equals(object x, object y)
            {
                Contract.Requires((x is double) && (y is double));
                return this.Equals((double)x, (double)y);
            }

            int IEqualityComparer.GetHashCode(object x)
            {
                Contract.Requires(x is double);
                return this.GetHashCode((double)x);
            }

            #endregion
        }

        /// <summary>
        ///     Compara los valores indicados.
        /// </summary>
        public sealed class EpsilonComparer : IComparer<double>, IComparer
        {
            public EpsilonComparer()
                : this(EPSILON)
            {
            }

            public EpsilonComparer(double epsilon)
            {
                this.epsilon = epsilon;
            }

            private readonly double epsilon;

            #region IComparer<REAL>

            public int Compare(double x, double y)
            {
                return x.EpsilonCompareTo(y, this.epsilon);
            }

            #endregion

            #region IComparer

            int IComparer.Compare(object x, object y)
            {
                Contract.Requires((x is double) && (y is double));
                return this.Compare((double)x, (double)y);
            }

            #endregion
        }

        /// <summary>
        ///     Compara los valores indicados.
        /// </summary>
        public sealed class Comparer : IComparer<double>, IComparer
        {
            #region IComparer<REAL>

            public int Compare(double x, double y)
            {
                return System.Math.Sign(x - y);
            }

            #endregion

            #region IComparer

            int IComparer.Compare(object x, object y)
            {
                Contract.Requires((x is double) && (y is double));
                return this.Compare((double)x, (double)y);
            }

            #endregion
        }

        #endregion
    }
}