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
using REAL = System.Double;

namespace Essence.Geometry.Core
{
    public interface IVector3D : IVector
    {
        [Pure]
        IConvertible X { get; }

        [Pure]
        IConvertible Y { get; }

        [Pure]
        IConvertible Z { get; }

        /// <summary>
        ///     Operacion producto vectorial (cross) en 3D: v1 x v2.
        ///     Regla de la mano derecha (OpenGL): VZ = VX * VY
        ///     <see cref="http://en.wikipedia.org/wiki/Cross_product" />
        ///     <code><![CDATA[
        /// ^ v1 x v2 ( + )
        /// |
        /// |   _
        /// |   /| v2
        /// |  / __
        /// | /  |\
        /// +------+---> v1
        /// ]]></code>
        ///     Base ortonormal: Vx * Vy = Vz ; Vy * Vz = Vx ; Vz * Vx = Vy
        /// </summary>
        [Pure]
        IVector3D Cross(IVector3D v2);

        /// <summary>
        ///     Operacion producto mixto en 3D (Mixed product/Triple product): v1 · ( v2 x v3 ).
        ///     <see cref="http://en.wikipedia.org/wiki/Mixed_product" />
        /// </summary>
        [Pure]
        REAL TripleProduct(IVector3D v2, IVector3D v3);
    }
}