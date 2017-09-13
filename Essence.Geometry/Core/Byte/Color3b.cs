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

        public Color3b(IColor3 c)
        {
            ColorSetter3b setter = new ColorSetter3b();
            c.GetColor(setter);
            this.Red = setter.C1;
            this.Green = setter.C2;
            this.Blue = setter.C3;
        }

        public Color3b(IColor c)
        {
            IColor3 c3 = c as IColor3;
            if (c3 != null)
            {
                ColorSetter3b setter = new ColorSetter3b();
                c3.GetColor(setter);
                this.Red = setter.C1;
                this.Green = setter.C2;
                this.Blue = setter.C3;
            }
            else
            {
                if (c.Dim < 3)
                {
                    throw new Exception("Vector no valido");
                }
                ColorSetter3b setter = new ColorSetter3b();
                c.GetColor(setter);
                this.Red = setter.C1;
                this.Green = setter.C2;
                this.Blue = setter.C3;
            }
        }

        public bool Equals(Color3b other)
        {
            return this.Red == other.Red
                   && this.Green == other.Green
                   && this.Blue == other.Blue;
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

        #region IColor

        [Pure]
        public int Dim
        {
            get { return 3; }
        }

        void IColor.GetColor(IColorSetter setter)
        {
            setter.SetColor(this.Red, this.Green, this.Blue);
        }

        #endregion

        #region IColor3

        public void GetColor(IColorSetter3 setter)
        {
            setter.SetColor(this.Red, this.Green, this.Blue);
        }

        #endregion

        #region IEpsilonEquatable<IPoint>

        [Pure]
        bool IEpsilonEquatable<IColor>.EpsilonEquals(IColor other, double epsilon)
        {
            //return this.EpsilonEquals(other.ToColor3b(), epsilon);
            return this.Equals(other.ToColor3b());
        }

        #endregion

        #region IEquatable<IPoint>

        [Pure]
        bool IEquatable<IColor>.Equals(IColor other)
        {
            return this.Equals(other.ToColor3b());
        }

        #endregion
    }
}