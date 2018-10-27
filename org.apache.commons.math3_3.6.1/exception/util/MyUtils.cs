using System;
using System.Collections.Generic;
using org.apache.commons.math3.exception;
using org.apache.commons.math3.util;

namespace org.apache.commons.math3.analysis.util
{
    using FastMath = System.Math;

    public static class MyUtils
    {
        #region Metodos Precision, FastMath y MathUtils

        /// <summary>
        /// Compute the signum of a number.
        /// The signum is -1 for negative numbers, +1 for positive numbers and 0 otherwise </summary>
        /// <param name="a"> number on which evaluation is done </param>
        /// <returns> -1.0, -0.0, +0.0, +1.0 or NaN depending on sign of a </returns>
        public static double Signum(double a)
        {
            return (a < 0.0) ? -1.0 : ((a > 0.0) ? 1.0 : a); // return +0.0/-0.0/NaN depending on a
        }

        /// <summary>
        /// Compute the signum of a number.
        /// The signum is -1 for negative numbers, +1 for positive numbers and 0 otherwise </summary>
        /// <param name="a"> number on which evaluation is done </param>
        /// <returns> -1.0, -0.0, +0.0, +1.0 or NaN depending on sign of a </returns>
        public static float Signum(float a)
        {
            return (a < 0.0f) ? -1.0f : ((a > 0.0f) ? 1.0f : a); // return +0.0/-0.0/NaN depending on a
        }

        /// <summary>
        /// Checks that an object is not null.
        /// </summary>
        /// <param name="o"> Object to be checked. </param>
        /// <exception cref="NullArgumentException"> if {@code o} is {@code null}. </exception>
        public static void CheckNotNull(object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException();
            }
        }

        internal static bool Equals(double p1, double p2, int p3 = 1)
        {
            return MathUtils.EpsilonEquals(p1, p2);
        }

        /// <summary>
        /// Returns true if both arguments are NaN or neither is NaN and they are
        /// equal as defined by <seealso cref="#equals(double,double) equals(x, y, 1)"/>.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <returns> {@code true} if the values are equal or both are NaN.
        /// @since 2.2 </returns>
        public static bool EqualsIncludingNaN(double x, double y)
        {
            return (double.IsNaN(x) && double.IsNaN(y)) || Equals(x, y, 1);
        }

        /// <summary>
        /// Returns true if both arguments are NaN or are equal or within the range
        /// of allowed error (inclusive).
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <param name="eps"> the amount of absolute error to allow. </param>
        /// <returns> {@code true} if the values are equal or within range of each other,
        /// or both are NaN.
        /// @since 2.2 </returns>
        public static bool EqualsIncludingNaN(double x, double y, double eps)
        {
            return EqualsIncludingNaN(x, y) || (FastMath.Abs(y - x) <= eps);
        }

        /// <summary>
        /// Returns a pseudo-random number between 0.0 and 1.0.
        /// <para><b>Note:</b> this implementation currently delegates to <seealso cref="Math#random"/>
        /// </para>
        /// </summary>
        /// <returns> a random number between 0.0 and 1.0 </returns>
        public static double Random()
        {
            return random.NextDouble();
        }

        private static readonly Random random = new Random();

        /// <summary>
        /// http://stackoverflow.com/questions/24104763/how-to-compute-ulpwhen-math-ulp-is-missing
        /// </summary>
        public static double ULP(double value)
        {
            // This is actually a constant in the same static class as this method, but 
            // we put it here for brevity of this example.
            const double MaxULP = 1.9958403095347198116563727130368E+292;

            if (Double.IsNaN(value))
            {
                return Double.NaN;
            }
            else if (Double.IsPositiveInfinity(value) || Double.IsNegativeInfinity(value))
            {
                return Double.PositiveInfinity;
            }
            else if (value == 0.0)
            {
                return Double.Epsilon; // Equivalent of Double.MIN_VALUE in Java; Double.MinValue in C# is the actual minimum value a double can hold.
            }
            else if (Math.Abs(value) == Double.MaxValue)
            {
                return MaxULP;
            }

            // All you need to understand about DoubleInfo is that it's a helper struct 
            // that provides more functionality than is used here, but in this situation, 
            // we only use the `Bits` property, which is just the double converted into a 
            // long.
            //DoubleInfo info = new DoubleInfo(value);
            //long bits = info.Bits;
            long bits = BitConverter.DoubleToInt64Bits(value);

            // This is safe because we already checked for value == Double.MaxValue.
            return Math.Abs(BitConverter.Int64BitsToDouble(bits + 1) - value);
        }

