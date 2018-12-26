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
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Geom3D
{
    public class Plane3d
    {
        /// <summary>
        /// Plano con los vectores ortonormalizados.
        /// </summary>
        public static Plane3d NewOrthonormal(Point3d origin, Vector3d dx, Vector3d dy)
        {
            // Se calcula el vector vx.
            Vector3d vx = dx.Unit;

            // Se calcula el vector vy.
            double y = vx.Dot(dy); // Mas eficiente que 'Proyectar', 'vx' ya es unitario
            Vector3d vy = (dy - vx * y).Unit;

            Plane3d plane = new Plane3d(origin, vx, vy);

            if (vx.IsZero || vy.IsZero)
            {
                plane.evaluated |= Evaluated.IsDegenerate;
                plane.isDegenerate = true;
            }
            else
            {
                plane.evaluated |= Evaluated.IsOrthonormal;
                plane.isOrthonormal = true;
            }

            return plane;
        }

        /// <summary>
        /// Plano con los vectores ortonormalizados.
        /// </summary>
        public static Plane3d NewOrthonormal(Point3d p0, Point3d p1, Point3d p2)
        {
            return NewOrthonormal(p0, p1 - p0, p2 - p0);
        }

        /// <summary>
        /// Plano con los vectores sin ortonormalizar.
        /// </summary>
        public static Plane3d NewNonOrthonormal(Point3d origin, Vector3d dx, Vector3d dy)
        {
            return new Plane3d(origin, dx, dy);
        }

        /// <summary>
        /// Plano con los vectores sin ortonormalizar.
        /// </summary>
        public static Plane3d NewNonOrthonormal(Point3d p0, Point3d p1, Point3d p2)
        {
            return NewNonOrthonormal(p0, p1 - p0, p2 - p0);
        }

        // ?????
        public static Plane3d NewPointDirection(Point3d o, Vector3d normal)
        {
            double c = o.X * normal.X + o.Y * normal.Y + o.Z * normal.Z;
            Vector3d vz = normal.Unit;
            int num = Maximum(Math.Abs(normal.X), Math.Abs(normal.Y), Math.Abs(normal.Z));
            switch (num)
            {
                case 0:
                {
                    Func<double, double, Point3d> func = ((y, z) => new Point3d((c - (y * vz.Y + z * vz.Z)) / vz.X, y, z));
                    Vector3d unit = (func(1.0, 0.0) - func(0.0, 0.0)).Unit;
                    Vector3d dy = vz.Cross(unit);
                    return new Plane3d(o, unit, dy);
                }
                case 1:
                {
                    Func<double, double, Point3d> func = ((x, z) => new Point3d(x, (c - (x * vz.X + z * vz.Z)) / vz.Y, z));
                    Vector3d unit = (func(1.0, 0.0) - func(0.0, 0.0)).Unit;
                    Vector3d dy = vz.Cross(unit);
                    return new Plane3d(o, unit, dy);
                }
                case 2:
                {
                    Func<double, double, Point3d> func = ((x, y) => new Point3d(x, y, (c - (x * vz.X + y * vz.Y)) / vz.Z));
                    Vector3d unit = (func(1.0, 0.0) - func(0.0, 0.0)).Unit;
                    Vector3d dy = vz.Cross(unit);
                    return new Plane3d(o, unit, dy);
                }
                default:
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        private static int Maximum(double x, double y, double z)
        {
            if (x >= y)
            {
                if (x >= z)
                {
                    return 0; // x
                }
                return 2; // z
            }
            else
            {
                if (y >= z)
                {
                    return 1; // y
                }
                return 2; // z
            }
        }

        /// <summary>
        /// Indica si esta degenerado: algun vector direccion es zero o son paralelos.
        /// </summary>
        public bool IsDegenerate
        {
            get
            {
                if ((this.evaluated & Evaluated.IsDegenerate) != Evaluated.IsDegenerate)
                {
                    this.evaluated |= Evaluated.IsDegenerate;

                    this.isDegenerate = this.DX.IsZero || this.DY.IsZero || this.DX.Cross(this.DY).LengthSquared.EpsilonEquals(0);
                }
                return this.isDegenerate;
            }
        }

        /// <summary>
        /// Indica si esta ortonormalizado: los vectores direccion son unitarios y perpendiculares.
        /// </summary>
        public bool IsOrthonormal
        {
            get
            {
                if ((this.evaluated & Evaluated.IsOrthonormal) != Evaluated.IsOrthonormal)
                {
                    this.evaluated |= Evaluated.IsOrthonormal;

                    this.isOrthonormal = this.DX.IsUnit && this.DY.IsUnit && this.DX.Dot(this.DY).EpsilonEquals(0);
                }
                return this.isOrthonormal;
            }
        }

        public Point3d Origin { get; private set; }

        public Vector3d DX { get; private set; }

        public Vector3d DY { get; private set; }

        /// <summary>
        /// Obtiene el vector normal normalizado.
        /// </summary>
        public Vector3d Normal
        {
            get
            {
                if ((this.evaluated & Evaluated.Normal) != Evaluated.Normal)
                {
                    this.evaluated |= Evaluated.Normal;

                    this.normal = this.DX.Cross(this.DY);
                    if (!this.IsOrthonormal)
                    {
                        this.normal = this.normal.Unit;
                    }
                }
                return this.normal;
            }
        }

        /// <summary>
        /// Constante en la ecuacion del plano.
        /// nx*x + ny*y + nz*z = c
        /// </summary>
        public double Constant
        {
            get
            {
                if ((this.evaluated & Evaluated.Constant) != Evaluated.Constant)
                {
                    this.evaluated |= Evaluated.Constant;

                    this.constant = this.Normal.Dot(this.Origin);
                }
                return this.constant;
            }
        }

        /// <summary>
        /// Evalua la proyeccion del punto sobre el plano, respecto del origen.
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <returns>Parametro [-Inf, Inf].</returns>
        public double[] Project(Point3d p)
        {
            Vector3d diff = (p - this.Origin);

            double u, v;
            if (this.IsOrthonormal)
            {
                //REAL w = this.Normal.Dot(diff);
                u = this.DX.Dot(diff);
                v = this.DY.Dot(diff);
            }
            else
            {
                double w;
                Geom3DUtils.Resolve(this.DX, this.DY, this.Normal, diff, out u, out v, out w);
            }
            return new[] { u, v };
        }

        /// <summary>
        /// Evalua el punto.
        /// </summary>
        /// <param name="u">Parametro [-Inf, Inf].</param>
        /// <param name="v">Parametro [-Inf, Inf].</param>
        /// <returns>Punto.</returns>
        public Point3d Evaluate(double u, double v)
        {
            return this.Origin + u * this.DX + v * this.DY;
        }

        #region Distancia

        /// <summary>
        /// Distancia (con signo) de un punto a la linea.
        /// </summary>
        public double Distance(Point3d p, out Point3d closestPoint)
        {
            Vector3d normal = this.Normal;
            double c = this.Constant;

            //REAL signedDistance = normal.Dot((VECTOR)p) - c = normal.Dot(p - this.Origin) = normal.Dot(p) - normal.Dot(this.Origin);
            double signedDistance = normal.X * p.X + normal.Y * p.Y + normal.Z * p.Z - c;
            closestPoint = p - signedDistance * normal;
            return signedDistance;
        }

        /// <summary>
        /// Distancia (con signo) de un punto a la linea.
        /// </summary>
        public double Distance(Point3d p)
        {
            Vector3d normal = this.Normal;
            double c = this.Constant;

            //REAL signedDistance = normal.Dot((VECTOR)p) - c = normal.Dot(p - this.Origin) = normal.Dot(p) - normal.Dot(this.Origin);
            double signedDistance = normal.X * p.X + normal.Y * p.Y + normal.Z * p.Z - c;
            return signedDistance;
        }

        /// <summary>
        /// Indica a que lado esta el punto respecto de la linea:
        /// - Si == 0, esta en la linea.
        /// - Si &gt; 0 esta debajo/derecha de la linea.
        /// - Si &lt; 0 esta encima/izquierda de la linea.
        /// <c><![CDATA[
        ///             |
        ///             |
        ///   (-)       +-----> (+) normal
        ///   back      |       front
        ///             |
        /// ]]></c>
        /// </summary>
        public PlaneSide WhichSide(Point3d p)
        {
            double distance = this.Distance(p);

            if (distance.EpsilonZero())
            {
                return PlaneSide.Middle;
            }
            else if (distance > 0)
            {
                // Derecha
                return PlaneSide.Front;
            }
            else // if (distance < 0)
            {
                // Izquierda
                return PlaneSide.Back;
            }
        }

        #endregion

        #region private

        private Plane3d(Point3d origin, Vector3d dx, Vector3d dy)
        {
            this.Origin = origin;
            this.DX = dx;
            this.DY = dy;
        }

        private Vector3d normal;
        private double constant;
        private bool isOrthonormal;
        private bool isDegenerate;
        private Evaluated evaluated = Evaluated.None;

        #endregion

        #region inner classes

        [Flags]
        private enum Evaluated
        {
            None = 0x0,
            Normal = 0x1,
            Constant = 0x2,
            IsOrthonormal = 0x4,
            IsDegenerate = 0x8,
        }

        #endregion
    }
}