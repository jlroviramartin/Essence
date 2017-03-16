#region License

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

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using Essence.Util.Math;
using Essence.Util.Math.Int;
using INT = System.Int32;
using REAL = System.Double;

namespace Essence.Geometry.Core.Int
{
    public struct Point2i : IPoint2D,
                            IEquatable<Point2i>,
                            IFormattable,
                            ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Tuple zero.</summary>
        public static Point2i Zero
        {
            get { return new Point2i(0, 0); }
        }

        /// <summary>Tuple one.</summary>
        public static Point2i One
        {
            get { return new Point2i(1, 1); }
        }

        /// <summary>Tuple with property X = 1 and others = 0.</summary>
        public static Point2i UX
        {
            get { return new Point2i(1, 0); }
        }

        /// <summary>Tuple with property Y = 1 and others = 0.</summary>
        public static Point2i UY
        {
            get { return new Point2i(0, 1); }
        }

        public Point2i(INT x, INT y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point2i(IPoint2D p)
        {
            this.X = p.X.ToInt32(null);
            this.Y = p.Y.ToInt32(null);
        }

        public Point2i(IPoint p)
        {
            IPoint2D p2 = p as IPoint2D;
            if (p2 != null)
            {
                this.X = p2.X.ToInt32(null);
                this.Y = p2.Y.ToInt32(null);
            }
            else
            {
                if (p.Dim < 2)
                {
                    throw new Exception("Punto no valido");
                }
                this.X = p[0].ToInt32(null);
                this.Y = p[1].ToInt32(null);
            }
        }

        /// <summary>Property X.</summary>
        public readonly INT X;

        /// <summary>Property Y.</summary>
        public readonly INT Y;

        #region operators

        /// <summary>
        ///     Casting a REAL[].
        /// </summary>
        public static explicit operator INT[](Point2i v)
        {
            return new INT[] { v.X, v.Y };
        }

        public static Point2i operator +(Point2i p, Vector2i v)
        {
            return p.Add(v);
        }

        public static Point2i operator -(Point2i p, Vector2i v)
        {
            return p.Sub(v);
        }

        public static Vector2i operator -(Point2i p1, Point2i p2)
        {
            return p1.Sub(p2);
        }

        public static implicit operator Vector2i(Point2i p)
        {
            return new Vector2i(p.X, p.Y);
        }

        #endregion

        [Pure]
        public int Dim
        {
            get { return 2; }
        }

        [Pure]
        public INT this[int i]
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
        ///     Indica si es cero.
        /// </summary>
        [Pure]
        public bool IsZero
        {
            get { return this.Equals(0, 0); }
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
        public Point2i Add(Vector2i v)
        {
            return new Point2i(this.X + v.X, this.Y + v.Y);
        }

        [Pure]
        public Point2i Sub(Vector2i v)
        {
            return new Point2i(this.X - v.X, this.Y - v.Y);
        }

        [Pure]
        public Vector2i Sub(Point2i p2)
        {
            return new Vector2i(this.X - p2.X, this.Y - p2.Y);
        }

        [Pure]
        public REAL Distance2To(Point2i p2)
        {
            return MathUtils.Cuad(p2.X - this.X) + MathUtils.Cuad(p2.Y - this.Y);
        }

        [Pure]
        public REAL DistanceTo(Point2i p2)
        {
            return (REAL)Math.Sqrt(this.Distance2To(p2));
        }

        [Pure]
        public Point2i Lerp(Point2i p2, REAL alpha)
        {
            return this.Lineal(p2, 1 - alpha, alpha);
        }

        [Pure]
        public REAL InvLerp(Point2i p2, Point2i pLerp)
        {
            Vector2i v12 = p2.Sub(this);
            return v12.Proy(pLerp.Sub(this));
        }

        [Pure]
        public Point2i Lineal(Point2i p2, REAL alpha, REAL beta)
        {
            return new Point2i((INT)(alpha * this.X + beta * p2.X), (INT)(alpha * this.Y + beta * p2.Y));
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
        public static Point2i Parse(string s,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Point2i result;
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
                                    out Point2i result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            INT[] ret;
            if (!VectorUtils.TryParse(s, 2, out ret, INT.TryParse, provider, vstyle, style))
            {
                result = Zero;
                return false;
            }
            result = new Point2i(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

        /// <summary>
        ///     Comprueba si son iguales.
        /// </summary>
        [Pure]
        private bool Equals(INT x, INT y)
        {
            return this.X == x && this.Y == y;
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
            if (!(obj is Point2i))
            {
                return false;
            }

            return this.Equals((Point2i)obj);
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

        #region IEquatable

        [Pure]
        public bool Equals(Point2i other)
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

            return VectorUtils.ToString(provider, format, (INT[])this);
        }

        #endregion

        #region ISerializable

        public Point2i(SerializationInfo info, StreamingContext context)
        {
            this.X = info.GetInt32(_X);
            this.Y = info.GetInt32(_Y);
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

        [Pure]
        IConvertible IPoint.this[int i]
        {
            get { return this[i]; }
        }

        [Pure]
        IPoint IPoint.Add(IVector v)
        {
            if (v is Vector2i)
            {
                return this.Add((Vector2i)v);
            }
            return this.Add(new Vector2i(v));
        }

        [Pure]
        IPoint IPoint.Sub(IVector v)
        {
            if (v is Vector2i)
            {
                return this.Sub((Vector2i)v);
            }
            return this.Sub(new Vector2i(v));
        }

        [Pure]
        IVector IPoint.Sub(IPoint p2)
        {
            if (p2 is Point2i)
            {
                return this.Sub((Point2i)p2);
            }
            return this.Sub(new Point2i(p2));
        }

        [Pure]
        IPoint IPoint.Lerp(IPoint p2, REAL alpha)
        {
            if (p2 is Point2i)
            {
                return this.Lerp((Point2i)p2, alpha);
            }
            return this.Lerp(new Point2i(p2), alpha);
        }

        [Pure]
        REAL IPoint.InvLerp(IPoint p2, IPoint pLerp)
        {
            if (p2 is Point2i)
            {
                if (pLerp is Point2i)
                {
                    return this.InvLerp((Point2i)p2, (Point2i)pLerp);
                }
                return this.InvLerp((Point2i)p2, new Point2i(pLerp));
            }
            return this.InvLerp(new Point2i(p2), new Point2i(pLerp));
        }

        [Pure]
        IPoint IPoint.Lineal(IPoint p2, REAL alpha, REAL beta)
        {
            if (p2 is Point2i)
            {
                return this.Lineal((Point2i)p2, alpha, beta);
            }
            return this.Lineal(new Point2i(p2), alpha, beta);
        }

        #endregion

        #region IPoint2D

        [Pure]
        IConvertible IPoint2D.X
        {
            get { return this.X; }
        }

        [Pure]
        IConvertible IPoint2D.Y
        {
            get { return this.Y; }
        }

        #endregion

        #region IEpsilonEquatable<IPoint>

        [Pure]
        bool IEpsilonEquatable<IPoint>.EpsilonEquals(IPoint other, REAL epsilon)
        {
            if (other is Point2i)
            {
                return this.Equals((Point2i)other);
            }
            return this.Equals(new Point2i(other));
        }

        #endregion

        #region inner classes

        /// <summary>
        ///     Compara los puntos en funcion a la coordenada indicada (X o Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Point2i>, IComparer
        {
            public CoordComparer(int coord)
            {
                this.coord = coord;
            }

            private readonly int coord;

            public int Compare(Point2i v1, Point2i v2)
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
                Contract.Requires(o1 is Point2i && o2 is Point2i);
                return this.Compare((Point2i)o1, (Point2i)o2);
            }
        }

        /// <summary>
        ///     Comparador lexicografico, primero compara por X y despues por Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Point2i>, IComparer
        {
            public int Compare(Point2i v1, Point2i v2)
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
                Contract.Requires(o1 is Point2i && o2 is Point2i);
                return this.Compare((Point2i)o1, (Point2i)o2);
            }
        }

        #endregion
    }
}