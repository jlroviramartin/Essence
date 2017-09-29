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
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    public struct BuffVector2d : IVector2, IOpVector2, IOpTuple2_Double
    {
        public BuffVector2d(IVector2 other)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            this.x = _other.X;
            this.y = _other.Y;
        }

        public BuffVector2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #region private

        private double x;
        private double y;

        private bool EpsilonEquals(double x, double y, double epsilon = MathUtils.EPSILON)
        {
            return this.x.EpsilonEquals(x, epsilon) && this.y.EpsilonEquals(y, epsilon);
        }

        private bool Equals(double x, double y)
        {
            return (this.x == x) && (this.y == y);
        }

        #endregion

        #region IEpsilonEquatable<IVector2>

        public bool EpsilonEquals(IVector2 other, double epsilon = MathUtils.EPSILON)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, epsilon);
        }

        #endregion

        #region IEquatable<IVector2>

        public bool Equals(IVector2 other)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y);
        }

        #endregion

        #region ITuple

        public bool IsValid
        {
            get
            {
                return MathUtils.IsValid(this.x)
                       && MathUtils.IsValid(this.y);
            }
        }

        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(this.x)
                       || double.IsInfinity(this.y);
            }
        }

        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0); }
        }

        #endregion

        #region ITuple2_Double / IOpTuple2_Double

        public void Set(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        #endregion

        #region IVector

        public bool IsUnit
        {
            get { return MathUtils.EpsilonEquals(this.Length, 1); }
        }

        public double Length
        {
            get { return Math.Sqrt(this.Length2); }
        }

        public double Length2
        {
            get { return this.Dot(this); }
        }

        public double LengthL1
        {
            get
            {
                return Math.Abs(this.x)
                       + Math.Abs(this.y);
            }
        }

        public double Dot(IVector2 v2)
        {
            ITuple2_Double _v2 = v2.AsTupleDouble();
            return this.x * _v2.X + this.y * _v2.Y;
        }

        public double Proj(IVector2 v2)
        {
            return this.Dot(v2) / this.Length;
        }

        public double InvLerp(IVector2 v2, IVector2 vLerp)
        {
            BuffVector2d v12 = new BuffVector2d(v2);
            v12.Sub(this);
            BuffVector2d v1Lerp = new BuffVector2d(vLerp);
            v1Lerp.Sub(this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IVector2

        public int Quadrant
        {
            get { return ((this.x >= 0) ? ((this.y >= 0) ? 0 : 3) : ((this.y >= 0) ? 1 : 2)); }
        }

        public double Cross(IVector2 v2)
        {
            ITuple2_Double _v2 = v2.AsTupleDouble();
            return this.x * _v2.Y - this.y * _v2.X;
        }

        #endregion

        #region IOpVector2

        public void Unit()
        {
            double len = this.Length;
            if (MathUtils.EpsilonZero(len))
            {
                this.Set(0, 0);
                return;
            }
            this.Div(len);
        }

        public void Add(IVector2 v2)
        {
            ITuple2_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x + _v2.X, this.y + _v2.Y);
        }

        public void Add(IVector2 v1, IVector2 v2)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            ITuple2_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X + _v2.X, _v1.Y + _v2.Y);
        }

        public void Sub(IVector2 v2)
        {
            ITuple2_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x - _v2.X, this.y - _v2.Y);
        }

        public void Sub(IVector2 v1, IVector2 v2)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            ITuple2_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X - _v2.X, _v1.Y - _v2.Y);
        }

        public void Sub(IPoint2 p1, IPoint2 p2)
        {
            ITuple2_Double _p1 = p1.AsTupleDouble();
            ITuple2_Double _p2 = p2.AsTupleDouble();
            this.Set(_p1.X - _p2.X, _p1.Y - _p2.Y);
        }

        public void Mul(double c)
        {
            this.Set(this.x * c, this.y * c);
        }

        public void Mul(IVector2 v1, double c)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            this.Set(_v1.X * c, _v1.Y * c);
        }

        public void Div(double c)
        {
            this.Set(this.x / c, this.y / c);
        }

        public void Div(IVector2 v1, double c)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            this.Set(_v1.X / c, _v1.Y / c);
        }

        public void SimpleMul(IVector2 v2)
        {
            ITuple2_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x * _v2.X, this.y * _v2.Y);
        }

        public void SimpleMul(IVector2 v1, IVector2 v2)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            ITuple2_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X * _v2.X, _v1.Y * _v2.Y);
        }

        public void Neg()
        {
            this.Set(-this.x, -this.y);
        }

        public void Neg(IVector2 v1)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            this.Set(-_v1.X, -_v1.Y);
        }

        public void Abs()
        {
            this.Set(Math.Abs(this.x), Math.Abs(this.y));
        }

        public void Abs(IVector2 v1)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            this.Set(Math.Abs(_v1.X), Math.Abs(_v1.Y));
        }

        public void Lerp(IVector2 v2, double alpha)
        {
            this.Lineal(v2, 1 - alpha, alpha);
        }

        public void Lerp(IVector2 v1, IVector2 v2, double alpha)
        {
            this.Lineal(v1, v2, 1 - alpha, alpha);
        }

        public void Lineal(IVector2 v2, double alpha, double beta)
        {
            ITuple2_Double _v2 = v2.AsTupleDouble();
            this.Set(alpha * this.x + beta * _v2.X,
                     alpha * this.y + beta * _v2.Y);
        }

        public void Lineal(IVector2 v1, IVector2 v2, double alpha, double beta)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            ITuple2_Double _v2 = v2.AsTupleDouble();
            this.Set(alpha * _v1.X + beta * _v2.X,
                     alpha * _v1.Y + beta * _v2.Y);
        }

        public void ProjV(IVector2 v2)
        {
            this.Mul(this.Proj(v2));
        }

        public void ProjV(IVector2 v1, IVector2 v2)
        {
            this.Mul(v1, this.Proj(v2));
        }

        public void PerpRight()
        {
            this.Set(this.y, -this.x);
        }

        public void PerpRight(IVector2 v1)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            this.Set(_v1.Y, -_v1.X);
        }

        public void PerpLeft()
        {
            this.Set(-this.y, this.x);
        }

        public void PerpLeft(IVector2 v1)
        {
            ITuple2_Double _v1 = v1.AsTupleDouble();
            this.Set(-_v1.Y, _v1.X);
        }

        public void UnitPerpRight()
        {
            this.PerpRight();
            this.Unit();
        }

        public void UnitPerpRight(IVector2 v1)
        {
            this.PerpRight(v1);
            this.Unit();
        }

        public void UnitPerpLeft()
        {
            this.PerpLeft();
            this.Unit();
        }

        public void UnitPerpLeft(IVector2 v1)
        {
            this.PerpLeft(v1);
            this.Unit();
        }

        #endregion
    }
}