        public static double LongBitsToDouble(long value)
        {
            return BitConverter.Int64BitsToDouble(value);
        }

        public static long DoubleToRawLongBits(double value)
        {
            return BitConverter.DoubleToInt64Bits(value);
        }

        public static int FloatToRawIntBits(float f)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(f), 0);
        }

        public static IEnumerator<T> ToEnumerator<T>(IJIterator<T> it)
        {
            while (it.HasNext())
            {
                yield return it.Next();
            }
        }

#if false
        http://stackoverflow.com/questions/24104763/how-to-compute-ulpwhen-math-ulp-is-missing
        /// <summary>
        /// use a precalculated value for the ulp of Double.MAX_VALUE
        /// </summary>
        private const double MAX_ULP = 1.9958403095347198E292;

        /// <summary>
        /// Returns the size of an ulp (units in the last place) of the argument.
        /// @param d value whose ulp is to be returned
        /// @return size of an ulp for the argument
        /// </summary>
        public static double ulp(double d)
        {
            if (Double.IsNaN(d))
            {
                // If the argument is NaN, then the result is NaN.
                return Double.NaN;
            }

            if (Double.IsInfinity(d))
            {
                // If the argument is positive or negative infinity, then the
                // result is positive infinity.
                return Double.PositiveInfinity;
            }

            if (d == 0.0)
            {
                // If the argument is positive or negative zero, then the result is Double.MIN_VALUE.
                return Double.MinValue;
            }

            d = Math.Abs(d);
            if (d == Double.MaxValue)
            {
                // If the argument is Double.MAX_VALUE, then the result is equal to 2^971.
                return MAX_ULP;
            }

            return nextAfter(d, Double.MaxValue) - d;
        }

        public static double copySign(double x, double y)
        {
            return com.codename1.util.MathUtil.copysign(x, y);
        }

        private static bool isSameSign(double x, double y)
        {
            return copySign(x, y) == x;
        }

        /// <summary>
        /// Returns the next representable floating point number after the first
        /// argument in the direction of the second argument.
        ///
        /// @param start starting value
        /// @param direction value indicating which of the neighboring representable
        ///  floating point number to return
        /// @return The floating-point number next to {@code start} in the
        /// direction of {@direction}.
        /// </summary>
        public static double nextAfter(double start, double direction)
        {
            if (Double.IsNaN(start) || Double.IsNaN(direction))
            {
                // If either argument is a NaN, then NaN is returned.
                return Double.NaN;
            }

            if (start == direction)
            {
                // If both arguments compare as equal the second argument is returned.
                return direction;
            }

            double absStart = Math.Abs(start);
            double absDir = Math.Abs(direction);
            bool toZero = !isSameSign(start, direction) || absDir < absStart;

            if (toZero)
            {
                // we are reducing the magnitude, going toward zero.
                if (absStart == Double.MinValue)
                {
                    return copySign(0.0, start);
                }
                if (Double.IsInfinity(absStart))
                {
                    return copySign(Double.MaxValue, start);
                }
                return copySign(BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(absStart) - 1L), start);
            }
            else
            {
                // we are increasing the magnitude, toward +-Infinity
                if (start == 0.0)
                {
                    return copySign(Double.MinValue, direction);
                }
                if (absStart == Double.MaxValue)
                {
                    return copySign(Double.PositiveInfinity, start);
                }
                return copySign(BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(absStart) + 1L), start);
            }
        }
#endif

        #endregion Metodos Precision, FastMath y MathUtils
    }
}