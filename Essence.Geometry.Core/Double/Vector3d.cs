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
    public struct Vector3d : IVector3, ITuple3_Double,
                             IEpsilonEquatable<Vector3d>,
                             IEquatable<Vector3d>,
                             IFormattable,
                             ISerializable
    {
        /// <summary>Name of the property <code>X</code>.</summary>
        public const string _X = "X";

        /// <summary>Name of the property <code>Y</code>.</summary>
        public const string _Y = "Y";

        /// <summary>Name of the property <code>Z</code>.</summary>
        public const string _Z = "Z";

        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Vector zero.</summary>
        public static readonly Vector3d Zero = new Vector3d(0, 0, 0);

        /// <summary>Vector one.</summary>
        public static readonly Vector3d One = new Vector3d(1, 1, 1);

        /// <summary>Vector with properties X = 1 and others = 0.</summary>
        public static readonly Vector3d UX = new Vector3d(1, 0, 0);

        /// <summary>Vector with properties Y = 1 and others = 0.</summary>
        public static readonly Vector3d UY = new Vector3d(0, 1, 0);

        /// <summary>Vector with properties Z = 1 and others = 0.</summary>
        public static readonly Vector3d UZ = new Vector3d(0, 0, 1);

        public Vector3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3d(IVector3 v)
        {
            ITuple3_Double _v = v.AsTupleDouble();
            this.X = _v.X;
            this.Y = _v.Y;
            this.Z = _v.Z;
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        /// <summary>Property Z.</summary>
        public readonly double Z;

        #region operators

        public static explicit operator double[](Vector3d v)
        {
            return new double[] { v.X, v.Y, v.Z };
        }

        public static Vector3d operator -(Vector3d v1)
        {
            return v1.Neg();
        }

        public static Vector3d operator +(Vector3d v1, Vector3d v2)
        {
            return v1.Add(v2);
        }

        public static Vector3d operator -(Vector3d v1, Vector3d v2)
        {
            return v1.Sub(v2);
        }

        public static Vector3d operator *(Vector3d v, double c)
        {
            return v.Mul(c);
        }

        public static Vector3d operator *(double c, Vector3d v)
        {
            return v.Mul(c);
        }

        public static Vector3d operator /(Vector3d v, double c)
        {
            return v.Div(c);
        }

        public static implicit operator Point3d(Vector3d p)
        {
            return new Point3d(p.X, p.Y, p.Z);
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
        public Vector3d Unit
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
        public Vector3d Add(Vector3d v2)
        {
            return new Vector3d(this.X + v2.X, this.Y + v2.Y, this.Z + v2.Z);
        }

        [Pure]
        public Vector3d Sub(Vector3d v2)
        {
            return new Vector3d(this.X - v2.X, this.Y - v2.Y, this.Z - v2.Z);
        }

        [Pure]
        public Vector3d Mul(double c)
        {
            return new Vector3d(this.X * c, this.Y * c, this.Z * c);
        }

        [Pure]
        public Vector3d Div(double c)
        {
            return new Vector3d(this.X / c, this.Y / c, this.Z / c);
        }

        [Pure]
        public Vector3d SimpleMul(Vector3d v2)
        {
            return new Vector3d(this.X * v2.X, this.Y * v2.Y, this.Z * v2.Z);
        }

        [Pure]
        public Vector3d Neg()
        {
            return new Vector3d(-this.X, -this.Y, -this.Z);
        }

        [Pure]
        public Vector3d Abs()
        {
            return new Vector3d(Math.Abs(this.X), Math.Abs(this.Y), Math.Abs(this.Z));
        }

        [Pure]
        public Vector3d Lerp(Vector3d v2, double alpha)
        {
            return this.Lineal(v2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Vector3d v2, Vector3d vLerp)
        {
            Vector3d v12 = v2.Sub(this);
            return v12.Proy(vLerp.Sub(this));
        }

        [Pure]
        public Vector3d Lineal(Vector3d v2, double alpha, double beta)
        {
            return new Vector3d(alpha * this.X + beta * v2.X, alpha * this.Y + beta * v2.Y, alpha * this.Z + beta * v2.Z);
        }

        [Pure]
        public double Dot(Vector3d v2)
        {
            return this.X * v2.X + this.Y * v2.Y + this.Z * v2.Z;
        }

        [Pure]
        public double Proy(Vector3d v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public Vector3d ProyV(Vector3d v2)
        {
            return this.Mul(this.Proy(v2));
        }

        [Pure]
        public Vector3d Cross(Vector3d v2)
        {
            return new Vector3d((this.Y * v2.Z) - (this.Z * v2.Y),
                                (this.Z * v2.X) - (this.X * v2.Z),
                                (this.X * v2.Y) - (this.Y * v2.X));
        }

        [Pure]
        public double TripleProduct(Vector3d v2, Vector3d v3)
        {
            return this.Dot(v2.Cross(v3));
        }

        /// <summary>
        /// Evaluates the angle of <c>v2</c> with respect to <c>v1</c>.
        /// Angle in radians between [0, PI].
        /// </summary>
        public static double EvAngle(Vector3d v1, Vector3d v2)
        {
            double lon1 = v1.LengthSquared;
            double lon2 = v2.LengthSquared;

            if (lon1.EpsilonZero() || lon2.EpsilonZero())
            {
                return 0;
            }

            double pesc = v1.Dot(v2);
            double v = (double)(pesc / Math.Sqrt(lon1 * lon2));

            // Evita los valores 'raros'.
            if (v >= 1)
            {
                v = 1;
            }
            else if (v <= -1)
            {
                v = -1;
            }

            return (double)Math.Acos(v);
        }

        #region parse

        public static Vector3d Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector3d result;
            if (!TryParse(s, out result, provider, vstyle, style))
            {
                throw new Exception();
            }
            return result;
        }

        public static bool TryParse(string s,
                                    out Vector3d result,
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
            result = new Vector3d(ret[0], ret[1], ret[2]);
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
            return (obj is IVector3) && this.Equals((Vector3d)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y, this.Z);
        }

        #endregion

        #region IEpsilonEquatable<Vector3d>

        [Pure]
        public bool EpsilonEquals(Vector3d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, other.Z, epsilon);
        }

        #endregion

        #region IEpsilonEquatable<Vector3>

        [Pure]
        public bool EpsilonEquals(IVector3 other, double epsilon = EPSILON)
        {
            ITuple3_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, _other.Z, epsilon);
        }

        #endregion

        #region IEquatable<Vector3d>

        [Pure]
        public bool Equals(Vector3d other)
        {
            return this.Equals(this.X, this.Y, this.Z);
        }

        #endregion

        #region IEquatable<IVector3>

        [Pure]
        public bool Equals(IVector3 other)
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

        public Vector3d(SerializationInfo info, StreamingContext context)
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
            get { return Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z); }
        }

        [Pure]
        public double Dot(IVector3 v2)
        {
            ITuple3_Double _v2 = v2.AsTupleDouble();
            return this.X * _v2.X + this.Y * _v2.Y + this.Z * _v2.Z;
        }

        [Pure]
        public double Proj(IVector3 v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
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

        [Pure]
        public double TripleProduct(IVector3 v2, IVector3 v3)
        {
            return this.TripleProduct(v2.ToVector3d(), v3.ToVector3d());
        }

        #endregion
    }
}