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
using org.apache.commons.math3.analysis.util;
using FastMath = System.Math;

namespace org.apache.commons.math3.analysis.integration.gauss
{
    /// <summary>
    /// Factory that creates a
    /// <a href="http://en.wikipedia.org/wiki/Gauss-Hermite_quadrature">
    ///  Gauss-type quadrature rule using Hermite polynomials</a>
    /// of the first kind.
    /// Such a quadrature rule allows the calculation of improper integrals
    /// of a function
    /// <code>
    ///  f(x) e<sup>-x<sup>2</sup></sup>
    /// </code>
    /// <br/>
    /// Recurrence relation and weights computation follow
    /// <a href="http://en.wikipedia.org/wiki/Abramowitz_and_Stegun"
    /// Abramowitz and Stegun, 1964</a>.
    /// <br/>
    /// The coefficients of the standard Hermite polynomials grow very rapidly;
    /// in order to avoid overflows, each Hermite polynomial is normalized with
    /// respect to the underlying scalar product.
    /// The initial interval for the application of the bisection method is
    /// based on the roots of the previous Hermite polynomial (interlacing).
    /// Upper and lower bounds of these roots are provided by
    /// <quote>
    ///  I. Krasikov,
    ///  <em>Nonnegative quadratic forms and bounds on orthogonal polynomials</em>,
    ///  Journal of Approximation theory <b>111</b>, 31-49
    /// </quote>
    /// 
    /// @since 3.3
    /// @version $Id: HermiteRuleFactory.java 1500018 2013-07-05 14:20:19Z erans $
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
        protected internal override Tuple<double[], double[]> ComputeRule(int numberOfPoints)
        {
            if (numberOfPoints == 1)
            {
                // Break recursion.
                return new Tuple<double[], double[]>(new double[] { 0d }, new double[] { SQRT_PI });
            }

            // Get previous rule.
            // If it has not been computed yet it will trigger a recursive call
            // to this method.
            int lastNumPoints = numberOfPoints - 1;
            double[] previousPoints = this.GetRuleInternal(lastNumPoints).Item1;

            // Compute next rule.
            double[] points = new double[numberOfPoints];
            double[] weights = new double[numberOfPoints];

            double sqrtTwoTimesLastNumPoints = FastMath.Sqrt(2 * lastNumPoints);
            double sqrtTwoTimesNumPoints = FastMath.Sqrt(2 * numberOfPoints);

            // Find i-th root of H[n+1] by bracketing.
            int iMax = numberOfPoints / 2;
            for (int i = 0; i < iMax; i++)
            {
                // Lower-bound of the interval.
                double a = (i == 0) ? - sqrtTwoTimesLastNumPoints : (double)previousPoints[i - 1];
                // Upper-bound of the interval.
                double b = (iMax == 1) ? - 0.5 : (double)previousPoints[i];

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
                    double jp1 = j + 1;
                    double s = FastMath.Sqrt(2 / jp1);
                    double sm = FastMath.Sqrt(j / jp1);
                    double hpa = s * a * ha - sm * hma;
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
                        double jp1 = j + 1;
                        double s = FastMath.Sqrt(2 / jp1);
                        double sm = FastMath.Sqrt(j / jp1);
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
                double d = sqrtTwoTimesNumPoints * hmc;
                double w = 2 / (d * d);

                points[i] = c;
                weights[i] = w;

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
                    double jp1 = j + 1;
                    hm = -FastMath.Sqrt(j / jp1) * hm;
                }
                double d = sqrtTwoTimesNumPoints * hm;
                double w = 2 / (d * d);

                points[iMax] = 0d;
                weights[iMax] = w;
            }

            return new Tuple<double[], double[]>(points, weights);
        }
    }
}