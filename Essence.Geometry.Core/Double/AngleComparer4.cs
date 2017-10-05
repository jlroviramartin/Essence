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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    /// Compares unit vectors using their angle.
    /// <pre><![CDATA[
    /// ^ normal = direccion.PerpLeft
    /// |
    /// | /__
    /// | \  \  incrementa el angulo
    /// |     |
    /// +-----+-----------> direccion
    /// ]]></pre>
    /// </summary>
    public sealed class AngleComparer4 : IComparer<IVector4>, IComparer
    {
        public AngleComparer4(IVector4 direccion, IVector4 normal)
        {
            Contract.Assert(direccion.IsUnit);
            this.direccion = direccion;
            this.normal = normal;
        }

        private readonly IVector4 direccion;
        private readonly IVector4 normal;

        public int Compare(IVector4 v1, IVector4 v2)
        {
            if (v1.IsZero)
            {
                if (v2.IsZero)
                {
                    return 0;
                }
                return -1; // v2 es mayor.
            }
            else if (v2.IsZero)
            {
                return 1; // v1 es mayor.
            }

            Contract.Assert(v1.IsUnit && v2.IsUnit);

            double nv1 = this.normal.Dot(v1);
            if (nv1 > 0)
            {
                // v1 esta encima.
                double nv2 = this.normal.Dot(v2);
                if (nv2 > 0)
                {
                    return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                }
                else if (nv2 < 0)
                {
                    return -1; // v2 es mayor.
                }
                else
                {
                    return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                }
            }
            else if (nv1 < 0)
            {
                // v1 esta debajo.
                double nv2 = this.normal.Dot(v2);
                if (nv2 > 0)
                {
                    return 1; // v1 es mayor.
                }
                else if (nv2 < 0)
                {
                    return this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                }
                else
                {
                    return 1;
                }
            }
            else // if (nv1 == 0)
            {
                // this.direccion.Dot(v1); // Es +1 o -1

                // v1 esta alineado.
                double nv2 = this.normal.Dot(v2);
                if (nv2 > 0)
                {
                    return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                }
                else if (nv2 < 0)
                {
                    return -1;
                }
                else
                {
                    return -this.direccion.Dot(v1).CompareTo(this.direccion.Dot(v2));
                }
            }
        }

        int IComparer.Compare(object o1, object o2)
        {
            Contract.Requires(o1 is IVector4 && o2 is IVector4);
            return this.Compare((IVector4)o1, (IVector4)o2);
        }
    }
}