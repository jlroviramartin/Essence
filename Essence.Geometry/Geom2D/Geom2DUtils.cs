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

using Essence.Geometry.Core.Double;
using REAL = System.Double;

namespace Essence.Geometry.Geom2D
{
    public static class Geom2DUtils
    {
        /// <summary>
        ///     Resuelve las ecuaciones:
        ///     u * uux + v * vvx = pox
        ///     u * uuy + v * vvy = poy
        ///     Ayuda a determinar las coordenadas del punto <c>po</c> segun el plano formado por <c>uu</c> y <c>vv</c>.
        /// </summary>
        internal static void Resolve(Vector2d uu, Vector2d vv, Vector2d po,
                                     out REAL u, out REAL v)
        {
            // Resuelve la ecuacion:
            //   u * uux + v * vvx = pox
            //   u * uuy + v * vvy = poy
            //
            // Se replantea como:
            //
            //  /         \   /   \   /     \
            //  | uux vvx |   | u |   | pox |
            //  |         | * |   | = |     |
            //  | uuy vvy |   | v |   | poy |
            //  \         /   \   /   \     /
            //
            // Si se toma el primera y segunda ecuacion, se multiplican
            // por uuy y uux respectivamente y se restan:
            //   (u * uux * uuy + v * vvx * uuy) - (u * uuy * uux + v * vvy * uux) = pox * uuy - poy * uux
            // Simplificando:
            //   v * vvx * uuy - v * vvy * uux = pox * uuy - poy * uux
            // despejando v:
            //   v = (pox * uuy - poy * uux) / (vvx * uuy - vvy * uux)
            // Y despejando u en cualquier ecuacion:
            //   u = (pox - v * vvx) / uux
            // Por estabilidad numerica se intenta que los divisores esten lo mas alejados
            // posible del 0.

            /*v = (po.X * uu.Y - po.Y * uu.X) / (vv.X * uu.Y - vv.Y * uu.X);

            if (Math.Abs(uu.X) > Math.Abs(uu.Y))
            {
                u = (po.X - v * vv.X) / uu.X;
            }
            else
            {
                u = (po.Y - v * vv.Y) / uu.Y;
            }*/

            // Se resuelve como un cambio de coordenadas.
            Matrix2x2d m = new Matrix2x2d(uu, vv);
            m.Inv();
            Point2d ret = m * po;
            u = ret.X;
            v = ret.Y;
        }
    }
}