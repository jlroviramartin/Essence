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

    using ExceptionContextProvider = org.apache.commons.math3.exception.util.ExceptionContextProvider;
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;

    /// <summary>
    /// Class to signal parse failures.
    /// 
    /// @since 2.2
    /// </summary>
    public class MathParseException : MathIllegalStateException, ExceptionContextProvider
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -6024911025449780478L;

        /// <param name="wrong"> Bad string representation of the object. </param>
        /// <param name="position"> Index, in the {@code wrong} string, that caused the
        /// parsing to fail. </param>
        /// <param name="type"> Class of the object supposedly represented by the
        /// {@code wrong} string. </param>
        public MathParseException(string wrong, int position, Type type)
        {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
            GetContext().addMessage(LocalizedFormats.CANNOT_PARSE_AS_TYPE, wrong, Convert.ToInt32(position), type.FullName);
        }

        /// <param name="wrong"> Bad string representation of the object. </param>
        /// <param name="position"> Index, in the {@code wrong} string, that caused the
        /// parsing to fail. </param>
        public MathParseException(string wrong, int position)
        {
            GetContext().addMessage(LocalizedFormats.CANNOT_PARSE, wrong, Convert.ToInt32(position));
        }
    }

}