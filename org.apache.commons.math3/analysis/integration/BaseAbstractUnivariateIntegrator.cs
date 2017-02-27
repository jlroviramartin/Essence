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
using org.apache.commons.math3.analysis.solvers;
using org.apache.commons.math3.analysis.util;
using org.apache.commons.math3.util;

namespace org.apache.commons.math3.analysis.integration
{
    /// <summary>
    /// Provide a default implementation for several generic functions.
    /// 
    /// @version $Id: BaseAbstractUnivariateIntegrator.java 1455194 2013-03-11 15:45:54Z luc $
    /// @since 1.2
    /// </summary>
    public abstract class BaseAbstractUnivariateIntegrator : UnivariateIntegrator
    {
        /// <summary>
        /// Default absolute accuracy. </summary>
        public const double DEFAULT_ABSOLUTE_ACCURACY = 1.0e-15;

        /// <summary>
        /// Default relative accuracy. </summary>
        public const double DEFAULT_RELATIVE_ACCURACY = 1.0e-6;

        /// <summary>
        /// Default minimal iteration count. </summary>
        public const int DEFAULT_MIN_ITERATIONS_COUNT = 3;

        /// <summary>
        /// Default maximal iteration count. </summary>
        public static readonly int DEFAULT_MAX_ITERATIONS_COUNT = int.MaxValue;

        /// <summary>
        /// The iteration count. </summary>
        protected internal readonly Incrementor iterations;

        /// <summary>
        /// Maximum absolute error. </summary>
        private readonly double absoluteAccuracy;

        /// <summary>
        /// Maximum relative error. </summary>
        private readonly double relativeAccuracy;

        /// <summary>
        /// minimum number of iterations </summary>
        private readonly int minimalIterationCount;

        /// <summary>
        /// The functions evaluation count. </summary>
        private readonly Incrementor evaluations;

        /// <summary>
        /// Function to integrate. </summary>
        private UnivariateFunction function;

        /// <summary>
        /// Lower bound for the interval. </summary>
        private double min;

        /// <summary>
        /// Upper bound for the interval. </summary>
        private double max;

        /// <summary>
        /// Construct an integrator with given accuracies and iteration counts.
        /// <para>
        /// The meanings of the various parameters are:
        /// <ul>
        ///   <li>relative accuracy:
        ///       this is used to stop iterations if the absolute accuracy can't be
        ///       achieved due to large values or short mantissa length. If this
        ///       should be the primary criterion for convergence rather then a
        ///       safety measure, set the absolute accuracy to a ridiculously small value,
        ///       like <seealso cref="org.apache.commons.math3.util.Precision#SAFE_MIN Precision.SAFE_MIN"/>.</li>
        ///   <li>absolute accuracy:
        ///       The default is usually chosen so that results in the interval
        ///       -10..-0.1 and +0.1..+10 can be found with a reasonable accuracy. If the
        ///       expected absolute value of your results is of much smaller magnitude, set
        ///       this to a smaller value.</li>
        ///   <li>minimum number of iterations:
        ///       minimal iteration is needed to avoid false early convergence, e.g.
        ///       the sample points happen to be zeroes of the function. Users can
        ///       use the default value or choose one that they see as appropriate.</li>
        ///   <li>maximum number of iterations:
        ///       usually a high iteration count indicates convergence problems. However,
        ///       the "reasonable value" varies widely for different algorithms. Users are
        ///       advised to use the default value supplied by the algorithm.</li>
        /// </ul>
        /// </para> </summary>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        protected internal BaseAbstractUnivariateIntegrator(double relativeAccuracy, double absoluteAccuracy, int minimalIterationCount, int maximalIterationCount)
        {
            // accuracy settings
            this.relativeAccuracy = relativeAccuracy;
            this.absoluteAccuracy = absoluteAccuracy;

            // iterations count settings
            if (minimalIterationCount <= 0)
            {
                throw new NotStrictlyPositiveException(minimalIterationCount);
            }
            if (maximalIterationCount <= minimalIterationCount)
            {
                throw new NumberIsTooSmallException(maximalIterationCount, minimalIterationCount, false);
            }
            this.minimalIterationCount = minimalIterationCount;
            this.iterations = new Incrementor();
            this.iterations.MaximalCount = maximalIterationCount;

            // prepare evaluations counter, but do not set it yet
            this.evaluations = new Incrementor();
        }

