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

namespace Essence.Geometry.Core.Byte
{
    public struct Color4b : IColor4
    {
        public const string RED = "Red";
        public const string GREEN = "Green";
        public const string BLUE = "Blue";
        public const string ALPHA = "Alpha";

        public static Color4b FromRGB(float r, float g, float b, float a)
        {
            return new Color4b((byte)(r * 255).Clamp(0, 255), (byte)(g * 255).Clamp(0, 255), (byte)(b * 255).Clamp(0, 255), (byte)(a * 255).Clamp(0, 255));
        }

        public Color4b(byte red, byte green, byte blue, byte alpha)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }

        public Color4b(IColor4 c)
        {
            ColorSetter4b setter = new ColorSetter4b();
            c.GetColor(setter);
            this.Red = setter.C1;
            this.Green = setter.C2;
            this.Blue = setter.C3;
            this.Alpha = setter.C3;
        }

        public Color4b(IColor c)
        {
            IColor3 c4 = c as IColor3;
            if (c4 != null)
            {
                ColorSetter4b setter = new ColorSetter4b();
                c4.GetColor(setter);
                this.Red = setter.C1;
                this.Green = setter.C2;
                this.Blue = setter.C3;
                this.Alpha = setter.C3;
            }
            else
            {
                if (c.Dim < 3)
                {
                    throw new Exception("Vector no valido");
                }
                ColorSetter4b setter = new ColorSetter4b();
                c.GetColor(setter);
                this.Red = setter.C1;
                this.Green = setter.C2;
                this.Blue = setter.C3;
                this.Alpha = setter.C3;
            }
        }

        public bool Equals(Color4b other)
        {
            return this.Red == other.Red
                   && this.Green == other.Green
                   && this.Blue == other.Blue
                   && this.Alpha == other.Alpha;
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

        #region IColor

        [Pure]
        public int Dim
        {
            get { return 4; }
        }

        void IColor.GetColor(IColorSetter setter)
        {
            setter.SetColor(this.Red, this.Green, this.Blue, this.Alpha);
        }

        #endregion

        #region IColor4

        public void GetColor(IColorSetter4 setter)
        {
            setter.SetColor(this.Red, this.Green, this.Blue, this.Alpha);
        }

        #endregion

        #region IEpsilonEquatable<IPoint>

        [Pure]
        bool IEpsilonEquatable<IColor>.EpsilonEquals(IColor other, double epsilon)
        {
            //return this.EpsilonEquals(other.ToColor4b(), epsilon);
            return this.Equals(other.ToColor4b());
        }

        #endregion

        #region IEquatable<IPoint>

        [Pure]
        bool IEquatable<IColor>.Equals(IColor other)
        {
            return this.Equals(other.ToColor4b());
        }

        #endregion
    }
}