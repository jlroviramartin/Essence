/// Apache Commons Math 3.6.1
using System;

/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace org.apache.commons.math3.util
{

    using MathArithmeticException = org.apache.commons.math3.exception.MathArithmeticException;
    using MathIllegalArgumentException = org.apache.commons.math3.exception.MathIllegalArgumentException;
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;

    /// <summary>
    /// Utilities for comparing numbers.
    /// 
    /// @since 3.0
    /// </summary>
    public class Precision
    {
        /// <summary>
        /// <para>
        /// Largest double-precision floating-point number such that
        /// {@code 1 + EPSILON} is numerically equal to 1. This value is an upper
        /// bound on the relative error due to rounding real numbers to double
        /// precision floating-point numbers.
        /// </para>
        /// <para>
        /// In IEEE 754 arithmetic, this is 2<sup>-53</sup>.
        /// </para>
        /// </summary>
        /// <seealso cref= <a href="http://en.wikipedia.org/wiki/Machine_epsilon">Machine epsilon</a> </seealso>
        public static readonly double EPSILON;

        /// <summary>
        /// Safe minimum, such that {@code 1 / SAFE_MIN} does not overflow.
        /// <br/>
        /// In IEEE 754 arithmetic, this is also the smallest normalized
        /// number 2<sup>-1022</sup>.
        /// </summary>
        public static readonly double SAFE_MIN;

        /// <summary>
        /// Exponent offset in IEEE754 representation. </summary>
        private const long EXPONENT_OFFSET = 1023l;

        /// <summary>
        /// Offset to order signed double numbers lexicographically. </summary>
        private const long SGN_MASK = unchecked((long)0x8000000000000000L);
        /// <summary>
        /// Offset to order signed double numbers lexicographically. </summary>
        private const int SGN_MASK_FLOAT = unchecked((int)0x80000000);
        /// <summary>
        /// Positive zero. </summary>
        private const double POSITIVE_ZERO = 0d;
        /// <summary>
        /// Positive zero bits. </summary>
        private static readonly long POSITIVE_ZERO_DOUBLE_BITS = BitConverter.DoubleToInt64Bits(+0.0);
        /// <summary>
        /// Negative zero bits. </summary>
        private static readonly long NEGATIVE_ZERO_DOUBLE_BITS = BitConverter.DoubleToInt64Bits(-0.0);
        /// <summary>
        /// Positive zero bits. </summary>
        private static readonly int POSITIVE_ZERO_FLOAT_BITS = Float.floatToRawIntBits(+0.0f);
        /// <summary>
        /// Negative zero bits. </summary>
        private static readonly int NEGATIVE_ZERO_FLOAT_BITS = Float.floatToRawIntBits(-0.0f);

        static Precision()
        {
            /*
             *  This was previously expressed as = 0x1.0p-53;
             *  However, OpenJDK (Sparc Solaris) cannot handle such small
             *  constants: MATH-721
             */
            EPSILON = BitConverter.Int64BitsToDouble((EXPONENT_OFFSET - 53l) << 52);

            /*
             * This was previously expressed as = 0x1.0p-1022;
             * However, OpenJDK (Sparc Solaris) cannot handle such small
             * constants: MATH-721
             */
            SAFE_MIN = BitConverter.Int64BitsToDouble((EXPONENT_OFFSET - 1022l) << 52);
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        private Precision()
        {
        }

        /// <summary>
        /// Compares two numbers given some amount of allowed error.
        /// </summary>
        /// <param name="x"> the first number </param>
        /// <param name="y"> the second number </param>
        /// <param name="eps"> the amount of error to allow when checking for equality </param>
        /// <returns> <ul><li>0 if  <seealso cref="#equals(double, double, double) equals(x, y, eps)"/></li>
        ///       <li>&lt; 0 if !<seealso cref="#equals(double, double, double) equals(x, y, eps)"/> &amp;&amp; x &lt; y</li>
        ///       <li>> 0 if !<seealso cref="#equals(double, double, double) equals(x, y, eps)"/> &amp;&amp; x > y or
        ///       either argument is NaN</li></ul> </returns>
        public static int CompareTo(double x, double y, double eps)
        {
            if (Equals(x, y, eps))
            {
                return 0;
            }
            else if (x < y)
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// Compares two numbers given some amount of allowed error.
        /// Two float numbers are considered equal if there are {@code (maxUlps - 1)}
        /// (or fewer) floating point numbers between them, i.e. two adjacent floating
        /// point numbers are considered equal.
        /// Adapted from <a
        /// href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/">
        /// Bruce Dawson</a>. Returns {@code false} if either of the arguments is NaN.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <param name="maxUlps"> {@code (maxUlps - 1)} is the number of floating point
        /// values between {@code x} and {@code y}. </param>
        /// <returns> <ul><li>0 if  <seealso cref="#equals(double, double, int) equals(x, y, maxUlps)"/></li>
        ///       <li>&lt; 0 if !<seealso cref="#equals(double, double, int) equals(x, y, maxUlps)"/> &amp;&amp; x &lt; y</li>
        ///       <li>&gt; 0 if !<seealso cref="#equals(double, double, int) equals(x, y, maxUlps)"/> &amp;&amp; x > y
        ///       or either argument is NaN</li></ul> </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int compareTo(final double x, final double y, final int maxUlps)
        public static int CompareTo(double x, double y, int maxUlps)
        {
            if (Equals(x, y, maxUlps))
            {
                return 0;
            }
            else if (x < y)
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// Returns true iff they are equal as defined by
        /// <seealso cref="#equals(float,float,int) equals(x, y, 1)"/>.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <returns> {@code true} if the values are equal. </returns>
        public static bool Equals(float x, float y)
        {
            return Equals(x, y, 1);
        }

        /// <summary>
        /// Returns true if both arguments are NaN or they are
        /// equal as defined by <seealso cref="#equals(float,float) equals(x, y, 1)"/>.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <returns> {@code true} if the values are equal or both are NaN.
        /// @since 2.2 </returns>
        public static bool EqualsIncludingNaN(float x, float y)
        {
            return (x != x || y != y) ?!(x != x ^ y != y) : Equals(x, y, 1);
        }

        /// <summary>
        /// Returns true if the arguments are equal or within the range of allowed
        /// error (inclusive).  Returns {@code false} if either of the arguments
        /// is NaN.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <param name="eps"> the amount of absolute error to allow. </param>
        /// <returns> {@code true} if the values are equal or within range of each other.
        /// @since 2.2 </returns>
        public static bool Equals(float x, float y, float eps)
        {
            return Equals(x, y, 1) || FastMath.Abs(y - x) <= eps;
        }

        /// <summary>
        /// Returns true if the arguments are both NaN, are equal, or are within the range
        /// of allowed error (inclusive).
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <param name="eps"> the amount of absolute error to allow. </param>
        /// <returns> {@code true} if the values are equal or within range of each other,
        /// or both are NaN.
        /// @since 2.2 </returns>
        public static bool EqualsIncludingNaN(float x, float y, float eps)
        {
            return EqualsIncludingNaN(x, y) || (FastMath.Abs(y - x) <= eps);
        }

        /// <summary>
        /// Returns true if the arguments are equal or within the range of allowed
        /// error (inclusive).
        /// Two float numbers are considered equal if there are {@code (maxUlps - 1)}
        /// (or fewer) floating point numbers between them, i.e. two adjacent floating
        /// point numbers are considered equal.
        /// Adapted from <a
        /// href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/">
        /// Bruce Dawson</a>.  Returns {@code false} if either of the arguments is NaN.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <param name="maxUlps"> {@code (maxUlps - 1)} is the number of floating point
        /// values between {@code x} and {@code y}. </param>
        /// <returns> {@code true} if there are fewer than {@code maxUlps} floating
        /// point values between {@code x} and {@code y}.
        /// @since 2.2 </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static boolean equals(final float x, final float y, final int maxUlps)
        public static bool Equals(float x, float y, int maxUlps)
        {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int xInt = Float.floatToRawIntBits(x);
            int xInt = Float.floatToRawIntBits(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int yInt = Float.floatToRawIntBits(y);
            int yInt = Float.floatToRawIntBits(y);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final boolean isEqual;
            bool isEqual;
            if (((xInt ^ yInt) & SGN_MASK_FLOAT) == 0)
            {
                // number have same sign, there is no risk of overflow
                isEqual = FastMath.Abs(xInt - yInt) <= maxUlps;
            }
            else
            {
                // number have opposite signs, take care of overflow
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int deltaPlus;
                int deltaPlus;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int deltaMinus;
                int deltaMinus;
                if (xInt < yInt)
                {
                    deltaPlus = yInt - POSITIVE_ZERO_FLOAT_BITS;
                    deltaMinus = xInt - NEGATIVE_ZERO_FLOAT_BITS;
                }
                else
                {
                    deltaPlus = xInt - POSITIVE_ZERO_FLOAT_BITS;
                    deltaMinus = yInt - NEGATIVE_ZERO_FLOAT_BITS;
                }

                if (deltaPlus > maxUlps)
                {
                    isEqual = false;
                }
                else
                {
                    isEqual = deltaMinus <= (maxUlps - deltaPlus);
                }

            }

            return isEqual && !float.IsNaN(x) && !float.IsNaN(y);

        }

        /// <summary>
        /// Returns true if the arguments are both NaN or if they are equal as defined
        /// by <seealso cref="#equals(float,float,int) equals(x, y, maxUlps)"/>.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <param name="maxUlps"> {@code (maxUlps - 1)} is the number of floating point
        /// values between {@code x} and {@code y}. </param>
        /// <returns> {@code true} if both arguments are NaN or if there are less than
        /// {@code maxUlps} floating point values between {@code x} and {@code y}.
        /// @since 2.2 </returns>
        public static bool EqualsIncludingNaN(float x, float y, int maxUlps)
        {
            return (x != x || y != y) ?!(x != x ^ y != y) : Equals(x, y, maxUlps);
        }

        /// <summary>
        /// Returns true iff they are equal as defined by
        /// <seealso cref="#equals(double,double,int) equals(x, y, 1)"/>.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <returns> {@code true} if the values are equal. </returns>
        public static bool Equals(double x, double y)
        {
            return Equals(x, y, 1);
        }

        /// <summary>
        /// Returns true if the arguments are both NaN or they are
        /// equal as defined by <seealso cref="#equals(double,double) equals(x, y, 1)"/>.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <returns> {@code true} if the values are equal or both are NaN.
        /// @since 2.2 </returns>
        public static bool EqualsIncludingNaN(double x, double y)
        {
            return (x != x || y != y) ?!(x != x ^ y != y) : Equals(x, y, 1);
        }

        /// <summary>
        /// Returns {@code true} if there is no double value strictly between the
        /// arguments or the difference between them is within the range of allowed
        /// error (inclusive). Returns {@code false} if either of the arguments
        /// is NaN.
        /// </summary>
        /// <param name="x"> First value. </param>
        /// <param name="y"> Second value. </param>
        /// <param name="eps"> Amount of allowed absolute error. </param>
        /// <returns> {@code true} if the values are two adjacent floating point
        /// numbers or they are within range of each other. </returns>
        public static bool Equals(double x, double y, double eps)
        {
            return Equals(x, y, 1) || FastMath.Abs(y - x) <= eps;
        }

        /// <summary>
        /// Returns {@code true} if there is no double value strictly between the
        /// arguments or the relative difference between them is less than or equal
        /// to the given tolerance. Returns {@code false} if either of the arguments
        /// is NaN.
        /// </summary>
        /// <param name="x"> First value. </param>
        /// <param name="y"> Second value. </param>
        /// <param name="eps"> Amount of allowed relative error. </param>
        /// <returns> {@code true} if the values are two adjacent floating point
        /// numbers or they are within range of each other.
        /// @since 3.1 </returns>
        public static bool EqualsWithRelativeTolerance(double x, double y, double eps)
        {
            if (Equals(x, y, 1))
            {
                return true;
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absoluteMax = FastMath.max(FastMath.abs(x), FastMath.abs(y));
            double absoluteMax = FastMath.Max(FastMath.Abs(x), FastMath.Abs(y));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double relativeDifference = FastMath.abs((x - y) / absoluteMax);
            double relativeDifference = FastMath.Abs((x - y) / absoluteMax);

            return relativeDifference <= eps;
        }

        /// <summary>
        /// Returns true if the arguments are both NaN, are equal or are within the range
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
        /// Returns true if the arguments are equal or within the range of allowed
        /// error (inclusive).
        /// <para>
        /// Two float numbers are considered equal if there are {@code (maxUlps - 1)}
        /// (or fewer) floating point numbers between them, i.e. two adjacent
        /// floating point numbers are considered equal.
        /// </para>
        /// <para>
        /// Adapted from <a
        /// href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/">
        /// Bruce Dawson</a>. Returns {@code false} if either of the arguments is NaN.
        /// </para>
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <param name="maxUlps"> {@code (maxUlps - 1)} is the number of floating point
        /// values between {@code x} and {@code y}. </param>
        /// <returns> {@code true} if there are fewer than {@code maxUlps} floating
        /// point values between {@code x} and {@code y}. </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static boolean equals(final double x, final double y, final int maxUlps)
        public static bool Equals(double x, double y, int maxUlps)
        {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long xInt = Double.doubleToRawLongBits(x);
            long xInt = Double.doubleToRawLongBits(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long yInt = Double.doubleToRawLongBits(y);
            long yInt = Double.doubleToRawLongBits(y);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final boolean isEqual;
            bool isEqual;
            if (((xInt ^ yInt) & SGN_MASK) == 0l)
            {
                // number have same sign, there is no risk of overflow
                isEqual = FastMath.Abs(xInt - yInt) <= maxUlps;
            }
            else
            {
                // number have opposite signs, take care of overflow
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long deltaPlus;
                long deltaPlus;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long deltaMinus;
                long deltaMinus;
                if (xInt < yInt)
                {
                    deltaPlus = yInt - POSITIVE_ZERO_DOUBLE_BITS;
                    deltaMinus = xInt - NEGATIVE_ZERO_DOUBLE_BITS;
                }
                else
                {
                    deltaPlus = xInt - POSITIVE_ZERO_DOUBLE_BITS;
                    deltaMinus = yInt - NEGATIVE_ZERO_DOUBLE_BITS;
                }

                if (deltaPlus > maxUlps)
                {
                    isEqual = false;
                }
                else
                {
                    isEqual = deltaMinus <= (maxUlps - deltaPlus);
                }

            }

            return isEqual && !double.IsNaN(x) && !double.IsNaN(y);

        }

        /// <summary>
        /// Returns true if both arguments are NaN or if they are equal as defined
        /// by <seealso cref="#equals(double,double,int) equals(x, y, maxUlps)"/>.
        /// </summary>
        /// <param name="x"> first value </param>
        /// <param name="y"> second value </param>
        /// <param name="maxUlps"> {@code (maxUlps - 1)} is the number of floating point
        /// values between {@code x} and {@code y}. </param>
        /// <returns> {@code true} if both arguments are NaN or if there are less than
        /// {@code maxUlps} floating point values between {@code x} and {@code y}.
        /// @since 2.2 </returns>
        public static bool EqualsIncludingNaN(double x, double y, int maxUlps)
        {
            return (x != x || y != y) ?!(x != x ^ y != y) : Equals(x, y, maxUlps);
        }

        /// <summary>
        /// Rounds the given value to the specified number of decimal places.
        /// The value is rounded using the <seealso cref="BigDecimal#ROUND_HALF_UP"/> method.
        /// </summary>
        /// <param name="x"> Value to round. </param>
        /// <param name="scale"> Number of digits to the right of the decimal point. </param>
        /// <returns> the rounded value.
        /// @since 1.1 (previously in {@code MathUtils}, moved as of version 3.0) </returns>
        public static double Round(double x, int scale)
        {
            return round(x, scale, decimal.ROUND_HALF_UP);
        }

        /// <summary>
        /// Rounds the given value to the specified number of decimal places.
        /// The value is rounded using the given method which is any method defined
        /// in <seealso cref="BigDecimal"/>.
        /// If {@code x} is infinite or {@code NaN}, then the value of {@code x} is
        /// returned unchanged, regardless of the other parameters.
        /// </summary>
        /// <param name="x"> Value to round. </param>
        /// <param name="scale"> Number of digits to the right of the decimal point. </param>
        /// <param name="roundingMethod"> Rounding method as defined in <seealso cref="BigDecimal"/>. </param>
        /// <returns> the rounded value. </returns>
        /// <exception cref="ArithmeticException"> if {@code roundingMethod == ROUND_UNNECESSARY}
        /// and the specified scaling operation would require rounding. </exception>
        /// <exception cref="IllegalArgumentException"> if {@code roundingMethod} does not
        /// represent a valid rounding mode.
        /// @since 1.1 (previously in {@code MathUtils}, moved as of version 3.0) </exception>
        public static double Round(double x, int scale, int roundingMethod)
        {
            try
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double rounded = (new java.math.BigDecimal(Convert.ToString(x))
                double rounded = (new decimal(Convert.ToString(x))
                       .setScale(scale, roundingMethod)).doubleValue();
                // MATH-1089: negative values rounded to zero should result in negative zero
                return rounded == POSITIVE_ZERO ? POSITIVE_ZERO * x : rounded;
            }
            catch (System.FormatException)
            {
                if (double.IsInfinity(x))
                {
                    return x;
                }
                else
                {
                    return Double.NaN;
                }
            }
        }

        /// <summary>
        /// Rounds the given value to the specified number of decimal places.
        /// The value is rounded using the <seealso cref="BigDecimal#ROUND_HALF_UP"/> method.
        /// </summary>
        /// <param name="x"> Value to round. </param>
        /// <param name="scale"> Number of digits to the right of the decimal point. </param>
        /// <returns> the rounded value.
        /// @since 1.1 (previously in {@code MathUtils}, moved as of version 3.0) </returns>
        public static float Round(float x, int scale)
        {
            return round(x, scale, decimal.ROUND_HALF_UP);
        }

        /// <summary>
        /// Rounds the given value to the specified number of decimal places.
        /// The value is rounded using the given method which is any method defined
        /// in <seealso cref="BigDecimal"/>.
        /// </summary>
        /// <param name="x"> Value to round. </param>
        /// <param name="scale"> Number of digits to the right of the decimal point. </param>
        /// <param name="roundingMethod"> Rounding method as defined in <seealso cref="BigDecimal"/>. </param>
        /// <returns> the rounded value.
        /// @since 1.1 (previously in {@code MathUtils}, moved as of version 3.0) </returns>
        /// <exception cref="MathArithmeticException"> if an exact operation is required but result is not exact </exception>
        /// <exception cref="MathIllegalArgumentException"> if {@code roundingMethod} is not a valid rounding method. </exception>
        public static float Round(float x, int scale, int roundingMethod)
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final float sign = FastMath.copySign(1f, x);
            float sign = FastMath.CopySign(1f, x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final float factor = (float) FastMath.pow(10.0f, scale) * sign;
            float factor = (float) FastMath.pow(10.0f, scale) * sign;
            return (float) RoundUnscaled(x * factor, sign, roundingMethod) / factor;
        }

        /// <summary>
        /// Rounds the given non-negative value to the "nearest" integer. Nearest is
        /// determined by the rounding method specified. Rounding methods are defined
        /// in <seealso cref="BigDecimal"/>.
        /// </summary>
        /// <param name="unscaled"> Value to round. </param>
        /// <param name="sign"> Sign of the original, scaled value. </param>
        /// <param name="roundingMethod"> Rounding method, as defined in <seealso cref="BigDecimal"/>. </param>
        /// <returns> the rounded value. </returns>
        /// <exception cref="MathArithmeticException"> if an exact operation is required but result is not exact </exception>
        /// <exception cref="MathIllegalArgumentException"> if {@code roundingMethod} is not a valid rounding method.
        /// @since 1.1 (previously in {@code MathUtils}, moved as of version 3.0) </exception>
        private static double RoundUnscaled(double unscaled, double sign, int roundingMethod)
        {
            switch (roundingMethod)
            {
            case decimal.ROUND_CEILING :
                if (sign == -1)
                {
                    unscaled = FastMath.Floor(FastMath.nextAfter(unscaled, double.NegativeInfinity));
                }
                else
                {
                    unscaled = FastMath.Ceil(FastMath.nextAfter(unscaled, double.PositiveInfinity));
                }
                break;
            case decimal.ROUND_DOWN :
                unscaled = FastMath.Floor(FastMath.nextAfter(unscaled, double.NegativeInfinity));
                break;
            case decimal.ROUND_FLOOR :
                if (sign == -1)
                {
                    unscaled = FastMath.Ceil(FastMath.nextAfter(unscaled, double.PositiveInfinity));
                }
                else
                {
                    unscaled = FastMath.Floor(FastMath.nextAfter(unscaled, double.NegativeInfinity));
                }
                break;
            case decimal.ROUND_HALF_DOWN :
            {
                unscaled = FastMath.nextAfter(unscaled, double.NegativeInfinity);
                double fraction = unscaled - FastMath.Floor(unscaled);
                if (fraction > 0.5)
                {
                    unscaled = FastMath.Ceil(unscaled);
                }
                else
                {
                    unscaled = FastMath.Floor(unscaled);
                }
                break;
            }
            case decimal.ROUND_HALF_EVEN :
            {
                double fraction = unscaled - FastMath.Floor(unscaled);
                if (fraction > 0.5)
                {
                    unscaled = FastMath.Ceil(unscaled);
                }
                else if (fraction < 0.5)
                {
                    unscaled = FastMath.Floor(unscaled);
                }
                else
                {
                    // The following equality test is intentional and needed for rounding purposes
                    if (FastMath.Floor(unscaled) / 2.0 == FastMath.Floor(FastMath.Floor(unscaled) / 2.0))
                    { // even
                        unscaled = FastMath.Floor(unscaled);
                    }
                    else
                    { // odd
                        unscaled = FastMath.Ceil(unscaled);
                    }
                }
                break;
            }
            case decimal.ROUND_HALF_UP :
            {
                unscaled = FastMath.nextAfter(unscaled, double.PositiveInfinity);
                double fraction = unscaled - FastMath.Floor(unscaled);
                if (fraction >= 0.5)
                {
                    unscaled = FastMath.Ceil(unscaled);
                }
                else
                {
                    unscaled = FastMath.Floor(unscaled);
                }
                break;
            }
            case decimal.ROUND_UNNECESSARY :
                if (unscaled != FastMath.Floor(unscaled))
                {
                    throw new MathArithmeticException();
                }
                break;
            case decimal.ROUND_UP :
                // do not round if the discarded fraction is equal to zero
                if (unscaled != FastMath.Floor(unscaled))
                {
                    unscaled = FastMath.Ceil(FastMath.nextAfter(unscaled, double.PositiveInfinity));
                }
                break;
            default :
                throw new MathIllegalArgumentException(LocalizedFormats.INVALID_ROUNDING_METHOD, roundingMethod, "ROUND_CEILING", decimal.ROUND_CEILING, "ROUND_DOWN", decimal.ROUND_DOWN, "ROUND_FLOOR", decimal.ROUND_FLOOR, "ROUND_HALF_DOWN", decimal.ROUND_HALF_DOWN, "ROUND_HALF_EVEN", decimal.ROUND_HALF_EVEN, "ROUND_HALF_UP", decimal.ROUND_HALF_UP, "ROUND_UNNECESSARY", decimal.ROUND_UNNECESSARY, "ROUND_UP", decimal.ROUND_UP);
            }
            return unscaled;
        }


        /// <summary>
        /// Computes a number {@code delta} close to {@code originalDelta} with
        /// the property that <pre><code>
        ///   x + delta - x
        /// </code></pre>
        /// is exactly machine-representable.
        /// This is useful when computing numerical derivatives, in order to reduce
        /// roundoff errors.
        /// </summary>
        /// <param name="x"> Value. </param>
        /// <param name="originalDelta"> Offset value. </param>
        /// <returns> a number {@code delta} so that {@code x + delta} and {@code x}
        /// differ by a representable floating number. </returns>
        public static double RepresentableDelta(double x, double originalDelta)
        {
            return x + originalDelta - x;
        }
    }

}