        /// <summary>
        /// Construct an integrator with given accuracies. </summary>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
        protected internal BaseAbstractUnivariateIntegrator(double relativeAccuracy, double absoluteAccuracy)
            : this(relativeAccuracy, absoluteAccuracy, DEFAULT_MIN_ITERATIONS_COUNT, DEFAULT_MAX_ITERATIONS_COUNT)
        {
        }

        /// <summary>
        /// Construct an integrator with given iteration counts. </summary>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        protected internal BaseAbstractUnivariateIntegrator(int minimalIterationCount, int maximalIterationCount)
            : this(DEFAULT_RELATIVE_ACCURACY, DEFAULT_ABSOLUTE_ACCURACY, minimalIterationCount, maximalIterationCount)
        {
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double RelativeAccuracy
        {
            get { return this.relativeAccuracy; }
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double AbsoluteAccuracy
        {
            get { return this.absoluteAccuracy; }
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int MinimalIterationCount
        {
            get { return this.minimalIterationCount; }
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int MaximalIterationCount
        {
            get { return this.iterations.MaximalCount; }
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int Evaluations
        {
            get { return this.evaluations.Count; }
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int Iterations
        {
            get { return this.iterations.Count; }
        }

        /// <returns> the lower bound. </returns>
        protected internal virtual double Min
        {
            get { return this.min; }
        }

        /// <returns> the upper bound. </returns>
        protected internal virtual double Max
        {
            get { return this.max; }
        }

        /// <summary>
        /// Compute the objective function value.
        /// </summary>
        /// <param name="point"> Point at which the objective function must be evaluated. </param>
        /// <returns> the objective function value at specified point. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximal number of function
        /// evaluations is exceeded. </exception>
        protected internal virtual double ComputeObjectiveValue(double point)
        {
            try
            {
                this.evaluations.IncrementCount();
            }
            catch (MaxCountExceededException e)
            {
                throw new TooManyEvaluationsException(e.Max);
            }
            return this.function.Value(point);
        }

        /// <summary>
        /// Prepare for computation.
        /// Subclasses must call this method if they override any of the
        /// {@code solve} methods.
        /// </summary>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <param name="f"> the integrand function </param>
        /// <param name="lower"> the min bound for the interval </param>
        /// <param name="upper"> the upper bound for the interval </param>
        /// <exception cref="NullArgumentException"> if {@code f} is {@code null}. </exception>
        /// <exception cref="MathIllegalArgumentException"> if {@code min >= max}. </exception>
        protected internal virtual void Setup(int maxEval, UnivariateFunction f, double lower, double upper)
        {
            // Checks.
            MyUtils.CheckNotNull(f);
            UnivariateSolverUtils.VerifyInterval(lower, upper);

            // Reset.
            this.min = lower;
            this.max = upper;
            this.function = f;
            this.evaluations.MaximalCount = maxEval;
            this.evaluations.ResetCount();
            this.iterations.ResetCount();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double Integrate(int maxEval, UnivariateFunction f, double lower, double upper)
        {
            // Initialization.
            this.Setup(maxEval, f, lower, upper);

            // Perform computation.
            return this.DoIntegrate();
        }

        /// <summary>
        /// Method for implementing actual integration algorithms in derived
        /// classes.
        /// </summary>
        /// <returns> the root. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximal number of evaluations
        /// is exceeded. </exception>
        /// <exception cref="MaxCountExceededException"> if the maximum iteration count is exceeded
        /// or the integrator detects convergence problems otherwise </exception>
        protected internal abstract double DoIntegrate();
    }
}