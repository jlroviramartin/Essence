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
namespace org.apache.commons.math3.analysis.integration.gauss
{


    using DimensionMismatchException = org.apache.commons.math3.exception.DimensionMismatchException;
    using Pair = org.apache.commons.math3.util.Pair;

    /// <summary>
    /// Factory that creates Gauss-type quadrature rule using Legendre polynomials.
    /// In this implementation, the lower and upper bounds of the natural interval
    /// of integration are -1 and 1, respectively.
    /// The Legendre polynomials are evaluated using the recurrence relation
    /// presented in <a href="http://en.wikipedia.org/wiki/Abramowitz_and_Stegun">
    /// Abramowitz and Stegun, 1964</a>.
    /// 
    /// @since 3.1
    /// </summary>
    public class LegendreHighPrecisionRuleFactory : BaseRuleFactory<decimal>
    {
        /// <summary>
        /// Settings for enhanced precision computations. </summary>
        private readonly MathContext mContext;
        /// <summary>
        /// The number {@code 2}. </summary>
        private readonly decimal two;
        /// <summary>
        /// The number {@code -1}. </summary>
        private readonly decimal minusOne;
        /// <summary>
        /// The number {@code 0.5}. </summary>
        private readonly decimal oneHalf;

        /// <summary>
        /// Default precision is <seealso cref="MathContext#DECIMAL128 DECIMAL128"/>.
        /// </summary>
        public LegendreHighPrecisionRuleFactory() : this(MathContext.DECIMAL128)
        {
        }

