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
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;

    /// <summary>
    /// Exception to be thrown when a number is too small.
    /// 
    /// @since 2.2
    /// </summary>
    public class NumberIsTooSmallException : MathIllegalNumberException
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -6100997100383932834L;
        /// <summary>
        /// Higher bound.
        /// </summary>
        private readonly Number min;
        /// <summary>
        /// Whether the maximum is included in the allowed range.
        /// </summary>
        private readonly bool boundIsAllowed;

        /// <summary>
        /// Construct the exception.
        /// </summary>
        /// <param name="wrong"> Value that is smaller than the minimum. </param>
        /// <param name="min"> Minimum. </param>
        /// <param name="boundIsAllowed"> Whether {@code min} is included in the allowed range. </param>
        public NumberIsTooSmallException(Number wrong, Number min, bool boundIsAllowed) : this(boundIsAllowed ? LocalizedFormats.NUMBER_TOO_SMALL : LocalizedFormats.NUMBER_TOO_SMALL_BOUND_EXCLUDED, wrong, min, boundIsAllowed)
        {
        }

        /// <summary>
        /// Construct the exception with a specific context.
        /// </summary>
        /// <param name="specific"> Specific context pattern. </param>
        /// <param name="wrong"> Value that is smaller than the minimum. </param>
        /// <param name="min"> Minimum. </param>
        /// <param name="boundIsAllowed"> Whether {@code min} is included in the allowed range. </param>
        public NumberIsTooSmallException(Localizable specific, Number wrong, Number min, bool boundIsAllowed) : base(specific, wrong, min)
        {

            this.min = min;
            this.boundIsAllowed = boundIsAllowed;
        }

        /// <returns> {@code true} if the minimum is included in the allowed range. </returns>
        public virtual bool GetBoundIsAllowed()
        {
            return boundIsAllowed;
        }

        /// <returns> the minimum. </returns>
        public virtual Number GetMin()
        {
            return min;
        }
    }

}