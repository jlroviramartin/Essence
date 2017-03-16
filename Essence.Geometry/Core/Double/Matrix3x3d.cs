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
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Essence.Util.Builders;
using Essence.Util.Math;
using Essence.Util.Math.Double;
using REAL = System.Double;

namespace Essence.Geometry.Core.Double
{
    [Guid("96251D8A-9628-433A-97E9-69C88F523B8D")]
    public sealed class Matrix3x3d : IEpsilonEquatable<Matrix3x3d>,
                                     IEquatable<Matrix3x3d>,
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
        public const string _M20 = "M20";
        public const string _M21 = "M21";
        public const string _M22 = "M22";

        /// <summary>
        ///     Matriz identidad.
        /// </summary>
        public static Matrix3x3d Identity
        {
            get
            {
                return new Matrix3x3d(1, 0, 0,
                                      0, 1, 0,
                                      0, 0, 1);
            }
        }

        /// <summary>
        ///     Matriz cero.
        /// </summary>
        public static Matrix3x3d Zero
        {
            get { return new Matrix3x3d(); }
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Matrix3x3d()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Matrix3x3d(REAL m00, REAL m01, REAL m02,
                          REAL m10, REAL m11, REAL m12,
                          REAL m20, REAL m21, REAL m22)
        {
            this.Set(m00, m01, m02,
                     m10, m11, m12,
                     m20, m21, m22);
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public Matrix3x3d(REAL[] items)
        {
            this.Set(items);
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public Matrix3x3d(REAL[,] items)
        {
            this.Set(items);
        }

        /// <summary>
        ///     Crea un mat 3x3 con los vectores directores <c>vX, vY</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        public Matrix3x3d(Vector2d vX, Vector2d vY)
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
        public Matrix3x3d(Vector2d vX, Vector2d vY, Point2d o)
        {
            this.Set(vX, vY, o);
        }

        /// <summary>
        ///     Crea un mat 3x3 con los vectores directores <c>vX, vY y vZ</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        /// <param name="vZ">Vector Z.</param>
        public Matrix3x3d(Vector3d vX, Vector3d vY, Vector3d vZ)
        {
            this.Set(vX, vY, vZ);
        }

        #region Multiplicacion vectores/puntos

        public static Vector2d operator *(Matrix3x3d mat, Vector2d v)
        {
            return mat.Mul(v);
        }

        public static Point2d operator *(Matrix3x3d mat, Point2d p)
        {
            return mat.Mul(p);
        }

        public static Vector3d operator *(Matrix3x3d mat, Vector3d v)
        {
            return mat.Mul(v);
        }

        public static Point3d operator *(Matrix3x3d mat, Point3d p)
        {
            return mat.Mul(p);
        }

        public Vector2d Mul(Vector2d v)
        {
            // NOTE: un vector sigue siendo un vector: w == 0.
            Contract.Requires((this.M20 * v.X + this.M21 * v.Y).EpsilonEquals(0));
            return new Vector2d(this.M00 * v.X + this.M01 * v.Y,
                                this.M10 * v.X + this.M11 * v.Y);
        }

        public Point2d Mul(Point2d p)
        {
            // NOTE: un punto sigue siendo un punto, se normaliza: w == 1.
            REAL d = this.M20 * p.X + this.M21 * p.Y + this.M22;
            return new Point2d((p.X * this.M00 + p.Y * this.M01 + this.M02) / d,
                               (p.X * this.M10 + p.Y * this.M11 + this.M12) / d);
        }

        public Vector3d Mul(Vector3d v)
        {
            return new Vector3d(this.M00 * v.X + this.M01 * v.Y + this.M02 * v.Z,
                                this.M10 * v.X + this.M11 * v.Y + this.M12 * v.Z,
                                this.M20 * v.X + this.M21 * v.Y + this.M22 * v.Z);
        }

        public Point3d Mul(Point3d p)
        {
            return new Point3d(this.M00 * p.X + this.M01 * p.Y + this.M02 * p.Z,
                               this.M10 * p.X + this.M11 * p.Y + this.M12 * p.Z,
                               this.M20 * p.X + this.M21 * p.Y + this.M22 * p.Z);
        }

        #endregion

        #region Premultiplicacion vectores/puntos

        public static Vector2d operator *(Vector2d v, Matrix3x3d mat)
        {
            return mat.PreMul(v);
        }

        public static Point2d operator *(Point2d p, Matrix3x3d mat)
        {
            return mat.PreMul(p);
        }

        public Vector2d PreMul(Vector2d v)
        {
            REAL d = v.X * this.M02 + v.Y * this.M12;
            return new Vector2d((v.X * this.M00 + v.Y * this.M10) / d,
                                (v.X * this.M01 + v.Y * this.M11) / d);
        }

        public Point2d PreMul(Point2d p)
        {
            REAL d = p.X * this.M02 + p.Y * this.M12 + this.M22;
            return new Point2d((p.X * this.M00 + p.Y * this.M10 + this.M20) / d,
                               (p.X * this.M01 + p.Y * this.M11 + this.M21) / d);
        }

        public Vector3d PreMul(Vector3d v)
        {
            return new Vector3d(v.X * this.M00 + v.Y * this.M10 + v.Z * this.M20,
                                v.X * this.M01 + v.Y * this.M11 + v.Z * this.M21,
                                v.X * this.M02 + v.Y * this.M12 + v.Z * this.M22);
        }

        public Point3d PreMul(Point3d p)
        {
            return new Point3d(p.X * this.M00 + p.Y * this.M10 + p.Z * this.M20,
                               p.X * this.M01 + p.Y * this.M11 + p.Z * this.M21,
                               p.X * this.M02 + p.Y * this.M12 + p.Z * this.M22);
        }

        #endregion

        #region operadores

        /// <summary>
        ///     Operacion sumar: mat1 + mat2.
        /// </summary>
        public static Matrix3x3d operator +(Matrix3x3d mat1, Matrix3x3d mat2)
        {
            Matrix3x3d matrizOut = mat1.Clone();
            matrizOut.Add(mat2);
            return matrizOut;
        }

        /// <summary>
        ///     Operacion restar: mat1 - mat2.
        /// </summary>
        public static Matrix3x3d operator -(Matrix3x3d mat1, Matrix3x3d mat2)
        {
            Matrix3x3d matrizOut = mat1.Clone();
            matrizOut.Add(mat2);
            return matrizOut;
        }

        /// <summary>
        ///     Operacion restar: mat1 - mat2.
        /// </summary>
        public static Matrix3x3d operator -(Matrix3x3d mat)
        {
            Matrix3x3d matrizOut = mat.Clone();
            matrizOut.Neg();
            return matrizOut;
        }

        /// <summary>
        ///     Operacion multiplicacion: mat * valor.
        /// </summary>
        public static Matrix3x3d operator *(Matrix3x3d mat, REAL v)
        {
            Matrix3x3d matrizOut = mat.Clone();
            matrizOut.Mul(v);
            return matrizOut;
        }

        /// <summary>
        ///     Operacion multiplicacion: valor * mat.
        /// </summary>
        public static Matrix3x3d operator *(REAL v, Matrix3x3d mat)
        {
            Matrix3x3d matrizOut = mat.Clone();
            matrizOut.Mul(v);
            return matrizOut;
        }

        /// <summary>
        ///     Operacion division: mat / valor.
        /// </summary>
        public static Matrix3x3d operator /(Matrix3x3d mat, REAL v)
        {
            Matrix3x3d matrizOut = mat.Clone();
            matrizOut.Div(v);
            return matrizOut;
        }

        /// <summary>
        ///     Operacion multiplicacion: mat1 * mat2.
        /// </summary>
        public static Matrix3x3d operator *(Matrix3x3d mat1, Matrix3x3d mat2)
        {
            Matrix3x3d matrizOut = mat1.Clone();
            matrizOut.Mul(mat2);
            return matrizOut;
        }

        #endregion

        #region casting

        /// <summary>
        ///     Casting a REAL[].
        /// </summary>
        public static explicit operator REAL[](Matrix3x3d m)
        {
            return new[]
            {
                m.M00, m.M01, m.M02,
                m.M10, m.M11, m.M12,
                m.M20, m.M21, m.M22
            };
        }

        /// <summary>
        ///     Casting a REAL[,].
        /// </summary>
        public static explicit operator REAL[,](Matrix3x3d m)
        {
            return new[,]
            {
                { m.M00, m.M01, m.M02 },
                { m.M10, m.M11, m.M12 },
                { m.M20, m.M21, m.M22 }
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
                return REAL.IsNaN(this.M00) || REAL.IsNaN(this.M01) || REAL.IsNaN(this.M02)
                       || REAL.IsNaN(this.M10) || REAL.IsNaN(this.M11) || REAL.IsNaN(this.M12)
                       || REAL.IsNaN(this.M20) || REAL.IsNaN(this.M21) || REAL.IsNaN(this.M22);
            }
        }

        /// <summary>
        ///     Indica que algun componente es infinito.
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return REAL.IsInfinity(this.M00) || REAL.IsInfinity(this.M01) || REAL.IsInfinity(this.M02)
                       || REAL.IsInfinity(this.M10) || REAL.IsInfinity(this.M11) || REAL.IsInfinity(this.M12)
                       || REAL.IsInfinity(this.M20) || REAL.IsInfinity(this.M21) || REAL.IsInfinity(this.M22);
            }
        }

        /// <summary>
        ///     Indica si es cero.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this.EpsilonEquals(0, 0, 0,
                                          0, 0, 0,
                                          0, 0, 0, MathUtils.ZERO_TOLERANCE);
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
                                          0, 1, 0,
                                          0, 0, 1, MathUtils.ZERO_TOLERANCE);
            }
        }

        /// <summary>
        ///     Indica si es invertible.
        /// </summary>
        public bool IsInvertible
        {
            get
            {
                REAL determinante = this.Determinant;
                if (REAL.IsNaN(determinante) || REAL.IsInfinity(determinante))
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
        public REAL this[int i, int j]
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
                                return this.M20;
                            case 1:
                                return this.M21;
                            case 2:
                                return this.M22;
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
                                this.M20 = value;
                                return;
                            case 1:
                                this.M21 = value;
                                return;
                            case 2:
                                this.M22 = value;
                                return;
                        }
                        break;
                }
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        ///     Elementos fila 0.
        /// </summary>
        public REAL M00, M01, M02;

        /// <summary>
        ///     Elementos fila 1.
        /// </summary>
        public REAL M10, M11, M12;

        /// <summary>
        ///     Elementos fila 2.
        /// </summary>
        public REAL M20, M21, M22;

        #endregion

        #region sets

        /// <summary>
        ///     Establece los elementos.
        /// </summary>
        public void Set(REAL m00, REAL m01, REAL m02,
                        REAL m10, REAL m11, REAL m12,
                        REAL m20, REAL m21, REAL m22)
        {
            this.M00 = m00;
            this.M01 = m01;
            this.M02 = m02;
            this.M10 = m10;
            this.M11 = m11;
            this.M12 = m12;
            this.M20 = m20;
            this.M21 = m21;
            this.M22 = m22;
        }

        /// <summary>
        ///     Establece a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public void Set(REAL[] items)
        {
            Contract.Requires((items != null)
                              && (items.GetLength(0) == this.Rows * this.Columns));

            this.Set(items[0], items[1], items[2],
                     items[3], items[4], items[5],
                     items[6], items[7], items[8]);
        }

        /// <summary>
        ///     Establece a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public void Set(REAL[,] items)
        {
            Contract.Requires((items != null) && (items.GetLength(0) == this.Rows)
                              && (items.GetLength(1) == this.Columns));

            this.Set(items[0, 0], items[0, 1], items[0, 2],
                     items[1, 0], items[1, 1], items[1, 2],
                     items[2, 0], items[2, 1], items[2, 2]);
        }

        /// <summary>
        ///     Establece a cero.
        /// </summary>
        public void SetZero()
        {
            this.Set(0, 0, 0,
                     0, 0, 0,
                     0, 0, 0);
        }

        /// <summary>
        ///     Establece a la identidad.
        /// </summary>
        public void SetIdentity()
        {
            this.Set(1, 0, 0,
                     0, 1, 0,
                     0, 0, 1);
        }

        /// <summary>
        ///     Establece a <c>mat</c>.
        /// </summary>
        /// <param name="mat">Matriz.</param>
        public void Set(Matrix3x3d mat)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00, mat.M01, mat.M02,
                     mat.M10, mat.M11, mat.M12,
                     mat.M20, mat.M21, mat.M22);
        }

        /// <summary>
        ///     Establece la mat 3x3 con los vectores directores <c>vX, vY</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        public void Set(Vector2d vX, Vector2d vY)
        {
            this.Set(vX.X, vY.X, 0,
                     vX.Y, vY.Y, 0,
                     0, 0, 1);
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
                     vX.Y, vY.Y, o.Y,
                     0, 0, 1);
        }

        /// <summary>
        ///     Establece la mat 3x3 con los vectores directores <c>vX, vY</c>
        ///     y el punto origen <c>o</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        /// <param name="vZ">Vector Z.</param>
        public void Set(Vector3d vX, Vector3d vY, Vector3d vZ)
        {
            this.Set(vX.X, vY.X, vZ.X,
                     vX.Y, vY.Y, vZ.Y,
                     vX.Z, vY.Z, vZ.Z);
        }

        #endregion

        #region operaciones

        /// <summary>
        ///     Operacion suma: this = this + mat.
        /// </summary>
        public void Add(Matrix3x3d mat)
        {
            this.Add(this, mat);
        }

        /// <summary>
        ///     Operacion suma: this = mat1 + mat2.
        /// </summary>
        public void Add(Matrix3x3d mat1, Matrix3x3d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 + mat2.M00, mat1.M01 + mat2.M01, mat1.M02 + mat2.M02,
                     mat1.M10 + mat2.M10, mat1.M11 + mat2.M11, mat1.M12 + mat2.M12,
                     mat1.M20 + mat2.M20, mat1.M21 + mat2.M21, mat1.M22 + mat2.M22);
        }

        /// <summary>
        ///     Operacion resta: this = this - mat.
        /// </summary>
        public void Sub(Matrix3x3d mat)
        {
            this.Sub(this, mat);
        }

        /// <summary>
        ///     Operacion resta: this = mat1 - mat2.
        /// </summary>
        public void Sub(Matrix3x3d mat1, Matrix3x3d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 - mat2.M00, mat1.M01 - mat2.M01, mat1.M02 - mat2.M02,
                     mat1.M10 - mat2.M10, mat1.M11 - mat2.M11, mat1.M12 - mat2.M12,
                     mat1.M20 - mat2.M20, mat1.M21 - mat2.M21, mat1.M22 - mat2.M22);
        }

        /// <summary>
        ///     Operacion multiplicacion: this = this * valor.
        /// </summary>
        public void Mul(REAL v)
        {
            this.Mul(this, v);
        }

        /// <summary>
        ///     Operacion multiplicacion: this = mat * valor.
        /// </summary>
        public void Mul(Matrix3x3d mat, REAL v)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00 * v, mat.M01 * v, mat.M02 * v,
                     mat.M10 * v, mat.M11 * v, mat.M12 * v,
                     mat.M20 * v, mat.M21 * v, mat.M22 * v);
        }

        /// <summary>
        ///     Operacion division: this = this / valor.
        /// </summary>
        public void Div(REAL v)
        {
            this.Div(this, v);
        }

        /// <summary>
        ///     Operacion division: this = mat / valor.
        /// </summary>
        public void Div(Matrix3x3d mat, REAL v)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00 / v, mat.M01 / v, mat.M02 / v,
                     mat.M10 / v, mat.M11 / v, mat.M12 / v,
                     mat.M20 / v, mat.M21 / v, mat.M22 / v);
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
        public void Neg(Matrix3x3d mat)
        {
            Contract.Requires(mat != null);

            this.Set(-mat.M00, -mat.M01, -mat.M02,
                     -mat.M10, -mat.M11, -mat.M12,
                     -mat.M20, -mat.M21, -mat.M22);
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
        public void Abs(Matrix3x3d mat)
        {
            Contract.Requires(mat != null);

            this.Set(Math.Abs(mat.M00), Math.Abs(mat.M01), Math.Abs(mat.M02),
                     Math.Abs(mat.M10), Math.Abs(mat.M11), Math.Abs(mat.M12),
                     Math.Abs(mat.M20), Math.Abs(mat.M21), Math.Abs(mat.M22));
        }

        /// <summary>
        ///     Operacion multiplicacion: this = this * mat.
        /// </summary>
        public void Mul(Matrix3x3d mat)
        {
            this.Mul(this, mat);
        }

        /// <summary>
        ///     Operacion multiplicacion: this = mat1 * mat2.
        /// </summary>
        public void Mul(Matrix3x3d mat1, Matrix3x3d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 * mat2.M00 + mat1.M01 * mat2.M10 + mat1.M02 * mat2.M20,
                     mat1.M00 * mat2.M01 + mat1.M01 * mat2.M11 + mat1.M02 * mat2.M21,
                     mat1.M00 * mat2.M02 + mat1.M01 * mat2.M12 + mat1.M02 * mat2.M22,
                     mat1.M10 * mat2.M00 + mat1.M11 * mat2.M10 + mat1.M12 * mat2.M20,
                     mat1.M10 * mat2.M01 + mat1.M11 * mat2.M11 + mat1.M12 * mat2.M21,
                     mat1.M10 * mat2.M02 + mat1.M11 * mat2.M12 + mat1.M12 * mat2.M22,
                     mat1.M20 * mat2.M00 + mat1.M21 * mat2.M10 + mat1.M22 * mat2.M20,
                     mat1.M20 * mat2.M01 + mat1.M21 * mat2.M11 + mat1.M22 * mat2.M21,
                     mat1.M20 * mat2.M02 + mat1.M21 * mat2.M12 + mat1.M22 * mat2.M22);
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
        public void Transpose(Matrix3x3d mat)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00, mat.M10, mat.M20,
                     mat.M01, mat.M11, mat.M21,
                     mat.M02, mat.M12, mat.M22);
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
        public void Inv(Matrix3x3d mat)
        {
            REAL s = mat.Determinant;
            if (s.EpsilonZero())
            {
                throw new Exception("SingularMatrixException");
            }

            s = 1 / s;
            this.Set((mat.M11 * mat.M22 - mat.M12 * mat.M21) * s,
                     (mat.M02 * mat.M21 - mat.M01 * mat.M22) * s,
                     (mat.M01 * mat.M12 - mat.M02 * mat.M11) * s,
                     (mat.M12 * mat.M20 - mat.M10 * mat.M22) * s,
                     (mat.M00 * mat.M22 - mat.M02 * mat.M20) * s,
                     (mat.M02 * mat.M10 - mat.M00 * mat.M12) * s,
                     (mat.M10 * mat.M21 - mat.M11 * mat.M20) * s,
                     (mat.M01 * mat.M20 - mat.M00 * mat.M21) * s,
                     (mat.M00 * mat.M11 - mat.M01 * mat.M10) * s);
        }

        /// <summary>
        ///     Operacion determinante.
        /// </summary>
        public REAL Determinant
        {
            get
            {
                return (this.M00 * (this.M11 * this.M22 - this.M21 * this.M12)
                        - this.M01 * (this.M10 * this.M22 - this.M20 * this.M12)
                        + this.M02 * (this.M10 * this.M21 - this.M20 * this.M11));
            }
        }

        /// <summary>
        ///     Crea una copia.
        /// </summary>
        /// <returns>Copia.</returns>
        public Matrix3x3d Clone()
        {
            Matrix3x3d m = (Matrix3x3d)this.MemberwiseClone();
            return m;
        }

        #endregion

        #region privados

        /// <summary>
        ///     Comprueba si son casi iguales.
        /// </summary>
        private bool EpsilonEquals(REAL m00, REAL m01, REAL m02,
                                   REAL m10, REAL m11, REAL m12,
                                   REAL m20, REAL m21, REAL m22,
                                   REAL epsilon)
        {
            return (this.M00.EpsilonEquals(m00, epsilon)
                    && this.M01.EpsilonEquals(m01, epsilon)
                    && this.M02.EpsilonEquals(m02, epsilon)
                    && this.M10.EpsilonEquals(m10, epsilon)
                    && this.M11.EpsilonEquals(m11, epsilon)
                    && this.M12.EpsilonEquals(m12, epsilon)
                    && this.M20.EpsilonEquals(m20, epsilon)
                    && this.M21.EpsilonEquals(m21, epsilon)
                    && this.M22.EpsilonEquals(m22, epsilon));
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

            Matrix3x3d m = obj as Matrix3x3d;
            return ((m != null) && this.Equals(m));
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(this.M00).Append(this.M01).Append(this.M02)
                .Append(this.M10).Append(this.M11).Append(this.M12)
                .Append(this.M20).Append(this.M21).Append(this.M22)
                .GetHashCode();
        }

        #endregion

        #region IEquatable<MATRIX>

        [Pure]
        public bool Equals(Matrix3x3d other)
        {
            if (other == null)
            {
                return false;
            }

            return (other.M00 == this.M00) && (other.M01 == this.M01) && (other.M02 == this.M02)
                   && (other.M10 == this.M10) && (other.M11 == this.M11) && (other.M12 == this.M12)
                   && (other.M20 == this.M20) && (other.M21 == this.M21) && (other.M22 == this.M22);
        }

        #endregion

        #region IEpsilonEquatable<MATRIX>

        public bool EpsilonEquals(Matrix3x3d mat, REAL epsilon = MathUtils.EPSILON)
        {
            if (mat == null)
            {
                return false;
            }

            return this.EpsilonEquals(mat.M00, mat.M01, mat.M02,
                                      mat.M10, mat.M11, mat.M12,
                                      mat.M20, mat.M21, mat.M22,
                                      (REAL)epsilon);
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
                vfi = (VectorFormatInfo)provider.GetFormat(typeof (VectorFormatInfo));
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
                                 this.M20.ToString(format, provider),
                                 this.M21.ToString(format, provider),
                                 this.M22.ToString(format, provider));
        }

        #endregion

        #region ISerializable

        public Matrix3x3d(SerializationInfo info, StreamingContext context)
        {
            this.Set((REAL[])info.GetValue(ITEMS, typeof (REAL[])));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(ITEMS, (REAL[])this);
        }

        #endregion
    }
}