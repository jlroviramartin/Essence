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

    using FastMath = System.Math;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;

    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/Bisection.html">
    /// bisection algorithm</a> for finding zeros of univariate real functions.
    /// <para>
    /// The function should be continuous but not necessarily smooth.</para>
    /// 
    /// </summary>
    public class BisectionSolver : AbstractUnivariateSolver
    {
        /// <summary>
        /// Default absolute accuracy. </summary>
        private const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;

        /// <summary>
        /// Construct a solver with default accuracy (1e-6).
        /// </summary>
        public BisectionSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public BisectionSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public BisectionSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        protected internal override double DoSolve()
        {
            double min = GetMin();
            double max = GetMax();
            VerifyInterval(min, max);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double absoluteAccuracy = getAbsoluteAccuracy();
            double absoluteAccuracy = GetAbsoluteAccuracy();
            double m;
            double fm;
            double fmin;

            while (true)
            {
                m = UnivariateSolverUtils.Midpoint(min, max);
                fmin = ComputeObjectiveValue(min);
                fm = ComputeObjectiveValue(m);

                if (fm * fmin > 0)
                {
                    // max and m bracket the root.
                    min = m;
                }
                else
                {
                    // min and m bracket the root.
                    max = m;
                }

                if (FastMath.Abs(max - min) <= absoluteAccuracy)
                {
                    m = UnivariateSolverUtils.Midpoint(min, max);
                    return m;
                }
            }
        }
    }

}