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

    using UnivariateDifferentiableFunction = org.apache.commons.math3.analysis.differentiation.UnivariateDifferentiableFunction;


    /// <summary>
    /// Interface for (univariate real) rootfinding algorithms.
    /// Implementations will search for only one zero in the given interval.
    /// 
    /// @since 3.1
    /// @version $Id: UnivariateDifferentiableSolver.java 1383441 2012-09-11 14:56:39Z luc $
    /// </summary>
    public interface UnivariateDifferentiableSolver : BaseUnivariateSolver<UnivariateDifferentiableFunction>
    {
    }

}