        /// <param name="mContext"> Precision setting for computing the quadrature rules. </param>
        public LegendreHighPrecisionRuleFactory(MathContext mContext)
        {
            this.mContext = mContext;
            two = new decimal("2", mContext);
            minusOne = new decimal("-1", mContext);
            oneHalf = new decimal("0.5", mContext);
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override Pair<decimal[], BigDecimal[]> ComputeRule(int numberOfPoints)
        {

            if (numberOfPoints == 1)
            {
                // Break recursion.
                return new Pair<decimal[], BigDecimal[]>(new decimal[] { decimal.Zero }, new decimal[] { two });
            }

            // Get previous rule.
            // If it has not been computed yet it will trigger a recursive call
            // to this method.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal[] previousPoints = getRuleInternal(numberOfPoints - 1).getFirst();
            decimal[] previousPoints = GetRuleInternal(numberOfPoints - 1).GetFirst();

            // Compute next rule.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal[] points = new java.math.BigDecimal[numberOfPoints];
            decimal[] points = new decimal[numberOfPoints];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal[] weights = new java.math.BigDecimal[numberOfPoints];
            decimal[] weights = new decimal[numberOfPoints];

            // Find i-th root of P[n+1] by bracketing.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int iMax = numberOfPoints / 2;
            int iMax = numberOfPoints / 2;
            for (int i = 0; i < iMax; i++)
            {
                // Lower-bound of the interval.
                decimal a = (i == 0) ? minusOne : previousPoints[i - 1];
                // Upper-bound of the interval.
                decimal b = (iMax == 1) ? decimal.One : previousPoints[i];
                // P[j-1](a)
                decimal pma = decimal.One;
                // P[j](a)
                decimal pa = a;
                // P[j-1](b)
                decimal pmb = decimal.One;
                // P[j](b)
                decimal pb = b;
                for (int j = 1; j < numberOfPoints; j++)
                {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal b_two_j_p_1 = new java.math.BigDecimal(2 * j + 1, mContext);
                    decimal b_two_j_p_1 = new decimal(2 * j + 1, mContext);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal b_j = new java.math.BigDecimal(j, mContext);
                    decimal b_j = new decimal(j, mContext);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal b_j_p_1 = new java.math.BigDecimal(j + 1, mContext);
                    decimal b_j_p_1 = new decimal(j + 1, mContext);

                    // Compute P[j+1](a)
                    // ppa = ((2 * j + 1) * a * pa - j * pma) / (j + 1);

                    decimal tmp1 = a.multiply(b_two_j_p_1, mContext);
                    tmp1 = pa.multiply(tmp1, mContext);
                    decimal tmp2 = pma.multiply(b_j, mContext);
                    // P[j+1](a)
                    decimal ppa = tmp1.subtract(tmp2, mContext);
                    ppa = ppa.divide(b_j_p_1, mContext);

                    // Compute P[j+1](b)
                    // ppb = ((2 * j + 1) * b * pb - j * pmb) / (j + 1);

                    tmp1 = b.multiply(b_two_j_p_1, mContext);
                    tmp1 = pb.multiply(tmp1, mContext);
                    tmp2 = pmb.multiply(b_j, mContext);
                    // P[j+1](b)
                    decimal ppb = tmp1.subtract(tmp2, mContext);
                    ppb = ppb.divide(b_j_p_1, mContext);

                    pma = pa;
                    pa = ppa;
                    pmb = pb;
                    pb = ppb;
                }
                // Now pa = P[n+1](a), and pma = P[n](a). Same holds for b.
                // Middle of the interval.
                decimal c = a.add(b, mContext).multiply(oneHalf, mContext);
                // P[j-1](c)
                decimal pmc = decimal.One;
                // P[j](c)
                decimal pc = c;
                bool done = false;
                while (!done)
                {
                    decimal tmp1 = b.subtract(a, mContext);
                    decimal tmp2 = c.ulp().multiply(10M, mContext);
                    done = tmp1.CompareTo(tmp2) <= 0;
                    pmc = decimal.One;
                    pc = c;
                    for (int j = 1; j < numberOfPoints; j++)
                    {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal b_two_j_p_1 = new java.math.BigDecimal(2 * j + 1, mContext);
                        decimal b_two_j_p_1 = new decimal(2 * j + 1, mContext);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal b_j = new java.math.BigDecimal(j, mContext);
                        decimal b_j = new decimal(j, mContext);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal b_j_p_1 = new java.math.BigDecimal(j + 1, mContext);
                        decimal b_j_p_1 = new decimal(j + 1, mContext);

                        // Compute P[j+1](c)
                        tmp1 = c.multiply(b_two_j_p_1, mContext);
                        tmp1 = pc.multiply(tmp1, mContext);
                        tmp2 = pmc.multiply(b_j, mContext);
                        // P[j+1](c)
                        decimal ppc = tmp1.subtract(tmp2, mContext);
                        ppc = ppc.divide(b_j_p_1, mContext);

                        pmc = pc;
                        pc = ppc;
                    }
                    // Now pc = P[n+1](c) and pmc = P[n](c).
                    if (!done)
                    {
                        if (pa.signum() * pc.signum() <= 0)
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
                        c = a.add(b, mContext).multiply(oneHalf, mContext);
                    }
                }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal nP = new java.math.BigDecimal(numberOfPoints, mContext);
                decimal nP = new decimal(numberOfPoints, mContext);
                decimal tmp1 = pmc.subtract(c.multiply(pc, mContext), mContext);
                tmp1 = tmp1 * nP;
                tmp1 = tmp1.pow(2, mContext);
                decimal tmp2 = c.pow(2, mContext);
                tmp2 = decimal.One.subtract(tmp2, mContext);
                tmp2 = tmp2.multiply(two, mContext);
                tmp2 = tmp2.divide(tmp1, mContext);

                points[i] = c;
                weights[i] = tmp2;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int idx = numberOfPoints - i - 1;
                int idx = numberOfPoints - i - 1;
                points[idx] = c.negate(mContext);
                weights[idx] = tmp2;
            }
            // If "numberOfPoints" is odd, 0 is a root.
            // Note: as written, the test for oddness will work for negative
            // integers too (although it is not necessary here), preventing
            // a FindBugs warning.
            if (numberOfPoints % 2 != 0)
            {
                decimal pmc = decimal.One;
                for (int j = 1; j < numberOfPoints; j += 2)
                {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal b_j = new java.math.BigDecimal(j, mContext);
                    decimal b_j = new decimal(j, mContext);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal b_j_p_1 = new java.math.BigDecimal(j + 1, mContext);
                    decimal b_j_p_1 = new decimal(j + 1, mContext);

                    // pmc = -j * pmc / (j + 1);
                    pmc = pmc.multiply(b_j, mContext);
                    pmc = pmc.divide(b_j_p_1, mContext);
                    pmc = pmc.negate(mContext);
                }

                // 2 / pow(numberOfPoints * pmc, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.math.BigDecimal nP = new java.math.BigDecimal(numberOfPoints, mContext);
                decimal nP = new decimal(numberOfPoints, mContext);
                decimal tmp1 = pmc.multiply(nP, mContext);
                tmp1 = tmp1.pow(2, mContext);
                decimal tmp2 = two.divide(tmp1, mContext);

                points[iMax] = decimal.Zero;
                weights[iMax] = tmp2;
            }

            return new Pair<decimal[], BigDecimal[]>(points, weights);
        }
    }

}