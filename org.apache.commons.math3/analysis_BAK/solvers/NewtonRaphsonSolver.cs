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

    using DerivativeStructure = org.apache.commons.math3.analysis.differentiation.DerivativeStructure;
    using UnivariateDifferentiableFunction = org.apache.commons.math3.analysis.differentiation.UnivariateDifferentiableFunction;
    using FastMath = org.apache.commons.math3.util.FastMath;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;

    /// <summary>
    /// Implements <a href="http://mathworld.wolfram.com/NewtonsMethod.html">
    /// Newton's Method</a> for finding zeros of real univariate differentiable
    /// functions.
    /// 
    /// @since 3.1
    /// @version $Id: NewtonRaphsonSolver.java 1383441 2012-09-11 14:56:39Z luc $
    /// </summary>
    public class NewtonRaphsonSolver : AbstractUnivariateDifferentiableSolver
    {
        /// <summary>
        /// Default absolute accuracy. </summary>
        private const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// Construct a solver.
        /// </summary>
        public NewtonRaphsonSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public NewtonRaphsonSolver(double absoluteAccuracy) : base(absoluteAccuracy)
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
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public double solve(int maxEval, final org.apache.commons.math3.analysis.differentiation.UnivariateDifferentiableFunction f, final double min, final double max) throws org.apache.commons.math3.exception.TooManyEvaluationsException
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        public override double Solve(int maxEval, UnivariateDifferentiableFunction f, double min, double max)
        {
            return base.Solve(maxEval, f, UnivariateSolverUtils.Midpoint(min, max));
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override protected double doSolve() throws org.apache.commons.math3.exception.TooManyEvaluationsException
        protected internal override double DoSolve()
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double startValue = getStartValue();
            double startValue = StartValue;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absoluteAccuracy = getAbsoluteAccuracy();
            double absoluteAccuracy = AbsoluteAccuracy;

            double x0 = startValue;
            double x1;
            while (true)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.apache.commons.math3.analysis.differentiation.DerivativeStructure y0 = computeObjectiveValueAndDerivative(x0);
                DerivativeStructure y0 = ComputeObjectiveValueAndDerivative(x0);
                x1 = x0 - (y0.Value / y0.getPartialDerivative(1));
                if (FastMath.abs(x1 - x0) <= absoluteAccuracy)
                {
                    return x1;
                }

                x0 = x1;
            }
        }
    }

}