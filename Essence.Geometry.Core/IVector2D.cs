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
    public interface IVector2D : IVector
    {
        /// <summary>
        /// This method gets the coordinates of <code>this</code> point.
        /// </summary>
        /// <param name="setter">Setter.</param>
        void GetCoordinates(ICoordinateSetter2D setter);

        /// <summary>
        /// This method calculates the cross product (producto vectorial) of <code>this</code> vector and <code>v2</code> vector.
        /// <see cref="http://en.wikipedia.org/wiki/Cross_product" />
        /// <code><![CDATA[
        /// ^ v1 x v2 ( + )
        /// |
        /// |   _
        /// |   /| v2
        /// |  /
        /// | /
        /// +----------> v1
        /// ]]></code>
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <returns>this x v2 = A x B = |A| * |B| * sin( angulo( A, B ) )</returns>
        [Pure]
        double Cross(IVector2D v2);
    }
}