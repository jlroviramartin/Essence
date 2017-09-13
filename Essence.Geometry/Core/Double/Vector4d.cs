﻿// Copyright 2017 Jose Luis Rovira Martin
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

        private const double ZERO_TOLERANCE = MathUtils.ZERO_TOLERANCE;
        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Tuple zero.</summary>
        public static Vector4d Zero
        {
            get { return new Vector4d(0, 0, 0, 0); }
        }

        /// <summary>Tuple one.</summary>
        public static Vector4d One
        {
            get { return new Vector4d(1, 1, 1, 1); }
        }

        /// <summary>Tuple with property X = 1 and others = 0.</summary>
        public static Vector4d UX
        {
            get { return new Vector4d(1, 0, 0, 0); }
        }

        /// <summary>Tuple with property Y = 1 and others = 0.</summary>
        public static Vector4d UY
        {
            get { return new Vector4d(0, 1, 0, 0); }
        }

        /// <summary>Tuple with property Z = 1 and others = 0.</summary>
        public static Vector4d UZ
        {
            get { return new Vector4d(0, 0, 1, 0); }
        }

        /// <summary>Tuple with property W = 1 and others = 0.</summary>
        public static Vector4d UW
        {
            get { return new Vector4d(0, 0, 0, 1); }
        }

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
                    throw new Exception("Vector no valido");
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
        ///     Casting a REAL[].
        /// </summary>
        public static explicit operator double[](Vector4d v)
        {
            return new double[] { v.X, v.Y, v.Z, v.W };
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
        ///     Indica si es valido: ningun componente es NaN ni Infinito.
        /// </summary>
        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y) && MathUtils.IsValid(this.Z) && MathUtils.IsValid(this.W); }
        }

        /// <summary>
        ///     Indica que algun componente es NaN.
        /// </summary>
        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z) || double.IsNaN(this.W); }
        }

        /// <summary>
        ///     Indica que algun componente es infinito.
        /// </summary>
        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y) || double.IsInfinity(this.Z) || double.IsInfinity(this.W); }
        }

        /// <summary>
        ///     Indica si es cero.
        /// </summary>
        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0, 0, 0); }
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
        public Vector4d Unit
        {
            get
            {
                double len = this.Length;
                if (len.EpsilonZero())
                    return Zero;
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
            Vector4d v12 = v2.Sub(this);
            return v12.Proy(vLerp.Sub(this));
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
        ///     Parsea la cadena de texto segun los estilos indicados y devuelve una tupla.
        /// </summary>
        /// <param name="s">Cadena de texto a parsear.</param>
        /// <param name="provider">Proveedor de formato.</param>
        /// <param name="vstyle">Estilo de vectores.</param>
        /// <param name="style">Estilo de numeros.</param>
        /// <returns>Resultado.</returns>
        public static Vector4d Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector4d result;
            if (!TryParse(s, out result, provider, vstyle, style))
                throw new Exception();
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
                                    out Vector4d result,
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
            result = new Vector4d(ret[0], ret[1], ret[2], ret[3]);
            return true;
        }

        #endregion

        #region protected

        #endregion

        #region private

        /// <summary>
        ///     Comprueba si son casi iguales.
        /// </summary>
        [Pure]
        private bool EpsilonEquals(double x, double y, double z, double w, double epsilon = ZERO_TOLERANCE)
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
                return false;

            return this.Equals((Vector4d)obj);
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
                hash = prime * hash + this.Z.GetHashCode();
                hash = prime * hash + this.W.GetHashCode();
            }
            return hash;
        }

        #endregion

        #region IEpsilonEquatable<Vector4d>

        [Pure]
        public bool EpsilonEquals(Vector4d other, double epsilon = EPSILON)
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
                    return formatter.Format(format, this, provider);
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
        //REAL LengthCuad { get; }

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
        double IVector.Proy(IVector v2)
        {
            return this.Proy(v2.ToVector4d());
        }

        [Pure]
        IVector IVector.ProyV(IVector v2)
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
        ///     Compara los puntos en funcion a la coordenada indicada (X o Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Vector4d>, IComparer
        {
            public CoordComparer(int coord)
            {
                this.coord = coord;
            }

            private readonly int coord;

            public int Compare(Vector4d v1, Vector4d v2)
            {
                switch (this.coord)
                {
                    case 0:
                        return v1.X.CompareTo(v2.X);
                    case 1:
                        return v1.Y.CompareTo(v2.Y);
                    case 2:
                        return v1.Z.CompareTo(v2.Z);
                    case 3:
                        return v1.W.CompareTo(v2.W);
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
        ///     Comparador lexicografico, primero compara por X y despues por Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Vector4d>, IComparer
        {
            public int Compare(Vector4d v1, Vector4d v2)
            {
                int i;
                i = v1.X.CompareTo(v2.X);
                if (i != 0)
                    return i;
                i = v1.Y.CompareTo(v2.Y);
                if (i != 0)
                    return i;
                i = v1.Z.CompareTo(v2.Z);
                if (i != 0)
                    return i;
                i = v1.W.CompareTo(v2.W);
                return i;
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector4d && o2 is Vector4d);
                return this.Compare((Vector4d)o1, (Vector4d)o2);
            }
        }

        /// <summary>
        ///     Compara los vectores en funcion a su longitud.
        /// </summary>
        public sealed class LengthComparer : IComparer<Vector4d>, IComparer
        {
            public int Compare(Vector4d v1, Vector4d v2)
            {
                return v1.LengthCuad.CompareTo(v2.LengthCuad);
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector4d && o2 is Vector4d);
                return this.Compare((Vector4d)o1, (Vector4d)o2);
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
                        return 0;
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
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    else if (nv2 < 0)
                        return -1; // v2 es mayor.
                    else
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                }
                else if (nv1 < 0)
                {
                    // v1 esta debajo.
                    double nv2 = this.normal.Dot(v2);
                    if (nv2 > 0)
                        return 1; // v1 es mayor.
                    else if (nv2 < 0)
                        return this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    else
                        return 1;
                }
                else // if (nv1 == 0)
                {
                    // this.direccion.Dot(v1); // Es +1 o -1

                    // v1 esta alineado.
                    double nv2 = this.normal.Dot(v2);
                    if (nv2 > 0)
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                    else if (nv2 < 0)
                        return -1;
                    else
                        return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
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