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

namespace Essence.Geometry.Core.Double
{
    public static class Matrix2x2dUtils
    {
        /// <summary>
        /// Crea una matriz con la rotacion.
        /// </summary>
        /// <param name="r">Angulo en radianes.</param>
        /// <returns>Matriz de rotacion.</returns>
        public static Matrix2x2d Rotate(double r)
        {
            double c = (double)Math.Cos(r);
            double s = (double)Math.Sin(r);
            return new Matrix2x2d(
                c, -s,
                s, c);
        }

        /// <summary>
        /// Crea una matriz con la escala.
        /// </summary>
        /// <param name="ex">Escala x.</param>
        /// <param name="ey">Escala y.</param>
        /// <returns>Matriz de escala.</returns>
        public static Matrix2x2d Scale(double ex, double ey)
        {
            return new Matrix2x2d(
                ex, 0,
                0, ey);
        }

        #region Traslacion, rotacion, escala

        /// <summary>
        /// Crea una matriz con la rotación y escala: mr * me.
        /// Primero se escala y después se rota.
        /// </summary>
        /// <param name="r">Rotación en radianes.</param>
        /// <param name="e">Escala.</param>
        /// <returns>Matriz de traslacion, rotacion y escala.</returns>
        public static Matrix2x2d RotateScale(double r, Vector2d e)
        {
            return RotateScale(r, e.X, e.Y);
        }

        /// <summary>
        /// Crea una matriz con la rotación y escala: mt * mr * me.
        /// Primero se escala y después se rota.
        /// </summary>
        /// <param name="r">Rotación en radianes.</param>
        /// <param name="ex">Escala x.</param>
        /// <param name="ey">Escala y.</param>
        /// <returns>Matriz de traslacion, rotacion y escala.</returns>
        public static Matrix2x2d RotateScale(double r, double ex, double ey)
        {
            double s = (double)Math.Sin(r);
            double c = (double)Math.Cos(r);
            return new Matrix2x2d(
                c * ex, -ey * s,
                ex * s, c * ey);
        }

        #endregion
    }
}