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
namespace org.apache.commons.math3.analysis.integration
{

    using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
    using NotStrictlyPositiveException = org.apache.commons.math3.exception.NotStrictlyPositiveException;
    using NumberIsTooLargeException = org.apache.commons.math3.exception.NumberIsTooLargeException;
    using NumberIsTooSmallException = org.apache.commons.math3.exception.NumberIsTooSmallException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;
    using FastMath = System.Math;

    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/RombergIntegration.html">
    /// Romberg Algorithm</a> for integration of real univariate functions. For
    /// reference, see <b>Introduction to Numerical Analysis</b>, ISBN 038795452X,
    /// chapter 3.
    /// <para>
    /// Romberg integration employs k successive refinements of the trapezoid
    /// rule to remove error terms less than order O(N^(-2k)). Simpson's rule
    /// is a special case of k = 2.</para>
    /// 
    /// @since 1.2
    /// </summary>
    public class RombergIntegrator : BaseAbstractUnivariateIntegrator
    {

        /// <summary>
        /// Maximal number of iterations for Romberg. </summary>
        public const int ROMBERG_MAX_ITERATIONS_COUNT = 32;

        /// <summary>
        /// Build a Romberg integrator with given accuracies and iterations counts. </summary>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations
        /// (must be less than or equal to <seealso cref="#ROMBERG_MAX_ITERATIONS_COUNT"/>) </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        /// <exception cref="NumberIsTooLargeException"> if maximal number of iterations
        /// is greater than <seealso cref="#ROMBERG_MAX_ITERATIONS_COUNT"/> </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public RombergIntegrator(final double relativeAccuracy, final double absoluteAccuracy, final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException, org.apache.commons.math3.exception.NumberIsTooLargeException
        public RombergIntegrator(double relativeAccuracy, double absoluteAccuracy, int minimalIterationCount, int maximalIterationCount) : base(relativeAccuracy, absoluteAccuracy, minimalIterationCount, maximalIterationCount)
        {
            if (maximalIterationCount > ROMBERG_MAX_ITERATIONS_COUNT)
            {
                throw new NumberIsTooLargeException(maximalIterationCount, ROMBERG_MAX_ITERATIONS_COUNT, false);
            }
        }

        /// <summary>
        /// Build a Romberg integrator with given iteration counts. </summary>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations
        /// (must be less than or equal to <seealso cref="#ROMBERG_MAX_ITERATIONS_COUNT"/>) </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        /// <exception cref="NumberIsTooLargeException"> if maximal number of iterations
        /// is greater than <seealso cref="#ROMBERG_MAX_ITERATIONS_COUNT"/> </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public RombergIntegrator(final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException, org.apache.commons.math3.exception.NumberIsTooLargeException
        public RombergIntegrator(int minimalIterationCount, int maximalIterationCount) : base(minimalIterationCount, maximalIterationCount)
        {
            if (maximalIterationCount > ROMBERG_MAX_ITERATIONS_COUNT)
            {
                throw new NumberIsTooLargeException(maximalIterationCount, ROMBERG_MAX_ITERATIONS_COUNT, false);
            }
        }

        /// <summary>
        /// Construct a Romberg integrator with default settings
        /// (max iteration count set to <seealso cref="#ROMBERG_MAX_ITERATIONS_COUNT"/>)
        /// </summary>
        public RombergIntegrator() : base(DEFAULT_MIN_ITERATIONS_COUNT, ROMBERG_MAX_ITERATIONS_COUNT)
        {
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override double DoIntegrate()
        {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int m = getMaximalIterationCount() + 1;
            int m = GetMaximalIterationCount() + 1;
            double[] previousRow = new double[m];
            double[] currentRow = new double[m];

            TrapezoidIntegrator qtrap = new TrapezoidIntegrator();
            currentRow[0] = qtrap.Stage(this, 0);
            IncrementCount();
            double olds = currentRow[0];
            while (true)
            {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int i = getIterations();
                int i = GetIterations();

                // switch rows
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] tmpRow = previousRow;
                double[] tmpRow = previousRow;
                previousRow = currentRow;
                currentRow = tmpRow;

                currentRow[0] = qtrap.Stage(this, i);
                IncrementCount();
                for (int j = 1; j <= i; j++)
                {
                    // Richardson extrapolation coefficient
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double r = (1L << (2 * j)) - 1;
                    double r = (1L << (2 * j)) - 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tIJm1 = currentRow[j - 1];
                    double tIJm1 = currentRow[j - 1];
                    currentRow[j] = tIJm1 + (tIJm1 - previousRow[j - 1]) / r;
                }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double s = currentRow[i];
                double s = currentRow[i];
                if (i >= GetMinimalIterationCount())
                {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double delta = org.apache.commons.math3.util.FastMath.abs(s - olds);
                    double delta = FastMath.Abs(s - olds);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double rLimit = getRelativeAccuracy() * (org.apache.commons.math3.util.FastMath.abs(olds) + org.apache.commons.math3.util.FastMath.abs(s)) * 0.5;
                    double rLimit = GetRelativeAccuracy() * (FastMath.Abs(olds) + FastMath.Abs(s)) * 0.5;
                    if ((delta <= rLimit) || (delta <= GetAbsoluteAccuracy()))
                    {
                        return s;
                    }
                }
                olds = s;
            }

        }

    }

}