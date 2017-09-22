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

using System.Diagnostics.Contracts;

namespace Essence.Geometry.Core
{
    public interface IVector3D : IVector
    {
        /// <summary>
        /// This method gets the coordinates of <code>this</code> point.
        /// </summary>
        /// <param name="setter">Setter.</param>
        void GetCoordinates(ICoordinateSetter3D setter);

        /// <summary>
        /// This method calculates the cross product (producto vectorial) of <code>this</code> vector and <code>v2</code> vector.
        /// Right hand rule (OpenGL): VZ = VX * VY
        /// <see cref="http://en.wikipedia.org/wiki/Cross_product" />
        /// <code><![CDATA[
        /// ^ v1 x v2 ( + )
        /// |
        /// |   _
        /// |   /| v2
        /// |  / __
        /// | /  |\
        /// +------+---> v1
        /// ]]></code>
        /// Base ortonormal: Vx * Vy = Vz ; Vy * Vz = Vx ; Vz * Vx = Vy
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <returns>this x v2 = A x B = |A| * |B| * sin( angulo( A, B ) )</returns>
        [Pure]
        IVector3D Cross(IVector3D v2);

        /// <summary>
        /// This method calculates the mixed/triple product (producto mixto/triple) of <code>this</code> vector, <code>v2</code> and <code>v3</code> vector.
        /// <see cref="http://en.wikipedia.org/wiki/Mixed_product" />
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <param name="v3">Other vector.</param>
        /// <returns>this · ( v2 x v3 )</returns>
        [Pure]
        double TripleProduct(IVector3D v2, IVector3D v3);
    }
}