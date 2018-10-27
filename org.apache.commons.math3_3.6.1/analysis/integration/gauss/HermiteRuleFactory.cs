/// Apache Commons Math 3.6.1
using System;
using org.apache.commons.math3.analysis.util;

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
namespace org.apache.commons.math3.analysis.integration.gauss
{

    using DimensionMismatchException = org.apache.commons.math3.exception.DimensionMismatchException;
    //using Pair = org.apache.commons.math3.util.Pair;
    using org.apache.commons.math3.util;
    using FastMath = System.Math;

    /// <summary>
    /// Factory that creates a
    /// <a href="http://en.wikipedia.org/wiki/Gauss-Hermite_quadrature">
    /// Gauss-type quadrature rule using Hermite polynomials</a>
    /// of the first kind.
    /// Such a quadrature rule allows the calculation of improper integrals
    /// of a function
    /// <para>
    ///  \(f(x) e^{-x^2}\)
    /// </para>
    /// </para><para>
    /// Recurrence relation and weights computation follow
    /// <a href="http://en.wikipedia.org/wiki/Abramowitz_and_Stegun">
    /// Abramowitz and Stegun, 1964</a>.
    /// </para><para>
    /// The coefficients of the standard Hermite polynomials grow very rapidly.
    /// In order to avoid overflows, each Hermite polynomial is normalized with
    /// respect to the underlying scalar product.
    /// The initial interval for the application of the bisection method is
    /// based on the roots of the previous Hermite polynomial (interlacing).
    /// Upper and lower bounds of these roots are provided by </p>
    /// <blockquote>
    ///  I. Krasikov,
    ///  <em>Nonnegative quadratic forms and bounds on orthogonal polynomials</em>,
    ///  Journal of Approximation theory <b>111</b>, 31-49
    /// </blockquote>
    /// 
    /// @since 3.3
    /// </summary>
    public class HermiteRuleFactory : BaseRuleFactory<double>
    {
        /// <summary>
        /// &pi;<sup>1/2</sup> </summary>
        private const double SQRT_PI = 1.77245385090551602729;
        /// <summary>
        /// &pi;<sup>-1/4</sup> </summary>
        private const double H0 = 7.5112554446494248286e-1;
        /// <summary>
        /// &pi;<sup>-1/4</sup> &radic;2 </summary>
        private const double H1 = 1.0622519320271969145;

        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override Pair<double[], double[]> ComputeRule(int numberOfPoints)
        {

            if (numberOfPoints == 1)
            {
                // Break recursion.
                return new Pair<double[], double[]>(new double[] { 0d }, new double[] { SQRT_PI });
            }

            // Get previous rule.
            // If it has not been computed yet it will trigger a recursive call
            // to this method.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int lastNumPoints = numberOfPoints - 1;
            int lastNumPoints = numberOfPoints - 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Nullable<double>[] previousPoints = getRuleInternal(lastNumPoints).getFirst();
            double[] previousPoints = GetRuleInternal(lastNumPoints).GetFirst();

            // Compute next rule.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Nullable<double>[] points = new Nullable<double>[numberOfPoints];
            double[] points = new double[numberOfPoints];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Nullable<double>[] weights = new Nullable<double>[numberOfPoints];
            double[] weights = new double[numberOfPoints];

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sqrtTwoTimesLastNumPoints = org.apache.commons.math3.util.FastMath.sqrt(2 * lastNumPoints);
            double sqrtTwoTimesLastNumPoints = FastMath.Sqrt(2 * lastNumPoints);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sqrtTwoTimesNumPoints = org.apache.commons.math3.util.FastMath.sqrt(2 * numberOfPoints);
            double sqrtTwoTimesNumPoints = FastMath.Sqrt(2 * numberOfPoints);

            // Find i-th root of H[n+1] by bracketing.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int iMax = numberOfPoints / 2;
            int iMax = numberOfPoints / 2;
            for (int i = 0; i < iMax; i++)
            {
                // Lower-bound of the interval.
                double a = (i == 0) ? -sqrtTwoTimesLastNumPoints : previousPoints[i - 1];
                // Upper-bound of the interval.
                double b = (iMax == 1) ? -0.5 : previousPoints[i];

                // H[j-1](a)
                double hma = H0;
                // H[j](a)
                double ha = H1 * a;
                // H[j-1](b)
                double hmb = H0;
                // H[j](b)
                double hb = H1 * b;
                for (int j = 1; j < numberOfPoints; j++)
                {
                    // Compute H[j+1](a) and H[j+1](b)
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double jp1 = j + 1;
                    double jp1 = j + 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double s = org.apache.commons.math3.util.FastMath.sqrt(2 / jp1);
                    double s = FastMath.Sqrt(2 / jp1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sm = org.apache.commons.math3.util.FastMath.sqrt(j / jp1);
                    double sm = FastMath.Sqrt(j / jp1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double hpa = s * a * ha - sm * hma;
                    double hpa = s * a * ha - sm * hma;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double hpb = s * b * hb - sm * hmb;
                    double hpb = s * b * hb - sm * hmb;
                    hma = ha;
                    ha = hpa;
                    hmb = hb;
                    hb = hpb;
                }

                // Now ha = H[n+1](a), and hma = H[n](a) (same holds for b).
                // Middle of the interval.
                double c = 0.5 * (a + b);
                // P[j-1](c)
                double hmc = H0;
                // P[j](c)
                double hc = H1 * c;
                bool done = false;
                while (!done)
                {
                    done = b - a <= MyUtils.ULP(c);
                    hmc = H0;
                    hc = H1 * c;
                    for (int j = 1; j < numberOfPoints; j++)
                    {
                        // Compute H[j+1](c)
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double jp1 = j + 1;
                        double jp1 = j + 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double s = org.apache.commons.math3.util.FastMath.sqrt(2 / jp1);
                        double s = FastMath.Sqrt(2 / jp1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double sm = org.apache.commons.math3.util.FastMath.sqrt(j / jp1);
                        double sm = FastMath.Sqrt(j / jp1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double hpc = s * c * hc - sm * hmc;
                        double hpc = s * c * hc - sm * hmc;
                        hmc = hc;
                        hc = hpc;
                    }
                    // Now h = H[n+1](c) and hm = H[n](c).
                    if (!done)
                    {
                        if (ha * hc < 0)
                        {
                            b = c;
                            hmb = hmc;
                            hb = hc;
                        }
                        else
                        {
                            a = c;
                            hma = hmc;
                            ha = hc;
                        }
                        c = 0.5 * (a + b);
                    }
                }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double d = sqrtTwoTimesNumPoints * hmc;
                double d = sqrtTwoTimesNumPoints * hmc;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double w = 2 / (d * d);
                double w = 2 / (d * d);

                points[i] = c;
                weights[i] = w;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int idx = lastNumPoints - i;
                int idx = lastNumPoints - i;
                points[idx] = -c;
                weights[idx] = w;
            }

            // If "numberOfPoints" is odd, 0 is a root.
            // Note: as written, the test for oddness will work for negative
            // integers too (although it is not necessary here), preventing
            // a FindBugs warning.
            if (numberOfPoints % 2 != 0)
            {
                double hm = H0;
                for (int j = 1; j < numberOfPoints; j += 2)
                {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double jp1 = j + 1;
                    double jp1 = j + 1;
                    hm = -FastMath.Sqrt(j / jp1) * hm;
                }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double d = sqrtTwoTimesNumPoints * hm;
                double d = sqrtTwoTimesNumPoints * hm;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double w = 2 / (d * d);
                double w = 2 / (d * d);

                points[iMax] = 0d;
                weights[iMax] = w;
            }

            return new Pair<double[], double[]>(points, weights);
        }
    }

}