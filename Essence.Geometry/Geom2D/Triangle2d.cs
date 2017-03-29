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
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using REAL = System.Double;

namespace Essence.Geometry.Geom2D
{
    public class Triangle2d
    {
        public Triangle2d(Point2d p0, Point2d p1, Point2d p2)
        {
            this.P0 = p0;
            this.P1 = p1;
            this.P2 = p2;
        }

        public readonly Point2d P0;
        public readonly Point2d P1;
        public readonly Point2d P2;

        public Vector2d VX
        {
            get { return this.P1 - this.P0; }
        }

        public Vector2d VY
        {
            get { return this.P2 - this.P0; }
        }

        /// <summary>
        ///     Obtiene el vertice con indice <c>i</c>.
        /// </summary>
        /// <param name="i">Indice del vertice.</param>
        /// <returns>Vertice.</returns>
        public Point2d this[int i]
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
            get
            {
                if ((this.evaluated & Evaluated.Degenerate) == Evaluated.None)
                {
                    this.evaluated |= Evaluated.Degenerate;

                    // http://en.wikipedia.org/wiki/Bilinear_interpolation
                    // http://www.gamedev.net/topic/380061-solve-triangle-interpolation-linear-equations/
                    // http://www.cc.gatech.edu/classes/AY2007/cs3451_spring/lininter.pdf
                    // http://www1.eonfusion.com/manual/index.php/Formulae_for_interpolation

                    double z = this.VX.Cross(this.VY);

                    this.degenerate = z.EpsilonEquals(0);
                }
                return this.degenerate;
            }
        }

        /// <summary>
        ///     Area del triangulo.
        /// </summary>
        public double Area
        {
            get
            {
                if ((this.evaluated & Evaluated.Area) == Evaluated.None)
                {
                    this.evaluated |= Evaluated.Area;

                    // https://en.wikipedia.org/wiki/Cross_product#Properties
                    double l = Math.Abs(this.VX.Cross(this.VY));
                    this.area = l / 2;
                }
                return this.area;
            }
        }

        /// <summary>
        ///     Orientacion del triangulo:
        ///     <list>
        ///         <item>&gt; 0 AntiHoraria, CounterClockWise, CCW ( + )</item>
        ///         <item>= 0 ninguna, degenrado</item>
        ///         <item>&lt; 0 Horaria, ClockWise, CW ( - )</item>
        ///     </list>
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                if ((this.evaluated & Evaluated.Orientation) == Evaluated.None)
                {
                    this.evaluated |= Evaluated.Orientation;

                    //VECTOR edge0 = this.VX;
                    //VECTOR edge1 = this.VY;
                    //cw = (edge0.DotPerpRight(edge1) < 0.0);

                    // ¿¿¿ return Punto2d.IsLeft(this.vertice0, this.vertice1, this.vertice2); ???
                    // orientacion (producto vectorial): (v1 - v0) x (v2 - v0)
                    // http://www-sop.inria.fr/prisme/fiches/Arithmetique/index.html.en
                    double res = (this.P1.X - this.P0.X) * (this.P2.Y - this.P0.Y)
                                 - (this.P1.Y - this.P0.Y) * (this.P2.X - this.P0.X);
                    if (res.EpsilonEquals(0))
                    {
                        this.orientation = Orientation.Degenerate;
                    }
                    else if (res > 0)
                    {
                        this.orientation = Orientation.CCW;
                    }
                    else //if (res < 0)
                    {
                        this.orientation = Orientation.CW;
                    }
                }
                return this.orientation;
            }
        }

        /// <summary>
        ///     Reordena los vertices del triangulo.
        /// </summary>
        public Triangle2d EnsureCCW()
        {
            if (this.Orientation == Orientation.CW)
            {
                // Triangle is clockwise, reorder it.
                return new Triangle2d(this.P0, this.P2, this.P1);
            }
            return this;
        }

        /// <summary>
        ///     Evalua un punto en el triangulo utilizando los vectores unitarios.
        ///     NOTA: <![CDATA[u + v <= 1]]>
        /// </summary>
        /// <param name="u">Parametro u: segun el vector unitario 'vertice1 - vertice0'.</param>
        /// <param name="v">Parametro v: segun el vector unitario 'vertice2 - vertice0'.</param>
        /// <returns></returns>
        public Point2d Evaluate01(double u, double v)
        {
            Contract.Assert((u + v).EpsilonLE(1));
            return (this.P0 + this.VX * u + this.VY * v);
        }

        /// <summary>
        ///     Localiza un punto en el triangulo utilizando los vectores unitarios (coordenadas trilineales).
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <returns>Parametros u, v.</returns>
        public double[] Project01(Point2d p)
        {
            Vector2d po = (p - this.P0);

            double u, v;
            Geom2DUtils.Resolve(this.VX, this.VY, po, out u, out v);

            return new[] { u, v };
        }

        /// <summary>
        ///     Evalua las coordenadas baricentricas del triangulo.
        /// </summary>
        public Point2d EvaluateBar(double u, double v)
        {
            return this.EvaluateBar(u, v, 1 - u - v);
        }

        /// <summary>
        ///     Evalua las coordenadas baricentricas del triangulo.
        ///     NOTA: <![CDATA[u + v + w = 1]]>
        /// </summary>
        public Point2d EvaluateBar(double u, double v, double w)
        {
            Contract.Assert((u + v + w).EpsilonEquals(1));
            return new Point2d(this.P0.X * u + this.P1.X * v + this.P2.X * w,
                               this.P0.Y * u + this.P1.Y * v + this.P2.Y * w);
        }

        /// <summary>
        ///     Localiza las coordenadas baricentricas del triangulo.
        ///     <see cref="ocs/core/toxi/geom/Triangle3D.html" />
        ///     <see cref="http://en.wikipedia.org/wiki/Barycentric_coordinate_system" />
        ///     <see cref="http://mathworld.wolfram.com/BarycentricCoordinates.html" />
        ///     <see
        ///         cref="http://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates" />
        /// </summary>
        public double[] ProjectBar(Point2d p)
        {
            double v, w, u;
            this.ProjectBar(p, out u, out v, out w);
            return new[] { u, v, w };
        }

        /// <summary>
        ///     Localiza las coordenadas baricentricas del triangulo.
        /// </summary>
        public void ProjectBar(Point2d p, out double u, out double v, out double w)
        {
            Vector2d v0 = this.VX;
            Vector2d v1 = this.VY;
            Vector2d v2 = p - this.P0;
            double d00 = v0.Dot(v0);
            double d01 = v0.Dot(v1);
            double d11 = v1.Dot(v1);
            double d20 = v2.Dot(v0);
            double d21 = v2.Dot(v1);
            double denom = d00 * d11 - d01 * d01;

            v = (d11 * d20 - d01 * d21) / denom;
            w = (d00 * d21 - d01 * d20) / denom;
            u = 1.0f - v - w;
        }

        #region private

        private double area;
        private bool degenerate;
        private Orientation orientation;

        private Evaluated evaluated = Evaluated.None;

        #endregion

        #region inner classes

        [Flags]
        private enum Evaluated
        {
            None = 0x0,
            Area = 0x1,
            Degenerate = 0x2,
            Orientation = 0x4,
        }

        #endregion
    }
}