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
namespace org.apache.commons.math3.exception.util
{

    /// <summary>
    /// Dummy implementation of the <seealso cref="Localizable"/> interface, without localization.
    /// 
    /// @since 2.2
    /// </summary>
    [Serializable]
    public class DummyLocalizable : Localizable
    {

        /// <summary>
        /// Serializable version identifier. </summary>
        private const long serialVersionUID = 8843275624471387299L;

        /// <summary>
        /// Source string. </summary>
        private readonly string source;

        /// <summary>
        /// Simple constructor. </summary>
        /// <param name="source"> source text </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public DummyLocalizable(final String source)
        public DummyLocalizable(string source)
        {
            this.source = source;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual string GetSourceString()
        {
            return source;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public virtual string GetLocalizedString(Locale locale)
        {
            return source;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public override string ToString()
        {
            return source;
        }

    }

}