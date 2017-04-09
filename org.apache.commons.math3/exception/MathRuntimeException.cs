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
    using ExceptionContext = org.apache.commons.math3.exception.util.ExceptionContext;
    using ExceptionContextProvider = org.apache.commons.math3.exception.util.ExceptionContextProvider;

    /// <summary>
    /// As of release 4.0, all exceptions thrown by the Commons Math code (except
    /// <seealso cref="NullArgumentException"/>) inherit from this class.
    /// In most cases, this class should not be instantiated directly: it should
    /// serve as a base class for implementing exception classes that describe a
    /// specific "problem".
    /// 
    /// @since 3.1
    /// </summary>
    public class MathRuntimeException : Exception, ExceptionContextProvider
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = 20120926L;
        /// <summary>
        /// Context. </summary>
        private readonly ExceptionContext context;

        /// <param name="pattern"> Message pattern explaining the cause of the error. </param>
        /// <param name="args"> Arguments. </param>
        public MathRuntimeException(Localizable pattern, params object[] args)
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
        public override string GetMessage()
        {
            return context.GetMessage();
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public override string GetLocalizedMessage()
        {
            return context.GetLocalizedMessage();
        }
    }

}