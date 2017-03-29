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

using Essence.Util.Math;
using Essence.Util.Math.Double;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using REAL = System.Double;
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

        private const double ZERO_TOLERANCE = MathUtils.ZERO_TOLERANCE;
        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Tuple zero.</summary>
        public static Vector2d Zero
        {
            get { return new Vector2d(0, 0); }
        }

        /// <summary>Tuple one.</summary>
        public static Vector2d One
        {
            get { return new Vector2d(1, 1); }
        }

        /// <summary>Tuple with property X = 1 and others = 0.</summary>
        public static Vector2d UX
        {
            get { return new Vector2d(1, 0); }
        }

        /// <summary>Tuple with property Y = 1 and others = 0.</summary>
        public static Vector2d UY
        {
            get { return new Vector2d(0, 1); }
        }

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
            this.X = v.X.ToDouble(null);
            this.Y = v.Y.ToDouble(null);
        }

        public Vector2d(IVector v)
        {
            IVector2D v2 = v as IVector2D;
            if (v2 != null)
            {
                this.X = v2.X.ToDouble(null);
                this.Y = v2.Y.ToDouble(null);
            }
            else
            {
                if (v.Dim < 2)
                {
                    throw new Exception("Vector no valido");
                }
                this.X = v[0].ToDouble(null);
                this.Y = v[1].ToDouble(null);
            }
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        #region operators

        /// <summary>
        ///     Casting a REAL[].
        /// </summary>
        public static explicit operator double[](Vector2d v)
        {
            return new double[] { v.X, v.Y };
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
        ///     Indica si es valido: ningun componente es NaN ni Infinito.
        /// </summary>
        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y); }
        }

        /// <summary>
        ///     Indica que algun componente es NaN.
        /// </summary>
        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y); }
        }

        /// <summary>
        ///     Indica que algun componente es infinito.
        /// </summary>
        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y); }
        }

        /// <summary>
        ///     Indica si es cero.
        /// </summary>
        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0); }
        }

        /// <summary>
        ///     Cuadrante en sentido CCW:
        ///     <pre><![CDATA[
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

        [Pure]
        public double Cross(Vector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        /// <summary>
        ///     Indica si es unitario.
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
            get { return (double)Math.Sqrt(this.LengthCuad); }
        }

        [Pure]
        public double LengthCuad
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
            Vector2d v12 = v2.Sub(this);
            return v12.Proy(vLerp.Sub(this));
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
        ///     Calcula el angulo de <c>this</c> respecto del eje X.
        ///     <pre><![CDATA[
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

        [Pure]
        public double AngleTo(Vector2d other)
        {
            //return (this.Angle(b) - this.Angle(a));
            // http://stackoverflow.com/questions/2150050/finding-signed-angle-between-vectors
            return SysMath.Atan2(this.X * other.Y - this.Y * other.X, this.X * other.X + this.Y * other.Y);
        }

        /// <summary>
        ///     Vector perpendicular a la derecha (Perp): (y, -x).
        /// </summary>
        [Pure]
        public Vector2d PerpRight
        {
            get { return new Vector2d(this.Y, -this.X); }
        }

        /// <summary>
        ///     Vector perpendicular a la izquierda: (-y, x).
        /// </summary>
        [Pure]
        public Vector2d PerpLeft
        {
            get { return new Vector2d(-this.Y, this.X); }
        }

        /// <summary>
        ///     Vector unitario perpendicular a la derecha (UnitPerp): PerpRight / Length.
        /// </summary>
        [Pure]
        public Vector2d UnitPerpRight
        {
            get { return this.PerpRight.Unit; }
        }

        /// <summary>
        ///     Vector unitario perpendicular a la izquierda:PerpLeft / Length.
        /// </summary>
        [Pure]
        public Vector2d UnitPerpLeft
        {
            get { return this.PerpLeft.Unit; }
        }

        /// <summary>
        ///     Producto escalar por el vector perpendicular a <c>v</c> (dotPerp).
        ///     returns DotPerp((x,y),(V.x,V.y)) = Dot((x,y),PerpRight(v)) = x*V.y - y*V.x
        ///     NOTA: igual al producto vectorial (cross).
        /// </summary>
        [Pure]
        public double DotPerpRight(Vector2d v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        /// <summary>
        ///     Rota el vector.
        /// </summary>
        /// <param name="v2">Vector.</param>
        /// <param name="rad">Angulo en radianes.</param>
        public static Vector2d Rot(Vector2d v2, double rad)
        {
            double s = (double)Math.Sin(rad);
            double c = (double)Math.Cos(rad);
            return new Vector2d(v2.X * c - v2.Y * s, v2.X * s + v2.Y * c);
        }

        /// <summary>
        ///     Calcula el angulo de <c>v2</c> respecto de <c>v1</c>.
        ///     Angulo en radianes entre [-PI, PI].
        ///     Es positivo si el giro es sentido horario [0, PI].
        ///     Es negativo si el giro es sentido anti-horario [-PI, 0].
        ///     <pre><![CDATA[
        ///               __
        ///              _/| v2
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
        ///              --| v1
        /// ]]></pre>
        /// </summary>
        /// <param name="v1">Vector.</param>
        /// <param name="v2">Vector.</param>
        /// <returns>Angulo.</returns>
        public static double EvAngle(Vector2d v1, Vector2d v2)
        {
            return (v2.Angle - v1.Angle);
        }

        #region parse

        /// <summary>
        ///     Parsea la cadena de texto segun los estilos indicados y devuelve una tupla.
        /// </summary>
        /// <param name="s">Cadena de texto a parsear.</param>
        /// <param name="provider">Proveedor de formato.</param>
        /// <param name="vstyle">Estilo de vectores.</param>
        /// <param name="style">Estilo de numeros.</param>
        /// <returns>Resultado.</returns>
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

        /// <summary>
        ///     Parsea la cadena de texto segun los estilos indicados y devuelve una tupla.
        /// </summary>
        /// <param name="s">Cadena de texto a parsear.</param>
        /// <param name="provider">Proveedor de formato.</param>
        /// <param name="vstyle">Estilo de vectores.</param>
        /// <param name="style">Estilo de numeros.</param>
        /// <param name="result">Resultado.</param>
        /// <returns>Indica si lo ha parseado correctamente.</returns>
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

        /// <summary>
        ///     Comprueba si son casi iguales.
        /// </summary>
        [Pure]
        private bool EpsilonEquals(double x, double y, double epsilon = ZERO_TOLERANCE)
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
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + this.X.GetHashCode();
                hash = prime * hash + this.Y.GetHashCode();
            }
            return hash;
        }

        #endregion

        #region IEpsilonEquatable<Vector2d>

        [Pure]
        public bool EpsilonEquals(Vector2d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, (double)epsilon);
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

        [Pure]
        IConvertible IVector.this[int i]
        {
            get { return this[i]; }
        }

        [Pure]
        IVector IVector.Unit
        {
            get { return this.Unit; }
        }

        //[Pure]
        //REAL Length { get; }

        //[Pure]
        //REAL LengthCuad { get; }

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
        double IVector.Proy(IVector v2)

        {
            return this.Proy(v2.ToVector2d());
        }

        [Pure]
        IVector IVector.ProyV(IVector v2)
        {
            return this.ProyV(v2.ToVector2d());
        }

        #endregion

        #region IVector2D

        [Pure]
        IConvertible IVector2D.X
        {
            get { return this.X; }
        }

        [Pure]
        IConvertible IVector2D.Y
        {
            get { return this.Y; }
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
        ///     Compara los puntos en funcion a la coordenada indicada (X o Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Vector2d>, IComparer
        {
            public CoordComparer(int coord)
            {
                this.coord = coord;
            }

            private readonly int coord;

            public int Compare(Vector2d v1, Vector2d v2)
            {
                switch (this.coord)
                {
                    case 0:
                        return v1.X.CompareTo(v2.X);
                    case 1:
                        return v1.Y.CompareTo(v2.Y);
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
        ///     Comparador lexicografico, primero compara por X y despues por Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Vector2d>, IComparer
        {
            public int Compare(Vector2d v1, Vector2d v2)
            {
                int i;
                i = v1.X.CompareTo(v2.X);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Y.CompareTo(v2.Y);
                return i;
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector2d && o2 is Vector2d);
                return this.Compare((Vector2d)o1, (Vector2d)o2);
            }
        }

        /// <summary>
        ///     Compara los vectores en funcion a su longitud.
        /// </summary>
        public sealed class LengthComparer : IComparer<Vector2d>, IComparer
        {
            public int Compare(Vector2d v1, Vector2d v2)
            {
                return v1.LengthCuad.CompareTo(v2.LengthCuad);
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector2d && o2 is Vector2d);
                return this.Compare((Vector2d)o1, (Vector2d)o2);
            }
        }

        /// <summary>
        ///     Compara vectores normalizados segun su angulo.
        ///     <![CDATA[
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
                        // v2 esta encima.
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                    else if (nv2 < 0)
                    {
                        // v2 esta debajo.
                        return -1; // v2 es mayor.
                    }
                    else
                    {
                        // Dot(this.direccion, v2) // Es +1 o -1

                        // v2 esta alineado.
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                }
                else if (nv1 < 0)
                {
                    // v1 esta debajo.
                    double nv2 = this.normal.Dot(v2);
                    if (nv2 > 0)
                    {
                        // v2 esta encima.
                        return 1; // v1 es mayor.
                    }
                    else if (nv2 < 0)
                    {
                        // v2 esta debajo o alineado.
                        return this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                    else
                    {
                        // Dot(this.direccion, v2) // Es +1 o -1

                        // v2 esta alineado.
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
                        // v2 esta encima.
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    }
                    else if (nv2 < 0)
                    {
                        // v2 esta debajo.
                        return -1;
                    }
                    else
                    {
                        // Dot(this.direccion, v2); // Es +1 o -1

                        // v2 esta alineado.
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