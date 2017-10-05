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
    public struct Color3b : IColor3, ITuple3_Byte
    {
        public const string RED = "Red";
        public const string GREEN = "Green";
        public const string BLUE = "Blue";

        public static Color3b FromRGB(byte r, byte g, byte b)
        {
            return new Color3b(r, g, b);
        }

        public Color3b(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public Color3b(IColor3 other)
        {
            ITuple3_Byte _other = other.AsTupleByte();
            this.Red = _other.X;
            this.Green = _other.Y;
            this.Blue = _other.Z;
        }

        #region operators

        public static explicit operator byte[](Color3b v)
        {
            return new[] { v.Red, v.Green, v.Blue };
        }

        #endregion operators

        [Pure]
        public int Dim
        {
            get { return 3; }
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
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public readonly byte Red;
        public readonly byte Green;
        public readonly byte Blue;

        #region private

        [Pure]
        private bool EpsilonEquals(byte r, byte g, byte b, byte epsilon)
        {
            return (this.Red == r) && (this.Green == g) && (this.Blue == b);
        }

        [Pure]
        private bool Equals(byte r, byte g, byte b)
        {
            return (this.Red == r) && (this.Green == g) && (this.Blue == b);
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
            return (obj is IColor3) && this.Equals((IColor3)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.Red, this.Green, this.Blue);
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

        public Color3b(SerializationInfo info, StreamingContext context)
        {
            this.Red = info.GetByte(RED);
            this.Green = info.GetByte(GREEN);
            this.Blue = info.GetByte(BLUE);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(RED, this.Red);
            info.AddValue(GREEN, this.Green);
            info.AddValue(BLUE, this.Blue);
        }

        #endregion

        #region IEpsilonEquatable<IColor3>

        [Pure]
        bool IEpsilonEquatable<IColor3>.EpsilonEquals(IColor3 other, double epsilon)
        {
            ITuple3_Byte _other = other.AsTupleByte();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, (byte)epsilon);
        }

        #endregion

        #region IEpsilonEquatable<Color3b>

        [Pure]
        public bool EpsilonEquals(Color3b other, double epsilon)
        {
            return this.EpsilonEquals(other.Red, other.Green, other.Blue, (byte)epsilon);
        }

        #endregion

        #region IEquatable<IColor3>

        [Pure]
        bool IEquatable<IColor3>.Equals(IColor3 other)
        {
            ITuple3_Byte _other = other.AsTupleByte();
            return this.Equals(_other.X, _other.Y, _other.Z);
        }

        #endregion

        #region IEquatable<Color3b>

        [Pure]
        public bool Equals(Color3b other)
        {
            return this.Equals(other.Red, other.Green, other.Blue);
        }

        #endregion

        #region ITuple3

        public void Get(IOpTuple3 setter)
        {
            IOpTuple3_Byte _setter = setter.AsOpTupleByte();
            _setter.Set(this.Red, this.Green, this.Blue);
        }

        #endregion

        #region ITuple3_Byte

        byte ITuple3_Byte.X
        {
            get { return this.Red; }
        }

        byte ITuple3_Byte.Y
        {
            get { return this.Green; }
        }

        byte ITuple3_Byte.Z
        {
            get { return this.Blue; }
        }

        #endregion
    }
}