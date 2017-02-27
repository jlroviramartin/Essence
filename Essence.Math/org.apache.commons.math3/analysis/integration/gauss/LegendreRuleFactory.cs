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

namespace org.apache.commons.math3.analysis.integration.gauss
{
    /// <summary>
    /// Factory that creates Gauss-type quadrature rule using Legendre polynomials.
    /// In this implementation, the lower and upper bounds of the natural interval
    /// of integration are -1 and 1, respectively.
    /// The Legendre polynomials are evaluated using the recurrence relation
    /// presented in <a href="http://en.wikipedia.org/wiki/Abramowitz_and_Stegun"
    /// Abramowitz and Stegun, 1964</a>.
    /// 
    /// @since 3.1
    /// @version $Id: LegendreRuleFactory.java 1455194 2013-03-11 15:45:54Z luc $
    /// </summary>
    public class LegendreRuleFactory : BaseRuleFactory<double>
    {
        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override Tuple<double[], double[]> ComputeRule(int numberOfPoints)
        {
            if (numberOfPoints == 1)
            {
                // Break recursion.
                return new Tuple<double[], double[]>(new double[] { 0d }, new double[] { 2d });
            }

            // Get previous rule.
            // If it has not been computed yet it will trigger a recursive call
            // to this method.
            double[] previousPoints = this.GetRuleInternal(numberOfPoints - 1).Item1;

            // Compute next rule.
            double[] points = new double[numberOfPoints];
            double[] weights = new double[numberOfPoints];

            // Find i-th root of P[n+1] by bracketing.
            int iMax = numberOfPoints / 2;
            for (int i = 0; i < iMax; i++)
            {
                // Lower-bound of the interval.
                double a = (i == 0) ? - 1 : (double)previousPoints[i - 1];
                // Upper-bound of the interval.
                double b = (iMax == 1) ? 1 : (double)previousPoints[i];
                // P[j-1](a)
                double pma = 1;
                // P[j](a)
                double pa = a;
                // P[j-1](b)
                double pmb = 1;
                // P[j](b)
                double pb = b;
                for (int j = 1; j < numberOfPoints; j++)
                {
                    int two_j_p_1 = 2 * j + 1;
                    int j_p_1 = j + 1;
                    // P[j+1](a)
                    double ppa = (two_j_p_1 * a * pa - j * pma) / j_p_1;
                    // P[j+1](b)
                    double ppb = (two_j_p_1 * b * pb - j * pmb) / j_p_1;
                    pma = pa;
                    pa = ppa;
                    pmb = pb;
                    pb = ppb;
                }
                // Now pa = P[n+1](a), and pma = P[n](a) (same holds for b).
                // Middle of the interval.
                double c = 0.5 * (a + b);
                // P[j-1](c)
                double pmc = 1;
                // P[j](c)
                double pc = c;
                bool done = false;
                while (!done)
                {
                    done = b - a <= MyUtils.ULP(c);
                    pmc = 1;
                    pc = c;
                    for (int j = 1; j < numberOfPoints; j++)
                    {
                        // P[j+1](c)
                        double ppc = ((2 * j + 1) * c * pc - j * pmc) / (j + 1);
                        pmc = pc;
                        pc = ppc;
                    }
                    // Now pc = P[n+1](c) and pmc = P[n](c).
                    if (!done)
                    {
                        if (pa * pc <= 0)
                        {
                            b = c;
                            pmb = pmc;
                            pb = pc;
                        }
                        else
                        {
                            a = c;
                            pma = pmc;
                            pa = pc;
                        }
                        c = 0.5 * (a + b);
                    }
                }
                double d = numberOfPoints * (pmc - c * pc);
                double w = 2 * (1 - c * c) / (d * d);

                points[i] = c;
                weights[i] = w;

                int idx = numberOfPoints - i - 1;
                points[idx] = -c;
                weights[idx] = w;
            }
            // If "numberOfPoints" is odd, 0 is a root.
            // Note: as written, the test for oddness will work for negative
            // integers too (although it is not necessary here), preventing
            // a FindBugs warning.
            if (numberOfPoints % 2 != 0)
            {
                double pmc = 1;
                for (int j = 1; j < numberOfPoints; j += 2)
                {
                    pmc = -j * pmc / (j + 1);
                }
                double d = numberOfPoints * pmc;
                double w = 2 / (d * d);

                points[iMax] = 0d;
                weights[iMax] = w;
            }

            return new Tuple<double[], double[]>(points, weights);
        }
    }
}