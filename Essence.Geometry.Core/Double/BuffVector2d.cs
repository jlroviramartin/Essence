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
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Core.Double
{
    public class BuffVector2d
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        public BuffVector2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public BuffVector2d(IVector2D v)
        {
            CoordinateSetter2d setter = new CoordinateSetter2d();
            v.GetCoordinates(setter);
            this.X = setter.X;
            this.Y = setter.Y;
        }

        public BuffVector2d(IVector v)
        {
            IVector2D v2 = v as IVector2D;
            if (v2 != null)
            {
                CoordinateSetter2d setter = new CoordinateSetter2d();
                v2.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
            }
            else
            {
                if (v.Dim < 2)
                {
                    throw new Exception("Vector no valido");
                }
                CoordinateSetter2d setter = new CoordinateSetter2d();
                v.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
            }
        }

        /// <summary>Property X.</summary>
        public double X;

        /// <summary>Property Y.</summary>
        public double Y;

        public static explicit operator double[](BuffVector2d v)
        {
            return new[] { v.X, v.Y };
        }

        public static explicit operator Vector2d(BuffVector2d v)
        {
            return new Vector2d(v.X, v.Y);
        }

        [Pure]
        public int Dim
        {
            get { return 2; }
        }

        [Pure]
        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return this.X;
                    case 1:
                        return this.Y;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                switch (i)
                {
                    case 0:
                        this.X = value;
                        break;
                    case 1:
                        this.Y = value;
                        break;
                }
                throw new IndexOutOfRangeException();
            }
        }

        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y); }
        }

        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y); }
        }

        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y); }
        }

        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0); }
        }

        [Pure]
        public int Quadrant
        {
            get
            {
                return ((this.X >= 0)
                    ? ((this.Y >= 0) ? 0 : 3)
                    : ((this.Y >= 0) ? 1 : 2));
            }
        }

        [Pure]
        public bool IsUnit
        {
            get { return this.Length.EpsilonEquals(1); }
        }

        [Pure]
        public double Length
        {
            get { return (double)Math.Sqrt(this.LengthCuad); }
        }

        [Pure]
        public double LengthCuad
        {
            get { return this.Dot(this); }
        }

        [Pure]
        public double LengthL1
        {
            get { return Math.Abs(this.X) + Math.Abs(this.Y); }
        }

        public void Add(BuffVector2d v2)
        {
            this.Set(this.X + v2.X, this.Y + v2.Y);
        }

        public void Add(Vector2d v2)
        {
            this.Set(this.X + v2.X, this.Y + v2.Y);
        }

        public void Sub(BuffVector2d v2)
        {
            this.Set(this.X - v2.X, this.Y - v2.Y);
        }

        public void Sub(Vector2d v2)
        {
            this.Set(this.X - v2.X, this.Y - v2.Y);
        }

        public void Mul(double c)
        {
            this.Set(this.X * c, this.Y * c);
        }

        public void Div(double c)
        {
            this.Set(this.X / c, this.Y / c);
        }

        public void SimpleMul(BuffVector2d v2)
        {
            this.Set(this.X * v2.X, this.Y * v2.Y);
        }

        public void SimpleMul(Vector2d v2)
        {
            this.Set(this.X * v2.X, this.Y * v2.Y);
        }

        public void Neg()
        {
            this.Set(-this.X, -this.Y);
        }

        public void Abs()
        {
            this.Set(Math.Abs(this.X), Math.Abs(this.Y));
        }

        public void Lerp(BuffVector2d v2, double alpha)
        {
            this.Lineal(v2, 1 - alpha, alpha);
        }

        public void Lerp(Vector2d v2, double alpha)
        {
            this.Lineal(v2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(BuffVector2d v2, BuffVector2d vLerp)
        {
            double x = (v2.X - this.X), y = (v2.Y - this.Y);

            double a = x * (vLerp.X - this.X)
                       + y * (vLerp.Y - this.Y);
            double b = x * x
                       + y * y;
            return a / Math.Sqrt(b);

            //Vector2d v12 = v2.Sub(this);
            //return v12.Proj(vLerp.Sub(this));
            //return v12.Dot(vLerp.Sub(this)) / v12.Length;
        }

        [Pure]
        public double InvLerp(Vector2d v2, Vector2d vLerp)
        {
            double a = (v2.X - this.X) * (vLerp.X - this.X)
                       + (v2.Y - this.Y) * (vLerp.Y - this.Y);
            double b = (v2.X - this.X) * (v2.X - this.X)
                       + (v2.Y - this.Y) * (v2.Y - this.Y);
            return a / Math.Sqrt(b);

            //Vector2d v12 = v2.Sub(this);
            //return v12.Proj(vLerp.Sub(this));
            //return v12.Dot(vLerp.Sub(this)) / v12.Length;
        }

        public void Lineal(BuffVector2d v2, double alpha, double beta)
        {
            this.Set(alpha * this.X + beta * v2.X, alpha * this.Y + beta * v2.Y);
        }

        public void Lineal(Vector2d v2, double alpha, double beta)
        {
            this.Set(alpha * this.X + beta * v2.X, alpha * this.Y + beta * v2.Y);
        }

        [Pure]
        public double Cross(BuffVector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        [Pure]
        public double Cross(Vector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        [Pure]
        public double Dot(BuffVector2d v2)
        {
            return this.X * v2.X + this.Y * v2.Y;
        }

        [Pure]
        public double Dot(Vector2d v2)
        {
            return this.X * v2.X + this.Y * v2.Y;
        }

        [Pure]
        public double Proy(BuffVector2d v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public double Proy(Vector2d v2)
        {
            return this.Dot(v2) / this.Length;
        }

        public void ProyV(BuffVector2d v2)
        {
            this.Mul(this.Proy(v2));
        }

        public void ProyV(Vector2d v2)
        {
            this.Mul(this.Proy(v2));
        }

        [Pure]
        public double Angle
        {
            get { return (double)SysMath.Atan2(this.Y, this.X); }
        }

        [Pure]
        public double AngleTo(BuffVector2d other)
        {
            //return (this.Angle(b) - this.Angle(a));
            // http://stackoverflow.com/questions/2150050/finding-signed-angle-between-vectors
            return SysMath.Atan2(this.X * other.Y - this.Y * other.X, this.X * other.X + this.Y * other.Y);
        }

        [Pure]
        public double AngleTo(Vector2d other)
        {
            //return (this.Angle(b) - this.Angle(a));
            // http://stackoverflow.com/questions/2150050/finding-signed-angle-between-vectors
            return SysMath.Atan2(this.X * other.Y - this.Y * other.X, this.X * other.X + this.Y * other.Y);
        }

        public void SetPerpRight()
        {
            this.Set(this.Y, -this.X);
        }

        public void SetPerpLeft()
        {
            this.Set(-this.Y, this.X);
        }

        public void SetUnitPerpRight()
        {
            this.SetPerpRight();
            this.SetUnit();
        }

        public void SetUnitPerpLeft()
        {
            this.SetPerpLeft();
            this.SetUnit();
        }

        [Pure]
        public double DotPerpRight(BuffVector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        [Pure]
        public double DotPerpRight(Vector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        public void SetRot(BuffVector2d v2, double rad)
        {
            double s = Math.Sin(rad);
            double c = Math.Cos(rad);
            this.Set(v2.X * c - v2.Y * s, v2.X * s + v2.Y * c);
        }

        public void SetRot(double angle, double len = 1)
        {
            this.Set(len * SysMath.Cos(angle), len * SysMath.Sin(angle));
        }

        public void Set(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public void SetZero()
        {
            this.Set(0, 0);
        }

        public void SetOne()
        {
            this.Set(1, 1);
        }

        public void SetUX()
        {
            this.Set(1, 0);
        }

        public void SetUY()
        {
            this.Set(0, 1);
        }

        public void SetUnit()
        {
            double len = this.Length;
            if (len.EpsilonZero())
            {
                this.Set(0, 0);
                return;
            }
            this.Div(len);
        }

        #region private

        [Pure]
        private bool EpsilonEquals(double x, double y, double epsilon = MathUtils.ZERO_TOLERANCE)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon);
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
            if (!(obj is BuffVector2d))
            {
                return false;
            }

            return this.Equals((BuffVector2d)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y);
        }

        #endregion

        #region IEpsilonEquatable<BuffVector2d>

        [Pure]
        public bool EpsilonEquals(BuffVector2d other, double epsilon = MathUtils.EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, epsilon);
        }

        #endregion

        #region IEquatable<BuffVector2d>

        [Pure]
        public bool Equals(BuffVector2d other)
        {
            return other.X == this.X && other.Y == this.Y;
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

            return VectorUtils.ToString(provider, format, (double[])this);
        }

        #endregion
    }
}