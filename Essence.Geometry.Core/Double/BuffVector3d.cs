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
    public struct BuffVector3d : IVector3, IOpVector3, IOpTuple3_Double
    {
        public BuffVector3d(IVector3 other)
        {
            ITuple3_Double _other = other.AsTupleDouble();
            this.x = _other.X;
            this.y = _other.Y;
            this.z = _other.Z;
        }

        public BuffVector3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region private

        private double x;
        private double y;
        private double z;

        private bool EpsilonEquals(double x, double y, double z, double epsilon = MathUtils.EPSILON)
        {
            return this.x.EpsilonEquals(x, epsilon) && this.y.EpsilonEquals(y, epsilon) && this.z.EpsilonEquals(z, epsilon);
        }

        private bool Equals(double x, double y, double z)
        {
            return (this.x == x) && (this.y == y) && (this.z == z);
        }

        #endregion

        #region IEpsilonEquatable<IVector3>

        public bool EpsilonEquals(IVector3 other, double epsilon = MathUtils.EPSILON)
        {
            ITuple3_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, epsilon);
        }

        #endregion

        #region IEquatable<IVector3>

        public bool Equals(IVector3 other)
        {
            ITuple3_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y, _other.Z);
        }

        #endregion

        #region ITuple

        public bool IsValid
        {
            get
            {
                return MathUtils.IsValid(this.x)
                       && MathUtils.IsValid(this.y)
                       && MathUtils.IsValid(this.z);
            }
        }

        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(this.x)
                       || double.IsInfinity(this.y)
                       || double.IsInfinity(this.z);
            }
        }

        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0, 0); }
        }

        #endregion

        #region ITuple3_Double / IOpTuple3_Double

        public void Set(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
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
                       + Math.Abs(this.z);
            }
        }

        public double Dot(IVector3 v2)
        {
            ITuple3_Double _v2 = v2.AsTupleDouble();
            return this.x * _v2.X + this.y * _v2.Y + this.z * _v2.Z;
        }

        public double Proj(IVector3 v2)
        {
            return this.Dot(v2) / this.Length;
        }

        public double InvLerp(IVector3 v2, IVector3 vLerp)
        {
            BuffVector3d v12 = new BuffVector3d(v2);
            v12.Sub(this);
            BuffVector3d v1Lerp = new BuffVector3d(vLerp);
            v1Lerp.Sub(this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IVector3

        public int Octant
        {
            get
            {
                return ((this.x >= 0)
                    ? ((this.y >= 0)
                        ? ((this.z >= 0) ? 0 : 4)
                        : ((this.z >= 0) ? 3 : 7))
                    : ((this.y >= 0)
                        ? ((this.z >= 0) ? 1 : 5)
                        : ((this.z >= 0) ? 2 : 6)));
            }
        }

        public double TripleProduct(IVector3 v2, IVector3 v3)
        {
            BuffVector3d aux = new BuffVector3d(v2);
            aux.Cross(v3);
            return this.Dot(aux);
        }

        #endregion

        #region IOpVector3

        public void Unit()
        {
            double len = this.Length;
            if (MathUtils.EpsilonZero(len))
            {
                this.Set(0, 0, 0);
                return;
            }
            this.Div(len);
        }

        public void Add(IVector3 v2)
        {
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x + _v2.X, this.y + _v2.Y, this.z + _v2.Z);
        }

        public void Add(IVector3 v1, IVector3 v2)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X + _v2.X, _v1.Y + _v2.Y, _v1.Z + _v2.Z);
        }

        public void Sub(IVector3 v2)
        {
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x - _v2.X, this.y - _v2.Y, this.z - _v2.Z);
        }

        public void Sub(IVector3 v1, IVector3 v2)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X - _v2.X, _v1.Y - _v2.Y, _v1.Z - _v2.Z);
        }

        public void Sub(IPoint3 p1, IPoint3 p2)
        {
            ITuple3_Double _p1 = p1.AsTupleDouble();
            ITuple3_Double _p2 = p2.AsTupleDouble();
            this.Set(_p1.X - _p2.X, _p1.Y - _p2.Y, _p1.Z - _p2.Z);
        }

        public void Mul(double c)
        {
            this.Set(this.x * c, this.y * c, this.z * c);
        }

        public void Mul(IVector3 v1, double c)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            this.Set(_v1.X * c, _v1.Y * c, _v1.Z * c);
        }

        public void Div(double c)
        {
            this.Set(this.x / c, this.y / c, this.z / c);
        }

        public void Div(IVector3 v1, double c)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            this.Set(_v1.X / c, _v1.Y / c, _v1.Z / c);
        }

        public void SimpleMul(IVector3 v2)
        {
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set(this.x * _v2.X, this.y * _v2.Y, this.z * _v2.Z);
        }

        public void SimpleMul(IVector3 v1, IVector3 v2)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set(_v1.X * _v2.X, _v1.Y * _v2.Y, _v1.Z * _v2.Z);
        }

        public void Neg()
        {
            this.Set(-this.x, -this.y, -this.z);
        }

        public void Neg(IVector3 v1)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            this.Set(-_v1.X, -_v1.Y, -_v1.Z);
        }

        public void Abs()
        {
            this.Set(Math.Abs(this.x), Math.Abs(this.y), Math.Abs(this.z));
        }

        public void Abs(IVector3 v1)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            this.Set(Math.Abs(_v1.X), Math.Abs(_v1.Y), Math.Abs(_v1.Z));
        }

        public void Lerp(IVector3 v2, double alpha)
        {
            this.Lineal(v2, 1 - alpha, alpha);
        }

        public void Lerp(IVector3 v1, IVector3 v2, double alpha)
        {
            this.Lineal(v1, v2, 1 - alpha, alpha);
        }

        public void Lineal(IVector3 v2, double alpha, double beta)
        {
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set(alpha * this.x + beta * _v2.X,
                     alpha * this.y + beta * _v2.Y,
                     alpha * this.z + beta * _v2.Z);
        }

        public void Lineal(IVector3 v1, IVector3 v2, double alpha, double beta)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set(alpha * _v1.X + beta * _v2.X,
                     alpha * _v1.Y + beta * _v2.Y,
                     alpha * _v1.Z + beta * _v2.Z);
        }

        public void ProjV(IVector3 v2)
        {
            this.Mul(this.Proj(v2));
        }

        public void ProjV(IVector3 v1, IVector3 v2)
        {
            this.Mul(v1, this.Proj(v2));
        }

        public void Cross(IVector3 v2)
        {
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set((this.y * _v2.Z) - (this.z * _v2.Y),
                     (this.z * _v2.X) - (this.x * _v2.Z),
                     (this.x * _v2.Y) - (this.y * _v2.X));
        }

        public void Cross(IVector3 v1, IVector3 v2)
        {
            ITuple3_Double _v1 = v1.AsTupleDouble();
            ITuple3_Double _v2 = v2.AsTupleDouble();
            this.Set((_v1.Y * _v2.Z) - (_v1.Z * _v2.Y),
                     (_v1.Z * _v2.X) - (_v1.X * _v2.Z),
                     (_v1.X * _v2.Y) - (_v1.Y * _v2.X));
        }

        #endregion
    }
}