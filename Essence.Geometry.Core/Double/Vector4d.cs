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
using System.Globalization;
using System.Runtime.Serialization;
using Essence.Util.Math;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Core.Double
{
    public struct Vector4d : IVector4, ITuple4_Double,
                             IEpsilonEquatable<Vector4d>,
                             IEquatable<Vector4d>,
                             IFormattable,
                             ISerializable
    {
        /// <summary>Name of the property <code>X</code>.</summary>
        public const string _X = "X";

        /// <summary>Name of the property <code>Y</code>.</summary>
        public const string _Y = "Y";

        /// <summary>Name of the property <code>Z</code>.</summary>
        public const string _Z = "Z";

        /// <summary>Name of the property <code>W</code>.</summary>
        public const string _W = "W";

        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Vector zero.</summary>
        public static readonly Vector4d Zero = new Vector4d(0, 0, 0, 0);

        /// <summary>Vector one.</summary>
        public static readonly Vector4d One = new Vector4d(1, 1, 1, 1);

        /// <summary>Vector with property X = 1 and others = 0.</summary>
        public static readonly Vector4d UX = new Vector4d(1, 0, 0, 0);

        /// <summary>Vector with property Y = 1 and others = 0.</summary>
        public static readonly Vector4d UY = new Vector4d(0, 1, 0, 0);

        /// <summary>Vector with property Z = 1 and others = 0.</summary>
        public static readonly Vector4d UZ = new Vector4d(0, 0, 1, 0);

        /// <summary>Vector with property W = 1 and others = 0.</summary>
        public static readonly Vector4d UW = new Vector4d(0, 0, 0, 1);

        public Vector4d(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Vector4d(IVector4 v)
        {
            ITuple4_Double _v = v.AsTupleDouble();
            this.X = _v.X;
            this.Y = _v.Y;
            this.Z = _v.Z;
            this.W = _v.W;
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        /// <summary>Property Z.</summary>
        public readonly double Z;

        /// <summary>Property W.</summary>
        public readonly double W;

        #region operators

        public static explicit operator double[](Vector4d v)
        {
            return new[] { v.X, v.Y, v.Z, v.W };
        }

        public static Vector4d operator -(Vector4d v1)
        {
            return v1.Neg();
        }

        public static Vector4d operator +(Vector4d v1, Vector4d v2)
        {
            return v1.Add(v2);
        }

        public static Vector4d operator -(Vector4d v1, Vector4d v2)
        {
            return v1.Sub(v2);
        }

        public static Vector4d operator *(Vector4d v, double c)
        {
            return v.Mul(c);
        }

        public static Vector4d operator *(double c, Vector4d v)
        {
            return v.Mul(c);
        }

        public static Vector4d operator /(Vector4d v, double c)
        {
            return v.Div(c);
        }

        public static implicit operator Point4d(Vector4d p)
        {
            return new Point4d(p.X, p.Y, p.Z, p.W);
        }

        #endregion

        [Pure]
        public int Dim
        {
            get { return 4; }
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
                    case 2:
                        return this.Z;
                    case 3:
                        return this.W;
                }
                throw new IndexOutOfRangeException();
            }
        }

        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z) || double.IsNaN(this.W); }
        }

        [Pure]
        public Vector4d Unit
        {
            get
            {
                double len = this.Length;
                if (len.EpsilonZero())
                {
                    return Zero;
                }
                return this.Div(len);
            }
        }

        [Pure]
        public Vector4d Add(Vector4d v2)
        {
            return new Vector4d(this.X + v2.X, this.Y + v2.Y, this.Z + v2.Z, this.W + v2.W);
        }

        [Pure]
        public Vector4d Sub(Vector4d v2)
        {
            return new Vector4d(this.X - v2.X, this.Y - v2.Y, this.Z - v2.Z, this.W - v2.W);
        }

        [Pure]
        public Vector4d Mul(double c)
        {
            return new Vector4d(this.X * c, this.Y * c, this.Z * c, this.W * c);
        }

        [Pure]
        public Vector4d Div(double c)
        {
            return new Vector4d(this.X / c, this.Y / c, this.Z / c, this.W / c);
        }

        [Pure]
        public Vector4d SimpleMul(Vector4d v2)
        {
            return new Vector4d(this.X * v2.X, this.Y * v2.Y, this.Z * v2.Z, this.W * v2.W);
        }

        [Pure]
        public Vector4d Neg()
        {
            return new Vector4d(-this.X, -this.Y, -this.Z, -this.W);
        }

        [Pure]
        public Vector4d Abs()
        {
            return new Vector4d(Math.Abs(this.X), Math.Abs(this.Y), Math.Abs(this.Z), Math.Abs(this.W));
        }

        [Pure]
        public Vector4d Lerp(Vector4d v2, double alpha)
        {
            return this.Lineal(v2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Vector4d v2, Vector4d vLerp)
        {
            Vector4d v12 = v2.Sub(this);
            return v12.Proy(vLerp.Sub(this));
        }

        [Pure]
        public Vector4d Lineal(Vector4d v2, double alpha, double beta)
        {
            return new Vector4d(alpha * this.X + beta * v2.X, alpha * this.Y + beta * v2.Y, alpha * this.Z + beta * v2.Z, alpha * this.W + beta * v2.W);
        }

        [Pure]
        public double Dot(Vector4d v2)
        {
            return this.X * v2.X + this.Y * v2.Y + this.Z * v2.Z + this.W * v2.W;
        }

        [Pure]
        public double Proy(Vector4d v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public Vector4d ProyV(Vector4d v2)
        {
            return this.Mul(this.Proy(v2));
        }

        #region parse

        public static Vector4d Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector4d result;
            if (!TryParse(s, out result, provider, vstyle, style))
            {
                throw new Exception();
            }
            return result;
        }

        public static bool TryParse(string s,
                                    out Vector4d result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            double[] ret;
            if (!VectorUtils.TryParse(s, 4, out ret, double.TryParse, provider, vstyle, style))
            {
                result = Zero;
                return false;
            }
            result = new Vector4d(ret[0], ret[1], ret[2], ret[3]);
            return true;
        }

        #endregion

        #region private

        [Pure]
        private bool EpsilonEquals(double x, double y, double z, double w, double epsilon = EPSILON)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon) && this.Z.EpsilonEquals(z, epsilon) && this.W.EpsilonEquals(w, epsilon);
        }

        [Pure]
        private bool Equals(double x, double y, double z, double w)
        {
            return (this.X == x) && (this.Y == y) && (this.Z == z) && (this.W == w);
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
            return (obj is IVector4) && this.Equals((IVector4)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y, this.Z, this.W);
        }

        #endregion

        #region IEpsilonEquatable<Vector4d>

        [Pure]
        public bool EpsilonEquals(Vector4d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, other.Z, other.W, epsilon);
        }

        #endregion

        #region IEpsilonEquatable<IVector4>

        [Pure]
        public bool EpsilonEquals(IVector4 other, double epsilon = EPSILON)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, _other.W, epsilon);
        }

        #endregion

        #region IEquatable<Vector4d>

        [Pure]
        public bool Equals(Vector4d other)
        {
            return this.Equals(other.X, other.Y, other.Z, other.W);
        }

        #endregion

        #region IEquatable<IVector4>

        [Pure]
        public bool Equals(IVector4 other)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y, _other.Z, _other.W);
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

        public Vector4d(SerializationInfo info, StreamingContext context)
        {
            this.X = info.GetDouble(_X);
            this.Y = info.GetDouble(_Y);
            this.Z = info.GetDouble(_Z);
            this.W = info.GetDouble(_W);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.X);
            info.AddValue(_Y, this.Y);
            info.AddValue(_Z, this.Z);
            info.AddValue(_W, this.W);
        }

        #endregion

        #region ITuple

        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y) && MathUtils.IsValid(this.Z) && MathUtils.IsValid(this.W); }
        }

        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y) || double.IsInfinity(this.Z) || double.IsInfinity(this.W); }
        }

        [Pure]
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

        #region ITuple4_Double

        double ITuple4_Double.X
        {
            get { return this.X; }
        }

        double ITuple4_Double.Y
        {
            get { return this.Y; }
        }

        double ITuple4_Double.Z
        {
            get { return this.Z; }
        }

        double ITuple4_Double.W
        {
            get { return this.W; }
        }

        #endregion

        #region IVector

        [Pure]
        public bool IsUnit
        {
            get { return this.Length.EpsilonEquals(1); }
        }

        [Pure]
        public double Length
        {
            get { return (double)Math.Sqrt(this.LengthSquared); }
        }

        [Pure]
        public double LengthSquared
        {
            get { return this.Dot(this); }
        }

        [Pure]
        public double LengthL1
        {
            get { return Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z) + Math.Abs(this.W); }
        }

        [Pure]
        public double Dot(IVector4 v2)
        {
            ITuple4_Double _v2 = v2.AsTupleDouble();
            return this.X * _v2.X + this.Y * _v2.Y + this.Z * _v2.Z + this.W * _v2.W;
        }

        [Pure]
        public double Proj(IVector4 v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public double InvLerp(IVector4 v2, IVector4 vLerp)
        {
            BuffVector4d v12 = new BuffVector4d(v2);
            v12.Sub(this);
            BuffVector4d v1Lerp = new BuffVector4d(vLerp);
            v1Lerp.Sub(this);
            return v12.Proj(v1Lerp);
        }

        #endregion
    }
}