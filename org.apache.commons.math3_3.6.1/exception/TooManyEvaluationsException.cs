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
namespace org.apache.commons.math3.exception
{

    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;
    using Number = System.IConvertible;

    /// <summary>
    /// Exception to be thrown when the maximal number of evaluations is exceeded.
    /// 
    /// @since 3.0
    /// </summary>
    public class TooManyEvaluationsException : MaxCountExceededException
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = 4330003017885151975L;

        /// <summary>
        /// Construct the exception.
        /// </summary>
        /// <param name="max"> Maximum number of evaluations. </param>
        public TooManyEvaluationsException(Number max) : base(max)
        {
            GetContext().AddMessage(LocalizedFormats.EVALUATIONS);
        }
    }

}