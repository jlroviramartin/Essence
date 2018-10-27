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

namespace org.apache.commons.math3.analysis.solvers
{

    using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
    using NoBracketingException = org.apache.commons.math3.exception.NoBracketingException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;
    using NumberIsTooLargeException = org.apache.commons.math3.exception.NumberIsTooLargeException;
    using NullArgumentException = org.apache.commons.math3.exception.NullArgumentException;
    using IntegerSequence = org.apache.commons.math3.util.IntegerSequence;
    using MathUtils = org.apache.commons.math3.util.MathUtils;

    /// <summary>
    /// Provide a default implementation for several functions useful to generic
    /// solvers.
    /// The default values for relative and function tolerances are 1e-14
    /// and 1e-15, respectively. It is however highly recommended to not
    /// rely on the default, but rather carefully consider values that match
    /// user's expectations, as well as the specifics of each implementation.
    /// </summary>
    /// @param <FUNC> Type of function to solve.
    /// 
    /// @since 2.0 </param>
    public abstract class BaseAbstractUnivariateSolver<FUNC> : BaseUnivariateSolver<FUNC> where FUNC : org.apache.commons.math3.analysis.UnivariateFunction
    {
        /// <summary>
        /// Default relative accuracy. </summary>
        private const double DEFAULT_RELATIVE_ACCURACY = 1e-14;
        /// <summary>
        /// Default function value accuracy. </summary>
        private const double DEFAULT_FUNCTION_VALUE_ACCURACY = 1e-15;
        /// <summary>
        /// Function value accuracy. </summary>
        private readonly double functionValueAccuracy;
        /// <summary>
        /// Absolute accuracy. </summary>
        private readonly double absoluteAccuracy;
        /// <summary>
        /// Relative accuracy. </summary>
        private readonly double relativeAccuracy;
        /// <summary>
        /// Evaluations counter. </summary>
        private IntegerSequence.Incrementor evaluations;
        /// <summary>
        /// Lower end of search interval. </summary>
        private double searchMin;
        /// <summary>
        /// Higher end of search interval. </summary>
        private double searchMax;
        /// <summary>
        /// Initial guess. </summary>
        private double searchStart;
        /// <summary>
        /// Function to solve. </summary>
        private FUNC function;

