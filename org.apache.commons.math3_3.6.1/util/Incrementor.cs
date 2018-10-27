/// Apache Commons Math 3.6.1
using System;
using System.CodeDom.Compiler;

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
namespace org.apache.commons.math3.util
{

    using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
    using NullArgumentException = org.apache.commons.math3.exception.NullArgumentException;

    /// <summary>
    /// Utility that increments a counter until a maximum is reached, at
    /// which point, the instance will by default throw a
    /// <seealso cref="MaxCountExceededException"/>.
    /// However, the user is able to override this behaviour by defining a
    /// custom <seealso cref="MaxCountExceededCallback callback"/>, in order to e.g.
    /// select which exception must be thrown.
    /// 
    /// @since 3.0 </summary>
    /// @deprecated Use <seealso cref="IntegerSequence.Incrementor"/> instead. 
    [Obsolete("Use <seealso cref=\"IntegerSequence.Incrementor\"/> instead.")]
    public class Incrementor
    {
        /// <summary>
        /// Upper limit for the counter.
        /// </summary>
        private int maximalCount;
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
        public Incrementor() : this(0)
        {
        }

#if false
        /// <summary>
        /// Defines a maximal count.
        /// </summary>
        /// <param name="max"> Maximal count. </param>
        public Incrementor(int max) : this(max, new MaxCountExceededCallbackAnonymousInnerClass(this, max))
        {
        }
#endif
        private class MaxCountExceededCallbackAnonymousInnerClass : MaxCountExceededCallback
        {
            private readonly Incrementor outerInstance;

            private int max;

            public MaxCountExceededCallbackAnonymousInnerClass(Incrementor outerInstance, int max)
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
        public Incrementor(int max, MaxCountExceededCallback cb = null)
        {
            cb = cb ?? new MaxCountExceededCallbackAnonymousInnerClass(this, max);
            if (cb == null)
            {
                throw new NullArgumentException();
            }
            maximalCount = max;
            maxCountCallback = cb;
        }

        /// <summary>
        /// Sets the upper limit for the counter.
        /// This does not automatically reset the current count to zero (see
        /// <seealso cref="#resetCount()"/>).
        /// </summary>
        /// <param name="max"> Upper limit of the counter. </param>
        public virtual void SetMaximalCount(int max)
        {
            maximalCount = max;
        }

        /// <summary>
        /// Gets the upper limit of the counter.
        /// </summary>
        /// <returns> the counter upper limit. </returns>
        public virtual int GetMaximalCount()
        {
            return maximalCount;
        }

        /// <summary>
        /// Gets the current count.
        /// </summary>
        /// <returns> the current count. </returns>
        public virtual int GetCount()
        {
            return count;
        }

        /// <summary>
        /// Checks whether a single increment is allowed.
        /// </summary>
        /// <returns> {@code false} if the next call to {@link #incrementCount(int)
        /// incrementCount} will trigger a {@code MaxCountExceededException},
        /// {@code true} otherwise. </returns>
        public virtual bool CanIncrement()
        {
            return count < maximalCount;
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
                IncrementCount();
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
            if (++count > maximalCount)
            {
                maxCountCallback.Trigger(maximalCount);
            }
        }

        /// <summary>
        /// Resets the counter to 0.
        /// </summary>
        public virtual void ResetCount()
        {
            count = 0;
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

        /// <summary>
        /// Create an instance that delegates everything to a <seealso cref="IntegerSequence.Incrementor"/>.
        /// <para>
        /// This factory method is intended only as a temporary hack for internal use in
        /// Apache Commons Math 3.X series, when {@code Incrementor} is required in
        /// interface (as a return value or in protected fields). It should <em>not</em>
        /// be used in other cases. The <seealso cref="IntegerSequence.Incrementor"/> class should
        /// be used instead of {@code Incrementor}.
        /// </para>
        /// <para>
        /// All methods are mirrored to the underlying <seealso cref="IntegerSequence.Incrementor"/>,
        /// as long as neither <seealso cref="#setMaximalCount(int)"/> nor <seealso cref="#resetCount()"/> are called.
        /// If one of these two methods is called, the created instance becomes independent
        /// of the <seealso cref="IntegerSequence.Incrementor"/> used at creation. The rationale is that
        /// <seealso cref="IntegerSequence.Incrementor"/> cannot change their maximal count and cannot be reset.
        /// </para> </summary>
        /// <param name="incrementor"> wrapped <seealso cref="IntegerSequence.Incrementor"/> </param>
        /// <returns> an incrementor wrapping an <seealso cref="IntegerSequence.Incrementor"/>
        /// @since 3.6 </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static Incrementor wrap(final IntegerSequence.Incrementor incrementor)
        public static Incrementor Wrap(IntegerSequence.Incrementor incrementor)
        {
            return new IncrementorAnonymousInnerClass(incrementor);
        }

        private class IncrementorAnonymousInnerClass : Incrementor
        {
            private org.apache.commons.math3.util.IntegerSequence.Incrementor incrementor;

            public IncrementorAnonymousInnerClass(org.apache.commons.math3.util.IntegerSequence.Incrementor incrementor)
            {
                this.incrementor = incrementor;
            }


                    /// <summary>
                    /// Underlying incrementor. </summary>
            private IntegerSequence.Incrementor @delegate;

    //        {
    //            // set up matching values at initialization
    //            @delegate = incrementor;
    //            base.setMaximalCount(@delegate.getMaximalCount());
    //            base.incrementCount(@delegate.getCount());
    //        }

            /// <summary>
            /// {@inheritDoc} </summary>
            public override void SetMaximalCount(int max)
            {
                base.SetMaximalCount(max);
                @delegate = @delegate.WithMaximalCount(max);
            }

            /// <summary>
            /// {@inheritDoc} </summary>
            public override void ResetCount()
            {
                base.ResetCount();
                @delegate = @delegate.WithStart(0);
            }

            /// <summary>
            /// {@inheritDoc} </summary>
            public override void IncrementCount()
            {
                base.IncrementCount();
                @delegate.Increment();
            }

        }

    }

}