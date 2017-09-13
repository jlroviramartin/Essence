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
using System.Runtime.InteropServices;

namespace Essence.Util
{
    public static class ConvertUtils
    {
        public static void Swap<T>(ref T o1, ref T o2)
        {
            T tmp = o1;
            o1 = o2;
            o2 = tmp;
        }

        public static int SingleToInt32Bits(float value)
        {
            //return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            return new Int32SingleUnion(value).AsInt32;
        }

        #region Convert

        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static T ChangeType<T>(this object obj, IFormatProvider provider)
        {
            return (T)Convert.ChangeType(obj, typeof(T), provider);
        }

        public static bool ToBoolean(this IConvertible convertible)
        {
            return convertible.ToBoolean(null);
        }

        public static char ToChar(this IConvertible convertible)
        {
            return convertible.ToChar(null);
        }

        public static sbyte ToSByte(this IConvertible convertible)
        {
            return convertible.ToSByte(null);
        }

        public static byte ToByte(this IConvertible convertible)
        {
            return convertible.ToByte(null);
        }

        public static short ToInt16(this IConvertible convertible)
        {
            return convertible.ToInt16(null);
        }

        public static ushort ToUInt16(this IConvertible convertible)
        {
            return convertible.ToUInt16(null);
        }

        public static int ToInt32(this IConvertible convertible)
        {
            return convertible.ToInt32(null);
        }

        public static uint ToUInt32(this IConvertible convertible)
        {
            return convertible.ToUInt32(null);
        }

        public static long ToInt64(this IConvertible convertible)
        {
            return convertible.ToInt64(null);
        }

        public static ulong ToUInt64(this IConvertible convertible)
        {
            return convertible.ToUInt64(null);
        }

        public static float ToSingle(this IConvertible convertible)
        {
            return convertible.ToSingle(null);
        }

        public static double ToDouble(this IConvertible convertible)
        {
            return convertible.ToDouble(null);
        }

        public static decimal ToDecimal(this IConvertible convertible)
        {
            return convertible.ToDecimal(null);
        }

        public static DateTime ToDateTime(this IConvertible convertible)
        {
            return convertible.ToDateTime(null);
        }

        public static string ToString(this IConvertible convertible)
        {
            return convertible.ToString(null);
        }

        public static object ToType(this IConvertible convertible, Type conversionType)
        {
            return convertible.ToType(conversionType, null);
        }

        public static object ToType<T>(this IConvertible convertible)
        {
            return convertible.ToType(typeof(T), null);
        }

        #endregion

        #region Inner classes

        // http://www.pcreview.co.uk/threads/how-to-do-bitconverter-singletoint32bits.2099564/
        // http://jonskeet.uk/csharp/miscutil/
        [StructLayout(LayoutKind.Explicit)]
        private struct Int32SingleUnion
        {
            /// <summary>
            ///     Int32 version of the value.
            /// </summary>
            [FieldOffset(0)]
            private readonly int i;

            /// <summary>
            ///     Single version of the value.
            /// </summary>
            [FieldOffset(0)]
            private readonly float f;

            /// <summary>
            ///     Creates an instance representing the given integer.
            /// </summary>
            /// <param name="i">The integer value of the new instance./param>
            internal Int32SingleUnion(int i)
            {
                this.f = 0; // Just to keep the compiler happy
                this.i = i;
            }

            /// <summary>
            ///     Creates an instance representing the given floating point
            ///     number.
            /// </summary>
            /// <param name="f">
            ///     The floating point value of the new instance
            /// </param>
            internal Int32SingleUnion(float f)
            {
                this.i = 0; // Just to keep the compiler happy
                this.f = f;
            }

            /// <summary>
            ///     Returns the value of the instance as an integer.
            /// </summary>
            internal int AsInt32
            {
                get { return this.i; }
            }

            /// <summary>
            ///     Returns the value of the instance as a floating point number.
            /// </summary>
            internal float AsSingle
            {
                get { return this.f; }
            }
        }

        #endregion Inner classes
    }
}