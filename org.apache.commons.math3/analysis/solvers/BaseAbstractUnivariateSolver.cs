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
using org.apache.commons.math3.util;
using org.apache.commons.math3.analysis.exception;
using MathUtils = System.Math;

namespace org.apache.commons.math3.analysis.solvers
{
    /// <summary>
    /// Provide a default implementation for several functions useful to generic
    /// solvers.
    /// </summary>
    /// @param <FUNC> Type of function to solve.
    /// 
    /// @since 2.0
    /// @version $Id: BaseAbstractUnivariateSolver.java 1455194 2013-03-11 15:45:54Z luc $ </param>
    public abstract class BaseAbstractUnivariateSolver<FUNC> : BaseUnivariateSolver<FUNC> where FUNC : UnivariateFunction
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
        private readonly Incrementor evaluations = new Incrementor();

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
        protected internal BaseAbstractUnivariateSolver(double absoluteAccuracy)
            : this(DEFAULT_RELATIVE_ACCURACY, absoluteAccuracy, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {
        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy"> Maximum relative error. </param>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
        protected internal BaseAbstractUnivariateSolver(double relativeAccuracy, double absoluteAccuracy)
            : this(relativeAccuracy, absoluteAccuracy, DEFAULT_FUNCTION_VALUE_ACCURACY)
        {
        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy"> Maximum relative error. </param>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
        /// <param name="functionValueAccuracy"> Maximum function value error. </param>
        protected internal BaseAbstractUnivariateSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy)
        {
            this.absoluteAccuracy = absoluteAccuracy;
            this.relativeAccuracy = relativeAccuracy;
            this.functionValueAccuracy = functionValueAccuracy;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int MaxEvaluations
        {
            get { return this.evaluations.MaximalCount; }
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual int Evaluations
        {
            get { return this.evaluations.Count; }
        }

        /// <returns> the lower end of the search interval. </returns>
        public virtual double Min
        {
            get { return this.searchMin; }
        }

        /// <returns> the higher end of the search interval. </returns>
        public virtual double Max
        {
            get { return this.searchMax; }
        }

        /// <returns> the initial guess. </returns>
        public virtual double StartValue
        {
            get { return this.searchStart; }
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual double AbsoluteAccuracy
        {
            get { return this.absoluteAccuracy; }
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual double RelativeAccuracy
        {
            get { return this.relativeAccuracy; }
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public virtual double FunctionValueAccuracy
        {
            get { return this.functionValueAccuracy; }
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
            this.IncrementEvaluationCount();
            return this.function.Value(point);
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
            MyUtils.CheckNotNull(f);

            // Reset.
            this.searchMin = min;
            this.searchMax = max;
            this.searchStart = startValue;
            this.function = f;
            this.evaluations.MaximalCount = maxEval;
            this.evaluations.ResetCount();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double Solve(int maxEval, FUNC f, double min, double max, double startValue)
        {
            // Initialization.
            this.Setup(maxEval, f, min, max, startValue);

            // Perform computation.
            return this.DoSolve();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double Solve(int maxEval, FUNC f, double min, double max)
        {
            return this.Solve(maxEval, f, min, max, min + 0.5 * (max - min));
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual double Solve(int maxEval, FUNC f, double startValue)
        {
            return this.Solve(maxEval, f, double.NaN, double.NaN, startValue);
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
        protected internal virtual bool IsBracketing(double lower, double upper)
        {
            return UnivariateSolverUtils.IsBracketing(this.function, lower, upper);
        }

        /// <summary>
        /// Check whether the arguments form a (strictly) increasing sequence.
        /// </summary>
        /// <param name="start"> First number. </param>
        /// <param name="mid"> Second number. </param>
        /// <param name="end"> Third number. </param>
        /// <returns> {@code true} if the arguments form an increasing sequence. </returns>
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
        protected internal virtual void VerifyBracketing(double lower, double upper)
        {
            UnivariateSolverUtils.VerifyBracketing(this.function, lower, upper);
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
                this.evaluations.IncrementCount();
            }
            catch (MaxCountExceededException e)
            {
                throw new TooManyEvaluationsException(e.Max);
            }
        }
    }
}