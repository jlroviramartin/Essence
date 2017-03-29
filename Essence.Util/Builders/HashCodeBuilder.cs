// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Essence.Util.Builders
{
    /// <summary>
    ///     Constructor para el metodo <c>Object.GetHashCode()</c>.
    ///     Permite de forma sencilla, implementar el metodo
    ///     <c>Object.GetHashCode()</c>.
    ///     <example><![CDATA[
    /// public override int GetHashCode()
    /// {
    ///     return new GetHashCodeBuilder()
    ///         .AppendBase(base.GetHashCode())
    ///         .Append(this.Ancho)
    ///         .Append(this.Alto)
    ///         .GetHashCode();
    /// }
    /// ]]></example>
    /// </summary>
    public struct HashCodeBuilder
    {
        public HashCodeBuilder(int initialNonZeroOddNumber = 17, int multiplierNonZeroOddNumber = 37)
        {
            if (initialNonZeroOddNumber == 0)
            {
                throw new ArgumentException("HashCodeBuilder requires a non zero initial value");
            }
            if (initialNonZeroOddNumber % 2 == 0)
            {
                throw new ArgumentException("HashCodeBuilder requires an odd initial value");
            }
            if (multiplierNonZeroOddNumber == 0)
            {
                throw new ArgumentException("HashCodeBuilder requires a non zero multiplier");
            }
            if (multiplierNonZeroOddNumber % 2 == 0)
            {
                throw new ArgumentException("HashCodeBuilder requires an odd multiplier");
            }
            this.prime = multiplierNonZeroOddNumber;
            this.hashCode = initialNonZeroOddNumber;
        }

        public HashCodeBuilder AppendSuper(int superHashCode)
        {
            this.UpdateHashCode(superHashCode);
            return this;
        }

        public HashCodeBuilder Append(bool value)
        {
            this.UpdateHashCode(value ? 0 : 1);
            return this;
        }

        public HashCodeBuilder Append(bool[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (bool element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(char value)
        {
            this.UpdateHashCode(value);
            return this;
        }

        public HashCodeBuilder Append(char[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (char element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(byte value)
        {
            this.UpdateHashCode(value);
            return this;
        }

        public HashCodeBuilder Append(byte[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (byte element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(sbyte value)
        {
            this.UpdateHashCode(value);
            return this;
        }

        public HashCodeBuilder Append(sbyte[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (sbyte element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(short value)
        {
            this.UpdateHashCode(value);
            return this;
        }

        public HashCodeBuilder Append(short[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (short element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(int value)
        {
            this.UpdateHashCode(value);
            return this;
        }

        public HashCodeBuilder Append(int[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (int element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(long value)
        {
            this.UpdateHashCode(((int)(value ^ (long)((ulong)value >> 32))));
            return this;
        }

        public HashCodeBuilder Append(long[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (long element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(ushort value)
        {
            this.UpdateHashCode(value);
            return this;
        }

        public HashCodeBuilder Append(ushort[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (ushort element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(uint value)
        {
            this.hashCode = unchecked((int)(this.prime * this.hashCode + value));
            return this;
        }

        public HashCodeBuilder Append(uint[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (uint element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(ulong value)
        {
            this.UpdateHashCode(((int)(value ^ (value >> 32))));
            return this;
        }

        public HashCodeBuilder Append(ulong[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (ulong element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(float value)
        {
            this.UpdateHashCode(ConvertUtils.SingleToInt32Bits(value));
            return this;
        }

        public HashCodeBuilder Append(float[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (float element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(double value)
        {
            return this.Append(BitConverter.DoubleToInt64Bits(value));
        }

        public HashCodeBuilder Append(double[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (double element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(decimal value)
        {
            this.hashCode = unchecked((int)(this.prime * this.hashCode + value));
            return this;
        }

        public HashCodeBuilder Append(decimal[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (decimal element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(object value)
        {
            if (value == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                if (value is Array)
                {
                    if (value is long[])
                    {
                        this.Append((long[])value);
                    }
                    else if (value is int[])
                    {
                        this.Append((int[])value);
                    }
                    else if (value is short[])
                    {
                        this.Append((short[])value);
                    }
                    if (value is ulong[])
                    {
                        this.Append((ulong[])value);
                    }
                    else if (value is uint[])
                    {
                        this.Append((uint[])value);
                    }
                    else if (value is ushort[])
                    {
                        this.Append((ushort[])value);
                    }
                    else if (value is byte[])
                    {
                        this.Append((byte[])value);
                    }
                    else if (value is sbyte[])
                    {
                        this.Append((sbyte[])value);
                    }
                    else if (value is double[])
                    {
                        this.Append((double[])value);
                    }
                    else if (value is float[])
                    {
                        this.Append((float[])value);
                    }
                    else if (value is decimal[])
                    {
                        this.Append((decimal[])value);
                    }
                    else if (value is char[])
                    {
                        this.Append((char[])value);
                    }
                    else if (value is bool[])
                    {
                        this.Append((bool[])value);
                    }
                    else
                    {
                        // Not an array of primitives
                        this.Append((object[])value);
                    }
                }
                else
                {
                    this.UpdateHashCode(value.GetHashCode());
                }
            }
            return this;
        }

        public HashCodeBuilder Append(object[] array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (object element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder Append(Array array)
        {
            if (array == null)
            {
                this.UpdateHashCode();
            }
            else
            {
                foreach (object element in array)
                {
                    this.Append(element);
                }
            }
            return this;
        }

        public HashCodeBuilder AppendRange<T>(IEnumerable<T> enumer)
        {
            foreach (T element in enumer)
            {
                this.Append(element);
            }
            return this;
        }

        public HashCodeBuilder AppendRange(IEnumerable enumer)
        {
            foreach (object element in enumer)
            {
                this.Append(element);
            }
            return this;
        }

        public int ToHashCode()
        {
            return this.hashCode;
        }

        public int Build()
        {
            return this.ToHashCode();
        }

        #region privados

        private void UpdateHashCode(int value = 0)
        {
            this.hashCode = unchecked(this.prime * this.hashCode + value);
        }

        private readonly int prime;
        private int hashCode;

        #endregion privados

        #region Object

        public override int GetHashCode()
        {
            return this.ToHashCode();
        }

        #endregion Object
    }
}