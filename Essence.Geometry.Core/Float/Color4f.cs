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
using Essence.Util.Math.Float;

namespace Essence.Geometry.Core.Float
{
    public struct Color4f : IColor4, ITuple4_Float
    {
        public const string RED = "Red";
        public const string GREEN = "Green";
        public const string BLUE = "Blue";
        public const string ALPHA = "Alpha";

        public static Color4f FromRGB(byte r, byte g, byte b, byte a)
        {
            return new Color4f(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        public Color4f(float red, float green, float blue, float alpha)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }

        public Color4f(IColor4 other)
        {
            ITuple4_Float _other = other.AsTupleFloat();
            this.Red = _other.X;
            this.Green = _other.Y;
            this.Blue = _other.Z;
            this.Alpha = _other.W;
        }

        public Color4f(IColor3 other, float alpha)
        {
            ITuple3_Float _other = other.AsTupleFloat();
            this.Red = _other.X;
            this.Green = _other.Y;
            this.Blue = _other.Z;
            this.Alpha = alpha;
        }

        #region operators

        public static explicit operator float[](Color4f v)
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
        public float this[int i]
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

        public readonly float Red;
        public readonly float Green;
        public readonly float Blue;
        public readonly float Alpha;

        #region private

        [Pure]
        private bool EpsilonEquals(float r, float g, float b, float a, float epsilon)
        {
            return this.Red.EpsilonEquals(r, epsilon) && this.Green.EpsilonEquals(g, epsilon) && this.Blue.EpsilonEquals(b, epsilon) && this.Alpha.EpsilonEquals(a, epsilon);
        }

        [Pure]
        private bool Equals(float r, float g, float b, float a)
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

            return VectorUtils.ToString(provider, format, (float[])this);
        }

        #endregion

        #region ISerializable

        public Color4f(SerializationInfo info, StreamingContext context)
        {
            this.Red = info.GetSingle(RED);
            this.Green = info.GetSingle(GREEN);
            this.Blue = info.GetSingle(BLUE);
            this.Alpha = info.GetSingle(ALPHA);
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
            ITuple4_Float _other = other.AsTupleFloat();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, _other.W, (float)epsilon);
        }

        #endregion

        #region IEpsilonEquatable<Color4f>

        [Pure]
        public bool EpsilonEquals(Color4f other, double epsilon)
        {
            return this.EpsilonEquals(other.Red, other.Green, other.Blue, other.Alpha, (float)epsilon);
        }

        #endregion

        #region IEquatable<IColor4>

        [Pure]
        bool IEquatable<IColor4>.Equals(IColor4 other)
        {
            ITuple4_Float _other = other.AsTupleFloat();
            return this.Equals(_other.X, _other.Y, _other.Z, _other.W);
        }

        #endregion

        #region IEquatable<Color4f>

        [Pure]
        public bool Equals(Color4f other)
        {
            return this.Equals(other.Red, other.Green, other.Blue, other.Alpha);
        }

        #endregion

        #region ITuple4_Float

        float ITuple4_Float.X
        {
            get { return this.Red; }
        }

        float ITuple4_Float.Y
        {
            get { return this.Green; }
        }

        float ITuple4_Float.Z
        {
            get { return this.Blue; }
        }

        float ITuple4_Float.W
        {
            get { return this.Alpha; }
        }

        #endregion
    }
}