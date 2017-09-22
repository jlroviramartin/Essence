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
        public static Matrix3x3d RemoveScaling(Matrix3x3d m, double scalingValue = 1)
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
        public static Matrix3x3d TranslateRotateScale(Vector2d t, double r, Vector2d e)
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
        public static Matrix3x3d TranslateRotateScale(Point2d o, double r, Vector2d e)
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
        public static Matrix3x3d TranslateRotateScale(double tx, double ty, double r, double ex, double ey)
        {
            double s = Math.Sin(r);
            double c = Math.Cos(r);
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
        public static Matrix3x3d Translate(double tx, double ty)
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
        public static Matrix3x3d RotateEuler(double heading, double attitude, double bank)
        {
            // Assuming the angles are in radians.
            double ch = Math.Cos(heading);
            double sh = Math.Sin(heading);
            double ca = Math.Cos(attitude);
            double sa = Math.Sin(attitude);
            double cb = Math.Cos(bank);
            double sb = Math.Sin(bank);
            return new Matrix3x3d(
                ch * ca, sh * sb - ch * sa * cb, ch * sa * sb + sh * cb,
                sa, ca * cb, -ca * sb,
                -sh * ca, sh * sa * cb + ch * sb, -sh * sa * sb + ch * cb);
        }

        public static Matrix3x3d RotateQuat(Vector4d v)
        {
            return RotateQuat(v.X, v.Y, v.Z, v.W);
        }

        public static Matrix3x3d RotateQuat(double x, double y, double z, double w)
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
        public static Matrix3x3d Rotate(double r)
        {
            double c = Math.Cos(r);
            double s = Math.Sin(r);
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
        public static Matrix3x3d Rotate(Point2d p, double r)
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
        public static Matrix3x3d Rotate(double px, double py, double r)
        {
            double c = Math.Cos(r);
            double s = Math.Sin(r);
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
        public static Matrix3x3d Scale(double e)
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
        public static Matrix3x3d Scale(double ex, double ey)
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
        public static Matrix3x3d Scale(Point2d p, double e)
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
        public static Matrix3x3d Scale(double px, double py, double e)
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
        public static Matrix3x3d Scale(double px, double py, double ex, double ey)
        {
            return new Matrix3x3d(
                ex, 0, px - ex * px,
                0, ey, py - ey * py,
                0, 0, 1);
        }

        #endregion
    }
}