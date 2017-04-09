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
    /// Exception to be thrown when two sets of dimensions differ.
    /// 
    /// @since 3.0
    /// </summary>
    public class MultiDimensionMismatchException : MathIllegalArgumentException
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -8415396756375798143L;

        /// <summary>
        /// Wrong dimensions. </summary>
        private readonly int?[] wrong;
        /// <summary>
        /// Correct dimensions. </summary>
        private readonly int?[] expected;

        /// <summary>
        /// Construct an exception from the mismatched dimensions.
        /// </summary>
        /// <param name="wrong"> Wrong dimensions. </param>
        /// <param name="expected"> Expected dimensions. </param>
        public MultiDimensionMismatchException(int?[] wrong, Integer[] expected) : this(LocalizedFormats.DIMENSIONS_MISMATCH, wrong, expected)
        {
        }

        /// <summary>
        /// Construct an exception from the mismatched dimensions.
        /// </summary>
        /// <param name="specific"> Message pattern providing the specific context of
        /// the error. </param>
        /// <param name="wrong"> Wrong dimensions. </param>
        /// <param name="expected"> Expected dimensions. </param>
        public MultiDimensionMismatchException(Localizable specific, Integer[] wrong, Integer[] expected) : base(specific, wrong, expected)
        {
            this.wrong = wrong.Clone();
            this.expected = expected.Clone();
        }

        /// <returns> an array containing the wrong dimensions. </returns>
        public virtual int?[] GetWrongDimensions()
        {
            return wrong.Clone();
        }
        /// <returns> an array containing the expected dimensions. </returns>
        public virtual int?[] GetExpectedDimensions()
        {
            return expected.Clone();
        }

        /// <param name="index"> Dimension index. </param>
        /// <returns> the wrong dimension stored at {@code index}. </returns>
        public virtual int GetWrongDimension(int index)
        {
            return wrong[index].Value;
        }
        /// <param name="index"> Dimension index. </param>
        /// <returns> the expected dimension stored at {@code index}. </returns>
        public virtual int GetExpectedDimension(int index)
        {
            return expected[index].Value;
        }
    }

}