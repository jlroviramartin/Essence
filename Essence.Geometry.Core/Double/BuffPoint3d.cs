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
    public struct BuffPoint3d : IOpPoint3, IOpTuple3_Double
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Name of the property Z.</summary>
        public const string _Z = "Z";

        public BuffPoint3d(IPoint3 other)
        {
            ITuple3_Double _other = other.AsTupleDouble();
            this.x = _other.X;
            this.y = _other.Y;
            this.z = _other.Z;
        }

        public BuffPoint3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region operators

        public static explicit operator double[](BuffPoint3d p)
        {
            return new[] { p.X, p.Y, p.Z };
        }

        #endregion

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

        #region object

        [Pure]
        public override string ToString()
        {
            return this.ToString("F3", null);
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return (obj is IPoint3) && this.Equals((IPoint3)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.x, this.y, this.z);
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

        public BuffPoint3d(SerializationInfo info, StreamingContext context)
        {
            this.x = info.GetDouble(_X);
            this.y = info.GetDouble(_Y);
            this.z = info.GetDouble(_Z);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.x);
            info.AddValue(_Y, this.y);
            info.AddValue(_Z, this.z);
        }

        #endregion

        #region IEpsilonEquatable<IVector3>

        public bool EpsilonEquals(IPoint3 other, double epsilon = MathUtils.EPSILON)
        {
            ITuple3_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, epsilon);
        }

        #endregion

        #region IEquatable<IPoint3>

        public bool Equals(IPoint3 other)
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

        #region ITuple3

        public void Get(IOpTuple3 setter)
        {
            IOpTuple3_Double _setter = setter.AsOpTupleDouble();
            _setter.Set(this.X, this.Y, this.Z);
        }

        #endregion

        #region IOpTuple3

        public void Set(ITuple3 tuple)
        {
            ITuple3_Double _tuple = tuple.AsTupleDouble();
            this.Set(_tuple.X, _tuple.Y, _tuple.Z);
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

        #region IPoint

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

        #endregion

        #region IPoint3

        public double InvLerp(IPoint3 p2, IPoint3 pLerp)
        {
            BuffVector3d v12 = new BuffVector3d();
            v12.Sub(p2, this);
            BuffVector3d v1Lerp = new BuffVector3d();
            v1Lerp.Sub(pLerp, this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IOpPoint3

        public void Add(IVector3 v)
        {
            ITuple3_Double _v = v.AsTupleDouble();
            this.Set(this.x + _v.X, this.y + _v.Y, this.z + _v.Z);
        }

        public void Add(IPoint3 p, IVector3 v)
        {
            ITuple3_Double _p = p.AsTupleDouble();
            ITuple3_Double _v = v.AsTupleDouble();
            this.Set(_p.X + _v.X, _p.Y + _v.Y, _p.Z + _v.Z);
        }

        public void Sub(IVector3 v)
        {
            ITuple3_Double _v = v.AsTupleDouble();
            this.Set(this.x - _v.X, this.y - _v.Y, this.z - _v.Z);
        }

        public void Sub(IPoint3 p, IVector3 v)
        {
            ITuple3_Double _p = p.AsTupleDouble();
            ITuple3_Double _v = v.AsTupleDouble();
            this.Set(_p.X - _v.X, _p.Y - _v.Y, _p.Z - _v.Z);
        }

        public void Lerp(IPoint3 p2, double alpha)
        {
            this.Lineal(p2, 1 - alpha, alpha);
        }

        public void Lerp(IPoint3 p1, IPoint3 p2, double alpha)
        {
            this.Lineal(p1, p2, 1 - alpha, alpha);
        }

        public void Lineal(IPoint3 p2, double alpha, double beta)
        {
            ITuple3_Double _p2 = p2.AsTupleDouble();
            this.Set(alpha * this.x + beta * _p2.X,
                     alpha * this.y + beta * _p2.Y,
                     alpha * this.z + beta * _p2.Z);
        }

        public void Lineal(IPoint3 p1, IPoint3 p2, double alpha, double beta)
        {
            ITuple3_Double _p1 = p1.AsTupleDouble();
            ITuple3_Double _p2 = p2.AsTupleDouble();
            this.Set(alpha * _p1.X + beta * _p2.X,
                     alpha * _p1.Y + beta * _p2.Y,
                     alpha * _p1.Z + beta * _p2.Z);
        }

        public void ProjectTo(IVector3 where)
        {
            BuffVector3d aux = new BuffVector3d(where);
            aux.ProjV(new Vector3d(this.x, this.y, this.z));
            this.Set(aux.X, aux.Y, aux.Z);
        }

        public void ProjectTo(IPoint3 p1, IVector3 where)
        {
            ITuple3_Double _p1 = p1.AsTupleDouble();
            BuffVector3d aux = new BuffVector3d(where);
            aux.ProjV(new Vector3d(_p1.X, _p1.Y, _p1.Z));
            this.Set(aux.X, aux.Y, aux.Z);
        }

        #endregion
    }
}