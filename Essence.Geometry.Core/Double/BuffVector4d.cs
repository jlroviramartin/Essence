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
    public struct BuffVector4d : IVector4, IOpVector4, IOpTuple4_Double
    {
        public BuffVector4d(IVector4 other)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            this.x = _other.X;
            this.y = _other.Y;
            this.z = _other.Z;
            this.w = _other.W;
        }

        public BuffVector4d(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #region private

        private double x;
        private double y;
        private double z;
        private double w;

        private bool EpsilonEquals(double x, double y, double z, double w, double epsilon = MathUtils.EPSILON)
        {
            return this.x.EpsilonEquals(x, epsilon) && this.y.EpsilonEquals(y, epsilon) && this.z.EpsilonEquals(z, epsilon) && this.w.EpsilonEquals(w, epsilon);
        }

        private bool Equals(double x, double y, double z, double w)
        {
            return (this.x == x) && (this.y == y) && (this.z == z) && (this.w == z);
        }

        #endregion

        #region ITuple

        public bool IsValid
        {
            get
            {
                return MathUtils.IsValid(this.x)
                       && MathUtils.IsValid(this.y)
                       && MathUtils.IsValid(this.z)
                       && MathUtils.IsValid(this.w);
            }
        }

        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(this.x)
                       || double.IsInfinity(this.y)
                       || double.IsInfinity(this.z)
                       || double.IsInfinity(this.w);
            }
        }

        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0, 0, 0); }
        }

        #endregion

        #region ITuple4_Double / IOpTuple4_Double

        public void Set(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
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

        public double Z
        {
            get { return this.z; }
            set { this.z = value; }
        }

        public double W
        {
            get { return this.w; }
            set { this.w = value; }
        }

        #endregion

        #region IEpsilonEquatable<IVector4>

        public bool EpsilonEquals(IVector4 other, double epsilon = MathUtils.EPSILON)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, _other.W, epsilon);
        }

        #endregion

        #region IEquatable<IVector4>

        public bool Equals(IVector4 other)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y, _other.Z, _other.W);
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
                       + Math.Abs(this.y)
                       + Math.Abs(this.z)
                       + Math.Abs(this.w);
            }
        }

        public double Dot(IVector4 v2)
        {
            ITuple4_Double _v2 = v2.AsTupleDouble();
            return this.x * _v2.X + this.y * _v2.Y + this.z * _v2.Z + this.w * _v2.W;
        }

        public double Proj(IVector4 v2)
        {
            return this.Dot(v2) / this.Length;
        }

        public double InvLerp(IVector4 v2, IVector4 vLerp)
        {
            BuffVector4d v12 = new BuffVector4d(v2);
            v12.Sub(this);
            BuffVector4d v1Lerp = new BuffVector4d(vLerp);
            v1Lerp.Sub(this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IOpVector4

        public void Unit()
        {
            double len = this.Length;
            if (MathUtils.EpsilonZero(len))
            {
                this.Set(0, 0, 0, 0);
                return;
            }
            this.Div(len);
        }

        public void Add(IVector4 v2)
        {
            ITuple4_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x + _v2.X, this.y + _v2.Y, this.z + _v2.Z, this.w + _v2.W);
        }

        public void Add(IVector4 v1, IVector4 v2)
        {
            ITuple4_Double _v1 = v1.AsTupleDouble();
            ITuple4_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X + _v2.X, _v1.Y + _v2.Y, _v1.Z + _v2.Z, _v1.W + _v2.W);
        }

        public void Sub(IVector4 v2)
        {
            ITuple4_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x - _v2.X, this.y - _v2.Y, this.z - _v2.Z, this.w - _v2.W);
        }

        public void Sub(IVector4 v1, IVector4 v2)
        {
            ITuple4_Double _v1 = v1.AsTupleDouble();
            ITuple4_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X - _v2.X, _v1.Y - _v2.Y, _v1.Z - _v2.Z, _v1.W - _v2.W);
        }

        public void Sub(IPoint4 p1, IPoint4 p2)
        {
            ITuple4_Double _p1 = p1.AsTupleDouble();
            ITuple4_Double _p2 = p2.AsTupleDouble();
            this.Set(_p1.X - _p2.X, _p1.Y - _p2.Y, _p1.Z - _p2.Z, _p1.W - _p2.W);
        }

        public void Mul(double c)
        {
            this.Set(this.x * c, this.y * c, this.z * c, this.w * c);
        }

        public void Mul(IVector4 v1, double c)
        {
            ITuple4_Double _v1 = v1.AsTupleDouble();
            this.Set(_v1.X * c, _v1.Y * c, _v1.Z * c, _v1.W * c);
        }

        public void Div(double c)
        {
            this.Set(this.x / c, this.y / c, this.z / c, this.w / c);
        }

        public void Div(IVector4 v1, double c)
        {
            ITuple4_Double _v1 = v1.AsTupleDouble();
            this.Set(_v1.X / c, _v1.Y / c, _v1.Z / c, _v1.W / c);
        }

        public void SimpleMul(IVector4 v2)
        {
            ITuple4_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x * _v2.X, this.y * _v2.Y, this.z * _v2.Z, this.w * _v2.W);
        }

        public void SimpleMul(IVector4 v1, IVector4 v2)
        {
            ITuple4_Double _v1 = v1.AsTupleDouble();
            ITuple4_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X * _v2.X, _v1.Y * _v2.Y, _v1.Z * _v2.Z, _v1.W * _v2.W);
        }

        public void Neg()
        {
            this.Set(-this.x, -this.y, -this.z, -this.w);
        }

        public void Neg(IVector4 v1)
        {
            ITuple4_Double _v1 = v1.AsTupleDouble();
            this.Set(-_v1.X, -_v1.Y, -_v1.Z, -_v1.W);
        }

        public void Abs()
        {
            this.Set(Math.Abs(this.x), Math.Abs(this.y), Math.Abs(this.z), Math.Abs(this.w));
        }

        public void Abs(IVector4 v1)
        {
            ITuple4_Double _v1 = v1.AsTupleDouble();
            this.Set(Math.Abs(_v1.X), Math.Abs(_v1.Y), Math.Abs(_v1.Z), Math.Abs(_v1.W));
        }

        public void Lerp(IVector4 v2, double alpha)
        {
            this.Lineal(v2, 1 - alpha, alpha);
        }

        public void Lerp(IVector4 v1, IVector4 v2, double alpha)
        {
            this.Lineal(v1, v2, 1 - alpha, alpha);
        }

        public void Lineal(IVector4 v2, double alpha, double beta)
        {
            ITuple4_Double _v2 = v2.AsTupleDouble();
            this.Set(alpha * this.x + beta * _v2.X,
                     alpha * this.y + beta * _v2.Y,
                     alpha * this.z + beta * _v2.Z,
                     alpha * this.w + beta * _v2.W);
        }

        public void Lineal(IVector4 v1, IVector4 v2, double alpha, double beta)
        {
            ITuple4_Double _v1 = v1.AsTupleDouble();
            ITuple4_Double _v2 = v2.AsTupleDouble();
            this.Set(alpha * _v1.X + beta * _v2.X,
                     alpha * _v1.Y + beta * _v2.Y,
                     alpha * _v1.Z + beta * _v2.Z,
                     alpha * _v1.W + beta * _v2.W);
        }

        public void ProjV(IVector4 v2)
        {
            this.Mul(this.Proj(v2));
        }

        public void ProjV(IVector4 v1, IVector4 v2)
        {
            this.Mul(v1, this.Proj(v2));
        }

        #endregion
    }
}