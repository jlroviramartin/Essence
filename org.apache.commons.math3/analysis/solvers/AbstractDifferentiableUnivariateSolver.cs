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
    /// Provide a default implementation for several functions useful to generic
    /// solvers.
    /// 
    /// @since 3.0
    /// @version $Id: AbstractDifferentiableUnivariateSolver.java 1455194 2013-03-11 15:45:54Z luc $ </summary>
    /// @deprecated as of 3.1, replaced by <seealso cref="AbstractUnivariateDifferentiableSolver"/> 
    //[Obsolete("as of 3.1, replaced by <seealso cref="AbstractUnivariateDifferentiableSolver"/>")]
    public abstract class AbstractDifferentiableUnivariateSolver : BaseAbstractUnivariateSolver<DifferentiableUnivariateFunction>, DifferentiableUnivariateSolver
    {
        /// <summary>
        /// Derivative of the function to solve. </summary>
        private UnivariateFunction functionDerivative;

        /// <summary>
        /// Construct a solver with given absolute accuracy.
        /// </summary>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
        protected internal AbstractDifferentiableUnivariateSolver(double absoluteAccuracy)
            : base(absoluteAccuracy)
        {
        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy"> Maximum relative error. </param>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
        /// <param name="functionValueAccuracy"> Maximum function value error. </param>
        protected internal AbstractDifferentiableUnivariateSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy)
            : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {
        }

        /// <summary>
        /// Compute the objective function value.
        /// </summary>
        /// <param name="point"> Point at which the objective function must be evaluated. </param>
        /// <returns> the objective function value at specified point. </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximal number of evaluations is exceeded. </exception>
        protected internal virtual double ComputeDerivativeObjectiveValue(double point)
        {
            this.IncrementEvaluationCount();
            return this.functionDerivative.Value(point);
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        protected internal override void Setup(int maxEval, DifferentiableUnivariateFunction f, double min, double max, double startValue)
        {
            base.Setup(maxEval, f, min, max, startValue);
            this.functionDerivative = f.Derivative();
        }
    }
}