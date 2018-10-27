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

    using GaussIntegratorFactory = org.apache.commons.math3.analysis.integration.gauss.GaussIntegratorFactory;
    using GaussIntegrator = org.apache.commons.math3.analysis.integration.gauss.GaussIntegrator;
    using MathIllegalArgumentException = org.apache.commons.math3.exception.MathIllegalArgumentException;
    using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
    using NotStrictlyPositiveException = org.apache.commons.math3.exception.NotStrictlyPositiveException;
    using NumberIsTooSmallException = org.apache.commons.math3.exception.NumberIsTooSmallException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;
    using FastMath = System.Math;

    /// <summary>
    /// This algorithm divides the integration interval into equally-sized
    /// sub-interval and on each of them performs a
    /// <a href="http://mathworld.wolfram.com/Legendre-GaussQuadrature.html">
    /// Legendre-Gauss</a> quadrature.
    /// Because of its <em>non-adaptive</em> nature, this algorithm can
    /// converge to a wrong value for the integral (for example, if the
    /// function is significantly different from zero toward the ends of the
    /// integration interval).
    /// In particular, a change of variables aimed at estimating integrals
    /// over infinite intervals as proposed
    /// <a href="http://en.wikipedia.org/w/index.php?title=Numerical_integration#Integrals_over_infinite_intervals">
    ///  here</a> should be avoided when using this class.
    /// 
    /// @since 3.1
    /// </summary>

    public class IterativeLegendreGaussIntegrator : BaseAbstractUnivariateIntegrator
    {
        /// <summary>
        /// Factory that computes the points and weights. </summary>
        private static readonly GaussIntegratorFactory FACTORY = new GaussIntegratorFactory();
        /// <summary>
        /// Number of integration points (per interval). </summary>
        private readonly int numberOfPoints;

        /// <summary>
        /// Builds an integrator with given accuracies and iterations counts.
        /// </summary>
        /// <param name="n"> Number of integration points. </param>
        /// <param name="relativeAccuracy"> Relative accuracy of the result. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy of the result. </param>
        /// <param name="minimalIterationCount"> Minimum number of iterations. </param>
        /// <param name="maximalIterationCount"> Maximum number of iterations. </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// or number of points are not strictly positive. </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is smaller than or equal to the minimal number of iterations. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public IterativeLegendreGaussIntegrator(final int n, final double relativeAccuracy, final double absoluteAccuracy, final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException
        public IterativeLegendreGaussIntegrator(int n, double relativeAccuracy, double absoluteAccuracy, int minimalIterationCount, int maximalIterationCount) : base(relativeAccuracy, absoluteAccuracy, minimalIterationCount, maximalIterationCount)
        {
            if (n <= 0)
            {
                throw new NotStrictlyPositiveException(LocalizedFormats.NUMBER_OF_POINTS, n);
            }
           numberOfPoints = n;
        }

        /// <summary>
        /// Builds an integrator with given accuracies.
        /// </summary>
        /// <param name="n"> Number of integration points. </param>
        /// <param name="relativeAccuracy"> Relative accuracy of the result. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy of the result. </param>
        /// <exception cref="NotStrictlyPositiveException"> if {@code n < 1}. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public IterativeLegendreGaussIntegrator(final int n, final double relativeAccuracy, final double absoluteAccuracy) throws org.apache.commons.math3.exception.NotStrictlyPositiveException
        public IterativeLegendreGaussIntegrator(int n, double relativeAccuracy, double absoluteAccuracy) : this(n, relativeAccuracy, absoluteAccuracy, DEFAULT_MIN_ITERATIONS_COUNT, DEFAULT_MAX_ITERATIONS_COUNT)
        {
        }

        /// <summary>
        /// Builds an integrator with given iteration counts.
        /// </summary>
        /// <param name="n"> Number of integration points. </param>
        /// <param name="minimalIterationCount"> Minimum number of iterations. </param>
        /// <param name="maximalIterationCount"> Maximum number of iterations. </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive. </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is smaller than or equal to the minimal number of iterations. </exception>
        /// <exception cref="NotStrictlyPositiveException"> if {@code n < 1}. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public IterativeLegendreGaussIntegrator(final int n, final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException
        public IterativeLegendreGaussIntegrator(int n, int minimalIterationCount, int maximalIterationCount) : this(n, DEFAULT_RELATIVE_ACCURACY, DEFAULT_ABSOLUTE_ACCURACY, minimalIterationCount, maximalIterationCount)
        {
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override double DoIntegrate()
        {
            // Compute first estimate with a single step.
            double oldt = Stage(1);

            int n = 2;
            while (true)
            {
                // Improve integral with a larger number of steps.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double t = stage(n);
                double t = Stage(n);

                // Estimate the error.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double delta = org.apache.commons.math3.util.FastMath.abs(t - oldt);
                double delta = FastMath.Abs(t - oldt);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double limit = org.apache.commons.math3.util.FastMath.max(getAbsoluteAccuracy(), getRelativeAccuracy() * (org.apache.commons.math3.util.FastMath.abs(oldt) + org.apache.commons.math3.util.FastMath.abs(t)) * 0.5);
                double limit = FastMath.Max(GetAbsoluteAccuracy(), GetRelativeAccuracy() * (FastMath.Abs(oldt) + FastMath.Abs(t)) * 0.5);

                // check convergence
                if (GetIterations() + 1 >= GetMinimalIterationCount() && delta <= limit)
                {
                    return t;
                }

                // Prepare next iteration.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ratio = org.apache.commons.math3.util.FastMath.min(4, org.apache.commons.math3.util.FastMath.pow(delta / limit, 0.5 / numberOfPoints));
                double ratio = FastMath.Min(4, FastMath.Pow(delta / limit, 0.5 / numberOfPoints));
                n = FastMath.Max((int)(ratio * n), n + 1);
                oldt = t;
                IncrementCount();
            }
        }

        /// <summary>
        /// Compute the n-th stage integral.
        /// </summary>
        /// <param name="n"> Number of steps. </param>
        /// <returns> the value of n-th stage integral. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximum number of evaluations
        /// is exceeded. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double stage(final int n) throws org.apache.commons.math3.exception.TooManyEvaluationsException
        private double Stage(int n)
        {
            // Function to be integrated is stored in the base class.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.apache.commons.math3.analysis.UnivariateFunction f = new org.apache.commons.math3.analysis.UnivariateFunction()
            UnivariateFunction f = new UnivariateFunctionAnonymousInnerClass(this);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double min = getMin();
            double min = GetMin();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double max = getMax();
            double max = GetMax();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double step = (max - min) / n;
            double step = (max - min) / n;

            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                // Integrate over each sub-interval [a, b].
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a = min + i * step;
                double a = min + i * step;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double b = a + step;
                double b = a + step;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.apache.commons.math3.analysis.integration.gauss.GaussIntegrator g = FACTORY.legendreHighPrecision(numberOfPoints, a, b);
                GaussIntegrator g = FACTORY.Legendre(this.numberOfPoints, a, b);
#if false
                GaussIntegrator g = FACTORY.LegendreHighPrecision(numberOfPoints, a, b);
#endif
                sum += g.Integrate(f);
            }

            return sum;
        }

        private class UnivariateFunctionAnonymousInnerClass : UnivariateFunction
        {
            private readonly IterativeLegendreGaussIntegrator outerInstance;

            public UnivariateFunctionAnonymousInnerClass(IterativeLegendreGaussIntegrator outerInstance)
            {
                this.outerInstance = outerInstance;
            }

                        /// <summary>
                        /// {@inheritDoc} </summary>
            public virtual double Value(double x)
            {
                return outerInstance.ComputeObjectiveValue(x);
            }
        }
    }

}