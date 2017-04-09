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

    using Localizable = org.apache.commons.math3.exception.util.Localizable;

    /// <summary>
    /// Exception to be thrown when the argument is not greater than 0.
    /// 
    /// @since 2.2
    /// </summary>
    public class NotStrictlyPositiveException : NumberIsTooSmallException
    {

        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -7824848630829852237L;

        /// <summary>
        /// Construct the exception.
        /// </summary>
        /// <param name="value"> Argument. </param>
        public NotStrictlyPositiveException(Number value) : base(value, INTEGER_ZERO, false)
        {
        }
        /// <summary>
        /// Construct the exception with a specific context.
        /// </summary>
        /// <param name="specific"> Specific context where the error occurred. </param>
        /// <param name="value"> Argument. </param>
        public NotStrictlyPositiveException(Localizable specific, Number value) : base(specific, value, INTEGER_ZERO, false)
        {
        }
    }

}