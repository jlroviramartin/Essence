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

using FastMath = System.Math;

namespace org.apache.commons.math3.analysis.solvers
{
    /// <summary>
    /// Implements the <em>Pegasus</em> method for root-finding (approximating
    /// a zero of a univariate real function). It is a modified
    /// <seealso cref="RegulaFalsiSolver <em>Regula Falsi</em>"/> method.
    /// 
    /// <para>Like the <em>Regula Falsi</em> method, convergence is guaranteed by
    /// maintaining a bracketed solution. The <em>Pegasus</em> method however,
    /// should converge much faster than the original <em>Regula Falsi</em>
    /// method. Furthermore, this implementation of the <em>Pegasus</em> method
    /// should not suffer from the same implementation issues as the <em>Regula
    /// Falsi</em> method, which may fail to convergence in certain cases. Also,
    /// the <em>Pegasus</em> method should converge faster than the
    /// <seealso cref="IllinoisSolver <em>Illinois</em>"/> method, another <em>Regula
    /// Falsi</em>-based method.</para>
    /// 
    /// <para>The <em>Pegasus</em> method assumes that the function is continuous,
    /// but not necessarily smooth.</para>
    /// 
    /// <para>Implementation based on the following article: M. Dowell and P. Jarratt,
    /// <em>The "Pegasus" method for computing the root of an equation</em>,
    /// BIT Numerical Mathematics, volume 12, number 4, pages 503-508, Springer,
    /// 1972.</para>
    /// 
    /// @since 3.0
    /// @version $Id: PegasusSolver.java 1364387 2012-07-22 18:14:11Z tn $
    /// </summary>
    public class PegasusSolver : BaseSecantSolver
    {
        /// <summary>
        /// Construct a solver with default accuracy (1e-6). </summary>
        public PegasusSolver()
            : base(DEFAULT_ABSOLUTE_ACCURACY, Method.PEGASUS)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public PegasusSolver(double absoluteAccuracy)
            : base(absoluteAccuracy, Method.PEGASUS)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public PegasusSolver(double relativeAccuracy, double absoluteAccuracy)
            : base(relativeAccuracy, absoluteAccuracy, Method.PEGASUS)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        /// <param name="functionValueAccuracy"> Maximum function value error. </param>
        public PegasusSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy)
            : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy, Method.PEGASUS)
        {
        }
    }
}