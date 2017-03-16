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
    public interface IVector2D : IVector
    {
        [Pure]
        IConvertible X { get; }

        [Pure]
        IConvertible Y { get; }

        /// <summary>
        ///     Operacion producto vectorial (cross) en 3D: v1 x v2. Tiene en cuenta el signo.
        ///     A x B = |A| * |B| * sin( angulo( A, B ) )
        ///     <see cref="http://en.wikipedia.org/wiki/Cross_product" />
        ///     <code><![CDATA[
        /// ^ v1 x v2 ( + )
        /// |
        /// |   _
        /// |   /| v2
        /// |  /
        /// | /
        /// +----------> v1
        /// ]]></code>
        /// </summary>
        [Pure]
        REAL Cross(IVector2D v2);
    }
}