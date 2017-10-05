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
    public interface IVector<TVector> : ITuple<TVector>
        where TVector : IVector<TVector>
    {
        /// <summary>
        /// Tests if <code>this</code> vector is unit.
        /// </summary>
        [Pure]
        bool IsUnit { get; }

        /// <summary>
        /// Evaluates the length of <code>this</code> vector.
        /// </summary>
        [Pure]
        double Length { get; }

        /// <summary>
        /// Evaluates the squared length of <code>this</code> vector.
        /// </summary>
        [Pure]
        double LengthSquared { get; }

        /// <summary>
        /// Evaluates the L1 (Manhattan) length of <code>this</code> vector.
        /// </summary>
        [Pure]
        double LengthL1 { get; }

        /// <summary>
        /// Evaluates the dot product (producto escalar) of <code>v2</code> over <code>this</code>.
        /// <pre><![CDATA[ A · B = |A| * |B| * cos( angulo( A, B ) ) ]]></pre>
        /// <see cref="http://en.wikipedia.org/wiki/Dot_product" />
        /// <pre><![CDATA[
        ///         ^
        ///  (-)    |   (+)     __ v2
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// --------+------------+------> this
        /// ]]></pre>
        /// If <c>this</c> is normalized, it is the projection of <c>v2</c> over <c>this</c>.
        /// </summary>
        [Pure]
        double Dot(TVector v2);

        /// <summary>
        /// Evaluates the projection of <code>v2</code> over <code>this</code>.
        /// <pre><![CDATA[ A.Proj( B ) = A · B / |A| = |A| * |B| * cos( angulo( A, B ) ) / |A| ]]></pre>
        /// <see cref="http://en.wikipedia.org/wiki/Dot_product" />
        /// <pre><![CDATA[
        ///         ^
        ///  (-)    |   (+)     __ v2
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// --------+------------+------> this
        /// ]]></pre>
        /// </summary>
        [Pure]
        double Proj(TVector v2);

        /// <summary>
        ///  Evaluates the inverse (alpha) of the interpolation between <code>this</code> and <code>v2</code>
        ///  at <code>vLerp</code>.
        ///  It returns (pLerp - this) = (p2 - this) * alpha.
        /// </summary>
        [Pure]
        double InvLerp(TVector v2, TVector vLerp);
    }
}