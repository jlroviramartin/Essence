/// Apache Commons Math 3.6.1
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
namespace org.apache.commons.math3.analysis.integration
{

    using UnivariateSolverUtils = org.apache.commons.math3.analysis.solvers.UnivariateSolverUtils;
    using MathIllegalArgumentException = org.apache.commons.math3.exception.MathIllegalArgumentException;
    using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
    using NotStrictlyPositiveException = org.apache.commons.math3.exception.NotStrictlyPositiveException;
    using NullArgumentException = org.apache.commons.math3.exception.NullArgumentException;
    using NumberIsTooSmallException = org.apache.commons.math3.exception.NumberIsTooSmallException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;
    using IntegerSequence = org.apache.commons.math3.util.IntegerSequence;
    using MathUtils = org.apache.commons.math3.util.MathUtils;

    /// <summary>
    /// Provide a default implementation for several generic functions.
    /// 
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
        /// @deprecated as of 3.6, this field has been replaced with <seealso cref="#incrementCount()"/> 
        [Obsolete("as of 3.6, this field has been replaced with <seealso cref=\"#incrementCount()\"/>")]
        protected internal org.apache.commons.math3.util.Incrementor iterations;

        /// <summary>
        /// The iteration count. </summary>
        private IntegerSequence.Incrementor count;

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
        private IntegerSequence.Incrementor evaluations;

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
        /// 
        /// </para>
        /// </summary>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations </param>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseAbstractUnivariateIntegrator(final double relativeAccuracy, final double absoluteAccuracy, final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException
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
            this.count = IntegerSequence.Incrementor.Create().WithMaximalCount(maximalIterationCount);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("deprecation") org.apache.commons.math3.util.Incrementor wrapped = org.apache.commons.math3.util.Incrementor.wrap(count);
            org.apache.commons.math3.util.Incrementor wrapped = org.apache.commons.math3.util.Incrementor.Wrap(count);
            this.iterations = wrapped;

            // prepare evaluations counter, but do not set it yet
            evaluations = IntegerSequence.Incrementor.Create();

        }

        /// <summary>
        /// Construct an integrator with given accuracies. </summary>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseAbstractUnivariateIntegrator(final double relativeAccuracy, final double absoluteAccuracy)
        protected internal BaseAbstractUnivariateIntegrator(double relativeAccuracy, double absoluteAccuracy) : this(relativeAccuracy, absoluteAccuracy, DEFAULT_MIN_ITERATIONS_COUNT, DEFAULT_MAX_ITERATIONS_COUNT)
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
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseAbstractUnivariateIntegrator(final int minimalIterationCount, final int maximalIterationCount) throws org.apache.commons.math3.exception.NotStrictlyPositiveException, org.apache.commons.math3.exception.NumberIsTooSmallException
        protected internal BaseAbstractUnivariateIntegrator(int minimalIterationCount, int maximalIterationCount) : this(DEFAULT_RELATIVE_ACCURACY, DEFAULT_ABSOLUTE_ACCURACY, minimalIterationCount, maximalIterationCount)
        {
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double GetRelativeAccuracy()
        {
            return relativeAccuracy;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double GetAbsoluteAccuracy()
        {
            return absoluteAccuracy;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int GetMinimalIterationCount()
        {
            return minimalIterationCount;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int GetMaximalIterationCount()
        {
            return count.GetMaximalCount();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int GetEvaluations()
        {
            return evaluations.GetCount();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int GetIterations()
        {
            return count.GetCount();
        }

        /// <summary>
        /// Increment the number of iterations. </summary>
        /// <exception cref="MaxCountExceededException"> if the number of iterations
        /// exceeds the allowed maximum number </exception>
        protected internal virtual void IncrementCount()
        {
            count.Increment();
        }

        /// <returns> the lower bound. </returns>
        protected internal virtual double GetMin()
        {
            return min;
        }
        /// <returns> the upper bound. </returns>
        protected internal virtual double GetMax()
        {
            return max;
        }

        /// <summary>
        /// Compute the objective function value.
        /// </summary>
        /// <param name="point"> Point at which the objective function must be evaluated. </param>
        /// <returns> the objective function value at specified point. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximal number of function
        /// evaluations is exceeded. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected double computeObjectiveValue(final double point) throws org.apache.commons.math3.exception.TooManyEvaluationsException
        protected internal virtual double ComputeObjectiveValue(double point)
        {
            try
            {
                evaluations.Increment();
            }
            catch (MaxCountExceededException e)
            {
                throw new TooManyEvaluationsException(e.GetMax());
            }
            return function.Value(point);
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
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void setup(final int maxEval, final org.apache.commons.math3.analysis.UnivariateFunction f, final double lower, final double upper) throws org.apache.commons.math3.exception.NullArgumentException, org.apache.commons.math3.exception.MathIllegalArgumentException
        protected internal virtual void Setup(int maxEval, UnivariateFunction f, double lower, double upper)
        {

            // Checks.
            MathUtils.CheckNotNull(f);
            UnivariateSolverUtils.VerifyInterval(lower, upper);

            // Reset.
            min = lower;
            max = upper;
            function = f;
            evaluations = evaluations.WithMaximalCount(maxEval).WithStart(0);
            count = count.WithStart(0);

        }

        /// <summary>
        /// {@inheritDoc} </summary>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public double integrate(final int maxEval, final org.apache.commons.math3.analysis.UnivariateFunction f, final double lower, final double upper) throws org.apache.commons.math3.exception.TooManyEvaluationsException, org.apache.commons.math3.exception.MaxCountExceededException, org.apache.commons.math3.exception.MathIllegalArgumentException, org.apache.commons.math3.exception.NullArgumentException
        public virtual double Integrate(int maxEval, UnivariateFunction f, double lower, double upper)
        {

            // Initialization.
            Setup(maxEval, f, lower, upper);

            // Perform computation.
            return DoIntegrate();

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