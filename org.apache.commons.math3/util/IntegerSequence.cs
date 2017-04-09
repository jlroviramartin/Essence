/// Apache Commons Math 3.6.1
using System.Collections.Generic;

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
    using MathUnsupportedOperationException = org.apache.commons.math3.exception.MathUnsupportedOperationException;
    using NotStrictlyPositiveException = org.apache.commons.math3.exception.NotStrictlyPositiveException;
    using ZeroException = org.apache.commons.math3.exception.ZeroException;

    /// <summary>
    /// Provides a sequence of integers.
    /// 
    /// @since 3.6
    /// </summary>
    public class IntegerSequence
    {
        /// <summary>
        /// Utility class contains only static methods.
        /// </summary>
        private IntegerSequence()
        {
        }

        /// <summary>
        /// Creates a sequence {@code [start .. end]}.
        /// It calls <seealso cref="#range(int,int,int) range(start, end, 1)"/>.
        /// </summary>
        /// <param name="start"> First value of the range. </param>
        /// <param name="end"> Last value of the range. </param>
        /// <returns> a range. </returns>
        public static Range NewRange(int start, int end)
        {
            return Range(start, end, 1);
        }

        /// <summary>
        /// Creates a sequence \( a_i, i < 0 <= n \)
        /// where \( a_i = start + i * step \)
        /// and \( n \) is such that \( a_n <= max \) and \( a_{n+1} > max \).
        /// </summary>
        /// <param name="start"> First value of the range. </param>
        /// <param name="max"> Last value of the range that satisfies the above
        /// construction rule. </param>
        /// <param name="step"> Increment. </param>
        /// <returns> a range. </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static Range range(final int start, final int max, final int step)
        public static Range NewRange(int start, int max, int step)
        {
            return new Range(start, max, step);
        }

        /// <summary>
        /// Generates a sequence of integers.
        /// </summary>
        public class Range : IEnumerable<int?>
        {
            /// <summary>
            /// Number of integers contained in this range. </summary>
            internal readonly int size;
            /// <summary>
            /// First value. </summary>
            internal readonly int start;
            /// <summary>
            /// Final value. </summary>
            internal readonly int max;
            /// <summary>
            /// Increment. </summary>
            internal readonly int step;

            /// <summary>
            /// Creates a sequence \( a_i, i < 0 <= n \)
            /// where \( a_i = start + i * step \)
            /// and \( n \) is such that \( a_n <= max \) and \( a_{n+1} > max \).
            /// </summary>
            /// <param name="start"> First value of the range. </param>
            /// <param name="max"> Last value of the range that satisfies the above
            /// construction rule. </param>
            /// <param name="step"> Increment. </param>
            public Range(int start, int max, int step)
            {
                this.start = start;
                this.max = max;
                this.step = step;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int s = (max - start) / step + 1;
                int s = (max - start) / step + 1;
                this.size = s < 0 ? 0 : s;
            }

            /// <summary>
            /// Gets the number of elements contained in the range.
            /// </summary>
            /// <returns> the size of the range. </returns>
            public virtual int Size()
            {
                return size;
            }

            /// <summary>
            /// {@inheritDoc} </summary>
            public virtual IEnumerator<int?> GetEnumerator()
            {
                return Incrementor.Create().WithStart(start).WithMaximalCount(max + (step > 0 ? 1 : -1)).WithIncrement(step);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Utility that increments a counter until a maximum is reached, at
        /// which point, the instance will by default throw a
        /// <seealso cref="MaxCountExceededException"/>.
        /// However, the user is able to override this behaviour by defining a
        /// custom <seealso cref="MaxCountExceededCallback callback"/>, in order to e.g.
        /// select which exception must be thrown.
        /// </summary>
        public class Incrementor : IEnumerator<int?>
        {
            /// <summary>
            /// Default callback. </summary>
            internal static readonly MaxCountExceededCallback CALLBACK = new MaxCountExceededCallbackAnonymousInnerClass();

            private class MaxCountExceededCallbackAnonymousInnerClass : MaxCountExceededCallback
            {
                public MaxCountExceededCallbackAnonymousInnerClass()
                {
                }

                                /// <summary>
                                /// {@inheritDoc} </summary>
                public virtual void Trigger(int max)
                {
                    throw new MaxCountExceededException(max);
                }
            }

            /// <summary>
            /// Initial value the counter. </summary>
            internal readonly int init;
            /// <summary>
            /// Upper limit for the counter. </summary>
            internal readonly int maximalCount;
            /// <summary>
            /// Increment. </summary>
            internal readonly int increment;
            /// <summary>
            /// Function called at counter exhaustion. </summary>
            internal readonly MaxCountExceededCallback maxCountCallback;
            /// <summary>
            /// Current count. </summary>
            internal int count = 0;

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
            /// Creates an incrementor.
            /// The counter will be exhausted either when {@code max} is reached
            /// or when {@code nTimes} increments have been performed.
            /// </summary>
            /// <param name="start"> Initial value. </param>
            /// <param name="max"> Maximal count. </param>
            /// <param name="step"> Increment. </param>
            /// <param name="cb"> Function to be called when the maximal count has been reached. </param>
            /// <exception cref="NullArgumentException"> if {@code cb} is {@code null}. </exception>
            internal Incrementor(int start, int max, int step, MaxCountExceededCallback cb)
            {
                if (cb == null)
                {
                    throw new NullArgumentException();
                }
                this.init = start;
                this.maximalCount = max;
                this.increment = step;
                this.maxCountCallback = cb;
                this.count = start;
            }

            /// <summary>
            /// Factory method that creates a default instance.
            /// The initial and maximal values are set to 0.
            /// For the new instance to be useful, the maximal count must be set
            /// by calling <seealso cref="#withMaximalCount(int) withMaximalCount"/>.
            /// </summary>
            /// <returns> an new instance. </returns>
            public static Incrementor Create()
            {
                return new Incrementor(0, 0, 1, CALLBACK);
            }

            /// <summary>
            /// Creates a new instance with a given initial value.
            /// The counter is reset to the initial value.
            /// </summary>
            /// <param name="start"> Initial value of the counter. </param>
            /// <returns> a new instance. </returns>
            public virtual Incrementor WithStart(int start)
            {
                return new Incrementor(start, this.maximalCount, this.increment, this.maxCountCallback);
            }

            /// <summary>
            /// Creates a new instance with a given maximal count.
            /// The counter is reset to the initial value.
            /// </summary>
            /// <param name="max"> Maximal count. </param>
            /// <returns> a new instance. </returns>
            public virtual Incrementor WithMaximalCount(int max)
            {
                return new Incrementor(this.init, max, this.increment, this.maxCountCallback);
            }

            /// <summary>
            /// Creates a new instance with a given increment.
            /// The counter is reset to the initial value.
            /// </summary>
            /// <param name="step"> Increment. </param>
            /// <returns> a new instance. </returns>
            public virtual Incrementor WithIncrement(int step)
            {
                if (step == 0)
                {
                    throw new ZeroException();
                }
                return new Incrementor(this.init, this.maximalCount, step, this.maxCountCallback);
            }

            /// <summary>
            /// Creates a new instance with a given callback.
            /// The counter is reset to the initial value.
            /// </summary>
            /// <param name="cb"> Callback to be called at counter exhaustion. </param>
            /// <returns> a new instance. </returns>
            public virtual Incrementor WithCallback(MaxCountExceededCallback cb)
            {
                return new Incrementor(this.init, this.maximalCount, this.increment, cb);
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
            /// Checks whether incrementing the counter {@code nTimes} is allowed.
            /// </summary>
            /// <returns> {@code false} if calling <seealso cref="#increment()"/>
            /// will trigger a {@code MaxCountExceededException},
            /// {@code true} otherwise. </returns>
            public virtual bool CanIncrement()
            {
                return CanIncrement(1);
            }

            /// <summary>
            /// Checks whether incrementing the counter several times is allowed.
            /// </summary>
            /// <param name="nTimes"> Number of increments. </param>
            /// <returns> {@code false} if calling {@link #increment(int)
            /// increment(nTimes)} would call the <seealso cref="MaxCountExceededCallback callback"/>
            /// {@code true} otherwise. </returns>
            public virtual bool CanIncrement(int nTimes)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int finalCount = count + nTimes * increment;
                int finalCount = count + nTimes * increment;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return increment < 0 ? finalCount > maximalCount : finalCount < maximalCount;
                return increment < 0 ? finalCount > maximalCount : finalCount < maximalCount;
            }

            /// <summary>
            /// Performs multiple increments.
            /// </summary>
            /// <param name="nTimes"> Number of increments. </param>
            /// <exception cref="MaxCountExceededException"> at counter exhaustion. </exception>
            /// <exception cref="NotStrictlyPositiveException"> if {@code nTimes <= 0}.
            /// </exception>
            /// <seealso cref= #increment() </seealso>
            public virtual void Increment(int nTimes)
            {
                if (nTimes <= 0)
                {
                    throw new NotStrictlyPositiveException(nTimes);
                }

                if (!CanIncrement(0))
                {
                    maxCountCallback.Trigger(maximalCount);
                }
                count += nTimes * increment;
            }

            /// <summary>
            /// Adds the increment value to the current iteration count.
            /// At counter exhaustion, this method will call the
            /// <seealso cref="MaxCountExceededCallback#trigger(int) trigger"/> method of the
            /// callback object passed to the
            /// <seealso cref="#withCallback(MaxCountExceededCallback)"/> method.
            /// If not explicitly set, a default callback is used that will throw
            /// a {@code MaxCountExceededException}.
            /// </summary>
            /// <exception cref="MaxCountExceededException"> at counter exhaustion, unless a
            /// custom <seealso cref="MaxCountExceededCallback callback"/> has been set.
            /// </exception>
            /// <seealso cref= #increment(int) </seealso>
            public virtual void Increment()
            {
                Increment(1);
            }

            /// <summary>
            /// {@inheritDoc} </summary>
            public virtual bool HasNext()
            {
                return CanIncrement(0);
            }

            /// <summary>
            /// {@inheritDoc} </summary>
            public virtual int? Next()
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int value = count;
                int value = count;
                Increment();
                return value;
            }

            /// <summary>
            /// Not applicable.
            /// </summary>
            /// <exception cref="MathUnsupportedOperationException"> </exception>
            public virtual void Remove()
            {
                throw new MathUnsupportedOperationException();
            }
        }
    }

}