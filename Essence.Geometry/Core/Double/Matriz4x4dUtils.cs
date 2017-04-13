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
using Essence.Util.Math.Double;
using REAL = System.Double;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    ///     Utilidades para las matrices <c>Matriz3x3d</c>.
    ///     <see cref="http://arantxa.ii.uam.es/~pedro/graficos/teoria/Transf3D/Transf3D.htm" />
    ///     <see cref="http://arantxa.ii.uam.es/~pedro/graficos/teoria/Proyecciones/Proyecciones.htm" />
    ///     <see cref="http://arantxa.ii.uam.es/~pedro/graficos/teoria/View3D/View3D.htm" />
    ///     <see cref="http://arantxa.ii.uam.es/~pedro/graficos/teoria/Clip3D/Clip3D.htm" />
    /// </summary>
    public static class Matriz4x4dUtils
    {
        /// <summary>
        ///     <see cref="http://adndevblog.typepad.com/autocad/2012/08/remove-scaling-from-transformation-matrix.html" />
        /// </summary>
        public static Matrix4x4d RemoveScaling(Matrix4x4d m, double scalingValue = 1)
        {
            Vector3d v0 = new Vector3d(m.M00, m.M01, m.M02).Unit.Mul(scalingValue);
            Vector3d v1 = new Vector3d(m.M10, m.M11, m.M12).Unit.Mul(scalingValue);
            Vector3d v2 = new Vector3d(m.M20, m.M21, m.M22).Unit.Mul(scalingValue);

            return new Matrix4x4d(
                new[,]
                {
                    { v0.X, v0.Y, v0.Z, m.M02 },
                    { v1.X, v1.Y, v1.Z, m.M12 },
                    { v2.X, v2.Y, v2.Z, m.M22 },
                    { m.M30, m.M31, m.M32, m.M33 }
                });
        }

        #region Translate, rotate, scale

        public static Matrix4x4d TranslateRotateScale(Vector3d t, Vector4d q, Vector3d e)
        {
            Matrix4x4d mt = Translate(t.X, t.Y, t.Z);
            Matrix4x4d mr = RotateQuat(q);
            Matrix4x4d me = Scale(e.X, e.Y, e.Z);

            // mt * mr * me
            Matrix4x4d m = mt;
            m.Mul(mr);
            m.Mul(me);
            return m;
        }

        /// <summary>
        ///     Crea una matriz con la traslacion, rotacion y escala.
        /// </summary>
        /// <param name="t">Traslacion.</param>
        /// <param name="r">Rotacion en radianes.</param>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz de escala, rotacion y traslacion.</returns>
        public static Matrix4x4d TranslateRotateScale(Vector3d t, Vector3d r, Vector3d e)
        {
            return TranslateRotateScale(
                t.X, t.Y, t.Z,
                r.X, r.Y, r.Z,
                e.X, e.Y, e.Z);
        }

        /// <summary>
        ///     Crea una matriz con la traslacion, rotacion y escala.
        /// </summary>
        /// <param name="tx">Traslacion x.</param>
        /// <param name="ty">Traslacion y.</param>
        /// <param name="tz">Traslacion z.</param>
        /// <param name="rx">Angulo en radianes x respecto al plano YZ.</param>
        /// <param name="ry">Angulo en radianes y respecto al plano XZ.</param>
        /// <param name="rz">Angulo en radianes z respecto al plano XY.</param>
        /// <param name="ex">Escala x.</param>
        /// <param name="ey">Escala y.</param>
        /// <param name="ez">Escala z.</param>
        /// <returns>Matriz de escala, rotacion y traslacion.</returns>
        public static Matrix4x4d TranslateRotateScale(
            double tx, double ty, double tz,
            double rx, double ry, double rz,
            double ex, double ey, double ez)
        {
            Matrix4x4d mt = Translate(tx, ty, tz);
            Matrix4x4d mr = Rotate(rx, ry, rz);
            Matrix4x4d me = Scale(ex, ey, ez);

            // mt * mr * me
            Matrix4x4d m = mt;
            m.Mul(mr);
            m.Mul(me);
            return m;
        }

        #endregion

        #region Translate

        /// <summary>
        ///     Crea una matriz con la traslacion.
        /// </summary>
        /// <param name="t">Traslacion.</param>
        /// <returns>Matriz de traslacion.</returns>
        public static Matrix4x4d Translate(Vector3d t)
        {
            return Translate(t.X, t.Y, t.Z);
        }

        /// <summary>
        ///     Crea una matriz con la traslacion.
        /// </summary>
        /// <param name="tx">Traslacion x.</param>
        /// <param name="ty">Traslacion y.</param>
        /// <param name="tz">Traslacion z.</param>
        /// <returns>Matriz de traslacion.</returns>
        public static Matrix4x4d Translate(double tx, double ty, double tz)
        {
            return new Matrix4x4d(
                1, 0, 0, tx,
                0, 1, 0, ty,
                0, 0, 1, tz,
                0, 0, 0, 1);
        }

        #endregion

        /// <summary>
        ///     Crea una matriz a la que se le han invertido los ejes.
        /// </summary>
        /// <param name="axis">Indica que ejes invierte.</param>
        /// <returns>Matriz.</returns>
        public static Matrix4x4d InvertAxis(Axis axis)
        {
            bool x = ((axis & Axis.X) != 0);
            bool y = ((axis & Axis.Y) != 0);
            bool z = ((axis & Axis.Z) != 0);
            return new Matrix4x4d(
                x ? -1 : 1, 0, 0, 0,
                0, y ? -1 : 1, 0, 0,
                0, 0, z ? -1 : 1, 0,
                0, 0, 0, 1);
        }

        #region Scale

        /// <summary>
        ///     Crea una matriz con la escala.
        /// </summary>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz de escala.</returns>
        public static Matrix4x4d Scale(Vector3d e)
        {
            return Scale(e.X, e.Y, e.Z);
        }

        /// <summary>
        ///     Crea una matriz con la escala respecto a un punto de aplicacion.
        /// </summary>
        /// <param name="p">Punto de aplicacion.</param>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix4x4d Scale(Point3d p, Vector3d e)
        {
            return Scale(p.X, p.Y, p.Z, e.X, e.Y, e.Z);
        }

        /// <summary>
        ///     Crea una matriz con la escala.
        /// </summary>
        /// <param name="ex">Escala x.</param>
        /// <param name="ey">Escala y.</param>
        /// <param name="ez">Escala z.</param>
        /// <returns>Matriz de escala.</returns>
        public static Matrix4x4d Scale(double ex, double ey, double ez)
        {
            return new Matrix4x4d(
                ex, 0, 0, 0,
                0, ey, 0, 0,
                0, 0, ez, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        ///     Crea una matriz con la escala respecto a un punto de aplicacion.
        /// </summary>
        /// <param name="px">Punto de aplicacion X.</param>
        /// <param name="py">Punto de aplicacion Y.</param>
        /// <param name="pz">Punto de aplicacion Z.</param>
        /// <param name="ex">Escala x.</param>
        /// <param name="ey">Escala y.</param>
        /// <param name="ez">Escala z.</param>
        /// <returns>Matriz escala.</returns>
        public static Matrix4x4d Scale(double px, double py, double pz, double ex, double ey, double ez)
        {
            return new Matrix4x4d(
                ex, 0, 0, px - ex * px,
                0, ey, 0, py - ey * py,
                0, 0, ez, pz - ez * pz,
                0, 0, 0, 1);
        }

        #endregion

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
        public static Matrix4x4d RotateEuler(double heading, double attitude, double bank)
        {
            Matrix3x3d m = Matrix3x3dUtils.RotateEuler(heading, attitude, bank);
            return new Matrix4x4d(
                m.M00, m.M01, m.M02, 0,
                m.M10, m.M11, m.M12, 0,
                m.M20, m.M21, m.M22, 0,
                0, 0, 0, 1);
        }

        public static Matrix4x4d RotateQuat(Vector4d v)
        {
            return RotateQuat(v.X, v.Y, v.Z, v.W);
        }

        public static Matrix4x4d RotateQuat(double x, double y, double z, double w)
        {
            Matrix3x3d m = Matrix3x3dUtils.RotateQuat(x, y, z, w);
            return new Matrix4x4d(
                m.M00, m.M01, m.M02, 0,
                m.M10, m.M11, m.M12, 0,
                m.M20, m.M21, m.M22, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        ///     Crea una matriz con la rotacion.
        /// </summary>
        /// <param name="r">Angulo en radianes.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix4x4d Rotate(Vector3d r)
        {
            return Rotate(r.X, r.Y, r.Z);
        }

        /// <summary>
        ///     Crea una matriz con la rotacion.
        /// </summary>
        /// <param name="rx">Angulo en radianes x respecto al plano YZ.</param>
        /// <param name="ry">Angulo en radianes y respecto al plano XZ.</param>
        /// <param name="rz">Angulo en radianes z respecto al plano XY.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix4x4d Rotate(double rx, double ry, double rz)
        {
            double ca = (double)Math.Cos(rx);
            double sa = (double)Math.Sin(rx);

            double cb = (double)Math.Cos(ry);
            double sb = (double)Math.Sin(ry);

            double cg = (double)Math.Cos(rz);
            double sg = (double)Math.Sin(rz);

            return new Matrix4x4d(
                cb * cg, sa * sb * cg - ca * sg, ca * sb * cg + sa * sg, 0,
                cb * sg, sa * sb * sg + ca * cg, ca * sb * sg - sa * cg, 0,
                -sb, sa * cb, ca * cb, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        ///     Crea una matriz con la rotacion.
        /// </summary>
        /// <param name="axis">Eje de rotacion (eje X = 0, Y = 1, Z = 2).</param>
        /// <param name="r">Angulo en radianes.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix4x4d Rotate(Axis axis, double r)
        {
            double c = (double)Math.Cos(r);
            double s = (double)Math.Sin(r);

            switch (axis)
            {
                case Axis.X:
                {
                    return new Matrix4x4d(
                        1, 0, 0, 0,
                        0, c, -s, 0,
                        0, s, c, 0,
                        0, 0, 0, 1);
                }
                case Axis.Y:
                {
                    return new Matrix4x4d(
                        c, 0, s, 0,
                        0, 1, 0, 0,
                        -s, 0, c, 0,
                        0, 0, 0, 1);
                }
                case Axis.Z:
                {
                    return new Matrix4x4d(
                        c, -s, 0, 0,
                        s, c, 0, 0,
                        0, 0, 1, 0,
                        0, 0, 0, 1);
                }
                default:
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Crea una matriz con la rotacion.
        /// </summary>
        /// <param name="v">VECTOR3 de rotacion.</param>
        /// <param name="r">Angulo en radianes.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix4x4d Rotate(Vector3d v, double r)
        {
            return Rotate(v.X, v.Y, v.Z, r);
        }

        /// <summary>
        ///     Crea una matriz con la rotacion.
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        /// <param name="z">Z.</param>
        /// <param name="r">Angulo en radianes.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix4x4d Rotate(double x, double y, double z, double r)
        {
            // Taken from Rick's which is taken from Wertz. pg. 412
            // Bug Fixed and changed into right-handed by hiranabe
            double n = (double)Math.Sqrt(x * x + y * y + z * z);
            // zero-div may occur
            n = 1 / n;
            x *= n;
            y *= n;
            z *= n;

            double c = (double)Math.Cos(r);
            double s = (double)Math.Sin(r);
            double umc = 1 - c;

            double m00 = x * x * umc + c;
            double m11 = y * y * umc + c;
            double m22 = z * z * umc + c;

            double xyumc = x * y * umc;
            double zs = z * s;
            double m01 = xyumc - zs;
            double m10 = xyumc + zs;

            double xzumc = x * z * umc;
            double ys = y * s;
            double m02 = xzumc + ys;
            double m20 = xzumc - ys;

            double yzumc = y * z * umc;
            double xs = x * s;
            double m12 = yzumc - xs;
            double m21 = yzumc + xs;

            return new Matrix4x4d(
                m00, m01, m02, 0,
                m10, m11, m12, 0,
                m20, m21, m22, 0,
                0, 0, 0, 1);
        }

        #endregion

        #region Extrude

        /// <summary cref="Extrude(REAL,REAL,REAL,REAL)" />
        /// <param name="vZ">Vector normal.</param>
        /// <returns>Matriz de extrusion.</returns>
        public static Matrix4x4d Extrude(Vector3d vZ)
        {
            return Extrude(vZ.X, vZ.Y, vZ.Z, 0);
        }

        /// <summary cref="Extrude(REAL,REAL,REAL,REAL)" />
        /// <param name="a">Coordenada X del vector normal.</param>
        /// <param name="b">Coordenada Y del vector normal.</param>
        /// <param name="c">Coordenada Z del vector normal.</param>
        /// <returns>Matriz de extrusion.</returns>
        public static Matrix4x4d Extrude(double a, double b, double c)
        {
            return Extrude(a, b, c, 0);
        }

        /// <summary cref="Extrude(REAL,REAL,REAL,REAL)" />
        /// <param name="vZ">Vector normal.</param>
        /// <param name="d">Distancia minima del plano al origen.</param>
        /// <returns>Matriz de extrusion.</returns>
        public static Matrix4x4d Extrude(Vector3d vZ, double d)
        {
            return Extrude(vZ.X, vZ.Y, vZ.Z, d);
        }

        /// <summary>
        ///     Crea una matriz con la extrusion. Una extrusion representa un plano
        ///     sobre el que se apoya el elemento. El plano se define a través del
        ///     vector normal y la distancia minima del plano al origen.
        ///     Ecuación implícita/normal del plano: ax + by + cz + d = 0.
        /// </summary>
        /// <param name="a">Coordenada X del vector normal.</param>
        /// <param name="b">Coordenada Y del vector normal.</param>
        /// <param name="c">Coordenada Z del vector normal.</param>
        /// <param name="d">Distancia minima del plano al origen.</param>
        /// <returns>Matriz de extrusion.</returns>
        public static Matrix4x4d Extrude(double a, double b, double c, double d)
        {
            if (a.EpsilonEquals(0) && b.EpsilonEquals(0) && c.EpsilonEquals(1))
            {
                return Translate(0, 0, d);
            }

            // Vector eje Z normalizado.
            Vector3d vZ = new Vector3d(a, b, c);
            double m = 1 / vZ.Length;
            vZ = vZ.Mul(m);

            // Origen.
            Point3d o = (Point3d)(vZ * (d * m));

            // Vector eje X normalizado.
            // Se comprueba si vZ.X e vZ.Y son casi cero.
            Vector3d vX;
            if (vZ.X.EpsilonEquals(0) && vZ.Y.EpsilonEquals(0))
            {
                vX = new Vector3d(vZ.Z, 0, -vZ.X).Unit;
            }
            else
            {
                vX = new Vector3d(-vZ.Y, vZ.X, 0).Unit;
            }

            // Vector eje Y normalizado.
            Vector3d vY = vZ.Cross(vX).Unit;

            return new Matrix4x4d(vX, vY, vZ, o);
        }

        #endregion

        public static Matrix4x4d LookAt(Point3d eye, Point3d center, Vector3d up)
        {
            return LookAt(eye, center - eye, up);
        }

        /// <summary>
        ///     Crea una matriz que coloca la camara en <c>posicion</c> mirando en direccion <c>dir</c>.
        ///     La direccion <c>up</c> marca la coordenada Y.
        /// </summary>
        /// <param name="eye">Posicion de la camara.</param>
        /// <param name="dir">Direccion a la que mira la camara.</param>
        /// <param name="up">Direccion Y de la camara.</param>
        /// <returns></returns>
        public static Matrix4x4d LookAt(Point3d eye, Vector3d dir, Vector3d up)
        {
            Vector3d fN = dir.Unit;
            Vector3d upN = up.Unit;

            Vector3d sN = fN.Cross(upN);
            Vector3d uN = sN.Cross(fN);

            Matrix4x4d m = new Matrix4x4d(
                sN.X, sN.Y, sN.Z, 0,
                uN.X, uN.Y, uN.Z, 0,
                -fN.X, -fN.Y, -fN.Z, 0,
                0, 0, 0, 1);
            m.Mul(Translate(eye.X, eye.Y, eye.Z));
            return m;
        }

        /// <summary>
        ///     Crea una matriz de proyeccion ortografica.
        /// </summary>
        /// <param name="xMin">Plano X de corte (> 0).</param>
        /// <param name="xMax">Plano X de corte (> 0).</param>
        /// <param name="yMin">Plano Y de corte (> 0).</param>
        /// <param name="yMax">Plano Y de corte (> 0).</param>
        /// <param name="zMin">Plano Z de corte (> 0).</param>
        /// <param name="zMax">Plano Z de corte (> 0).</param>
        /// <returns></returns>
        public static Matrix4x4d Orto(double xMin, double xMax,
                                      double yMin, double yMax,
                                      double zMin, double zMax)
        {
            double m00 = 2 / (xMax - xMin);
            double m11 = 2 / (yMax - yMin);
            double m22 = -2 / (zMax - zMin);

            double m03 = -(xMax + xMin) / (xMax - xMin);
            double m13 = -(yMax + yMin) / (yMax - yMin);
            double m23 = -(zMax + zMin) / (zMax - zMin);

            return new Matrix4x4d(
                new[,]
                {
                    { m00, 0, 0, m03 },
                    { 0, m11, 0, m13 },
                    { 0, 0, m22, m23 },
                    { 0, 0, 0, 1 }
                });
        }

        /// <summary>
        ///     Crea una matriz inversa de proyeccion ortografica.
        /// </summary>
        /// <param name="xMin">Plano X de corte (> 0).</param>
        /// <param name="xMax">Plano X de corte (> 0).</param>
        /// <param name="yMin">Plano Y de corte (> 0).</param>
        /// <param name="yMax">Plano Y de corte (> 0).</param>
        /// <param name="zMin">Plano Z de corte (> 0).</param>
        /// <param name="zMax">Plano Z de corte (> 0).</param>
        /// <returns></returns>
        public static Matrix4x4d InvOrto(double xMin, double xMax,
                                         double yMin, double yMax,
                                         double zMin, double zMax)
        {
            double m00 = (xMax - xMin) / 2;
            double m03 = (xMax + xMin) / 2;

            double m11 = (yMax - yMin) / 2;
            double m13 = (yMax + yMin) / 2;

            double m22 = -(zMax - zMin) / 2;
            double m23 = (zMax + zMin) / 2;

            return new Matrix4x4d(
                new[,]
                {
                    { m00, 0, 0, m03 },
                    { 0, m11, 0, m13 },
                    { 0, 0, m22, m23 },
                    { 0, 0, 0, 1 }
                });
        }

        /// <summary>
        ///     Crea una matriz de proyeccion de prespectiva.
        /// </summary>
        /// <param name="campoDeVistaY">Angulo del campo de vista en radianes en la direccion Y.</param>
        /// <param name="aspecto">Ratio de aspecto que determina la direccion X (ancho / alto).</param>
        /// <param name="zMin">Distancia del punto de vista al plano de corte mas cercano (> 0).</param>
        /// <param name="zMax">Distancia del punto de vista al plano de corte mas lejano (> 0).</param>
        public static Matrix4x4d Frustum(double campoDeVistaY,
                                         double aspecto,
                                         double zMin, double zMax)
        {
            double range = zMin * (double)Math.Tan(campoDeVistaY / 2);

            return Frustum(-range * aspecto, range * aspecto,
                           -range, range,
                           zMin, zMax);
        }

        /// <summary>
        ///     Crea una matriz de proyeccion de prespectiva.
        /// </summary>
        /// <param name="xMin">Plano X de corte (> 0).</param>
        /// <param name="xMax">Plano X de corte (> 0).</param>
        /// <param name="yMin">Plano Y de corte (> 0).</param>
        /// <param name="yMax">Plano Y de corte (> 0).</param>
        /// <param name="zMin">Plano Z de corte (> 0).</param>
        /// <param name="zMax">Plano Z de corte (> 0).</param>
        /// <returns></returns>
        public static Matrix4x4d Frustum(double xMin, double xMax,
                                         double yMin, double yMax,
                                         double zMin, double zMax)
        {
            double m00 = (2 * zMin) / (xMax - xMin);
            double m02 = (xMax + xMin) / (xMax - xMin);

            double m11 = (2 * zMin) / (yMax - yMin);
            double m12 = (yMax + yMin) / (yMax - yMin);

            double m22 = -(zMax + zMin) / (zMax - zMin);
            double m23 = -(2 * zMax * zMin) / (zMax - zMin);

            return new Matrix4x4d(
                new[,]
                {
                    { m00, 0, m02, 0 },
                    { 0, m11, m12, 0 },
                    { 0, 0, m22, m23 },
                    { 0, 0, -1, 0 }
                });
        }

        /// <summary>
        ///     Crea una matriz inversa de proyeccion de prespectiva.
        /// </summary>
        /// <param name="xMin">Plano X de corte (> 0).</param>
        /// <param name="xMax">Plano X de corte (> 0).</param>
        /// <param name="yMin">Plano Y de corte (> 0).</param>
        /// <param name="yMax">Plano Y de corte (> 0).</param>
        /// <param name="zMin">Plano Z de corte (> 0).</param>
        /// <param name="zMax">Plano Z de corte (> 0).</param>
        /// <returns></returns>
        public static Matrix4x4d InvFrustum(double xMin, double xMax,
                                            double yMin, double yMax,
                                            double zMin, double zMax)
        {
            double m00 = (xMax - xMin) / (2 * zMin);
            double m03 = (xMax + xMin) / (2 * zMin);

            double m11 = (yMax - yMin) / (2 * zMin);
            double m13 = (yMax + yMin) / (2 * zMin);

            double m32 = -(zMax - zMin) / (2 * zMax * zMin);
            double m33 = (zMax + zMin) / (2 * zMax * zMin);

            return new Matrix4x4d(
                new[,]
                {
                    { m00, 0, 0, m03 },
                    { 0, m11, 0, m13 },
                    { 0, 0, 0, -1 },
                    { 0, 0, m32, m33 }
                });
        }
    }
}