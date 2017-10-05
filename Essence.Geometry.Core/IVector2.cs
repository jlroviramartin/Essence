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
    public interface IVector2 : IVector<IVector2>, ITuple2
    {
        /// <summary>
        /// Evaluates the quadrant of <code>this</code> vector.
        /// <pre><![CDATA[
        ///        ^
        ///    1   |   0
        ///        |
        ///  <-----+----->
        ///        |
        ///    2   |   3
        ///        v
        ///  ]]></pre>
        /// </summary>
        [Pure]
        int Quadrant { get; }

        /// <summary>
        /// Evaluates the cross product (producto vectorial) of <code>v2</code> over <code>this</code>.
        /// The sign is important.
        /// <pre><![CDATA[ A x B = |A| * |B| * sin( angulo( A, B ) ) ]]></pre>
        /// <see cref="http://en.wikipedia.org/wiki/Cross_product" />
        /// <pre><![CDATA[
        /// ^ v1 x v2 ( + )
        /// |
        /// |   _
        /// |   /| v2
        /// |  /
        /// | /
        /// +----------> v1
        /// ]]></pre>
        /// </summary>
        [Pure]
        double Cross(IVector2 v2);
    }
}