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
    /// Exception to be thrown when function values have the same sign at both
    /// ends of an interval.
    /// 
    /// @since 3.0
    /// </summary>
    public class NoBracketingException : MathIllegalArgumentException
    {
        /// <summary>
        /// Serializable version Id. </summary>
        private const long serialVersionUID = -3629324471511904459L;
        /// <summary>
        /// Lower end of the interval. </summary>
        private readonly double lo;
        /// <summary>
        /// Higher end of the interval. </summary>
        private readonly double hi;
        /// <summary>
        /// Value at lower end of the interval. </summary>
        private readonly double fLo;
        /// <summary>
        /// Value at higher end of the interval. </summary>
        private readonly double fHi;

        /// <summary>
        /// Construct the exception.
        /// </summary>
        /// <param name="lo"> Lower end of the interval. </param>
        /// <param name="hi"> Higher end of the interval. </param>
        /// <param name="fLo"> Value at lower end of the interval. </param>
        /// <param name="fHi"> Value at higher end of the interval. </param>
        public NoBracketingException(double lo, double hi, double fLo, double fHi) : this(LocalizedFormats.SAME_SIGN_AT_ENDPOINTS, lo, hi, fLo, fHi)
        {
        }

        /// <summary>
        /// Construct the exception with a specific context.
        /// </summary>
        /// <param name="specific"> Contextual information on what caused the exception. </param>
        /// <param name="lo"> Lower end of the interval. </param>
        /// <param name="hi"> Higher end of the interval. </param>
        /// <param name="fLo"> Value at lower end of the interval. </param>
        /// <param name="fHi"> Value at higher end of the interval. </param>
        /// <param name="args"> Additional arguments. </param>
        public NoBracketingException(Localizable specific, double lo, double hi, double fLo, double fHi, params object[] args) : base(specific, Convert.ToDouble(lo), Convert.ToDouble(hi), Convert.ToDouble(fLo), Convert.ToDouble(fHi), args)
        {
            this.lo = lo;
            this.hi = hi;
            this.fLo = fLo;
            this.fHi = fHi;
        }

        /// <summary>
        /// Get the lower end of the interval.
        /// </summary>
        /// <returns> the lower end. </returns>
        public virtual double GetLo()
        {
            return lo;
        }
        /// <summary>
        /// Get the higher end of the interval.
        /// </summary>
        /// <returns> the higher end. </returns>
        public virtual double GetHi()
        {
            return hi;
        }
        /// <summary>
        /// Get the value at the lower end of the interval.
        /// </summary>
        /// <returns> the value at the lower end. </returns>
        public virtual double GetFLo()
        {
            return fLo;
        }
        /// <summary>
        /// Get the value at the higher end of the interval.
        /// </summary>
        /// <returns> the value at the higher end. </returns>
        public virtual double GetFHi()
        {
            return fHi;
        }
    }

}