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
namespace org.apache.commons.math3.util
{

    /// <summary>
    /// Generic pair.
    /// <br/>
    /// Although the instances of this class are immutable, it is impossible
    /// to ensure that the references passed to the constructor will not be
    /// modified by the caller.
    /// </summary>
    /// @param <K> Key type. </param>
    /// @param <V> Value type.
    /// 
    /// @since 3.0 </param>
    public class Pair<K, V>
    {
        /// <summary>
        /// Key. </summary>
        private readonly K key;
        /// <summary>
        /// Value. </summary>
        private readonly V value;

        /// <summary>
        /// Create an entry representing a mapping from the specified key to the
        /// specified value.
        /// </summary>
        /// <param name="k"> Key (first element of the pair). </param>
        /// <param name="v"> Value (second element of the pair). </param>
        public Pair(K k, V v)
        {
            key = k;
            value = v;
        }

        /// <summary>
        /// Create an entry representing the same mapping as the specified entry.
        /// </summary>
        /// <param name="entry"> Entry to copy. </param>
        /*public Pair<T1>(Pair<T1> entry) where T1 : K : this(entry.GetKey(), entry.GetValue())
        {
        }*/

        /// <summary>
        /// Get the key.
        /// </summary>
        /// <returns> the key (first element of the pair). </returns>
        public virtual K GetKey()
        {
            return key;
        }

        /// <summary>
        /// Get the value.
        /// </summary>
        /// <returns> the value (second element of the pair). </returns>
        public virtual V GetValue()
        {
            return value;
        }

        /// <summary>
        /// Get the first element of the pair.
        /// </summary>
        /// <returns> the first element of the pair.
        /// @since 3.1 </returns>
        public virtual K GetFirst()
        {
            return key;
        }

        /// <summary>
        /// Get the second element of the pair.
        /// </summary>
        /// <returns> the second element of the pair.
        /// @since 3.1 </returns>
        public virtual V GetSecond()
        {
            return value;
        }

        /// <summary>
        /// Compare the specified object with this entry for equality.
        /// </summary>
        /// <param name="o"> Object. </param>
        /// <returns> {@code true} if the given object is also a map entry and
        /// the two entries represent the same mapping. </returns>
        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is Pair<K, V>))
            {
                return false;
            }
            else
            {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: Pair<?, ?> oP = (Pair<?, ?>) o;
                Pair<K, V> oP = (Pair<K, V>) o;
                return (key == null ? oP.key == null : key.Equals(oP.key)) && (value == null ? oP.value == null : value.Equals(oP.value));
            }
        }

        /// <summary>
        /// Compute a hash code.
        /// </summary>
        /// <returns> the hash code value. </returns>
        public override int GetHashCode()
        {
            int result = key == null ? 0 : key.GetHashCode();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int h = value == null ? 0 : value.hashCode();
            int h = value == null ? 0 : value.GetHashCode();
            result = 37 * result + h ^ ((int)((uint)h >> 16));

            return result;
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        public override string ToString()
        {
            return "[" + GetKey() + ", " + GetValue() + "]";
        }

        /// <summary>
        /// Convenience factory method that calls the
        /// <seealso cref="#Pair(Object, Object) constructor"/>.
        /// </summary>
        /// @param <K> the key type </param>
        /// @param <V> the value type </param>
        /// <param name="k"> First element of the pair. </param>
        /// <param name="v"> Second element of the pair. </param>
        /// <returns> a new {@code Pair} containing {@code k} and {@code v}.
        /// @since 3.3 </returns>
        public static Pair<K, V> create<K, V>(K k, V v)
        {
            return new Pair<K, V>(k, v);
        }
    }

}