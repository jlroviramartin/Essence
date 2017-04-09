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
    /// <summary>
    /// Interface for (univariate real) rootfinding algorithms.
    /// Implementations will search for only one zero in the given interval.
    /// 
    /// This class is not intended for use outside of the Apache Commons Math
    /// library, regular user should rely on more specific interfaces like
    /// <seealso cref="UnivariateSolver"/>, <seealso cref="PolynomialSolver"/> or {@link
    /// DifferentiableUnivariateSolver}. </summary>
    /// @param <FUNC> Type of function to solve.
    /// 
    /// @since 3.0
    /// @version $Id: BaseUnivariateSolver.java 1455194 2013-03-11 15:45:54Z luc $ </param>
    /// <seealso cref= UnivariateSolver </seealso>
    /// <seealso cref= PolynomialSolver </seealso>
    /// <seealso cref= DifferentiableUnivariateSolver </seealso>
    public interface BaseUnivariateSolver<FUNC> where FUNC : UnivariateFunction
    {
        /// <summary>
        /// Get the maximum number of function evaluations.
        /// </summary>
        /// <returns> the maximum number of function evaluations. </returns>
        int MaxEvaluations { get; }

        /// <summary>
        /// Get the number of evaluations of the objective function.
        /// The number of evaluations corresponds to the last call to the
        /// {@code optimize} method. It is 0 if the method has not been
        /// called yet.
        /// </summary>
        /// <returns> the number of evaluations of the objective function. </returns>
        int Evaluations { get; }

        /// <summary>
        /// Get the absolute accuracy of the solver.  Solutions returned by the
        /// solver should be accurate to this tolerance, i.e., if &epsilon; is the
        /// absolute accuracy of the solver and {@code v} is a value returned by
        /// one of the {@code solve} methods, then a root of the function should
        /// exist somewhere in the interval ({@code v} - &epsilon;, {@code v} + &epsilon;).
        /// </summary>
        /// <returns> the absolute accuracy. </returns>
        double AbsoluteAccuracy { get; }

        /// <summary>
        /// Get the relative accuracy of the solver.  The contract for relative
        /// accuracy is the same as <seealso cref="#getAbsoluteAccuracy()"/>, but using
        /// relative, rather than absolute error.  If &rho; is the relative accuracy
        /// configured for a solver and {@code v} is a value returned, then a root
        /// of the function should exist somewhere in the interval
        /// ({@code v} - &rho; {@code v}, {@code v} + &rho; {@code v}).
        /// </summary>
        /// <returns> the relative accuracy. </returns>
        double RelativeAccuracy { get; }

        /// <summary>
        /// Get the function value accuracy of the solver.  If {@code v} is
        /// a value returned by the solver for a function {@code f},
        /// then by contract, {@code |f(v)|} should be less than or equal to
        /// the function value accuracy configured for the solver.
        /// </summary>
        /// <returns> the function value accuracy. </returns>
        double FunctionValueAccuracy { get; }

        /// <summary>
        /// Solve for a zero root in the given interval.
        /// A solver may require that the interval brackets a single zero root.
        /// Solvers that do require bracketing should be able to handle the case
        /// where one of the endpoints is itself a root.
        /// </summary>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <param name="f"> Function to solve. </param>
        /// <param name="min"> Lower bound for the interval. </param>
        /// <param name="max"> Upper bound for the interval. </param>
        /// <returns> a value where the function is zero. </returns>
        /// <exception cref="MathIllegalArgumentException">
        /// if the arguments do not satisfy the requirements specified by the solver. </exception>
        /// <exception cref="TooManyEvaluationsException"> if
        /// the allowed number of evaluations is exceeded. </exception>
        double Solve(int maxEval, FUNC f, double min, double max);

        /// <summary>
        /// Solve for a zero in the given interval, start at {@code startValue}.
        /// A solver may require that the interval brackets a single zero root.
        /// Solvers that do require bracketing should be able to handle the case
        /// where one of the endpoints is itself a root.
        /// </summary>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <param name="f"> Function to solve. </param>
        /// <param name="min"> Lower bound for the interval. </param>
        /// <param name="max"> Upper bound for the interval. </param>
        /// <param name="startValue"> Start value to use. </param>
        /// <returns> a value where the function is zero. </returns>
        /// <exception cref="MathIllegalArgumentException">
        /// if the arguments do not satisfy the requirements specified by the solver. </exception>
        /// <exception cref="TooManyEvaluationsException"> if
        /// the allowed number of evaluations is exceeded. </exception>
        double Solve(int maxEval, FUNC f, double min, double max, double startValue);

        /// <summary>
        /// Solve for a zero in the vicinity of {@code startValue}.
        /// </summary>
        /// <param name="f"> Function to solve. </param>
        /// <param name="startValue"> Start value to use. </param>
        /// <returns> a value where the function is zero. </returns>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <exception cref="org.apache.commons.math3.exception.MathIllegalArgumentException">
        /// if the arguments do not satisfy the requirements specified by the solver. </exception>
        /// <exception cref="org.apache.commons.math3.exception.TooManyEvaluationsException"> if
        /// the allowed number of evaluations is exceeded. </exception>
        double Solve(int maxEval, FUNC f, double startValue);
    }
}