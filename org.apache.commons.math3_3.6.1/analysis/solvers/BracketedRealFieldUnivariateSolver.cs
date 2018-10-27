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


    /// <summary>
    /// Interface for {@link UnivariateSolver (univariate real) root-finding
    /// algorithms} that maintain a bracketed solution. There are several advantages
    /// to having such root-finding algorithms:
    /// <ul>
    ///  <li>The bracketed solution guarantees that the root is kept within the
    ///      interval. As such, these algorithms generally also guarantee
    ///      convergence.</li>
    ///  <li>The bracketed solution means that we have the opportunity to only
    ///      return roots that are greater than or equal to the actual root, or
    ///      are less than or equal to the actual root. That is, we can control
    ///      whether under-approximations and over-approximations are
    ///      <seealso cref="AllowedSolution allowed solutions"/>. Other root-finding
    ///      algorithms can usually only guarantee that the solution (the root that
    ///      was found) is around the actual root.</li>
    /// </ul>
    /// 
    /// <para>For backwards compatibility, all root-finding algorithms must have
    /// <seealso cref="AllowedSolution#ANY_SIDE ANY_SIDE"/> as default for the allowed
    /// solutions.</para>
    /// </summary>
    /// <seealso cref= AllowedSolution </seealso>
    /// @param <T> the type of the field elements
    /// @since 3.6 </param>
    public interface BracketedRealFieldUnivariateSolver<T> where T : org.apache.commons.math3.RealFieldElement<T>
    {

        /// <summary>
        /// Get the maximum number of function evaluations.
        /// </summary>
        /// <returns> the maximum number of function evaluations. </returns>
        int GetMaxEvaluations();

        /// <summary>
        /// Get the number of evaluations of the objective function.
        /// The number of evaluations corresponds to the last call to the
        /// {@code optimize} method. It is 0 if the method has not been
        /// called yet.
        /// </summary>
        /// <returns> the number of evaluations of the objective function. </returns>
        int GetEvaluations();

        /// <summary>
        /// Get the absolute accuracy of the solver.  Solutions returned by the
        /// solver should be accurate to this tolerance, i.e., if &epsilon; is the
        /// absolute accuracy of the solver and {@code v} is a value returned by
        /// one of the {@code solve} methods, then a root of the function should
        /// exist somewhere in the interval ({@code v} - &epsilon;, {@code v} + &epsilon;).
        /// </summary>
        /// <returns> the absolute accuracy. </returns>
        T GetAbsoluteAccuracy();

        /// <summary>
        /// Get the relative accuracy of the solver.  The contract for relative
        /// accuracy is the same as <seealso cref="#getAbsoluteAccuracy()"/>, but using
        /// relative, rather than absolute error.  If &rho; is the relative accuracy
        /// configured for a solver and {@code v} is a value returned, then a root
        /// of the function should exist somewhere in the interval
        /// ({@code v} - &rho; {@code v}, {@code v} + &rho; {@code v}).
        /// </summary>
        /// <returns> the relative accuracy. </returns>
        T GetRelativeAccuracy();

        /// <summary>
        /// Get the function value accuracy of the solver.  If {@code v} is
        /// a value returned by the solver for a function {@code f},
        /// then by contract, {@code |f(v)|} should be less than or equal to
        /// the function value accuracy configured for the solver.
        /// </summary>
        /// <returns> the function value accuracy. </returns>
        T GetFunctionValueAccuracy();

        /// <summary>
        /// Solve for a zero in the given interval.
        /// A solver may require that the interval brackets a single zero root.
        /// Solvers that do require bracketing should be able to handle the case
        /// where one of the endpoints is itself a root.
        /// </summary>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <param name="f"> Function to solve. </param>
        /// <param name="min"> Lower bound for the interval. </param>
        /// <param name="max"> Upper bound for the interval. </param>
        /// <param name="allowedSolution"> The kind of solutions that the root-finding algorithm may
        /// accept as solutions. </param>
        /// <returns> A value where the function is zero. </returns>
        /// <exception cref="org.apache.commons.math3.exception.MathIllegalArgumentException">
        /// if the arguments do not satisfy the requirements specified by the solver. </exception>
        /// <exception cref="org.apache.commons.math3.exception.TooManyEvaluationsException"> if
        /// the allowed number of evaluations is exceeded. </exception>
        T Solve(int maxEval, RealFieldUnivariateFunction<T> f, T min, T max, AllowedSolution allowedSolution);

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
        /// <param name="allowedSolution"> The kind of solutions that the root-finding algorithm may
        /// accept as solutions. </param>
        /// <returns> A value where the function is zero. </returns>
        /// <exception cref="org.apache.commons.math3.exception.MathIllegalArgumentException">
        /// if the arguments do not satisfy the requirements specified by the solver. </exception>
        /// <exception cref="org.apache.commons.math3.exception.TooManyEvaluationsException"> if
        /// the allowed number of evaluations is exceeded. </exception>
        T Solve(int maxEval, RealFieldUnivariateFunction<T> f, T min, T max, T startValue, AllowedSolution allowedSolution);

    }

}