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
    public struct Vector3d : IVector3D,
                             IEpsilonEquatable<Vector3d>,
                             IEquatable<Vector3d>,
                             IFormattable,
                             ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Name of the property Z.</summary>
        public const string _Z = "Z";

        private const double ZERO_TOLERANCE = MathUtils.ZERO_TOLERANCE;
        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Tuple zero.</summary>
        public static readonly Vector3d Zero = new Vector3d(0, 0, 0);

        /// <summary>Tuple one.</summary>
        public static readonly Vector3d One = new Vector3d(1, 1, 1);

        /// <summary>Tuple with property X = 1 and others = 0.</summary>
        public static readonly Vector3d UX = new Vector3d(1, 0, 0);

        /// <summary>Tuple with property Y = 1 and others = 0.</summary>
        public static readonly Vector3d UY = new Vector3d(0, 1, 0);

        /// <summary>Tuple with property Z = 1 and others = 0.</summary>
        public static readonly Vector3d UZ = new Vector3d(0, 0, 1);

        public Vector3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3d(IVector3D v)
        {
            CoordinateSetter3d setter = new CoordinateSetter3d();
            v.GetCoordinates(setter);
            this.X = setter.X;
            this.Y = setter.Y;
            this.Z = setter.Z;
        }

        public Vector3d(IVector v)
        {
            IVector3D v3 = v as IVector3D;
            if (v3 != null)
            {
                CoordinateSetter3d setter = new CoordinateSetter3d();
                v3.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
                this.Z = setter.Z;
            }
            else
            {
                if (v.Dim < 3)
                {
                    throw new Exception("Vector no valido");
                }
                CoordinateSetter3d setter = new CoordinateSetter3d();
                v.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
                this.Z = setter.Z;
            }
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        /// <summary>Property Z.</summary>
        public readonly double Z;

        #region operators

        /// <summary>
        ///     Casting a REAL[].
        /// </summary>
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

        /// <summary>
        ///     Indica si es valido: ningun componente es NaN ni Infinito.
        /// </summary>
        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y) && MathUtils.IsValid(this.Z); }
        }

        /// <summary>
        ///     Indica que algun componente es NaN.
        /// </summary>
        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z); }
        }

        /// <summary>
        ///     Indica que algun componente es infinito.
        /// </summary>
        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y) || double.IsInfinity(this.Z); }
        }

        /// <summary>
        ///     Indica si es cero.
        /// </summary>
        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0, 0); }
        }

        /// <summary>
        ///     Octante:
        ///     <pre><![CDATA[
        ///        ^
        ///    1   |   0
        ///        |
        ///  <-----+-----> z >= 0
        ///        |
        ///    2   |   3
        ///        v
        /// 
        ///        ^
        ///    5   |   4
        ///        |
        ///  <-----+-----> z < 0
        ///        |
        ///    6   |   7
        ///        v
        ///  ]]></pre>
        /// </summary>
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
        ///     Indica si es unitario.
        /// </summary>
        [Pure]
        public bool IsUnit
        {
            get { return this.Length.EpsilonEquals(1); }
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
            get { return Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z); }
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

        /// <summary>
        ///     Calcula el angulo de <c>v2</c> respecto de <c>v1</c>.
        ///     Angulo en radianes entre [0, PI].
        /// </summary>
        public static double EvAngle(Vector3d v1, Vector3d v2)
        {
            double lon1 = v1.LengthCuad;
            double lon2 = v2.LengthCuad;

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

        /// <summary>
        ///     Parsea la cadena de texto segun los estilos indicados y devuelve una tupla.
        /// </summary>
        /// <param name="s">Cadena de texto a parsear.</param>
        /// <param name="provider">Proveedor de formato.</param>
        /// <param name="vstyle">Estilo de vectores.</param>
        /// <param name="style">Estilo de numeros.</param>
        /// <returns>Resultado.</returns>
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

        #region protected

        #endregion

        #region private

        /// <summary>
        ///     Comprueba si son casi iguales.
        /// </summary>
        [Pure]
        private bool EpsilonEquals(double x, double y, double z, double epsilon = ZERO_TOLERANCE)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon) && this.Z.EpsilonEquals(z, epsilon);
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
            if (!(obj is Vector3d))
            {
                return false;
            }

            return this.Equals((Vector3d)obj);
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
            }
            return hash;
        }

        #endregion

        #region IEpsilonEquatable<Vector3d>

        [Pure]
        public bool EpsilonEquals(Vector3d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, other.Z, (double)epsilon);
        }

        #endregion

        #region IEquatable<Vector3d>

        [Pure]
        public bool Equals(Vector3d other)
        {
            return other.X == this.X && other.Y == this.Y && other.Z == this.Z;
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

        #region IVector

        //int Dim { get; }

        void IVector.GetCoordinates(ICoordinateSetter setter)
        {
            setter.SetCoords(this.X, this.Y, this.Z);
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
            return this.Add(v2.ToVector3d());
        }

        [Pure]
        IVector IVector.Sub(IVector v2)
        {
            return this.Sub(v2.ToVector3d());
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
            return this.SimpleMul(v2.ToVector3d());
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
            return this.Lerp(v2.ToVector3d(), alpha);
        }

        [Pure]
        double IVector.InvLerp(IVector v2, IVector vLerp)
        {
            return this.InvLerp(v2.ToVector3d(), vLerp.ToVector3d());
        }

        [Pure]
        IVector IVector.Lineal(IVector v2, double alpha, double beta)
        {
            return this.Lineal(v2.ToVector3d(), alpha, beta);
        }

        [Pure]
        double IVector.Dot(IVector v2)
        {
            return this.Dot(v2.ToVector3d());
        }

        [Pure]
        double IVector.Proy(IVector v2)

        {
            return this.Proy(v2.ToVector3d());
        }

        [Pure]
        IVector IVector.ProyV(IVector v2)
        {
            return this.ProyV(v2.ToVector3d());
        }

        #endregion

        #region IVector3D

        void IVector3D.GetCoordinates(ICoordinateSetter3D setter)
        {
            setter.SetCoords(this.X, this.Y, this.Z);
        }

        [Pure]
        IVector3D IVector3D.Cross(IVector3D v2)
        {
            return this.Cross(v2.ToVector3d());
        }

        [Pure]
        double IVector3D.TripleProduct(IVector3D v2, IVector3D v3)
        {
            return this.TripleProduct(v2.ToVector3d(), v3.ToVector3d());
        }

        #endregion

        #region IEpsilonEquatable<IVector>

        [Pure]
        bool IEpsilonEquatable<IVector>.EpsilonEquals(IVector other, double epsilon)
        {
            return this.EpsilonEquals(other.ToVector3d(), epsilon);
        }

        #endregion

        #region inner classes

        /// <summary>
        ///     Compara los puntos en funcion a la coordenada indicada (X o Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Vector3d>, IComparer
        {
            public CoordComparer(int coord)
            {
                this.coord = coord;
            }

            private readonly int coord;

            public int Compare(Vector3d v1, Vector3d v2)
            {
                switch (this.coord)
                {
                    case 0:
                        return v1.X.CompareTo(v2.X);
                    case 1:
                        return v1.Y.CompareTo(v2.Y);
                    case 2:
                        return v1.Z.CompareTo(v2.Z);
                }
                throw new IndexOutOfRangeException();
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector3d && o2 is Vector3d);
                return this.Compare((Vector3d)o1, (Vector3d)o2);
            }
        }

        /// <summary>
        ///     Comparador lexicografico, primero compara por X y despues por Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Vector3d>, IComparer
        {
            public int Compare(Vector3d v1, Vector3d v2)
            {
                int i;
                i = v1.X.CompareTo(v2.X);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Y.CompareTo(v2.Y);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Z.CompareTo(v2.Z);
                return i;
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector3d && o2 is Vector3d);
                return this.Compare((Vector3d)o1, (Vector3d)o2);
            }
        }

        /// <summary>
        ///     Compara los vectores en funcion a su longitud.
        /// </summary>
        public sealed class LengthComparer : IComparer<Vector3d>, IComparer
        {
            public int Compare(Vector3d v1, Vector3d v2)
            {
                return v1.LengthCuad.CompareTo(v2.LengthCuad);
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Vector3d && o2 is Vector3d);
                return this.Compare((Vector3d)o1, (Vector3d)o2);
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
        public struct AngleComparer : IComparer<Vector3d>, IComparer
        {
            public AngleComparer(Vector3d direccion, Vector3d normal)
            {
                Contract.Assert(direccion.IsUnit);
                this.direccion = direccion;
                this.normal = normal;
            }

            private readonly Vector3d direccion;
            private readonly Vector3d normal;

            public int Compare(Vector3d v1, Vector3d v2)
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
                Contract.Requires(o1 is Vector3d && o2 is Vector3d);
                return this.Compare((Vector3d)o1, (Vector3d)o2);
            }
        }

        #endregion
    }
}