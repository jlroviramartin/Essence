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
using Essence.Util.Math.Float;

namespace Essence.Geometry.Core
{
    public interface IColorConvertible
    {
        byte ToByte(IFormatProvider provider = null);

        float ToSingle(IFormatProvider provider = null);
    }

    public struct ByteColor : IColorConvertible
    {
        public ByteColor(byte value)
        {
            this.value = value;
        }

        public ByteColor(float value)
        {
            this.value = FloatColor.GetByte(value);
        }

        public static float GetSingle(byte value)
        {
            return (float)value / 255f;
        }

        private readonly byte value;

        public byte ToByte(IFormatProvider provider)
        {
            return this.value;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return GetSingle(this.value);
        }
    }

    public struct FloatColor : IColorConvertible
    {
        public FloatColor(byte value)
        {
            this.value = ByteColor.GetSingle(value);
        }

        public FloatColor(float value)
        {
            this.value = value;
        }

        public static byte GetByte(float value)
        {
            return (byte)(value * 255).Clamp(0, 255);
        }

        private readonly float value;

        public byte ToByte(IFormatProvider provider)
        {
            return GetByte(this.value);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return this.value;
        }
    }
}