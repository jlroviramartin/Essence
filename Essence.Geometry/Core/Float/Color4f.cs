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

namespace Essence.Geometry.Core.Float
{
    public struct Color4f : IColor4
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

        public readonly float Red;
        public readonly float Green;
        public readonly float Blue;
        public readonly float Alpha;

        #region IColor

        [Pure]
        public int Dim
        {
            get { return 4; }
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
                    case 3:
                        return new ByteColor(this.Alpha);
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
            get { return new FloatColor(this.Red); }
        }

        [Pure]
        IColorConvertible IColor3.Green
        {
            get { return new FloatColor(this.Green); }
        }

        [Pure]
        IColorConvertible IColor3.Blue
        {
            get { return new FloatColor(this.Blue); }
        }

        #endregion

        #region IColor4

        [Pure]
        IColorConvertible IColor4.Alpha
        {
            get { return new FloatColor(this.Alpha); }
        }

        #endregion
    }
}