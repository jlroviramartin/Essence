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
using System.Runtime.CompilerServices;
using Essence.Geometry.Core.Double;
using Essence.Util.Math;

namespace Essence.Geometry.Geom2D
{
    public sealed class Segment2d
    {
        public Segment2d(Point2d p0, Point2d p1)
        {
            this.P0 = p0;
            this.P1 = p1;
        }

        public Point2d P0 { get; set; }

        public Point2d P1 { get; set; }

        public Point2d Center
        {
            get { return this.P0.Lerp(this.P1, 0.5); }
        }

        public Vector2d Direction
        {
            get
            {
                this.EvaluateDirectionLength();
                return this.direction;
            }
        }

        public double Length
        {
            get
            {
                this.EvaluateDirectionLength();
                return this.length;
            }
        }

        /// <summary>
        ///     Evalua la proyeccion del punto sobre el segmento entre P0 y P1.
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <param name="adjust">Indica si ajusta a los limites [0, longitud].</param>
        /// <returns>Parametro [0, longitud].</returns>
        public double Proyect0L(Point2d p, bool adjust = true)
        {
            Vector2d diff = p - this.P0;
            double param = this.Direction.Dot(diff);

            if (adjust)
            {
                param = param.Clamp(0, this.Length);
            }

            return param;
        }

        /// <summary>
        ///     Evalua el punto entre P0 y P1.
        /// </summary>
        /// <param name="u">Parametro [0, longitud].</param>
        /// <returns>Punto.</returns>
        public Point2d Evaluate0L(double u)
        {
            return this.P0 + u * this.Direction;
        }

        /// <summary>
        ///     Evalua la proyeccion del punto sobre el segmento entre P0 y P1.
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <param name="adjust">Indica si ajusta a los limites [0, 1].</param>
        /// <returns>Parametro [0, 1].</returns>
        public double Proyect01(Point2d p, bool adjust = true)
        {
            Vector2d diff = p - this.P0;
            double u = this.Direction.Dot(diff) / this.Length;

            if (adjust)
            {
                u = u.Clamp(0, 1);
            }

            return u;
        }

        /// <summary>
        ///     Evalua el punto entre P0 y P1.
        /// </summary>
        /// <param name="u">Parametro [0, 1].</param>
        /// <returns>Punto.</returns>
        public Point2d Evaluate01(double u)
        {
            return this.P0 + (u * this.Length) * this.Direction;
        }

        #region private

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EvaluateDirectionLength()
        {
            if ((this.evaluated & Evaluated.DirectionLength) != Evaluated.DirectionLength)
            {
                this.evaluated |= Evaluated.DirectionLength;

                Vector2d aux = (this.P1 - this.P0);
                this.length = aux.Length;
                this.direction = aux.Div(this.length);
            }
        }

        private Vector2d direction;
        private double length;
        private Evaluated evaluated = Evaluated.None;

        #endregion

        #region inner classes

        [Flags]
        private enum Evaluated
        {
            None = 0x0,
            DirectionLength = 0x1,
        }

        #endregion
    }
}