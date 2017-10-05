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
    public struct Color3f : IColor3, ITuple3_Float
    {
        public const string RED = "Red";
        public const string GREEN = "Green";
        public const string BLUE = "Blue";

        public static Color3f FromRGB(byte r, byte g, byte b)
        {
            return new Color3f(r / 255f, g / 255f, b / 255f);
        }

        public Color3f(float red, float green, float blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public Color3f(IColor3 other)
        {
            ITuple3_Float _other = other.AsTupleFloat();
            this.Red = _other.X;
            this.Green = _other.Y;
            this.Blue = _other.Z;
        }

        #region operators

        public static explicit operator float[](Color3f v)
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
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public readonly float Red;
        public readonly float Green;
        public readonly float Blue;

        #region private

        [Pure]
        private bool EpsilonEquals(float r, float g, float b, float epsilon)
        {
            return this.Red.EpsilonEquals(r, epsilon) && this.Green.EpsilonEquals(g, epsilon) && this.Blue.EpsilonEquals(b, epsilon);
        }

        [Pure]
        private bool Equals(float r, float g, float b)
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

            return VectorUtils.ToString(provider, format, (float[])this);
        }

        #endregion

        #region ISerializable

        public Color3f(SerializationInfo info, StreamingContext context)
        {
            this.Red = info.GetSingle(RED);
            this.Green = info.GetSingle(GREEN);
            this.Blue = info.GetSingle(BLUE);
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
            ITuple3_Float _other = other.AsTupleFloat();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, (float)epsilon);
        }

        #endregion

        #region IEpsilonEquatable<Color3f>

        [Pure]
        public bool EpsilonEquals(Color3f other, double epsilon)
        {
            return this.EpsilonEquals(other.Red, other.Green, other.Blue, (float)epsilon);
        }

        #endregion

        #region IEquatable<IColor3>

        [Pure]
        bool IEquatable<IColor3>.Equals(IColor3 other)
        {
            ITuple3_Float _other = other.AsTupleFloat();
            return this.Equals(_other.X, _other.Y, _other.Z);
        }

        #endregion

        #region IEquatable<Color3f>

        [Pure]
        public bool Equals(Color3f other)
        {
            return this.Equals(other.Red, other.Green, other.Blue);
        }

        #endregion

        #region ITuple3

        public void Get(IOpTuple3 setter)
        {
            IOpTuple3_Float _setter = setter.AsOpTupleFloat();
            _setter.Set(this.Red, this.Green, this.Blue);
        }

        #endregion

        #region ITuple3_Float

        float ITuple3_Float.X
        {
            get { return this.Red; }
        }

        float ITuple3_Float.Y
        {
            get { return this.Green; }
        }

        float ITuple3_Float.Z
        {
            get { return this.Blue; }
        }

        #endregion
    }
}