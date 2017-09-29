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
    public struct Vector2d : IVector2, ITuple2_Double,
                             IEpsilonEquatable<Vector2d>,
                             IEquatable<Vector2d>,
                             IFormattable,
                             ISerializable
    {
        /// <summary>Name of the property <code>X</code>.</summary>
        public const string _X = "X";

        /// <summary>Name of the property <code>Y</code>.</summary>
        public const string _Y = "Y";

        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Vector zero.</summary>
        public static readonly Vector2d Zero = new Vector2d(0, 0);

        /// <summary>Vector one.</summary>
        public static readonly Vector2d One = new Vector2d(1, 1);

        /// <summary>Vector with properties X = 1 and others = 0.</summary>
        public static readonly Vector2d UX = new Vector2d(1, 0);

        /// <summary>Vector with properties Y = 1 and others = 0.</summary>
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

        public Vector2d(IVector2 v)
        {
            ITuple2_Double _v = v.AsTupleDouble();
            this.X = _v.X;
            this.Y = _v.Y;
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        #region operators

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

        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y); }
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
            Vector2d v12 = v2.Sub(this);
            return v12.Proj(vLerp.Sub(this));
        }

        [Pure]
        public Vector2d Lineal(Vector2d v2, double alpha, double beta)
        {
            return new Vector2d(alpha * this.X + beta * v2.X, alpha * this.Y + beta * v2.Y);
        }

        [Pure]
        public double Dot(Vector2d v2)
        {
            return this.X * v2.X + this.Y * v2.Y;
        }

        [Pure]
        public double Proj(Vector2d v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public Vector2d ProjV(Vector2d v2)
        {
            return this.Mul(this.Proj(v2));
        }

        [Pure]
        public double Cross(Vector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        /// <summary>
        /// Evaluates the angle of <c>this</c> with respect to the X axis. It is the same as 'new Vector2d(1, 0).AngleTo(v)'.
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
        /// <returns>Angle in radians between [-PI, PI].</returns>
        [Pure]
        public double Angle
        {
            get { return (double)SysMath.Atan2(this.Y, this.X); }
        }

        /// <summary>
        /// Evaluates the angle of <c>other</c> vector with respect to <code>this</code> vector.
        /// Es positivo si el giro es sentido horario [0, PI].
        /// Es negativo si el giro es sentido anti-horario [-PI, 0].
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
        /// <returns>Angle in radians between [-PI, PI].</returns>
        [Pure]
        public double AngleTo(Vector2d other)
        {
            //return (this.Angle(b) - this.Angle(a));
            // http://stackoverflow.com/questions/2150050/finding-signed-angle-between-vectors
            return SysMath.Atan2(this.X * other.Y - this.Y * other.X, this.X * other.X + this.Y * other.Y);
        }

        /// <summary>
        /// Perpendicular vector to the right (Perp) of <code>this</code> vector: (y, -x).
        /// </summary>
        [Pure]
        public Vector2d PerpRight
        {
            get { return new Vector2d(this.Y, -this.X); }
        }

        /// <summary>
        /// Perpendicular vector to the left of <code>this</code> vector: (-y, x).
        /// </summary>
        [Pure]
        public Vector2d PerpLeft
        {
            get { return new Vector2d(-this.Y, this.X); }
        }

        /// <summary>
        /// Perpendicular unit vector to the right (UnitPerp) of <code>this</code> vector: PerpRight / Length.
        /// </summary>
        [Pure]
        public Vector2d UnitPerpRight
        {
            get { return this.PerpRight.Unit; }
        }

        /// <summary>
        /// Perpendicular vector to the left of <code>this</code> vector: PerpLeft / Length.
        /// </summary>
        [Pure]
        public Vector2d UnitPerpLeft
        {
            get { return this.PerpLeft.Unit; }
        }

        /// <summary>
        /// Dot product of <code>this</code> and the perpendicular vector to the right of <c>v2</c> (dotPerp).
        /// returns DotPerp((x,y),(V.x,V.y)) = Dot((x,y),PerpRight(v)) = x*V.y - y*V.x
        /// NOTA: It is the same as the cross product.
        /// </summary>
        [Pure]
        public double DotPerpRight(Vector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        /// <summary>
        /// Rotates the <code>v2</code> vector.
        /// </summary>
        /// <param name="v2">Vector.</param>
        /// <param name="rad">Angle in radians.</param>
        public static Vector2d Rot(Vector2d v2, double rad)
        {
            double s = Math.Sin(rad);
            double c = Math.Cos(rad);
            return new Vector2d(v2.X * c - v2.Y * s, v2.X * s + v2.Y * c);
        }

        #region parse

        public static Vector2d Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector2d result;
            if (!TryParse(s, out result, provider, vstyle, style))
            {
                throw new Exception();
            }
            return result;
        }

        public static bool TryParse(string s,
                                    out Vector2d result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            double[] ret;
            if (!VectorUtils.TryParse(s, 2, out ret, double.TryParse, provider, vstyle, style))
            {
                result = Zero;
                return false;
            }
            result = new Vector2d(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

        [Pure]
        private bool EpsilonEquals(double x, double y, double epsilon = EPSILON)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon);
        }

        [Pure]
        private bool Equals(double x, double y)
        {
            return (this.X == x) && (this.Y == y);
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
            return (obj is IVector2) && this.Equals((IVector2)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y);
        }

        #endregion

        #region IEpsilonEquatable<Vector2d>

        [Pure]
        public bool EpsilonEquals(Vector2d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, epsilon);
        }

        #endregion

        #region IEpsilonEquatable<IVector2>

        [Pure]
        public bool EpsilonEquals(IVector2 other, double epsilon = EPSILON)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, epsilon);
        }

        #endregion

        #region IEquatable<Vector2d>

        [Pure]
        public bool Equals(Vector2d other)
        {
            return this.Equals(other.X, other.Y);
        }

        #endregion

        #region IEquatable<IVector2>

        [Pure]
        public bool Equals(IVector2 other)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y);
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

        #region ITuple

        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y); }
        }

        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y); }
        }

        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0); }
        }

        #endregion

        #region ITuple2_Double

        double ITuple2_Double.X
        {
            get { return this.X; }
        }

        double ITuple2_Double.Y
        {
            get { return this.Y; }
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
            get { return (double)Math.Sqrt(this.Length2); }
        }

        [Pure]
        public double Length2
        {
            get { return this.Dot(this); }
        }

        [Pure]
        public double LengthL1
        {
            get { return Math.Abs(this.X) + Math.Abs(this.Y); }
        }

        [Pure]
        public double Dot(IVector2 v2)
        {
            ITuple2_Double _v2 = v2.AsTupleDouble();
            return this.X * _v2.X + this.Y * _v2.Y;
        }

        [Pure]
        public double Proj(IVector2 v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public double InvLerp(IVector2 v2, IVector2 vLerp)
        {
            BuffVector2d v12 = new BuffVector2d(v2);
            v12.Sub(this);
            BuffVector2d v1Lerp = new BuffVector2d(vLerp);
            v1Lerp.Sub(this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IVector3

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

        [Pure]
        public double Cross(IVector2 v2)
        {
            ITuple2_Double _v2 = v2.AsTupleDouble();
            return this.X * _v2.Y - this.Y * _v2.X;
        }

        #endregion

        #region inner classes

        /// <summary>
        /// Compares unit vectors using their angle.
        /// <pre><![CDATA[
        /// ^ normal = direccion.PerpLeft
        /// |
        /// | /__
        /// | \  \  incrementa el angulo
        /// |     |
        /// +-----+-----------> direccion
        /// ]]></pre>
        /// </summary>
        public struct AngleComparer : IComparer<Vector2d>, IComparer
        {
            public AngleComparer(Vector2d direccion, double epsilon)
                : this(direccion, direccion.PerpLeft, epsilon)
            {
            }

            public AngleComparer(Vector2d direccion, Vector2d normal, double epsilon)
            {
                Contract.Assert(direccion.IsUnit);
                this.direccion = direccion;
                this.normal = normal;
                this.epsilon = epsilon;
            }

            private readonly Vector2d direccion;
            private readonly Vector2d normal;
            private readonly double epsilon;

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

        /// <summary>
        /// Compares unit vectors using their angle.
        /// <pre><![CDATA[
        /// ^ normal = direccion.PerpLeft
        /// |
        /// | /__
        /// | \  \  incrementa el angulo
        /// |     |
        /// +-----+-----------> direccion
        /// ]]></pre>
        /// </summary>
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