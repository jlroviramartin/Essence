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
    [Guid("AFF707D2-1E3E-430D-A500-7C420A560F5C")]
    public sealed class Matrix4x4d : IEpsilonEquatable<Matrix4x4d>,
                                     IEquatable<Matrix4x4d>,
                                     IFormattable,
                                     ICloneable,
                                     ISerializable
    {
        public const string ITEMS = "Items";
        public const string _M00 = "M00";
        public const string _M01 = "M01";
        public const string _M02 = "M02";
        public const string _M03 = "M03";
        public const string _M10 = "M10";
        public const string _M11 = "M11";
        public const string _M12 = "M12";
        public const string _M13 = "M13";
        public const string _M20 = "M20";
        public const string _M21 = "M21";
        public const string _M22 = "M22";
        public const string _M23 = "M23";
        public const string _M30 = "M30";
        public const string _M31 = "M31";
        public const string _M32 = "M32";
        public const string _M33 = "M33";

        /// <summary>
        /// Matriz identidad.
        /// </summary>
        public static Matrix4x4d Identity
        {
            get
            {
                return new Matrix4x4d(1, 0, 0, 0,
                                      0, 1, 0, 0,
                                      0, 0, 1, 0,
                                      0, 0, 0, 1);
            }
        }

        /// <summary>
        /// Matriz cero.
        /// </summary>
        public static Matrix4x4d Zero
        {
            get { return new Matrix4x4d(); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Matrix4x4d()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Matrix4x4d(double m00, double m01, double m02, double m03,
                          double m10, double m11, double m12, double m13,
                          double m20, double m21, double m22, double m23,
                          double m30, double m31, double m32, double m33)
        {
            this.Set(m00, m01, m02, m03,
                     m10, m11, m12, m13,
                     m20, m21, m22, m23,
                     m30, m31, m32, m33);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public Matrix4x4d(double[] items)
        {
            this.Set(items);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public Matrix4x4d(double[,] items)
        {
            this.Set(items);
        }

        /// <summary>
        /// Crea un mat 4x4 con los vectores directores <c>vX, vY, vZ</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        /// <param name="vZ">Vector Z.</param>
        public Matrix4x4d(Vector3d vX, Vector3d vY, Vector3d vZ)
        {
            this.Set(vX, vY, vZ);
        }

        /// <summary>
        /// Crea un mat 4x4 con los vectores directores <c>vX, vY, vZ</c>
        /// y el punto origen <c>o</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        /// <param name="vZ">Vector Z.</param>
        /// <param name="o">Punto origen.</param>
        public Matrix4x4d(Vector3d vX, Vector3d vY, Vector3d vZ, Point3d o)
        {
            this.Set(vX, vY, vZ, o);
        }

        #region Multiplicacion vectores/puntos

        public static Vector3d operator *(Matrix4x4d mat, Vector3d v)
        {
            return mat.Mul(v);
        }

        public static Point3d operator *(Matrix4x4d mat, Point3d p)
        {
            return mat.Mul(p);
        }

        public static Vector4d operator *(Matrix4x4d mat, Vector4d v)
        {
            return mat.Mul(v);
        }

        public static Point4d operator *(Matrix4x4d mat, Point4d p)
        {
            return mat.Mul(p);
        }

        public IVector3 Mul(IVector3 v)
        {
            ITuple3_Double _v = v.AsTupleDouble();

            Contract.Requires((this.M30 * _v.X + this.M31 * _v.Y + this.M32 * _v.Z).EpsilonEquals(0));
            return new Vector3d(this.M00 * _v.X + this.M01 * _v.Y + this.M02 * _v.Z,
                                this.M10 * _v.X + this.M11 * _v.Y + this.M12 * _v.Z,
                                this.M20 * _v.X + this.M21 * _v.Y + this.M22 * _v.Z);
        }

        public void Mul(IVector3 v, IOpVector3 vout)
        {
            ITuple3_Double _v = v.AsTupleDouble();
            IOpTuple3_Double _vout = vout.AsOpTupleDouble();

            Contract.Requires((this.M30 * _v.X + this.M31 * _v.Y + this.M32 * _v.Z).EpsilonEquals(0));
            _vout.Set(this.M00 * _v.X + this.M01 * _v.Y + this.M02 * _v.Z,
                      this.M10 * _v.X + this.M11 * _v.Y + this.M12 * _v.Z,
                      this.M20 * _v.X + this.M21 * _v.Y + this.M22 * _v.Z);
        }

        public IPoint3 Mul(IPoint3 p)
        {
            ITuple3_Double _p = p.AsTupleDouble();

            double d = this.M30 * _p.X + this.M31 * _p.Y + this.M32 * _p.Z + this.M33;
            return new Point3d((this.M00 * _p.X + this.M01 * _p.Y + this.M02 * _p.Z + this.M03) / d,
                               (this.M10 * _p.X + this.M11 * _p.Y + this.M12 * _p.Z + this.M13) / d,
                               (this.M20 * _p.X + this.M21 * _p.Y + this.M22 * _p.Z + this.M23) / d);
        }

        public void Mul(IPoint3 p, IOpPoint3 pout)
        {
            ITuple3_Double _p = p.AsTupleDouble();
            IOpTuple3_Double _vout = pout.AsOpTupleDouble();

            double d = this.M30 * _p.X + this.M31 * _p.Y + this.M32 * _p.Z + this.M33;
            _vout.Set((this.M00 * _p.X + this.M01 * _p.Y + this.M02 * _p.Z + this.M03) / d,
                      (this.M10 * _p.X + this.M11 * _p.Y + this.M12 * _p.Z + this.M13) / d,
                      (this.M20 * _p.X + this.M21 * _p.Y + this.M22 * _p.Z + this.M23) / d);
        }

        public Vector3d Mul(Vector3d v)
        {
            // NOTE: un vector sigue siendo un vector: w == 0.
            Contract.Requires((this.M30 * v.X + this.M31 * v.Y + this.M32 * v.Z).EpsilonEquals(0));
            return new Vector3d(this.M00 * v.X + this.M01 * v.Y + this.M02 * v.Z,
                                this.M10 * v.X + this.M11 * v.Y + this.M12 * v.Z,
                                this.M20 * v.X + this.M21 * v.Y + this.M22 * v.Z);
        }

        public Point3d Mul(Point3d p)
        {
            // NOTE: un punto sigue siendo un punto, se normaliza: w == 1.
            double d = this.M30 * p.X + this.M31 * p.Y + this.M32 * p.Z + this.M33;
            return new Point3d((this.M00 * p.X + this.M01 * p.Y + this.M02 * p.Z + this.M03) / d,
                               (this.M10 * p.X + this.M11 * p.Y + this.M12 * p.Z + this.M13) / d,
                               (this.M20 * p.X + this.M21 * p.Y + this.M22 * p.Z + this.M23) / d);
        }

        public Vector4d Mul(Vector4d v)
        {
            return new Vector4d(this.M00 * v.X + this.M01 * v.Y + this.M02 * v.Z + this.M03 * v.W,
                                this.M10 * v.X + this.M11 * v.Y + this.M12 * v.Z + this.M13 * v.W,
                                this.M20 * v.X + this.M21 * v.Y + this.M22 * v.Z + this.M23 * v.W,
                                this.M30 * v.X + this.M31 * v.Y + this.M32 * v.Z + this.M33 * v.W);
        }

        public Point4d Mul(Point4d p)
        {
            return new Point4d(this.M00 * p.X + this.M01 * p.Y + this.M02 * p.Z + this.M03 * p.W,
                               this.M10 * p.X + this.M11 * p.Y + this.M12 * p.Z + this.M13 * p.W,
                               this.M20 * p.X + this.M21 * p.Y + this.M22 * p.Z + this.M23 * p.W,
                               this.M30 * p.X + this.M31 * p.Y + this.M32 * p.Z + this.M33 * p.W);
        }

        #endregion

        #region Premultiplicacion vectores/puntos

        public static Vector3d operator *(Vector3d v, Matrix4x4d mat)
        {
            return mat.PreMul(v);
        }

        public static Point3d operator *(Point3d p, Matrix4x4d mat)
        {
            return mat.PreMul(p);
        }

        public static Vector4d operator *(Vector4d v, Matrix4x4d mat)
        {
            return mat.PreMul(v);
        }

        public static Point4d operator *(Point4d p, Matrix4x4d mat)
        {
            return mat.PreMul(p);
        }

        public Vector3d PreMul(Vector3d v)
        {
            double d = v.X * this.M03 + v.Y * this.M13 + v.Z * this.M23;
            return new Vector3d((v.X * this.M00 + v.Y * this.M10 + v.Z * this.M20) / d,
                                (v.X * this.M01 + v.Y * this.M11 + v.Z * this.M21) / d,
                                (v.X * this.M02 + v.Y * this.M12 + v.Z * this.M22) / d);
        }

        public Point3d PreMul(Point3d p)
        {
            double d = p.X * this.M03 + p.Y * this.M13 + p.Z * this.M23 + this.M33;
            return new Point3d((p.X * this.M00 + p.Y * this.M10 + p.Z * this.M20 + this.M30) / d,
                               (p.X * this.M01 + p.Y * this.M11 + p.Z * this.M21 + this.M31) / d,
                               (p.X * this.M02 + p.Y * this.M12 + p.Z * this.M22 + this.M32) / d);
        }

        public Vector4d PreMul(Vector4d v)
        {
            return new Vector4d(v.X * this.M00 + v.Y * this.M10 + v.Z * this.M20 + v.W * this.M30,
                                v.X * this.M01 + v.Y * this.M11 + v.Z * this.M21 + v.W * this.M31,
                                v.X * this.M02 + v.Y * this.M12 + v.Z * this.M22 + v.W * this.M32,
                                v.X * this.M03 + v.Y * this.M13 + v.Z * this.M23 + v.W * this.M33);
        }

        public Point4d PreMul(Point4d p)
        {
            return new Point4d(p.X * this.M00 + p.Y * this.M10 + p.Z * this.M20 + p.W * this.M30,
                               p.X * this.M01 + p.Y * this.M11 + p.Z * this.M21 + p.W * this.M31,
                               p.X * this.M02 + p.Y * this.M12 + p.Z * this.M22 + p.W * this.M32,
                               p.X * this.M03 + p.Y * this.M13 + p.Z * this.M23 + p.W * this.M33);
        }

        #endregion

        #region operadores

        /// <summary>
        /// Operacion sumar: mat1 + mat2.
        /// </summary>
        public static Matrix4x4d operator +(Matrix4x4d mat1, Matrix4x4d mat2)
        {
            Matrix4x4d matrizOut = mat1.Clone();
            matrizOut.Add(mat2);
            return matrizOut;
        }

        /// <summary>
        /// Operacion restar: mat1 - mat2.
        /// </summary>
        public static Matrix4x4d operator -(Matrix4x4d mat1, Matrix4x4d mat2)
        {
            Matrix4x4d matrizOut = mat1.Clone();
            matrizOut.Sub(mat2);
            return matrizOut;
        }

        /// <summary>
        /// Operacion restar: mat1 - mat2.
        /// </summary>
        public static Matrix4x4d operator -(Matrix4x4d mat)
        {
            Matrix4x4d matrizOut = mat.Clone();
            matrizOut.Neg();
            return matrizOut;
        }

        /// <summary>
        /// Operacion multiplicacion: mat * valor.
        /// </summary>
        public static Matrix4x4d operator *(Matrix4x4d mat, double v)
        {
            Matrix4x4d matrizOut = mat.Clone();
            matrizOut.Mul(v);
            return matrizOut;
        }

        /// <summary>
        /// Operacion multiplicacion: valor * mat.
        /// </summary>
        public static Matrix4x4d operator *(double v, Matrix4x4d mat)
        {
            Matrix4x4d matrizOut = mat.Clone();
            matrizOut.Mul(v);
            return matrizOut;
        }

        /// <summary>
        /// Operacion division: mat / valor.
        /// </summary>
        public static Matrix4x4d operator /(Matrix4x4d mat, double v)
        {
            Matrix4x4d matrizOut = mat.Clone();
            matrizOut.Div(v);
            return matrizOut;
        }

        /// <summary>
        /// Operacion multiplicacion: mat1 * mat2.
        /// </summary>
        public static Matrix4x4d operator *(Matrix4x4d mat1, Matrix4x4d mat2)
        {
            Matrix4x4d matrizOut = mat1.Clone();
            matrizOut.Mul(mat2);
            return matrizOut;
        }

        #endregion

        #region casting

        /// <summary>
        /// Casting a REAL[].
        /// </summary>
        public static explicit operator double[](Matrix4x4d m)
        {
            return new[]
            {
                m.M00, m.M01, m.M02, m.M03,
                m.M10, m.M11, m.M12, m.M13,
                m.M20, m.M21, m.M22, m.M23,
                m.M30, m.M31, m.M32, m.M33
            };
        }

        /// <summary>
        /// Casting a REAL[,].
        /// </summary>
        public static explicit operator double[,](Matrix4x4d m)
        {
            return new[,]
            {
                { m.M00, m.M01, m.M02, m.M03 },
                { m.M10, m.M11, m.M12, m.M13 },
                { m.M20, m.M21, m.M22, m.M23 },
                { m.M30, m.M31, m.M32, m.M33 }
            };
        }

        #endregion

        #region propiedades

        /// <summary>
        /// Filas.
        /// </summary>
        public int Rows
        {
            get { return 4; }
        }

        /// <summary>
        /// Columnas.
        /// </summary>
        public int Columns
        {
            get { return 4; }
        }

        /// <summary>
        /// Indica si es valido: ningun componente es NaN ni Infinito.
        /// </summary>
        public bool IsValid
        {
            get { return !this.IsNaN && !this.IsInfinity; }
        }

        /// <summary>
        /// Indica que algun componente es NaN.
        /// </summary>
        public bool IsNaN
        {
            get
            {
                return (double.IsNaN(this.M00) || double.IsNaN(this.M01)
                                               || double.IsNaN(this.M02) || double.IsNaN(this.M03))
                       || (double.IsNaN(this.M10) || double.IsNaN(this.M11)
                                                  || double.IsNaN(this.M12) || double.IsNaN(this.M13))
                       || (double.IsNaN(this.M20) || double.IsNaN(this.M21)
                                                  || double.IsNaN(this.M22) || double.IsNaN(this.M23))
                       || (double.IsNaN(this.M20) || double.IsNaN(this.M21)
                                                  || double.IsNaN(this.M22) || double.IsNaN(this.M23))
                       || (double.IsNaN(this.M30) || double.IsNaN(this.M31)
                                                  || double.IsNaN(this.M32) || double.IsNaN(this.M33));
            }
        }

        /// <summary>
        /// Indica que algun componente es infinito.
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return (double.IsInfinity(this.M00) || double.IsInfinity(this.M01)
                                                    || double.IsInfinity(this.M02) || double.IsInfinity(this.M03))
                       || (double.IsInfinity(this.M10) || double.IsInfinity(this.M11)
                                                       || double.IsInfinity(this.M12) || double.IsInfinity(this.M13))
                       || (double.IsInfinity(this.M20) || double.IsInfinity(this.M21)
                                                       || double.IsInfinity(this.M22) || double.IsInfinity(this.M23))
                       || (double.IsInfinity(this.M20) || double.IsInfinity(this.M21)
                                                       || double.IsInfinity(this.M22) || double.IsInfinity(this.M23))
                       || (double.IsInfinity(this.M30) || double.IsInfinity(this.M31)
                                                       || double.IsInfinity(this.M32) || double.IsInfinity(this.M33));
            }
        }

        /// <summary>
        /// Indica si es cero.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this.EpsilonEquals(0, 0, 0, 0,
                                          0, 0, 0, 0,
                                          0, 0, 0, 0,
                                          0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Indica si es identidad.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return this.EpsilonEquals(1, 0, 0, 0,
                                          0, 1, 0, 0,
                                          0, 0, 1, 0,
                                          0, 0, 0, 1);
            }
        }

        /// <summary>
        /// Indica si es invertible.
        /// </summary>
        public bool IsInvertible
        {
            get { return !this.Determinant.EpsilonZero(); }
        }

        /// <summary>
        /// Indica si es cuadrada.
        /// </summary>
        public bool IsSquared
        {
            get { return true; }
        }

        /// <summary>
        /// Elemento <c>i, j</c>.
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
                            case 3:
                                return this.M03;
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
                            case 3:
                                return this.M13;
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
                            case 3:
                                return this.M23;
                        }
                        break;

                    case 3:
                        switch (j)
                        {
                            case 0:
                                return this.M30;
                            case 1:
                                return this.M31;
                            case 2:
                                return this.M32;
                            case 3:
                                return this.M33;
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
                            case 3:
                                this.M03 = value;
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
                            case 3:
                                this.M13 = value;
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
                            case 3:
                                this.M23 = value;
                                return;
                        }
                        break;

                    case 3:
                        switch (j)
                        {
                            case 0:
                                this.M30 = value;
                                return;
                            case 1:
                                this.M31 = value;
                                return;
                            case 2:
                                this.M32 = value;
                                return;
                            case 3:
                                this.M33 = value;
                                return;
                        }
                        break;
                }
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Elementos fila 0.
        /// </summary>
        public double M00, M01, M02, M03;

        /// <summary>
        /// Elementos fila 1.
        /// </summary>
        public double M10, M11, M12, M13;

        /// <summary>
        /// Elementos fila 2.
        /// </summary>
        public double M20, M21, M22, M23;

        /// <summary>
        /// Elementos fila 3.
        /// </summary>
        public double M30, M31, M32, M33;

        #endregion

        #region sets

        /// <summary>
        /// Establece los elementos.
        /// </summary>
        public void Set(double m00, double m01, double m02, double m03,
                        double m10, double m11, double m12, double m13,
                        double m20, double m21, double m22, double m23,
                        double m30, double m31, double m32, double m33)
        {
            this.M00 = m00;
            this.M01 = m01;
            this.M02 = m02;
            this.M03 = m03;
            this.M10 = m10;
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M20 = m20;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M30 = m30;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
        }

        /// <summary>
        /// Establece a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public void Set(double[] items)
        {
            Contract.Requires((items != null)
                              && (items.GetLength(0) == this.Rows * this.Columns));

            this.Set(items[0], items[1], items[2], items[3],
                     items[4], items[5], items[6], items[7],
                     items[8], items[9], items[10], items[11],
                     items[12], items[13], items[14], items[15]);
        }

        /// <summary>
        /// Establece a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        public void Set(double[,] items)
        {
            Contract.Requires((items != null) && (items.GetLength(0) == this.Rows)
                                              && (items.GetLength(1) == this.Columns));

            this.Set(items[0, 0], items[0, 1], items[0, 2], items[0, 2],
                     items[1, 0], items[1, 1], items[1, 2], items[1, 2],
                     items[2, 0], items[2, 1], items[2, 2], items[2, 3],
                     items[3, 0], items[3, 1], items[3, 2], items[3, 3]);
        }

        /// <summary>
        /// Establece a cero.
        /// </summary>
        public void SetZero()
        {
            this.Set(0, 0, 0, 0,
                     0, 0, 0, 0,
                     0, 0, 0, 0,
                     0, 0, 0, 0);
        }

        /// <summary>
        /// Establece a la identidad.
        /// </summary>
        public void SetIdentity()
        {
            this.Set(1, 0, 0, 0,
                     0, 1, 0, 0,
                     0, 0, 1, 0,
                     0, 0, 0, 1);
        }

        /// <summary>
        /// Establece a <c>mat</c>.
        /// </summary>
        /// <param name="mat">Matriz.</param>
        public void Set(Matrix4x4d mat)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00, mat.M01, mat.M02, mat.M03,
                     mat.M10, mat.M11, mat.M12, mat.M13,
                     mat.M20, mat.M21, mat.M22, mat.M23,
                     mat.M30, mat.M31, mat.M32, mat.M33);
        }

        /// <summary>
        /// Establece la mat 4x4 con los vectores directores <c>vX, vY, vZ</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        /// <param name="vZ">Vector Z.</param>
        public void Set(Vector3d vX, Vector3d vY, Vector3d vZ)
        {
            this.Set(vX.X, vY.X, vZ.X, 0,
                     vX.Y, vY.Y, vZ.Y, 0,
                     vX.Z, vY.Z, vZ.Z, 0,
                     0, 0, 0, 1);
        }

        /// <summary>
        /// Establece la mat 4x4 con los vectores directores <c>vX, vY, vZ</c>
        /// y el punto origen <c>o</c>.
        /// </summary>
        /// <param name="vX">Vector X.</param>
        /// <param name="vY">Vector Y.</param>
        /// <param name="vZ">Vector Z.</param>
        /// <param name="o">Origen.</param>
        public void Set(Vector3d vX, Vector3d vY, Vector3d vZ, Point3d o)
        {
            this.Set(vX.X, vY.X, vZ.X, o.X,
                     vX.Y, vY.Y, vZ.Y, o.Y,
                     vX.Z, vY.Z, vZ.Z, o.Z,
                     0, 0, 0, 1);
        }

        #endregion

        #region operaciones

        /// <summary>
        /// Operacion suma: this = this + mat.
        /// </summary>
        public void Add(Matrix4x4d mat)
        {
            this.Add(this, mat);
        }

        /// <summary>
        /// Operacion suma: this = mat1 + mat2.
        /// </summary>
        public void Add(Matrix4x4d mat1, Matrix4x4d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 + mat2.M00, mat1.M01 + mat2.M01, mat1.M02 + mat2.M02, mat1.M03 + mat2.M03,
                     mat1.M10 + mat2.M10, mat1.M11 + mat2.M11, mat1.M12 + mat2.M12, mat1.M13 + mat2.M13,
                     mat1.M20 + mat2.M20, mat1.M21 + mat2.M21, mat1.M22 + mat2.M22, mat1.M23 + mat2.M23,
                     mat1.M30 + mat2.M30, mat1.M31 + mat2.M31, mat1.M32 + mat2.M32, mat1.M33 + mat2.M33);
        }

        /// <summary>
        /// Operacion resta: this = this - mat.
        /// </summary>
        public void Sub(Matrix4x4d mat)
        {
            this.Sub(this, mat);
        }

        /// <summary>
        /// Operacion resta: this = mat1 - mat2.
        /// </summary>
        public void Sub(Matrix4x4d mat1, Matrix4x4d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 - mat2.M00, mat1.M01 - mat2.M01, mat1.M02 - mat2.M02, mat1.M03 - mat2.M03,
                     mat1.M10 - mat2.M10, mat1.M11 - mat2.M11, mat1.M12 - mat2.M12, mat1.M13 - mat2.M13,
                     mat1.M20 - mat2.M20, mat1.M21 - mat2.M21, mat1.M22 - mat2.M22, mat1.M23 - mat2.M23,
                     mat1.M30 - mat2.M30, mat1.M31 - mat2.M31, mat1.M32 - mat2.M32, mat1.M33 - mat2.M33);
        }

        /// <summary>
        /// Operacion multiplicacion: this = this * valor.
        /// </summary>
        public void Mul(double v)
        {
            this.Mul(this, v);
        }

        /// <summary>
        /// Operacion multiplicacion: this = mat * valor.
        /// </summary>
        public void Mul(Matrix4x4d mat, double v)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00 * v, mat.M01 * v, mat.M02 * v, mat.M03 * v,
                     mat.M10 * v, mat.M11 * v, mat.M12 * v, mat.M13 * v,
                     mat.M20 * v, mat.M21 * v, mat.M22 * v, mat.M23 * v,
                     mat.M30 * v, mat.M31 * v, mat.M32 * v, mat.M33 * v);
        }

        /// <summary>
        /// Operacion division: this = this / valor.
        /// </summary>
        public void Div(double v)
        {
            this.Div(this, v);
        }

        /// <summary>
        /// Operacion division: this = mat / valor.
        /// </summary>
        public void Div(Matrix4x4d mat, double v)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00 / v, mat.M01 / v, mat.M02 / v, mat.M03 / v,
                     mat.M10 / v, mat.M11 / v, mat.M12 / v, mat.M13 / v,
                     mat.M20 / v, mat.M21 / v, mat.M22 / v, mat.M23 / v,
                     mat.M30 / v, mat.M31 / v, mat.M32 / v, mat.M33 / v);
        }

        /// <summary>
        /// Operacion cambio signo: this = -this.
        /// </summary>
        public void Neg()
        {
            this.Neg(this);
        }

        /// <summary>
        /// Operacion cambio signo: this = -mat.
        /// </summary>
        public void Neg(Matrix4x4d mat)
        {
            Contract.Requires(mat != null);

            this.Set(-mat.M00, -mat.M01, -mat.M02, -mat.M03,
                     -mat.M10, -mat.M11, -mat.M12, -mat.M13,
                     -mat.M20, -mat.M21, -mat.M22, -mat.M23,
                     -mat.M30, -mat.M31, -mat.M32, -mat.M33);
        }

        /// <summary>
        /// Operacion valor absoluto: this = Absoluto ( this ).
        /// </summary>
        public void Abs()
        {
            this.Abs(this);
        }

        /// <summary>
        /// Operacion valor absoluto: this = Absoluto ( mat ).
        /// </summary>
        public void Abs(Matrix4x4d mat)
        {
            Contract.Requires(mat != null);

            this.Set(Math.Abs(mat.M00), Math.Abs(mat.M01), Math.Abs(mat.M02), Math.Abs(mat.M03),
                     Math.Abs(mat.M10), Math.Abs(mat.M11), Math.Abs(mat.M12), Math.Abs(mat.M13),
                     Math.Abs(mat.M20), Math.Abs(mat.M21), Math.Abs(mat.M22), Math.Abs(mat.M23),
                     Math.Abs(mat.M30), Math.Abs(mat.M31), Math.Abs(mat.M32), Math.Abs(mat.M33));
        }

        /// <summary>
        /// Operacion multiplicacion: this = this * mat.
        /// </summary>
        public void Mul(Matrix4x4d mat)
        {
            this.Mul(this, mat);
        }

        /// <summary>
        /// Operacion multiplicacion: this = mat1 * mat2.
        /// </summary>
        public void Mul(Matrix4x4d mat1, Matrix4x4d mat2)
        {
            Contract.Requires((mat1 != null) && (mat2 != null));

            this.Set(mat1.M00 * mat2.M00 + mat1.M01 * mat2.M10 + mat1.M02 * mat2.M20 + mat1.M03 * mat2.M30,
                     mat1.M00 * mat2.M01 + mat1.M01 * mat2.M11 + mat1.M02 * mat2.M21 + mat1.M03 * mat2.M31,
                     mat1.M00 * mat2.M02 + mat1.M01 * mat2.M12 + mat1.M02 * mat2.M22 + mat1.M03 * mat2.M32,
                     mat1.M00 * mat2.M03 + mat1.M01 * mat2.M13 + mat1.M02 * mat2.M23 + mat1.M03 * mat2.M33,
                     mat1.M10 * mat2.M00 + mat1.M11 * mat2.M10 + mat1.M12 * mat2.M20 + mat1.M13 * mat2.M30,
                     mat1.M10 * mat2.M01 + mat1.M11 * mat2.M11 + mat1.M12 * mat2.M21 + mat1.M13 * mat2.M31,
                     mat1.M10 * mat2.M02 + mat1.M11 * mat2.M12 + mat1.M12 * mat2.M22 + mat1.M13 * mat2.M32,
                     mat1.M10 * mat2.M03 + mat1.M11 * mat2.M13 + mat1.M12 * mat2.M23 + mat1.M13 * mat2.M33,
                     mat1.M20 * mat2.M00 + mat1.M21 * mat2.M10 + mat1.M22 * mat2.M20 + mat1.M23 * mat2.M30,
                     mat1.M20 * mat2.M01 + mat1.M21 * mat2.M11 + mat1.M22 * mat2.M21 + mat1.M23 * mat2.M31,
                     mat1.M20 * mat2.M02 + mat1.M21 * mat2.M12 + mat1.M22 * mat2.M22 + mat1.M23 * mat2.M32,
                     mat1.M20 * mat2.M03 + mat1.M21 * mat2.M13 + mat1.M22 * mat2.M23 + mat1.M23 * mat2.M33,
                     mat1.M30 * mat2.M00 + mat1.M31 * mat2.M10 + mat1.M32 * mat2.M20 + mat1.M33 * mat2.M30,
                     mat1.M30 * mat2.M01 + mat1.M31 * mat2.M11 + mat1.M32 * mat2.M21 + mat1.M33 * mat2.M31,
                     mat1.M30 * mat2.M02 + mat1.M31 * mat2.M12 + mat1.M32 * mat2.M22 + mat1.M33 * mat2.M32,
                     mat1.M30 * mat2.M03 + mat1.M31 * mat2.M13 + mat1.M32 * mat2.M23 + mat1.M33 * mat2.M33);
        }

        /// <summary>
        /// Operacion trasposicion: this = Trasponer( this ).
        /// </summary>
        public void Transpose()
        {
            this.Transpose(this);
        }

        /// <summary>
        /// Operacion trasposicion: this = Trasponer( mat ).
        /// </summary>
        public void Transpose(Matrix4x4d mat)
        {
            Contract.Requires(mat != null);

            this.Set(mat.M00, mat.M10, mat.M20, mat.M30,
                     mat.M01, mat.M11, mat.M21, mat.M31,
                     mat.M02, mat.M12, mat.M22, mat.M32,
                     mat.M03, mat.M13, mat.M23, mat.M33);
        }

        /// <summary>
        /// Operacion inversion: this = Invertir( this ).
        /// </summary>
        public void Inv()
        {
            this.Inv(this);
        }

        /// <summary>
        /// Operacion inversion: this = Invertir( mat ).
        /// </summary>
        public void Inv(Matrix4x4d mat)
        {
            double s = mat.Determinant;
            if (s.EpsilonZero())
            {
                throw new Exception("SingularMatrixException");
            }

            s = 1 / s;
            this.Set((mat.M11 * (mat.M22 * mat.M33 - mat.M23 * mat.M32)
                      + mat.M12 * (mat.M23 * mat.M31 - mat.M21 * mat.M33)
                      + mat.M13 * (mat.M21 * mat.M32 - mat.M22 * mat.M31)) * s,
                     (mat.M21 * (mat.M02 * mat.M33 - mat.M03 * mat.M32)
                      + mat.M22 * (mat.M03 * mat.M31 - mat.M01 * mat.M33)
                      + mat.M23 * (mat.M01 * mat.M32 - mat.M02 * mat.M31)) * s,
                     (mat.M31 * (mat.M02 * mat.M13 - mat.M03 * mat.M12)
                      + mat.M32 * (mat.M03 * mat.M11 - mat.M01 * mat.M13)
                      + mat.M33 * (mat.M01 * mat.M12 - mat.M02 * mat.M11)) * s,
                     (mat.M01 * (mat.M13 * mat.M22 - mat.M12 * mat.M23)
                      + mat.M02 * (mat.M11 * mat.M23 - mat.M13 * mat.M21)
                      + mat.M03 * (mat.M12 * mat.M21 - mat.M11 * mat.M22)) * s,
                     (mat.M12 * (mat.M20 * mat.M33 - mat.M23 * mat.M30)
                      + mat.M13 * (mat.M22 * mat.M30 - mat.M20 * mat.M32)
                      + mat.M10 * (mat.M23 * mat.M32 - mat.M22 * mat.M33)) * s,
                     (mat.M22 * (mat.M00 * mat.M33 - mat.M03 * mat.M30)
                      + mat.M23 * (mat.M02 * mat.M30 - mat.M00 * mat.M32)
                      + mat.M20 * (mat.M03 * mat.M32 - mat.M02 * mat.M33)) * s,
                     (mat.M32 * (mat.M00 * mat.M13 - mat.M03 * mat.M10)
                      + mat.M33 * (mat.M02 * mat.M10 - mat.M00 * mat.M12)
                      + mat.M30 * (mat.M03 * mat.M12 - mat.M02 * mat.M13)) * s,
                     (mat.M02 * (mat.M13 * mat.M20 - mat.M10 * mat.M23)
                      + mat.M03 * (mat.M10 * mat.M22 - mat.M12 * mat.M20)
                      + mat.M00 * (mat.M12 * mat.M23 - mat.M13 * mat.M22)) * s,
                     (mat.M13 * (mat.M20 * mat.M31 - mat.M21 * mat.M30)
                      + mat.M10 * (mat.M21 * mat.M33 - mat.M23 * mat.M31)
                      + mat.M11 * (mat.M23 * mat.M30 - mat.M20 * mat.M33)) * s,
                     (mat.M23 * (mat.M00 * mat.M31 - mat.M01 * mat.M30)
                      + mat.M20 * (mat.M01 * mat.M33 - mat.M03 * mat.M31)
                      + mat.M21 * (mat.M03 * mat.M30 - mat.M00 * mat.M33)) * s,
                     (mat.M33 * (mat.M00 * mat.M11 - mat.M01 * mat.M10)
                      + mat.M30 * (mat.M01 * mat.M13 - mat.M03 * mat.M11)
                      + mat.M31 * (mat.M03 * mat.M10 - mat.M00 * mat.M13)) * s,
                     (mat.M03 * (mat.M11 * mat.M20 - mat.M10 * mat.M21)
                      + mat.M00 * (mat.M13 * mat.M21 - mat.M11 * mat.M23)
                      + mat.M01 * (mat.M10 * mat.M23 - mat.M13 * mat.M20)) * s,
                     (mat.M10 * (mat.M22 * mat.M31 - mat.M21 * mat.M32)
                      + mat.M11 * (mat.M20 * mat.M32 - mat.M22 * mat.M30)
                      + mat.M12 * (mat.M21 * mat.M30 - mat.M20 * mat.M31)) * s,
                     (mat.M20 * (mat.M02 * mat.M31 - mat.M01 * mat.M32)
                      + mat.M21 * (mat.M00 * mat.M32 - mat.M02 * mat.M30)
                      + mat.M22 * (mat.M01 * mat.M30 - mat.M00 * mat.M31)) * s,
                     (mat.M30 * (mat.M02 * mat.M11 - mat.M01 * mat.M12)
                      + mat.M31 * (mat.M00 * mat.M12 - mat.M02 * mat.M10)
                      + mat.M32 * (mat.M01 * mat.M10 - mat.M00 * mat.M11)) * s,
                     (mat.M00 * (mat.M11 * mat.M22 - mat.M12 * mat.M21)
                      + mat.M01 * (mat.M12 * mat.M20 - mat.M10 * mat.M22)
                      + mat.M02 * (mat.M10 * mat.M21 - mat.M11 * mat.M20)) * s);
        }

        /// <summary>
        /// Operacion determinante.
        /// </summary>
        public double Determinant
        {
            get
            {
                return ((this.M00 * this.M11 - this.M01 * this.M10) * (this.M22 * this.M33 - this.M23 * this.M32)
                        - (this.M00 * this.M12 - this.M02 * this.M10) * (this.M21 * this.M33 - this.M23 * this.M31)
                        + (this.M00 * this.M13 - this.M03 * this.M10) * (this.M21 * this.M32 - this.M22 * this.M31)
                        + (this.M01 * this.M12 - this.M02 * this.M11) * (this.M20 * this.M33 - this.M23 * this.M30)
                        - (this.M01 * this.M13 - this.M03 * this.M11) * (this.M20 * this.M32 - this.M22 * this.M30)
                        + (this.M02 * this.M13 - this.M03 * this.M12) * (this.M20 * this.M31 - this.M21 * this.M30));
            }
        }

        /// <summary>
        /// Crea una copia.
        /// </summary>
        /// <returns>Copia.</returns>
        public Matrix4x4d Clone()
        {
            Matrix4x4d m = (Matrix4x4d)this.MemberwiseClone();
            return m;
        }

        #endregion

        #region privados

        /// <summary>
        /// Comprueba si son casi iguales.
        /// </summary>
        private bool EpsilonEquals(double m00, double m01, double m02, double m03,
                                   double m10, double m11, double m12, double m13,
                                   double m20, double m21, double m22, double m23,
                                   double m30, double m31, double m32, double m33,
                                   double epsilon = MathUtils.EPSILON)
        {
            return (this.M00.EpsilonEquals(m00, epsilon)
                    && this.M01.EpsilonEquals(m01, epsilon)
                    && this.M02.EpsilonEquals(m02, epsilon)
                    && this.M03.EpsilonEquals(m03, epsilon)
                    && this.M10.EpsilonEquals(m10, epsilon)
                    && this.M11.EpsilonEquals(m11, epsilon)
                    && this.M12.EpsilonEquals(m12, epsilon)
                    && this.M13.EpsilonEquals(m13, epsilon)
                    && this.M20.EpsilonEquals(m20, epsilon)
                    && this.M21.EpsilonEquals(m21, epsilon)
                    && this.M22.EpsilonEquals(m22, epsilon)
                    && this.M23.EpsilonEquals(m23, epsilon)
                    && this.M30.EpsilonEquals(m30, epsilon)
                    && this.M31.EpsilonEquals(m31, epsilon)
                    && this.M32.EpsilonEquals(m32, epsilon)
                    && this.M33.EpsilonEquals(m33, epsilon));
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

            Matrix4x4d m = obj as Matrix4x4d;
            return ((m != null) && this.Equals(m));
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                   .Append(this.M00).Append(this.M01).Append(this.M02).Append(this.M03)
                   .Append(this.M10).Append(this.M11).Append(this.M12).Append(this.M13)
                   .Append(this.M20).Append(this.M21).Append(this.M22).Append(this.M23)
                   .Append(this.M30).Append(this.M31).Append(this.M32).Append(this.M33)
                   .GetHashCode();
        }

        #endregion

        #region IEquatable<Matrix4x4d>

        [Pure]
        public bool Equals(Matrix4x4d other)
        {
            if (other == null)
            {
                return false;
            }

            return (other.M00 == this.M00) && (other.M01 == this.M01) && (other.M02 == this.M02) && (other.M03 == this.M03)
                   && (other.M10 == this.M10) && (other.M11 == this.M11) && (other.M12 == this.M12) && (other.M13 == this.M13)
                   && (other.M20 == this.M20) && (other.M21 == this.M21) && (other.M22 == this.M22) && (other.M23 == this.M23)
                   && (other.M30 == this.M30) && (other.M31 == this.M31) && (other.M32 == this.M32) && (other.M33 == this.M33);
        }

        #endregion

        #region IEpsilonEquatable<Matrix4x4d>

        public bool EpsilonEquals(Matrix4x4d mat, double epsilon = MathUtils.EPSILON)
        {
            if (mat == null)
            {
                return false;
            }

            return this.EpsilonEquals(mat.M00, mat.M01, mat.M02, mat.M03,
                                      mat.M10, mat.M11, mat.M12, mat.M13,
                                      mat.M20, mat.M21, mat.M22, mat.M23,
                                      mat.M30, mat.M31, mat.M32, mat.M33,
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
                                 "{0}{0}{3}{1} {4}{1} {5}{1} {6}{2}{1} "
                                 + "{0}{7}{1} {8}{1} {9}{1} {10}{2}{1} "
                                 + "{0}{11}{1} {12}{1} {13}{1} {14}{2}{1} "
                                 + "{0}{15}{1} {16}{1} {17}{1} {18}{2}{2} ",
                                 vfi.Beg, vfi.Sep, vfi.End,
                                 this.M00.ToString(format, provider),
                                 this.M01.ToString(format, provider),
                                 this.M02.ToString(format, provider),
                                 this.M03.ToString(format, provider),
                                 this.M10.ToString(format, provider),
                                 this.M11.ToString(format, provider),
                                 this.M12.ToString(format, provider),
                                 this.M13.ToString(format, provider),
                                 this.M20.ToString(format, provider),
                                 this.M21.ToString(format, provider),
                                 this.M22.ToString(format, provider),
                                 this.M23.ToString(format, provider),
                                 this.M30.ToString(format, provider),
                                 this.M31.ToString(format, provider),
                                 this.M32.ToString(format, provider),
                                 this.M33.ToString(format, provider));
        }

        #endregion

        #region ISerializable

        public Matrix4x4d(SerializationInfo info, StreamingContext context)
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