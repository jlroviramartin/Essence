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

    using MathIllegalArgumentException = org.apache.commons.math3.exception.MathIllegalArgumentException;
    using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
    using NotStrictlyPositiveException = org.apache.commons.math3.exception.NotStrictlyPositiveException;
    using NumberIsTooLargeException = org.apache.commons.math3.exception.NumberIsTooLargeException;
    using NumberIsTooSmallException = org.apache.commons.math3.exception.NumberIsTooSmallException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;
    using FastMath = org.apache.commons.math3.util.FastMath;

    /// <summary>
    /// Implements the <a href="http://en.wikipedia.org/wiki/Midpoint_method">
    /// Midpoint Rule</a> for integration of real univariate functions. For
    /// reference, see <b>Numerical Mathematics</b>, ISBN 0387989595,
    /// chapter 9.2.
    /// <para>
    /// The function should be integrable.</para>
    /// 
    /// @since 3.3
    /// </summary>
    public class MidPointIntegrator : BaseAbstractUnivariateIntegrator
    {

        /// <summary>
        /// Maximum number of iterations for midpoint. </summary>
        public const int MIDPOINT_MAX_ITERATIONS_COUNT = 64;

        /// <summary>
        /// Build a midpoint integrator with given accuracies and iterations counts. </summary>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations
        /// (must be less than or equal to <seealso cref="#MIDPOINT_MAX_ITERATIONS_COUNT"/> </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        /// <exception cref="NumberIsTooLargeException"> if maximal number of iterations
        /// is greater than <seealso cref="#MIDPOINT_MAX_ITERATIONS_COUNT"/> </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public MidPointIntegrator(final double relativeAccuracy, final double absoluteAccuracy, final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException, org.apache.commons.math3.exception.NumberIsTooLargeException
        public MidPointIntegrator(double relativeAccuracy, double absoluteAccuracy, int minimalIterationCount, int maximalIterationCount) : base(relativeAccuracy, absoluteAccuracy, minimalIterationCount, maximalIterationCount)
        {
            if (maximalIterationCount > MIDPOINT_MAX_ITERATIONS_COUNT)
            {
                throw new NumberIsTooLargeException(maximalIterationCount, MIDPOINT_MAX_ITERATIONS_COUNT, false);
            }
        }

        /// <summary>
        /// Build a midpoint integrator with given iteration counts. </summary>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations
        /// (must be less than or equal to <seealso cref="#MIDPOINT_MAX_ITERATIONS_COUNT"/> </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        /// <exception cref="NumberIsTooLargeException"> if maximal number of iterations
        /// is greater than <seealso cref="#MIDPOINT_MAX_ITERATIONS_COUNT"/> </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public MidPointIntegrator(final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException, org.apache.commons.math3.exception.NumberIsTooLargeException
        public MidPointIntegrator(int minimalIterationCount, int maximalIterationCount) : base(minimalIterationCount, maximalIterationCount)
        {
            if (maximalIterationCount > MIDPOINT_MAX_ITERATIONS_COUNT)
            {
                throw new NumberIsTooLargeException(maximalIterationCount, MIDPOINT_MAX_ITERATIONS_COUNT, false);
            }
        }

        /// <summary>
        /// Construct a midpoint integrator with default settings.
        /// (max iteration count set to <seealso cref="#MIDPOINT_MAX_ITERATIONS_COUNT"/>)
        /// </summary>
        public MidPointIntegrator() : base(DEFAULT_MIN_ITERATIONS_COUNT, MIDPOINT_MAX_ITERATIONS_COUNT)
        {
        }

        /// <summary>
        /// Compute the n-th stage integral of midpoint rule.
        /// This function should only be called by API <code>integrate()</code> in the package.
        /// To save time it does not verify arguments - caller does.
        /// <para>
        /// The interval is divided equally into 2^n sections rather than an
        /// arbitrary m sections because this configuration can best utilize the
        /// already computed values.</para>
        /// </summary>
        /// <param name="n"> the stage of 1/2 refinement. Must be larger than 0. </param>
        /// <param name="previousStageResult"> Result from the previous call to the
        /// {@code stage} method. </param>
        /// <param name="min"> Lower bound of the integration interval. </param>
        /// <param name="diffMaxMin"> Difference between the lower bound and upper bound
        /// of the integration interval. </param>
        /// <returns> the value of n-th stage integral </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximal number of evaluations
        /// is exceeded. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double stage(final int n, double previousStageResult, double min, double diffMaxMin) throws org.apache.commons.math3.exception.TooManyEvaluationsException
        private double Stage(int n, double previousStageResult, double min, double diffMaxMin)
        {

            // number of new points in this stage
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long np = 1L << (n - 1);
            long np = 1L << (n - 1);
            double sum = 0;

            // spacing between adjacent new points
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double spacing = diffMaxMin / np;
            double spacing = diffMaxMin / np;

            // the first new point
            double x = min + 0.5 * spacing;
            for (long i = 0; i < np; i++)
            {
                sum += ComputeObjectiveValue(x);
                x += spacing;
            }
            // add the new sum to previously calculated result
            return 0.5 * (previousStageResult + sum * spacing);
        }


        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override double DoIntegrate()
        {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double min = getMin();
            double min = GetMin();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double diff = getMax() - min;
            double diff = GetMax() - min;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double midPoint = min + 0.5 * diff;
            double midPoint = min + 0.5 * diff;

            double oldt = diff * ComputeObjectiveValue(midPoint);

            while (true)
            {
                IncrementCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int i = getIterations();
                int i = GetIterations();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double t = stage(i, oldt, min, diff);
                double t = Stage(i, oldt, min, diff);
                if (i >= GetMinimalIterationCount())
                {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double delta = org.apache.commons.math3.util.FastMath.abs(t - oldt);
                    double delta = FastMath.Abs(t - oldt);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double rLimit = getRelativeAccuracy() * (org.apache.commons.math3.util.FastMath.abs(oldt) + org.apache.commons.math3.util.FastMath.abs(t)) * 0.5;
                    double rLimit = GetRelativeAccuracy() * (FastMath.Abs(oldt) + FastMath.Abs(t)) * 0.5;
                    if ((delta <= rLimit) || (delta <= GetAbsoluteAccuracy()))
                    {
                        return t;
                    }
                }
                oldt = t;
            }

        }

    }

}