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
    public struct Point2d : IPoint2D,
                            IEpsilonEquatable<Point2d>,
                            IEquatable<Point2d>,
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
        public static readonly Point2d Zero = new Point2d(0, 0);

        /// <summary>Tuple one.</summary>
        public static readonly Point2d One = new Point2d(1, 1);

        /// <summary>Tuple with property X = 1 and others = 0.</summary>
        public static readonly Point2d UX = new Point2d(1, 0);

        /// <static readonly>Tuple with property Y = 1 and others = 0.</summary>
        public static Point2d UY = new Point2d(0, 1);

        public Point2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point2d(IPoint2D p)
        {
            CoordinateSetter2d setter = new CoordinateSetter2d();
            p.GetCoordinates(setter);
            this.X = setter.X;
            this.Y = setter.Y;
        }

        public Point2d(IPoint p)
        {
            IPoint2D p2 = p as IPoint2D;
            if (p2 != null)
            {
                CoordinateSetter2d setter = new CoordinateSetter2d();
                p2.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
            }
            else
            {
                if (p.Dim < 2)
                {
                    throw new Exception("Punto no valido");
                }
                CoordinateSetter2d setter = new CoordinateSetter2d();
                p.GetCoordinates(setter);
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
        ///     Casting a REAL[].
        /// </summary>
        public static explicit operator double[](Point2d v)
        {
            return new double[] { v.X, v.Y };
        }

        public static Point2d operator +(Point2d p, Vector2d v)
        {
            return p.Add(v);
        }

        public static Point2d operator -(Point2d p, Vector2d v)
        {
            return p.Sub(v);
        }

        public static Vector2d operator -(Point2d p1, Point2d p2)
        {
            return p1.Sub(p2);
        }

        public static implicit operator Vector2d(Point2d p)
        {
            return new Vector2d(p.X, p.Y);
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
        public Point2d Add(Vector2d v)
        {
            return new Point2d(this.X + v.X, this.Y + v.Y);
        }

        [Pure]
        public Point2d Sub(Vector2d v)
        {
            return new Point2d(this.X - v.X, this.Y - v.Y);
        }

        [Pure]
        public Vector2d Sub(Point2d p2)
        {
            return new Vector2d(this.X - p2.X, this.Y - p2.Y);
        }

        [Pure]
        public double Distance2To(Point2d p2)
        {
            return (MathUtils.Cuad(p2.X - this.X) + MathUtils.Cuad(p2.Y - this.Y));
        }

        [Pure]
        public double DistanceTo(Point2d p2)
        {
            return (double)Math.Sqrt(this.Distance2To(p2));
        }

        [Pure]
        public Point2d Lerp(Point2d p2, double alpha)
        {
            return this.Lineal(p2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Point2d p2, Point2d pLerp)
        {
            Vector2d v12 = p2.Sub(this);
            return v12.Proy(pLerp.Sub(this));
        }

        [Pure]
        public Point2d Lineal(Point2d p2, double alpha, double beta)
        {
            return new Point2d(alpha * this.X + beta * p2.X, alpha * this.Y + beta * p2.Y);
        }

        /// <summary>
        ///     Calcula el angulo de <c>p2 - 0</c> respecto a <c>p1 - 0</c>.
        ///     Angulo en radianes entre [-PI, PI].
        ///     Es positivo si el giro es sentido horario [0, PI].
        ///     Es negativo si el giro es sentido anti-horario [-PI, 0].
        ///     <pre><![CDATA[
        ///               __
        ///              _/| p2
        ///            _/
        ///          _/
        ///        _/ __
        ///      _/   |\ angulo +
        ///    _/       |
        ///   +--------------------> p1
        /// origen      |
        ///       \_  |/  angulo -
        ///         \_|--
        ///           \_
        ///             \_
        ///               \|
        ///              --| p2
        /// ]]></pre>
        /// </summary>
        /// <param name="o">Origen.</param>
        /// <param name="p1">Punto base.</param>
        /// <param name="p2">Punto.</param>
        /// <returns>Angulo.</returns>
        public static double EvAngle(Point2d o, Point2d p1, Point2d p2)
        {
            return (p1 - o).AngleTo(p2 - o);
        }

        /// <summary>
        /// This method test if the {@code p} point is at the left, on or at the right of the {@code a, b} line
        /// {@link http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm}.
        /// <ul>
        /// <li>+1 if the {@code p} point is at the left of the line</li>
        /// <li>0 if the {@code p} point is on the line</li>
        /// <li>-1 if the {@code p} point is at the right of the line</li>
        /// </ul>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static int IsLeft(Point2d a, Point2d b, Point2d p, double epsilon)
        {
            return (int)WhichSide(a, b, p, epsilon);
        }

        /// <summary>
        /// This method test if the {@code p} point is at the left, on or at the right of the {@code a, b} line
        /// {@link http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm}.
        /// <ul>
        /// <li>+1 if the {@code p} point is at the left of the line</li>
        /// <li>0 if the {@code p} point is on the line</li>
        /// <li>-1 if the {@code p} point is at the right of the line</li>
        /// </ul>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int IsLeft(Point2d a, Point2d b, Point2d p)
        {
            return IsLeft(a, b, p, EPSILON);
        }

        /// <summary>
        /// This method test if the {@code p} point is at the left, on or at the right of the {@code a, b} line
        /// {@link http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm}.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static LineSide WhichSide(Point2d a, Point2d b, Point2d p, double epsilon)
        {
            double v = ((b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X));
            return (MathUtils.EpsilonEquals(v, 0, epsilon) ? LineSide.Middle : (v < 0 ? LineSide.Right : LineSide.Left));
        }

        /// <summary>
        /// This method test if the {@code p} point is at the left, on or at the right of the {@code a, b} line
        /// {@link http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm}.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static LineSide WhichSide(Point2d a, Point2d b, Point2d p)
        {
            return WhichSide(a, b, p, EPSILON);
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
        public static Point2d Parse(string s,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Point2d result;
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
                                    out Point2d result,
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
            result = new Point2d(ret[0], ret[1]);
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
            if (!(obj is Point2d))
            {
                return false;
            }

            return this.Equals((Point2d)obj);
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

        #region IEpsilonEquatable<Point2d>

        [Pure]
        public bool EpsilonEquals(Point2d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, (double)epsilon);
        }

        #endregion

        #region IEquatable<Point2d>

        [Pure]
        public bool Equals(Point2d other)
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

        public Point2d(SerializationInfo info, StreamingContext context)
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

        #region IPoint

        //[Pure]
        //int Dim { get; }

        void IPoint.GetCoordinates(ICoordinateSetter setter)
        {
            setter.SetCoords(this.X, this.Y);
        }

        [Pure]
        IPoint IPoint.Add(IVector v)
        {
            return this.Add(v.ToVector2d());
        }

        [Pure]
        IPoint IPoint.Sub(IVector v)
        {
            return this.Sub(v.ToVector2d());
        }

        [Pure]
        IVector IPoint.Sub(IPoint p2)
        {
            return this.Sub(p2.ToPoint2d());
        }

        [Pure]
        IPoint IPoint.Lerp(IPoint p2, double alpha)
        {
            return this.Lerp(p2.ToPoint2d(), alpha);
        }

        [Pure]
        double IPoint.InvLerp(IPoint p2, IPoint pLerp)
        {
            return this.InvLerp(p2.ToPoint2d(), pLerp.ToPoint2d());
        }

        [Pure]
        IPoint IPoint.Lineal(IPoint p2, double alpha, double beta)
        {
            return this.Lineal(p2.ToPoint2d(), alpha, beta);
        }

        #endregion

        #region IPoint2D

        void IPoint2D.GetCoordinates(ICoordinateSetter2D setter)
        {
            setter.SetCoords(this.X, this.Y);
        }

        #endregion

        #region IEpsilonEquatable<IPoint>

        [Pure]
        bool IEpsilonEquatable<IPoint>.EpsilonEquals(IPoint other, double epsilon)
        {
            return this.EpsilonEquals(other.ToPoint2d(), epsilon);
        }

        #endregion

        #region inner classes

        /// <summary>
        ///     Compara los puntos en funcion a la coordenada indicada (X o Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Point2d>, IComparer
        {
            public CoordComparer(int coord)
            {
                this.coord = coord;
            }

            private readonly int coord;

            public int Compare(Point2d v1, Point2d v2)
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
                Contract.Requires(o1 is Point2d && o2 is Point2d);
                return this.Compare((Point2d)o1, (Point2d)o2);
            }
        }

        /// <summary>
        ///     Comparador lexicografico, primero compara por X y despues por Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Point2d>, IComparer
        {
            public int Compare(Point2d v1, Point2d v2)
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
                Contract.Requires(o1 is Point2d && o2 is Point2d);
                return this.Compare((Point2d)o1, (Point2d)o2);
            }
        }

        #endregion
    }
}