        /// <summary>
        /// Construct a solver with given absolute accuracy.
        /// </summary>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseAbstractUnivariateSolver(final double absoluteAccuracy)
        protected internal BaseAbstractUnivariateSolver(double absoluteAccuracy) : this(DEFAULT_RELATIVE_ACCURACY, absoluteAccuracy, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {
        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy"> Maximum relative error. </param>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseAbstractUnivariateSolver(final double relativeAccuracy, final double absoluteAccuracy)
        protected internal BaseAbstractUnivariateSolver(double relativeAccuracy, double absoluteAccuracy) : this(relativeAccuracy, absoluteAccuracy, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {
        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy"> Maximum relative error. </param>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
        /// <param name="functionValueAccuracy"> Maximum function value error. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseAbstractUnivariateSolver(final double relativeAccuracy, final double absoluteAccuracy, final double functionValueAccuracy)
        protected internal BaseAbstractUnivariateSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy)
        {
            this.absoluteAccuracy = absoluteAccuracy;
            this.relativeAccuracy = relativeAccuracy;
            this.functionValueAccuracy = functionValueAccuracy;
            this.evaluations = IntegerSequence.Incrementor.Create();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int GetMaxEvaluations()
        {
            return evaluations.GetMaximalCount();
        }
        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int GetEvaluations()
        {
            return evaluations.GetCount();
        }
        /// <returns> the lower end of the search interval. </returns>
        public virtual double GetMin()
        {
            return searchMin;
        }
        /// <returns> the higher end of the search interval. </returns>
        public virtual double GetMax()
        {
            return searchMax;
        }
        /// <returns> the initial guess. </returns>
        public virtual double GetStartValue()
        {
            return searchStart;
        }
        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual double GetAbsoluteAccuracy()
        {
            return absoluteAccuracy;
        }
        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual double GetRelativeAccuracy()
        {
            return relativeAccuracy;
        }
        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual double GetFunctionValueAccuracy()
        {
            return functionValueAccuracy;
        }

        /// <summary>
        /// Compute the objective function value.
        /// </summary>
        /// <param name="point"> Point at which the objective function must be evaluated. </param>
        /// <returns> the objective function value at specified point. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximal number of evaluations
        /// is exceeded. </exception>
        protected internal virtual double ComputeObjectiveValue(double point)
        {
            IncrementEvaluationCount();
            return function.Value(point);
        }

        /// <summary>
        /// Prepare for computation.
        /// Subclasses must call this method if they override any of the
        /// {@code solve} methods.
        /// </summary>
        /// <param name="f"> Function to solve. </param>
        /// <param name="min"> Lower bound for the interval. </param>
        /// <param name="max"> Upper bound for the interval. </param>
        /// <param name="startValue"> Start value to use. </param>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <exception cref="NullArgumentException"> if f is null </exception>
        protected internal virtual void Setup(int maxEval, FUNC f, double min, double max, double startValue)
        {
            // Checks.
            MathUtils.CheckNotNull(f);

            // Reset.
            searchMin = min;
            searchMax = max;
            searchStart = startValue;
            function = f;
            evaluations = evaluations.WithMaximalCount(maxEval).WithStart(0);
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double Solve(int maxEval, FUNC f, double min, double max, double startValue)
        {
            // Initialization.
            Setup(maxEval, f, min, max, startValue);

            // Perform computation.
            return DoSolve();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double Solve(int maxEval, FUNC f, double min, double max)
        {
            return Solve(maxEval, f, min, max, min + 0.5 * (max - min));
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double Solve(int maxEval, FUNC f, double startValue)
        {
            return Solve(maxEval, f, double.NaN, double.NaN, startValue);
        }

        /// <summary>
        /// Method for implementing actual optimization algorithms in derived
        /// classes.
        /// </summary>
        /// <returns> the root. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximal number of evaluations
        /// is exceeded. </exception>
        /// <exception cref="NoBracketingException"> if the initial search interval does not bracket
        /// a root and the solver requires it. </exception>
        protected internal abstract double DoSolve();

        /// <summary>
        /// Check whether the function takes opposite signs at the endpoints.
        /// </summary>
        /// <param name="lower"> Lower endpoint. </param>
        /// <param name="upper"> Upper endpoint. </param>
        /// <returns> {@code true} if the function values have opposite signs at the
        /// given points. </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected boolean isBracketing(final double lower, final double upper)
        protected internal virtual bool IsBracketing(double lower, double upper)
        {
            return UnivariateSolverUtils.IsBracketing(function, lower, upper);
        }

        /// <summary>
        /// Check whether the arguments form a (strictly) increasing sequence.
        /// </summary>
        /// <param name="start"> First number. </param>
        /// <param name="mid"> Second number. </param>
        /// <param name="end"> Third number. </param>
        /// <returns> {@code true} if the arguments form an increasing sequence. </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected boolean isSequence(final double start, final double mid, final double end)
        protected internal virtual bool IsSequence(double start, double mid, double end)
        {
            return UnivariateSolverUtils.IsSequence(start, mid, end);
        }

        /// <summary>
        /// Check that the endpoints specify an interval.
        /// </summary>
        /// <param name="lower"> Lower endpoint. </param>
        /// <param name="upper"> Upper endpoint. </param>
        /// <exception cref="NumberIsTooLargeException"> if {@code lower >= upper}. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void verifyInterval(final double lower, final double upper) throws org.apache.commons.math3.exception.NumberIsTooLargeException
        protected internal virtual void VerifyInterval(double lower, double upper)
        {
            UnivariateSolverUtils.VerifyInterval(lower, upper);
        }

        /// <summary>
        /// Check that {@code lower < initial < upper}.
        /// </summary>
        /// <param name="lower"> Lower endpoint. </param>
        /// <param name="initial"> Initial value. </param>
        /// <param name="upper"> Upper endpoint. </param>
        /// <exception cref="NumberIsTooLargeException"> if {@code lower >= initial} or
        /// {@code initial >= upper}. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void verifySequence(final double lower, final double initial, final double upper) throws org.apache.commons.math3.exception.NumberIsTooLargeException
        protected internal virtual void VerifySequence(double lower, double initial, double upper)
        {
            UnivariateSolverUtils.VerifySequence(lower, initial, upper);
        }

        /// <summary>
        /// Check that the endpoints specify an interval and the function takes
        /// opposite signs at the endpoints.
        /// </summary>
        /// <param name="lower"> Lower endpoint. </param>
        /// <param name="upper"> Upper endpoint. </param>
        /// <exception cref="NullArgumentException"> if the function has not been set. </exception>
        /// <exception cref="NoBracketingException"> if the function has the same sign at
        /// the endpoints. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void verifyBracketing(final double lower, final double upper) throws org.apache.commons.math3.exception.NullArgumentException, org.apache.commons.math3.exception.NoBracketingException
        protected internal virtual void VerifyBracketing(double lower, double upper)
        {
            UnivariateSolverUtils.VerifyBracketing(function, lower, upper);
        }

        /// <summary>
        /// Increment the evaluation count by one.
        /// Method <seealso cref="#computeObjectiveValue(double)"/> calls this method internally.
        /// It is provided for subclasses that do not exclusively use
        /// {@code computeObjectiveValue} to solve the function.
        /// See e.g. <seealso cref="AbstractUnivariateDifferentiableSolver"/>.
        /// </summary>
        /// <exception cref="TooManyEvaluationsException"> when the allowed number of function
        /// evaluations has been exhausted. </exception>
        protected internal virtual void IncrementEvaluationCount()
        {
            try
            {
                evaluations.Increment();
            }
            catch (MaxCountExceededException e)
            {
                throw new TooManyEvaluationsException(e.GetMax());
            }
        }
    }

}