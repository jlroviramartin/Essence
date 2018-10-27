/// Apache Commons Math 3.6.1
using System;

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
    /// Exception to be thrown when two dimensions differ.
    /// 
    /// @since 2.2
    /// </summary>
    public class DimensionMismatchException : MathIllegalNumberException
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -8415396756375798143L;
        /// <summary>
        /// Correct dimension. </summary>
        private readonly int dimension;

        /// <summary>
        /// Construct an exception from the mismatched dimensions.
        /// </summary>
        /// <param name="specific"> Specific context information pattern. </param>
        /// <param name="wrong"> Wrong dimension. </param>
        /// <param name="expected"> Expected dimension. </param>
        public DimensionMismatchException(Localizable specific, int wrong, int expected) : base(specific, Convert.ToInt32(wrong), Convert.ToInt32(expected))
        {
            dimension = expected;
        }

        public DimensionMismatchException(LocalizedFormats specific, int wrong, int expected) : base(specific, Convert.ToInt32(wrong), Convert.ToInt32(expected))
        {
            dimension = expected;
        }

        /// <summary>
        /// Construct an exception from the mismatched dimensions.
        /// </summary>
        /// <param name="wrong"> Wrong dimension. </param>
        /// <param name="expected"> Expected dimension. </param>
        public DimensionMismatchException(int wrong, int expected) : this(LocalizedFormats.DIMENSIONS_MISMATCH_SIMPLE, wrong, expected)
        {
        }

        /// <returns> the expected dimension. </returns>
        public virtual int GetDimension()
        {
            return dimension;
        }
    }

}