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
using Essence.Util.Math;

namespace Essence.Geometry.Core
{
    public interface IPoint : IEpsilonEquatable<IPoint>
    {
        /// <summary>
        /// Dimension of <code>this</code> point.
        /// </summary>
        [Pure]
        int Dim { get; }

        /// <summary>
        /// This method gets the coordinates of <code>this</code> point.
        /// </summary>
        /// <param name="setter">Setter.</param>
        void GetCoordinates(ICoordinateSetter setter);

        /// <summary>
        /// This method calculates the addition of <code>this</code> point and <code>v</code> vector.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <returns>Addition.</returns>
        [Pure]
        IPoint Add(IVector v);

        /// <summary>
        /// This method calculates the subtraction of <code>this</code> point and <code>v</code> vector.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <returns>Subtraction.</returns>
        [Pure]
        IPoint Sub(IVector v);

        /// <summary>
        /// This method calculates the subtraction of <code>this</code> point and <code>p</code> point.
        /// </summary>
        /// <param name="p">Other point.</param>
        /// <returns>Addition.</returns>
        [Pure]
        IVector Sub(IPoint p);

        /// <summary>
        /// This method calculates the interpolation at <code>alpha</code> between <code>this</code> point and <code>p2</code> point.
        /// </summary>
        /// <param name="p2">Other point.</param>
        /// <param name="alpha">Alpha.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>if alpha == 0 then this</item>
        /// <item>if alpha == 1 then p2</item>
        /// <item>else then (1 - alpha) * this + alpha * p2 = this + (p2 - this) * alpha</item>
        /// </list>
        /// </returns>
        [Pure]
        IPoint Lerp(IPoint p2, double alpha);

        /// <summary>
        /// This method calculates the inverse of the interpolation of <code>pLerp</code> point with respect
        /// to <code>this</code> point and <code>p2</code> point.
        /// </summary>
        /// <param name="p2">Other point.</param>
        /// <param name="pLerp">Point interpolated.</param>
        /// <returns>(pLerp - this) = (p2 - this) * alpha</returns>
        [Pure]
        double InvLerp(IPoint p2, IPoint pLerp);

        /// <summary>
        /// This method calculates the lineal combination at <code>alpha</code> and <code>beta</code>
        /// between <code>this</code> point and <code>p2</code> point.
        /// </summary>
        /// <param name="p2">Other point.</param>
        /// <param name="alpha">Alpha.</param>
        /// <param name="beta">Beta.</param>
        /// <returns>alpha * this + beta * p2</returns>
        [Pure]
        IPoint Lineal(IPoint p2, double alpha, double beta);
    }
}