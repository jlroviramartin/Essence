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

    using Localizable = org.apache.commons.math3.exception.util.Localizable;

    /// <summary>
    /// Base class for exceptions raised by a wrong number.
    /// This class is not intended to be instantiated directly: it should serve
    /// as a base class to create all the exceptions that are raised because some
    /// precondition is violated by a number argument.
    /// 
    /// @since 2.2
    /// </summary>
    public class MathIllegalNumberException : MathIllegalArgumentException
    {

        /// <summary>
        /// Helper to avoid boxing warnings. @since 3.3 </summary>
        protected internal static readonly int? INTEGER_ZERO = Convert.ToInt32(0);

        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -7447085893598031110L;

        /// <summary>
        /// Requested. </summary>
        private readonly int argument;

        /// <summary>
        /// Construct an exception.
        /// </summary>
        /// <param name="pattern"> Localizable pattern. </param>
        /// <param name="wrong"> Wrong number. </param>
        /// <param name="arguments"> Arguments. </param>
        protected internal MathIllegalNumberException(Localizable pattern, int wrong, params object[] arguments) : base(pattern, wrong, arguments)
        {
            argument = wrong;
        }

        /// <returns> the requested value. </returns>
        public virtual int GetArgument()
        {
            return argument;
        }
    }

}