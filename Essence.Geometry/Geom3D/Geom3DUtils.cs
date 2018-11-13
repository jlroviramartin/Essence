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
using System.Collections.Generic;
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using REAL = System.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Geom3D
{
    public static class Geom3DUtils
    {
        /// <summary>
        /// Resuelve las ecuaciones:
        /// u * uux + v * vvx = pox
        /// u * uuy + v * vvy = poy
        /// u * uuz + v * vvz = poz
        /// Ayuda a determinar las coordenadas del punto <c>po</c> segun el plano formado por <c>uu</c> y <c>vv</c>.
        /// Importante: <c>po</c> ha de estar sobre el plano.
        /// </summary>
        internal static void Resolve(Vector3d uu, Vector3d vv, Vector3d po,
                                     out double u, out double v)
        {
            // Resuelve la ecuacion:
            //   u * uux + v * vvx = pox
            //   u * uuy + v * vvy = poy
            //   u * uuz + v * vvz = poz
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

            LinkedList<double[]> ecuations = new LinkedList<double[]>();
            ecuations.AddLast(new[] { uu.X, vv.X, po.X });
            ecuations.AddLast(new[] { uu.Y, vv.Y, po.Y });
            ecuations.AddLast(new[] { uu.Z, vv.Z, po.Z });

            // Se busca la ecuacion con el maximo |uu.{i}|.
            LinkedListNode<double[]> e1 = ecuations.First;
            double div1 = e1.Value[0];
            {
                LinkedListNode<double[]> emaxxNext = e1.Next;
                while (emaxxNext != null)
                {
                    double aux = emaxxNext.Value[0];
                    if (SysMath.Abs(div1) < SysMath.Abs(aux))
                    {
                        div1 = aux;
                        e1 = emaxxNext;
                    }
                    emaxxNext = emaxxNext.Next;
                }
                ecuations.Remove(e1);
            }

            // Se busca la ecuacion con el maximo |vv.{i} * uu.{j} - uu.{i} * vv.{j}|.
            LinkedListNode<double[]> e2 = ecuations.First;
            double div2 = e1.Value[1] * e2.Value[0] - e1.Value[0] * e2.Value[1];
            {
                LinkedListNode<double[]> edivNext = e2.Next;
                while (edivNext != null)
                {
                    double aux = e1.Value[1] * edivNext.Value[0] - e1.Value[0] * edivNext.Value[1];
                    if (SysMath.Abs(div2) < SysMath.Abs(aux))
                    {
                        div2 = aux;
                        e2 = edivNext;
                    }
                    edivNext = edivNext.Next;
                }
                ecuations.Remove(e2);
            }

            // Se valcula 'u' y 'v'.
            // div2 = e1.Value[1] * e2.Value[0] - e1.Value[0] * e2.Value[1]
            v = (e1.Value[2] * e2.Value[0] - e1.Value[0] * e2.Value[2]) / div2;

            // div1 = e1.Value[0]
            u = (e1.Value[2] - v * e1.Value[1]) / div1;

            // Se comprueba en la '3a' ecuacion que todo cuadre.
            LinkedListNode<double[]> e3 = ecuations.First;
            if (!(e3.Value[0] * u + e3.Value[1] * v).EpsilonEquals(e3.Value[2]))
            {
                // No cuadra!
                u = double.NaN;
                v = double.NaN;
            }
        }

        /// <summary>
        /// Resuelve las ecuaciones:
        /// u * uux + v * vvx + w * wwx = pox
        /// u * uuy + v * vvy + w * wwy = poy
        /// u * uuz + v * vvz + w * wwz = poz
        /// Ayuda a determinar las coordenadas del punto <c>po</c> segun el plano formado por <c>uu</c> y <c>vv</c>.
        /// </summary>
        internal static void Resolve(Vector3d uu, Vector3d vv, Vector3d ww, Vector3d po,
                                     out double u, out double v, out double w)
        {
            // Resuelve la ecuacion:
            //   u * uux + v * vvx + w * wwx = pox
            //   u * uuy + v * vvy + w * wwy = poy
            //   u * uuz + v * vvz + w * wwz = poz
            //
            // Se replantea como:
            //
            //  /             \   /   \   /     \
            //  | uux vvx wwx |   | u |   | pox |
            //  | uuy vvy wwy | * | v | = | poy |
            //  | uuz vvz wwz |   | w |   | poz |
            //  \             /   \   /   \     /
            //

            // Se resuelve como un cambio de coordenadas.
            Matrix3x3d m = new Matrix3x3d(uu, vv, ww);
            m.Inv();
            Point3d ret = m * po;
            u = ret.X;
            v = ret.Y;
            w = ret.Z;
        }
    }
}