/// Apache Commons Math 3.6.1
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
    using NotFiniteNumberException = org.apache.commons.math3.exception.NotFiniteNumberException;
    using NullArgumentException = org.apache.commons.math3.exception.NullArgumentException;
    using Localizable = org.apache.commons.math3.exception.util.Localizable;
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;

    /// <summary>
    /// Miscellaneous utility functions.
    /// </summary>
    /// <seealso cref= ArithmeticUtils </seealso>
    /// <seealso cref= Precision </seealso>
    /// <seealso cref= MathArrays
    ///  </seealso>
    public sealed class MathUtils
    {
        /// <summary>
        /// \(2\pi\)
        /// @since 2.1
        /// </summary>
        public static readonly double TWO_PI = 2 * FastMath.PI;

        /// <summary>
        /// \(\pi^2\)
        /// @since 3.4
        /// </summary>
        public static readonly double PI_SQUARED = FastMath.PI * FastMath.PI;


        /// <summary>
        /// Class contains only static methods.
        /// </summary>
        private MathUtils()
        {
        }


        /// <summary>
        /// Returns an integer hash code representing the given double value.
        /// </summary>
        /// <param name="value"> the value to be hashed </param>
        /// <returns> the hash code </returns>
        public static int Hash(double value)
        {
            return (new double?(value)).GetHashCode();
        }

        /// <summary>
        /// Returns {@code true} if the values are equal according to semantics of
        /// <seealso cref="Double#equals(Object)"/>.
        /// </summary>
        /// <param name="x"> Value </param>
        /// <param name="y"> Value </param>
        /// <returns> {@code new Double(x).equals(new Double(y))} </returns>
        public static bool Equals(double x, double y)
        {
            return (new double?(x)).Equals(new double?(y));
        }

        /// <summary>
        /// Returns an integer hash code representing the given double array.
        /// </summary>
        /// <param name="value"> the value to be hashed (may be null) </param>
        /// <returns> the hash code
        /// @since 1.2 </returns>
        public static int Hash(double[] value)
        {
            return Arrays.GetHashCode(value);
        }

        /// <summary>
        /// Normalize an angle in a 2&pi; wide interval around a center value.
        /// <para>This method has three main uses:</para>
        /// <ul>
        ///   <li>normalize an angle between 0 and 2&pi;:<br/>
        ///       {@code a = MathUtils.normalizeAngle(a, FastMath.PI);}</li>
        ///   <li>normalize an angle between -&pi; and +&pi;<br/>
        ///       {@code a = MathUtils.normalizeAngle(a, 0.0);}</li>
        ///   <li>compute the angle between two defining angular positions:<br>
        ///       {@code angle = MathUtils.normalizeAngle(end, start) - start;}</li>
        /// </ul>
        /// <para>Note that due to numerical accuracy and since &pi; cannot be represented
        /// exactly, the result interval is <em>closed</em>, it cannot be half-closed
        /// as would be more satisfactory in a purely mathematical view.</para> </summary>
        /// <param name="a"> angle to normalize </param>
        /// <param name="center"> center of the desired 2&pi; interval for the result </param>
        /// <returns> a-2k&pi; with integer k and center-&pi; &lt;= a-2k&pi; &lt;= center+&pi;
        /// @since 1.2 </returns>
         public static double NormalizeAngle(double a, double center)
         {
             return a - TWO_PI * FastMath.Floor((a + FastMath.PI - center) / TWO_PI);
         }

         /// <summary>
         /// Find the maximum of two field elements. </summary>
         /// @param <T> the type of the field elements </param>
         /// <param name="e1"> first element </param>
         /// <param name="e2"> second element </param>
         /// <returns> max(a1, e2)
         /// @since 3.6 </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static <T extends org.apache.commons.math3.RealFieldElement<T>> T max(final T e1, final T e2)
         public static T max<T>(T e1, T e2) where T : org.apache.commons.math3.RealFieldElement<T>
         {
             return e1.subtract(e2).getReal() >= 0 ? e1 : e2;
         }

         /// <summary>
         /// Find the minimum of two field elements. </summary>
         /// @param <T> the type of the field elements </param>
         /// <param name="e1"> first element </param>
         /// <param name="e2"> second element </param>
         /// <returns> min(a1, e2)
         /// @since 3.6 </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static <T extends org.apache.commons.math3.RealFieldElement<T>> T min(final T e1, final T e2)
         public static T min<T>(T e1, T e2) where T : org.apache.commons.math3.RealFieldElement<T>
         {
             return e1.subtract(e2).getReal() >= 0 ? e2 : e1;
         }

        /// <summary>
        /// <para>Reduce {@code |a - offset|} to the primary interval
        /// {@code [0, |period|)}.</para>
        /// 
        /// <para>Specifically, the value returned is <br/>
        /// {@code a - |period| * floor((a - offset) / |period|) - offset}.</para>
        /// 
        /// <para>If any of the parameters are {@code NaN} or infinite, the result is
        /// {@code NaN}.</para>
        /// </summary>
        /// <param name="a"> Value to reduce. </param>
        /// <param name="period"> Period. </param>
        /// <param name="offset"> Value that will be mapped to {@code 0}. </param>
        /// <returns> the value, within the interval {@code [0 |period|)},
        /// that corresponds to {@code a}. </returns>
        public static double Reduce(double a, double period, double offset)
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double p = FastMath.abs(period);
            double p = FastMath.Abs(period);
            return a - p * FastMath.Floor((a - offset) / p) - offset;
        }

        /// <summary>
        /// Returns the first argument with the sign of the second argument.
        /// </summary>
        /// <param name="magnitude"> Magnitude of the returned value. </param>
        /// <param name="sign"> Sign of the returned value. </param>
        /// <returns> a value with magnitude equal to {@code magnitude} and with the
        /// same sign as the {@code sign} argument. </returns>
        /// <exception cref="MathArithmeticException"> if {@code magnitude == Byte.MIN_VALUE}
        /// and {@code sign >= 0}. </exception>
        public static sbyte CopySign(sbyte magnitude, sbyte sign)
        {
            if ((magnitude >= 0 && sign >= 0) || (magnitude < 0 && sign < 0))
            { // Sign is OK.
                return magnitude;
            }
            else if (sign >= 0 && magnitude == sbyte.MinValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW);
            }
            else
            {
                return (sbyte) -magnitude; // Flip sign.
            }
        }

        /// <summary>
        /// Returns the first argument with the sign of the second argument.
        /// </summary>
        /// <param name="magnitude"> Magnitude of the returned value. </param>
        /// <param name="sign"> Sign of the returned value. </param>
        /// <returns> a value with magnitude equal to {@code magnitude} and with the
        /// same sign as the {@code sign} argument. </returns>
        /// <exception cref="MathArithmeticException"> if {@code magnitude == Short.MIN_VALUE}
        /// and {@code sign >= 0}. </exception>
        public static short CopySign(short magnitude, short sign)
        {
            if ((magnitude >= 0 && sign >= 0) || (magnitude < 0 && sign < 0))
            { // Sign is OK.
                return magnitude;
            }
            else if (sign >= 0 && magnitude == short.MinValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW);
            }
            else
            {
                return (short) -magnitude; // Flip sign.
            }
        }

        /// <summary>
        /// Returns the first argument with the sign of the second argument.
        /// </summary>
        /// <param name="magnitude"> Magnitude of the returned value. </param>
        /// <param name="sign"> Sign of the returned value. </param>
        /// <returns> a value with magnitude equal to {@code magnitude} and with the
        /// same sign as the {@code sign} argument. </returns>
        /// <exception cref="MathArithmeticException"> if {@code magnitude == Integer.MIN_VALUE}
        /// and {@code sign >= 0}. </exception>
        public static int CopySign(int magnitude, int sign)
        {
            if ((magnitude >= 0 && sign >= 0) || (magnitude < 0 && sign < 0))
            { // Sign is OK.
                return magnitude;
            }
            else if (sign >= 0 && magnitude == int.MinValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW);
            }
            else
            {
                return -magnitude; // Flip sign.
            }
        }

        /// <summary>
        /// Returns the first argument with the sign of the second argument.
        /// </summary>
        /// <param name="magnitude"> Magnitude of the returned value. </param>
        /// <param name="sign"> Sign of the returned value. </param>
        /// <returns> a value with magnitude equal to {@code magnitude} and with the
        /// same sign as the {@code sign} argument. </returns>
        /// <exception cref="MathArithmeticException"> if {@code magnitude == Long.MIN_VALUE}
        /// and {@code sign >= 0}. </exception>
        public static long CopySign(long magnitude, long sign)
        {
            if ((magnitude >= 0 && sign >= 0) || (magnitude < 0 && sign < 0))
            { // Sign is OK.
                return magnitude;
            }
            else if (sign >= 0 && magnitude == long.MinValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW);
            }
            else
            {
                return -magnitude; // Flip sign.
            }
        }
        /// <summary>
        /// Check that the argument is a real number.
        /// </summary>
        /// <param name="x"> Argument. </param>
        /// <exception cref="NotFiniteNumberException"> if {@code x} is not a
        /// finite real number. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static void checkFinite(final double x) throws org.apache.commons.math3.exception.NotFiniteNumberException
        public static void CheckFinite(double x)
        {
            if (double.IsInfinity(x) || double.IsNaN(x))
            {
                throw new NotFiniteNumberException(x);
            }
        }

        /// <summary>
        /// Check that all the elements are real numbers.
        /// </summary>
        /// <param name="val"> Arguments. </param>
        /// <exception cref="NotFiniteNumberException"> if any values of the array is not a
        /// finite real number. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static void checkFinite(final double[] val) throws org.apache.commons.math3.exception.NotFiniteNumberException
        public static void CheckFinite(double[] val)
        {
            for (int i = 0; i < val.Length; i++)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = val[i];
                double x = val[i];
                if (double.IsInfinity(x) || double.IsNaN(x))
                {
                    throw new NotFiniteNumberException(LocalizedFormats.ARRAY_ELEMENT, x, i);
                }
            }
        }

        /// <summary>
        /// Checks that an object is not null.
        /// </summary>
        /// <param name="o"> Object to be checked. </param>
        /// <param name="pattern"> Message pattern. </param>
        /// <param name="args"> Arguments to replace the placeholders in {@code pattern}. </param>
        /// <exception cref="NullArgumentException"> if {@code o} is {@code null}. </exception>
        public static void CheckNotNull(object o, Localizable pattern, params object[] args)
        {
            if (o == null)
            {
                throw new NullArgumentException(pattern, args);
            }
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
                throw new NullArgumentException();
            }
        }
    }

}