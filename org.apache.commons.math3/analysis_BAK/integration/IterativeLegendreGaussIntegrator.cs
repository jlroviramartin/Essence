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

using org.apache.commons.math3.analysis.exception;
using org.apache.commons.math3.analysis.integration.gauss;
using FastMath = System.Math;

namespace org.apache.commons.math3.analysis.integration
{
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
    /// @version $Id: IterativeLegendreGaussIntegrator.java 1499765 2013-07-04 14:24:11Z erans $
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
        public IterativeLegendreGaussIntegrator(int n, double relativeAccuracy, double absoluteAccuracy, int minimalIterationCount, int maximalIterationCount)
            : base(relativeAccuracy, absoluteAccuracy, minimalIterationCount, maximalIterationCount)
        {
            if (n <= 0)
            {
                throw new NotStrictlyPositiveException("LocalizedFormats.NUMBER_OF_POINTS", n);
            }
            this.numberOfPoints = n;
        }

        /// <summary>
        /// Builds an integrator with given accuracies.
        /// </summary>
        /// <param name="n"> Number of integration points. </param>
        /// <param name="relativeAccuracy"> Relative accuracy of the result. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy of the result. </param>
        /// <exception cref="NotStrictlyPositiveException"> if {@code n < 1}. </exception>
        public IterativeLegendreGaussIntegrator(int n, double relativeAccuracy, double absoluteAccuracy)
            : this(n, relativeAccuracy, absoluteAccuracy, DEFAULT_MIN_ITERATIONS_COUNT, DEFAULT_MAX_ITERATIONS_COUNT)
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
        public IterativeLegendreGaussIntegrator(int n, int minimalIterationCount, int maximalIterationCount)
            : this(n, DEFAULT_RELATIVE_ACCURACY, DEFAULT_ABSOLUTE_ACCURACY, minimalIterationCount, maximalIterationCount)
        {
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override double DoIntegrate()
        {
            // Compute first estimate with a single step.
            double oldt = this.Stage(1);

            int n = 2;
            while (true)
            {
                // Improve integral with a larger number of steps.
                double t = this.Stage(n);

                // Estimate the error.
                double delta = FastMath.Abs(t - oldt);
                double limit = FastMath.Max(this.AbsoluteAccuracy, this.RelativeAccuracy * (FastMath.Abs(oldt) + FastMath.Abs(t)) * 0.5);

                // check convergence
                if (this.iterations.Count + 1 >= this.MinimalIterationCount && delta <= limit)
                {
                    return t;
                }

                // Prepare next iteration.
                double ratio = FastMath.Min(4, FastMath.Pow(delta / limit, 0.5 / this.numberOfPoints));
                n = FastMath.Max((int)(ratio * n), n + 1);
                oldt = t;
                this.iterations.IncrementCount();
            }
        }

        /// <summary>
        /// Compute the n-th stage integral.
        /// </summary>
        /// <param name="n"> Number of steps. </param>
        /// <returns> the value of n-th stage integral. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximum number of evaluations
        /// is exceeded. </exception>
        private double Stage(int n)
        {
            // Function to be integrated is stored in the base class.
            UnivariateFunction f = new UnivariateFunctionAnonymousInnerClassHelper(this);

            double min = this.Min;
            double max = this.Max;
            double step = (max - min) / n;

            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                // Integrate over each sub-interval [a, b].
                double a = min + i * step;
                double b = a + step;
                GaussIntegrator g = FACTORY.Legendre(this.numberOfPoints, a, b);
#if false
                GaussIntegrator g = FACTORY.LegendreHighPrecision(this.numberOfPoints, a, b);
#endif
                sum += g.Integrate(f);
            }

            return sum;
        }

        private class UnivariateFunctionAnonymousInnerClassHelper : UnivariateFunction
        {
            private readonly IterativeLegendreGaussIntegrator outerInstance;

            public UnivariateFunctionAnonymousInnerClassHelper(IterativeLegendreGaussIntegrator outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public virtual double Value(double x)
            {
                return this.outerInstance.ComputeObjectiveValue(x);
            }
        }
    }
}