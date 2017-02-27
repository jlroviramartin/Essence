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

using System;
using org.apache.commons.math3.analysis.exception;

namespace org.apache.commons.math3.util
{
    /// <summary>
    /// Utility that increments a counter until a maximum is reached, at
    /// which point, the instance will by default throw a
    /// <seealso cref="MaxCountExceededException"/>.
    /// However, the user is able to override this behaviour by defining a
    /// custom <seealso cref="MaxCountExceededCallback callback"/>, in order to e.g.
    /// select which exception must be thrown.
    /// 
    /// @since 3.0
    /// @version $Id: Incrementor.java 1455194 2013-03-11 15:45:54Z luc $
    /// </summary>
    public class Incrementor
    {
        /// <summary>
        /// Upper limit for the counter.
        /// </summary>
        private int maximalcount;

        /// <summary>
        /// Current count.
        /// </summary>
        private int count = 0;

        /// <summary>
        /// Function called at counter exhaustion.
        /// </summary>
        private readonly MaxCountExceededCallback maxCountCallback;

        /// <summary>
        /// Default constructor.
        /// For the new instance to be useful, the maximal count must be set
        /// by calling <seealso cref="#setMaximalCount(int) setMaximalCount"/>.
        /// </summary>
        public Incrementor()
            : this(0)
        {
        }

        /// <summary>
        /// Defines a maximal count.
        /// </summary>
        /// <param name="max"> Maximal count. </param>
        public Incrementor(int max)
        {
            this.maximalcount = max;
            this.maxCountCallback = new MaxCountExceededCallbackAnonymousInnerClassHelper(this, max);
        }

        private class MaxCountExceededCallbackAnonymousInnerClassHelper : MaxCountExceededCallback
        {
            private readonly Incrementor outerInstance;

            private int max;

            public MaxCountExceededCallbackAnonymousInnerClassHelper(Incrementor outerInstance, int max)
            {
                this.outerInstance = outerInstance;
                this.max = max;
            }

            /// <summary>
            /// {@inheritDoc} </summary>
            public virtual void Trigger(int max)
            {
                throw new MaxCountExceededException(max);
            }
        }

        /// <summary>
        /// Defines a maximal count and a callback method to be triggered at
        /// counter exhaustion.
        /// </summary>
        /// <param name="max"> Maximal count. </param>
        /// <param name="cb"> Function to be called when the maximal count has been reached. </param>
        /// <exception cref="NullArgumentException"> if {@code cb} is {@code null} </exception>
        public Incrementor(int max, MaxCountExceededCallback cb)
        {
            if (cb == null)
            {
                throw new ArgumentNullException();
            }
            this.maximalcount = max;
            this.maxCountCallback = cb;
        }

        /// <summary>
        /// Sets the upper limit for the counter.
        /// This does not automatically reset the current count to zero (see
        /// <seealso cref="#resetCount()"/>).
        /// </summary>
        /// <param name="max"> Upper limit of the counter. </param>
        public virtual int MaximalCount
        {
            set { this.maximalcount = value; }
            get { return this.maximalcount; }
        }

        /// <summary>
        /// Gets the current count.
        /// </summary>
        /// <returns> the current count. </returns>
        public virtual int Count
        {
            get { return this.count; }
        }

        /// <summary>
        /// Checks whether a single increment is allowed.
        /// </summary>
        /// <returns> {@code false} if the next call to {@link #incrementCount(int)
        /// incrementCount} will trigger a {@code MaxCountExceededException},
        /// {@code true} otherwise. </returns>
        public virtual bool CanIncrement()
        {
            return this.count < this.maximalcount;
        }

        /// <summary>
        /// Performs multiple increments.
        /// See the other <seealso cref="#incrementCount() incrementCount"/> method).
        /// </summary>
        /// <param name="value"> Number of increments. </param>
        /// <exception cref="MaxCountExceededException"> at counter exhaustion. </exception>
        public virtual void IncrementCount(int value)
        {
            for (int i = 0; i < value; i++)
            {
                this.IncrementCount();
            }
        }

        /// <summary>
        /// Adds one to the current iteration count.
        /// At counter exhaustion, this method will call the
        /// <seealso cref="MaxCountExceededCallback#trigger(int) trigger"/> method of the
        /// callback object passed to the
        /// <seealso cref="#Incrementor(int,MaxCountExceededCallback) constructor"/>.
        /// If not explictly set, a default callback is used that will throw
        /// a {@code MaxCountExceededException}.
        /// </summary>
        /// <exception cref="MaxCountExceededException"> at counter exhaustion, unless a
        /// custom <seealso cref="MaxCountExceededCallback callback"/> has been set at
        /// construction. </exception>
        public virtual void IncrementCount()
        {
            if (++this.count > this.maximalcount)
            {
                this.maxCountCallback.Trigger(this.maximalcount);
            }
        }

        /// <summary>
        /// Resets the counter to 0.
        /// </summary>
        public virtual void ResetCount()
        {
            this.count = 0;
        }

        /// <summary>
        /// Defines a method to be called at counter exhaustion.
        /// The <seealso cref="#trigger(int) trigger"/> method should usually throw an exception.
        /// </summary>
        public interface MaxCountExceededCallback
        {
            /// <summary>
            /// Function called when the maximal count has been reached.
            /// </summary>
            /// <param name="maximalCount"> Maximal count. </param>
            /// <exception cref="MaxCountExceededException"> at counter exhaustion </exception>
            void Trigger(int maximalCount);
        }
    }
}