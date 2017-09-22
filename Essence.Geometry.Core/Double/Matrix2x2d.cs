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
    [Guid("58F8993C-5A83-436A-8A62-2F6B36B06FF2")]
    public sealed class Matrix2x2d : IEpsilonEquatable<Matrix2x2d>,
                                     IEquatable<Matrix2x2d>,
                                     IFormattable,
                                     ICloneable,
                                     ISerializable

    {
        public const string ITEMS = "Items";
        public const string _M00 = "M00";
        public const string _M01 = "M01";
        public const string _M10 = "M10";
        public const string _M11 = "M11";

        /// <summary>Identity matrix.</summary>
        public static Matrix2x2d Identity
        {
            get
            {
                return new Matrix2x2d(1, 0,
                                      0, 1);
            }
        }

        /// <summary>Zero matrix.</summary>
        public static Matrix2x2d Zero
        {
            get { return new Matrix2x2d(); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Matrix2x2d()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Matrix2x2d(double m00, double m01,
                          double m10, double m11)
        {
            this.Set(m00, m01,
                     m10, m11);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">Items.</param>
        public Matrix2x2d(double[] items)
        {
            this.Set(items);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">Items.</param>
        public Matrix2x2d(double[,] items)
        {
            this.Set(items);
        }

        /// <summary>
        /// Builds a 2x2 matrix with the director vectors <c>vX, vY</c>.
        /// </summary>
        /// <param name="vX">X vector.</param>
        /// <param name="vY">Y vector.</param>
        public Matrix2x2d(Vector2d vX, Vector2d vY)
        {
            this.Set(vX, vY);
        }

        #region Multiplicacion y Premultiplicacion vectores/puntos

        public static Vector2d operator *(Matrix2x2d mat, Vector2d v)
        {
            return mat.Mul(v);
        }

        public static Point2d operator *(Matrix2x2d mat, Point2d p)
        {
            return mat.Mul(p);
        }

        public Vector2d Mul(Vector2d v)
        {
            return new Vector2d(this.M00 * v.X + this.M01 * v.Y,
                                this.M10 * v.X + this.M11 * v.Y);
        }

        public Point2d Mul(Point2d p)
        {
            return new Point2d(p.X * this.M00 + p.Y * this.M01,
                               p.X * this.M10 + p.Y * this.M11);
        }

        public static Vector2d operator *(Vector2d v, Matrix2x2d mat)
        {
            return mat.PreMul(v);
        }

        public static Point2d operator *(Point2d p, Matrix2x2d mat)
        {
            return mat.PreMul(p);
        }

        public Vector2d PreMul(Vector2d v)
        {
            return new Vector2d(v.X * this.M00 + v.Y * this.M10,
                                v.X * this.M01 + v.Y * this.M11);
        }

        public Point2d PreMul(Point2d p)
        {
            return new Point2d(p.X * this.M00 + p.Y * this.M10,
                               p.X * this.M01 + p.Y * this.M11);
        }

        #endregion

        #region operadores

        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="mat1">Matrix 1.</param>
        /// <param name="mat2">Matrix 2.</param>
        /// <returns>mat1 + mat2</returns>
        public static Matrix2x2d operator +(Matrix2x2d mat1, Matrix2x2d mat2)
        {
            Matrix2x2d matrizOut = mat1.Clone();
            matrizOut.Add(mat2);
            return matrizOut;
        }

        /// <summary>
        /// Subs two matrices.
        /// </summary>
        /// <param name="mat1">Matrix 1.</param>
        /// <param name="mat2">Matrix 2.</param>
        /// <returns>mat1 - mat2</returns>
        public static Matrix2x2d operator -(Matrix2x2d mat1, Matrix2x2d mat2)
        {
            Matrix2x2d matrizOut = mat1.Clone();
            matrizOut.Sub(mat2);
            return matrizOut;
        }

        /// <summary>
        /// Negates a matrix.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <returns>- mat</returns>
        public static Matrix2x2d operator -(Matrix2x2d mat)
        {
            Matrix2x2d matrizOut = mat.Clone();
            matrizOut.Neg();
            return matrizOut;
        }

        /// <summary>
        /// Multiplies a matrix with a value.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <param name="v">Value.</param>
        /// <returns>mat * v</returns>
        public static Matrix2x2d operator *(Matrix2x2d mat, double v)
        {
            Matrix2x2d matrizOut = mat.Clone();
            matrizOut.Mul(v);
            return matrizOut;
        }

        /// <summary>
        /// Multiplies a matrix with a value.
        /// </summary>
        /// <param name="v">Value.</param>
        /// <param name="mat">Matrix.</param>
        /// <returns>v * mat</returns>
        public static Matrix2x2d operator *(double v, Matrix2x2d mat)
        {
            Matrix2x2d matrizOut = mat.Clone();
            matrizOut.Mul(v);
            return matrizOut;
        }

        /// <summary>
        /// Divides a matrix by a value.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <param name="v">Value.</param>
        /// <returns>mat / v</returns>
        public static Matrix2x2d operator /(Matrix2x2d mat, double v)
        {
            Matrix2x2d matrizOut = mat.Clone();
            matrizOut.Div(v);
            return matrizOut;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="mat1">Matrix 1.</param>
        /// <param name="mat2">Matrix 2.</param>
        /// <returns>mat1 * mat2</returns>
        public static Matrix2x2d operator *(Matrix2x2d mat1, Matrix2x2d mat2)
        {
            Matrix2x2d matrizOut = mat1.Clone();
            matrizOut.Mul(mat2);
            return matrizOut;
        }

        #endregion

        #region casting

        /// <summary>
        /// Casting to an array.
        /// </summary>
        public static explicit operator double[](Matrix2x2d m)
        {
            return new[]
            {
                m.M00, m.M01,
                m.M10, m.M11,
            };
        }

        /// <summary>
        /// Casting to an array.
        /// </summary>
        public static explicit operator double[,](Matrix2x2d m)
        {
            return new[,]
            {
                { m.M00, m.M01 },
                { m.M10, m.M11 },
            };
        }

        #endregion

        #region propiedades

        /// <summary>Gets the rows.</summary>
        public int Rows
        {
            get { return 2; }
        }

        /// <summary>Gets the columns.</summary>
        public int Columns
        {
            get { return 2; }
        }

        /// <summary>
        /// Tests if <code>this</code> matrix is valid (not any item is NaN or infinity).
        /// </summary>
        public bool IsValid
        {
            get { return !this.IsNaN && !this.IsInfinity; }
        }

        /// <summary>
        /// Tests if <code>this</code> matrix is NaN (any item is NaN).
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return double.IsNaN(this.M00) || double.IsNaN(this.M01)
                       || double.IsNaN(this.M10) || double.IsNaN(this.M11);
            }
        }

        /// <summary>
        ///     Indica que algun componente es infinito.
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(this.M00) || double.IsInfinity(this.M01)
                       || double.IsInfinity(this.M10) || double.IsInfinity(this.M11);
            }
        }

        /// <summary>
        ///     Indica si es cero.
        /// </summary>
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0, 0, 0); }
        }

        /// <summary>
        ///     Indica si es identidad.
        /// </summary>
        public bool IsIdentity
        {
            get { return this.EpsilonEquals(1, 0, 0, 1); }
        }

        /// <summary>
        ///     Indica si es invertible.
        /// </summary>
        public bool IsInvertible
        {
            get { return !this.Determinant.EpsilonZero(); }
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
                    {
                        switch (j)
                        {
                            case 0:
                            {
                                return this.M00;
                            }
                            case 1:
                            {
                                return this.M01;
                            }
                        }
                        break;
                    }
                    case 1:
                    {
                        switch (j)
                        {
                            case 0:
                            {
                                return this.M10;
                            }
                            case 1:
                            {
                                return this.M11;
                            }
                        }
                        break;
                    }
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                switch (i)
                {
                    case 0:
                    {
                        switch (j)
                        {
                            case 0:
                            {
                                this.M00 = value;
                                return;
                            }
                            case 1:
                            {
                                this.M01 = value;
                                return;
                            }
                        }
                        break;
                    }
                    case 1:
                    {
                        switch (j)
                        {
                            case 0:
                            {
                                this.M10 = value;
                                return;
                            }
                            case 1:
                            {
                                this.M11 = value;
                                return;
                            }
                        }
                        break;
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }

        public double M00, M01;
        public double M10, M11;

        #endregion

        #region sets

        /// <summary>
        ///     Establece los elementos.
        /// </summary>
        public void Set(double m00, double m01,
                        double m10, double m11)
        {
            this.M00 = m00;
            this.M01 = m01;
            this.M10 = m10;
            this.M11 = m11;
        }

        /// <summary>
        ///     Establece a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public void Set(double[] items)
        {
            Contract.Requires((items != null)
                              && (items.GetLength(0) == this.Rows * this.Columns));

            this.Set(items[0], items[1],
                     items[2], items[3]);
        }

        /// <summary>
        ///     Establece a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public void Set(double[,] items)
        {
            Contract.Requires((items != null) && (items.GetLength(0) == this.Rows)
                              && (items.GetLength(1) == this.Columns));

            this.Set(items[0, 0], items[0, 1],
                     items[1, 0], items[1, 1]);
        }

        /// <summary>
        ///     Establece a cero.
        /// </summary>
        public void SetZero()
        {
            this.Set(0, 0,
                     0, 0);
        }

        /// <summary>
        ///     Establece a la identidad.
        /// </summary>
        public void SetIdentity()
        {
            this.Set(1, 0,
                     0, 1);
        }

        /// <summary>
        ///     Establece a <c>mat</c>.
        /// </summary>
        /// <param name="mat">Matriz.</param>
        public void Set(Matrix2x2d mat)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00, mat.M01,
                     mat.M10, mat.M11);
        }

        /// <summary>
        ///     Establece la mat 2x2 con los vectores directores <c>vX, vY</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        public void Set(Vector2d vX, Vector2d vY)
        {
            this.Set(vX.X, vY.X,
                     vX.Y, vY.Y);
        }

        #endregion

        #region operaciones

        /// <summary>
        ///     Operacion suma: this = this + mat.
        /// </summary>
        public void Add(Matrix2x2d mat)
        {
            this.Add(this, mat);
        }

        /// <summary>
        ///     Operacion suma: this = mat1 + mat2.
        /// </summary>
        public void Add(Matrix2x2d mat1, Matrix2x2d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 + mat2.M00, mat1.M01 + mat2.M01,
                     mat1.M10 + mat2.M10, mat1.M11 + mat2.M11);
        }

        /// <summary>
        ///     Operacion resta: this = this - mat.
        /// </summary>
        public void Sub(Matrix2x2d mat)
        {
            this.Sub(this, mat);
        }

        /// <summary>
        ///     Operacion resta: this = mat1 - mat2.
        /// </summary>
        public void Sub(Matrix2x2d mat1, Matrix2x2d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 - mat2.M00, mat1.M01 - mat2.M01,
                     mat1.M10 - mat2.M10, mat1.M11 - mat2.M11);
        }

        /// <summary>
        ///     Operacion multiplicacion: this = this * valor.
        /// </summary>
        public void Mul(double v)
        {
            this.Mul(this, v);
        }

        /// <summary>
        ///     Operacion multiplicacion: this = mat * valor.
        /// </summary>
        public void Mul(Matrix2x2d mat, double v)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00 * v, mat.M01 * v,
                     mat.M10 * v, mat.M11 * v);
        }

        /// <summary>
        ///     Operacion division: this = this / valor.
        /// </summary>
        public void Div(double v)
        {
            this.Div(this, v);
        }

        /// <summary>
        ///     Operacion division: this = mat / valor.
        /// </summary>
        public void Div(Matrix2x2d mat, double v)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00 / v, mat.M01 / v,
                     mat.M10 / v, mat.M11 / v);
        }

        /// <summary>
        ///     Operacion cambio signo: this = -this.
        /// </summary>
        public void Neg()
        {
            this.Neg(this);
        }

        /// <summary>
        ///     Operacion cambio signo: this = -mat.
        /// </summary>
        public void Neg(Matrix2x2d mat)
        {
            Contract.Requires(mat != null);

            this.Set(-mat.M00, -mat.M01,
                     -mat.M10, -mat.M11);
        }

        /// <summary>
        ///     Operacion valor absoluto: this = Absoluto ( this ).
        /// </summary>
        public void Abs()
        {
            this.Abs(this);
        }

        /// <summary>
        ///     Operacion valor absoluto: this = Absoluto ( mat ).
        /// </summary>
        public void Abs(Matrix2x2d mat)
        {
            Contract.Requires(mat != null);

            this.Set(Math.Abs(mat.M00), Math.Abs(mat.M01),
                     Math.Abs(mat.M10), Math.Abs(mat.M11));
        }

        /// <summary>
        ///     Operacion multiplicacion: this = this * mat.
        /// </summary>
        public void Mul(Matrix2x2d mat)
        {
            this.Mul(this, mat);
        }

        /// <summary>
        ///     Operacion multiplicacion: this = mat1 * mat2.
        /// </summary>
        public void Mul(Matrix2x2d mat1, Matrix2x2d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 * mat2.M00 + mat1.M01 * mat2.M10,
                     mat1.M00 * mat2.M01 + mat1.M01 * mat2.M11,
                     mat1.M10 * mat2.M00 + mat1.M11 * mat2.M10,
                     mat1.M10 * mat2.M01 + mat1.M11 * mat2.M11);
        }

        /// <summary>
        ///     Operacion trasposicion: this = Trasponer( this ).
        /// </summary>
        public void Transpose()
        {
            this.Transpose(this);
        }

        /// <summary>
        ///     Operacion trasposicion: this = Trasponer( mat ).
        /// </summary>
        public void Transpose(Matrix2x2d mat)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00, mat.M10,
                     mat.M01, mat.M11);
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
        public void Inv(Matrix2x2d mat)
        {
            double s = mat.Determinant;
            if (s.EpsilonZero())
            {
                throw new Exception("SingularMatrixException");
            }

            s = 1 / s;
            this.Set(mat.M11 * s, -mat.M01 * s,
                     -mat.M10 * s, mat.M00 * s);
        }

        /// <summary>
        ///     Operacion determinante.
        /// </summary>
        public double Determinant
        {
            get
            {
                return (this.M00 * this.M11
                        - this.M01 * this.M10);
            }
        }

        /// <summary>
        ///     Crea una copia.
        /// </summary>
        /// <returns>Copia.</returns>
        public Matrix2x2d Clone()
        {
            Matrix2x2d m = (Matrix2x2d)this.MemberwiseClone();
            return m;
        }

        #endregion

        #region private

        /// <summary>
        ///     Comprueba si son casi iguales.
        /// </summary>
        private bool EpsilonEquals(double m00, double m01,
                                   double m10, double m11,
                                   double epsilon = MathUtils.EPSILON)
        {
            return (this.M00.EpsilonEquals(m00, epsilon)
                    && this.M01.EpsilonEquals(m01, epsilon)
                    && this.M10.EpsilonEquals(m10, epsilon)
                    && this.M11.EpsilonEquals(m11, epsilon));
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

            Matrix2x2d m = obj as Matrix2x2d;
            return ((m != null) && this.Equals(m));
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(this.M00).Append(this.M01)
                .Append(this.M10).Append(this.M11)
                .GetHashCode();
        }

        #endregion

        #region IEquatable<Matrix2x2d>

        [Pure]
        public bool Equals(Matrix2x2d other)
        {
            if (other == null)
            {
                return false;
            }

            return (other.M00 == this.M00) && (other.M01 == this.M01)
                   && (other.M10 == this.M10) && (other.M11 == this.M11);
        }

        #endregion

        #region IEpsilonEquatable<Matrix2x2d>

        public bool EpsilonEquals(Matrix2x2d mat, double epsilon = MathUtils.EPSILON)
        {
            if (mat == null)
            {
                return false;
            }

            return this.EpsilonEquals(mat.M00, mat.M01,
                                      mat.M10, mat.M11,
                                      epsilon);
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
                                 "{0}{0}{3}{1} {4}{2}{1} "
                                 + "{0}{5}{1} {6}{2}{2}",
                                 vfi.Beg, vfi.Sep, vfi.End,
                                 this.M00.ToString(format, provider),
                                 this.M01.ToString(format, provider),
                                 this.M10.ToString(format, provider),
                                 this.M11.ToString(format, provider));
        }

        #endregion

        #region ISerializable

        public Matrix2x2d(SerializationInfo info, StreamingContext context)
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