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
    public struct Vector2d : IVector2D,
                             IEpsilonEquatable<Vector2d>,
                             IEquatable<Vector2d>,
                             IFormattable,
                             ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Vector zero.</summary>
        public static readonly Vector2d Zero = new Vector2d(0, 0);

        /// <summary>Vector one.</summary>
        public static readonly Vector2d One = new Vector2d(1, 1);

        /// <summary>Vector with property X = 1 and others = 0.</summary>
        public static readonly Vector2d UX = new Vector2d(1, 0);

        /// <summary>Vector with property Y = 1 and others = 0.</summary>
        public static readonly Vector2d UY = new Vector2d(0, 1);

        public static Vector2d NewRotate(double angle, double len = 1)
        {
            return new Vector2d(len * SysMath.Cos(angle), len * SysMath.Sin(angle));
        }

        public Vector2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2d(IVector2D v)
        {
            CoordinateSetter2d setter = new CoordinateSetter2d();
            v.GetCoordinates(setter);
            this.X = setter.X;
            this.Y = setter.Y;
        }

        public Vector2d(IVector v)
        {
            IVector2D v2 = v as IVector2D;
            if (v2 != null)
            {
                CoordinateSetter2d setter = new CoordinateSetter2d();
                v2.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
            }
            else
            {
                if (v.Dim < 2)
                {
                    throw new Exception("Vector no valido");
                }
                CoordinateSetter2d setter = new CoordinateSetter2d();
                v.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
            }
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        #region operators

        /// <summary>
        /// Casting to an array.
        /// </summary>
        public static explicit operator double[](Vector2d v)
        {
            return new[] { v.X, v.Y };
        }

        public static Vector2d operator -(Vector2d v1)
        {
            return v1.Neg();
        }

        public static Vector2d operator +(Vector2d v1, Vector2d v2)
        {
            return v1.Add(v2);
        }

        public static Vector2d operator -(Vector2d v1, Vector2d v2)
        {
            return v1.Sub(v2);
        }

        public static Vector2d operator *(Vector2d v, double c)
        {
            return v.Mul(c);
        }

        public static Vector2d operator *(double c, Vector2d v)
        {
            return v.Mul(c);
        }

        public static Vector2d operator /(Vector2d v, double c)
        {
            return v.Div(c);
        }

        public static implicit operator Point2d(Vector2d p)
        {
            return new Point2d(p.X, p.Y);
        }

        #endregion

        [Pure]
        public int Dim
        {
            get { return 2; }
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
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y); }
        }

        /// <summary>
        /// Tests if <code>this</code> vector is NaN (any coordinate is NaN).
        /// </summary>
        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y); }
        }

        /// <summary>
        /// Tests if <code>this</code> vector is infinity (any coordinate is infinity).
        /// </summary>
        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y); }
        }

        /// <summary>
        /// Tests if <code>this</code> vector is zero (all coordinates are 0).
        /// </summary>
        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0); }
        }

        /// <summary>
        /// Counterclockwise quadrant:
        /// <pre><![CDATA[
        ///       ^
        ///   1   |   0
        ///       |
        /// <-----+-----> p1
        ///       |
        ///   2   |   3
        ///       v
        /// ]]></pre>
        /// </summary>
        [Pure]
        public int Quadrant
        {
            get
            {
                return ((this.X >= 0)
                    ? ((this.Y >= 0) ? 0 : 3)
                    : ((this.Y >= 0) ? 1 : 2));
            }
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
        public Vector2d Unit
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
            get { return Math.Abs(this.X) + Math.Abs(this.Y); }
        }

        [Pure]
        public Vector2d Add(Vector2d v2)
        {
            return new Vector2d(this.X + v2.X, this.Y + v2.Y);
        }

        [Pure]
        public Vector2d Sub(Vector2d v2)
        {
            return new Vector2d(this.X - v2.X, this.Y - v2.Y);
        }

        [Pure]
        public Vector2d Mul(double c)
        {
            return new Vector2d(this.X * c, this.Y * c);
        }

        [Pure]
        public Vector2d Div(double c)
        {
            return new Vector2d(this.X / c, this.Y / c);
        }

        [Pure]
        public Vector2d SimpleMul(Vector2d v2)
        {
            return new Vector2d(this.X * v2.X, this.Y * v2.Y);
        }

        [Pure]
        public Vector2d Neg()
        {
            return new Vector2d(-this.X, -this.Y);
        }

        [Pure]
        public Vector2d Abs()
        {
            return new Vector2d(Math.Abs(this.X), Math.Abs(this.Y));
        }

        [Pure]
        public Vector2d Lerp(Vector2d v2, double alpha)
        {
            return this.Lineal(v2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Vector2d v2, Vector2d vLerp)
        {
            double x = (v2.X - this.X), y = (v2.Y - this.Y);

            double a = x * (vLerp.X - this.X)
                       + y * (vLerp.Y - this.Y);
            double b = x * x
                       + y * y;
            return a / Math.Sqrt(b);

            //Vector2d v12 = v2.Sub(this);
            //return v12.Proj(vLerp.Sub(this));
            //return v12.Dot(vLerp.Sub(this)) / v12.Length;
        }

        [Pure]
        public Vector2d Lineal(Vector2d v2, double alpha, double beta)
        {
            return new Vector2d(alpha * this.X + beta * v2.X, alpha * this.Y + beta * v2.Y);
        }

        [Pure]
        public double Cross(Vector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        [Pure]
        public double Dot(Vector2d v2)
        {
            return this.X * v2.X + this.Y * v2.Y;
        }

        [Pure]
        public double Proy(Vector2d v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public Vector2d ProyV(Vector2d v2)
        {
            return this.Mul(this.Proy(v2));
        }

        /// <summary>
        /// Evaluates the angle of <c>this</c> vector with respect to the X axis.
        /// It is the same as 'new Vector2d(1, 0).AngleTo(v)'.
        /// <pre><![CDATA[
        ///   ^           __
        ///   |          _/| this
        ///   |        _/
        ///   |      _/
        ///   |    _/ __
        ///   |  _/   |\ angulo +
        ///   |_/       |
        /// --+------------> X
        /// origen      |
        ///   |   \_  |/  angulo -
        ///   |     \_|--
        ///   |       \_
        ///   |         \_
        ///   |           \|
        ///   v          --| this
        /// ]]></pre>
        /// </summary>
        [Pure]
        public double Angle
        {
            get { return (double)SysMath.Atan2(this.Y, this.X); }
        }

        /// <summary>
        /// Evaluates the angle of <c>other</c> vector with respect to <code>this</code> vector.
        /// Angle in radians between [-PI, PI].
        /// If it is a clockwise rotation [0, PI] then it is positive.
        /// If it is a counterclockwise rotation [-PI, 0] then it is negative.
        /// <pre><![CDATA[
        ///               __
        ///              _/| other
        ///            _/
        ///          _/
        ///        _/ __
        ///      _/   |\ angulo +
        ///    _/       |
        ///   +          |
        /// origen      |
        ///       \_   /
        ///         \_/  
        ///           \_
        ///             \_
        ///               \|
        ///              --| this
        /// ]]></pre>
        /// </summary>
        /// <param name="other">Vector.</param>
        /// <returns>Angulo.</returns>
        [Pure]
        public double AngleTo(Vector2d other)
        {
            //return (this.Angle(b) - this.Angle(a));
            // http://stackoverflow.com/questions/2150050/finding-signed-angle-between-vectors
            return SysMath.Atan2(this.X * other.Y - this.Y * other.X, this.X * other.X + this.Y * other.Y);
        }

        /// <summary>
        /// Gets the perpendicular vector of <code>this</code> to the right (Perp): (y, -x).
        /// </summary>
        [Pure]
        public Vector2d PerpRight
        {
            get { return new Vector2d(this.Y, -this.X); }
        }

        /// <summary>
        /// Gets the perpendicular vector of <code>this</code> to the left: (-y, x).
        /// </summary>
        [Pure]
        public Vector2d PerpLeft
        {
            get { return new Vector2d(-this.Y, this.X); }
        }

        /// <summary>
        /// Gets the perpendicular unit vector of <code>this</code> to the right (UnitPerp): PerpRight / Length.
        /// </summary>
        [Pure]
        public Vector2d UnitPerpRight
        {
            get { return this.PerpRight.Unit; }
        }

        /// <summary>
        /// Gets the perpendicular unit vector of <code>this</code> to the left: PerpLeft / Length.
        /// </summary>
        [Pure]
        public Vector2d UnitPerpLeft
        {
            get { return this.PerpLeft.Unit; }
        }

        /// <summary>
        /// Calculates the dot product of <code>this</code> vector with the perpendicular vector of <c>v</c> (dotPerp).
        /// NOTA: it is similar to the cross product.
        /// </summary>
        /// <param name="v2">Vector</param>
        /// <returns>DotPerp( ( x, y ), ( V.x, V.y ) ) = Dot( ( x, y ), PerpRight( v ) ) = x * V.y - y * V.x</returns>
        [Pure]
        public double DotPerpRight(Vector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        /// <summary>
        /// Rotates <code>this</code> vector.
        /// </summary>
        /// <param name="v2">Vector.</param>
        /// <param name="rad">Angle in radians.</param>
        public static Vector2d Rot(Vector2d v2, double rad)
        {
            double s = (double)Math.Sin(rad);
            double c = (double)Math.Cos(rad);
            return new Vector2d(v2.X * c - v2.Y * s, v2.X * s + v2.Y * c);
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
        public static Vector2d Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector2d result;
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
                                    out Vector2d result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            double[] ret;
            if (!VectorUtils.TryParse(s, 2, out ret, double.TryParse, provider, vstyle, nstyle))
            {
                result = Zero;
                return false;
            }
            result = new Vector2d(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

        /// <summary>
        /// Tests if the coordinates of <code>this</code> vector are equals to <code>x</code> and <code>y</code>.
        /// </summary>
        [Pure]
        private bool EpsilonEquals(double x, double y, double epsilon = MathUtils.ZERO_TOLERANCE)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon);
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
            if (!(obj is Vector2d))
            {
                return false;
            }

            return this.Equals((Vector2d)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y);
        }

        #endregion

        #region IEpsilonEquatable<Vector2d>

        [Pure]
        public bool EpsilonEquals(Vector2d other, double epsilon = MathUtils.EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, epsilon);
        }

        #endregion

        #region IEquatable<Vector2d>

        [Pure]
        public bool Equals(Vector2d other)
        {
            return other.X == this.X && other.Y == this.Y;
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

        public Vector2d(SerializationInfo info, StreamingContext context)
        {
            this.X = info.GetDouble(_X);
            this.Y = info.GetDouble(_Y);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.X);
            info.AddValue(_Y, this.Y);
        }

        #endregion

        #region IVector

        //int Dim { get; }

        void IVector.GetCoordinates(ICoordinateSetter setter)
        {
            setter.SetCoords(this.X, this.Y);
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
            return this.Add(v2.ToVector2d());
        }

        [Pure]
        IVector IVector.Sub(IVector v2)
        {
            return this.Sub(v2.ToVector2d());
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
            return this.SimpleMul(v2.ToVector2d());
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
            return this.Lerp(v2.ToVector2d(), alpha);
        }

        [Pure]
        double IVector.InvLerp(IVector v2, IVector vLerp)
        {
            return this.InvLerp(v2.ToVector2d(), vLerp.ToVector2d());
        }

        [Pure]
        IVector IVector.Lineal(IVector v2, double alpha, double beta)
        {
            return this.Lineal(v2.ToVector2d(), alpha, beta);
        }

        [Pure]
        double IVector.Dot(IVector v2)
        {
            return this.Dot(v2.ToVector2d());
        }

        [Pure]
        double IVector.Proj(IVector v2)

        {
            return this.Proy(v2.ToVector2d());
        }

        [Pure]
        IVector IVector.ProjV(IVector v2)
        {
            return this.ProyV(v2.ToVector2d());
        }

        #endregion

        #region IVector2D

        void IVector2D.GetCoordinates(ICoordinateSetter2D setter)
        {
            setter.SetCoords(this.X, this.Y);
        }

        [Pure]
        double IVector2D.Cross(IVector2D v2)
        {
            return this.Cross(v2.ToVector2d());
        }

        #endregion

        #region IEpsilonEquatable<IVector>

        [Pure]
        bool IEpsilonEquatable<IVector>.EpsilonEquals(IVector other, double epsilon)
        {
            return this.EpsilonEquals(other.ToVector2d(), epsilon);
        }

        #endregion

        #region inner classes

        /// <summary>
        /// This class compares vectors by coordinate (X or Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Vector2d>, IComparer
        {
            public CoordComparer(int coord, double epsilon = MathUtils.EPSILON)
            {
                this.coord = coord;
                this.epsilon = epsilon;
            }

            private readonly int coord;
            private readonly double epsilon;

            public int Compare(Vector2d v1, Vector2d v2)
            {
                switch (this.coord)
                {
                    case 0:
                        return v1.X.EpsilonCompareTo(v2.X, this.epsilon);
                    case 1:
                        return v1.Y.EpsilonCompareTo(v2.Y, this.epsilon);
                }
                throw new IndexOutOfRangeException();
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector2d && o2 is Vector2d);
                return this.Compare((Vector2d)o1, (Vector2d)o2);
            }
        }

        /// <summary>
        /// This class lexicographically compares vectors: it compares X -> Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Vector2d>, IComparer
        {
            public LexComparer(double epsilon = MathUtils.EPSILON)
            {
                this.epsilon = epsilon;
            }

            private readonly double epsilon;

            public int Compare(Vector2d v1, Vector2d v2)
            {
                int i;
                i = v1.X.EpsilonCompareTo(v2.X, this.epsilon);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Y.EpsilonCompareTo(v2.Y, this.epsilon);
                return i;
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector2d && o2 is Vector2d);
                return this.Compare((Vector2d)o1, (Vector2d)o2);
            }
        }

        /// <summary>
        /// This class compares vectors using their length.
        /// </summary>
        public sealed class LengthComparer : IComparer<Vector2d>, IComparer
        {
            public int Compare(Vector2d v1, Vector2d v2)
            {
                return v1.LengthSquared.CompareTo(v2.LengthSquared);
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector2d && o2 is Vector2d);
                return this.Compare((Vector2d)o1, (Vector2d)o2);
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
        public struct AngleComparer : IComparer<Vector2d>, IComparer
        {
            public AngleComparer(Vector2d direccion)
                : this(direccion, direccion.PerpLeft)
            {
            }

            public AngleComparer(Vector2d direccion, Vector2d normal)
            {
                Contract.Assert(direccion.IsUnit);
                this.direccion = direccion;
                this.normal = normal;
            }

            private readonly Vector2d direccion;
            private readonly Vector2d normal;

            public int Compare(Vector2d v1, Vector2d v2)
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
                Contract.Requires(o1 is Vector2d && o2 is Vector2d);
                return this.Compare((Vector2d)o1, (Vector2d)o2);
            }
        }

        public struct AngleComparer2 : IComparer<Vector2d>, IComparer
        {
            public int Compare(Vector2d v1, Vector2d v2)
            {
                // Se hace un cambio de cuadrande:
                //   0  |  1
                //  ----+----
                //   3  |  2
                //int c1 = (5 - v1.Cuadrante) % 4;
                //int c2 = (5 - v2.Cuadrante) % 4;
                int c1 = v1.Quadrant;
                int c2 = v2.Quadrant;
                if (c1 == c2)
                {
                    // Se convierten al cuadrante 0.
                    switch (c1)
                    {
                        case 1:
                            // Rotacion 90 a la derecha.
                            v1 = Rot(v1, (double)(-Math.PI / 2)); // -1
                            break;
                        case 2:
                            // Rotacion 180 a la derecha.
                            v1 = Rot(v1, (double)(-Math.PI)); // -2
                            break;
                        case 3:
                            // Rotacion 270 a la derecha.
                            v1 = Rot(v1, (double)(-3 * Math.PI / 2)); // -3
                            break;
                    }
                    double m1 = v1.Y / v1.X;
                    double m2 = v2.Y / v2.X;
                    return m1.CompareTo(m2);
                }
                else
                {
                    return c1.CompareTo(c2);
                }
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector2d && o2 is Vector2d);
                return this.Compare((Vector2d)o1, (Vector2d)o2);
            }
        }

        #endregion
    }
}