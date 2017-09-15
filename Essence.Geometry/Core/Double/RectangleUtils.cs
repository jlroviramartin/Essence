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
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    public static class RectangleUtils
    {
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

            double m00 = recDst.DX / recOrig.DX;
            double m10 = 0;
            double m01 = 0;
            double m11 = recDst.DY / recOrig.DY;

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

            double m00 = (pd1.X - pd0.X) / recOrig.DX;
            double m10 = (pd1.Y - pd0.Y) / recOrig.DX;
            double m01 = (pd2.X - pd0.X) / recOrig.DY;
            double m11 = (pd2.Y - pd0.Y) / recOrig.DY;

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

            double m00 = (po1.X - po0.X) / recDst.DX;
            double m10 = (po1.Y - po0.Y) / recDst.DX;
            double m01 = (po2.X - po0.X) / recDst.DY;
            double m11 = (po2.Y - po0.Y) / recDst.DY;

            double det = 1 / (m00 * m11 - m01 * m10);

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
                double m00 = (pd1.X - pd0.X);
                double m10 = (pd1.Y - pd0.Y);
                double m01 = (pd2.X - pd0.X);
                double m11 = (pd2.Y - pd0.Y);
                m = new Matrix3x3d(
                    m00, m01, pd0.X,
                    m10, m11, pd0.Y,
                    0, 0, 1);
            }
            ////////////////////////////////////////////////////////////////////////////////////////
            //MATRIX aux = TransformRectangle(rec, po0, po1, po2);
            Matrix3x3d aux;
            {
                double m00 = (po1.X - po0.X);
                double m10 = (po1.Y - po0.Y);
                double m01 = (po2.X - po0.X);
                double m11 = (po2.Y - po0.Y);

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
            return Matrix3x3dUtils.Translate(
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
            double sx = dest.DX / orig.DX;
            double sy = dest.DY / orig.DY;

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