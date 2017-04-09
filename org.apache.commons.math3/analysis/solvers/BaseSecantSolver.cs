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
    using ConvergenceException = org.apache.commons.math3.exception.ConvergenceException;
    using MathInternalError = org.apache.commons.math3.exception.MathInternalError;

    /// <summary>
    /// Base class for all bracketing <em>Secant</em>-based methods for root-finding
    /// (approximating a zero of a univariate real function).
    /// 
    /// <para>Implementation of the <seealso cref="RegulaFalsiSolver <em>Regula Falsi</em>"/> and
    /// <seealso cref="IllinoisSolver <em>Illinois</em>"/> methods is based on the
    /// following article: M. Dowell and P. Jarratt,
    /// <em>A modified regula falsi method for computing the root of an
    /// equation</em>, BIT Numerical Mathematics, volume 11, number 2,
    /// pages 168-174, Springer, 1971.</para>
    /// 
    /// <para>Implementation of the <seealso cref="PegasusSolver <em>Pegasus</em>"/> method is
    /// based on the following article: M. Dowell and P. Jarratt,
    /// <em>The "Pegasus" method for computing the root of an equation</em>,
    /// BIT Numerical Mathematics, volume 12, number 4, pages 503-508, Springer,
    /// 1972.</para>
    /// 
    /// <para>The <seealso cref="SecantSolver <em>Secant</em>"/> method is <em>not</em> a
    /// bracketing method, so it is not implemented here. It has a separate
    /// implementation.</para>
    /// 
    /// @since 3.0
    /// </summary>
    public abstract class BaseSecantSolver : AbstractUnivariateSolver, BracketedUnivariateSolver<UnivariateFunction>
    {
        //public abstract double Solve(int maxEval, FUNC f, double min, double max, double startValue, AllowedSolution allowedSolution);
        //public abstract double Solve(int maxEval, FUNC f, double min, double max, AllowedSolution allowedSolution);

        /// <summary>
        /// Default absolute accuracy. </summary>
        protected internal const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// The kinds of solutions that the algorithm may accept. </summary>
        private AllowedSolution allowed;

        /// <summary>
        /// The <em>Secant</em>-based root-finding method to use. </summary>
        private readonly Method method;

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        /// <param name="method"> <em>Secant</em>-based root-finding method to use. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseSecantSolver(final double absoluteAccuracy, final Method method)
        protected internal BaseSecantSolver(double absoluteAccuracy, Method method) : base(absoluteAccuracy)
        {
            this.allowed = AllowedSolution.ANY_SIDE;
            this.method = method;
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        /// <param name="method"> <em>Secant</em>-based root-finding method to use. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseSecantSolver(final double relativeAccuracy, final double absoluteAccuracy, final Method method)
        protected internal BaseSecantSolver(double relativeAccuracy, double absoluteAccuracy, Method method) : base(relativeAccuracy, absoluteAccuracy)
        {
            this.allowed = AllowedSolution.ANY_SIDE;
            this.method = method;
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Maximum relative error. </param>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
        /// <param name="functionValueAccuracy"> Maximum function value error. </param>
        /// <param name="method"> <em>Secant</em>-based root-finding method to use </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected BaseSecantSolver(final double relativeAccuracy, final double absoluteAccuracy, final double functionValueAccuracy, final Method method)
        protected internal BaseSecantSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy, Method method) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {
            this.allowed = AllowedSolution.ANY_SIDE;
            this.method = method;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public double solve(final int maxEval, final org.apache.commons.math3.analysis.UnivariateFunction f, final double min, final double max, final AllowedSolution allowedSolution)
        public virtual double Solve(int maxEval, UnivariateFunction f, double min, double max, AllowedSolution allowedSolution)
        {
            return Solve(maxEval, f, min, max, min + 0.5 * (max - min), allowedSolution);
        }

        /// <summary>
        /// {@inheritDoc} </summary>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public double solve(final int maxEval, final org.apache.commons.math3.analysis.UnivariateFunction f, final double min, final double max, final double startValue, final AllowedSolution allowedSolution)
        public virtual double Solve(int maxEval, UnivariateFunction f, double min, double max, double startValue, AllowedSolution allowedSolution)
        {
            this.allowed = allowedSolution;
            return base.Solve(maxEval, f, min, max, startValue);
        }

        /// <summary>
        /// {@inheritDoc} </summary>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public double solve(final int maxEval, final org.apache.commons.math3.analysis.UnivariateFunction f, final double min, final double max, final double startValue)
        public virtual double Solve(int maxEval, UnivariateFunction f, double min, double max, double startValue)
        {
            return Solve(maxEval, f, min, max, startValue, AllowedSolution.ANY_SIDE);
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        /// <exception cref="ConvergenceException"> if the algorithm failed due to finite
        /// precision. </exception>
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

            // Keep track of inverted intervals, meaning that the left bound is
            // larger than the right bound.
            bool inverted = false;

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
                if (f1 * fx < 0)
                {
                    // The value of x1 has switched to the other bound, thus inverting
                    // the interval.
                    x0 = x1;
                    f0 = f1;
                    inverted = !inverted;
                }
                else
                {
                    switch (method)
                    {
                    case org.apache.commons.math3.analysis.solvers.BaseSecantSolver.Method.ILLINOIS:
                        f0 *= 0.5;
                        break;
                    case org.apache.commons.math3.analysis.solvers.BaseSecantSolver.Method.PEGASUS:
                        f0 *= f1 / (f1 + fx);
                        break;
                    case org.apache.commons.math3.analysis.solvers.BaseSecantSolver.Method.REGULA_FALSI:
                        // Detect early that algorithm is stuck, instead of waiting
                        // for the maximum number of iterations to be exceeded.
                        if (x == x1)
                        {
                            throw new ConvergenceException();
                        }
                        break;
                    default:
                        // Should never happen.
                        throw new MathInternalError();
                    }
                }
                // Update from [x0, x1] to [x0, x].
                x1 = x;
                f1 = fx;

                // If the function value of the last approximation is too small,
                // given the function value accuracy, then we can't get closer to
                // the root than we already are.
                if (FastMath.Abs(f1) <= ftol)
                {
                    switch (allowed)
                    {
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.ANY_SIDE:
                        return x1;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.LEFT_SIDE:
                        if (inverted)
                        {
                            return x1;
                        }
                        break;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.RIGHT_SIDE:
                        if (!inverted)
                        {
                            return x1;
                        }
                        break;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.BELOW_SIDE:
                        if (f1 <= 0)
                        {
                            return x1;
                        }
                        break;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.ABOVE_SIDE:
                        if (f1 >= 0)
                        {
                            return x1;
                        }
                        break;
                    default:
                        throw new MathInternalError();
                    }
                }

                // If the current interval is within the given accuracies, we
                // are satisfied with the current approximation.
                if (FastMath.Abs(x1 - x0) < FastMath.Max(rtol * FastMath.Abs(x1), atol))
                {
                    switch (allowed)
                    {
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.ANY_SIDE:
                        return x1;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.LEFT_SIDE:
                        return inverted ? x1 : x0;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.RIGHT_SIDE:
                        return inverted ? x0 : x1;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.BELOW_SIDE:
                        return (f1 <= 0) ? x1 : x0;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.ABOVE_SIDE:
                        return (f1 >= 0) ? x1 : x0;
                    default:
                        throw new MathInternalError();
                    }
                }
            }
        }

        /// <summary>
        /// <em>Secant</em>-based root-finding methods. </summary>
        protected internal enum Method
        {

            /// <summary>
            /// The <seealso cref="RegulaFalsiSolver <em>Regula Falsi</em>"/> or
            /// <em>False Position</em> method.
            /// </summary>
            REGULA_FALSI,

            /// <summary>
            /// The <seealso cref="IllinoisSolver <em>Illinois</em>"/> method. </summary>
            ILLINOIS,

            /// <summary>
            /// The <seealso cref="PegasusSolver <em>Pegasus</em>"/> method. </summary>
            PEGASUS

        }
    }

}