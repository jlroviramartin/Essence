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
    /// Exception to be thrown when the required data is missing.
    /// 
    /// @since 2.2
    /// </summary>
    public class NoDataException : MathIllegalArgumentException
    {

        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -3629324471511904459L;

        /// <summary>
        /// Construct the exception.
        /// </summary>
        public NoDataException() : this(LocalizedFormats.NO_DATA)
        {
        }
        /// <summary>
        /// Construct the exception with a specific context.
        /// </summary>
        /// <param name="specific"> Contextual information on what caused the exception. </param>
        public NoDataException(Localizable specific) : base(specific)
        {
        }
    }

}