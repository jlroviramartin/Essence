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
using Essence.Util.Math.Double;
using REAL = System.Double;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    ///     Utilidades para las matrices <c>MATRIX</c>.
    ///     <see cref="http://arantxa.ii.uam.es/~pedro/graficos/teoria/Transformaciones2D/Transformaciones2D.htm" />
    ///     <see cref="http://arantxa.ii.uam.es/~pedro/graficos/teoria/Windows/Windows.htm" />
    ///     <see cref="http://arantxa.ii.uam.es/~pedro/graficos/teoria/Recorte2D/Recorte2D.htm" />
    /// </summary>
    public static class Matrix3x3dUtils
    {
        /// <summary>
        ///     <see cref="http://adndevblog.typepad.com/autocad/2012/08/remove-scaling-from-transformation-matrix.html" />
        /// </summary>
        public static Matrix3x3d RemoveScaling(Matrix3x3d m, REAL scalingValue = 1)
        {
            Vector2d v0 = new Vector2d(m.M00, m.M01).Unit.Mul(scalingValue);
            Vector2d v1 = new Vector2d(m.M10, m.M11).Unit.Mul(scalingValue);

            return new Matrix3x3d(
                new[,]
                {
                    { v0.X, v0.Y, m.M02 },
                    { v1.X, v1.Y, m.M12 },
                    { m.M20, m.M21, m.M22 }
                });
        }

        #region Traslacion, rotacion, escala

        /// <summary>
        ///     Crea una matriz con la traslacion, rotacion y escala: mt * mr * me.
        ///     Primero se escala, despues se rota y finalmente se traslada.
        /// </summary>
        /// <param name="t">Traslacion.</param>
        /// <param name="r">Rotacion en radianes.</param>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz de traslacion, rotacion y escala.</returns>
        public static Matrix3x3d TranslateRotateScale(Vector2d t, REAL r, Vector2d e)
        {
            return TranslateRotateScale(t.X, t.Y, r, e.X, e.Y);
        }

        /// <summary>
        ///     Crea una matriz con la traslacion, rotacion y escala: mt * mr * me.
        ///     Primero se escala, despues se rota y finalmente se traslada.
        /// </summary>
        /// <param name="o">Origen.</param>
        /// <param name="r">Rotacion en radianes.</param>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz de traslacion, rotacion y escala.</returns>
        public static Matrix3x3d TranslateRotateScale(Point2d o, REAL r, Vector2d e)
        {
            return TranslateRotateScale(o.X, o.Y, r, e.X, e.Y);
        }

        /// <summary>
        ///     Crea una matriz con la traslacion, rotacion y escala: mt * mr * me.
        ///     Primero se escala, despues se rota y finalmente se traslada.
        /// </summary>
        /// <param name="tx">Traslacion x.</param>
        /// <param name="ty">Traslacion y.</param>
        /// <param name="r">Rotacion en radianes.</param>
        /// <param name="ex">Escala x.</param>
        /// <param name="ey">Escala y.</param>
        /// <returns>Matriz de traslacion, rotacion y escala.</returns>
        public static Matrix3x3d TranslateRotateScale(REAL tx, REAL ty, REAL r, REAL ex, REAL ey)
        {
            REAL s = (REAL)Math.Sin(r);
            REAL c = (REAL)Math.Cos(r);
            return new Matrix3x3d(
                c * ex, -ey * s, tx,
                ex * s, c * ey, ty,
                0, 0, 1);
        }

        #endregion

        #region Translate

        /// <summary>
        ///     Crea una matriz con la traslacion.
        /// </summary>
        /// <param name="t">Traslacion.</param>
        /// <returns>Matriz de traslacion.</returns>
        public static Matrix3x3d Translate(Vector2d t)
        {
            return Translate(t.X, t.Y);
        }

        /// <summary>
        ///     Crea una matriz con la traslacion.
        /// </summary>
        /// <param name="o">Origen.</param>
        /// <returns>Matriz de traslacion.</returns>
        public static Matrix3x3d Translate(Point2d o)
        {
            return Translate(o.X, o.Y);
        }

        /// <summary>
        ///     Crea una matriz con la traslacion.
        /// </summary>
        /// <param name="tx">Traslacion x.</param>
        /// <param name="ty">Traslacion y.</param>
        /// <returns>Matriz de traslacion.</returns>
        public static Matrix3x3d Translate(REAL tx, REAL ty)
        {
            return new Matrix3x3d(
                1, 0, tx,
                0, 1, ty,
                0, 0, 1);
        }

        #endregion

        /// <summary>
        ///     Crea una matriz a la que se le han invertido los ejes.
        /// </summary>
        /// <param name="axis">Indica que ejes invierte.</param>
        /// <returns>Matriz.</returns>
        public static Matrix3x3d InvertAxis(Axis axis)
        {
            bool x = ((axis & Axis.X) != 0);
            bool y = ((axis & Axis.Y) != 0);
            return new Matrix3x3d(
                x ? -1 : 1, 0, 0,
                0, y ? -1 : 1, 0,
                0, 0, 1);
        }

        #region Rotate

        /// this conversion uses NASA standard aeroplane conventions as described on page:
        /// http://www.euclideanspace.com/maths/geometry/rotations/euler/index.htm
        /// Coordinate System: right hand
        /// Positive angle: right hand
        /// Order of euler angles: heading first, then attitude, then bank
        /// matrix row column ordering:
        /// [m00 m01 m02 0]
        /// [m10 m11 m12 0]
        /// [m20 m21 m22 0]
        /// [0   0   0   1]
        public static Matrix3x3d RotateEuler(REAL heading, REAL attitude, REAL bank)
        {
            // Assuming the angles are in radians.
            REAL ch = (REAL)Math.Cos(heading);
            REAL sh = (REAL)Math.Sin(heading);
            REAL ca = (REAL)Math.Cos(attitude);
            REAL sa = (REAL)Math.Sin(attitude);
            REAL cb = (REAL)Math.Cos(bank);
            REAL sb = (REAL)Math.Sin(bank);
            return new Matrix3x3d(
                ch * ca, sh * sb - ch * sa * cb, ch * sa * sb + sh * cb,
                sa, ca * cb, -ca * sb,
                -sh * ca, sh * sa * cb + ch * sb, -sh * sa * sb + ch * cb);
        }

        public static Matrix3x3d RotateQuat(Vector4d v)
        {
            return RotateQuat(v.X, v.Y, v.Z, v.W);
        }

        public static Matrix3x3d RotateQuat(REAL x, REAL y, REAL z, REAL w)
        {
            // squared length
            double length2 = w * w + x * x + y * y + z * z;

            double rlength2;
            if (!length2.EpsilonEquals(1.0))
            {
                rlength2 = 2.0 / length2;
            }
            else
            {
                rlength2 = 2.0;
            }

            double x2 = rlength2 * x;
            double y2 = rlength2 * y;
            double z2 = rlength2 * z;
            double xx = x2 * x;
            double xy = y2 * x;
            double xz = z2 * x;
            double yy = y2 * y;
            double yz = z2 * y;
            double zz = z2 * z;
            double wx = x2 * w;
            double wy = y2 * w;
            double wz = z2 * w;

            return new Matrix3x3d(
                1.0 - (yy + zz), xy - wz, xz + wy,
                xy + wz, 1.0 - (xx + zz), yz - wx,
                xz - wy, yz + wx, 1.0 - (xx + yy));
        }

        /// <summary>
        ///     Crea una matriz con la rotacion respecto al origen.
        /// </summary>
        /// <param name="r">Angulo en radianes.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix3x3d Rotate(REAL r)
        {
            REAL c = (REAL)Math.Cos(r);
            REAL s = (REAL)Math.Sin(r);
            return new Matrix3x3d(
                c, -s, 0,
                s, c, 0,
                0, 0, 1);
        }

        /// <summary>
        ///     Crea una matriz con la rotacion respecto a un punto de aplicacion.
        /// </summary>
        /// <param name="p">Punto de aplicacion.</param>
        /// <param name="r">Angulo en radianes.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix3x3d Rotate(Point2d p, REAL r)
        {
            return Rotate(p.X, p.Y, r);
        }

        /// <summary>
        ///     Crea una matriz con la rotacion respecto a un punto de aplicacion.
        /// </summary>
        /// <param name="px">Punto de aplicacion X.</param>
        /// <param name="py">Punto de aplicacion Y.</param>
        /// <param name="r">Angulo en radianes.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix3x3d Rotate(REAL px, REAL py, REAL r)
        {
            REAL c = (REAL)Math.Cos(r);
            REAL s = (REAL)Math.Sin(r);
            return new Matrix3x3d(
                c, -s, -px * c + py * s + px,
                s, c, -px * s - py * c + py,
                0, 0, 1);
        }

        #endregion

        #region Scale

        /// <summary>
        ///     Crea una matriz con la escala respecto al origen.
        /// </summary>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix3x3d Scale(REAL e)
        {
            return Scale(e, e);
        }

        /// <summary>
        ///     Crea una matriz con la escala respecto al origen.
        /// </summary>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix3x3d Scale(Vector2d e)
        {
            return Scale(e.X, e.Y);
        }

        /// <summary>
        ///     Crea una matriz con la escala respecto al origen.
        /// </summary>
        /// <param name="ex">Escala x.</param>
        /// <param name="ey">Escala y.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix3x3d Scale(REAL ex, REAL ey)
        {
            return new Matrix3x3d(
                ex, 0, 0,
                0, ey, 0,
                0, 0, 1);
        }

        /// <summary>
        ///     Crea una matriz con la escala respecto a un punto de aplicacion.
        /// </summary>
        /// <param name="p">Punto de aplicacion.</param>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix3x3d Scale(Point2d p, REAL e)
        {
            return Scale(p.X, p.Y, e, e);
        }

        /// <summary>
        ///     Crea una matriz con la escala respecto a un punto de aplicacion.
        /// </summary>
        /// <param name="p">Punto de aplicacion.</param>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix3x3d Scale(Point2d p, Vector2d e)
        {
            return Scale(p.X, p.Y, e.X, e.Y);
        }

        /// <summary>
        ///     Crea una matriz con la escala respecto a un punto de aplicacion.
        /// </summary>
        /// <param name="px">Punto de aplicacion X.</param>
        /// <param name="py">Punto de aplicacion Y.</param>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix3x3d Scale(REAL px, REAL py, REAL e)
        {
            return Scale(px, py, e, e);
        }

        /// <summary>
        ///     Crea una matriz con la escala respecto a un punto de aplicacion.
        /// </summary>
        /// <param name="px">Punto de aplicacion X.</param>
        /// <param name="py">Punto de aplicacion Y.</param>
        /// <param name="ex">Escala x.</param>
        /// <param name="ey">Escala y.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix3x3d Scale(REAL px, REAL py, REAL ex, REAL ey)
        {
            return new Matrix3x3d(
                ex, 0, px - ex * px,
                0, ey, py - ey * py,
                0, 0, 1);
        }

        #endregion

        /// <summary>
        ///     Crea una transformacion de un rectangulo.
        ///     <code><![CDATA[
        ///  *
        ///  |                  * p2
        ///  |            ==>   |
        ///  |                  *-----------*
        ///  *--------*         p0          p1
        ///  recOrig
        /// ]]></code>
        /// </summary>
        /// <param name="recOrig">Rectangulo origen.</param>
        /// <param name="recDst">Rectangulo destino.</param>
        /// <returns>Transformacion.</returns>
        public static Matrix3x3d TransformRectangle(Rectangle2d recOrig,
                                                    Rectangle2d recDst)
        {
            Contract.Requires((recOrig.DX > 0) && (recOrig.DY > 0));

            REAL m00 = recDst.DX / recOrig.DX;
            REAL m10 = 0;
            REAL m01 = 0;
            REAL m11 = recDst.DY / recOrig.DY;

            /*MATRIX m = new MATRIX(
                m00, m01, recDst.Origen.X,
                m10, m11, recDst.Origen.Y,
                0, 0, 1);
            MATRIX m2 = Translate(-recOrig.X, -recOrig.Y);
            m.Mul(m2);

            return m;*/
            return new Matrix3x3d(
                m00, m01, (-m01 * recOrig.Y) - m00 * recOrig.X + recDst.X,
                m10, m11, (-m11 * recOrig.Y) - m10 * recOrig.X + recDst.Y,
                0, 0, 1);
        }

        /// <summary>
        ///     Crea una transformacion de un rectangulo.
        ///     <code><![CDATA[
        ///  *
        ///  |                  * pd2   * pd1
        ///  |            ==>    \     /
        ///  |                    \   /
        ///  *--------*            \ /
        ///  recOrig                * pd0
        /// ]]></code>
        /// </summary>
        /// <param name="recOrig">Rectangulo origen.</param>
        /// <param name="pd0">Punto destino LB.</param>
        /// <param name="pd1">Punto destino RB.</param>
        /// <param name="pd2">Punto destino LT.</param>
        /// <returns>Transformacion.</returns>
        public static Matrix3x3d TransformRectangle(Rectangle2d recOrig,
                                                    Point2d pd0, Point2d pd1, Point2d pd2)
        {
            Contract.Requires((recOrig.DX > 0) && (recOrig.DY > 0));

            REAL m00 = (pd1.X - pd0.X) / recOrig.DX;
            REAL m10 = (pd1.Y - pd0.Y) / recOrig.DX;
            REAL m01 = (pd2.X - pd0.X) / recOrig.DY;
            REAL m11 = (pd2.Y - pd0.Y) / recOrig.DY;

            /*MATRIX m = new MATRIX(
                m00, m01, pd0.X,
                m10, m11, pd0.Y,
                0, 0, 1);
            MATRIX m2 = Translate(-recOrig.X, -recOrig.Y);
            m.Mul(m2);

            return m;*/
            return new Matrix3x3d(
                m00, m01, (-m01 * recOrig.Y) - m00 * recOrig.X + pd0.X,
                m10, m11, (-m11 * recOrig.Y) - m10 * recOrig.X + pd0.Y,
                0, 0, 1);
        }

        /// <summary>
        ///     Crea una transformacion de un rectangulo.
        ///     <code><![CDATA[
        ///                    *
        ///  * po2   * po1     |
        ///   \     /          |
        ///    \   /      ==>  |
        ///     \ /            *--------*
        ///      * po0         recDst
        /// ]]></code>
        /// </summary>
        /// <param name="po0">Punto origen LB.</param>
        /// <param name="po1">Punto origen RB.</param>
        /// <param name="po2">Punto origen LT.</param>
        /// <param name="recDst">Rectangulo destino.</param>
        /// <returns>Transformacion.</returns>
        public static Matrix3x3d TransformRectangle(Point2d po0, Point2d po1, Point2d po2,
                                                    Rectangle2d recDst)
        {
            /*MATRIX m = TransformRectangle(recDst, po0, po1, po2);
            m.Inv();

            return m;*/
            Contract.Requires((recDst.DX > 0) && (recDst.DY > 0));

            REAL m00 = (po1.X - po0.X) / recDst.DX;
            REAL m10 = (po1.Y - po0.Y) / recDst.DX;
            REAL m01 = (po2.X - po0.X) / recDst.DY;
            REAL m11 = (po2.Y - po0.Y) / recDst.DY;

            REAL det = 1 / (m00 * m11 - m01 * m10);

            return new Matrix3x3d(
                m11 * det, -m01 * det, (m01 * ((-m11 * recDst.Y) - m10 * recDst.X + po0.Y) - m11 * ((-m01 * recDst.Y) - m00 * recDst.X + po0.X)) * det,
                -m10 * det, m00 * det, (m10 * ((-m01 * recDst.Y) - m00 * recDst.X + po0.X) - m00 * ((-m11 * recDst.Y) - m10 * recDst.X + po0.Y)) * det,
                0, 0, 1);
        }

        /// <summary>
        ///     Crea una transformacion de un rectangulo.
        ///     <code><![CDATA[
        ///      * po2
        ///     /               * pd2   * pd1
        ///    /                 \     /
        ///   /           ==>     \   /
        ///  * po0                 \ /
        ///   \                     * pd0
        ///    \
        ///     \
        ///      *
        ///      po1          
        /// ]]></code>
        /// </summary>
        /// <param name="po0">Punto origen LB.</param>
        /// <param name="po1">Punto origen RB.</param>
        /// <param name="po2">Punto origen LT.</param>
        /// <param name="pd0">Punto destino LB.</param>
        /// <param name="pd1">Punto destino RB.</param>
        /// <param name="pd2">Punto destino LT.</param>
        /// <returns>Transformacion.</returns>
        public static Matrix3x3d TransformRectangle(Point2d po0, Point2d po1, Point2d po2,
                                                    Point2d pd0, Point2d pd1, Point2d pd2)
        {
            //RECTANGLE2 rec = new RECTANGLE2(0, 0, 1, 1);

            ////////////////////////////////////////////////////////////////////////////////////////
            //MATRIX m = TransformRectangle(rec, pd0, pd1, pd2);
            Matrix3x3d m;
            {
                REAL m00 = (pd1.X - pd0.X);
                REAL m10 = (pd1.Y - pd0.Y);
                REAL m01 = (pd2.X - pd0.X);
                REAL m11 = (pd2.Y - pd0.Y);
                m = new Matrix3x3d(
                    m00, m01, pd0.X,
                    m10, m11, pd0.Y,
                    0, 0, 1);
            }
            ////////////////////////////////////////////////////////////////////////////////////////
            //MATRIX aux = TransformRectangle(rec, po0, po1, po2);
            Matrix3x3d aux;
            {
                REAL m00 = (po1.X - po0.X);
                REAL m10 = (po1.Y - po0.Y);
                REAL m01 = (po2.X - po0.X);
                REAL m11 = (po2.Y - po0.Y);

                aux = new Matrix3x3d(
                    m00, m01, po0.X,
                    m10, m11, po0.Y,
                    0, 0, 1);
            }
            aux.Inv();
            ////////////////////////////////////////////////////////////////////////////////////////

            m.Mul(aux);

            return m;
        }

        /// <summary>
        ///     Calcula la transformacion que permite que el rectangulo <c>rec</c> se centre en el punto
        ///     <c>punto</c>.
        /// </summary>
        /// <param name="orig">Rectangulo.</param>
        /// <param name="punto">Posicion.</param>
        public static Matrix3x3d Center(Rectangle2d orig, Point2d punto)
        {
            // Calcula la transformacion.
            return Translate(
                punto.X - orig.X - orig.DY / 2,
                punto.Y - orig.Y - orig.DY / 2);
        }

        /// <summary>
        ///     Calcula la transformacion que permite que el rectangulo <c>orig</c> se ajuste
        ///     al rectangulo <c>dest</c>.
        /// </summary>
        /// <param name="orig">Rectangulo origen.</param>
        /// <param name="dest">Rectangulo destino.</param>
        /// <param name="deformar">Indica si se permiten deformaciones.</param>
        public static Matrix3x3d AdjustTo(Rectangle2d orig, Rectangle2d dest, bool deformar)
        {
            REAL sx = dest.DX / orig.DX;
            REAL sy = dest.DY / orig.DY;

            // Calcula la transformacion.
            Matrix3x3d t;
            if (deformar || sx.EpsilonEquals(sy)) // Se puede deformar o las escalas son iguales.
            {
                t = TransformRectangle(
                    orig,
                    new Point2d(dest.X, dest.Y),
                    new Point2d(dest.X + dest.DX, dest.Y),
                    new Point2d(dest.X, dest.Y + dest.DY));
            }
            else if (sx > sy) // Hay mas escala en X que en Y.
            {
                dest = new Rectangle2d(
                    dest.X + (dest.DX - orig.DX * sy) / 2, dest.Y,
                    orig.DX * sy, dest.DY);

                t = TransformRectangle(
                    orig,
                    new Point2d(dest.X, dest.Y),
                    new Point2d(dest.X + dest.DX, dest.Y),
                    new Point2d(dest.X, dest.Y + dest.DY));
            }
            else // Hay mas escala en Y que en X.
            {
                dest = new Rectangle2d(
                    dest.X, dest.Y + (dest.DY - orig.DY * sx) / 2,
                    dest.DX, orig.DY * sx);

                t = TransformRectangle(
                    orig,
                    new Point2d(dest.X, dest.Y),
                    new Point2d(dest.X + dest.DX, dest.Y),
                    new Point2d(dest.X, dest.Y + dest.DY));
            }

            return t;
        }
    }
}