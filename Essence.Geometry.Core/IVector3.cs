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
    public interface IVector3 : IVector<IVector3>, ITuple3
    {
        /// <summary>
        /// Evaluates the octant of <code>this</code> vector.
        /// <pre><![CDATA[
        ///        ^
        ///    1   |   0
        ///        |
        ///  <-----+-----> z >= 0
        ///        |
        ///    2   |   3
        ///        v
        /// 
        ///        ^
        ///    5   |   4
        ///        |
        ///  <-----+-----> z < 0
        ///        |
        ///    6   |   7
        ///        v
        ///  ]]></pre>
        /// </summary>
        [Pure]
        int Octant { get; }

        /// <summary>
        ///     Operacion producto mixto en 3D (Mixed product/Triple product): v1 · ( v2 x v3 ).
        ///     <see cref="http://en.wikipedia.org/wiki/Mixed_product" />
        /// </summary>
        [Pure]
        double TripleProduct(IVector3 v2, IVector3 v3);
    }
}