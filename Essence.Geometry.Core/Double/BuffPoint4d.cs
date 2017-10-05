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
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    public struct BuffPoint4d : IOpPoint4, IOpTuple4_Double
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Name of the property Z.</summary>
        public const string _Z = "Z";

        /// <summary>Name of the property W.</summary>
        public const string _W = "W";

        public BuffPoint4d(IPoint4 other)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            this.x = _other.X;
            this.y = _other.Y;
            this.z = _other.Z;
            this.w = _other.W;
        }

        public BuffPoint4d(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #region operators

        public static explicit operator double[](BuffPoint4d p)
        {
            return new[] { p.X, p.Y, p.Z, p.W };
        }

        #endregion

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
            return (this.x == x) && (this.y == y) && (this.z == z) && (this.w == w);
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
            return (obj is IPoint4) && this.Equals((IPoint4)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.x, this.y, this.z, this.w);
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

        #region ISerializable

        public BuffPoint4d(SerializationInfo info, StreamingContext context)
        {
            this.x = info.GetDouble(_X);
            this.y = info.GetDouble(_Y);
            this.z = info.GetDouble(_Z);
            this.w = info.GetDouble(_W);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.x);
            info.AddValue(_Y, this.y);
            info.AddValue(_Z, this.z);
            info.AddValue(_W, this.w);
        }

        #endregion

        #region IEpsilonEquatable<IVector4>

        public bool EpsilonEquals(IPoint4 other, double epsilon = MathUtils.EPSILON)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, _other.W, epsilon);
        }

        #endregion

        #region IEquatable<IPoint4>

        public bool Equals(IPoint4 other)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y, _other.Z, _other.W);
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

        #region ITuple4

        public void Get(IOpTuple4 setter)
        {
            IOpTuple4_Double _setter = setter.AsOpTupleDouble();
            _setter.Set(this.X, this.Y, this.Z, this.W);
        }

        #endregion

        #region IOpTuple4

        public void Set(ITuple4 tuple)
        {
            ITuple4_Double _tuple = tuple.AsTupleDouble();
            this.Set(_tuple.X, _tuple.Y, _tuple.Z, _tuple.W);
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

        #region IPoint

        public double InvLerp(IPoint4 p2, IPoint4 pLerp)
        {
            BuffVector4d v12 = new BuffVector4d();
            v12.Sub(p2, this);
            BuffVector4d v1Lerp = new BuffVector4d();
            v1Lerp.Sub(pLerp, this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IOpPoint4

        public void Add(IVector4 v)
        {
            ITuple4_Double _v = v.AsTupleDouble();
            this.Set(this.x + _v.X, this.y + _v.Y, this.z + _v.Z, this.w + _v.W);
        }

        public void Add(IPoint4 p, IVector4 v)
        {
            ITuple4_Double _p = p.AsTupleDouble();
            ITuple4_Double _v = v.AsTupleDouble();
            this.Set(_p.X + _v.X, _p.Y + _v.Y, _p.Z + _v.Z, _p.W + _v.W);
        }

        public void Sub(IVector4 v)
        {
            ITuple4_Double _v = v.AsTupleDouble();
            this.Set(this.x - _v.X, this.y - _v.Y, this.z - _v.Z, this.w - _v.W);
        }

        public void Sub(IPoint4 p, IVector4 v)
        {
            ITuple4_Double _p = p.AsTupleDouble();
            ITuple4_Double _v = v.AsTupleDouble();
            this.Set(_p.X - _v.X, _p.Y - _v.Y, _p.Z - _v.Z, _p.W - _v.W);
        }

        public void Lerp(IPoint4 p2, double alpha)
        {
            this.Lineal(p2, 1 - alpha, alpha);
        }

        public void Lerp(IPoint4 p1, IPoint4 p2, double alpha)
        {
            this.Lineal(p1, p2, 1 - alpha, alpha);
        }

        public void Lineal(IPoint4 p2, double alpha, double beta)
        {
            ITuple4_Double _p2 = p2.AsTupleDouble();
            this.Set(alpha * this.x + beta * _p2.X,
                     alpha * this.y + beta * _p2.Y,
                     alpha * this.z + beta * _p2.Z,
                     alpha * this.w + beta * _p2.W);
        }

        public void Lineal(IPoint4 p1, IPoint4 p2, double alpha, double beta)
        {
            ITuple4_Double _p1 = p1.AsTupleDouble();
            ITuple4_Double _p2 = p2.AsTupleDouble();
            this.Set(alpha * _p1.X + beta * _p2.X,
                     alpha * _p1.Y + beta * _p2.Y,
                     alpha * _p1.Z + beta * _p2.Z,
                     alpha * _p1.W + beta * _p2.W);
        }

        public void ProjectTo(IVector4 where)
        {
            BuffVector4d aux = new BuffVector4d(where);
            aux.ProjV(new Vector4d(this.x, this.y, this.z, this.w));
            this.Set(aux.X, aux.Y, aux.Z, aux.W);
        }

        public void ProjectTo(IPoint4 p1, IVector4 where)
        {
            ITuple4_Double _p1 = p1.AsTupleDouble();
            BuffVector4d aux = new BuffVector4d(where);
            aux.ProjV(new Vector4d(_p1.X, _p1.Y, _p1.Z, _p1.W));
            this.Set(aux.X, aux.Y, aux.Z, aux.W);
        }

        #endregion
    }
}