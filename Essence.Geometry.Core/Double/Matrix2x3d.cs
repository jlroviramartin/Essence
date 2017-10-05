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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Essence.Util.Builders;
using Essence.Util.Math;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    [Guid("96251D8A-9628-433A-97E9-69C88F523B8D")]
    public sealed class Matrix2x3d : IEpsilonEquatable<Matrix2x3d>,
                                     IEquatable<Matrix2x3d>,
                                     IFormattable,
                                     ICloneable,
                                     ISerializable
    {
        public const string ITEMS = "Items";
        public const string _M00 = "M00";
        public const string _M01 = "M01";
        public const string _M02 = "M02";
        public const string _M10 = "M10";
        public const string _M11 = "M11";
        public const string _M12 = "M12";

        /// <summary>
        ///     Matriz identidad.
        /// </summary>
        public static Matrix2x3d Identity
        {
            get
            {
                return new Matrix2x3d(1, 0, 0,
                                      0, 1, 0);
            }
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Matrix2x3d()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Matrix2x3d(double m00, double m01, double m02,
                          double m10, double m11, double m12)
        {
            this.Set(m00, m01, m02,
                     m10, m11, m12);
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public Matrix2x3d(double[] items)
        {
            this.Set(items);
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public Matrix2x3d(double[,] items)
        {
            this.Set(items);
        }

        /// <summary>
        ///     Crea un mat 3x3 con los vectores directores <c>vX, vY</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        public Matrix2x3d(Vector2d vX, Vector2d vY)
        {
            this.Set(vX, vY);
        }

        /// <summary>
        ///     Crea un mat 3x3 con los vectores directores <c>vX, vY</c>
        ///     y el origen <c>o</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        /// <param name="o">Origen.</param>
        public Matrix2x3d(Vector2d vX, Vector2d vY, Point2d o)
        {
            this.Set(vX, vY, o);
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Matrix2x3d(Matrix3x3d mat)
        {
            Contract.Assert(mat.M20.EpsilonEquals(0) && mat.M21.EpsilonEquals(0) && mat.M22.EpsilonEquals(1));
            this.Set(mat.M00, mat.M01, mat.M02,
                     mat.M10, mat.M11, mat.M12);
        }

        #region Multiplicacion vectores/puntos

        public static Vector2d operator *(Matrix2x3d mat, Vector2d v)
        {
            return mat.Mul(v);
        }

        public static Point2d operator *(Matrix2x3d mat, Point2d p)
        {
            return mat.Mul(p);
        }

        public static Vector3d operator *(Matrix2x3d mat, Vector3d v)
        {
            return mat.Mul(v);
        }

        public static Point3d operator *(Matrix2x3d mat, Point3d p)
        {
            return mat.Mul(p);
        }

        public IVector2 Mul(IVector2 v)
        {
            ITuple2_Double _v = v.AsTupleDouble();

            return new Vector2d(this.M00 * _v.X + this.M01 * _v.Y,
                                this.M10 * _v.X + this.M11 * _v.Y);
        }

        public void Mul(IVector2 v, IOpVector2 vout)
        {
            ITuple2_Double _v = v.AsTupleDouble();
            IOpTuple2_Double _vout = vout.AsOpTupleDouble();

            _vout.Set(this.M00 * _v.X + this.M01 * _v.Y,
                      this.M10 * _v.X + this.M11 * _v.Y);
        }

        public IPoint2 Mul(IPoint2 p)
        {
            ITuple2_Double _v = p.AsTupleDouble();

            return new Point2d(this.M00 * _v.X + this.M01 * _v.Y,
                               this.M10 * _v.X + this.M11 * _v.Y);
        }

        public void Mul(IPoint2 p, IOpPoint2 pout)
        {
            ITuple2_Double _v = p.AsTupleDouble();
            IOpTuple2_Double _vout = pout.AsOpTupleDouble();

            _vout.Set(this.M00 * _v.X + this.M01 * _v.Y,
                      this.M10 * _v.X + this.M11 * _v.Y);
        }

        public IVector3 Mul(IVector3 v)
        {
            ITuple3_Double _v = v.AsTupleDouble();

            return new Vector3d(this.M00 * _v.X + this.M01 * _v.Y + this.M02 * _v.Z,
                                this.M10 * _v.X + this.M11 * _v.Y + this.M12 * _v.Z,
                                _v.Z);
        }

        public void Mul(IVector3 v, IOpVector3 vout)
        {
            ITuple3_Double _v = v.AsTupleDouble();
            IOpTuple3_Double _vout = vout.AsOpTupleDouble();

            _vout.Set(this.M00 * _v.X + this.M01 * _v.Y + this.M02 * _v.Z,
                      this.M10 * _v.X + this.M11 * _v.Y + this.M12 * _v.Z,
                      _v.Z);
        }

        public IPoint3 Mul(IPoint3 p)
        {
            ITuple3_Double _p = p.AsTupleDouble();

            return new Point3d(this.M00 * _p.X + this.M01 * _p.Y + this.M02 * _p.Z,
                               this.M10 * _p.X + this.M11 * _p.Y + this.M12 * _p.Z,
                               _p.Z);
        }

        public void Mul(IPoint3 p, IOpPoint3 pout)
        {
            ITuple3_Double _p = p.AsTupleDouble();
            IOpTuple3_Double _vout = pout.AsOpTupleDouble();

            _vout.Set(this.M00 * _p.X + this.M01 * _p.Y + this.M02 * _p.Z,
                      this.M10 * _p.X + this.M11 * _p.Y + this.M12 * _p.Z,
                      _p.Z);
        }

        public Vector2d Mul(Vector2d v)
        {
            // NOTE: un vector sigue siendo un vector: w == 0.
            return new Vector2d(this.M00 * v.X + this.M01 * v.Y,
                                this.M10 * v.X + this.M11 * v.Y);
        }

        public Point2d Mul(Point2d p)
        {
            // NOTE: un punto sigue siendo un punto, se normaliza: w == 1.
            return new Point2d(p.X * this.M00 + p.Y * this.M01 + this.M02,
                               p.X * this.M10 + p.Y * this.M11 + this.M12);
        }

        public Vector3d Mul(Vector3d v)
        {
            return new Vector3d(this.M00 * v.X + this.M01 * v.Y + this.M02 * v.Z,
                                this.M10 * v.X + this.M11 * v.Y + this.M12 * v.Z,
                                v.Z);
        }

        public Point3d Mul(Point3d p)
        {
            return new Point3d(this.M00 * p.X + this.M01 * p.Y + this.M02 * p.Z,
                               this.M10 * p.X + this.M11 * p.Y + this.M12 * p.Z,
                               p.Z);
        }

        #endregion

        #region operadores

        /// <summary>
        ///     Operacion multiplicacion: mat1 * mat2.
        /// </summary>
        public static Matrix2x3d operator *(Matrix2x3d mat1, Matrix2x3d mat2)
        {
            Matrix2x3d matrizOut = mat1.Clone();
            matrizOut.Mul(mat2);
            return matrizOut;
        }

        #endregion

        #region casting

        /// <summary>
        ///     Casting a REAL[].
        /// </summary>
        public static explicit operator double[](Matrix2x3d m)
        {
            return new[]
            {
                m.M00, m.M01, m.M02,
                m.M10, m.M11, m.M12,
                0, 0, 1
            };
        }

        /// <summary>
        ///     Casting a REAL[,].
        /// </summary>
        public static explicit operator double[,](Matrix2x3d m)
        {
            return new[,]
            {
                { m.M00, m.M01, m.M02 },
                { m.M10, m.M11, m.M12 },
                { 0, 0, 1 }
            };
        }

        #endregion

        #region propiedades

        /// <summary>
        ///     Filas.
        /// </summary>
        public int Rows
        {
            get { return 3; }
        }

        /// <summary>
        ///     Columnas.
        /// </summary>
        public int Columns
        {
            get { return 3; }
        }

        /// <summary>
        ///     Indica si es valido: ningun componente es NaN ni Infinito.
        /// </summary>
        public bool IsValid
        {
            get { return !this.IsNaN && !this.IsInfinity; }
        }

        /// <summary>
        ///     Indica que algun componente es NaN.
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return double.IsNaN(this.M00) || double.IsNaN(this.M01) || double.IsNaN(this.M02)
                       || double.IsNaN(this.M10) || double.IsNaN(this.M11) || double.IsNaN(this.M12);
            }
        }

        /// <summary>
        ///     Indica que algun componente es infinito.
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(this.M00) || double.IsInfinity(this.M01) || double.IsInfinity(this.M02)
                       || double.IsInfinity(this.M10) || double.IsInfinity(this.M11) || double.IsInfinity(this.M12);
            }
        }

        /// <summary>
        ///     Indica si es identidad.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return this.EpsilonEquals(1, 0, 0,
                                          0, 1, 0, MathUtils.EPSILON);
            }
        }

        /// <summary>
        ///     Indica si es invertible.
        /// </summary>
        public bool IsInvertible
        {
            get
            {
                double determinante = this.Determinant;
                if (double.IsNaN(determinante) || double.IsInfinity(determinante))
                {
                    return false;
                }
                return !determinante.EpsilonZero();
            }
        }

        /// <summary>
        ///     Indica si es cuadrada.
        /// </summary>
        public bool IsSquared
        {
            get { return true; }
        }

        /// <summary>
        ///     Elemento <c>i, j</c>.
        /// </summary>
        /// <param name="i">Fila.</param>
        /// <param name="j">Columna.</param>
        /// <returns>Elemento.</returns>
        public double this[int i, int j]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        switch (j)
                        {
                            case 0:
                                return this.M00;
                            case 1:
                                return this.M01;
                            case 2:
                                return this.M02;
                        }
                        break;

                    case 1:
                        switch (j)
                        {
                            case 0:
                                return this.M10;
                            case 1:
                                return this.M11;
                            case 2:
                                return this.M12;
                        }
                        break;

                    case 2:
                        switch (j)
                        {
                            case 0:
                                return 0;
                            case 1:
                                return 0;
                            case 2:
                                return 1;
                        }
                        break;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                switch (i)
                {
                    case 0:
                        switch (j)
                        {
                            case 0:
                                this.M00 = value;
                                return;
                            case 1:
                                this.M01 = value;
                                return;
                            case 2:
                                this.M02 = value;
                                return;
                        }
                        break;

                    case 1:
                        switch (j)
                        {
                            case 0:
                                this.M10 = value;
                                return;
                            case 1:
                                this.M11 = value;
                                return;
                            case 2:
                                this.M12 = value;
                                return;
                        }
                        break;

                    case 2:
                        switch (j)
                        {
                            case 0:
                                if (!value.EpsilonEquals(0))
                                {
                                    throw new NotSupportedException();
                                }
                                break;
                            case 1:
                                if (!value.EpsilonEquals(0))
                                {
                                    throw new NotSupportedException();
                                }
                                break;
                            case 2:
                                if (!value.EpsilonEquals(1))
                                {
                                    throw new NotSupportedException();
                                }
                                break;
                        }
                        break;
                }
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        ///     Elementos fila 0.
        /// </summary>
        public double M00, M01, M02;

        /// <summary>
        ///     Elementos fila 1.
        /// </summary>
        public double M10, M11, M12;

        #endregion

        #region sets

        /// <summary>
        ///     Establece los elementos.
        /// </summary>
        public void Set(double m00, double m01, double m02,
                        double m10, double m11, double m12)
        {
            this.M00 = m00;
            this.M01 = m01;
            this.M02 = m02;
            this.M10 = m10;
            this.M11 = m11;
            this.M12 = m12;
        }

        /// <summary>
        ///     Establece a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public void Set(double[] items)
        {
            Contract.Requires((items != null)
                              && (items.GetLength(0) == this.Rows * this.Columns));

            this.Set(items[0], items[1], items[2],
                     items[3], items[4], items[5]);
        }

        /// <summary>
        ///     Establece a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public void Set(double[,] items)
        {
            Contract.Requires((items != null) && (items.GetLength(0) == this.Rows)
                              && (items.GetLength(1) == this.Columns));

            this.Set(items[0, 0], items[0, 1], items[0, 2],
                     items[1, 0], items[1, 1], items[1, 2]);
        }

        /// <summary>
        ///     Establece a la identidad.
        /// </summary>
        public void SetIdentity()
        {
            this.Set(1, 0, 0,
                     0, 1, 0);
        }

        /// <summary>
        ///     Establece a <c>mat</c>.
        /// </summary>
        /// <param name="mat">Matriz.</param>
        public void Set(Matrix2x3d mat)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00, mat.M01, mat.M02,
                     mat.M10, mat.M11, mat.M12);
        }

        /// <summary>
        ///     Establece la mat 3x3 con los vectores directores <c>vX, vY</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        public void Set(Vector2d vX, Vector2d vY)
        {
            this.Set(vX.X, vY.X, 0,
                     vX.Y, vY.Y, 0);
        }

        /// <summary>
        ///     Establece la mat 3x3 con los vectores directores <c>vX, vY</c>
        ///     y el punto origen <c>o</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        /// <param name="o">Origen.</param>
        public void Set(Vector2d vX, Vector2d vY, Point2d o)
        {
            this.Set(vX.X, vY.X, o.X,
                     vX.Y, vY.Y, o.Y);
        }

        #endregion

        #region operaciones

        /// <summary>
        ///     Operacion multiplicacion: this = this * mat.
        /// </summary>
        public void Mul(Matrix2x3d mat)
        {
            this.Mul(this, mat);
        }

        /// <summary>
        ///     Operacion multiplicacion: this = mat1 * mat2.
        /// </summary>
        public void Mul(Matrix2x3d mat1, Matrix2x3d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 * mat2.M00 + mat1.M01 * mat2.M10,
                     mat1.M00 * mat2.M01 + mat1.M01 * mat2.M11,
                     mat1.M00 * mat2.M02 + mat1.M01 * mat2.M12 + mat1.M02,
                     mat1.M10 * mat2.M00 + mat1.M11 * mat2.M10,
                     mat1.M10 * mat2.M01 + mat1.M11 * mat2.M11,
                     mat1.M10 * mat2.M02 + mat1.M11 * mat2.M12 + mat1.M12);
        }

        /// <summary>
        ///     Operacion inversion: this = Invertir( this ).
        /// </summary>
        public void Inv()
        {
            this.Inv(this);
        }

        /// <summary>
        ///     Operacion inversion: this = Invertir( mat ).
        /// </summary>
        public void Inv(Matrix2x3d mat)
        {
            double s = mat.Determinant;
            if (s.EpsilonZero())
            {
                throw new Exception("SingularMatrixException");
            }

            s = 1 / s;
            this.Set(mat.M11 * s,
                     (-mat.M01) * s,
                     (mat.M01 * mat.M12 - mat.M02 * mat.M11) * s,
                     (-mat.M10) * s,
                     mat.M00 * s,
                     (mat.M02 * mat.M10 - mat.M00 * mat.M12) * s);
        }

        /// <summary>
        ///     Operacion determinante.
        /// </summary>
        public double Determinant
        {
            get { return (this.M00 * this.M11 - this.M01 * this.M10); }
        }

        /// <summary>
        ///     Crea una copia.
        /// </summary>
        /// <returns>Copia.</returns>
        public Matrix2x3d Clone()
        {
            Matrix2x3d m = (Matrix2x3d)this.MemberwiseClone();
            return m;
        }

        #endregion

        #region privados

        /// <summary>
        ///     Comprueba si son casi iguales.
        /// </summary>
        private bool EpsilonEquals(double m00, double m01, double m02,
                                   double m10, double m11, double m12,
                                   double epsilon)
        {
            return (this.M00.EpsilonEquals(m00, epsilon)
                    && this.M01.EpsilonEquals(m01, epsilon)
                    && this.M02.EpsilonEquals(m02, epsilon)
                    && this.M10.EpsilonEquals(m10, epsilon)
                    && this.M11.EpsilonEquals(m11, epsilon)
                    && this.M12.EpsilonEquals(m12, epsilon));
        }

        #endregion

        #region Object

        public override string ToString()
        {
            return this.ToString("F3", null);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            Matrix2x3d m = obj as Matrix2x3d;
            return ((m != null) && this.Equals(m));
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(this.M00).Append(this.M01).Append(this.M02)
                .Append(this.M10).Append(this.M11).Append(this.M12)
                .GetHashCode();
        }

        #endregion

        #region IEquatable<Matrix2x3d>

        [Pure]
        public bool Equals(Matrix2x3d other)
        {
            if (other == null)
            {
                return false;
            }

            return (other.M00 == this.M00) && (other.M01 == this.M01) && (other.M02 == this.M02)
                   && (other.M10 == this.M10) && (other.M11 == this.M11) && (other.M12 == this.M12);
        }

        #endregion

        #region IEpsilonEquatable<Matrix2x3d>

        public bool EpsilonEquals(Matrix2x3d mat, double epsilon = MathUtils.EPSILON)
        {
            if (mat == null)
            {
                return false;
            }

            return this.EpsilonEquals(mat.M00, mat.M01, mat.M02,
                                      mat.M10, mat.M11, mat.M12,
                                      (double)epsilon);
        }

        #endregion

        #region ICloneable

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region IFormattable

        public string ToString(string format, IFormatProvider provider)
        {
            // Se obtiene la configuracion.
            VectorFormatInfo vfi = null;
            if (provider != null)
            {
                vfi = (VectorFormatInfo)provider.GetFormat(typeof(VectorFormatInfo));
            }
            if (vfi == null)
            {
                vfi = VectorFormatInfo.CurrentInfo;
            }

            return string.Format(provider,
                                 "{0}{0}{3}{1} {4}{1} {5}{2}{1} "
                                 + "{0}{6}{1} {7}{1} {8}{2}{1} "
                                 + "{0}{9}{1} {10}{1} {11}{2}{2}",
                                 vfi.Beg, vfi.Sep, vfi.End,
                                 this.M00.ToString(format, provider),
                                 this.M01.ToString(format, provider),
                                 this.M02.ToString(format, provider),
                                 this.M10.ToString(format, provider),
                                 this.M11.ToString(format, provider),
                                 this.M12.ToString(format, provider),
                                 0,
                                 0,
                                 1);
        }

        #endregion

        #region ISerializable

        public Matrix2x3d(SerializationInfo info, StreamingContext context)
        {
            this.Set((double[])info.GetValue(ITEMS, typeof(double[])));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(ITEMS, (double[])this);
        }

        #endregion
    }
}