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
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;

    /// <summary>
    /// Faster, more accurate, portable alternative to <seealso cref="Math"/> and
    /// <seealso cref="StrictMath"/> for large scale computation.
    /// <para>
    /// FastMath is a drop-in replacement for both Math and StrictMath. This
    /// means that for any method in Math (say {@code Math.sin(x)} or
    /// {@code Math.cbrt(y)}), user can directly change the class and use the
    /// methods as is (using {@code FastMath.sin(x)} or {@code FastMath.cbrt(y)}
    /// in the previous example).
    /// </para>
    /// <para>
    /// FastMath speed is achieved by relying heavily on optimizing compilers
    /// to native code present in many JVMs today and use of large tables.
    /// The larger tables are lazily initialised on first use, so that the setup
    /// time does not penalise methods that don't need them.
    /// </para>
    /// <para>
    /// Note that FastMath is
    /// extensively used inside Apache Commons Math, so by calling some algorithms,
    /// the overhead when the the tables need to be intialised will occur
    /// regardless of the end-user calling FastMath methods directly or not.
    /// Performance figures for a specific JVM and hardware can be evaluated by
    /// running the FastMathTestPerformance tests in the test directory of the source
    /// distribution.
    /// </para>
    /// <para>
    /// FastMath accuracy should be mostly independent of the JVM as it relies only
    /// on IEEE-754 basic operations and on embedded tables. Almost all operations
    /// are accurate to about 0.5 ulp throughout the domain range. This statement,
    /// of course is only a rough global observed behavior, it is <em>not</em> a
    /// guarantee for <em>every</em> double numbers input (see William Kahan's <a
    /// href="http://en.wikipedia.org/wiki/Rounding#The_table-maker.27s_dilemma">Table
    /// Maker's Dilemma</a>).
    /// </para>
    /// <para>
    /// FastMath additionally implements the following methods not found in Math/StrictMath:
    /// <ul>
    /// <li><seealso cref="#asinh(double)"/></li>
    /// <li><seealso cref="#acosh(double)"/></li>
    /// <li><seealso cref="#atanh(double)"/></li>
    /// </ul>
    /// The following methods are found in Math/StrictMath since 1.6 only, they are provided
    /// by FastMath even in 1.5 Java virtual machines
    /// <ul>
    /// <li><seealso cref="#copySign(double, double)"/></li>
    /// <li><seealso cref="#getExponent(double)"/></li>
    /// <li><seealso cref="#nextAfter(double,double)"/></li>
    /// <li><seealso cref="#nextUp(double)"/></li>
    /// <li><seealso cref="#scalb(double, int)"/></li>
    /// <li><seealso cref="#copySign(float, float)"/></li>
    /// <li><seealso cref="#getExponent(float)"/></li>
    /// <li><seealso cref="#nextAfter(float,double)"/></li>
    /// <li><seealso cref="#nextUp(float)"/></li>
    /// <li><seealso cref="#scalb(float, int)"/></li>
    /// </ul>
    /// </para>
    /// @since 2.2
    /// </summary>
    public class FastMath
    {
        /// <summary>
        /// Archimede's constant PI, ratio of circle circumference to diameter. </summary>
        public static readonly double PI = 105414357.0 / 33554432.0 + 1.984187159361080883e-9;

        /// <summary>
        /// Napier's constant e, base of the natural logarithm. </summary>
        public static readonly double E = 2850325.0 / 1048576.0 + 8.254840070411028747e-8;

        /// <summary>
        /// Index of exp(0) in the array of integer exponentials. </summary>
        internal const int EXP_INT_TABLE_MAX_INDEX = 750;
        /// <summary>
        /// Length of the array of integer exponentials. </summary>
        internal static readonly int EXP_INT_TABLE_LEN = EXP_INT_TABLE_MAX_INDEX * 2;
        /// <summary>
        /// Logarithm table length. </summary>
        internal const int LN_MANT_LEN = 1024;
        /// <summary>
        /// Exponential fractions table length. </summary>
        internal const int EXP_FRAC_TABLE_LEN = 1025; // 0, 1/1024, ... 1024/1024

        /// <summary>
        /// StrictMath.log(Double.MAX_VALUE): {@value} </summary>
        private static readonly double LOG_MAX_VALUE = Math.Log(double.MaxValue);

        /// <summary>
        /// Indicator for tables initialization.
        /// <para>
        /// This compile-time constant should be set to true only if one explicitly
        /// wants to compute the tables at class loading time instead of using the
        /// already computed ones provided as literal arrays below.
        /// </para>
        /// </summary>
        private const bool RECOMPUTE_TABLES_AT_RUNTIME = false;

        /// <summary>
        /// log(2) (high bits). </summary>
        private const double LN_2_A = 0.693147063255310059;

        /// <summary>
        /// log(2) (low bits). </summary>
        private const double LN_2_B = 1.17304635250823482e-7;

        /// <summary>
        /// Coefficients for log, when input 0.99 < x < 1.01. </summary>
        private static readonly double[][] LN_QUICK_COEF = new double[][]
        {
            new double[] { 1.0, 5.669184079525E-24 },
            new double[] { -0.25, -0.25 },
            new double[] { 0.3333333134651184, 1.986821492305628E-8 },
            new double[] { -0.25, -6.663542893624021E-14 },
            new double[] { 0.19999998807907104, 1.1921056801463227E-8 },
            new double[] { -0.1666666567325592, -7.800414592973399E-9 },
            new double[] { 0.1428571343421936, 5.650007086920087E-9 },
            new double[] { -0.12502530217170715, -7.44321345601866E-11 },
            new double[] { 0.11113807559013367, 9.219544613762692E-9 }
        };

        /// <summary>
        /// Coefficients for log in the range of 1.0 < x < 1.0 + 2^-10. </summary>
        private static readonly double[][] LN_HI_PREC_COEF = new double[][]
        {
            new double[] { 1.0, -6.032174644509064E-23 },
            new double[] { -0.25, -0.25 },
            new double[] { 0.3333333134651184, 1.9868161777724352E-8 },
            new double[] { -0.2499999701976776, -2.957007209750105E-8 },
            new double[] { 0.19999954104423523, 1.5830993332061267E-10 },
            new double[] { -0.16624879837036133, -2.6033824355191673E-8 }
        };

        /// <summary>
        /// Sine, Cosine, Tangent tables are for 0, 1/8, 2/8, ... 13/8 = PI/2 approx. </summary>
        private const int SINE_TABLE_LEN = 14;

        /// <summary>
        /// Sine table (high bits). </summary>
        private static readonly double[] SINE_TABLE_A = new double[] { +0.0d, +0.1246747374534607d, +0.24740394949913025d, +0.366272509098053d, +0.4794255495071411d, +0.5850973129272461d, +0.6816387176513672d, +0.7675435543060303d, +0.8414709568023682d, +0.902267575263977d, +0.9489846229553223d, +0.9808930158615112d, +0.9974949359893799d, +0.9985313415527344d };

        /// <summary>
        /// Sine table (low bits). </summary>
        private static readonly double[] SINE_TABLE_B = new double[] { +0.0d, -4.068233003401932E-9d, +9.755392680573412E-9d, +1.9987994582857286E-8d, -1.0902938113007961E-8d, -3.9986783938944604E-8d, +4.23719669792332E-8d, -5.207000323380292E-8d, +2.800552834259E-8d, +1.883511811213715E-8d, -3.5997360512765566E-9d, +4.116164446561962E-8d, +5.0614674548127384E-8d, -1.0129027912496858E-9d };

        /// <summary>
        /// Cosine table (high bits). </summary>
        private static readonly double[] COSINE_TABLE_A = new double[] { +1.0d, +0.9921976327896118d, +0.9689123630523682d, +0.9305076599121094d, +0.8775825500488281d, +0.8109631538391113d, +0.7316888570785522d, +0.6409968137741089d, +0.5403022766113281d, +0.4311765432357788d, +0.3153223395347595d, +0.19454771280288696d, +0.07073719799518585d, -0.05417713522911072d };

        /// <summary>
        /// Cosine table (low bits). </summary>
        private static readonly double[] COSINE_TABLE_B = new double[] { +0.0d, +3.4439717236742845E-8d, +5.865827662008209E-8d, -3.7999795083850525E-8d, +1.184154459111628E-8d, -3.43338934259355E-8d, +1.1795268640216787E-8d, +4.438921624363781E-8d, +2.925681159240093E-8d, -2.6437112632041807E-8d, +2.2860509143963117E-8d, -4.813899778443457E-9d, +3.6725170580355583E-9d, +2.0217439756338078E-10d };


        /// <summary>
        /// Tangent table, used by atan() (high bits). </summary>
        private static readonly double[] TANGENT_TABLE_A = new double[] { +0.0d, +0.1256551444530487d, +0.25534194707870483d, +0.3936265707015991d, +0.5463024377822876d, +0.7214844226837158d, +0.9315965175628662d, +1.1974215507507324d, +1.5574076175689697d, +2.092571258544922d, +3.0095696449279785d, +5.041914939880371d, +14.101419448852539d, -18.430862426757812d };

        /// <summary>
        /// Tangent table, used by atan() (low bits). </summary>
        private static readonly double[] TANGENT_TABLE_B = new double[] { +0.0d, -7.877917738262007E-9d, -2.5857668567479893E-8d, +5.2240336371356666E-9d, +5.206150291559893E-8d, +1.8307188599677033E-8d, -5.7618793749770706E-8d, +7.848361555046424E-8d, +1.0708593250394448E-7d, +1.7827257129423813E-8d, +2.893485277253286E-8d, +3.1660099222737955E-7d, +4.983191803254889E-7d, -3.356118100840571E-7d };

        /// <summary>
        /// Bits of 1/(2*pi), need for reducePayneHanek(). </summary>
        private static readonly long[] RECIP_2PI = new long[] { (0x28be60dbL << 32) | 0x9391054aL, (0x7f09d5f4L << 32) | 0x7d4d3770L, (0x36d8a566L << 32) | 0x4f10e410L, (0x7f9458eaL << 32) | 0xf7aef158L, (0x6dc91b8eL << 32) | 0x909374b8L, (0x01924bbaL << 32) | 0x82746487L, (0x3f877ac7L << 32) | 0x2c4a69cfL, (0xba208d7dL << 32) | 0x4baed121L, (0x3a671c09L << 32) | 0xad17df90L, (0x4e64758eL << 32) | 0x60d4ce7dL, (0x272117e2L << 32) | 0xef7e4a0eL, (0xc7fe25ffL << 32) | 0xf7816603L, (0xfbcbc462L << 32) | 0xd6829b47L, (0xdb4d9fb3L << 32) | 0xc9f2c26dL, (0xd3d18fd9L << 32) | 0xa797fa8bL, (0x5d49eeb1L << 32) | 0xfaf97c5eL, (0xcf41ce7dL << 32) | 0xe294a4baL, 0x9afed7ecL << 32 };

        /// <summary>
        /// Bits of pi/4, need for reducePayneHanek(). </summary>
        private static readonly long[] PI_O_4_BITS = new long[] { (0xc90fdaa2L << 32) | 0x2168c234L, (0xc4c6628bL << 32) | 0x80dc1cd1L };

        /// <summary>
        /// Eighths.
        /// This is used by sinQ, because its faster to do a table lookup than
        /// a multiply in this time-critical routine
        /// </summary>
        private static readonly double[] EIGHTHS = new double[] { 0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1.0, 1.125, 1.25, 1.375, 1.5, 1.625 };

        /// <summary>
        /// Table of 2^((n+2)/3) </summary>
        private static readonly double[] CBRTTWO = new double[] { 0.6299605249474366, 0.7937005259840998, 1.0, 1.2599210498948732, 1.5874010519681994 };

        /*
         *  There are 52 bits in the mantissa of a double.
         *  For additional precision, the code splits double numbers into two parts,
         *  by clearing the low order 30 bits if possible, and then performs the arithmetic
         *  on each half separately.
         */

        /// <summary>
        /// 0x40000000 - used to split a double into two parts, both with the low order bits cleared.
        /// Equivalent to 2^30.
        /// </summary>
        private const long HEX_40000000 = 0x40000000L; // 1073741824L

        /// <summary>
        /// Mask used to clear low order 30 bits </summary>
        private static readonly long MASK_30BITS = -1L - (HEX_40000000 - 1); // 0xFFFFFFFFC0000000L;

        /// <summary>
        /// Mask used to clear the non-sign part of an int. </summary>
        private const int MASK_NON_SIGN_INT = 0x7fffffff;

        /// <summary>
        /// Mask used to clear the non-sign part of a long. </summary>
        private const long MASK_NON_SIGN_LONG = 0x7fffffffffffffffl;

        /// <summary>
        /// Mask used to extract exponent from double bits. </summary>
        private const long MASK_DOUBLE_EXPONENT = 0x7ff0000000000000L;

        /// <summary>
        /// Mask used to extract mantissa from double bits. </summary>
        private const long MASK_DOUBLE_MANTISSA = 0x000fffffffffffffL;

        /// <summary>
        /// Mask used to add implicit high order bit for normalized double. </summary>
        private const long IMPLICIT_HIGH_BIT = 0x0010000000000000L;

        /// <summary>
        /// 2^52 - double numbers this large must be integral (no fraction) or NaN or Infinite </summary>
        private const double TWO_POWER_52 = 4503599627370496.0;

        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_3 = 1d / 3d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_5 = 1d / 5d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_7 = 1d / 7d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_9 = 1d / 9d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_11 = 1d / 11d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_13 = 1d / 13d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_15 = 1d / 15d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_17 = 1d / 17d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_3_4 = 3d / 4d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_15_16 = 15d / 16d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_13_14 = 13d / 14d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_11_12 = 11d / 12d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_9_10 = 9d / 10d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_7_8 = 7d / 8d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_5_6 = 5d / 6d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_2 = 1d / 2d;
        /// <summary>
        /// Constant: {@value}. </summary>
        private static readonly double F_1_4 = 1d / 4d;

        /// <summary>
        /// Private Constructor
        /// </summary>
        private FastMath()
        {
        }

        // Generic helper methods

        /// <summary>
        /// Get the high order bits from the mantissa.
        /// Equivalent to adding and subtracting HEX_40000 but also works for very large numbers
        /// </summary>
        /// <param name="d"> the value to split </param>
        /// <returns> the high order part of the mantissa </returns>
        private static double DoubleHighPart(double d)
        {
            if (d > -Precision.SAFE_MIN && d < Precision.SAFE_MIN)
            {
                return d; // These are un-normalised - don't try to convert
            }
            long xl = BitConverter.DoubleToInt64Bits(d); // can take raw bits because just gonna convert it back
            xl &= MASK_30BITS; // Drop low order bits
            return BitConverter.DoubleToInt64Bits(xl);
        }

        /// <summary>
        /// Compute the square root of a number.
        /// <para><b>Note:</b> this implementation currently delegates to <seealso cref="Math#sqrt"/>
        /// </para>
        /// </summary>
        /// <param name="a"> number on which evaluation is done </param>
        /// <returns> square root of a </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double sqrt(final double a)
        public static double Sqrt(double a)
        {
            return Math.Sqrt(a);
        }

        /// <summary>
        /// Compute the hyperbolic cosine of a number. </summary>
        /// <param name="x"> number on which evaluation is done </param>
        /// <returns> hyperbolic cosine of x </returns>
        public static double Cosh(double x)
        {
          if (x != x)
          {
              return x;
          }

          // cosh[z] = (exp(z) + exp(-z))/2

          // for numbers with magnitude 20 or so,
          // exp(-z) can be ignored in comparison with exp(z)

          if (x > 20)
          {
              if (x >= LOG_MAX_VALUE)
              {
                  // Avoid overflow (MATH-905).
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double t = exp(0.5 * x);
                  double t = Exp(0.5 * x);
                  return (0.5 * t) * t;
              }
              else
              {
                  return 0.5 * Exp(x);
              }
          }
          else if (x < -20)
          {
              if (x <= -LOG_MAX_VALUE)
              {
                  // Avoid overflow (MATH-905).
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double t = exp(-0.5 * x);
                  double t = Exp(-0.5 * x);
                  return (0.5 * t) * t;
              }
              else
              {
                  return 0.5 * Exp(-x);
              }
          }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double hiPrec[] = new double[2];
          double[] hiPrec = new double[2];
          if (x < 0.0)
          {
              x = -x;
          }
          Exp(x, 0.0, hiPrec);

          double ya = hiPrec[0] + hiPrec[1];
          double yb = -(ya - hiPrec[0] - hiPrec[1]);

          double temp = ya * HEX_40000000;
          double yaa = ya + temp - temp;
          double yab = ya - yaa;

          // recip = 1/y
          double recip = 1.0 / ya;
          temp = recip * HEX_40000000;
          double recipa = recip + temp - temp;
          double recipb = recip - recipa;

          // Correct for rounding in division
          recipb += (1.0 - yaa * recipa - yaa * recipb - yab * recipa - yab * recipb) * recip;
          // Account for yb
          recipb += -yb * recip * recip;

          // y = y + 1/y
          temp = ya + recipa;
          yb += -(temp - ya - recipa);
          ya = temp;
          temp = ya + recipb;
          yb += -(temp - ya - recipb);
          ya = temp;

          double result = ya + yb;
          result *= 0.5;
          return result;
        }

        /// <summary>
        /// Compute the hyperbolic sine of a number. </summary>
        /// <param name="x"> number on which evaluation is done </param>
        /// <returns> hyperbolic sine of x </returns>
        public static double Sinh(double x)
        {
          bool negate = false;
          if (x != x)
          {
              return x;
          }

          // sinh[z] = (exp(z) - exp(-z) / 2

          // for values of z larger than about 20,
          // exp(-z) can be ignored in comparison with exp(z)

          if (x > 20)
          {
              if (x >= LOG_MAX_VALUE)
              {
                  // Avoid overflow (MATH-905).
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double t = exp(0.5 * x);
                  double t = Exp(0.5 * x);
                  return (0.5 * t) * t;
              }
              else
              {
                  return 0.5 * Exp(x);
              }
          }
          else if (x < -20)
          {
              if (x <= -LOG_MAX_VALUE)
              {
                  // Avoid overflow (MATH-905).
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double t = exp(-0.5 * x);
                  double t = Exp(-0.5 * x);
                  return (-0.5 * t) * t;
              }
              else
              {
                  return -0.5 * Exp(-x);
              }
          }

          if (x == 0)
          {
              return x;
          }

          if (x < 0.0)
          {
              x = -x;
              negate = true;
          }

          double result;

          if (x > 0.25)
          {
              double[] hiPrec = new double[2];
              Exp(x, 0.0, hiPrec);

              double ya = hiPrec[0] + hiPrec[1];
              double yb = -(ya - hiPrec[0] - hiPrec[1]);

              double temp = ya * HEX_40000000;
              double yaa = ya + temp - temp;
              double yab = ya - yaa;

              // recip = 1/y
              double recip = 1.0 / ya;
              temp = recip * HEX_40000000;
              double recipa = recip + temp - temp;
              double recipb = recip - recipa;

              // Correct for rounding in division
              recipb += (1.0 - yaa * recipa - yaa * recipb - yab * recipa - yab * recipb) * recip;
              // Account for yb
              recipb += -yb * recip * recip;

              recipa = -recipa;
              recipb = -recipb;

              // y = y + 1/y
              temp = ya + recipa;
              yb += -(temp - ya - recipa);
              ya = temp;
              temp = ya + recipb;
              yb += -(temp - ya - recipb);
              ya = temp;

              result = ya + yb;
              result *= 0.5;
          }
          else
          {
              double[] hiPrec = new double[2];
              Expm1(x, hiPrec);

              double ya = hiPrec[0] + hiPrec[1];
              double yb = -(ya - hiPrec[0] - hiPrec[1]);

              /* Compute expm1(-x) = -expm1(x) / (expm1(x) + 1) */
              double denom = 1.0 + ya;
              double denomr = 1.0 / denom;
              double denomb = -(denom - 1.0 - ya) + yb;
              double ratio = ya * denomr;
              double temp = ratio * HEX_40000000;
              double ra = ratio + temp - temp;
              double rb = ratio - ra;

              temp = denom * HEX_40000000;
              double za = denom + temp - temp;
              double zb = denom - za;

              rb += (ya - za * ra - za * rb - zb * ra - zb * rb) * denomr;

              // Adjust for yb
              rb += yb * denomr; // numerator
              rb += -ya * denomb * denomr * denomr; // denominator

              // y = y - 1/y
              temp = ya + ra;
              yb += -(temp - ya - ra);
              ya = temp;
              temp = ya + rb;
              yb += -(temp - ya - rb);
              ya = temp;

              result = ya + yb;
              result *= 0.5;
          }

          if (negate)
          {
              result = -result;
          }

          return result;
        }

        /// <summary>
        /// Compute the hyperbolic tangent of a number. </summary>
        /// <param name="x"> number on which evaluation is done </param>
        /// <returns> hyperbolic tangent of x </returns>
        public static double Tanh(double x)
        {
          bool negate = false;

          if (x != x)
          {
              return x;
          }

          // tanh[z] = sinh[z] / cosh[z]
          // = (exp(z) - exp(-z)) / (exp(z) + exp(-z))
          // = (exp(2x) - 1) / (exp(2x) + 1)

          // for magnitude > 20, sinh[z] == cosh[z] in double precision

          if (x > 20.0)
          {
              return 1.0;
          }

          if (x < -20)
          {
              return -1.0;
          }

          if (x == 0)
          {
              return x;
          }

          if (x < 0.0)
          {
              x = -x;
              negate = true;
          }

          double result;
          if (x >= 0.5)
          {
              double[] hiPrec = new double[2];
              // tanh(x) = (exp(2x) - 1) / (exp(2x) + 1)
              Exp(x * 2.0, 0.0, hiPrec);

              double ya = hiPrec[0] + hiPrec[1];
              double yb = -(ya - hiPrec[0] - hiPrec[1]);

              /* Numerator */
              double na = -1.0 + ya;
              double nb = -(na + 1.0 - ya);
              double temp = na + yb;
              nb += -(temp - na - yb);
              na = temp;

              /* Denominator */
              double da = 1.0 + ya;
              double db = -(da - 1.0 - ya);
              temp = da + yb;
              db += -(temp - da - yb);
              da = temp;

              temp = da * HEX_40000000;
              double daa = da + temp - temp;
              double dab = da - daa;

              // ratio = na/da
              double ratio = na / da;
              temp = ratio * HEX_40000000;
              double ratioa = ratio + temp - temp;
              double ratiob = ratio - ratioa;

              // Correct for rounding in division
              ratiob += (na - daa * ratioa - daa * ratiob - dab * ratioa - dab * ratiob) / da;

              // Account for nb
              ratiob += nb / da;
              // Account for db
              ratiob += -db * na / da / da;

              result = ratioa + ratiob;
          }
          else
          {
              double[] hiPrec = new double[2];
              // tanh(x) = expm1(2x) / (expm1(2x) + 2)
              Expm1(x * 2.0, hiPrec);

              double ya = hiPrec[0] + hiPrec[1];
              double yb = -(ya - hiPrec[0] - hiPrec[1]);

              /* Numerator */
              double na = ya;
              double nb = yb;

              /* Denominator */
              double da = 2.0 + ya;
              double db = -(da - 2.0 - ya);
              double temp = da + yb;
              db += -(temp - da - yb);
              da = temp;

              temp = da * HEX_40000000;
              double daa = da + temp - temp;
              double dab = da - daa;

              // ratio = na/da
              double ratio = na / da;
              temp = ratio * HEX_40000000;
              double ratioa = ratio + temp - temp;
              double ratiob = ratio - ratioa;

              // Correct for rounding in division
              ratiob += (na - daa * ratioa - daa * ratiob - dab * ratioa - dab * ratiob) / da;

              // Account for nb
              ratiob += nb / da;
              // Account for db
              ratiob += -db * na / da / da;

              result = ratioa + ratiob;
          }

          if (negate)
          {
              result = -result;
          }

          return result;
        }

        /// <summary>
        /// Compute the inverse hyperbolic cosine of a number. </summary>
        /// <param name="a"> number on which evaluation is done </param>
        /// <returns> inverse hyperbolic cosine of a </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double acosh(final double a)
        public static double Acosh(double a)
        {
            return FastMath.Log(a + FastMath.Sqrt(a * a - 1));
        }

        /// <summary>
        /// Compute the inverse hyperbolic sine of a number. </summary>
        /// <param name="a"> number on which evaluation is done </param>
        /// <returns> inverse hyperbolic sine of a </returns>
        public static double Asinh(double a)
        {
            bool negative = false;
            if (a < 0)
            {
                negative = true;
                a = -a;
            }

            double absAsinh;
            if (a > 0.167)
            {
                absAsinh = FastMath.Log(FastMath.Sqrt(a * a + 1) + a);
            }
            else
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a2 = a * a;
                double a2 = a * a;
                if (a > 0.097)
                {
                    absAsinh = a * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * (F_1_9 - a2 * (F_1_11 - a2 * (F_1_13 - a2 * (F_1_15 - a2 * F_1_17 * F_15_16) * F_13_14) * F_11_12) * F_9_10) * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                }
                else if (a > 0.036)
                {
                    absAsinh = a * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * (F_1_9 - a2 * (F_1_11 - a2 * F_1_13 * F_11_12) * F_9_10) * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                }
                else if (a > 0.0036)
                {
                    absAsinh = a * (1 - a2 * (F_1_3 - a2 * (F_1_5 - a2 * (F_1_7 - a2 * F_1_9 * F_7_8) * F_5_6) * F_3_4) * F_1_2);
                }
                else
                {
                    absAsinh = a * (1 - a2 * (F_1_3 - a2 * F_1_5 * F_3_4) * F_1_2);
                }
            }

            return negative ? -absAsinh : absAsinh;
        }

        /// <summary>
        /// Compute the inverse hyperbolic tangent of a number. </summary>
        /// <param name="a"> number on which evaluation is done </param>
        /// <returns> inverse hyperbolic tangent of a </returns>
        public static double Atanh(double a)
        {
            bool negative = false;
            if (a < 0)
            {
                negative = true;
                a = -a;
            }

            double absAtanh;
            if (a > 0.15)
            {
                absAtanh = 0.5 * FastMath.Log((1 + a) / (1 - a));
            }
            else
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a2 = a * a;
                double a2 = a * a;
                if (a > 0.087)
                {
                    absAtanh = a * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * (F_1_9 + a2 * (F_1_11 + a2 * (F_1_13 + a2 * (F_1_15 + a2 * F_1_17))))))));
                }
                else if (a > 0.031)
                {
                    absAtanh = a * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * (F_1_9 + a2 * (F_1_11 + a2 * F_1_13))))));
                }
                else if (a > 0.003)
                {
                    absAtanh = a * (1 + a2 * (F_1_3 + a2 * (F_1_5 + a2 * (F_1_7 + a2 * F_1_9))));
                }
                else
                {
                    absAtanh = a * (1 + a2 * (F_1_3 + a2 * F_1_5));
                }
            }

            return negative ? -absAtanh : absAtanh;
        }

        /// <summary>
        /// Compute the signum of a number.
        /// The signum is -1 for negative numbers, +1 for positive numbers and 0 otherwise </summary>
        /// <param name="a"> number on which evaluation is done </param>
        /// <returns> -1.0, -0.0, +0.0, +1.0 or NaN depending on sign of a </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double signum(final double a)
        public static double Signum(double a)
        {
            return (a < 0.0) ? -1.0 : ((a > 0.0) ? 1.0 : a); // return +0.0/-0.0/NaN depending on a
        }

        /// <summary>
        /// Compute the signum of a number.
        /// The signum is -1 for negative numbers, +1 for positive numbers and 0 otherwise </summary>
        /// <param name="a"> number on which evaluation is done </param>
        /// <returns> -1.0, -0.0, +0.0, +1.0 or NaN depending on sign of a </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static float signum(final float a)
        public static float Signum(float a)
        {
            return (a < 0.0f) ? -1.0f : ((a > 0.0f) ? 1.0f : a); // return +0.0/-0.0/NaN depending on a
        }

        /// <summary>
        /// Compute next number towards positive infinity. </summary>
        /// <param name="a"> number to which neighbor should be computed </param>
        /// <returns> neighbor of a towards positive infinity </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double nextUp(final double a)
        public static double NextUp(double a)
        {
            return nextAfter(a, double.PositiveInfinity);
        }

        /// <summary>
        /// Compute next number towards positive infinity. </summary>
        /// <param name="a"> number to which neighbor should be computed </param>
        /// <returns> neighbor of a towards positive infinity </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static float nextUp(final float a)
        public static float NextUp(float a)
        {
            return nextAfter(a, float.PositiveInfinity);
        }

        /// <summary>
        /// Compute next number towards negative infinity. </summary>
        /// <param name="a"> number to which neighbor should be computed </param>
        /// <returns> neighbor of a towards negative infinity
        /// @since 3.4 </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double nextDown(final double a)
        public static double NextDown(double a)
        {
            return nextAfter(a, double.NegativeInfinity);
        }

        /// <summary>
        /// Compute next number towards negative infinity. </summary>
        /// <param name="a"> number to which neighbor should be computed </param>
        /// <returns> neighbor of a towards negative infinity
        /// @since 3.4 </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static float nextDown(final float a)
        public static float NextDown(float a)
        {
            return nextAfter(a, float.NegativeInfinity);
        }

        /// <summary>
        /// Returns a pseudo-random number between 0.0 and 1.0.
        /// <para><b>Note:</b> this implementation currently delegates to <seealso cref="Math#random"/>
        /// </para>
        /// </summary>
        /// <returns> a random number between 0.0 and 1.0 </returns>
        public static double Random()
        {
            return GlobalRandom.NextDouble;
        }

        /// <summary>
        /// Exponential function.
        /// 
        /// Computes exp(x), function result is nearly rounded.   It will be correctly
        /// rounded to the theoretical value for 99.9% of input values, otherwise it will
        /// have a 1 ULP error.
        /// 
        /// Method:
        ///    Lookup intVal = exp(int(x))
        ///    Lookup fracVal = exp(int(x-int(x) / 1024.0) * 1024.0 );
        ///    Compute z as the exponential of the remaining bits by a polynomial minus one
        ///    exp(x) = intVal * fracVal * (1 + z)
        /// 
        /// Accuracy:
        ///    Calculation is done with 63 bits of precision, so result should be correctly
        ///    rounded for 99.9% of input values, with less than 1 ULP error otherwise.
        /// </summary>
        /// <param name="x">   a double </param>
        /// <returns> double e<sup>x</sup> </returns>
        public static double Exp(double x)
        {
            return Exp(x, 0.0, null);
        }

        /// <summary>
        /// Internal helper method for exponential function. </summary>
        /// <param name="x"> original argument of the exponential function </param>
        /// <param name="extra"> extra bits of precision on input (To Be Confirmed) </param>
        /// <param name="hiPrec"> extra bits of precision on output (To Be Confirmed) </param>
        /// <returns> exp(x) </returns>
        private static double Exp(double x, double extra, double[] hiPrec)
        {
            double intPartA;
            double intPartB;
            int intVal = (int) x;

            /* Lookup exp(floor(x)).
             * intPartA will have the upper 22 bits, intPartB will have the lower
             * 52 bits.
             */
            if (x < 0.0)
            {

                // We don't check against intVal here as conversion of large negative double values
                // may be affected by a JIT bug. Subsequent comparisons can safely use intVal
                if (x < -746d)
                {
                    if (hiPrec != null)
                    {
                        hiPrec[0] = 0.0;
                        hiPrec[1] = 0.0;
                    }
                    return 0.0;
                }

                if (intVal < -709)
                {
                    /* This will produce a subnormal output */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double result = exp(x+40.19140625, extra, hiPrec) / 285040095144011776.0;
                    double result = Exp(x + 40.19140625, extra, hiPrec) / 285040095144011776.0;
                    if (hiPrec != null)
                    {
                        hiPrec[0] /= 285040095144011776.0;
                        hiPrec[1] /= 285040095144011776.0;
                    }
                    return result;
                }

                if (intVal == -709)
                {
                    /* exp(1.494140625) is nearly a machine number... */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double result = exp(x+1.494140625, extra, hiPrec) / 4.455505956692756620;
                    double result = Exp(x + 1.494140625, extra, hiPrec) / 4.455505956692756620;
                    if (hiPrec != null)
                    {
                        hiPrec[0] /= 4.455505956692756620;
                        hiPrec[1] /= 4.455505956692756620;
                    }
                    return result;
                }

                intVal--;

            }
            else
            {
                if (intVal > 709)
                {
                    if (hiPrec != null)
                    {
                        hiPrec[0] = double.PositiveInfinity;
                        hiPrec[1] = 0.0;
                    }
                    return double.PositiveInfinity;
                }

            }

            intPartA = ExpIntTable.EXP_INT_TABLE_A[EXP_INT_TABLE_MAX_INDEX + intVal];
            intPartB = ExpIntTable.EXP_INT_TABLE_B[EXP_INT_TABLE_MAX_INDEX + intVal];

            /* Get the fractional part of x, find the greatest multiple of 2^-10 less than
             * x and look up the exp function of it.
             * fracPartA will have the upper 22 bits, fracPartB the lower 52 bits.
             */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int intFrac = (int)((x - intVal) * 1024.0);
            int intFrac = (int)((x - intVal) * 1024.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fracPartA = ExpFracTable.EXP_FRAC_TABLE_A[intFrac];
            double fracPartA = ExpFracTable.EXP_FRAC_TABLE_A[intFrac];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fracPartB = ExpFracTable.EXP_FRAC_TABLE_B[intFrac];
            double fracPartB = ExpFracTable.EXP_FRAC_TABLE_B[intFrac];

            /* epsilon is the difference in x from the nearest multiple of 2^-10.  It
             * has a value in the range 0 <= epsilon < 2^-10.
             * Do the subtraction from x as the last step to avoid possible loss of precision.
             */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double epsilon = x - (intVal + intFrac / 1024.0);
            double epsilon = x - (intVal + intFrac / 1024.0);

            /* Compute z = exp(epsilon) - 1.0 via a minimax polynomial.  z has
           full double precision (52 bits).  Since z < 2^-10, we will have
           62 bits of precision when combined with the constant 1.  This will be
           used in the last addition below to get proper rounding. */

            /* Remez generated polynomial.  Converges on the interval [0, 2^-10], error
           is less than 0.5 ULP */
            double z = 0.04168701738764507;
            z = z * epsilon + 0.1666666505023083;
            z = z * epsilon + 0.5000000000042687;
            z = z * epsilon + 1.0;
            z = z * epsilon + -3.940510424527919E-20;

            /* Compute (intPartA+intPartB) * (fracPartA+fracPartB) by binomial
           expansion.
           tempA is exact since intPartA and intPartB only have 22 bits each.
           tempB will have 52 bits of precision.
             */
            double tempA = intPartA * fracPartA;
            double tempB = intPartA * fracPartB + intPartB * fracPartA + intPartB * fracPartB;

            /* Compute the result.  (1+z)(tempA+tempB).  Order of operations is
           important.  For accuracy add by increasing size.  tempA is exact and
           much larger than the others.  If there are extra bits specified from the
           pow() function, use them. */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tempC = tempB + tempA;
            double tempC = tempB + tempA;

            // If tempC is positive infinite, the evaluation below could result in NaN,
            // because z could be negative at the same time.
            if (tempC == double.PositiveInfinity)
            {
                return double.PositiveInfinity;
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double result;
            double result;
            if (extra != 0.0)
            {
                result = tempC * extra * z + tempC * extra + tempC * z + tempB + tempA;
            }
            else
            {
                result = tempC * z + tempB + tempA;
            }

            if (hiPrec != null)
            {
                // If requesting high precision
                hiPrec[0] = tempA;
                hiPrec[1] = tempC * extra * z + tempC * extra + tempC * z + tempB;
            }

            return result;
        }

        /// <summary>
        /// Compute exp(x) - 1 </summary>
        /// <param name="x"> number to compute shifted exponential </param>
        /// <returns> exp(x) - 1 </returns>
        public static double Expm1(double x)
        {
          return Expm1(x, null);
        }

        /// <summary>
        /// Internal helper method for expm1 </summary>
        /// <param name="x"> number to compute shifted exponential </param>
        /// <param name="hiPrecOut"> receive high precision result for -1.0 < x < 1.0 </param>
        /// <returns> exp(x) - 1 </returns>
        private static double Expm1(double x, double[] hiPrecOut)
        {
            if (x != x || x == 0.0)
            { // NaN or zero
                return x;
            }

            if (x <= -1.0 || x >= 1.0)
            {
                // If not between +/- 1.0
                //return exp(x) - 1.0;
                double[] hiPrec = new double[2];
                Exp(x, 0.0, hiPrec);
                if (x > 0.0)
                {
                    return -1.0 + hiPrec[0] + hiPrec[1];
                }
                else
                {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ra = -1.0 + hiPrec[0];
                    double ra = -1.0 + hiPrec[0];
                    double rb = -(ra + 1.0 - hiPrec[0]);
                    rb += hiPrec[1];
                    return ra + rb;
                }
            }

            double baseA;
            double baseB;
            double epsilon;
            bool negative = false;

            if (x < 0.0)
            {
                x = -x;
                negative = true;
            }

            {
                int intFrac = (int)(x * 1024.0);
                double tempA = ExpFracTable.EXP_FRAC_TABLE_A[intFrac] - 1.0;
                double tempB = ExpFracTable.EXP_FRAC_TABLE_B[intFrac];

                double temp = tempA + tempB;
                tempB = -(temp - tempA - tempB);
                tempA = temp;

                temp = tempA * HEX_40000000;
                baseA = tempA + temp - temp;
                baseB = tempB + (tempA - baseA);

                epsilon = x - intFrac / 1024.0;
            }


            /* Compute expm1(epsilon) */
            double zb = 0.008336750013465571;
            zb = zb * epsilon + 0.041666663879186654;
            zb = zb * epsilon + 0.16666666666745392;
            zb = zb * epsilon + 0.49999999999999994;
            zb *= epsilon;
            zb *= epsilon;

            double za = epsilon;
            double temp = za + zb;
            zb = -(temp - za - zb);
            za = temp;

            temp = za * HEX_40000000;
            temp = za + temp - temp;
            zb += za - temp;
            za = temp;

            /* Combine the parts.   expm1(a+b) = expm1(a) + expm1(b) + expm1(a)*expm1(b) */
            double ya = za * baseA;
            //double yb = za*baseB + zb*baseA + zb*baseB;
            temp = ya + za * baseB;
            double yb = -(temp - ya - za * baseB);
            ya = temp;

            temp = ya + zb * baseA;
            yb += -(temp - ya - zb * baseA);
            ya = temp;

            temp = ya + zb * baseB;
            yb += -(temp - ya - zb * baseB);
            ya = temp;

            //ya = ya + za + baseA;
            //yb = yb + zb + baseB;
            temp = ya + baseA;
            yb += -(temp - baseA - ya);
            ya = temp;

            temp = ya + za;
            //yb += (ya > za) ? -(temp - ya - za) : -(temp - za - ya);
            yb += -(temp - ya - za);
            ya = temp;

            temp = ya + baseB;
            //yb += (ya > baseB) ? -(temp - ya - baseB) : -(temp - baseB - ya);
            yb += -(temp - ya - baseB);
            ya = temp;

            temp = ya + zb;
            //yb += (ya > zb) ? -(temp - ya - zb) : -(temp - zb - ya);
            yb += -(temp - ya - zb);
            ya = temp;

            if (negative)
            {
                /* Compute expm1(-x) = -expm1(x) / (expm1(x) + 1) */
                double denom = 1.0 + ya;
                double denomr = 1.0 / denom;
                double denomb = -(denom - 1.0 - ya) + yb;
                double ratio = ya * denomr;
                temp = ratio * HEX_40000000;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ra = ratio + temp - temp;
                double ra = ratio + temp - temp;
                double rb = ratio - ra;

                temp = denom * HEX_40000000;
                za = denom + temp - temp;
                zb = denom - za;

                rb += (ya - za * ra - za * rb - zb * ra - zb * rb) * denomr;

                // f(x) = x/1+x
                // Compute f'(x)
                // Product rule:  d(uv) = du*v + u*dv
                // Chain rule:  d(f(g(x)) = f'(g(x))*f(g'(x))
                // d(1/x) = -1/(x*x)
                // d(1/1+x) = -1/( (1+x)^2) *  1 =  -1/((1+x)*(1+x))
                // d(x/1+x) = -x/((1+x)(1+x)) + 1/1+x = 1 / ((1+x)(1+x))

                // Adjust for yb
                rb += yb * denomr; // numerator
                rb += -ya * denomb * denomr * denomr; // denominator

                // negate
                ya = -ra;
                yb = -rb;
            }

            if (hiPrecOut != null)
            {
                hiPrecOut[0] = ya;
                hiPrecOut[1] = yb;
            }

            return ya + yb;
        }

        /// <summary>
        /// Natural logarithm.
        /// </summary>
        /// <param name="x">   a double </param>
        /// <returns> log(x) </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double log(final double x)
        public static double Log(double x)
        {
            return Log(x, null);
        }

        /// <summary>
        /// Internal helper method for natural logarithm function. </summary>
        /// <param name="x"> original argument of the natural logarithm function </param>
        /// <param name="hiPrec"> extra bits of precision on output (To Be Confirmed) </param>
        /// <returns> log(x) </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private static double log(final double x, final double[] hiPrec)
        private static double Log(double x, double[] hiPrec)
        {
            if (x == 0)
            { // Handle special case of +0/-0
                return double.NegativeInfinity;
            }
            long bits = BitConverter.DoubleToInt64Bits(x);

            /* Handle special cases of negative input, and NaN */
            if (((bits & 0x8000000000000000L) != 0 || x != x) && x != 0.0)
            {
                if (hiPrec != null)
                {
                    hiPrec[0] = Double.NaN;
                }

                return Double.NaN;
            }

            /* Handle special cases of Positive infinity. */
            if (x == double.PositiveInfinity)
            {
                if (hiPrec != null)
                {
                    hiPrec[0] = double.PositiveInfinity;
                }

                return double.PositiveInfinity;
            }

            /* Extract the exponent */
            int exp = (int)(bits >> 52) - 1023;

            if ((bits & 0x7ff0000000000000L) == 0)
            {
                // Subnormal!
                if (x == 0)
                {
                    // Zero
                    if (hiPrec != null)
                    {
                        hiPrec[0] = double.NegativeInfinity;
                    }

                    return double.NegativeInfinity;
                }

                /* Normalize the subnormal number. */
                bits <<= 1;
                while ((bits & 0x0010000000000000L) == 0)
                {
                    --exp;
                    bits <<= 1;
                }
            }


            if ((exp == -1 || exp == 0) && x < 1.01 && x > 0.99 && hiPrec == null)
            {
                /* The normal method doesn't work well in the range [0.99, 1.01], so call do a straight
               polynomial expansion in higer precision. */

                /* Compute x - 1.0 and split it */
                double xa = x - 1.0;
                double xb = xa - x + 1.0;
                double tmp = xa * HEX_40000000;
                double aa = xa + tmp - tmp;
                double ab = xa - aa;
                xa = aa;
                xb = ab;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] lnCoef_last = LN_QUICK_COEF[LN_QUICK_COEF.length - 1];
                double[] lnCoef_last = LN_QUICK_COEF[LN_QUICK_COEF.Length - 1];
                double ya = lnCoef_last[0];
                double yb = lnCoef_last[1];

                for (int i = LN_QUICK_COEF.Length - 2; i >= 0; i--)
                {
                    /* Multiply a = y * x */
                    aa = ya * xa;
                    ab = ya * xb + yb * xa + yb * xb;
                    /* split, so now y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;

                    /* Add  a = y + lnQuickCoef */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] lnCoef_i = LN_QUICK_COEF[i];
                    double[] lnCoef_i = LN_QUICK_COEF[i];
                    aa = ya + lnCoef_i[0];
                    ab = yb + lnCoef_i[1];
                    /* Split y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;
                }

                /* Multiply a = y * x */
                aa = ya * xa;
                ab = ya * xb + yb * xa + yb * xb;
                /* split, so now y = a */
                tmp = aa * HEX_40000000;
                ya = aa + tmp - tmp;
                yb = aa - ya + ab;

                return ya + yb;
            }

            // lnm is a log of a number in the range of 1.0 - 2.0, so 0 <= lnm < ln(2)
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] lnm = lnMant.LN_MANT[(int)((bits & 0x000ffc0000000000L) >> 42)];
            double[] lnm = lnMant.LN_MANT[(int)((bits & 0x000ffc0000000000L) >> 42)];

            /*
        double epsilon = x / Double.longBitsToDouble(bits & 0xfffffc0000000000L);
    
        epsilon -= 1.0;
             */

            // y is the most significant 10 bits of the mantissa
            //double y = Double.longBitsToDouble(bits & 0xfffffc0000000000L);
            //double epsilon = (x - y) / y;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double epsilon = (bits & 0x3ffffffffffL) / (TWO_POWER_52 + (bits & 0x000ffc0000000000L));
            double epsilon = (bits & 0x3ffffffffffL) / (TWO_POWER_52 + (bits & 0x000ffc0000000000L));

            double lnza = 0.0;
            double lnzb = 0.0;

            if (hiPrec != null)
            {
                /* split epsilon -> x */
                double tmp = epsilon * HEX_40000000;
                double aa = epsilon + tmp - tmp;
                double ab = epsilon - aa;
                double xa = aa;
                double xb = ab;

                /* Need a more accurate epsilon, so adjust the division. */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double numer = bits & 0x3ffffffffffL;
                double numer = bits & 0x3ffffffffffL;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double denom = TWO_POWER_52 + (bits & 0x000ffc0000000000L);
                double denom = TWO_POWER_52 + (bits & 0x000ffc0000000000L);
                aa = numer - xa * denom - xb * denom;
                xb += aa / denom;

                /* Remez polynomial evaluation */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] lnCoef_last = LN_HI_PREC_COEF[LN_HI_PREC_COEF.length-1];
                double[] lnCoef_last = LN_HI_PREC_COEF[LN_HI_PREC_COEF.Length - 1];
                double ya = lnCoef_last[0];
                double yb = lnCoef_last[1];

                for (int i = LN_HI_PREC_COEF.Length - 2; i >= 0; i--)
                {
                    /* Multiply a = y * x */
                    aa = ya * xa;
                    ab = ya * xb + yb * xa + yb * xb;
                    /* split, so now y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;

                    /* Add  a = y + lnHiPrecCoef */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] lnCoef_i = LN_HI_PREC_COEF[i];
                    double[] lnCoef_i = LN_HI_PREC_COEF[i];
                    aa = ya + lnCoef_i[0];
                    ab = yb + lnCoef_i[1];
                    /* Split y = a */
                    tmp = aa * HEX_40000000;
                    ya = aa + tmp - tmp;
                    yb = aa - ya + ab;
                }

                /* Multiply a = y * x */
                aa = ya * xa;
                ab = ya * xb + yb * xa + yb * xb;

                /* split, so now lnz = a */
                /*
          tmp = aa * 1073741824.0;
          lnza = aa + tmp - tmp;
          lnzb = aa - lnza + ab;
                 */
                lnza = aa + ab;
                lnzb = -(lnza - aa - ab);
            }
            else
            {
                /* High precision not required.  Eval Remez polynomial
             using standard double precision */
                lnza = -0.16624882440418567;
                lnza = lnza * epsilon + 0.19999954120254515;
                lnza = lnza * epsilon + -0.2499999997677497;
                lnza = lnza * epsilon + 0.3333333333332802;
                lnza = lnza * epsilon + -0.5;
                lnza = lnza * epsilon + 1.0;
                lnza *= epsilon;
            }

            /* Relative sizes:
             * lnzb     [0, 2.33E-10]
             * lnm[1]   [0, 1.17E-7]
             * ln2B*exp [0, 1.12E-4]
             * lnza      [0, 9.7E-4]
             * lnm[0]   [0, 0.692]
             * ln2A*exp [0, 709]
             */

            /* Compute the following sum:
             * lnzb + lnm[1] + ln2B*exp + lnza + lnm[0] + ln2A*exp;
             */

            //return lnzb + lnm[1] + ln2B*exp + lnza + lnm[0] + ln2A*exp;
            double a = LN_2_A * exp;
            double b = 0.0;
            double c = a + lnm[0];
            double d = -(c - a - lnm[0]);
            a = c;
            b += d;

            c = a + lnza;
            d = -(c - a - lnza);
            a = c;
            b += d;

            c = a + LN_2_B * exp;
            d = -(c - a - LN_2_B * exp);
            a = c;
            b += d;

            c = a + lnm[1];
            d = -(c - a - lnm[1]);
            a = c;
            b += d;

            c = a + lnzb;
            d = -(c - a - lnzb);
            a = c;
            b += d;

            if (hiPrec != null)
            {
                hiPrec[0] = a;
                hiPrec[1] = b;
            }

            return a + b;
        }

        /// <summary>
        /// Computes log(1 + x).
        /// </summary>
        /// <param name="x"> Number. </param>
        /// <returns> {@code log(1 + x)}. </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double log1p(final double x)
        public static double Log1p(double x)
        {
            if (x == -1)
            {
                return double.NegativeInfinity;
            }

            if (x == double.PositiveInfinity)
            {
                return double.PositiveInfinity;
            }

            if (x > 1e-6 || x < -1e-6)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xpa = 1 + x;
                double xpa = 1 + x;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xpb = -(xpa - 1 - x);
                double xpb = -(xpa - 1 - x);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] hiPrec = new double[2];
                double[] hiPrec = new double[2];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lores = log(xpa, hiPrec);
                double lores = Log(xpa, hiPrec);
                if (double.IsInfinity(lores))
                { // Don't allow this to be converted to NaN
                    return lores;
                }

                // Do a taylor series expansion around xpa:
                //   f(x+y) = f(x) + f'(x) y + f''(x)/2 y^2
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fx1 = xpb / xpa;
                double fx1 = xpb / xpa;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double epsilon = 0.5 * fx1 + 1;
                double epsilon = 0.5 * fx1 + 1;
                return epsilon * fx1 + hiPrec[1] + hiPrec[0];
            }
            else
            {
                // Value is small |x| < 1e6, do a Taylor series centered on 1.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = (x * F_1_3 - F_1_2) * x + 1;
                double y = (x * F_1_3 - F_1_2) * x + 1;
                return y * x;
            }
        }

        /// <summary>
        /// Compute the base 10 logarithm. </summary>
        /// <param name="x"> a number </param>
        /// <returns> log10(x) </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double log10(final double x)
        public static double Log10(double x)
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double hiPrec[] = new double[2];
            double[] hiPrec = new double[2];

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lores = log(x, hiPrec);
            double lores = Log(x, hiPrec);
            if (double.IsInfinity(lores))
            { // don't allow this to be converted to NaN
                return lores;
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tmp = hiPrec[0] * HEX_40000000;
            double tmp = hiPrec[0] * HEX_40000000;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lna = hiPrec[0] + tmp - tmp;
            double lna = hiPrec[0] + tmp - tmp;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lnb = hiPrec[0] - lna + hiPrec[1];
            double lnb = hiPrec[0] - lna + hiPrec[1];

            const double rln10a = 0.4342944622039795;
            const double rln10b = 1.9699272335463627E-8;

            return rln10b * lnb + rln10b * lna + rln10a * lnb + rln10a * lna;
        }

        /// <summary>
        /// Computes the <a href="http://mathworld.wolfram.com/Logarithm.html">
        /// logarithm</a> in a given base.
        /// 
        /// Returns {@code NaN} if either argument is negative.
        /// If {@code base} is 0 and {@code x} is positive, 0 is returned.
        /// If {@code base} is positive and {@code x} is 0,
        /// {@code Double.NEGATIVE_INFINITY} is returned.
        /// If both arguments are 0, the result is {@code NaN}.
        /// </summary>
        /// <param name="base"> Base of the logarithm, must be greater than 0. </param>
        /// <param name="x"> Argument, must be greater than 0. </param>
        /// <returns> the value of the logarithm, i.e. the number {@code y} such that
        /// <code>base<sup>y</sup> = x</code>.
        /// @since 1.2 (previously in {@code MathUtils}, moved as of version 3.0) </returns>
        public static double Log(double @base, double x)
        {
            return Log(x) / Log(@base);
        }

        /// <summary>
        /// Power function.  Compute x^y.
        /// </summary>
        /// <param name="x">   a double </param>
        /// <param name="y">   a double </param>
        /// <returns> double </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double pow(final double x, final double y)
        public static double Pow(double x, double y)
        {

            if (y == 0)
            {
                // y = -0 or y = +0
                return 1.0;
            }
            else
            {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long yBits = Double.doubleToRawLongBits(y);
                long yBits = BitConverter.DoubleToInt64Bits(y);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int yRawExp = (int)((yBits & MASK_DOUBLE_EXPONENT) >> 52);
                int yRawExp = (int)((yBits & MASK_DOUBLE_EXPONENT) >> 52);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long yRawMantissa = yBits & MASK_DOUBLE_MANTISSA;
                long yRawMantissa = yBits & MASK_DOUBLE_MANTISSA;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long xBits = Double.doubleToRawLongBits(x);
                long xBits = BitConverter.DoubleToInt64Bits(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int xRawExp = (int)((xBits & MASK_DOUBLE_EXPONENT) >> 52);
                int xRawExp = (int)((xBits & MASK_DOUBLE_EXPONENT) >> 52);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long xRawMantissa = xBits & MASK_DOUBLE_MANTISSA;
                long xRawMantissa = xBits & MASK_DOUBLE_MANTISSA;

                if (yRawExp > 1085)
                {
                    // y is either a very large integral value that does not fit in a long or it is a special number

                    if ((yRawExp == 2047 && yRawMantissa != 0) || (xRawExp == 2047 && xRawMantissa != 0))
                    {
                        // NaN
                        return Double.NaN;
                    }
                    else if (xRawExp == 1023 && xRawMantissa == 0)
                    {
                        // x = -1.0 or x = +1.0
                        if (yRawExp == 2047)
                        {
                            // y is infinite
                            return Double.NaN;
                        }
                        else
                        {
                            // y is a large even integer
                            return 1.0;
                        }
                    }
                    else
                    {
                        // the absolute value of x is either greater or smaller than 1.0

                        // if yRawExp == 2047 and mantissa is 0, y = -infinity or y = +infinity
                        // if 1085 < yRawExp < 2047, y is simply a large number, however, due to limited
                        // accuracy, at this magnitude it behaves just like infinity with regards to x
                        if ((y > 0) ^ (xRawExp < 1023))
                        {
                            // either y = +infinity (or large engouh) and abs(x) > 1.0
                            // or     y = -infinity (or large engouh) and abs(x) < 1.0
                            return double.PositiveInfinity;
                        }
                        else
                        {
                            // either y = +infinity (or large engouh) and abs(x) < 1.0
                            // or     y = -infinity (or large engouh) and abs(x) > 1.0
                            return +0.0;
                        }
                    }

                }
                else
                {
                    // y is a regular non-zero number

                    if (yRawExp >= 1023)
                    {
                        // y may be an integral value, which should be handled specifically
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long yFullMantissa = IMPLICIT_HIGH_BIT | yRawMantissa;
                        long yFullMantissa = IMPLICIT_HIGH_BIT | yRawMantissa;
                        if (yRawExp < 1075)
                        {
                            // normal number with negative shift that may have a fractional part
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long integralMask = (-1L) << (1075 - yRawExp);
                            long integralMask = (-1L) << (1075 - yRawExp);
                            if ((yFullMantissa & integralMask) == yFullMantissa)
                            {
                                // all fractional bits are 0, the number is really integral
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long l = yFullMantissa >> (1075 - yRawExp);
                                long l = yFullMantissa >> (1075 - yRawExp);
                                return FastMath.Pow(x, (y < 0) ? -l : l);
                            }
                        }
                        else
                        {
                            // normal number with positive shift, always an integral value
                            // we know it fits in a primitive long because yRawExp > 1085 has been handled above
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long l = yFullMantissa << (yRawExp - 1075);
                            long l = yFullMantissa << (yRawExp - 1075);
                            return FastMath.Pow(x, (y < 0) ? -l : l);
                        }
                    }

                    // y is a non-integral value

                    if (x == 0)
                    {
                        // x = -0 or x = +0
                        // the integer powers have already been handled above
                        return y < 0 ? double.PositiveInfinity : +0.0;
                    }
                    else if (xRawExp == 2047)
                    {
                        if (xRawMantissa == 0)
                        {
                            // x = -infinity or x = +infinity
                            return (y < 0) ? +0.0 : double.PositiveInfinity;
                        }
                        else
                        {
                            // NaN
                            return Double.NaN;
                        }
                    }
                    else if (x < 0)
                    {
                        // the integer powers have already been handled above
                        return Double.NaN;
                    }
                    else
                    {

                        // this is the general case, for regular fractional numbers x and y

                        // Split y into ya and yb such that y = ya+yb
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tmp = y * HEX_40000000;
                        double tmp = y * HEX_40000000;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ya = (y + tmp) - tmp;
                        double ya = (y + tmp) - tmp;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double yb = y - ya;
                        double yb = y - ya;

                        /* Compute ln(x) */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lns[] = new double[2];
                        double[] lns = new double[2];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lores = log(x, lns);
                        double lores = Log(x, lns);
                        if (double.IsInfinity(lores))
                        { // don't allow this to be converted to NaN
                            return lores;
                        }

                        double lna = lns[0];
                        double lnb = lns[1];

                        /* resplit lns */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tmp1 = lna * HEX_40000000;
                        double tmp1 = lna * HEX_40000000;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tmp2 = (lna + tmp1) - tmp1;
                        double tmp2 = (lna + tmp1) - tmp1;
                        lnb += lna - tmp2;
                        lna = tmp2;

                        // y*ln(x) = (aa+ab)
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double aa = lna * ya;
                        double aa = lna * ya;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ab = lna * yb + lnb * ya + lnb * yb;
                        double ab = lna * yb + lnb * ya + lnb * yb;

                        lna = aa + ab;
                        lnb = -(lna - aa - ab);

                        double z = 1.0 / 120.0;
                        z = z * lnb + (1.0 / 24.0);
                        z = z * lnb + (1.0 / 6.0);
                        z = z * lnb + 0.5;
                        z = z * lnb + 1.0;
                        z *= lnb;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double result = exp(lna, z, null);
                        double result = Exp(lna, z, null);
                        //result = result + result * z;
                        return result;

                    }
                }

            }

        }

        /// <summary>
        /// Raise a double to an int power.
        /// </summary>
        /// <param name="d"> Number to raise. </param>
        /// <param name="e"> Exponent. </param>
        /// <returns> d<sup>e</sup>
        /// @since 3.1 </returns>
        public static double Pow(double d, int e)
        {
            return Pow(d, (long) e);
        }

        /// <summary>
        /// Raise a double to a long power.
        /// </summary>
        /// <param name="d"> Number to raise. </param>
        /// <param name="e"> Exponent. </param>
        /// <returns> d<sup>e</sup>
        /// @since 3.6 </returns>
        public static double Pow(double d, long e)
        {
            if (e == 0)
            {
                return 1.0;
            }
            else if (e > 0)
            {
                return (new Split(d)).Pow(e).full;
            }
            else
            {
                return (new Split(d)).Reciprocal().Pow(-e).full;
            }
        }

        /// <summary>
        /// Class operator on double numbers split into one 26 bits number and one 27 bits number. </summary>
        private class Split
        {

            /// <summary>
            /// Split version of NaN. </summary>
            public static readonly Split NAN = new Split(Double.NaN, 0);

            /// <summary>
            /// Split version of positive infinity. </summary>
            public static readonly Split POSITIVE_INFINITY = new Split(double.PositiveInfinity, 0);

            /// <summary>
            /// Split version of negative infinity. </summary>
            public static readonly Split NEGATIVE_INFINITY = new Split(double.NegativeInfinity, 0);

            /// <summary>
            /// Full number. </summary>
            internal readonly double full;

            /// <summary>
            /// High order bits. </summary>
            internal readonly double high;

            /// <summary>
            /// Low order bits. </summary>
            internal readonly double low;

            /// <summary>
            /// Simple constructor. </summary>
            /// <param name="x"> number to split </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: Split(final double x)
            internal Split(double x)
            {
                full = x;
                high = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(x) & ((-1L) << 27));
                low = x - high;
            }

            /// <summary>
            /// Simple constructor. </summary>
            /// <param name="high"> high order bits </param>
            /// <param name="low"> low order bits </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: Split(final double high, final double low)
            internal Split(double high, double low) : this(high == 0.0 ? (low == 0.0 && BitConverter.DoubleToInt64Bits(high) == long.MinValue ? -0.0 : low) : high + low, high, low)
            {
            }

            /// <summary>
            /// Simple constructor. </summary>
            /// <param name="full"> full number </param>
            /// <param name="high"> high order bits </param>
            /// <param name="low"> low order bits </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: Split(final double full, final double high, final double low)
            internal Split(double full, double high, double low)
            {
                this.full = full;
                this.high = high;
                this.low = low;
            }

            /// <summary>
            /// Multiply the instance by another one. </summary>
            /// <param name="b"> other instance to multiply by </param>
            /// <returns> product </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public Split multiply(final Split b)
            public virtual Split Multiply(Split b)
            {
                // beware the following expressions must NOT be simplified, they rely on floating point arithmetic properties
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Split mulBasic = new Split(full * b.full);
                Split mulBasic = new Split(full * b.full);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double mulError = low * b.low - (((mulBasic.full - high * b.high) - low * b.high) - high * b.low);
                double mulError = low * b.low - (((mulBasic.full - high * b.high) - low * b.high) - high * b.low);
                return new Split(mulBasic.high, mulBasic.low + mulError);
            }

            /// <summary>
            /// Compute the reciprocal of the instance. </summary>
            /// <returns> reciprocal of the instance </returns>
            public virtual Split Reciprocal()
            {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double approximateInv = 1.0 / full;
                double approximateInv = 1.0 / full;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Split splitInv = new Split(approximateInv);
                Split splitInv = new Split(approximateInv);

                // if 1.0/d were computed perfectly, remultiplying it by d should give 1.0
                // we want to estimate the error so we can fix the low order bits of approximateInvLow
                // beware the following expressions must NOT be simplified, they rely on floating point arithmetic properties
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Split product = multiply(splitInv);
                Split product = Multiply(splitInv);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double error = (product.high - 1) + product.low;
                double error = (product.high - 1) + product.low;

                // better accuracy estimate of reciprocal
                return double.IsNaN(error) ? splitInv : new Split(splitInv.high, splitInv.low - error / full);

            }

            /// <summary>
            /// Computes this^e. </summary>
            /// <param name="e"> exponent (beware, here it MUST be > 0; the only exclusion is Long.MIN_VALUE) </param>
            /// <returns> d^e, split in high and low bits
            /// @since 3.6 </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private Split pow(final long e)
            internal virtual Split Pow(long e)
            {

                // prepare result
                Split result = new Split(1);

                // d^(2p)
                Split d2p = new Split(full, high, low);

                for (long p = e; p != 0; p = (long)((ulong)p >> 1))
                {

                    if ((p & 0x1) != 0)
                    {
                        // accurate multiplication result = result * d^(2p) using Veltkamp TwoProduct algorithm
                        result = result.Multiply(d2p);
                    }

                    // accurate squaring d^(2(p+1)) = d^(2p) * d^(2p) using Veltkamp TwoProduct algorithm
                    d2p = d2p.Multiply(d2p);

                }

                if (double.IsNaN(result.full))
                {
                    if (double.IsNaN(full))
                    {
                        return Split.NAN;
                    }
                    else
                    {
                        // some intermediate numbers exceeded capacity,
                        // and the low order bits became NaN (because infinity - infinity = NaN)
                        if (FastMath.Abs(full) < 1)
                        {
                            return new Split(FastMath.CopySign(0.0, full), 0.0);
                        }
                        else if (full < 0 && (e & 0x1) == 1)
                        {
                            return Split.NEGATIVE_INFINITY;
                        }
                        else
                        {
                            return Split.POSITIVE_INFINITY;
                        }
                    }
                }
                else
                {
                    return result;
                }

            }

        }

        /// <summary>
        ///  Computes sin(x) - x, where |x| < 1/16.
        ///  Use a Remez polynomial approximation. </summary>
        ///  <param name="x"> a number smaller than 1/16 </param>
        ///  <returns> sin(x) - x </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private static double polySine(final double x)
        private static double PolySine(double x)
        {
            double x2 = x * x;

            double p = 2.7553817452272217E-6;
            p = p * x2 + -1.9841269659586505E-4;
            p = p * x2 + 0.008333333333329196;
            p = p * x2 + -0.16666666666666666;
            //p *= x2;
            //p *= x;
            p = p * x2 * x;

            return p;
        }

        /// <summary>
        ///  Computes cos(x) - 1, where |x| < 1/16.
        ///  Use a Remez polynomial approximation. </summary>
        ///  <param name="x"> a number smaller than 1/16 </param>
        ///  <returns> cos(x) - 1 </returns>
        private static double PolyCosine(double x)
        {
            double x2 = x * x;

            double p = 2.479773539153719E-5;
            p = p * x2 + -0.0013888888689039883;
            p = p * x2 + 0.041666666666621166;
            p = p * x2 + -0.49999999999999994;
            p *= x2;

            return p;
        }

        /// <summary>
        ///  Compute sine over the first quadrant (0 < x < pi/2).
        ///  Use combination of table lookup and rational polynomial expansion. </summary>
        ///  <param name="xa"> number from which sine is requested </param>
        ///  <param name="xb"> extra bits for x (may be 0.0) </param>
        ///  <returns> sin(xa + xb) </returns>
        private static double SinQ(double xa, double xb)
        {
            int idx = (int)((xa * 8.0) + 0.5);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double epsilon = xa - EIGHTHS[idx];
            double epsilon = xa - EIGHTHS[idx]; //idx*0.125;

            // Table lookups
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sintA = SINE_TABLE_A[idx];
            double sintA = SINE_TABLE_A[idx];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sintB = SINE_TABLE_B[idx];
            double sintB = SINE_TABLE_B[idx];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double costA = COSINE_TABLE_A[idx];
            double costA = COSINE_TABLE_A[idx];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double costB = COSINE_TABLE_B[idx];
            double costB = COSINE_TABLE_B[idx];

            // Polynomial eval of sin(epsilon), cos(epsilon)
            double sinEpsA = epsilon;
            double sinEpsB = PolySine(epsilon);
            const double cosEpsA = 1.0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double cosEpsB = polyCosine(epsilon);
            double cosEpsB = PolyCosine(epsilon);

            // Split epsilon   xa + xb = x
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double temp = sinEpsA * HEX_40000000;
            double temp = sinEpsA * HEX_40000000;
            double temp2 = (sinEpsA + temp) - temp;
            sinEpsB += sinEpsA - temp2;
            sinEpsA = temp2;

            /* Compute sin(x) by angle addition formula */
            double result;

            /* Compute the following sum:
             *
             * result = sintA + costA*sinEpsA + sintA*cosEpsB + costA*sinEpsB +
             *          sintB + costB*sinEpsA + sintB*cosEpsB + costB*sinEpsB;
             *
             * Ranges of elements
             *
             * xxxtA   0            PI/2
             * xxxtB   -1.5e-9      1.5e-9
             * sinEpsA -0.0625      0.0625
             * sinEpsB -6e-11       6e-11
             * cosEpsA  1.0
             * cosEpsB  0           -0.0625
             *
             */

            //result = sintA + costA*sinEpsA + sintA*cosEpsB + costA*sinEpsB +
            //          sintB + costB*sinEpsA + sintB*cosEpsB + costB*sinEpsB;

            //result = sintA + sintA*cosEpsB + sintB + sintB * cosEpsB;
            //result += costA*sinEpsA + costA*sinEpsB + costB*sinEpsA + costB * sinEpsB;
            double a = 0;
            double b = 0;

            double t = sintA;
            double c = a + t;
            double d = -(c - a - t);
            a = c;
            b += d;

            t = costA * sinEpsA;
            c = a + t;
            d = -(c - a - t);
            a = c;
            b += d;

            b = b + sintA * cosEpsB + costA * sinEpsB;
            /*
        t = sintA*cosEpsB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;
    
        t = costA*sinEpsB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;
             */

            b = b + sintB + costB * sinEpsA + sintB * cosEpsB + costB * sinEpsB;
            /*
        t = sintB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;
    
        t = costB*sinEpsA;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;
    
        t = sintB*cosEpsB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;
    
        t = costB*sinEpsB;
        c = a + t;
        d = -(c - a - t);
        a = c;
        b = b + d;
             */

            if (xb != 0.0)
            {
                t = ((costA + costB) * (cosEpsA + cosEpsB) - (sintA + sintB) * (sinEpsA + sinEpsB)) * xb; // approximate cosine*xb
                c = a + t;
                d = -(c - a - t);
                a = c;
                b += d;
            }

            result = a + b;

            return result;
        }

        /// <summary>
        /// Compute cosine in the first quadrant by subtracting input from PI/2 and
        /// then calling sinQ.  This is more accurate as the input approaches PI/2. </summary>
        ///  <param name="xa"> number from which cosine is requested </param>
        ///  <param name="xb"> extra bits for x (may be 0.0) </param>
        ///  <returns> cos(xa + xb) </returns>
        private static double CosQ(double xa, double xb)
        {
            const double pi2a = 1.5707963267948966;
            const double pi2b = 6.123233995736766E-17;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a = pi2a - xa;
            double a = pi2a - xa;
            double b = -(a - pi2a + xa);
            b += pi2b - xb;

            return SinQ(a, b);
        }

        /// <summary>
        ///  Compute tangent (or cotangent) over the first quadrant.   0 < x < pi/2
        ///  Use combination of table lookup and rational polynomial expansion. </summary>
        ///  <param name="xa"> number from which sine is requested </param>
        ///  <param name="xb"> extra bits for x (may be 0.0) </param>
        ///  <param name="cotanFlag"> if true, compute the cotangent instead of the tangent </param>
        ///  <returns> tan(xa+xb) (or cotangent, depending on cotanFlag) </returns>
        private static double TanQ(double xa, double xb, bool cotanFlag)
        {

            int idx = (int)((xa * 8.0) + 0.5);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double epsilon = xa - EIGHTHS[idx];
            double epsilon = xa - EIGHTHS[idx]; //idx*0.125;

            // Table lookups
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sintA = SINE_TABLE_A[idx];
            double sintA = SINE_TABLE_A[idx];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sintB = SINE_TABLE_B[idx];
            double sintB = SINE_TABLE_B[idx];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double costA = COSINE_TABLE_A[idx];
            double costA = COSINE_TABLE_A[idx];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double costB = COSINE_TABLE_B[idx];
            double costB = COSINE_TABLE_B[idx];

            // Polynomial eval of sin(epsilon), cos(epsilon)
            double sinEpsA = epsilon;
            double sinEpsB = PolySine(epsilon);
            const double cosEpsA = 1.0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double cosEpsB = polyCosine(epsilon);
            double cosEpsB = PolyCosine(epsilon);

            // Split epsilon   xa + xb = x
            double temp = sinEpsA * HEX_40000000;
            double temp2 = (sinEpsA + temp) - temp;
            sinEpsB += sinEpsA - temp2;
            sinEpsA = temp2;

            /* Compute sin(x) by angle addition formula */

            /* Compute the following sum:
             *
             * result = sintA + costA*sinEpsA + sintA*cosEpsB + costA*sinEpsB +
             *          sintB + costB*sinEpsA + sintB*cosEpsB + costB*sinEpsB;
             *
             * Ranges of elements
             *
             * xxxtA   0            PI/2
             * xxxtB   -1.5e-9      1.5e-9
             * sinEpsA -0.0625      0.0625
             * sinEpsB -6e-11       6e-11
             * cosEpsA  1.0
             * cosEpsB  0           -0.0625
             *
             */

            //result = sintA + costA*sinEpsA + sintA*cosEpsB + costA*sinEpsB +
            //          sintB + costB*sinEpsA + sintB*cosEpsB + costB*sinEpsB;

            //result = sintA + sintA*cosEpsB + sintB + sintB * cosEpsB;
            //result += costA*sinEpsA + costA*sinEpsB + costB*sinEpsA + costB * sinEpsB;
            double a = 0;
            double b = 0;

            // Compute sine
            double t = sintA;
            double c = a + t;
            double d = -(c - a - t);
            a = c;
            b += d;

            t = costA * sinEpsA;
            c = a + t;
            d = -(c - a - t);
            a = c;
            b += d;

            b += sintA * cosEpsB + costA * sinEpsB;
            b += sintB + costB * sinEpsA + sintB * cosEpsB + costB * sinEpsB;

            double sina = a + b;
            double sinb = -(sina - a - b);

            // Compute cosine

            a = b = c = d = 0.0;

            t = costA * cosEpsA;
            c = a + t;
            d = -(c - a - t);
            a = c;
            b += d;

            t = -sintA * sinEpsA;
            c = a + t;
            d = -(c - a - t);
            a = c;
            b += d;

            b += costB * cosEpsA + costA * cosEpsB + costB * cosEpsB;
            b -= sintB * sinEpsA + sintA * sinEpsB + sintB * sinEpsB;

            double cosa = a + b;
            double cosb = -(cosa - a - b);

            if (cotanFlag)
            {
                double tmp;
                tmp = cosa;
                cosa = sina;
                sina = tmp;
                tmp = cosb;
                cosb = sinb;
                sinb = tmp;
            }


            /* estimate and correct, compute 1.0/(cosa+cosb) */
            /*
        double est = (sina+sinb)/(cosa+cosb);
        double err = (sina - cosa*est) + (sinb - cosb*est);
        est += err/(cosa+cosb);
        err = (sina - cosa*est) + (sinb - cosb*est);
             */

            // f(x) = 1/x,   f'(x) = -1/x^2

            double est = sina / cosa;

            /* Split the estimate to get more accurate read on division rounding */
            temp = est * HEX_40000000;
            double esta = (est + temp) - temp;
            double estb = est - esta;

            temp = cosa * HEX_40000000;
            double cosaa = (cosa + temp) - temp;
            double cosab = cosa - cosaa;

            //double err = (sina - est*cosa)/cosa;  // Correction for division rounding
            double err = (sina - esta * cosaa - esta * cosab - estb * cosaa - estb * cosab) / cosa; // Correction for division rounding
            err += sinb / cosa; // Change in est due to sinb
            err += -sina * cosb / cosa / cosa; // Change in est due to cosb

            if (xb != 0.0)
            {
                // tan' = 1 + tan^2      cot' = -(1 + cot^2)
                // Approximate impact of xb
                double xbadj = xb + est * est * xb;
                if (cotanFlag)
                {
                    xbadj = -xbadj;
                }

                err += xbadj;
            }

            return est + err;
        }

        /// <summary>
        /// Reduce the input argument using the Payne and Hanek method.
        ///  This is good for all inputs 0.0 < x < inf
        ///  Output is remainder after dividing by PI/2
        ///  The result array should contain 3 numbers.
        ///  result[0] is the integer portion, so mod 4 this gives the quadrant.
        ///  result[1] is the upper bits of the remainder
        ///  result[2] is the lower bits of the remainder
        /// </summary>
        /// <param name="x"> number to reduce </param>
        /// <param name="result"> placeholder where to put the result </param>
        private static void ReducePayneHanek(double x, double[] result)
        {
            /* Convert input double to bits */
            long inbits = BitConverter.DoubleToInt64Bits(x);
            int exponent = (int)((inbits >> 52) & 0x7ff) - 1023;

            /* Convert to fixed point representation */
            inbits &= 0x000fffffffffffffL;
            inbits |= 0x0010000000000000L;

            /* Normalize input to be between 0.5 and 1.0 */
            exponent++;
            inbits <<= 11;

            /* Based on the exponent, get a shifted copy of recip2pi */
            long shpi0;
            long shpiA;
            long shpiB;
            int idx = exponent >> 6;
            int shift = exponent - (idx << 6);

            if (shift != 0)
            {
                shpi0 = (idx == 0) ? 0 : (RECIP_2PI[idx - 1] << shift);
                shpi0 |= (long)((ulong)RECIP_2PI[idx] >> (64 - shift));
                shpiA = (RECIP_2PI[idx] << shift) | ((long)((ulong)RECIP_2PI[idx + 1] >> (64 - shift)));
                shpiB = (RECIP_2PI[idx + 1] << shift) | ((long)((ulong)RECIP_2PI[idx + 2] >> (64 - shift)));
            }
            else
            {
                shpi0 = (idx == 0) ? 0 : RECIP_2PI[idx - 1];
                shpiA = RECIP_2PI[idx];
                shpiB = RECIP_2PI[idx + 1];
            }

            /* Multiply input by shpiA */
            long a = (long)((ulong)inbits >> 32);
            long b = inbits & 0xffffffffL;

            long c = (long)((ulong)shpiA >> 32);
            long d = shpiA & 0xffffffffL;

            long ac = a * c;
            long bd = b * d;
            long bc = b * c;
            long ad = a * d;

            long prodB = bd + (ad << 32);
            long prodA = ac + ((long)((ulong)ad >> 32));

            bool bita = (bd & 0x8000000000000000L) != 0;
            bool bitb = (ad & 0x80000000L) != 0;
            bool bitsum = (prodB & 0x8000000000000000L) != 0;

            /* Carry */
            if ((bita && bitb) || ((bita || bitb) && !bitsum))
            {
                prodA++;
            }

            bita = (prodB & 0x8000000000000000L) != 0;
            bitb = (bc & 0x80000000L) != 0;

            prodB += bc << 32;
            prodA += (long)((ulong)bc >> 32);

            bitsum = (prodB & 0x8000000000000000L) != 0;

            /* Carry */
            if ((bita && bitb) || ((bita || bitb) && !bitsum))
            {
                prodA++;
            }

            /* Multiply input by shpiB */
            c = (long)((ulong)shpiB >> 32);
            d = shpiB & 0xffffffffL;
            ac = a * c;
            bc = b * c;
            ad = a * d;

            /* Collect terms */
            ac += (long)((ulong)(bc + ad) >> 32);

            bita = (prodB & 0x8000000000000000L) != 0;
            bitb = (ac & 0x8000000000000000L) != 0;
            prodB += ac;
            bitsum = (prodB & 0x8000000000000000L) != 0;
            /* Carry */
            if ((bita && bitb) || ((bita || bitb) && !bitsum))
            {
                prodA++;
            }

            /* Multiply by shpi0 */
            c = (long)((ulong)shpi0 >> 32);
            d = shpi0 & 0xffffffffL;

            bd = b * d;
            bc = b * c;
            ad = a * d;

            prodA += bd + ((bc + ad) << 32);

            /*
             * prodA, prodB now contain the remainder as a fraction of PI.  We want this as a fraction of
             * PI/2, so use the following steps:
             * 1.) multiply by 4.
             * 2.) do a fixed point muliply by PI/4.
             * 3.) Convert to floating point.
             * 4.) Multiply by 2
             */

            /* This identifies the quadrant */
            int intPart = (int)((long)((ulong)prodA >> 62));

            /* Multiply by 4 */
            prodA <<= 2;
            prodA |= (long)((ulong)prodB >> 62);
            prodB <<= 2;

            /* Multiply by PI/4 */
            a = (long)((ulong)prodA >> 32);
            b = prodA & 0xffffffffL;

            c = (long)((ulong)PI_O_4_BITS[0] >> 32);
            d = PI_O_4_BITS[0] & 0xffffffffL;

            ac = a * c;
            bd = b * d;
            bc = b * c;
            ad = a * d;

            long prod2B = bd + (ad << 32);
            long prod2A = ac + ((long)((ulong)ad >> 32));

            bita = (bd & 0x8000000000000000L) != 0;
            bitb = (ad & 0x80000000L) != 0;
            bitsum = (prod2B & 0x8000000000000000L) != 0;

            /* Carry */
            if ((bita && bitb) || ((bita || bitb) && !bitsum))
            {
                prod2A++;
            }

            bita = (prod2B & 0x8000000000000000L) != 0;
            bitb = (bc & 0x80000000L) != 0;

            prod2B += bc << 32;
            prod2A += (long)((ulong)bc >> 32);

            bitsum = (prod2B & 0x8000000000000000L) != 0;

            /* Carry */
            if ((bita && bitb) || ((bita || bitb) && !bitsum))
            {
                prod2A++;
            }

            /* Multiply input by pio4bits[1] */
            c = (long)((ulong)PI_O_4_BITS[1] >> 32);
            d = PI_O_4_BITS[1] & 0xffffffffL;
            ac = a * c;
            bc = b * c;
            ad = a * d;

            /* Collect terms */
            ac += (long)((ulong)(bc + ad) >> 32);

            bita = (prod2B & 0x8000000000000000L) != 0;
            bitb = (ac & 0x8000000000000000L) != 0;
            prod2B += ac;
            bitsum = (prod2B & 0x8000000000000000L) != 0;
            /* Carry */
            if ((bita && bitb) || ((bita || bitb) && !bitsum))
            {
                prod2A++;
            }

            /* Multiply inputB by pio4bits[0] */
            a = (long)((ulong)prodB >> 32);
            b = prodB & 0xffffffffL;
            c = (long)((ulong)PI_O_4_BITS[0] >> 32);
            d = PI_O_4_BITS[0] & 0xffffffffL;
            ac = a * c;
            bc = b * c;
            ad = a * d;

            /* Collect terms */
            ac += (long)((ulong)(bc + ad) >> 32);

            bita = (prod2B & 0x8000000000000000L) != 0;
            bitb = (ac & 0x8000000000000000L) != 0;
            prod2B += ac;
            bitsum = (prod2B & 0x8000000000000000L) != 0;
            /* Carry */
            if ((bita && bitb) || ((bita || bitb) && !bitsum))
            {
                prod2A++;
            }

            /* Convert to double */
            double tmpA = ((long)((ulong)prod2A >> 12)) / TWO_POWER_52; // High order 52 bits
            double tmpB = (((prod2A & 0xfffL) << 40) + ((long)((ulong)prod2B >> 24))) / TWO_POWER_52 / TWO_POWER_52; // Low bits

            double sumA = tmpA + tmpB;
            double sumB = -(sumA - tmpA - tmpB);

            /* Multiply by PI/2 and return */
            result[0] = intPart;
            result[1] = sumA * 2.0;
            result[2] = sumB * 2.0;
        }

        /// <summary>
        /// Sine function.
        /// </summary>
        /// <param name="x"> Argument. </param>
        /// <returns> sin(x) </returns>
        public static double Sin(double x)
        {
            bool negative = false;
            int quadrant = 0;
            double xa;
            double xb = 0.0;

            /* Take absolute value of the input */
            xa = x;
            if (x < 0)
            {
                negative = true;
                xa = -xa;
            }

            /* Check for zero and negative zero */
            if (xa == 0.0)
            {
                long bits = BitConverter.DoubleToInt64Bits(x);
                if (bits < 0)
                {
                    return -0.0;
                }
                return 0.0;
            }

            if (xa != xa || xa == double.PositiveInfinity)
            {
                return Double.NaN;
            }

            /* Perform any argument reduction */
            if (xa > 3294198.0)
            {
                // PI * (2**20)
                // Argument too big for CodyWaite reduction.  Must use
                // PayneHanek.
                double[] reduceResults = new double[3];
                ReducePayneHanek(xa, reduceResults);
                quadrant = ((int) reduceResults[0]) & 3;
                xa = reduceResults[1];
                xb = reduceResults[2];
            }
            else if (xa > 1.5707963267948966)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CodyWaite cw = new CodyWaite(xa);
                CodyWaite cw = new CodyWaite(xa);
                quadrant = cw.GetK() & 3;
                xa = cw.GetRemA();
                xb = cw.GetRemB();
            }

            if (negative)
            {
                quadrant ^= 2; // Flip bit 1
            }

            switch (quadrant)
            {
                case 0:
                    return SinQ(xa, xb);
                case 1:
                    return CosQ(xa, xb);
                case 2:
                    return -SinQ(xa, xb);
                case 3:
                    return -CosQ(xa, xb);
                default:
                    return Double.NaN;
            }
        }

        /// <summary>
        /// Cosine function.
        /// </summary>
        /// <param name="x"> Argument. </param>
        /// <returns> cos(x) </returns>
        public static double Cos(double x)
        {
            int quadrant = 0;

            /* Take absolute value of the input */
            double xa = x;
            if (x < 0)
            {
                xa = -xa;
            }

            if (xa != xa || xa == double.PositiveInfinity)
            {
                return Double.NaN;
            }

            /* Perform any argument reduction */
            double xb = 0;
            if (xa > 3294198.0)
            {
                // PI * (2**20)
                // Argument too big for CodyWaite reduction.  Must use
                // PayneHanek.
                double[] reduceResults = new double[3];
                ReducePayneHanek(xa, reduceResults);
                quadrant = ((int) reduceResults[0]) & 3;
                xa = reduceResults[1];
                xb = reduceResults[2];
            }
            else if (xa > 1.5707963267948966)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CodyWaite cw = new CodyWaite(xa);
                CodyWaite cw = new CodyWaite(xa);
                quadrant = cw.GetK() & 3;
                xa = cw.GetRemA();
                xb = cw.GetRemB();
            }

            //if (negative)
            //  quadrant = (quadrant + 2) % 4;

            switch (quadrant)
            {
                case 0:
                    return CosQ(xa, xb);
                case 1:
                    return -SinQ(xa, xb);
                case 2:
                    return -CosQ(xa, xb);
                case 3:
                    return SinQ(xa, xb);
                default:
                    return Double.NaN;
            }
        }

        /// <summary>
        /// Tangent function.
        /// </summary>
        /// <param name="x"> Argument. </param>
        /// <returns> tan(x) </returns>
        public static double Tan(double x)
        {
            bool negative = false;
            int quadrant = 0;

            /* Take absolute value of the input */
            double xa = x;
            if (x < 0)
            {
                negative = true;
                xa = -xa;
            }

            /* Check for zero and negative zero */
            if (xa == 0.0)
            {
                long bits = BitConverter.DoubleToInt64Bits(x);
                if (bits < 0)
                {
                    return -0.0;
                }
                return 0.0;
            }

            if (xa != xa || xa == double.PositiveInfinity)
            {
                return Double.NaN;
            }

            /* Perform any argument reduction */
            double xb = 0;
            if (xa > 3294198.0)
            {
                // PI * (2**20)
                // Argument too big for CodyWaite reduction.  Must use
                // PayneHanek.
                double[] reduceResults = new double[3];
                ReducePayneHanek(xa, reduceResults);
                quadrant = ((int) reduceResults[0]) & 3;
                xa = reduceResults[1];
                xb = reduceResults[2];
            }
            else if (xa > 1.5707963267948966)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CodyWaite cw = new CodyWaite(xa);
                CodyWaite cw = new CodyWaite(xa);
                quadrant = cw.GetK() & 3;
                xa = cw.GetRemA();
                xb = cw.GetRemB();
            }

            if (xa > 1.5)
            {
                // Accuracy suffers between 1.5 and PI/2
                const double pi2a = 1.5707963267948966;
                const double pi2b = 6.123233995736766E-17;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a = pi2a - xa;
                double a = pi2a - xa;
                double b = -(a - pi2a + xa);
                b += pi2b - xb;

                xa = a + b;
                xb = -(xa - a - b);
                quadrant ^= 1;
                negative ^= true;
            }

            double result;
            if ((quadrant & 1) == 0)
            {
                result = TanQ(xa, xb, false);
            }
            else
            {
                result = -TanQ(xa, xb, true);
            }

            if (negative)
            {
                result = -result;
            }

            return result;
        }

        /// <summary>
        /// Arctangent function </summary>
        ///  <param name="x"> a number </param>
        ///  <returns> atan(x) </returns>
        public static double Atan(double x)
        {
            return Atan(x, 0.0, false);
        }

        /// <summary>
        /// Internal helper function to compute arctangent. </summary>
        /// <param name="xa"> number from which arctangent is requested </param>
        /// <param name="xb"> extra bits for x (may be 0.0) </param>
        /// <param name="leftPlane"> if true, result angle must be put in the left half plane </param>
        /// <returns> atan(xa + xb) (or angle shifted by {@code PI} if leftPlane is true) </returns>
        private static double Atan(double xa, double xb, bool leftPlane)
        {
            if (xa == 0.0)
            { // Matches +/- 0.0; return correct sign
                return leftPlane ? copySign(Math.PI, xa) : xa;
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final boolean negate;
            bool negate;
            if (xa < 0)
            {
                // negative
                xa = -xa;
                xb = -xb;
                negate = true;
            }
            else
            {
                negate = false;
            }

            if (xa > 1.633123935319537E16)
            { // Very large input
                return (negate ^ leftPlane) ? (-Math.PI * F_1_2) : (Math.PI * F_1_2);
            }

            /* Estimate the closest tabulated arctan value, compute eps = xa-tangentTable */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int idx;
            int idx;
            if (xa < 1)
            {
                idx = (int)(((-1.7168146928204136 * xa * xa + 8.0) * xa) + 0.5);
            }
            else
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double oneOverXa = 1 / xa;
                double oneOverXa = 1 / xa;
                idx = (int)(-((-1.7168146928204136 * oneOverXa * oneOverXa + 8.0) * oneOverXa) + 13.07);
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ttA = TANGENT_TABLE_A[idx];
            double ttA = TANGENT_TABLE_A[idx];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ttB = TANGENT_TABLE_B[idx];
            double ttB = TANGENT_TABLE_B[idx];

            double epsA = xa - ttA;
            double epsB = -(epsA - xa + ttA);
            epsB += xb - ttB;

            double temp = epsA + epsB;
            epsB = -(temp - epsA - epsB);
            epsA = temp;

            /* Compute eps = eps / (1.0 + xa*tangent) */
            temp = xa * HEX_40000000;
            double ya = xa + temp - temp;
            double yb = xb + xa - ya;
            xa = ya;
            xb += yb;

            //if (idx > 8 || idx == 0)
            if (idx == 0)
            {
                /* If the slope of the arctan is gentle enough (< 0.45), this approximation will suffice */
                //double denom = 1.0 / (1.0 + xa*tangentTableA[idx] + xb*tangentTableA[idx] + xa*tangentTableB[idx] + xb*tangentTableB[idx]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double denom = 1d / (1d + (xa + xb) * (ttA + ttB));
                double denom = 1d / (1d + (xa + xb) * (ttA + ttB));
                //double denom = 1.0 / (1.0 + xa*tangentTableA[idx]);
                ya = epsA * denom;
                yb = epsB * denom;
            }
            else
            {
                double temp2 = xa * ttA;
                double za = 1d + temp2;
                double zb = -(za - 1d - temp2);
                temp2 = xb * ttA + xa * ttB;
                temp = za + temp2;
                zb += -(temp - za - temp2);
                za = temp;

                zb += xb * ttB;
                ya = epsA / za;

                temp = ya * HEX_40000000;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double yaa = (ya + temp) - temp;
                double yaa = (ya + temp) - temp;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double yab = ya - yaa;
                double yab = ya - yaa;

                temp = za * HEX_40000000;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double zaa = (za + temp) - temp;
                double zaa = (za + temp) - temp;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double zab = za - zaa;
                double zab = za - zaa;

                /* Correct for rounding in division */
                yb = (epsA - yaa * zaa - yaa * zab - yab * zaa - yab * zab) / za;

                yb += -epsA * zb / za / za;
                yb += epsB / za;
            }


            epsA = ya;
            epsB = yb;

            /* Evaluate polynomial */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double epsA2 = epsA * epsA;
            double epsA2 = epsA * epsA;

            /*
        yb = -0.09001346640161823;
        yb = yb * epsA2 + 0.11110718400605211;
        yb = yb * epsA2 + -0.1428571349122913;
        yb = yb * epsA2 + 0.19999999999273194;
        yb = yb * epsA2 + -0.33333333333333093;
        yb = yb * epsA2 * epsA;
             */

            yb = 0.07490822288864472;
            yb = yb * epsA2 - 0.09088450866185192;
            yb = yb * epsA2 + 0.11111095942313305;
            yb = yb * epsA2 - 0.1428571423679182;
            yb = yb * epsA2 + 0.19999999999923582;
            yb = yb * epsA2 - 0.33333333333333287;
            yb = yb * epsA2 * epsA;


            ya = epsA;

            temp = ya + yb;
            yb = -(temp - ya - yb);
            ya = temp;

            /* Add in effect of epsB.   atan'(x) = 1/(1+x^2) */
            yb += epsB / (1d + epsA * epsA);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double eighths = EIGHTHS[idx];
            double eighths = EIGHTHS[idx];

            //result = yb + eighths[idx] + ya;
            double za = eighths + ya;
            double zb = -(za - eighths - ya);
            temp = za + yb;
            zb += -(temp - za - yb);
            za = temp;

            double result = za + zb;

            if (leftPlane)
            {
                // Result is in the left plane
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double resultb = -(result - za - zb);
                double resultb = -(result - za - zb);
                const double pia = 1.5707963267948966 * 2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double pib = 6.123233995736766E-17 * 2;
                double pib = 6.123233995736766E-17 * 2;

                za = pia - result;
                zb = -(za - pia + result);
                zb += pib - resultb;

                result = za + zb;
            }


            if (negate ^ leftPlane)
            {
                result = -result;
            }

            return result;
        }

        /// <summary>
        /// Two arguments arctangent function </summary>
        /// <param name="y"> ordinate </param>
        /// <param name="x"> abscissa </param>
        /// <returns> phase angle of point (x,y) between {@code -PI} and {@code PI} </returns>
        public static double Atan2(double y, double x)
        {
            if (x != x || y != y)
            {
                return Double.NaN;
            }

            if (y == 0)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double result = x * y;
                double result = x * y;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double invx = 1d / x;
                double invx = 1d / x;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double invy = 1d / y;
                double invy = 1d / y;

                if (invx == 0)
                { // X is infinite
                    if (x > 0)
                    {
                        return y; // return +/- 0.0
                    }
                    else
                    {
                        return copySign(Math.PI, y);
                    }
                }

                if (x < 0 || invx < 0)
                {
                    if (y < 0 || invy < 0)
                    {
                        return -Math.PI;
                    }
                    else
                    {
                        return Math.PI;
                    }
                }
                else
                {
                    return result;
                }
            }

            // y cannot now be zero

            if (y == double.PositiveInfinity)
            {
                if (x == double.PositiveInfinity)
                {
                    return Math.PI * F_1_4;
                }

                if (x == double.NegativeInfinity)
                {
                    return Math.PI * F_3_4;
                }

                return Math.PI * F_1_2;
            }

            if (y == double.NegativeInfinity)
            {
                if (x == double.PositiveInfinity)
                {
                    return -Math.PI * F_1_4;
                }

                if (x == double.NegativeInfinity)
                {
                    return -Math.PI * F_3_4;
                }

                return -Math.PI * F_1_2;
            }

            if (x == double.PositiveInfinity)
            {
                if (y > 0 || 1 / y > 0)
                {
                    return 0d;
                }

                if (y < 0 || 1 / y < 0)
                {
                    return -0d;
                }
            }

            if (x == double.NegativeInfinity)
            {
                if (y > 0.0 || 1 / y > 0.0)
                {
                    return Math.PI;
                }

                if (y < 0 || 1 / y < 0)
                {
                    return -Math.PI;
                }
            }

            // Neither y nor x can be infinite or NAN here

            if (x == 0)
            {
                if (y > 0 || 1 / y > 0)
                {
                    return Math.PI * F_1_2;
                }

                if (y < 0 || 1 / y < 0)
                {
                    return -Math.PI * F_1_2;
                }
            }

            // Compute ratio r = y/x
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double r = y / x;
            double r = y / x;
            if (double.IsInfinity(r))
            { // bypass calculations that can create NaN
                return Atan(r, 0, x < 0);
            }

            double ra = DoubleHighPart(r);
            double rb = r - ra;

            // Split x
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xa = doubleHighPart(x);
            double xa = DoubleHighPart(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xb = x - xa;
            double xb = x - xa;

            rb += (y - ra * xa - ra * xb - rb * xa - rb * xb) / x;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double temp = ra + rb;
            double temp = ra + rb;
            rb = -(temp - ra - rb);
            ra = temp;

            if (ra == 0)
            { // Fix up the sign so atan works correctly
                ra = CopySign(0d, y);
            }

            // Call atan
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double result = atan(ra, rb, x < 0);
            double result = Atan(ra, rb, x < 0);

            return result;
        }

        /// <summary>
        /// Compute the arc sine of a number. </summary>
        /// <param name="x"> number on which evaluation is done </param>
        /// <returns> arc sine of x </returns>
        public static double Asin(double x)
        {
          if (x != x)
          {
              return Double.NaN;
          }

          if (x > 1.0 || x < -1.0)
          {
              return Double.NaN;
          }

          if (x == 1.0)
          {
              return Math.PI / 2.0;
          }

          if (x == -1.0)
          {
              return -Math.PI / 2.0;
          }

          if (x == 0.0)
          { // Matches +/- 0.0; return correct sign
              return x;
          }

          /* Compute asin(x) = atan(x/sqrt(1-x*x)) */

          /* Split x */
          double temp = x * HEX_40000000;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xa = x + temp - temp;
          double xa = x + temp - temp;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xb = x - xa;
          double xb = x - xa;

          /* Square it */
          double ya = xa * xa;
          double yb = xa * xb * 2.0 + xb * xb;

          /* Subtract from 1 */
          ya = -ya;
          yb = -yb;

          double za = 1.0 + ya;
          double zb = -(za - 1.0 - ya);

          temp = za + yb;
          zb += -(temp - za - yb);
          za = temp;

          /* Square root */
          double y;
          y = Sqrt(za);
          temp = y * HEX_40000000;
          ya = y + temp - temp;
          yb = y - ya;

          /* Extend precision of sqrt */
          yb += (za - ya * ya - 2 * ya * yb - yb * yb) / (2.0 * y);

          /* Contribution of zb to sqrt */
          double dx = zb / (2.0 * y);

          // Compute ratio r = x/y
          double r = x / y;
          temp = r * HEX_40000000;
          double ra = r + temp - temp;
          double rb = r - ra;

          rb += (x - ra * ya - ra * yb - rb * ya - rb * yb) / y; // Correct for rounding in division
          rb += -x * dx / y / y; // Add in effect additional bits of sqrt.

          temp = ra + rb;
          rb = -(temp - ra - rb);
          ra = temp;

          return Atan(ra, rb, false);
        }

        /// <summary>
        /// Compute the arc cosine of a number. </summary>
        /// <param name="x"> number on which evaluation is done </param>
        /// <returns> arc cosine of x </returns>
        public static double Acos(double x)
        {
          if (x != x)
          {
              return Double.NaN;
          }

          if (x > 1.0 || x < -1.0)
          {
              return Double.NaN;
          }

          if (x == -1.0)
          {
              return Math.PI;
          }

          if (x == 1.0)
          {
              return 0.0;
          }

          if (x == 0)
          {
              return Math.PI / 2.0;
          }

          /* Compute acos(x) = atan(sqrt(1-x*x)/x) */

          /* Split x */
          double temp = x * HEX_40000000;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xa = x + temp - temp;
          double xa = x + temp - temp;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xb = x - xa;
          double xb = x - xa;

          /* Square it */
          double ya = xa * xa;
          double yb = xa * xb * 2.0 + xb * xb;

          /* Subtract from 1 */
          ya = -ya;
          yb = -yb;

          double za = 1.0 + ya;
          double zb = -(za - 1.0 - ya);

          temp = za + yb;
          zb += -(temp - za - yb);
          za = temp;

          /* Square root */
          double y = Sqrt(za);
          temp = y * HEX_40000000;
          ya = y + temp - temp;
          yb = y - ya;

          /* Extend precision of sqrt */
          yb += (za - ya * ya - 2 * ya * yb - yb * yb) / (2.0 * y);

          /* Contribution of zb to sqrt */
          yb += zb / (2.0 * y);
          y = ya + yb;
          yb = -(y - ya - yb);

          // Compute ratio r = y/x
          double r = y / x;

          // Did r overflow?
          if (double.IsInfinity(r))
          { // x is effectively zero
              return Math.PI / 2; // so return the appropriate value
          }

          double ra = DoubleHighPart(r);
          double rb = r - ra;

          rb += (y - ra * xa - ra * xb - rb * xa - rb * xb) / x; // Correct for rounding in division
          rb += yb / x; // Add in effect additional bits of sqrt.

          temp = ra + rb;
          rb = -(temp - ra - rb);
          ra = temp;

          return Atan(ra, rb, x < 0);
        }

        /// <summary>
        /// Compute the cubic root of a number. </summary>
        /// <param name="x"> number on which evaluation is done </param>
        /// <returns> cubic root of x </returns>
        public static double Cbrt(double x)
        {
          /* Convert input double to bits */
          long inbits = BitConverter.DoubleToInt64Bits(x);
          int exponent = (int)((inbits >> 52) & 0x7ff) - 1023;
          bool subnormal = false;

          if (exponent == -1023)
          {
              if (x == 0)
              {
                  return x;
              }

              /* Subnormal, so normalize */
              subnormal = true;
              x *= 1.8014398509481984E16; // 2^54
              inbits = BitConverter.DoubleToInt64Bits(x);
              exponent = (int)((inbits >> 52) & 0x7ff) - 1023;
          }

          if (exponent == 1024)
          {
              // Nan or infinity.  Don't care which.
              return x;
          }

          /* Divide the exponent by 3 */
          int exp3 = exponent / 3;

          /* p2 will be the nearest power of 2 to x with its exponent divided by 3 */
          double p2 = BitConverter.Int64BitsToDouble((inbits & 0x8000000000000000L) | (long)(((exp3 + 1023) & 0x7ff)) << 52);

          /* This will be a number between 1 and 2 */
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double mant = Double.longBitsToDouble((inbits & 0x000fffffffffffffL) | 0x3ff0000000000000L);
          double mant = BitConverter.Int64BitsToDouble((inbits & 0x000fffffffffffffL) | 0x3ff0000000000000L);

          /* Estimate the cube root of mant by polynomial */
          double est = -0.010714690733195933;
          est = est * mant + 0.0875862700108075;
          est = est * mant + -0.3058015757857271;
          est = est * mant + 0.7249995199969751;
          est = est * mant + 0.5039018405998233;

          est *= CBRTTWO[exponent % 3 + 2];

          // est should now be good to about 15 bits of precision.   Do 2 rounds of
          // Newton's method to get closer,  this should get us full double precision
          // Scale down x for the purpose of doing newtons method.  This avoids over/under flows.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xs = x / (p2*p2*p2);
          double xs = x / (p2 * p2 * p2);
          est += (xs - est * est * est) / (3 * est * est);
          est += (xs - est * est * est) / (3 * est * est);

          // Do one round of Newton's method in extended precision to get the last bit right.
          double temp = est * HEX_40000000;
          double ya = est + temp - temp;
          double yb = est - ya;

          double za = ya * ya;
          double zb = ya * yb * 2.0 + yb * yb;
          temp = za * HEX_40000000;
          double temp2 = za + temp - temp;
          zb += za - temp2;
          za = temp2;

          zb = za * yb + ya * zb + zb * yb;
          za *= ya;

          double na = xs - za;
          double nb = -(na - xs + za);
          nb -= zb;

          est += (na + nb) / (3 * est * est);

          /* Scale by a power of two, so this is exact. */
          est *= p2;

          if (subnormal)
          {
              est *= 3.814697265625E-6; // 2^-18
          }

          return est;
        }

        /// <summary>
        ///  Convert degrees to radians, with error of less than 0.5 ULP </summary>
        ///  <param name="x"> angle in degrees </param>
        ///  <returns> x converted into radians </returns>
        public static double ToRadians(double x)
        {
            if (double.IsInfinity(x) || x == 0.0)
            { // Matches +/- 0.0; return correct sign
                return x;
            }

            // These are PI/180 split into high and low order bits
            const double facta = 0.01745329052209854;
            const double factb = 1.997844754509471E-9;

            double xa = DoubleHighPart(x);
            double xb = x - xa;

            double result = xb * factb + xb * facta + xa * factb + xa * facta;
            if (result == 0)
            {
                result *= x; // ensure correct sign if calculation underflows
            }
            return result;
        }

        /// <summary>
        ///  Convert radians to degrees, with error of less than 0.5 ULP </summary>
        ///  <param name="x"> angle in radians </param>
        ///  <returns> x converted into degrees </returns>
        public static double ToDegrees(double x)
        {
            if (double.IsInfinity(x) || x == 0.0)
            { // Matches +/- 0.0; return correct sign
                return x;
            }

            // These are 180/PI split into high and low order bits
            const double facta = 57.2957763671875;
            const double factb = 3.145894820876798E-6;

            double xa = DoubleHighPart(x);
            double xb = x - xa;

            return xb * factb + xb * facta + xa * factb + xa * facta;
        }

        /// <summary>
        /// Absolute value. </summary>
        /// <param name="x"> number from which absolute value is requested </param>
        /// <returns> abs(x) </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int abs(final int x)
        public static int Abs(int x)
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int i = x >>> 31;
            int i = (int)((uint)x >> 31);
            return (x ^ (~i + 1)) + i;
        }

        /// <summary>
        /// Absolute value. </summary>
        /// <param name="x"> number from which absolute value is requested </param>
        /// <returns> abs(x) </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long abs(final long x)
        public static long Abs(long x)
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long l = x >>> 63;
            long l = (long)((ulong)x >> 63);
            // l is one if x negative zero else
            // ~l+1 is zero if x is positive, -1 if x is negative
            // x^(~l+1) is x is x is positive, ~x if x is negative
            // add around
            return (x ^ (~l + 1)) + l;
        }

        /// <summary>
        /// Absolute value. </summary>
        /// <param name="x"> number from which absolute value is requested </param>
        /// <returns> abs(x) </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static float abs(final float x)
        public static float Abs(float x)
        {
            return Float.intBitsToFloat(MASK_NON_SIGN_INT & Float.floatToRawIntBits(x));
        }

        /// <summary>
        /// Absolute value. </summary>
        /// <param name="x"> number from which absolute value is requested </param>
        /// <returns> abs(x) </returns>
        public static double Abs(double x)
        {
            return BitConverter.Int64BitsToDouble(MASK_NON_SIGN_LONG & BitConverter.DoubleToInt64Bits(x));
        }

        /// <summary>
        /// Compute least significant bit (Unit in Last Position) for a number. </summary>
        /// <param name="x"> number from which ulp is requested </param>
        /// <returns> ulp(x) </returns>
        public static double Ulp(double x)
        {
            if (double.IsInfinity(x))
            {
                return double.PositiveInfinity;
            }
            return Abs(x - BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(x) ^ 1));
        }

        /// <summary>
        /// Compute least significant bit (Unit in Last Position) for a number. </summary>
        /// <param name="x"> number from which ulp is requested </param>
        /// <returns> ulp(x) </returns>
        public static float Ulp(float x)
        {
            if (float.IsInfinity(x))
            {
                return float.PositiveInfinity;
            }
            return abs(x - Float.intBitsToFloat(Float.floatToIntBits(x) ^ 1));
        }

        /// <summary>
        /// Multiply a double number by a power of 2. </summary>
        /// <param name="d"> number to multiply </param>
        /// <param name="n"> power of 2 </param>
        /// <returns> d &times; 2<sup>n</sup> </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double scalb(final double d, final int n)
        public static double Scalb(double d, int n)
        {

            // first simple and fast handling when 2^n can be represented using normal numbers
            if ((n > -1023) && (n < 1024))
            {
                return d * BitConverter.Int64BitsToDouble(((long)(n + 1023)) << 52);
            }

            // handle special cases
            if (double.IsNaN(d) || double.IsInfinity(d) || (d == 0))
            {
                return d;
            }
            if (n < -2098)
            {
                return (d > 0) ? 0.0 : -0.0;
            }
            if (n > 2097)
            {
                return (d > 0) ? double.PositiveInfinity : double.NegativeInfinity;
            }

            // decompose d
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long bits = Double.doubleToRawLongBits(d);
            long bits = BitConverter.DoubleToInt64Bits(d);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long sign = bits & 0x8000000000000000L;
            long sign = bits & unchecked((long)0x8000000000000000L);
            int exponent = ((int)((long)((ulong)bits >> 52))) & 0x7ff;
            long mantissa = bits & 0x000fffffffffffffL;

            // compute scaled exponent
            int scaledExponent = exponent + n;

            if (n < 0)
            {
                // we are really in the case n <= -1023
                if (scaledExponent > 0)
                {
                    // both the input and the result are normal numbers, we only adjust the exponent
                    return BitConverter.Int64BitsToDouble(sign | (((long) scaledExponent) << 52) | mantissa);
                }
                else if (scaledExponent > -53)
                {
                    // the input is a normal number and the result is a subnormal number

                    // recover the hidden mantissa bit
                    mantissa |= 1L << 52;

                    // scales down complete mantissa, hence losing least significant bits
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long mostSignificantLostBit = mantissa & (1L << (-scaledExponent));
                    long mostSignificantLostBit = mantissa & (1L << (-scaledExponent));
                    mantissa = (long)((ulong)mantissa >> 1 - scaledExponent);
                    if (mostSignificantLostBit != 0)
                    {
                        // we need to add 1 bit to round up the result
                        mantissa++;
                    }
                    return BitConverter.Int64BitsToDouble(sign | mantissa);

                }
                else
                {
                    // no need to compute the mantissa, the number scales down to 0
                    return (sign == 0L) ? 0.0 : -0.0;
                }
            }
            else
            {
                // we are really in the case n >= 1024
                if (exponent == 0)
                {

                    // the input number is subnormal, normalize it
                    while (((long)((ulong)mantissa >> 52)) != 1)
                    {
                        mantissa <<= 1;
                        --scaledExponent;
                    }
                    ++scaledExponent;
                    mantissa &= 0x000fffffffffffffL;

                    if (scaledExponent < 2047)
                    {
                        return BitConverter.Int64BitsToDouble(sign | (((long) scaledExponent) << 52) | mantissa);
                    }
                    else
                    {
                        return (sign == 0L) ? double.PositiveInfinity : double.NegativeInfinity;
                    }

                }
                else if (scaledExponent < 2047)
                {
                    return BitConverter.Int64BitsToDouble(sign | (((long) scaledExponent) << 52) | mantissa);
                }
                else
                {
                    return (sign == 0L) ? double.PositiveInfinity : double.NegativeInfinity;
                }
            }

        }

        /// <summary>
        /// Multiply a float number by a power of 2. </summary>
        /// <param name="f"> number to multiply </param>
        /// <param name="n"> power of 2 </param>
        /// <returns> f &times; 2<sup>n</sup> </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static float scalb(final float f, final int n)
        public static float Scalb(float f, int n)
        {

            // first simple and fast handling when 2^n can be represented using normal numbers
            if ((n > -127) && (n < 128))
            {
                return f * Float.intBitsToFloat((n + 127) << 23);
            }

            // handle special cases
            if (float.IsNaN(f) || float.IsInfinity(f) || (f == 0f))
            {
                return f;
            }
            if (n < -277)
            {
                return (f > 0) ? 0.0f : -0.0f;
            }
            if (n > 276)
            {
                return (f > 0) ? float.PositiveInfinity : float.NegativeInfinity;
            }

            // decompose f
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int bits = Float.floatToIntBits(f);
            int bits = Float.floatToIntBits(f);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int sign = bits & 0x80000000;
            int sign = bits & unchecked((int)0x80000000);
            int exponent = ((int)((uint)bits >> 23)) & 0xff;
            int mantissa = bits & 0x007fffff;

            // compute scaled exponent
            int scaledExponent = exponent + n;

            if (n < 0)
            {
                // we are really in the case n <= -127
                if (scaledExponent > 0)
                {
                    // both the input and the result are normal numbers, we only adjust the exponent
                    return Float.intBitsToFloat(sign | (scaledExponent << 23) | mantissa);
                }
                else if (scaledExponent > -24)
                {
                    // the input is a normal number and the result is a subnormal number

                    // recover the hidden mantissa bit
                    mantissa |= 1 << 23;

                    // scales down complete mantissa, hence losing least significant bits
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int mostSignificantLostBit = mantissa & (1 << (-scaledExponent));
                    int mostSignificantLostBit = mantissa & (1 << (-scaledExponent));
                    mantissa = (int)((uint)mantissa >> 1 - scaledExponent);
                    if (mostSignificantLostBit != 0)
                    {
                        // we need to add 1 bit to round up the result
                        mantissa++;
                    }
                    return Float.intBitsToFloat(sign | mantissa);

                }
                else
                {
                    // no need to compute the mantissa, the number scales down to 0
                    return (sign == 0) ? 0.0f : -0.0f;
                }
            }
            else
            {
                // we are really in the case n >= 128
                if (exponent == 0)
                {

                    // the input number is subnormal, normalize it
                    while (((int)((uint)mantissa >> 23)) != 1)
                    {
                        mantissa <<= 1;
                        --scaledExponent;
                    }
                    ++scaledExponent;
                    mantissa &= 0x007fffff;

                    if (scaledExponent < 255)
                    {
                        return Float.intBitsToFloat(sign | (scaledExponent << 23) | mantissa);
                    }
                    else
                    {
                        return (sign == 0) ? float.PositiveInfinity : float.NegativeInfinity;
                    }

                }
                else if (scaledExponent < 255)
                {
                    return Float.intBitsToFloat(sign | (scaledExponent << 23) | mantissa);
                }
                else
                {
                    return (sign == 0) ? float.PositiveInfinity : float.NegativeInfinity;
                }
            }

        }

        /// <summary>
        /// Get the next machine representable number after a number, moving
        /// in the direction of another number.
        /// <para>
        /// The ordering is as follows (increasing):
        /// <ul>
        /// <li>-INFINITY</li>
        /// <li>-MAX_VALUE</li>
        /// <li>-MIN_VALUE</li>
        /// <li>-0.0</li>
        /// <li>+0.0</li>
        /// <li>+MIN_VALUE</li>
        /// <li>+MAX_VALUE</li>
        /// <li>+INFINITY</li>
        /// <li></li>
        /// </para>
        /// <para>
        /// If arguments compare equal, then the second argument is returned.
        /// </para>
        /// <para>
        /// If {@code direction} is greater than {@code d},
        /// the smallest machine representable number strictly greater than
        /// {@code d} is returned; if less, then the largest representable number
        /// strictly less than {@code d} is returned.</para>
        /// <para>
        /// If {@code d} is infinite and direction does not
        /// bring it back to finite numbers, it is returned unchanged.</para>
        /// </summary>
        /// <param name="d"> base number </param>
        /// <param name="direction"> (the only important thing is whether
        /// {@code direction} is greater or smaller than {@code d}) </param>
        /// <returns> the next machine representable number in the specified direction </returns>
        public static double NextAfter(double d, double direction)
        {

            // handling of some important special cases
            if (double.IsNaN(d) || double.IsNaN(direction))
            {
                return Double.NaN;
            }
            else if (d == direction)
            {
                return direction;
            }
            else if (double.IsInfinity(d))
            {
                return (d < 0) ? -double.MaxValue : double.MaxValue;
            }
            else if (d == 0)
            {
                return (direction < 0) ? -double.Epsilon : double.Epsilon;
            }
            // special cases MAX_VALUE to infinity and  MIN_VALUE to 0
            // are handled just as normal numbers
            // can use raw bits since already dealt with infinity and NaN
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long bits = Double.doubleToRawLongBits(d);
            long bits = BitConverter.DoubleToInt64Bits(d);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long sign = bits & 0x8000000000000000L;
            long sign = bits & unchecked((long)0x8000000000000000L);
            if ((direction < d) ^ (sign == 0L))
            {
                return BitConverter.Int64BitsToDouble(sign | ((bits & 0x7fffffffffffffffL) + 1));
            }
            else
            {
                return BitConverter.Int64BitsToDouble(sign | ((bits & 0x7fffffffffffffffL) - 1));
            }

        }

        /// <summary>
        /// Get the next machine representable number after a number, moving
        /// in the direction of another number.
        /// <para>
        /// The ordering is as follows (increasing):
        /// <ul>
        /// <li>-INFINITY</li>
        /// <li>-MAX_VALUE</li>
        /// <li>-MIN_VALUE</li>
        /// <li>-0.0</li>
        /// <li>+0.0</li>
        /// <li>+MIN_VALUE</li>
        /// <li>+MAX_VALUE</li>
        /// <li>+INFINITY</li>
        /// <li></li>
        /// </para>
        /// <para>
        /// If arguments compare equal, then the second argument is returned.
        /// </para>
        /// <para>
        /// If {@code direction} is greater than {@code f},
        /// the smallest machine representable number strictly greater than
        /// {@code f} is returned; if less, then the largest representable number
        /// strictly less than {@code f} is returned.</para>
        /// <para>
        /// If {@code f} is infinite and direction does not
        /// bring it back to finite numbers, it is returned unchanged.</para>
        /// </summary>
        /// <param name="f"> base number </param>
        /// <param name="direction"> (the only important thing is whether
        /// {@code direction} is greater or smaller than {@code f}) </param>
        /// <returns> the next machine representable number in the specified direction </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static float nextAfter(final float f, final double direction)
        public static float NextAfter(float f, double direction)
        {

            // handling of some important special cases
            if (double.IsNaN(f) || double.IsNaN(direction))
            {
                return Float.NaN;
            }
            else if (f == direction)
            {
                return (float) direction;
            }
            else if (float.IsInfinity(f))
            {
                return (f < 0f) ? -float.MaxValue : float.MaxValue;
            }
            else if (f == 0f)
            {
                return (direction < 0) ? -float.Epsilon : float.Epsilon;
            }
            // special cases MAX_VALUE to infinity and  MIN_VALUE to 0
            // are handled just as normal numbers

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int bits = Float.floatToIntBits(f);
            int bits = Float.floatToIntBits(f);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int sign = bits & 0x80000000;
            int sign = bits & unchecked((int)0x80000000);
            if ((direction < f) ^ (sign == 0))
            {
                return Float.intBitsToFloat(sign | ((bits & 0x7fffffff) + 1));
            }
            else
            {
                return Float.intBitsToFloat(sign | ((bits & 0x7fffffff) - 1));
            }

        }

        /// <summary>
        /// Get the largest whole number smaller than x. </summary>
        /// <param name="x"> number from which floor is requested </param>
        /// <returns> a double number f such that f is an integer f <= x < f + 1.0 </returns>
        public static double Floor(double x)
        {
            long y;

            if (x != x)
            { // NaN
                return x;
            }

            if (x >= TWO_POWER_52 || x <= -TWO_POWER_52)
            {
                return x;
            }

            y = (long) x;
            if (x < 0 && y != x)
            {
                y--;
            }

            if (y == 0)
            {
                return x * y;
            }

            return y;
        }

        /// <summary>
        /// Get the smallest whole number larger than x. </summary>
        /// <param name="x"> number from which ceil is requested </param>
        /// <returns> a double number c such that c is an integer c - 1.0 < x <= c </returns>
        public static double Ceil(double x)
        {
            double y;

            if (x != x)
            { // NaN
                return x;
            }

            y = Floor(x);
            if (y == x)
            {
                return y;
            }

            y += 1.0;

            if (y == 0)
            {
                return x * y;
            }

            return y;
        }

        /// <summary>
        /// Get the whole number that is the nearest to x, or the even one if x is exactly half way between two integers. </summary>
        /// <param name="x"> number from which nearest whole number is requested </param>
        /// <returns> a double number r such that r is an integer r - 0.5 <= x <= r + 0.5 </returns>
        public static double Rint(double x)
        {
            double y = Floor(x);
            double d = x - y;

            if (d > 0.5)
            {
                if (y == -1.0)
                {
                    return -0.0; // Preserve sign of operand
                }
                return y + 1.0;
            }
            if (d < 0.5)
            {
                return y;
            }

            /* half way, round to even */
            long z = (long) y;
            return (z & 1) == 0 ? y : y + 1.0;
        }

        /// <summary>
        /// Get the closest long to x. </summary>
        /// <param name="x"> number from which closest long is requested </param>
        /// <returns> closest long to x </returns>
        public static long Round(double x)
        {
            return (long) Floor(x + 0.5);
        }

        /// <summary>
        /// Get the closest int to x. </summary>
        /// <param name="x"> number from which closest int is requested </param>
        /// <returns> closest int to x </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int round(final float x)
        public static int Round(float x)
        {
            return (int) Floor(x + 0.5f);
        }

        /// <summary>
        /// Compute the minimum of two values </summary>
        /// <param name="a"> first value </param>
        /// <param name="b"> second value </param>
        /// <returns> a if a is lesser or equal to b, b otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int min(final int a, final int b)
        public static int Min(int a, int b)
        {
            return (a <= b) ? a : b;
        }

        /// <summary>
        /// Compute the minimum of two values </summary>
        /// <param name="a"> first value </param>
        /// <param name="b"> second value </param>
        /// <returns> a if a is lesser or equal to b, b otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long min(final long a, final long b)
        public static long Min(long a, long b)
        {
            return (a <= b) ? a : b;
        }

        /// <summary>
        /// Compute the minimum of two values </summary>
        /// <param name="a"> first value </param>
        /// <param name="b"> second value </param>
        /// <returns> a if a is lesser or equal to b, b otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static float min(final float a, final float b)
        public static float Min(float a, float b)
        {
            if (a > b)
            {
                return b;
            }
            if (a < b)
            {
                return a;
            }
            /* if either arg is NaN, return NaN */
            if (a != b)
            {
                return Float.NaN;
            }
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x80000000 == Float.floatToRawIntBits(-0.0d) */
            int bits = Float.floatToRawIntBits(a);
            if (bits == 0x80000000)
            {
                return a;
            }
            return b;
        }

        /// <summary>
        /// Compute the minimum of two values </summary>
        /// <param name="a"> first value </param>
        /// <param name="b"> second value </param>
        /// <returns> a if a is lesser or equal to b, b otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double min(final double a, final double b)
        public static double Min(double a, double b)
        {
            if (a > b)
            {
                return b;
            }
            if (a < b)
            {
                return a;
            }
            /* if either arg is NaN, return NaN */
            if (a != b)
            {
                return Double.NaN;
            }
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x8000000000000000L == Double.doubleToRawLongBits(-0.0d) */
            long bits = BitConverter.DoubleToInt64Bits(a);
            if (bits == unchecked((long)0x8000000000000000L))
            {
                return a;
            }
            return b;
        }

        /// <summary>
        /// Compute the maximum of two values </summary>
        /// <param name="a"> first value </param>
        /// <param name="b"> second value </param>
        /// <returns> b if a is lesser or equal to b, a otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int max(final int a, final int b)
        public static int Max(int a, int b)
        {
            return (a <= b) ? b : a;
        }

        /// <summary>
        /// Compute the maximum of two values </summary>
        /// <param name="a"> first value </param>
        /// <param name="b"> second value </param>
        /// <returns> b if a is lesser or equal to b, a otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long max(final long a, final long b)
        public static long Max(long a, long b)
        {
            return (a <= b) ? b : a;
        }

        /// <summary>
        /// Compute the maximum of two values </summary>
        /// <param name="a"> first value </param>
        /// <param name="b"> second value </param>
        /// <returns> b if a is lesser or equal to b, a otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static float max(final float a, final float b)
        public static float Max(float a, float b)
        {
            if (a > b)
            {
                return a;
            }
            if (a < b)
            {
                return b;
            }
            /* if either arg is NaN, return NaN */
            if (a != b)
            {
                return Float.NaN;
            }
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x80000000 == Float.floatToRawIntBits(-0.0d) */
            int bits = Float.floatToRawIntBits(a);
            if (bits == 0x80000000)
            {
                return b;
            }
            return a;
        }

        /// <summary>
        /// Compute the maximum of two values </summary>
        /// <param name="a"> first value </param>
        /// <param name="b"> second value </param>
        /// <returns> b if a is lesser or equal to b, a otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double max(final double a, final double b)
        public static double Max(double a, double b)
        {
            if (a > b)
            {
                return a;
            }
            if (a < b)
            {
                return b;
            }
            /* if either arg is NaN, return NaN */
            if (a != b)
            {
                return Double.NaN;
            }
            /* min(+0.0,-0.0) == -0.0 */
            /* 0x8000000000000000L == Double.doubleToRawLongBits(-0.0d) */
            long bits = BitConverter.DoubleToInt64Bits(a);
            if (bits == unchecked((long)0x8000000000000000L))
            {
                return b;
            }
            return a;
        }

        /// <summary>
        /// Returns the hypotenuse of a triangle with sides {@code x} and {@code y}
        /// - sqrt(<i>x</i><sup>2</sup>&nbsp;+<i>y</i><sup>2</sup>)<br/>
        /// avoiding intermediate overflow or underflow.
        /// 
        /// <ul>
        /// <li> If either argument is infinite, then the result is positive infinity.</li>
        /// <li> else, if either argument is NaN then the result is NaN.</li>
        /// </ul>
        /// </summary>
        /// <param name="x"> a value </param>
        /// <param name="y"> a value </param>
        /// <returns> sqrt(<i>x</i><sup>2</sup>&nbsp;+<i>y</i><sup>2</sup>) </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double hypot(final double x, final double y)
        public static double Hypot(double x, double y)
        {
            if (double.IsInfinity(x) || double.IsInfinity(y))
            {
                return double.PositiveInfinity;
            }
            else if (double.IsNaN(x) || double.IsNaN(y))
            {
                return Double.NaN;
            }
            else
            {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int expX = getExponent(x);
                int expX = GetExponent(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int expY = getExponent(y);
                int expY = GetExponent(y);
                if (expX > expY + 27)
                {
                    // y is neglectible with respect to x
                    return Abs(x);
                }
                else if (expY > expX + 27)
                {
                    // x is neglectible with respect to y
                    return Abs(y);
                }
                else
                {

                    // find an intermediate scale to avoid both overflow and underflow
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int middleExp = (expX + expY) / 2;
                    int middleExp = (expX + expY) / 2;

                    // scale parameters without losing precision
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double scaledX = scalb(x, -middleExp);
                    double scaledX = Scalb(x, -middleExp);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double scaledY = scalb(y, -middleExp);
                    double scaledY = Scalb(y, -middleExp);

                    // compute scaled hypotenuse
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double scaledH = sqrt(scaledX * scaledX + scaledY * scaledY);
                    double scaledH = Sqrt(scaledX * scaledX + scaledY * scaledY);

                    // remove scaling
                    return Scalb(scaledH, middleExp);

                }

            }
        }

        /// <summary>
        /// Computes the remainder as prescribed by the IEEE 754 standard.
        /// The remainder value is mathematically equal to {@code x - y*n}
        /// where {@code n} is the mathematical integer closest to the exact mathematical value
        /// of the quotient {@code x/y}.
        /// If two mathematical integers are equally close to {@code x/y} then
        /// {@code n} is the integer that is even.
        /// <para>
        /// <ul>
        /// <li>If either operand is NaN, the result is NaN.</li>
        /// <li>If the result is not NaN, the sign of the result equals the sign of the dividend.</li>
        /// <li>If the dividend is an infinity, or the divisor is a zero, or both, the result is NaN.</li>
        /// <li>If the dividend is finite and the divisor is an infinity, the result equals the dividend.</li>
        /// <li>If the dividend is a zero and the divisor is finite, the result equals the dividend.</li>
        /// </ul>
        /// </para>
        /// <para><b>Note:</b> this implementation currently delegates to <seealso cref="StrictMath#IEEEremainder"/>
        /// </para>
        /// </summary>
        /// <param name="dividend"> the number to be divided </param>
        /// <param name="divisor"> the number by which to divide </param>
        /// <returns> the remainder, rounded </returns>
        public static double IEEEremainder(double dividend, double divisor)
        {
            return Math.IEEERemainder(dividend, divisor); // TODO provide our own implementation
        }

        /// <summary>
        /// Convert a long to interger, detecting overflows </summary>
        /// <param name="n"> number to convert to int </param>
        /// <returns> integer with same valie as n if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if n cannot fit into an int
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int toIntExact(final long n) throws org.apache.commons.math3.exception.MathArithmeticException
        public static int ToIntExact(long n)
        {
            if (n < int.MinValue || n > int.MaxValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW);
            }
            return (int) n;
        }

        /// <summary>
        /// Increment a number, detecting overflows. </summary>
        /// <param name="n"> number to increment </param>
        /// <returns> n+1 if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int incrementExact(final int n) throws org.apache.commons.math3.exception.MathArithmeticException
        public static int IncrementExact(int n)
        {

            if (n == int.MaxValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_ADDITION, n, 1);
            }

            return n + 1;

        }

        /// <summary>
        /// Increment a number, detecting overflows. </summary>
        /// <param name="n"> number to increment </param>
        /// <returns> n+1 if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long incrementExact(final long n) throws org.apache.commons.math3.exception.MathArithmeticException
        public static long IncrementExact(long n)
        {

            if (n == long.MaxValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_ADDITION, n, 1);
            }

            return n + 1;

        }

        /// <summary>
        /// Decrement a number, detecting overflows. </summary>
        /// <param name="n"> number to decrement </param>
        /// <returns> n-1 if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int decrementExact(final int n) throws org.apache.commons.math3.exception.MathArithmeticException
        public static int DecrementExact(int n)
        {

            if (n == int.MinValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_SUBTRACTION, n, 1);
            }

            return n - 1;

        }

        /// <summary>
        /// Decrement a number, detecting overflows. </summary>
        /// <param name="n"> number to decrement </param>
        /// <returns> n-1 if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long decrementExact(final long n) throws org.apache.commons.math3.exception.MathArithmeticException
        public static long DecrementExact(long n)
        {

            if (n == long.MinValue)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_SUBTRACTION, n, 1);
            }

            return n - 1;

        }

        /// <summary>
        /// Add two numbers, detecting overflows. </summary>
        /// <param name="a"> first number to add </param>
        /// <param name="b"> second number to add </param>
        /// <returns> a+b if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int addExact(final int a, final int b) throws org.apache.commons.math3.exception.MathArithmeticException
        public static int AddExact(int a, int b)
        {

            // compute sum
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int sum = a + b;
            int sum = a + b;

            // check for overflow
            if ((a ^ b) >= 0 && (sum ^ b) < 0)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_ADDITION, a, b);
            }

            return sum;

        }

        /// <summary>
        /// Add two numbers, detecting overflows. </summary>
        /// <param name="a"> first number to add </param>
        /// <param name="b"> second number to add </param>
        /// <returns> a+b if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long addExact(final long a, final long b) throws org.apache.commons.math3.exception.MathArithmeticException
        public static long AddExact(long a, long b)
        {

            // compute sum
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long sum = a + b;
            long sum = a + b;

            // check for overflow
            if ((a ^ b) >= 0 && (sum ^ b) < 0)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_ADDITION, a, b);
            }

            return sum;

        }

        /// <summary>
        /// Subtract two numbers, detecting overflows. </summary>
        /// <param name="a"> first number </param>
        /// <param name="b"> second number to subtract from a </param>
        /// <returns> a-b if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int subtractExact(final int a, final int b)
        public static int SubtractExact(int a, int b)
        {

            // compute subtraction
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int sub = a - b;
            int sub = a - b;

            // check for overflow
            if ((a ^ b) < 0 && (sub ^ b) >= 0)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_SUBTRACTION, a, b);
            }

            return sub;

        }

        /// <summary>
        /// Subtract two numbers, detecting overflows. </summary>
        /// <param name="a"> first number </param>
        /// <param name="b"> second number to subtract from a </param>
        /// <returns> a-b if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long subtractExact(final long a, final long b)
        public static long SubtractExact(long a, long b)
        {

            // compute subtraction
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long sub = a - b;
            long sub = a - b;

            // check for overflow
            if ((a ^ b) < 0 && (sub ^ b) >= 0)
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_SUBTRACTION, a, b);
            }

            return sub;

        }

        /// <summary>
        /// Multiply two numbers, detecting overflows. </summary>
        /// <param name="a"> first number to multiply </param>
        /// <param name="b"> second number to multiply </param>
        /// <returns> a*b if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int multiplyExact(final int a, final int b)
        public static int MultiplyExact(int a, int b)
        {
            if (((b > 0) && (a > int.MaxValue / b || a < int.MinValue / b)) || ((b < -1) && (a > int.MinValue / b || a < int.MaxValue / b)) || ((b == -1) && (a == int.MinValue)))
            {
                throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_MULTIPLICATION, a, b);
            }
            return a * b;
        }

        /// <summary>
        /// Multiply two numbers, detecting overflows. </summary>
        /// <param name="a"> first number to multiply </param>
        /// <param name="b"> second number to multiply </param>
        /// <returns> a*b if no overflows occur </returns>
        /// <exception cref="MathArithmeticException"> if an overflow occurs
        /// @since 3.4 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long multiplyExact(final long a, final long b)
        public static long MultiplyExact(long a, long b)
        {
            if (((b > 0l) && (a > long.MaxValue / b || a < long.MinValue / b)) || ((b < -1l) && (a > long.MinValue / b || a < long.MaxValue / b)) || ((b == -1l) && (a == long.MinValue)))
            {
                    throw new MathArithmeticException(LocalizedFormats.OVERFLOW_IN_MULTIPLICATION, a, b);
            }
                return a * b;
        }

        /// <summary>
        /// Finds q such that a = q b + r with 0 <= r < b if b > 0 and b < r <= 0 if b < 0.
        /// <para>
        /// This methods returns the same value as integer division when
        /// a and b are same signs, but returns a different value when
        /// they are opposite (i.e. q is negative).
        /// </para> </summary>
        /// <param name="a"> dividend </param>
        /// <param name="b"> divisor </param>
        /// <returns> q such that a = q b + r with 0 <= r < b if b > 0 and b < r <= 0 if b < 0 </returns>
        /// <exception cref="MathArithmeticException"> if b == 0 </exception>
        /// <seealso cref= #floorMod(int, int)
        /// @since 3.4 </seealso>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int floorDiv(final int a, final int b) throws org.apache.commons.math3.exception.MathArithmeticException
        public static int FloorDiv(int a, int b)
        {

            if (b == 0)
            {
                throw new MathArithmeticException(LocalizedFormats.ZERO_DENOMINATOR);
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int m = a % b;
            int m = a % b;
            if ((a ^ b) >= 0 || m == 0)
            {
                // a an b have same sign, or division is exact
                return a / b;
            }
            else
            {
                // a and b have opposite signs and division is not exact
                return (a / b) - 1;
            }

        }

        /// <summary>
        /// Finds q such that a = q b + r with 0 <= r < b if b > 0 and b < r <= 0 if b < 0.
        /// <para>
        /// This methods returns the same value as integer division when
        /// a and b are same signs, but returns a different value when
        /// they are opposite (i.e. q is negative).
        /// </para> </summary>
        /// <param name="a"> dividend </param>
        /// <param name="b"> divisor </param>
        /// <returns> q such that a = q b + r with 0 <= r < b if b > 0 and b < r <= 0 if b < 0 </returns>
        /// <exception cref="MathArithmeticException"> if b == 0 </exception>
        /// <seealso cref= #floorMod(long, long)
        /// @since 3.4 </seealso>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long floorDiv(final long a, final long b) throws org.apache.commons.math3.exception.MathArithmeticException
        public static long FloorDiv(long a, long b)
        {

            if (b == 0l)
            {
                throw new MathArithmeticException(LocalizedFormats.ZERO_DENOMINATOR);
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long m = a % b;
            long m = a % b;
            if ((a ^ b) >= 0l || m == 0l)
            {
                // a an b have same sign, or division is exact
                return a / b;
            }
            else
            {
                // a and b have opposite signs and division is not exact
                return (a / b) - 1l;
            }

        }

        /// <summary>
        /// Finds r such that a = q b + r with 0 <= r < b if b > 0 and b < r <= 0 if b < 0.
        /// <para>
        /// This methods returns the same value as integer modulo when
        /// a and b are same signs, but returns a different value when
        /// they are opposite (i.e. q is negative).
        /// </para> </summary>
        /// <param name="a"> dividend </param>
        /// <param name="b"> divisor </param>
        /// <returns> r such that a = q b + r with 0 <= r < b if b > 0 and b < r <= 0 if b < 0 </returns>
        /// <exception cref="MathArithmeticException"> if b == 0 </exception>
        /// <seealso cref= #floorDiv(int, int)
        /// @since 3.4 </seealso>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int floorMod(final int a, final int b) throws org.apache.commons.math3.exception.MathArithmeticException
        public static int FloorMod(int a, int b)
        {

            if (b == 0)
            {
                throw new MathArithmeticException(LocalizedFormats.ZERO_DENOMINATOR);
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int m = a % b;
            int m = a % b;
            if ((a ^ b) >= 0 || m == 0)
            {
                // a an b have same sign, or division is exact
                return m;
            }
            else
            {
                // a and b have opposite signs and division is not exact
                return b + m;
            }

        }

        /// <summary>
        /// Finds r such that a = q b + r with 0 <= r < b if b > 0 and b < r <= 0 if b < 0.
        /// <para>
        /// This methods returns the same value as integer modulo when
        /// a and b are same signs, but returns a different value when
        /// they are opposite (i.e. q is negative).
        /// </para> </summary>
        /// <param name="a"> dividend </param>
        /// <param name="b"> divisor </param>
        /// <returns> r such that a = q b + r with 0 <= r < b if b > 0 and b < r <= 0 if b < 0 </returns>
        /// <exception cref="MathArithmeticException"> if b == 0 </exception>
        /// <seealso cref= #floorDiv(long, long)
        /// @since 3.4 </seealso>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static long floorMod(final long a, final long b)
        public static long FloorMod(long a, long b)
        {

            if (b == 0l)
            {
                throw new MathArithmeticException(LocalizedFormats.ZERO_DENOMINATOR);
            }

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long m = a % b;
            long m = a % b;
            if ((a ^ b) >= 0l || m == 0l)
            {
                // a an b have same sign, or division is exact
                return m;
            }
            else
            {
                // a and b have opposite signs and division is not exact
                return b + m;
            }

        }

        /// <summary>
        /// Returns the first argument with the sign of the second argument.
        /// A NaN {@code sign} argument is treated as positive.
        /// </summary>
        /// <param name="magnitude"> the value to return </param>
        /// <param name="sign"> the sign for the returned value </param>
        /// <returns> the magnitude with the same sign as the {@code sign} argument </returns>
        public static double CopySign(double magnitude, double sign)
        {
            // The highest order bit is going to be zero if the
            // highest order bit of m and s is the same and one otherwise.
            // So (m^s) will be positive if both m and s have the same sign
            // and negative otherwise.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long m = Double.doubleToRawLongBits(magnitude);
            long m = BitConverter.DoubleToInt64Bits(magnitude); // don't care about NaN
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long s = Double.doubleToRawLongBits(sign);
            long s = BitConverter.DoubleToInt64Bits(sign);
            if ((m ^ s) >= 0)
            {
                return magnitude;
            }
            return -magnitude; // flip sign
        }

        /// <summary>
        /// Returns the first argument with the sign of the second argument.
        /// A NaN {@code sign} argument is treated as positive.
        /// </summary>
        /// <param name="magnitude"> the value to return </param>
        /// <param name="sign"> the sign for the returned value </param>
        /// <returns> the magnitude with the same sign as the {@code sign} argument </returns>
        public static float CopySign(float magnitude, float sign)
        {
            // The highest order bit is going to be zero if the
            // highest order bit of m and s is the same and one otherwise.
            // So (m^s) will be positive if both m and s have the same sign
            // and negative otherwise.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int m = Float.floatToRawIntBits(magnitude);
            int m = Float.floatToRawIntBits(magnitude);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int s = Float.floatToRawIntBits(sign);
            int s = Float.floatToRawIntBits(sign);
            if ((m ^ s) >= 0)
            {
                return magnitude;
            }
            return -magnitude; // flip sign
        }

        /// <summary>
        /// Return the exponent of a double number, removing the bias.
        /// <para>
        /// For double numbers of the form 2<sup>x</sup>, the unbiased
        /// exponent is exactly x.
        /// </para> </summary>
        /// <param name="d"> number from which exponent is requested </param>
        /// <returns> exponent for d in IEEE754 representation, without bias </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int getExponent(final double d)
        public static int GetExponent(double d)
        {
            // NaN and Infinite will return 1024 anywho so can use raw bits
            return (int)(((int)((uint)BitConverter.DoubleToInt64Bits(d) >> 52)) & 0x7ff) - 1023;
        }

        /// <summary>
        /// Return the exponent of a float number, removing the bias.
        /// <para>
        /// For float numbers of the form 2<sup>x</sup>, the unbiased
        /// exponent is exactly x.
        /// </para> </summary>
        /// <param name="f"> number from which exponent is requested </param>
        /// <returns> exponent for d in IEEE754 representation, without bias </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static int getExponent(final float f)
        public static int GetExponent(float f)
        {
            // NaN and Infinite will return the same exponent anywho so can use raw bits
            return (((int)((uint)Float.floatToRawIntBits(f) >> 23)) & 0xff) - 127;
        }

        /// <summary>
        /// Print out contents of arrays, and check the length.
        /// <para>used to generate the preset arrays originally.</para> </summary>
        /// <param name="a"> unused </param>
        /*public static void Main(string[] a)
        {
            PrintStream @out = System.out;
            FastMathCalc.Printarray(@out, "EXP_INT_TABLE_A", EXP_INT_TABLE_LEN, ExpIntTable.EXP_INT_TABLE_A);
            FastMathCalc.Printarray(@out, "EXP_INT_TABLE_B", EXP_INT_TABLE_LEN, ExpIntTable.EXP_INT_TABLE_B);
            FastMathCalc.Printarray(@out, "EXP_FRAC_TABLE_A", EXP_FRAC_TABLE_LEN, ExpFracTable.EXP_FRAC_TABLE_A);
            FastMathCalc.Printarray(@out, "EXP_FRAC_TABLE_B", EXP_FRAC_TABLE_LEN, ExpFracTable.EXP_FRAC_TABLE_B);
            FastMathCalc.Printarray(@out, "LN_MANT",LN_MANT_LEN, lnMant.LN_MANT);
            FastMathCalc.Printarray(@out, "SINE_TABLE_A", SINE_TABLE_LEN, SINE_TABLE_A);
            FastMathCalc.Printarray(@out, "SINE_TABLE_B", SINE_TABLE_LEN, SINE_TABLE_B);
            FastMathCalc.Printarray(@out, "COSINE_TABLE_A", SINE_TABLE_LEN, COSINE_TABLE_A);
            FastMathCalc.Printarray(@out, "COSINE_TABLE_B", SINE_TABLE_LEN, COSINE_TABLE_B);
            FastMathCalc.Printarray(@out, "TANGENT_TABLE_A", SINE_TABLE_LEN, TANGENT_TABLE_A);
            FastMathCalc.Printarray(@out, "TANGENT_TABLE_B", SINE_TABLE_LEN, TANGENT_TABLE_B);
        }*/

        /// <summary>
        /// Enclose large data table in nested static class so it's only loaded on first access. </summary>
        private class ExpIntTable
        {
            /// <summary>
            /// Exponential evaluated at integer values,
            /// exp(x) =  expIntTableA[x + EXP_INT_TABLE_MAX_INDEX] + expIntTableB[x+EXP_INT_TABLE_MAX_INDEX].
            /// </summary>
            internal static readonly double[] EXP_INT_TABLE_A;
            /// <summary>
            /// Exponential evaluated at integer values,
            /// exp(x) =  expIntTableA[x + EXP_INT_TABLE_MAX_INDEX] + expIntTableB[x+EXP_INT_TABLE_MAX_INDEX]
            /// </summary>
            internal static readonly double[] EXP_INT_TABLE_B;

            static ExpIntTable()
            {
                if (RECOMPUTE_TABLES_AT_RUNTIME)
                {
                    EXP_INT_TABLE_A = new double[FastMath.EXP_INT_TABLE_LEN];
                    EXP_INT_TABLE_B = new double[FastMath.EXP_INT_TABLE_LEN];

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tmp[] = new double[2];
                    double[] tmp = new double[2];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double recip[] = new double[2];
                    double[] recip = new double[2];

                    // Populate expIntTable
                    for (int i = 0; i < FastMath.EXP_INT_TABLE_MAX_INDEX; i++)
                    {
                        FastMathCalc.Expint(i, tmp);
                        EXP_INT_TABLE_A[i + FastMath.EXP_INT_TABLE_MAX_INDEX] = tmp[0];
                        EXP_INT_TABLE_B[i + FastMath.EXP_INT_TABLE_MAX_INDEX] = tmp[1];

                        if (i != 0)
                        {
                            // Negative integer powers
                            FastMathCalc.SplitReciprocal(tmp, recip);
                            EXP_INT_TABLE_A[FastMath.EXP_INT_TABLE_MAX_INDEX - i] = recip[0];
                            EXP_INT_TABLE_B[FastMath.EXP_INT_TABLE_MAX_INDEX - i] = recip[1];
                        }
                    }
                }
                else
                {
                    EXP_INT_TABLE_A = FastMathLiteralArrays.LoadExpIntA();
                    EXP_INT_TABLE_B = FastMathLiteralArrays.LoadExpIntB();
                }
            }
        }

        /// <summary>
        /// Enclose large data table in nested static class so it's only loaded on first access. </summary>
        private class ExpFracTable
        {
            /// <summary>
            /// Exponential over the range of 0 - 1 in increments of 2^-10
            /// exp(x/1024) =  expFracTableA[x] + expFracTableB[x].
            /// 1024 = 2^10
            /// </summary>
            internal static readonly double[] EXP_FRAC_TABLE_A;
            /// <summary>
            /// Exponential over the range of 0 - 1 in increments of 2^-10
            /// exp(x/1024) =  expFracTableA[x] + expFracTableB[x].
            /// </summary>
            internal static readonly double[] EXP_FRAC_TABLE_B;

            static ExpFracTable()
            {
                if (RECOMPUTE_TABLES_AT_RUNTIME)
                {
                    EXP_FRAC_TABLE_A = new double[FastMath.EXP_FRAC_TABLE_LEN];
                    EXP_FRAC_TABLE_B = new double[FastMath.EXP_FRAC_TABLE_LEN];

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tmp[] = new double[2];
                    double[] tmp = new double[2];

                    // Populate expFracTable
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double factor = 1d / (EXP_FRAC_TABLE_LEN - 1);
                    double factor = 1d / (EXP_FRAC_TABLE_LEN - 1);
                    for (int i = 0; i < EXP_FRAC_TABLE_A.Length; i++)
                    {
                        FastMathCalc.Slowexp(i * factor, tmp);
                        EXP_FRAC_TABLE_A[i] = tmp[0];
                        EXP_FRAC_TABLE_B[i] = tmp[1];
                    }
                }
                else
                {
                    EXP_FRAC_TABLE_A = FastMathLiteralArrays.LoadExpFracA();
                    EXP_FRAC_TABLE_B = FastMathLiteralArrays.LoadExpFracB();
                }
            }
        }

        /// <summary>
        /// Enclose large data table in nested static class so it's only loaded on first access. </summary>
        private class lnMant
        {
            /// <summary>
            /// Extended precision logarithm table over the range 1 - 2 in increments of 2^-10. </summary>
            internal static readonly double[][] LN_MANT;

            static lnMant()
            {
                if (RECOMPUTE_TABLES_AT_RUNTIME)
                {
                    LN_MANT = new double[FastMath.LN_MANT_LEN][];

                    // Populate lnMant table
                    for (int i = 0; i < LN_MANT.Length; i++)
                    {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double d = Double.longBitsToDouble((((long) i) << 42) | 0x3ff0000000000000L);
                        double d = BitConverter.Int64BitsToDouble((((long) i) << 42) | 0x3ff0000000000000L);
                        LN_MANT[i] = FastMathCalc.SlowLog(d);
                    }
                }
                else
                {
                    LN_MANT = FastMathLiteralArrays.LoadLnMant();
                }
            }
        }

        /// <summary>
        /// Enclose the Cody/Waite reduction (used in "sin", "cos" and "tan"). </summary>
        private class CodyWaite
        {
            /// <summary>
            /// k </summary>
            internal readonly int finalK;
            /// <summary>
            /// remA </summary>
            internal readonly double finalRemA;
            /// <summary>
            /// remB </summary>
            internal readonly double finalRemB;

            /// <param name="xa"> Argument. </param>
            internal CodyWaite(double xa)
            {
                // Estimate k.
                //k = (int)(xa / 1.5707963267948966);
                int k = (int)(xa * 0.6366197723675814);

                // Compute remainder.
                double remA;
                double remB;
                while (true)
                {
                    double a = -k * 1.570796251296997;
                    remA = xa + a;
                    remB = -(remA - xa - a);

                    a = -k * 7.549789948768648E-8;
                    double b = remA;
                    remA = a + b;
                    remB += -(remA - b - a);

                    a = -k * 6.123233995736766E-17;
                    b = remA;
                    remA = a + b;
                    remB += -(remA - b - a);

                    if (remA > 0)
                    {
                        break;
                    }

                    // Remainder is negative, so decrement k and try again.
                    // This should only happen if the input is very close
                    // to an even multiple of pi/2.
                    --k;
                }

                this.finalK = k;
                this.finalRemA = remA;
                this.finalRemB = remB;
            }

            /// <returns> k </returns>
            internal virtual int GetK()
            {
                return finalK;
            }
            /// <returns> remA </returns>
            internal virtual double GetRemA()
            {
                return finalRemA;
            }
            /// <returns> remB </returns>
            internal virtual double GetRemB()
            {
                return finalRemB;
            }
        }
    }

}