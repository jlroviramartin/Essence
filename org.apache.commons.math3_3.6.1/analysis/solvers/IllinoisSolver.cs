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
    /// Implements the <em>Illinois</em> method for root-finding (approximating
    /// a zero of a univariate real function). It is a modified
    /// <seealso cref="RegulaFalsiSolver <em>Regula Falsi</em>"/> method.
    /// 
    /// <para>Like the <em>Regula Falsi</em> method, convergence is guaranteed by
    /// maintaining a bracketed solution. The <em>Illinois</em> method however,
    /// should converge much faster than the original <em>Regula Falsi</em>
    /// method. Furthermore, this implementation of the <em>Illinois</em> method
    /// should not suffer from the same implementation issues as the <em>Regula
    /// Falsi</em> method, which may fail to convergence in certain cases.</para>
    /// 
    /// <para>The <em>Illinois</em> method assumes that the function is continuous,
    /// but not necessarily smooth.</para>
    /// 
    /// <para>Implementation based on the following article: M. Dowell and P. Jarratt,
    /// <em>A modified regula falsi method for computing the root of an
    /// equation</em>, BIT Numerical Mathematics, volume 11, number 2,
    /// pages 168-174, Springer, 1971.</para>
    /// 
    /// @since 3.0
    /// </summary>
    public class IllinoisSolver : BaseSecantSolver
    {

        /// <summary>
        /// Construct a solver with default accuracy (1e-6). </summary>
        public IllinoisSolver() : base(DEFAULT_ABSOLUTE_ACCURACY, Method.ILLINOIS)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public IllinoisSolver(final double absoluteAccuracy)
        public IllinoisSolver(double absoluteAccuracy) : base(absoluteAccuracy, Method.ILLINOIS)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public IllinoisSolver(final double relativeAccuracy, final double absoluteAccuracy)
        public IllinoisSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy, Method.ILLINOIS)
        {
        }

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        /// <param name="functionValueAccuracy"> Maximum function value error. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public IllinoisSolver(final double relativeAccuracy, final double absoluteAccuracy, final double functionValueAccuracy)
        public IllinoisSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy, Method.PEGASUS)
        {
        }
    }

}