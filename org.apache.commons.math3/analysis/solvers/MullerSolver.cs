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
using FastMath = System.Math;

namespace org.apache.commons.math3.analysis.solvers
{
    /// <summary>
    /// This class implements the <a href="http://mathworld.wolfram.com/MullersMethod.html">
    /// Muller's Method</a> for root finding of real univariate functions. For
    /// reference, see <b>Elementary Numerical Analysis</b>, ISBN 0070124477,
    /// chapter 3.
    /// <para>
    /// Muller's method applies to both real and complex functions, but here we
    /// restrict ourselves to real functions.
    /// This class differs from <seealso cref="MullerSolver"/> in the way it avoids complex
    /// operations.</para>
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
    /// @version $Id: MullerSolver.java 1391927 2012-09-30 00:03:30Z erans $
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
        public MullerSolver()
            : this(DEFAULT_ABSOLUTE_ACCURACY)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public MullerSolver(double absoluteAccuracy)
            : base(absoluteAccuracy)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public MullerSolver(double relativeAccuracy, double absoluteAccuracy)
            : base(relativeAccuracy, absoluteAccuracy)
        {
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        protected internal override double DoSolve()
        {
            double min = this.Min;
            double max = this.Max;
            double initial = this.StartValue;

            double functionValueAccuracy = this.FunctionValueAccuracy;

            this.VerifySequence(min, initial, max);

            // check for zeros before verifying bracketing
            double fMin = this.ComputeObjectiveValue(min);
            if (FastMath.Abs(fMin) < functionValueAccuracy)
            {
                return min;
            }
            double fMax = this.ComputeObjectiveValue(max);
            if (FastMath.Abs(fMax) < functionValueAccuracy)
            {
                return max;
            }
            double fInitial = this.ComputeObjectiveValue(initial);
            if (FastMath.Abs(fInitial) < functionValueAccuracy)
            {
                return initial;
            }

            this.VerifyBracketing(min, max);

            if (this.IsBracketing(min, initial))
            {
                return this.Solve(min, initial, fMin, fInitial);
            }
            else
            {
                return this.Solve(initial, max, fInitial, fMax);
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
            double relativeAccuracy = this.RelativeAccuracy;
            double absoluteAccuracy = this.AbsoluteAccuracy;
            double functionValueAccuracy = this.FunctionValueAccuracy;

            // [x0, x2] is the bracketing interval in each iteration
            // x1 is the last approximation and an interpolation point in (x0, x2)
            // x is the new root approximation and new x1 for next round
            // d01, d12, d012 are divided differences

            double x0 = min;
            double y0 = fMin;
            double x2 = max;
            double y2 = fMax;
            double x1 = 0.5 * (x0 + x2);
            double y1 = this.ComputeObjectiveValue(x1);

            double oldx = double.PositiveInfinity;
            while (true)
            {
                // Muller's method employs quadratic interpolation through
                // x0, x1, x2 and x is the zero of the interpolating parabola.
                // Due to bracketing condition, this parabola must have two
                // real roots and we choose one in [x0, x2] to be x.
                double d01 = (y1 - y0) / (x1 - x0);
                double d12 = (y2 - y1) / (x2 - x1);
                double d012 = (d12 - d01) / (x2 - x0);
                double c1 = d01 + (x1 - x0) * d012;
                double delta = c1 * c1 - 4 * y1 * d012;
                double xplus = x1 + (-2.0 * y1) / (c1 + FastMath.Sqrt(delta));
                double xminus = x1 + (-2.0 * y1) / (c1 - FastMath.Sqrt(delta));
                // xplus and xminus are two roots of parabola and at least
                // one of them should lie in (x0, x2)
                double x = this.IsSequence(x0, xplus, x2) ? xplus : xminus;
                double y = this.ComputeObjectiveValue(x);

                // check for convergence
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
                    double ym = this.ComputeObjectiveValue(xm);
                    if (MyUtils.Signum(y0) + MyUtils.Signum(ym) == 0.0)
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
                    y1 = this.ComputeObjectiveValue(x1);
                    oldx = double.PositiveInfinity;
                }
            }
        }
    }
}