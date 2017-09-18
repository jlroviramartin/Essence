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
using Essence.Geometry.Core;
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Geom2D
{
    public sealed class Line2d
    {
        /// <summary>
        ///     Plano con el vector normalizado.
        /// </summary>
        public static Line2d NewNormal(Point2d origin, Vector2d direction)
        {
            Vector2d vx = direction.Unit;

            Line2d line = new Line2d(origin, vx);
            if (vx.IsZero)
            {
                line.evaluated |= Evaluated.IsDegenerate;
                line.isDegenerate = true;
            }
            else
            {
                line.evaluated |= Evaluated.IsNormal;
                line.isNormal = true;
            }

            return line;
        }

        /// <summary>
        ///     Plano con el vector normalizado.
        /// </summary>
        public static Line2d NewNormal(Point2d p0, Point2d p1)
        {
            return NewNormal(p0, p1 - p0);
        }

        /// <summary>
        ///     Plano con el vector sin normalizar.
        /// </summary>
        public static Line2d NewNonNormal(Point2d origin, Vector2d direction)
        {
            return new Line2d(origin, direction);
        }

        /// <summary>
        ///     Plano con el vector sin normalizar.
        /// </summary>
        public static Line2d NewNonNormal(Point2d p0, Point2d p1)
        {
            return NewNonNormal(p0, p1 - p0);
        }

        /// <summary>
        ///     Indica si esta degenerado: el vector direccion es zero.
        /// </summary>
        public bool IsDegenerate
        {
            get
            {
                if ((this.evaluated & Evaluated.IsDegenerate) != Evaluated.IsDegenerate)
                {
                    this.evaluated |= Evaluated.IsDegenerate;

                    this.isDegenerate = this.Direction.IsZero;
                }
                return this.isDegenerate;
            }
        }

        /// <summary>
        ///     Indica si esta normalizado: el vector direccion es unitario.
        /// </summary>
        public bool IsNormal
        {
            get
            {
                if ((this.evaluated & Evaluated.IsNormal) != Evaluated.IsNormal)
                {
                    this.evaluated |= Evaluated.IsNormal;

                    this.isNormal = this.Direction.IsUnit;
                }
                return this.isNormal;
            }
        }

        public Point2d Origin { get; private set; }

        public Vector2d Direction { get; private set; }

        /// <summary>
        ///     Obtiene el vector normal (perpendicular a la derecha) normalizado.
        /// </summary>
        public Vector2d Normal
        {
            get
            {
                if ((this.evaluated & Evaluated.Normal) != Evaluated.Normal)
                {
                    this.evaluated |= Evaluated.Normal;

                    this.normal = this.Direction.PerpRight;
                    if (!this.IsNormal)
                    {
                        this.normal = this.normal.Unit;
                    }
                }
                return this.normal;
            }
        }

        /// <summary>
        ///     Constante en la ecuacion de la recta.
        ///     nx*x + ny*y = c
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
        ///     Evalua la proyeccion del punto sobre la linea, respecto del origen.
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <returns>Parametro [-Inf, Inf].</returns>
        public double Project(Point2d p)
        {
            Vector2d diff = p - this.Origin;

            double u;
            if (this.IsNormal)
            {
                //REAL v = this.Normal.Dot(diff);
                u = this.Direction.Dot(diff);
            }
            else
            {
                double v;
                Geom2DUtils.Resolve(this.Direction, this.Normal, diff, out u, out v);
            }
            return u;
        }

        /// <summary>
        ///     Evalua el punto.
        /// </summary>
        /// <param name="u">Parametro [-Inf, Inf].</param>
        /// <returns>Punto.</returns>
        public Point2d Evaluate(double u)
        {
            return this.Origin + u * this.Direction;
        }

        #region Distancia

        /// <summary>
        ///     Distancia (con signo) de un punto a la linea.
        /// </summary>
        public double Distance(Point2d p, out Point2d closestPoint)
        {
            Vector2d normal = this.Normal;
            double c = this.Constant;

            //REAL signedDistance = normal.Dot((VECTOR)p) - c = normal.Dot(p - this.Origin) = normal.Dot(p) - normal.Dot(this.Origin);
            double signedDistance = normal.X * p.X + normal.Y * p.Y - c;
            closestPoint = p - signedDistance * normal;
            return signedDistance;
        }

        /// <summary>
        ///     Distancia (con signo) de un punto a la linea.
        /// </summary>
        public double Distance(Point2d p)
        {
            Vector2d normal = this.Normal;
            double c = this.Constant;

            //REAL signedDistance = normal.Dot((VECTOR)p) - c = normal.Dot(p - this.Origin) = normal.Dot(p) - normal.Dot(this.Origin);
            double signedDistance = normal.X * p.X + normal.Y * p.Y - c;
            return signedDistance;
        }

        /// <summary>
        ///     Indica a que lado esta el punto respecto de la linea:
        ///     - Si == 0, esta en la linea.
        ///     - Si &gt; 0 esta debajo/derecha de la linea.
        ///     - Si &lt; 0 esta encima/izquierda de la linea.
        ///     <c><![CDATA[
        ///             ^ direction
        ///             |
        ///             |
        ///   (-)       +-----> (+) normal
        ///   left      |       right
        ///             |
        /// ]]></c>
        /// </summary>
        public LineSide WhichSide(Point2d p)
        {
            double distance = this.Distance(p);

            if (distance.EpsilonZero())
            {
                return LineSide.Middle;
            }
            else if (distance > 0)
            {
                // Derecha
                return LineSide.Right;
            }
            else // if (distance < 0)
            {
                // Izquierda
                return LineSide.Left;
            }
        }

        /// <summary>
        ///     Indica a que lado esta el rectangulo respecto de la linea:
        ///     - Si == 0, toca la linea o la cruza.
        ///     - Si &gt; 0 esta debajo/derecha de la linea.
        ///     - Si &lt; 0 esta encima/izquierda de la linea.
        /// </summary>
        public LineSide WhichSide(BoundingBox2d r)
        {
            int[] sides = new int[3];
            foreach (Point2d p in r.GetVertices())
            {
                LineSide lado = this.WhichSide(p);
                sides[(int)lado]++;
            }

            if (sides[(int)LineSide.Middle] > 0)
            {
                // Toca la linea.
                return LineSide.Middle;
            }
            if ((sides[(int)LineSide.Left] > 0) && (sides[(int)LineSide.Right] > 0))
            {
                // Cruza la linea.
                return LineSide.Middle;
            }
            if (sides[(int)LineSide.Left] > 0)
            {
                // totalmente a un lado
                return LineSide.Left;
            }
            // if (sides[(int)Side.Right] > 0)
            // totalmente a un lado
            return LineSide.Right;
        }

        #endregion

        #region private

        private Line2d(Point2d origin, Vector2d direction)
        {
            this.Origin = origin;
            this.Direction = direction;
        }

        private Vector2d normal;
        private double constant;
        private bool isNormal;
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
            IsNormal = 0x4,
            IsDegenerate = 0x8,
        }

        #endregion
    }
}