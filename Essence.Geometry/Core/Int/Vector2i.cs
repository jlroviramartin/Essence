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

using Essence.Util.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using INT = System.Int32;
using REAL = System.Double;

namespace Essence.Geometry.Core.Int
{
    public struct Vector2i : IVector2D,
                             IEquatable<Vector2i>,
                             IFormattable,
                             ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Tuple zero.</summary>
        public static Vector2i Zero
        {
            get { return new Vector2i(0, 0); }
        }

        /// <summary>Tuple one.</summary>
        public static Vector2i One
        {
            get { return new Vector2i(1, 1); }
        }

        /// <summary>Tuple with property X = 1 and others = 0.</summary>
        public static Vector2i UX
        {
            get { return new Vector2i(1, 0); }
        }

        /// <summary>Tuple with property Y = 1 and others = 0.</summary>
        public static Vector2i UY
        {
            get { return new Vector2i(0, 1); }
        }

        public Vector2i(INT x, INT y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2i(IVector2D v)
        {
            this.X = v.X.ToInt32(null);
            this.Y = v.Y.ToInt32(null);
        }

        public Vector2i(IVector v)
        {
            IVector2D v2 = v as IVector2D;
            if (v2 != null)
            {
                this.X = v2.X.ToInt32(null);
                this.Y = v2.Y.ToInt32(null);
            }
            else
            {
                if (v.Dim < 2)
                {
                    throw new Exception("Vector no valido");
                }
                this.X = v[0].ToInt32(null);
                this.Y = v[1].ToInt32(null);
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
        public static explicit operator INT[](Vector2i v)
        {
            return new INT[] { v.X, v.Y };
        }

        public static Vector2i operator -(Vector2i v1)
        {
            return v1.Neg();
        }

        public static Vector2i operator +(Vector2i v1, Vector2i v2)
        {
            return v1.Add(v2);
        }

        public static Vector2i operator -(Vector2i v1, Vector2i v2)
        {
            return v1.Sub(v2);
        }

        public static Vector2i operator *(Vector2i v, INT c)
        {
            return v.Mul(c);
        }

        public static Vector2i operator *(INT c, Vector2i v)
        {
            return v.Mul(c);
        }

        public static Vector2i operator /(Vector2i v, INT c)
        {
            return v.Div(c);
        }

        public static implicit operator Point2i(Vector2i p)
        {
            return new Point2i(p.X, p.Y);
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
        public INT Cross(Vector2i v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        [Pure]
        public REAL Length
        {
            get { return (REAL)Math.Sqrt(this.LengthCuad); }
        }

        [Pure]
        public REAL LengthCuad
        {
            get { return this.Dot(this); }
        }

        [Pure]
        public REAL LengthL1
        {
            get { return Math.Abs(this.X) + Math.Abs(this.Y); }
        }

        [Pure]
        public Vector2i Add(Vector2i v2)
        {
            return new Vector2i(this.X + v2.X, this.Y + v2.Y);
        }

        [Pure]
        public Vector2i Sub(Vector2i v2)
        {
            return new Vector2i(this.X - v2.X, this.Y - v2.Y);
        }

        [Pure]
        public Vector2i Mul(INT c)
        {
            return new Vector2i(this.X * c, this.Y * c);
        }

        [Pure]
        public Vector2i Div(INT c)
        {
            return new Vector2i(this.X / c, this.Y / c);
        }

        [Pure]
        public Vector2i SimpleMul(Vector2i v2)
        {
            return new Vector2i(this.X * v2.X, this.Y * v2.Y);
        }

        [Pure]
        public Vector2i Neg()
        {
            return new Vector2i(-this.X, -this.Y);
        }

        [Pure]
        public Vector2i Abs()
        {
            return new Vector2i(Math.Abs(this.X), Math.Abs(this.Y));
        }

        [Pure]
        public Vector2i Lerp(Vector2i v2, REAL alpha)
        {
            return this.Lineal(v2, 1 - alpha, alpha);
        }

        [Pure]
        public REAL InvLerp(Vector2i v2, Vector2i vLerp)
        {
            Vector2i v12 = v2.Sub(this);
            return v12.Proy(vLerp.Sub(this));
        }

        [Pure]
        public Vector2i Lineal(Vector2i v2, REAL alpha, REAL beta)
        {
            return new Vector2i((INT)(alpha * this.X + beta * v2.X), (INT)(alpha * this.Y + beta * v2.Y));
        }

        [Pure]
        public INT Dot(Vector2i v2)
        {
            return this.X * v2.X + this.Y * v2.Y;
        }

        [Pure]
        public REAL Proy(Vector2i v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public Vector2i ProyV(Vector2i v2)
        {
            return this.Mul(this.Proy(v2));
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
        public static Vector2i Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector2i result;
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
                                    out Vector2i result,
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
            result = new Vector2i(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

        [Pure]
        private Vector2i Unit
        {
            get
            {
                REAL len = this.Length;
                if (Essence.Util.Math.Double.MathUtils.EpsilonZero(len))
                {
                    return Zero;
                }
                return this.Div(len);
            }
        }

        [Pure]
        private Vector2i Mul(REAL c)
        {
            return new Vector2i((INT)(this.X * c), (INT)(this.Y * c));
        }

        [Pure]
        private Vector2i Div(REAL c)
        {
            return new Vector2i((INT)(this.X / c), (INT)(this.Y / c));
        }

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
            if (!(obj is Vector2i))
            {
                return false;
            }

            return this.Equals((Vector2i)obj);
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

        #region IEquatable<Vector2i>

        [Pure]
        public bool Equals(Vector2i other)
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

        public Vector2i(SerializationInfo info, StreamingContext context)
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
            if (v2 is Vector2i)
            {
                return this.Add((Vector2i)v2);
            }
            return this.Add(new Vector2i(v2));
        }

        [Pure]
        IVector IVector.Sub(IVector v2)
        {
            if (v2 is Vector2i)
            {
                return this.Add((Vector2i)v2);
            }
            return this.Sub(new Vector2i(v2));
        }

        [Pure]
        IVector IVector.Mul(REAL c)
        {
            return this.Mul(c);
        }

        [Pure]
        IVector IVector.Div(REAL c)
        {
            return this.Div(c);
        }

        [Pure]
        IVector IVector.SimpleMul(IVector v2)
        {
            if (v2 is Vector2i)
            {
                return this.SimpleMul((Vector2i)v2);
            }
            return this.SimpleMul(new Vector2i(v2));
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
        IVector IVector.Lerp(IVector v2, REAL alpha)
        {
            if (v2 is Vector2i)
            {
                return this.Lerp((Vector2i)v2, alpha);
            }
            return this.Lerp(new Vector2i(v2), alpha);
        }

        [Pure]
        REAL IVector.InvLerp(IVector v2, IVector vLerp)
        {
            if (v2 is Vector2i)
            {
                if (vLerp is Vector2i)
                {
                    return this.InvLerp((Vector2i)v2, (Vector2i)vLerp);
                }
                else
                {
                    return this.InvLerp((Vector2i)v2, new Vector2i(vLerp));
                }
            }
            return this.InvLerp(new Vector2i(v2), new Vector2i(vLerp));
        }

        [Pure]
        IVector IVector.Lineal(IVector v2, REAL alpha, REAL beta)
        {
            if (v2 is Vector2i)
            {
                return this.Lineal((Vector2i)v2, alpha, beta);
            }
            return this.Lineal(new Vector2i(v2), alpha, beta);
        }

        [Pure]
        REAL IVector.Dot(IVector v2)
        {
            if (v2 is Vector2i)
            {
                return this.Dot((Vector2i)v2);
            }
            return this.Dot(new Vector2i(v2));
        }

        [Pure]
        REAL IVector.Proy(IVector v2)

        {
            if (v2 is Vector2i)
            {
                return this.Proy((Vector2i)v2);
            }
            return this.Proy(new Vector2i(v2));
        }

        [Pure]
        IVector IVector.ProyV(IVector v2)
        {
            if (v2 is Vector2i)
            {
                return this.ProyV((Vector2i)v2);
            }
            return this.ProyV(new Vector2i(v2));
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
        REAL IVector2D.Cross(IVector2D v2)
        {
            if (v2 is Vector2i)
            {
                return this.Cross((Vector2i)v2);
            }
            return this.Cross(new Vector2i(v2));
        }

        #endregion

        #region IEpsilonEquatable<IVector>

        [Pure]
        bool IEpsilonEquatable<IVector>.EpsilonEquals(IVector other, REAL epsilon)
        {
            if (other is Vector2i)
            {
                return this.Equals((Vector2i)other);
            }
            return this.Equals(new Vector2i(other));
        }

        #endregion

        #region inner classes

        /// <summary>
        ///     Compara los puntos en funcion a la coordenada indicada (X o Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Vector2i>, IComparer
        {
            public CoordComparer(int coord)
            {
                this.coord = coord;
            }

            private readonly int coord;

            public int Compare(Vector2i v1, Vector2i v2)
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
                Contract.Requires(o1 is Vector2i && o2 is Vector2i);
                return this.Compare((Vector2i)o1, (Vector2i)o2);
            }
        }

        /// <summary>
        ///     Comparador lexicografico, primero compara por X y despues por Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Vector2i>, IComparer
        {
            public int Compare(Vector2i v1, Vector2i v2)
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
                Contract.Requires(o1 is Vector2i && o2 is Vector2i);
                return this.Compare((Vector2i)o1, (Vector2i)o2);
            }
        }

        #endregion
    }
}