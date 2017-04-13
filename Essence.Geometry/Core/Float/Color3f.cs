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
using Essence.Util.Math;
using Essence.Util.Math.Float;

namespace Essence.Geometry.Core.Float
{
    public struct Color3f : IColor3
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

        public bool Equals(Color3f other)
        {
            return this.Red == other.Red
                   && this.Green == other.Green
                   && this.Blue == other.Blue;
        }

        public bool EpsilonEquals(Color3f other, double epsilon)
        {
            return this.Red.EpsilonEquals(other.Red, (float)epsilon)
                   && this.Green.EpsilonEquals(other.Green, (float)epsilon)
                   && this.Blue.EpsilonEquals(other.Blue, (float)epsilon);
        }

        public readonly float Red;
        public readonly float Green;
        public readonly float Blue;

        #region IColor

        [Pure]
        public int Dim
        {
            get { return 3; }
        }

        [Pure]
        IColorConvertible IColor.this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return new ByteColor(this.Red);
                    case 1:
                        return new ByteColor(this.Green);
                    case 2:
                        return new ByteColor(this.Blue);
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        #region IColor3

        [Pure]
        IColorConvertible IColor3.Red
        {
            get { return new ByteColor(this.Red); }
        }

        [Pure]
        IColorConvertible IColor3.Green
        {
            get { return new ByteColor(this.Green); }
        }

        [Pure]
        IColorConvertible IColor3.Blue
        {
            get { return new ByteColor(this.Blue); }
        }

        #endregion

        #region IEpsilonEquatable<IPoint>

        [Pure]
        bool IEpsilonEquatable<IColor>.EpsilonEquals(IColor other, double epsilon)
        {
            return this.EpsilonEquals(other.ToColor3f(), epsilon);
        }

        #endregion

        #region IEquatable<IPoint>

        [Pure]
        bool IEquatable<IColor>.Equals(IColor other)
        {
            return this.Equals(other.ToColor3f());
        }

        #endregion
    }
}