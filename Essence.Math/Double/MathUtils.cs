using System;
using System.Collections.Generic;

namespace Essence.Math.Double
{
    public static partial class MathUtils
    {
        public static IEnumerable<double> For(double i, double f, int c)
        {
            yield return i;

            double tt = (f - i) / (c + 1);

            double t = i;
            while (c > 0)
            {
                t += tt;
                yield return t;
                c--;
            }

            yield return f;
        }

        public static bool EpsilonEquals(this double a, double b, double epsilon = 0.001)
        {
            //return (SysMath.Abs(b - a) <= epsilon);
            // Resuelve las comparaciones con PositiveInfinity / NegativeInfinity.
            if (b > a)
            {
                return b <= a + epsilon;
            }
            else
            {
                return a <= b + epsilon;
            }
        }

        public static bool Between(this double v, double a, double b)
        {
            return v >= a && v <= b;
        }

        public static bool BetweenClosedOpen(this double v, double a, double b)
        {
            return v >= a && v < b;
        }

        public static double Clamp(this double v, double a, double b)
        {
            return (v <= a) ? a : ((v >= b) ? b : v);
        }

        /*public static double EpsilonClamp(this double v, double a, double b, double epsilon = 0.001)
        {
            return (v <= a) ? a : ((v >= b) ? b : v);
        }*/

        public static int SafeSign(double v)
        {
            return ((v < 0) ? -1 : 1);
        }

        public static int ToInt16<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToInt16(null);
        }

        public static int ToInt32<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToInt32(null);
        }

        public static long ToInt64<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToInt64(null);
        }

        public static float ToSingle<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToSingle(null);
        }

        public static double ToDouble<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToDouble(null);
        }
    }
}