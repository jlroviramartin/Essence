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

    using DerivativeStructure = org.apache.commons.math3.analysis.differentiation.DerivativeStructure;
    using UnivariateDifferentiableFunction = org.apache.commons.math3.analysis.differentiation.UnivariateDifferentiableFunction;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;

    /// <summary>
    /// Provide a default implementation for several functions useful to generic
    /// solvers.
    /// 
    /// @since 3.1
    /// @version $Id: AbstractUnivariateDifferentiableSolver.java 1455194 2013-03-11 15:45:54Z luc $
    /// </summary>
    public abstract class AbstractUnivariateDifferentiableSolver : BaseAbstractUnivariateSolver<UnivariateDifferentiableFunction>, UnivariateDifferentiableSolver
    {

        /// <summary>
        /// Function to solve. </summary>
        private UnivariateDifferentiableFunction Function;

        /// <summary>
        /// Construct a solver with given absolute accuracy.
        /// </summary>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: protected AbstractUnivariateDifferentiableSolver(final double absoluteAccuracy)
        protected internal AbstractUnivariateDifferentiableSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
        }

        /// <summary>
        /// Construct a solver with given accuracies.
        /// </summary>
        /// <param name="relativeAccuracy"> Maximum relative error. </param>
        /// <param name="absoluteAccuracy"> Maximum absolute error. </param>
        /// <param name="functionValueAccuracy"> Maximum function value error. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: protected AbstractUnivariateDifferentiableSolver(final double relativeAccuracy, final double absoluteAccuracy, final double functionValueAccuracy)
        protected internal AbstractUnivariateDifferentiableSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {
        }

        /// <summary>
        /// Compute the objective function value.
        /// </summary>
        /// <param name="point"> Point at which the objective function must be evaluated. </param>
        /// <returns> the objective function value and derivative at specified point. </returns>
        /// <exception cref="TooManyEvaluationsException">
        /// if the maximal number of evaluations is exceeded. </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: protected org.apache.commons.math3.analysis.differentiation.DerivativeStructure computeObjectiveValueAndDerivative(double point) throws org.apache.commons.math3.exception.TooManyEvaluationsException
        protected internal virtual DerivativeStructure ComputeObjectiveValueAndDerivative(double point)
        {
            IncrementEvaluationCount();
            return Function.value(new DerivativeStructure(1, 1, 0, point));
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        protected internal override void Setup(int maxEval, UnivariateDifferentiableFunction f, double min, double max, double startValue)
        {
            base.Setup(maxEval, f, min, max, startValue);
            Function = f;
        }
    }

}