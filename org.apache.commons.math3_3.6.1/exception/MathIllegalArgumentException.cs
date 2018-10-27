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

using org.apache.commons.math3.exception.util;

namespace org.apache.commons.math3.exception
{

    using Localizable = org.apache.commons.math3.exception.util.Localizable;
    using ExceptionContext = org.apache.commons.math3.exception.util.ExceptionContext;
    using ExceptionContextProvider = org.apache.commons.math3.exception.util.ExceptionContextProvider;

    /// <summary>
    /// Base class for all preconditions violation exceptions.
    /// In most cases, this class should not be instantiated directly: it should
    /// serve as a base class to create all the exceptions that have the semantics
    /// of the standard <seealso cref="IllegalArgumentException"/>.
    /// 
    /// @since 2.2
    /// </summary>
    public class MathIllegalArgumentException : System.ArgumentException, ExceptionContextProvider
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -6024911025449780478L;
        /// <summary>
        /// Context. </summary>
        private readonly ExceptionContext context;

        /// <param name="pattern"> Message pattern explaining the cause of the error. </param>
        /// <param name="args"> Arguments. </param>
        public MathIllegalArgumentException(Localizable pattern, params object[] args)
        {
            context = new ExceptionContext(this);
            context.AddMessage(pattern, args);
        }

        public MathIllegalArgumentException(LocalizedFormats pattern, params object[] args)
        {
            context = new ExceptionContext(this);
            context.AddMessage(pattern, args);
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual ExceptionContext GetContext()
        {
            return context;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual string GetMessage()
        {
            return context.GetMessage();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual string GetLocalizedMessage()
        {
            return context.GetLocalizedMessage();
        }
    }

}