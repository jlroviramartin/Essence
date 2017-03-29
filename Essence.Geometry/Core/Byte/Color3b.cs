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
using Essence.Util.Math.Float;

namespace Essence.Geometry.Core.Byte
{
    public struct Color3b : IColor3
    {
        public const string RED = "Red";
        public const string GREEN = "Green";
        public const string BLUE = "Blue";

        public static Color3b FromRGB(float r, float g, float b)
        {
            return new Color3b((byte)(r * 255).Clamp(0, 255), (byte)(g * 255).Clamp(0, 255), (byte)(b * 255).Clamp(0, 255));
        }

        public Color3b(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public readonly byte Red;
        public readonly byte Green;
        public readonly byte Blue;

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
    }
}