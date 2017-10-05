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
    public struct Point4d : IPoint4, ITuple4_Double,
                            IEpsilonEquatable<Point4d>,
                            IEquatable<Point4d>,
                            IFormattable,
                            ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Name of the property Z.</summary>
        public const string _Z = "Z";

        /// <summary>Name of the property W.</summary>
        public const string _W = "W";

        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Point zero.</summary>
        public static readonly Point4d Zero = new Point4d(0, 0, 0, 0);

        /// <summary>Point one.</summary>
        public static readonly Point4d One = new Point4d(1, 1, 1, 1);

        /// <summary>Point with property X = 1 and others = 0.</summary>
        public static readonly Point4d UX = new Point4d(1, 0, 0, 0);

        /// <summary>Point with property Y = 1 and others = 0.</summary>
        public static readonly Point4d UY = new Point4d(0, 1, 0, 0);

        /// <summary>Point with property Z = 1 and others = 0.</summary>
        public static readonly Point4d UZ = new Point4d(0, 0, 1, 0);

        /// <summary>Point with property W = 1 and others = 0.</summary>
        public static readonly Point4d UW = new Point4d(0, 0, 0, 1);

        public Point4d(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Point4d(IPoint4 p)
        {
            ITuple4_Double _p = p.AsTupleDouble();
            this.X = _p.X;
            this.Y = _p.Y;
            this.Z = _p.Z;
            this.W = _p.W;
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

        public static explicit operator double[](Point4d v)
        {
            return new[] { v.X, v.Y, v.Z, v.W };
        }

        public static Point4d operator +(Point4d p, Vector4d v)
        {
            return p.Add(v);
        }

        public static Point4d operator -(Point4d p, Vector4d v)
        {
            return p.Sub(v);
        }

        public static Vector4d operator -(Point4d p1, Point4d p2)
        {
            return p1.Sub(p2);
        }

        public static implicit operator Vector4d(Point4d p)
        {
            return new Vector4d(p.X, p.Y, p.Z, p.W);
        }

        #endregion

        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z) || double.IsNaN(this.W); }
        }

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
        public Point4d Add(Vector4d v)
        {
            return new Point4d(this.X + v.X, this.Y + v.Y, this.Z + v.Z, this.W + v.W);
        }

        [Pure]
        public Point4d Sub(Vector4d v)
        {
            return new Point4d(this.X - v.X, this.Y - v.Y, this.Z - v.Z, this.W - v.W);
        }

        [Pure]
        public Vector4d Sub(Point4d p2)
        {
            return new Vector4d(this.X - p2.X, this.Y - p2.Y, this.Z - p2.Z, this.W - p2.W);
        }

        [Pure]
        public double Distance2To(Point4d p2)
        {
            return (MathUtils.Cuad(p2.X - this.X) + MathUtils.Cuad(p2.Y - this.Y) + MathUtils.Cuad(p2.Z - this.Z) + MathUtils.Cuad(p2.W - this.W));
        }

        [Pure]
        public double DistanceTo(Point4d p2)
        {
            return (double)Math.Sqrt(this.Distance2To(p2));
        }

        [Pure]
        public Point4d Lerp(Point4d p2, double alpha)
        {
            return this.Lineal(p2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Point4d p2, Point4d p3)
        {
            Vector4d v12 = p2.Sub(this);
            return v12.Proy(p3.Sub(this));
        }

        [Pure]
        public Point4d Lineal(Point4d p2, double alpha, double beta)
        {
            return new Point4d(alpha * this.X + beta * p2.X, alpha * this.Y + beta * p2.Y, alpha * this.Z + beta * p2.Z, alpha * this.W + beta * p2.W);
        }

        #region parse

        public static Point4d Parse(string s,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Point4d result;
            if (!TryParse(s, out result, provider, vstyle, style))
            {
                throw new Exception();
            }
            return result;
        }

        public static bool TryParse(string s,
                                    out Point4d result,
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
            result = new Point4d(ret[0], ret[1], ret[2], ret[3]);
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
            return (obj is Point4d) && this.Equals((Point4d)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y, this.Z, this.W);
        }

        #endregion

        #region IEpsilonEquatable<Point4d>

        [Pure]
        public bool EpsilonEquals(Point4d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, other.Z, other.W, epsilon);
        }

        #endregion

        #region IEpsilonEquatable<IPoint4>

        [Pure]
        public bool EpsilonEquals(IPoint4 other, double epsilon = EPSILON)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, _other.W, epsilon);
        }

        #endregion

        #region IEquatable<Point4d>

        [Pure]
        public bool Equals(Point4d other)
        {
            return this.Equals(other.X, other.Y, other.Z, other.W);
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

        #region IEquatable<IPoint4>

        [Pure]
        public bool Equals(IPoint4 other)
        {
            ITuple4_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y, _other.Z, _other.W);
        }

        #endregion

        #region ISerializable

        public Point4d(SerializationInfo info, StreamingContext context)
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

        #region IPoint

        [Pure]
        public double InvLerp(IPoint4 p2, IPoint4 pLerp)
        {
            BuffVector4d v12 = new BuffVector4d();
            v12.Sub(p2, this);
            BuffVector4d v1Lerp = new BuffVector4d();
            v1Lerp.Sub(pLerp, this);
            return v12.Proj(v1Lerp);
        }

        #endregion
    }
}