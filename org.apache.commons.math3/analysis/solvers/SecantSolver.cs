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

    using FastMath = org.apache.commons.math3.util.FastMath;
    using NoBracketingException = org.apache.commons.math3.exception.NoBracketingException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;

    /// <summary>
    /// Implements the <em>Secant</em> method for root-finding (approximating a
    /// zero of a univariate real function). The solution that is maintained is
    /// not bracketed, and as such convergence is not guaranteed.
    /// 
    /// <para>Implementation based on the following article: M. Dowell and P. Jarratt,
    /// <em>A modified regula falsi method for computing the root of an
    /// equation</em>, BIT Numerical Mathematics, volume 11, number 2,
    /// pages 168-174, Springer, 1971.</para>
    /// 
    /// <para>Note that since release 3.0 this class implements the actual
    /// <em>Secant</em> algorithm, and not a modified one. As such, the 3.0 version
    /// is not backwards compatible with previous versions. To use an algorithm
    /// similar to the pre-3.0 releases, use the
    /// <seealso cref="IllinoisSolver <em>Illinois</em>"/> algorithm or the
    /// <seealso cref="PegasusSolver <em>Pegasus</em>"/> algorithm.</para>
    /// 
    /// </summary>
    public class SecantSolver : AbstractUnivariateSolver
    {

        /// <summary>
        /// Default absolute accuracy. </summary>
        protected internal const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// Construct a solver with default accuracy (1e-6). </summary>
        public SecantSolver() : base(DEFAULT_ABSOLUTE_ACCURACY)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> absolute accuracy </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public SecantSolver(final double absoluteAccuracy)
        public SecantSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> relative accuracy </param>
        /// <param name="absoluteAccuracy"> absolute accuracy </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public SecantSolver(final double relativeAccuracy, final double absoluteAccuracy)
        public SecantSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override sealed double DoSolve()
        {
            // Get initial solution
            double x0 = GetMin();
            double x1 = GetMax();
            double f0 = ComputeObjectiveValue(x0);
            double f1 = ComputeObjectiveValue(x1);

            // If one of the bounds is the exact root, return it. Since these are
            // not under-approximations or over-approximations, we can return them
            // regardless of the allowed solutions.
            if (f0 == 0.0)
            {
                return x0;
            }
            if (f1 == 0.0)
            {
                return x1;
            }

            // Verify bracketing of initial solution.
            VerifyBracketing(x0, x1);

            // Get accuracies.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ftol = getFunctionValueAccuracy();
            double ftol = GetFunctionValueAccuracy();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double atol = getAbsoluteAccuracy();
            double atol = GetAbsoluteAccuracy();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double rtol = getRelativeAccuracy();
            double rtol = GetRelativeAccuracy();

            // Keep finding better approximations.
            while (true)
            {
                // Calculate the next approximation.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = x1 - ((f1 * (x1 - x0)) / (f1 - f0));
                double x = x1 - ((f1 * (x1 - x0)) / (f1 - f0));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fx = computeObjectiveValue(x);
                double fx = ComputeObjectiveValue(x);

                // If the new approximation is the exact root, return it. Since
                // this is not an under-approximation or an over-approximation,
                // we can return it regardless of the allowed solutions.
                if (fx == 0.0)
                {
                    return x;
                }

                // Update the bounds with the new approximation.
                x0 = x1;
                f0 = f1;
                x1 = x;
                f1 = fx;

                // If the function value of the last approximation is too small,
                // given the function value accuracy, then we can't get closer to
                // the root than we already are.
                if (FastMath.Abs(f1) <= ftol)
                {
                    return x1;
                }

                // If the current interval is within the given accuracies, we
                // are satisfied with the current approximation.
                if (FastMath.Abs(x1 - x0) < FastMath.Max(rtol * FastMath.Abs(x1), atol))
                {
                    return x1;
                }
            }
        }

    }

}