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
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;

    /// <summary>
    /// Exception triggered when something that shouldn't happen does happen.
    /// 
    /// @since 2.2
    /// </summary>
    public class MathInternalError : MathIllegalStateException
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -6276776513966934846L;
        /// <summary>
        /// URL for reporting problems. </summary>
        private const string REPORT_URL = "https://issues.apache.org/jira/browse/MATH";

        /// <summary>
        /// Simple constructor.
        /// </summary>
        public MathInternalError()
        {
            GetContext().AddMessage(LocalizedFormats.INTERNAL_ERROR, REPORT_URL);
        }

        /// <summary>
        /// Simple constructor. </summary>
        /// <param name="cause"> root cause </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public MathInternalError(final Throwable cause)
        public MathInternalError(Exception cause) : base(cause, LocalizedFormats.INTERNAL_ERROR, REPORT_URL)
        {
        }

        /// <summary>
        /// Constructor accepting a localized message.
        /// </summary>
        /// <param name="pattern"> Message pattern explaining the cause of the error. </param>
        /// <param name="args"> Arguments. </param>
        public MathInternalError(Localizable pattern, params object[] args) : base(pattern, args)
        {
        }
    }

}