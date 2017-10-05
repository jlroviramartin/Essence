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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    ///     Compares unit vectors using their angle.
    ///     <pre><![CDATA[
    /// ^ normal = direccion.PerpLeft
    /// |
    /// | /__
    /// | \  \  incrementa el angulo
    /// |     |
    /// +-----+-----------> direccion
    /// ]]></pre>
    /// </summary>
    public struct AngleComparer2_v2 : IComparer<IVector2>, IComparer
    {
        public int Compare(IVector2 v1, IVector2 v2)
        {
            // Se hace un cambio de cuadrande:
            //   -1 |  0
            //  ----+----
            //   -2 | -3
            //int c1 = (5 - v1.Cuadrante) % 4;
            //int c2 = (5 - v2.Cuadrante) % 4;
            int c1 = v1.Quadrant;
            int c2 = v2.Quadrant;
            if (c1 == c2)
            {
                // Se convierten al cuadrante 0.
                switch (c1)
                {
                    case 1:
                        // Rotacion 90 a la derecha.
                        v1 = Rot(v1, (-Math.PI / 2)); // -1
                        break;
                    case 2:
                        // Rotacion 180 a la derecha.
                        v1 = Rot(v1, (-Math.PI)); // -2
                        break;
                    case 3:
                        // Rotacion 270 a la derecha.
                        v1 = Rot(v1, (-3 * Math.PI / 2)); // -3
                        break;
                }
                double m1 = v1.Y / v1.X;
                double m2 = v2.Y / v2.X;
                return m1.CompareTo(m2);
            }
            else
            {
                return c1.CompareTo(c2);
            }
        }

        int IComparer.Compare(object o1, object o2)
        {
            Contract.Requires(o1 is IVector2 && o2 is IVector2);
            return this.Compare((IVector2)o1, (IVector2)o2);
        }
    }
}