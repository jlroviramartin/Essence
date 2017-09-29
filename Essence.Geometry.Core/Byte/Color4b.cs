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
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using Essence.Util.Math;

namespace Essence.Geometry.Core.Byte
{
    public struct Color4b : IColor4, ITuple4_Byte
    {
        public const string RED = "Red";
        public const string GREEN = "Green";
        public const string BLUE = "Blue";
        public const string ALPHA = "Alpha";

        public static Color4b FromRGB(byte r, byte g, byte b, byte a)
        {
            return new Color4b(r, g, b, a);
        }

        public Color4b(byte red, byte green, byte blue, byte alpha)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }

        public Color4b(IColor4 other)
        {
            ITuple4_Byte _other = other.AsTupleByte();
            this.Red = _other.X;
            this.Green = _other.Y;
            this.Blue = _other.Z;
            this.Alpha = _other.W;
        }

        public Color4b(IColor3 other, byte alpha)
        {
            ITuple3_Byte _other = other.AsTupleByte();
            this.Red = _other.X;
            this.Green = _other.Y;
            this.Blue = _other.Z;
            this.Alpha = alpha;
        }

        #region operators

        public static explicit operator byte[](Color4b v)
        {
            return new[] { v.Red, v.Green, v.Blue, v.Alpha };
        }

        #endregion operators

        [Pure]
        public int Dim
        {
            get { return 4; }
        }

        [Pure]
        public byte this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return this.Red;
                    case 1:
                        return this.Green;
                    case 2:
                        return this.Blue;
                    case 3:
                        return this.Alpha;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public readonly byte Red;
        public readonly byte Green;
        public readonly byte Blue;
        public readonly byte Alpha;

        #region private

        [Pure]
        private bool EpsilonEquals(byte r, byte g, byte b, byte a, byte epsilon)
        {
            return (this.Red == r) && (this.Green == g) && (this.Blue == b) && (this.Alpha == a);
        }

        [Pure]
        private bool Equals(byte r, byte g, byte b, byte a)
        {
            return (this.Red == r) && (this.Green == g) && (this.Blue == b) && (this.Alpha == a);
        }

        #endregion

        #region object

        [Pure]
        public override string ToString()
        {
            return this.ToString("F3", null);
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return (obj is IColor4) && this.Equals((IColor4)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.Red, this.Green, this.Blue, this.Alpha);
        }

        #endregion

        #region IFormattable

        [Pure]
        public string ToString(string format, IFormatProvider provider)
        {
            if (provider != null)
            {
                ICustomFormatter formatter = provider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(format, this, provider);
                }
            }

            return VectorUtils.ToString(provider, format, (byte[])this);
        }

        #endregion

        #region ISerializable

        public Color4b(SerializationInfo info, StreamingContext context)
        {
            this.Red = info.GetByte(RED);
            this.Green = info.GetByte(GREEN);
            this.Blue = info.GetByte(BLUE);
            this.Alpha = info.GetByte(ALPHA);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(RED, this.Red);
            info.AddValue(GREEN, this.Green);
            info.AddValue(BLUE, this.Blue);
            info.AddValue(ALPHA, this.Alpha);
        }

        #endregion

        #region IEpsilonEquatable<IColor4>

        [Pure]
        bool IEpsilonEquatable<IColor4>.EpsilonEquals(IColor4 other, double epsilon)
        {
            ITuple4_Byte _other = other.AsTupleByte();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, _other.W, (byte)epsilon);
        }

        #endregion

        #region IEpsilonEquatable<Color4b>

        [Pure]
        public bool EpsilonEquals(Color4b other, double epsilon)
        {
            return this.EpsilonEquals(other.Red, other.Green, other.Blue, other.Alpha, (byte)epsilon);
        }

        #endregion

        #region IEquatable<IColor4>

        [Pure]
        bool IEquatable<IColor4>.Equals(IColor4 other)
        {
            ITuple4_Byte _other = other.AsTupleByte();
            return this.Equals(_other.X, _other.Y, _other.Z, _other.W);
        }

        #endregion

        #region IEquatable<Color4b>

        [Pure]
        public bool Equals(Color4b other)
        {
            return this.Equals(other.Red, other.Green, other.Blue, other.Alpha);
        }

        #endregion

        #region ITuple4_Byte

        byte ITuple4_Byte.X
        {
            get { return this.Red; }
        }

        byte ITuple4_Byte.Y
        {
            get { return this.Green; }
        }

        byte ITuple4_Byte.Z
        {
            get { return this.Blue; }
        }

        byte ITuple4_Byte.W
        {
            get { return this.Alpha; }
        }

        #endregion
    }
}