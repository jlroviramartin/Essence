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
    public struct Point3d : IPoint3, ITuple3_Double,
                            IEpsilonEquatable<Point3d>,
                            IEquatable<Point3d>,
                            IFormattable,
                            ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Name of the property Z.</summary>
        public const string _Z = "Z";

        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Point zero.</summary>
        public static readonly Point3d Zero = new Point3d(0, 0, 0);

        /// <summary>Point one.</summary>
        public static readonly Point3d One = new Point3d(1, 1, 1);

        /// <summary>Point with property X = 1 and others = 0.</summary>
        public static readonly Point3d UX = new Point3d(1, 0, 0);

        /// <summary>Point with property Y = 1 and others = 0.</summary>
        public static readonly Point3d UY = new Point3d(0, 1, 0);

        /// <summary>Point with property Z = 1 and others = 0.</summary>
        public static readonly Point3d UZ = new Point3d(0, 0, 1);

        public Point3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3d(IPoint3 p)
        {
            ITuple3_Double _p = p.AsTupleDouble();
            this.X = _p.X;
            this.Y = _p.Y;
            this.Z = _p.Z;
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        /// <summary>Property Z.</summary>
        public readonly double Z;

        #region operators

        public static explicit operator double[](Point3d v)
        {
            return new[] { v.X, v.Y, v.Z };
        }

        public static Point3d operator +(Point3d p, Vector3d v)
        {
            return p.Add(v);
        }

        public static Point3d operator -(Point3d p, Vector3d v)
        {
            return p.Sub(v);
        }

        public static Vector3d operator -(Point3d p1, Point3d p2)
        {
            return p1.Sub(p2);
        }

        public static implicit operator Vector3d(Point3d p)
        {
            return new Vector3d(p.X, p.Y, p.Z);
        }

        #endregion

        [Pure]
        public int Dim
        {
            get { return 3; }
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
                }
                throw new IndexOutOfRangeException();
            }
        }

        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z); }
        }

        [Pure]
        public Point3d Add(Vector3d v)
        {
            return new Point3d(this.X + v.X, this.Y + v.Y, this.Z + v.Z);
        }

        [Pure]
        public Point3d Sub(Vector3d v)
        {
            return new Point3d(this.X - v.X, this.Y - v.Y, this.Z - v.Z);
        }

        [Pure]
        public Vector3d Sub(Point3d p2)
        {
            return new Vector3d(this.X - p2.X, this.Y - p2.Y, this.Z - p2.Z);
        }

        [Pure]
        public double Distance2To(Point3d p2)
        {
            return (MathUtils.Cuad(p2.X - this.X) + MathUtils.Cuad(p2.Y - this.Y) + MathUtils.Cuad(p2.Z - this.Z));
        }

        [Pure]
        public double DistanceTo(Point3d p2)
        {
            return Math.Sqrt(this.Distance2To(p2));
        }

        [Pure]
        public Point3d Lerp(Point3d p2, double alpha)
        {
            return this.Lineal(p2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Point3d p2, Point3d pLerp)
        {
            Vector3d v12 = p2.Sub(this);
            return v12.Proy(pLerp.Sub(this));
        }

        [Pure]
        public Point3d Lineal(Point3d p2, double alpha, double beta)
        {
            return new Point3d(alpha * this.X + beta * p2.X, alpha * this.Y + beta * p2.Y, alpha * this.Z + beta * p2.Z);
        }

        #region parse

        public static Point3d Parse(string s,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Point3d result;
            if (!TryParse(s, out result, provider, vstyle, style))
            {
                throw new Exception();
            }
            return result;
        }

        public static bool TryParse(string s,
                                    out Point3d result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            double[] ret;
            if (!VectorUtils.TryParse(s, 3, out ret, double.TryParse, provider, vstyle, style))
            {
                result = Zero;
                return false;
            }
            result = new Point3d(ret[0], ret[1], ret[2]);
            return true;
        }

        #endregion

        #region private

        [Pure]
        private bool EpsilonEquals(double x, double y, double z, double epsilon = EPSILON)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon) && this.Z.EpsilonEquals(z, epsilon);
        }

        [Pure]
        private bool Equals(double x, double y, double z)
        {
            return (this.X == x) && (this.Y == y) && (this.Z == z);
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
            return (obj is IPoint3) && this.Equals((Point3d)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y, this.Z);
        }

        #endregion

        #region IEpsilonEquatable<Point3d>

        [Pure]
        public bool EpsilonEquals(Point3d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, other.Z, epsilon);
        }

        #endregion

        #region IEpsilonEquatable<IPoint3>

        [Pure]
        public bool EpsilonEquals(IPoint3 other, double epsilon = EPSILON)
        {
            ITuple3_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, epsilon);
        }

        #endregion

        #region IEquatable<Point3d>

        [Pure]
        public bool Equals(Point3d other)
        {
            return this.Equals(other.X, other.Y, other.Z);
        }

        #endregion

        #region IEquatable<IPoint3>

        [Pure]
        public bool Equals(IPoint3 other)
        {
            ITuple3_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y, _other.Z);
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

        public Point3d(SerializationInfo info, StreamingContext context)
        {
            this.X = info.GetDouble(_X);
            this.Y = info.GetDouble(_Y);
            this.Z = info.GetDouble(_Z);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.X);
            info.AddValue(_Y, this.Y);
            info.AddValue(_Z, this.Z);
        }

        #endregion

        #region ITuple

        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y) && MathUtils.IsValid(this.Z); }
        }

        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y) || double.IsInfinity(this.Z); }
        }

        [Pure]
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

        #region ITuple3_Double

        double ITuple3_Double.X
        {
            get { return this.X; }
        }

        double ITuple3_Double.Y
        {
            get { return this.Y; }
        }

        double ITuple3_Double.Z
        {
            get { return this.Z; }
        }

        #endregion

        #region IPoint

        [Pure]
        public double InvLerp(IPoint3 p2, IPoint3 pLerp)
        {
            BuffVector3d v12 = new BuffVector3d();
            v12.Sub(p2, this);
            BuffVector3d v1Lerp = new BuffVector3d();
            v1Lerp.Sub(pLerp, this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IPoint3

        [Pure]
        public int Octant
        {
            get
            {
                return ((this.X >= 0)
                    ? ((this.Y >= 0)
                        ? ((this.Z >= 0) ? 0 : 4)
                        : ((this.Z >= 0) ? 3 : 7))
                    : ((this.Y >= 0)
                        ? ((this.Z >= 0) ? 1 : 5)
                        : ((this.Z >= 0) ? 2 : 6)));
            }
        }

        #endregion
    }
}