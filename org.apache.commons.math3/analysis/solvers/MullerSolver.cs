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

    using NoBracketingException = org.apache.commons.math3.exception.NoBracketingException;
    using NumberIsTooLargeException = org.apache.commons.math3.exception.NumberIsTooLargeException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;
    using FastMath = org.apache.commons.math3.util.FastMath;

    /// <summary>
    /// This class implements the <a href="http://mathworld.wolfram.com/MullersMethod.html">
    /// Muller's Method</a> for root finding of real univariate functions. For
    /// reference, see <b>Elementary Numerical Analysis</b>, ISBN 0070124477,
    /// chapter 3.
    /// <para>
    /// Muller's method applies to both real and complex functions, but here we
    /// restrict ourselves to real functions.
    /// This class differs from <seealso cref="MullerSolver"/> in the way it avoids complex
    /// </para>
    /// operations.</para><para>
    /// Muller's original method would have function evaluation at complex point.
    /// Since our f(x) is real, we have to find ways to avoid that. Bracketing
    /// condition is one way to go: by requiring bracketing in every iteration,
    /// the newly computed approximation is guaranteed to be real.</p>
    /// <para>
    /// Normally Muller's method converges quadratically in the vicinity of a
    /// zero, however it may be very slow in regions far away from zeros. For
    /// example, f(x) = exp(x) - 1, min = -50, max = 100. In such case we use
    /// bisection as a safety backup if it performs very poorly.</para>
    /// <para>
    /// The formulas here use divided differences directly.</para>
    /// 
    /// @since 1.2 </summary>
    /// <seealso cref= MullerSolver2 </seealso>
    public class MullerSolver : AbstractUnivariateSolver
    {

        /// <summary>
        /// Default absolute accuracy. </summary>
        private const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// Construct a solver with default accuracy (1e-6).
        /// </summary>
        public MullerSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public MullerSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public MullerSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        protected internal override double DoSolve()
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double min = getMin();
            double min = GetMin();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double max = getMax();
            double max = GetMax();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double initial = getStartValue();
            double initial = GetStartValue();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double functionValueAccuracy = getFunctionValueAccuracy();
            double functionValueAccuracy = GetFunctionValueAccuracy();

            VerifySequence(min, initial, max);

            // check for zeros before verifying bracketing
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fMin = computeObjectiveValue(min);
            double fMin = ComputeObjectiveValue(min);
            if (FastMath.Abs(fMin) < functionValueAccuracy)
            {
                return min;
            }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fMax = computeObjectiveValue(max);
            double fMax = ComputeObjectiveValue(max);
            if (FastMath.Abs(fMax) < functionValueAccuracy)
            {
                return max;
            }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fInitial = computeObjectiveValue(initial);
            double fInitial = ComputeObjectiveValue(initial);
            if (FastMath.Abs(fInitial) < functionValueAccuracy)
            {
                return initial;
            }

            VerifyBracketing(min, max);

            if (IsBracketing(min, initial))
            {
                return Solve(min, initial, fMin, fInitial);
            }
            else
            {
                return Solve(initial, max, fInitial, fMax);
            }
        }

        /// <summary>
        /// Find a real root in the given interval.
        /// </summary>
        /// <param name="min"> Lower bound for the interval. </param>
        /// <param name="max"> Upper bound for the interval. </param>
        /// <param name="fMin"> function value at the lower bound. </param>
        /// <param name="fMax"> function value at the upper bound. </param>
        /// <returns> the point at which the function value is zero. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the allowed number of calls to
        /// the function to be solved has been exhausted. </exception>
        private double Solve(double min, double max, double fMin, double fMax)
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double relativeAccuracy = getRelativeAccuracy();
            double relativeAccuracy = GetRelativeAccuracy();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absoluteAccuracy = getAbsoluteAccuracy();
            double absoluteAccuracy = GetAbsoluteAccuracy();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double functionValueAccuracy = getFunctionValueAccuracy();
            double functionValueAccuracy = GetFunctionValueAccuracy();

            // [x0, x2] is the bracketing interval in each iteration
            // x1 is the last approximation and an interpolation point in (x0, x2)
            // x is the new root approximation and new x1 for next round
            // d01, d12, d012 are divided differences

            double x0 = min;
            double y0 = fMin;
            double x2 = max;
            double y2 = fMax;
            double x1 = 0.5 * (x0 + x2);
            double y1 = ComputeObjectiveValue(x1);

            double oldx = double.PositiveInfinity;
            while (true)
            {
                // Muller's method employs quadratic interpolation through
                // x0, x1, x2 and x is the zero of the interpolating parabola.
                // Due to bracketing condition, this parabola must have two
                // real roots and we choose one in [x0, x2] to be x.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double d01 = (y1 - y0) / (x1 - x0);
                double d01 = (y1 - y0) / (x1 - x0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double d12 = (y2 - y1) / (x2 - x1);
                double d12 = (y2 - y1) / (x2 - x1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double d012 = (d12 - d01) / (x2 - x0);
                double d012 = (d12 - d01) / (x2 - x0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double c1 = d01 + (x1 - x0) * d012;
                double c1 = d01 + (x1 - x0) * d012;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double delta = c1 * c1 - 4 * y1 * d012;
                double delta = c1 * c1 - 4 * y1 * d012;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xplus = x1 + (-2.0 * y1) / (c1 + org.apache.commons.math3.util.FastMath.sqrt(delta));
                double xplus = x1 + (-2.0 * y1) / (c1 + FastMath.Sqrt(delta));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double xminus = x1 + (-2.0 * y1) / (c1 - org.apache.commons.math3.util.FastMath.sqrt(delta));
                double xminus = x1 + (-2.0 * y1) / (c1 - FastMath.Sqrt(delta));
                // xplus and xminus are two roots of parabola and at least
                // one of them should lie in (x0, x2)
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = isSequence(x0, xplus, x2) ? xplus : xminus;
                double x = IsSequence(x0, xplus, x2) ? xplus : xminus;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = computeObjectiveValue(x);
                double y = ComputeObjectiveValue(x);

                // check for convergence
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tolerance = org.apache.commons.math3.util.FastMath.max(relativeAccuracy * org.apache.commons.math3.util.FastMath.abs(x), absoluteAccuracy);
                double tolerance = FastMath.Max(relativeAccuracy * FastMath.Abs(x), absoluteAccuracy);
                if (FastMath.Abs(x - oldx) <= tolerance || FastMath.Abs(y) <= functionValueAccuracy)
                {
                    return x;
                }

                // Bisect if convergence is too slow. Bisection would waste
                // our calculation of x, hopefully it won't happen often.
                // the real number equality test x == x1 is intentional and
                // completes the proximity tests above it
                bool bisect = (x < x1 && (x1 - x0) > 0.95 * (x2 - x0)) || (x > x1 && (x2 - x1) > 0.95 * (x2 - x0)) || (x == x1);
                // prepare the new bracketing interval for next iteration
                if (!bisect)
                {
                    x0 = x < x1 ? x0 : x1;
                    y0 = x < x1 ? y0 : y1;
                    x2 = x > x1 ? x2 : x1;
                    y2 = x > x1 ? y2 : y1;
                    x1 = x;
                    y1 = y;
                    oldx = x;
                }
                else
                {
                    double xm = 0.5 * (x0 + x2);
                    double ym = ComputeObjectiveValue(xm);
                    if (FastMath.Signum(y0) + FastMath.Signum(ym) == 0.0)
                    {
                        x2 = xm;
                        y2 = ym;
                    }
                    else
                    {
                        x0 = xm;
                        y0 = ym;
                    }
                    x1 = 0.5 * (x0 + x2);
                    y1 = ComputeObjectiveValue(x1);
                    oldx = double.PositiveInfinity;
                }
            }
        }
    }

}