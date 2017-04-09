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
    /// Implements the <a href="http://mathworld.wolfram.com/TrapezoidalRule.html">
    /// Trapezoid Rule</a> for integration of real univariate functions. For
    /// reference, see <b>Introduction to Numerical Analysis</b>, ISBN 038795452X,
    /// chapter 3.
    /// <para>
    /// The function should be integrable.</para>
    /// 
    /// @since 1.2
    /// </summary>
    public class TrapezoidIntegrator : BaseAbstractUnivariateIntegrator
    {

        /// <summary>
        /// Maximum number of iterations for trapezoid. </summary>
        public const int TRAPEZOID_MAX_ITERATIONS_COUNT = 64;

        /// <summary>
        /// Intermediate result. </summary>
        private double s;

        /// <summary>
        /// Build a trapezoid integrator with given accuracies and iterations counts. </summary>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations
        /// (must be less than or equal to <seealso cref="#TRAPEZOID_MAX_ITERATIONS_COUNT"/> </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        /// <exception cref="NumberIsTooLargeException"> if maximal number of iterations
        /// is greater than <seealso cref="#TRAPEZOID_MAX_ITERATIONS_COUNT"/> </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public TrapezoidIntegrator(final double relativeAccuracy, final double absoluteAccuracy, final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException, org.apache.commons.math3.exception.NumberIsTooLargeException
        public TrapezoidIntegrator(double relativeAccuracy, double absoluteAccuracy, int minimalIterationCount, int maximalIterationCount) : base(relativeAccuracy, absoluteAccuracy, minimalIterationCount, maximalIterationCount)
        {
            if (maximalIterationCount > TRAPEZOID_MAX_ITERATIONS_COUNT)
            {
                throw new NumberIsTooLargeException(maximalIterationCount, TRAPEZOID_MAX_ITERATIONS_COUNT, false);
            }
        }

        /// <summary>
        /// Build a trapezoid integrator with given iteration counts. </summary>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations
        /// (must be less than or equal to <seealso cref="#TRAPEZOID_MAX_ITERATIONS_COUNT"/> </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        /// <exception cref="NumberIsTooLargeException"> if maximal number of iterations
        /// is greater than <seealso cref="#TRAPEZOID_MAX_ITERATIONS_COUNT"/> </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public TrapezoidIntegrator(final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException, org.apache.commons.math3.exception.NumberIsTooLargeException
        public TrapezoidIntegrator(int minimalIterationCount, int maximalIterationCount) : base(minimalIterationCount, maximalIterationCount)
        {
            if (maximalIterationCount > TRAPEZOID_MAX_ITERATIONS_COUNT)
            {
                throw new NumberIsTooLargeException(maximalIterationCount, TRAPEZOID_MAX_ITERATIONS_COUNT, false);
            }
        }

        /// <summary>
        /// Construct a trapezoid integrator with default settings.
        /// (max iteration count set to <seealso cref="#TRAPEZOID_MAX_ITERATIONS_COUNT"/>)
        /// </summary>
        public TrapezoidIntegrator() : base(DEFAULT_MIN_ITERATIONS_COUNT, TRAPEZOID_MAX_ITERATIONS_COUNT)
        {
        }

        /// <summary>
        /// Compute the n-th stage integral of trapezoid rule. This function
        /// should only be called by API <code>integrate()</code> in the package.
        /// To save time it does not verify arguments - caller does.
        /// <para>
        /// The interval is divided equally into 2^n sections rather than an
        /// arbitrary m sections because this configuration can best utilize the
        /// already computed values.</para>
        /// </summary>
        /// <param name="baseIntegrator"> integrator holding integration parameters </param>
        /// <param name="n"> the stage of 1/2 refinement, n = 0 is no refinement </param>
        /// <returns> the value of n-th stage integral </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximal number of evaluations
        /// is exceeded. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: double stage(final BaseAbstractUnivariateIntegrator baseIntegrator, final int n) throws org.apache.commons.math3.exception.TooManyEvaluationsException
        internal virtual double Stage(BaseAbstractUnivariateIntegrator baseIntegrator, int n)
        {

            if (n == 0)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double max = baseIntegrator.getMax();
                double max = baseIntegrator.GetMax();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double min = baseIntegrator.getMin();
                double min = baseIntegrator.GetMin();
                s = 0.5 * (max - min) * (baseIntegrator.ComputeObjectiveValue(min) + baseIntegrator.ComputeObjectiveValue(max));
                return s;
            }
            else
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long np = 1L << (n-1);
                long np = 1L << (n - 1); // number of new points in this stage
                double sum = 0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double max = baseIntegrator.getMax();
                double max = baseIntegrator.GetMax();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double min = baseIntegrator.getMin();
                double min = baseIntegrator.GetMin();
                // spacing between adjacent new points
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double spacing = (max - min) / np;
                double spacing = (max - min) / np;
                double x = min + 0.5 * spacing; // the first new point
                for (long i = 0; i < np; i++)
                {
                    sum += baseIntegrator.ComputeObjectiveValue(x);
                    x += spacing;
                }
                // add the new sum to previously calculated result
                s = 0.5 * (s + sum * spacing);
                return s;
            }
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override double DoIntegrate()
        {

            double oldt = Stage(this, 0);
            IncrementCount();
            while (true)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int i = getIterations();
                int i = GetIterations();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double t = stage(this, i);
                double t = Stage(this, i);
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
                IncrementCount();
            }

        }

    }

}