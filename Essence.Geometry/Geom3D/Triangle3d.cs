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
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using REAL = System.Double;

namespace Essence.Geometry.Geom3D
{
    /// <summary>
    ///     <see cref="http://toxiclibs.org/docs/core/toxi/geom/Triangle3D.html" />
    ///     <see cref="http://en.wikipedia.org/wiki/Barycentric_coordinate_system" />
    ///     <see cref="http://mathworld.wolfram.com/BarycentricCoordinates.html" />
    ///     <see
    ///         cref="http://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates" />
    ///     <see cref="https://en.wikipedia.org/wiki/Trilinear_coordinates" />
    ///     <see cref="http://mathworld.wolfram.com/TrilinearCoordinates.html" />
    ///     <see cref="https://groups.google.com/forum/#!topic/geometry.research/ItbsZgvaAvo" />
    ///     <see cref="http://mathforum.org/kb/message.jspa?messageID=1091956" />
    /// </summary>
    public class Triangle3d
    {
        public Triangle3d(Point3d p0, Point3d p1, Point3d p2)
        {
            this.P0 = p0;
            this.P1 = p1;
            this.P2 = p2;
        }

        public readonly Point3d P0;
        public readonly Point3d P1;
        public readonly Point3d P2;

        public Vector3d VX
        {
            get { return this.P1 - this.P0; }
        }

        public Vector3d VY
        {
            get { return this.P2 - this.P0; }
        }

        /// <summary>
        ///     Obtiene el vertice con indice <c>i</c>.
        /// </summary>
        /// <param name="i">Indice del vertice.</param>
        /// <returns>Vertice.</returns>
        public Point3d this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                    {
                        return this.P0;
                    }
                    case 1:
                    {
                        return this.P1;
                    }
                    case 2:
                    {
                        return this.P2;
                    }
                    default:
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
            }
        }

        /// <summary>
        ///     Indica si el triangulo es valido: todos sus puntos son validos.
        /// </summary>
        public bool IsValid
        {
            get { return this.P0.IsValid && this.P1.IsValid && this.P2.IsValid; }
        }

        /// <summary>
        ///     Indica si el triangulo es degenerado: recta o punto.
        /// </summary>
        public bool IsDegenerate
        {
            get { return this.Normal.IsZero; }
        }

        /// <summary>
        ///     Area del triangulo.
        /// </summary>
        public REAL Area
        {
            get
            {
                if ((this.evaluated & Evaluated.Area) == Evaluated.None)
                {
                    this.evaluated |= Evaluated.Area;

                    // https://en.wikipedia.org/wiki/Cross_product#Properties
                    REAL l = this.VX.Cross(this.VY).Length;
                    this.area = l / 2;
                }
                return this.area;
            }
        }

        /// <summary>
        ///     Obtiene el vector normal al triangulo.
        /// </summary>
        public Vector3d Normal
        {
            get
            {
                if ((this.evaluated & Evaluated.Normal) == Evaluated.None)
                {
                    this.evaluated |= Evaluated.Normal;

                    Vector3d vz = this.VX.Cross(this.VY).Unit;
                    this.normal = vz;
                }
                return this.normal;
            }
        }

        /// <summary>
        ///     Evalua un punto en el triangulo utilizando los vectores unitarios.
        ///     NOTA: <![CDATA[u + v <= 1]]>
        /// </summary>
        /// <param name="u">Parametro u: segun el vector unitario 'vertice1 - vertice0'.</param>
        /// <param name="v">Parametro v: segun el vector unitario 'vertice2 - vertice0'.</param>
        /// <returns></returns>
        public Point3d Evaluate01(REAL u, REAL v)
        {
            Contract.Assert((u + v).EpsilonLE(1));
            return (this.P0 + this.VX * u + this.VY * v);
        }

        /// <summary>
        ///     Localiza un punto en el triangulo utilizando los vectores unitarios (coordenadas trilineales).
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <returns>Parametros u, v.</returns>
        public REAL[] Project01(Point3d p)
        {
            Vector3d po = (p - this.P0);

            REAL u, v, w;
            Geom3DUtils.Resolve(this.VX, this.VY, this.Normal, po, out u, out v, out w);

            return new[] { u, v };
        }

        /// <summary>
        ///     Evalua las coordenadas baricentricas del triangulo.
        /// </summary>
        public Point3d EvaluateBar(REAL u, REAL v)
        {
            return this.EvaluateBar(u, v, 1 - u - v);
        }

        /// <summary>
        ///     Evalua las coordenadas baricentricas del triangulo.
        ///     NOTA: <![CDATA[u + v + w = 1]]>
        /// </summary>
        public Point3d EvaluateBar(REAL u, REAL v, REAL w)
        {
            Contract.Assert((u + v + w).EpsilonEquals(1));
            return new Point3d(this.P0.X * u + this.P1.X * v + this.P2.X * w,
                               this.P0.Y * u + this.P1.Y * v + this.P2.Y * w,
                               this.P0.Z * u + this.P1.Z * v + this.P2.Z * w);
        }

        /// <summary>
        ///     Localiza las coordenadas baricentricas del triangulo.
        /// </summary>
        public REAL[] ProjectBar(Point3d p)
        {
            REAL v, w, u;
            this.ProjectBar(p, out u, out v, out w);
            return new[] { u, v, w };
        }

        /// <summary>
        ///     Localiza las coordenadas baricentricas del triangulo.
        /// </summary>
        public void ProjectBar(Point3d p, out REAL u, out REAL v, out REAL w)
        {
            Vector3d v0 = this.VX;
            Vector3d v1 = this.VY;
            Vector3d v2 = p - this.P0;
            REAL d00 = v0.Dot(v0);
            REAL d01 = v0.Dot(v1);
            REAL d11 = v1.Dot(v1);
            REAL d20 = v2.Dot(v0);
            REAL d21 = v2.Dot(v1);
            REAL denom = d00 * d11 - d01 * d01;

            v = (d11 * d20 - d01 * d21) / denom;
            w = (d00 * d21 - d01 * d20) / denom;
            u = 1.0f - v - w;
        }

        /// <summary>
        ///     Indica si el punto esta en el plano que forma el triangulo.
        /// </summary>
        public bool InPlane(Point3d pt, double precision = MathUtils.ZERO_TOLERANCE)
        {
            // http://blogs.msdn.com/b/rezanour/archive/2011/08/07/barycentric-coordinates-and-point-in-triangle-tests.aspx
            // Prepare our barycentric variables
            Vector3d u = this.VX;
            Vector3d v = this.VY;
            Vector3d w = pt - this.P0;

            Vector3d vCrossW = v.Cross(w);
            Vector3d vCrossU = v.Cross(u);

            // Test sign of r
            if (vCrossW.Dot(vCrossU) < 0)
            {
                return false;
            }

            Vector3d uCrossW = u.Cross(w);
            Vector3d uCrossV = u.Cross(v);

            // Test sign of t
            if (uCrossW.Dot(uCrossV) < 0)
            {
                return false;
            }

            return true;
        }

        #region private

        private REAL area;
        private Vector3d normal;

        private Evaluated evaluated = Evaluated.None;

        #endregion

        #region inner classes

        [Flags]
        private enum Evaluated
        {
            None = 0x0,
            Area = 0x1,
            Degenerate = 0x2,
            Normal = 0x4,
        }

        #endregion
    }
}