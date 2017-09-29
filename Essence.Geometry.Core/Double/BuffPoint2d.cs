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
    public struct BuffPoint2d : IPoint2, IOpPoint2, IOpTuple2_Double
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        public BuffPoint2d(IPoint2 other)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            this.x = _other.X;
            this.y = _other.Y;
        }

        public BuffPoint2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #region operators

        public static explicit operator double[](BuffPoint2d p)
        {
            return new[] { p.X, p.Y };
        }

        #endregion

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

        #region object

        [Pure]
        public override string ToString()
        {
            return this.ToString("F3", null);
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return (obj is IPoint2) && this.Equals((IPoint2)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.x, this.y);
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

        public BuffPoint2d(SerializationInfo info, StreamingContext context)
        {
            this.x = info.GetDouble(_X);
            this.y = info.GetDouble(_Y);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.x);
            info.AddValue(_Y, this.y);
        }

        #endregion

        #region IEpsilonEquatable<IVector2>

        public bool EpsilonEquals(IPoint2 other, double epsilon = MathUtils.EPSILON)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, epsilon);
        }

        #endregion

        #region IEquatable<IPoint2>

        public bool Equals(IPoint2 other)
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

        #region IPoint

        public int Quadrant
        {
            get { return ((this.x >= 0) ? ((this.y >= 0) ? 0 : 3) : ((this.y >= 0) ? 1 : 2)); }
        }

        #endregion

        #region IPoint2

        public double InvLerp(IPoint2 p2, IPoint2 pLerp)
        {
            BuffVector2d v12 = new BuffVector2d();
            v12.Sub(p2, this);
            BuffVector2d v1Lerp = new BuffVector2d();
            v1Lerp.Sub(pLerp, this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IOpPoint2

        public void Add(IVector2 v)
        {
            ITuple2_Double _v = v.AsTupleDouble();
            this.Set(this.x + _v.X, this.y + _v.Y);
        }

        public void Add(IPoint2 p, IVector2 v)
        {
            ITuple2_Double _p = p.AsTupleDouble();
            ITuple2_Double _v = v.AsTupleDouble();
            this.Set(_p.X + _v.X, _p.Y + _v.Y);
        }

        public void Sub(IVector2 v)
        {
            ITuple2_Double _v = v.AsTupleDouble();
            this.Set(this.x - _v.X, this.y - _v.Y);
        }

        public void Sub(IPoint2 p, IVector2 v)
        {
            ITuple2_Double _p = p.AsTupleDouble();
            ITuple2_Double _v = v.AsTupleDouble();
            this.Set(_p.X - _v.X, _p.Y - _v.Y);
        }

        public void Lerp(IPoint2 p2, double alpha)
        {
            this.Lineal(p2, 1 - alpha, alpha);
        }

        public void Lerp(IPoint2 p1, IPoint2 p2, double alpha)
        {
            this.Lineal(p1, p2, 1 - alpha, alpha);
        }

        public void Lineal(IPoint2 p2, double alpha, double beta)
        {
            ITuple2_Double _p2 = p2.AsTupleDouble();
            this.Set(alpha * this.x + beta * _p2.X,
                     alpha * this.y + beta * _p2.Y);
        }

        public void Lineal(IPoint2 p1, IPoint2 p2, double alpha, double beta)
        {
            ITuple2_Double _p1 = p1.AsTupleDouble();
            ITuple2_Double _p2 = p2.AsTupleDouble();
            this.Set(alpha * _p1.X + beta * _p2.X,
                     alpha * _p1.Y + beta * _p2.Y);
        }

        public void ProjectTo(IVector2 where)
        {
            BuffVector2d aux = new BuffVector2d(where);
            aux.ProjV(new Vector2d(this.x, this.y));
            this.Set(aux.X, aux.Y);
        }

        public void ProjectTo(IPoint2 p1, IVector2 where)
        {
            ITuple2_Double _p1 = p1.AsTupleDouble();
            BuffVector2d aux = new BuffVector2d(where);
            aux.ProjV(new Vector2d(_p1.X, _p1.Y));
            this.Set(aux.X, aux.Y);
        }

        #endregion
    }
}