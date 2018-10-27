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

    /// <summary>
    /// Exception to be thrown when a number is not a number.
    /// 
    /// @since 3.1
    /// </summary>
    public class NotANumberException : MathIllegalNumberException
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = 20120906L;

        /// <summary>
        /// Construct the exception.
        /// </summary>
        public NotANumberException() : base(LocalizedFormats.NAN_NOT_ALLOWED, Convert.ToDouble(Double.NaN))
        {
        }

    }

}