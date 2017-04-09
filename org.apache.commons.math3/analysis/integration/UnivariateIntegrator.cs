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
namespace org.apache.commons.math3.analysis.integration
{

    using MathIllegalArgumentException = org.apache.commons.math3.exception.MathIllegalArgumentException;
    using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
    using NullArgumentException = org.apache.commons.math3.exception.NullArgumentException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;

    /// <summary>
    /// Interface for univariate real integration algorithms.
    /// 
    /// @since 1.2
    /// </summary>
    public interface UnivariateIntegrator
    {

        /// <summary>
        /// Get the relative accuracy.
        /// </summary>
        /// <returns> the accuracy </returns>
        double GetRelativeAccuracy();

        /// <summary>
        /// Get the absolute accuracy.
        /// </summary>
        /// <returns> the accuracy </returns>
        double GetAbsoluteAccuracy();

        /// <summary>
        /// Get the min limit for the number of iterations.
        /// </summary>
        /// <returns> the actual min limit </returns>
        int GetMinimalIterationCount();

        /// <summary>
        /// Get the upper limit for the number of iterations.
        /// </summary>
        /// <returns> the actual upper limit </returns>
        int GetMaximalIterationCount();

        /// <summary>
        /// Integrate the function in the given interval.
        /// </summary>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <param name="f"> the integrand function </param>
        /// <param name="min"> the lower bound for the interval </param>
        /// <param name="max"> the upper bound for the interval </param>
        /// <returns> the value of integral </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximum number of function
        /// evaluations is exceeded </exception>
        /// <exception cref="MaxCountExceededException"> if the maximum iteration count is exceeded
        /// or the integrator detects convergence problems otherwise </exception>
        /// <exception cref="MathIllegalArgumentException"> if {@code min > max} or the endpoints do not
        /// satisfy the requirements specified by the integrator </exception>
        /// <exception cref="NullArgumentException"> if {@code f} is {@code null}. </exception>
        double Integrate(int maxEval, UnivariateFunction f, double min, double max);

        /// <summary>
        /// Get the number of function evaluations of the last run of the integrator.
        /// </summary>
        /// <returns> number of function evaluations </returns>
        int GetEvaluations();

        /// <summary>
        /// Get the number of iterations of the last run of the integrator.
        /// </summary>
        /// <returns> number of iterations </returns>
        int GetIterations();

    }

}