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
    /// Implements the <a href="http://mathworld.wolfram.com/RiddersMethod.html">
    /// Ridders' Method</a> for root finding of real univariate functions. For
    /// reference, see C. Ridders, <i>A new algorithm for computing a single root
    /// of a real continuous function </i>, IEEE Transactions on Circuits and
    /// Systems, 26 (1979), 979 - 980.
    /// <para>
    /// The function should be continuous but not necessarily smooth.</para>
    /// 
    /// @version $Id: RiddersSolver.java 1379560 2012-08-31 19:40:30Z erans $
    /// @since 1.2
    /// </summary>
    public class RiddersSolver : AbstractUnivariateSolver
    {
        /// <summary>
        /// Default absolute accuracy. </summary>
        private const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// Construct a solver with default accuracy (1e-6).
        /// </summary>
        public RiddersSolver()
            : this(DEFAULT_ABSOLUTE_ACCURACY)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public RiddersSolver(double absoluteAccuracy)
            : base(absoluteAccuracy)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public RiddersSolver(double relativeAccuracy, double absoluteAccuracy)
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
            // [x1, x2] is the bracketing interval in each iteration
            // x3 is the midpoint of [x1, x2]
            // x is the new root approximation and an endpoint of the new interval
            double x1 = min;
            double y1 = this.ComputeObjectiveValue(x1);
            double x2 = max;
            double y2 = this.ComputeObjectiveValue(x2);

            // check for zeros before verifying bracketing
            if (y1 == 0)
            {
                return min;
            }
            if (y2 == 0)
            {
                return max;
            }
            this.VerifyBracketing(min, max);

            double absoluteAccuracy = this.AbsoluteAccuracy;
            double functionValueAccuracy = this.FunctionValueAccuracy;
            double relativeAccuracy = this.RelativeAccuracy;

            double oldx = double.PositiveInfinity;
            while (true)
            {
                // calculate the new root approximation
                double x3 = 0.5 * (x1 + x2);
                double y3 = this.ComputeObjectiveValue(x3);
                if (FastMath.Abs(y3) <= functionValueAccuracy)
                {
                    return x3;
                }
                double delta = 1 - (y1 * y2) / (y3 * y3); // delta > 1 due to bracketing
                double correction = (MyUtils.Signum(y2) * MyUtils.Signum(y3)) * (x3 - x1) / FastMath.Sqrt(delta);
                double x = x3 - correction; // correction != 0
                double y = this.ComputeObjectiveValue(x);

                // check for convergence
                double tolerance = FastMath.Max(relativeAccuracy * FastMath.Abs(x), absoluteAccuracy);
                if (FastMath.Abs(x - oldx) <= tolerance)
                {
                    return x;
                }
                if (FastMath.Abs(y) <= functionValueAccuracy)
                {
                    return x;
                }

                // prepare the new interval for next iteration
                // Ridders' method guarantees x1 < x < x2
                if (correction > 0.0) // x1 < x < x3
                {
                    if (MyUtils.Signum(y1) + MyUtils.Signum(y) == 0.0)
                    {
                        x2 = x;
                        y2 = y;
                    }
                    else
                    {
                        x1 = x;
                        x2 = x3;
                        y1 = y;
                        y2 = y3;
                    }
                } // x3 < x < x2
                else
                {
                    if (MyUtils.Signum(y2) + MyUtils.Signum(y) == 0.0)
                    {
                        x1 = x;
                        y1 = y;
                    }
                    else
                    {
                        x1 = x3;
                        x2 = x;
                        y1 = y3;
                        y2 = y;
                    }
                }
                oldx = x;
            }
        }
    }
}