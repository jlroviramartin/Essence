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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Essence.Util.Math.Float
{
    public static class MathUtils
    {
        /// <summary>Error.</summary>
        public const float EPSILON = 1e-05f;

        /// <summary>Tolerancia a cero.</summary>
        public const float ZERO_TOLERANCE = 1e-04f;

        public static void MinMax(float a, float b, out float min, out float max)
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

        public static float Reparam(float t, float a, float b, float c, float d)
        {
            float tt = ((t - a) / (b - a));
            return c + tt * (d - c);
        }

        internal static float InvSqrt(float value)
        {
            if (value.EpsilonZero())
            {
                throw new Exception("Division by zero in InvSqr");
                return 0;
            }

            return (float)1.0 / (float)System.Math.Sqrt(value);
        }

        /// <summary>
        /// Comprueba si el valor <c>v</c> no es NaN ni Infinito.
        /// </summary>
        /// <param name="v">Valor.</param>
        /// <returns>Indica si no es NaN ni Infinito.</returns>
        public static bool IsValid(this float v)
        {
            return (!float.IsNaN(v) && !float.IsInfinity(v));
        }

        public static float Max(params float[] values)
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
                    float max = values[0];
                    for (int i = 1; i < values.Length; i++)
                    {
                        float v = values[i];
                        if (v > max)
                        {
                            max = v;
                        }
                    }
                    return max;
            }
        }

        public static float Min(params float[] values)
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
                    float min = values[0];
                    for (int i = 1; i < values.Length; i++)
                    {
                        float v = values[i];
                        if (v < min)
                        {
                            min = v;
                        }
                    }
                    return min;
            }
        }

        public static float EpsilonClamp(this float v, float min, float max, float epsilon = ZERO_TOLERANCE)
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

        public static float Clamp(this float v, float min, float max)
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

        public static int EpsilonSign(this float v, float epsilon = EPSILON)
        {
            return ((System.Math.Abs(v) <= epsilon) ? 0 : ((v < 0) ? -1 : 1));
        }

        /// <summary>
        /// Comprueba si el valor <c>v</c> es igual a cero con un error.
        /// </summary>
        /// <param name="v">Valor.</param>
        /// <param name="epsilon">Error maximo.</param>
        /// <returns>Indica si son iguales.</returns>
        public static bool EpsilonZero(this float v, float epsilon = ZERO_TOLERANCE)
        {
            return (System.Math.Abs(v) <= epsilon);
        }

        /// <summary>
        /// Comprueba si los valores <c>v1</c> y <c>v2</c> son iguales con un error.
        /// </summary>
        /// <param name="v1">Valor 1.</param>
        /// <param name="v2">Valor 2.</param>
        /// <param name="epsilon">Error maximo.</param>
        /// <returns>Indica si son iguales.</returns>
        public static bool EpsilonEquals(this float v1, float v2, float epsilon = EPSILON)
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
        /// Compara los valores <c>v1</c> y <c>v2</c>.
        /// </summary>
        /// <param name="v1">Valor 1.</param>
        /// <param name="v2">Valor 2.</param>
        /// <param name="epsilon">Error maximo.</param>
        /// <returns>Comparacion.</returns>
        public static int EpsilonCompareTo(this float v1, float v2, float epsilon = EPSILON)
        {
            if (v1.EpsilonEquals(v2, epsilon))
            {
                return 0;
            }
            return v1.CompareTo(v2);
        }

        /// <summary>
        /// Comprueba si los valores <c>v1s</c> y <c>v2s</c> son iguales con un error.
        /// </summary>
        /// <param name="v1s">Valores 1.</param>
        /// <param name="v2s">Valores 2.</param>
        /// <param name="epsilon">Error maximo.</param>
        /// <returns>Indica si son iguales.</returns>
        public static bool EpsilonEquals(float[] v1s, float[] v2s, float epsilon = EPSILON)
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
        /// Comprueba si el valor <c>v</c> es mayor o igual que un minimo con un error.
        /// <![CDATA[(min - error) <= v]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si el valor es mayor que el minimo.</returns>
        public static bool EpsilonGE(this float v, float min, float epsilon = EPSILON)
        {
            return ((min - epsilon) <= v);
        }

        /// <summary>
        /// Comprueba si el valor <c>v</c> es extrictamente mayor que un minimo con un error.
        /// <![CDATA[(min + error) < v]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si el valor es mayor que el minimo.</returns>
        public static bool EpsilonG(this float v, float min, float epsilon = EPSILON)
        {
            return ((min + epsilon) < v);
        }

        /// <summary>
        /// Comprueba si el valor <c>v</c> es extrictamente menor que un maximo con un error.
        /// <![CDATA[v < (max - error)]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="max">Maximo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si el valor es menor que el maximo.</returns>
        public static bool EpsilonL(this float v, float max, float epsilon = EPSILON)
        {
            return (v < (max - epsilon));
        }

        /// <summary>
        /// Comprueba si el valor <c>v</c> es menor o igual que un maximo con un error.
        /// <![CDATA[v <= (max + error)]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="max">Maximo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si el valor es menor que el maximo.</returns>
        public static bool EpsilonLE(this float v, float max, float epsilon = EPSILON)
        {
            return (v <= (max + epsilon));
        }

        /// <summary>
        /// Comprueba si el valor <c>v</c> esta entre el minimo y maximo con un error.
        /// <![CDATA[(min - error) <= v Y v <= (max + error)]]>
        /// <![CDATA[ v en [ min - error,  max + error ] OPEN ]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="max">Maximo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si esta entre los valores minimo y maximo.</returns>
        public static bool EpsilonBetweenClosed(this float v, float min, float max, float epsilon = EPSILON)
        {
            return ((min - epsilon) <= v) && (v <= (max + epsilon));
        }

        /// <summary>
        /// Comprueba si el valor <c>v</c> esta estrictamente comprendido entre el minimo y maximo con un error.
        /// <![CDATA[(min - error) < v Y v < (max + error)]]>
        /// <![CDATA[ v en ( min - error,  max + error ) CLOSED ]]>
        /// </summary>
        /// <param name="v">Valor a comprobar.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="max">Maximo.</param>
        /// <param name="epsilon">Error.</param>
        /// <returns>Indica si esta estrictamente entre los valores minimo y maximo.</returns>
        public static bool EpsilonBetweenOpen(this float v, float min, float max, float epsilon = EPSILON)
        {
            return ((min + epsilon) < v) && (v < (max - epsilon));
        }

        #endregion

        public static float Cuad(float d)
        {
            return d * d;
        }

        /// <summary>
        /// Conversion de grados a radianes.
        /// </summary>
        /// <param name="grad">Grados.</param>
        /// <returns>Radianes.</returns>
        public static float Rad(float grad)
        {
            return (float)(grad * System.Math.PI / 180.0);
        }

        /// <summary>
        /// Conversion de grados centesimales a radianes.
        /// </summary>
        /// <param name="grad">Grados centesimales.</param>
        /// <returns>Radianes.</returns>
        public static float CRad(float grad)
        {
            return (float)(grad * System.Math.PI / 200);
        }

        /// <summary>
        /// Conversion de radianes a grados.
        /// </summary>
        /// <param name="rad">Radianes.</param>
        /// <returns>Grados.</returns>
        public static float Grad(float rad)
        {
            return (float)(rad * 180.0 / System.Math.PI);
        }

        /// <summary>
        /// Une dos enumerados ordenados.
        /// </summary>
        public static IEnumerable<float> ConcatSorted(IEnumerable<float> t1, IEnumerable<float> t2, float error = EPSILON)
        {
            using (IEnumerator<float> enumer1 = t1.GetEnumerator())
            {
                using (IEnumerator<float> enumer2 = t2.GetEnumerator())
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
        /// Interpolación lineal.
        /// </summary>
        public static float Lerp(float d1, float d2, float alpha)
        {
            return (1 - alpha) * d1 + alpha * d2;
        }

        /// <summary>
        /// Interpolación lineal.
        /// </summary>
        public static float Lerp(float x, float x1, float y1, float x2, float y2)
        {
            if (EpsilonEquals(x1, x2) || EpsilonEquals(y1, y2))
            {
                return y1;
            }
            return y1 + (y2 - y1) * (x - x1) / (x2 - x1);
        }

        public static IEnumerable<double> For(float min, float max, int c)
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

        public static bool Between(this float v, float a, float b)
        {
            return v >= a && v <= b;
        }

        public static bool BetweenClosedOpen(this float v, float a, float b)
        {
            return v >= a && v < b;
        }

        public static int SafeSign(float v)
        {
            return ((v < 0) ? -1 : 1);
        }

        #region Inner clases

        /// <summary>
        /// Compara los valores indicados.
        /// </summary>
        public sealed class EpsilonEqualityComparer : IEqualityComparer<float>, IEqualityComparer
        {
            public EpsilonEqualityComparer(float epsilon = EPSILON)
            {
                this.epsilon = epsilon;
            }

            private readonly float epsilon;

            #region IComparer<REAL>

            public bool Equals(float x, float y)
            {
                return x.EpsilonEquals(y, this.epsilon);
            }

            public int GetHashCode(float x)
            {
                return x.GetHashCode();
            }

            #endregion

            #region IComparer

            bool IEqualityComparer.Equals(object x, object y)
            {
                Contract.Requires((x is float) && (y is float));
                return this.Equals((float)x, (float)y);
            }

            int IEqualityComparer.GetHashCode(object x)
            {
                Contract.Requires(x is float);
                return this.GetHashCode((float)x);
            }

            #endregion
        }

        /// <summary>
        /// Compara los valores indicados.
        /// </summary>
        public sealed class EpsilonComparer : IComparer<float>, IComparer
        {
            public EpsilonComparer(float epsilon = EPSILON)
            {
                this.epsilon = epsilon;
            }

            private readonly float epsilon;

            #region IComparer<REAL>

            public int Compare(float x, float y)
            {
                return x.EpsilonCompareTo(y, this.epsilon);
            }

            #endregion

            #region IComparer

            int IComparer.Compare(object x, object y)
            {
                Contract.Requires((x is float) && (y is float));
                return this.Compare((float)x, (float)y);
            }

            #endregion
        }

        /// <summary>
        /// Compara los valores indicados.
        /// </summary>
        public sealed class Comparer : IComparer<float>, IComparer
        {
            #region IComparer<REAL>

            public int Compare(float x, float y)
            {
                return System.Math.Sign(x - y);
            }

            #endregion

            #region IComparer

            int IComparer.Compare(object x, object y)
            {
                Contract.Requires((x is float) && (y is float));
                return this.Compare((float)x, (float)y);
            }

            #endregion
        }

        #endregion
    }
}