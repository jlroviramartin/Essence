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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using Essence.Util.Math;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Core.Double
{
    public struct Vector4d : IVector4D,
                             IEpsilonEquatable<Vector4d>,
                             IEquatable<Vector4d>,
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

        public Vector4d(IVector4D v)
        {
            CoordinateSetter4d setter = new CoordinateSetter4d();
            v.GetCoordinates(setter);
            this.X = setter.X;
            this.Y = setter.Y;
            this.Z = setter.Z;
            this.W = setter.W;
        }

        public Vector4d(IVector v)
        {
            IVector4D v4 = v as IVector4D;
            if (v4 != null)
            {
                CoordinateSetter4d setter = new CoordinateSetter4d();
                v4.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
                this.Z = setter.Z;
                this.W = setter.W;
            }
            else
            {
                if (v.Dim < 4)
                {
                    throw new Exception("Vector no valido");
                }
                CoordinateSetter4d setter = new CoordinateSetter4d();
                v.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
                this.Z = setter.Z;
                this.W = setter.W;
            }
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

        /// <summary>
        /// Casting to an array.
        /// </summary>
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

        /// <summary>
        /// Tests if <code>this</code> vector is valid (not any coordinate is NaN or infinity).
        /// </summary>
        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y) && MathUtils.IsValid(this.Z) && MathUtils.IsValid(this.W); }
        }

        /// <summary>
        /// Tests if <code>this</code> vector is NaN (any coordinate is NaN).
        /// </summary>
        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z) || double.IsNaN(this.W); }
        }

        /// <summary>
        /// Tests if <code>this</code> vector is infinity (any coordinate is infinity).
        /// </summary>
        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y) || double.IsInfinity(this.Z) || double.IsInfinity(this.W); }
        }

        /// <summary>
        /// Tests if <code>this</code> vector is zero (all coordinates are 0).
        /// </summary>
        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0, 0, 0); }
        }

        /// <summary>
        /// Tests if <code>this</code> vector is unit.
        /// </summary>
        [Pure]
        public bool IsUnit
        {
            get { return this.Length.EpsilonEquals(1); }
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
            double x = (v2.X - this.X), y = (v2.Y - this.Y), z = (v2.Z - this.Z), w = (v2.W - this.W);

            double a = x * (vLerp.X - this.X)
                       + y * (vLerp.Y - this.Y)
                       + z * (vLerp.Z - this.Z)
                       + w * (vLerp.W - this.W);
            double b = x * x
                       + y * y
                       + z * z
                       + w * w;
            return a / Math.Sqrt(b);
            //Vector4d v12 = v2.Sub(this);
            //return v12.Proj(vLerp.Sub(this));
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

        /// <summary>
        /// Parses the <code>s</code> string using <code>vstyle</code> and <code>nstyle</code> styles.
        /// </summary>
        /// <param name="s">String.</param>
        /// <param name="provider">Provider.</param>
        /// <param name="vstyle">Vector style.</param>
        /// <param name="nstyle">Number style.</param>
        /// <returns>Vector.</returns>
        public static Vector4d Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector4d result;
            if (!TryParse(s, out result, provider, vstyle, nstyle))
            {
                throw new Exception();
            }
            return result;
        }

        /// <summary>
        /// Tries to parse the <code>s</code> string using <code>vstyle</code> and <code>nstyle</code> styles.
        /// </summary>
        /// <param name="s">String.</param>
        /// <param name="provider">Provider.</param>
        /// <param name="vstyle">Vector style.</param>
        /// <param name="nstyle">Number style.</param>
        /// <param name="result">Vector.</param>
        /// <returns><code>True</code> if everything is correct, <code>false</code> otherwise.</returns>
        public static bool TryParse(string s,
                                    out Vector4d result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            double[] ret;
            if (!VectorUtils.TryParse(s, 4, out ret, double.TryParse, provider, vstyle, nstyle))
            {
                result = Zero;
                return false;
            }
            result = new Vector4d(ret[0], ret[1], ret[2], ret[3]);
            return true;
        }

        #endregion

        #region protected

        #endregion

        #region private

        /// <summary>
        /// Tests if the coordinates of <code>this</code> vector are equals to <code>x</code>, <code>y</code>, <code>z</code> and <code>w</code>.
        /// </summary>
        [Pure]
        private bool EpsilonEquals(double x, double y, double z, double w, double epsilon = MathUtils.ZERO_TOLERANCE)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon) && this.Z.EpsilonEquals(z, epsilon) && this.W.EpsilonEquals(w, epsilon);
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
            if (!(obj is Vector4d))
            {
                return false;
            }

            return this.Equals((Vector4d)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y, this.Z, this.W);
        }

        #endregion

        #region IEpsilonEquatable<Vector4d>

        [Pure]
        public bool EpsilonEquals(Vector4d other, double epsilon = MathUtils.EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, other.Z, other.W, (double)epsilon);
        }

        #endregion

        #region IEquatable<Vector4d>

        [Pure]
        public bool Equals(Vector4d other)
        {
            return other.X == this.X && other.Y == this.Y && other.Z == this.Z && other.W == this.W;
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

        #region IVector

        //int Dim { get; }

        void IVector.GetCoordinates(ICoordinateSetter setter)
        {
            setter.SetCoords(this.X, this.Y, this.Z, this.W);
        }

        [Pure]
        IVector IVector.Unit
        {
            get { return this.Unit; }
        }

        //[Pure]
        //REAL Length { get; }

        //[Pure]
        //REAL LengthSquared { get; }

        //[Pure]
        //REAL LengthL1 { get; }

        [Pure]
        IVector IVector.Add(IVector v2)
        {
            return this.Add(v2.ToVector4d());
        }

        [Pure]
        IVector IVector.Sub(IVector v2)
        {
            return this.Sub(v2.ToVector4d());
        }

        [Pure]
        IVector IVector.Mul(double c)
        {
            return this.Mul(c);
        }

        [Pure]
        IVector IVector.Div(double c)
        {
            return this.Div(c);
        }

        [Pure]
        IVector IVector.SimpleMul(IVector v2)
        {
            return this.SimpleMul(v2.ToVector4d());
        }

        [Pure]
        IVector IVector.Neg()
        {
            return this.Neg();
        }

        [Pure]
        IVector IVector.Abs()
        {
            return this.Abs();
        }

        [Pure]
        IVector IVector.Lerp(IVector v2, double alpha)
        {
            return this.Lerp(v2.ToVector4d(), alpha);
        }

        [Pure]
        double IVector.InvLerp(IVector v2, IVector vLerp)
        {
            return this.InvLerp(v2.ToVector4d(), vLerp.ToVector4d());
        }

        [Pure]
        IVector IVector.Lineal(IVector v2, double alpha, double beta)
        {
            return this.Lineal(v2.ToVector4d(), alpha, beta);
        }

        [Pure]
        double IVector.Dot(IVector v2)
        {
            return this.Dot(v2.ToVector4d());
        }

        [Pure]
        double IVector.Proj(IVector v2)
        {
            return this.Proy(v2.ToVector4d());
        }

        [Pure]
        IVector IVector.ProjV(IVector v2)
        {
            return this.ProyV(v2.ToVector4d());
        }

        #endregion

        #region IVector4D

        void IVector4D.GetCoordinates(ICoordinateSetter4D setter)
        {
            setter.SetCoords(this.X, this.Y, this.Z, this.W);
        }

        #endregion

        #region IEpsilonEquatable<IVector>

        [Pure]
        bool IEpsilonEquatable<IVector>.EpsilonEquals(IVector other, double epsilon)
        {
            return this.EpsilonEquals(other.ToVector4d(), epsilon);
        }

        #endregion

        #region inner classes

        /// <summary>
        /// This class compares vectors by coordinate (X or Y or Z).
        /// </summary>
        public sealed class CoordComparer : IComparer<Vector4d>, IComparer
        {
            public CoordComparer(int coord, double epsilon = MathUtils.EPSILON)
            {
                this.coord = coord;
                this.epsilon = epsilon;
            }

            private readonly int coord;
            private readonly double epsilon;

            public int Compare(Vector4d v1, Vector4d v2)
            {
                switch (this.coord)
                {
                    case 0:
                        return v1.X.EpsilonCompareTo(v2.X, this.epsilon);
                    case 1:
                        return v1.Y.EpsilonCompareTo(v2.Y, this.epsilon);
                    case 2:
                        return v1.Z.EpsilonCompareTo(v2.Z, this.epsilon);
                    case 3:
                        return v1.W.EpsilonCompareTo(v2.W, this.epsilon);
                }
                throw new IndexOutOfRangeException();
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector4d && o2 is Vector4d);
                return this.Compare((Vector4d)o1, (Vector4d)o2);
            }
        }

        /// <summary>
        /// This class lexicographically compares vectors: it compares X -> Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Vector4d>, IComparer
        {
            public LexComparer(double epsilon = MathUtils.EPSILON)
            {
                this.epsilon = epsilon;
            }

            private readonly double epsilon;

            public int Compare(Vector4d v1, Vector4d v2)
            {
                int i;
                i = v1.X.EpsilonCompareTo(v2.X, this.epsilon);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Y.EpsilonCompareTo(v2.Y, this.epsilon);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Z.EpsilonCompareTo(v2.Z, this.epsilon);
                if (i != 0)
                {
                    return i;
                }
                i = v1.W.EpsilonCompareTo(v2.W, this.epsilon);
                return i;
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector4d && o2 is Vector4d);
                return this.Compare((Vector4d)o1, (Vector4d)o2);
            }
        }

        /// <summary>
        /// This class compares vectors using their length.
        /// </summary>
        public sealed class LengthComparer : IComparer<Vector4d>, IComparer
        {
            public int Compare(Vector4d v1, Vector4d v2)
            {
                return v1.LengthSquared.CompareTo(v2.LengthSquared);
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector4d && o2 is Vector4d);
                return this.Compare((Vector4d)o1, (Vector4d)o2);
            }
        }

        /// <summary>
        /// This class compares unit vectors using their angle.
        /// <![CDATA[
        ///  ^ normal = direccion.PerpLeft
        ///  |
        ///  | /__
        ///  | \  \  incrementa el angulo
        ///  |     |
        ///  +-----+-----------> direccion
        /// ]]>
        /// </summary>
        public struct AngleComparer : IComparer<Vector4d>, IComparer
        {
            public AngleComparer(Vector4d direccion, Vector4d normal)
            {
                Contract.Assert(direccion.IsUnit);
                this.direccion = direccion;
                this.normal = normal;
            }

            private readonly Vector4d direccion;
            private readonly Vector4d normal;

            public int Compare(Vector4d v1, Vector4d v2)
            {
                if (v1.IsZero)
                {
                    if (v2.IsZero)
                    {
                        return 0;
                    }
                    return -1; // v2 es mayor.
                }
                else if (v2.IsZero)
                {
                    return 1; // v1 es mayor.
                }

                Contract.Assert(v1.IsUnit && v2.IsUnit);

                double nv1 = this.normal.Dot(v1);
                if (nv1 > 0)
                {
                    // v1 esta encima.
                    double nv2 = this.normal.Dot(v2);
                    if (nv2 > 0)
                    {
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                    else if (nv2 < 0)
                    {
                        return -1; // v2 es mayor.
                    }
                    else
                    {
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                }
                else if (nv1 < 0)
                {
                    // v1 esta debajo.
                    double nv2 = this.normal.Dot(v2);
                    if (nv2 > 0)
                    {
                        return 1; // v1 es mayor.
                    }
                    else if (nv2 < 0)
                    {
                        return this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                    else
                    {
                        return 1;
                    }
                }
                else // if (nv1 == 0)
                {
                    // this.direccion.Dot(v1); // Es +1 o -1

                    // v1 esta alineado.
                    double nv2 = this.normal.Dot(v2);
                    if (nv2 > 0)
                    {
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                    else if (nv2 < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                }
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector4d && o2 is Vector4d);
                return this.Compare((Vector4d)o1, (Vector4d)o2);
            }
        }

        #endregion
    }
}