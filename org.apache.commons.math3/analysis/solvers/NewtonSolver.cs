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

namespace org.apache.commons.math3.analysis.solvers
{

    using FastMath = org.apache.commons.math3.util.FastMath;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;

    /// <summary>
    /// Implements <a href="http://mathworld.wolfram.com/NewtonsMethod.html">
    /// Newton's Method</a> for finding zeros of real univariate functions.
    /// <para>
    /// The function should be continuous but not necessarily smooth.</para>
    /// </summary>
    /// @deprecated as of 3.1, replaced by <seealso cref="NewtonRaphsonSolver"/> 
    [Obsolete("as of 3.1, replaced by <seealso cref=\"NewtonRaphsonSolver\"/>")]
    public class NewtonSolver : AbstractDifferentiableUnivariateSolver
    {
        /// <summary>
        /// Default absolute accuracy. </summary>
        private const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// Construct a solver.
        /// </summary>
        public NewtonSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public NewtonSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
        }

        /// <summary>
        /// Find a zero near the midpoint of {@code min} and {@code max}.
        /// </summary>
        /// <param name="f"> Function to solve. </param>
        /// <param name="min"> Lower bound for the interval. </param>
        /// <param name="max"> Upper bound for the interval. </param>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <returns> the value where the function is zero. </returns>
        /// <exception cref="org.apache.commons.math3.exception.TooManyEvaluationsException">
        /// if the maximum evaluation count is exceeded. </exception>
        /// <exception cref="org.apache.commons.math3.exception.NumberIsTooLargeException">
        /// if {@code min >= max}. </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double solve(int maxEval, final org.apache.commons.math3.analysis.DifferentiableUnivariateFunction f, final double min, final double max) throws org.apache.commons.math3.exception.TooManyEvaluationsException
        public virtual double Solve(int maxEval, DifferentiableUnivariateFunction f, double min, double max)
        {
            return base.Solve(maxEval, f, UnivariateSolverUtils.Midpoint(min, max));
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        protected internal override double DoSolve()
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double startValue = getStartValue();
            double startValue = GetStartValue();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absoluteAccuracy = getAbsoluteAccuracy();
            double absoluteAccuracy = GetAbsoluteAccuracy();

            double x0 = startValue;
            double x1;
            while (true)
            {
                x1 = x0 - (ComputeObjectiveValue(x0) / ComputeDerivativeObjectiveValue(x0));
                if (FastMath.Abs(x1 - x0) <= absoluteAccuracy)
                {
                    return x1;
                }

                x0 = x1;
            }
        }
    }

}