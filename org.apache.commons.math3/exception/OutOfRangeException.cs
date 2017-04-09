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
    using Localizable = org.apache.commons.math3.exception.util.Localizable;

    /// <summary>
    /// Exception to be thrown when some argument is out of range.
    /// 
    /// @since 2.2
    /// </summary>
    public class OutOfRangeException : MathIllegalNumberException
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = 111601815794403609L;
        /// <summary>
        /// Lower bound. </summary>
        private readonly Number lo;
        /// <summary>
        /// Higher bound. </summary>
        private readonly Number hi;

        /// <summary>
        /// Construct an exception from the mismatched dimensions.
        /// </summary>
        /// <param name="wrong"> Requested value. </param>
        /// <param name="lo"> Lower bound. </param>
        /// <param name="hi"> Higher bound. </param>
        public OutOfRangeException(Number wrong, Number lo, Number hi) : this(LocalizedFormats.OUT_OF_RANGE_SIMPLE, wrong, lo, hi)
        {
        }

        /// <summary>
        /// Construct an exception from the mismatched dimensions with a
        /// specific context information.
        /// </summary>
        /// <param name="specific"> Context information. </param>
        /// <param name="wrong"> Requested value. </param>
        /// <param name="lo"> Lower bound. </param>
        /// <param name="hi"> Higher bound. </param>
        public OutOfRangeException(Localizable specific, Number wrong, Number lo, Number hi) : base(specific, wrong, lo, hi)
        {
            this.lo = lo;
            this.hi = hi;
        }

        /// <returns> the lower bound. </returns>
        public virtual Number GetLo()
        {
            return lo;
        }
        /// <returns> the higher bound. </returns>
        public virtual Number GetHi()
        {
            return hi;
        }
    }

}