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

using Essence.Util.Math;
using System.Diagnostics.Contracts;

namespace Essence.Geometry.Core
{
    public interface IVector : IEpsilonEquatable<IVector>
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
        /// Unit vector of <code>this</code> vector.
        /// </summary>
        /// <returns>this / Length</returns>
        [Pure]
        IVector Unit { get; }

        /// <summary>
        /// Length/module/magnitude of <code>this</code> vector.
        /// </summary>
        /// <returns>Length.</returns>
        [Pure]
        double Length { get; }

        /// <summary>
        /// Squared length/module/magnitude of <code>this</code> vector.
        /// </summary>
        /// <returns>Length.</returns>
        [Pure]
        double LengthSquared { get; }

        /// <summary>
        /// L1 (Manhattan) length of <code>this</code> vector.
        /// </summary>
        /// <returns>L1 length.</returns>
        [Pure]
        double LengthL1 { get; }

        /// <summary>
        /// This method calculates the addition of <code>this</code> vector and <code>v2</code> vector.
        /// </summary>
        /// <param name="v2">Vector.</param>
        /// <returns>Addition.</returns>
        [Pure]
        IVector Add(IVector v2);

        /// <summary>
        /// This method calculates the subtraction of <code>this</code> vector and <code>v2</code> vector.
        /// </summary>
        /// <param name="v2">Vector.</param>
        /// <returns>Subtraction.</returns>
        [Pure]
        IVector Sub(IVector v2);

        /// <summary>
        /// This method calculates the multiplication of <code>this</code> vector and <code>c</code> scalar.
        /// </summary>
        /// <param name="c">Scalar.</param>
        /// <returns>Multiplication.</returns>
        [Pure]
        IVector Mul(double c);

        /// <summary>
        /// This method calculates the division of <code>this</code> vector and <code>c</code> scalar.
        /// </summary>
        /// <param name="c">Scalar.</param>
        /// <returns>Division.</returns>
        [Pure]
        IVector Div(double c);

        /// <summary>
        /// This method calculates the multiplication of <code>this</code> vector and <code>v2</code> vector, component by component.
        /// </summary>
        /// <param name="v2">Vector.</param>
        /// <returns>Multiplication, component by component.</returns>
        [Pure]
        IVector SimpleMul(IVector v2);

        /// <summary>
        /// This method calculates the negation of <code>this</code> vector.
        /// </summary>
        /// <returns>Negation.</returns>
        [Pure]
        IVector Neg();

        /// <summary>
        /// This method calculates the absolute value of <code>this</code> vector.
        /// </summary>
        /// <returns>Absolute value.</returns>
        [Pure]
        IVector Abs();

        /// <summary>
        /// This method calculates the interpolation at <code>alpha</code> between <code>this</code> vector and <code>v2</code> vector.
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <param name="alpha">Alpha.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>if alpha == 0 then this</item>
        /// <item>if alpha == 1 then v2</item>
        /// <item>else then (1 - alpha) * this + alpha * v2 = this + (v2 - this) * alpha</item>
        /// </list>
        /// </returns>
        [Pure]
        IVector Lerp(IVector v2, double alpha);

        /// <summary>
        /// This method calculates the inverse of the interpolation of <code>vLerp</code> vector with respect
        /// to <code>this</code> vector and <code>v2</code> vector.
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <param name="vLerp">Vector interpolated.</param>
        /// <returns>(vLerp - this) = (v2 - this) * alpha</returns>
        [Pure]
        double InvLerp(IVector v2, IVector vLerp);

        /// <summary>
        /// This method calculates the lineal combination at <code>alpha</code> and <code>beta</code>
        /// between <code>this</code> vector and <code>v2</code> vector.
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <param name="alpha">Alpha.</param>
        /// <param name="beta">Beta.</param>
        /// <returns>alpha * this + beta * v2</returns>
        [Pure]
        IVector Lineal(IVector v2, double alpha, double beta);

        /// <summary>
        /// This method calculates the dot product (producto escalar) of <code>this</code> vector and <code>v2</code> vector.
        /// <see cref="http://en.wikipedia.org/wiki/Dot_product" />
        /// <pre><![CDATA[
        ///         ^
        ///  (-)    |   (+)     __ v2
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// <-------+------------+------> v1
        /// ]]></pre>
        /// if <c>this</c> is normalized, the result is the projection of <c>v2</c> vector to <c>this</c> vector.
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <returns>this · v2 = A · B = |A| * |B| * cos( angulo( A, B ) )</returns>
        [Pure]
        double Dot(IVector v2);

        /// <summary>
        /// This method calculates the projection of <code>v2</code> vector to <code>this</code> vector.
        /// <pre><![CDATA[
        ///          
        ///  (-)    |   (+)     __ v2
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// --------+------------+------> v1
        /// ]]></pre>
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <returns>this.Unit.Dot( v2 )</returns>
        [Pure]
        double Proj(IVector v2);

        /// <summary>
        /// This method calculates the projection vector of <code>v2</code> vector to <code>this</code> vector.
        /// <pre><![CDATA[
        ///          
        ///  (-)        (+)     __ v2
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// --------+----------->+------> v1
        /// ]]></pre>
        /// </summary>
        /// <param name="v2">Other vector.</param>
        /// <returns>this.Unit.Mul( this.Unit.Dot( v2 ) )</returns>
        [Pure]
        IVector ProjV(IVector v2);
    }
}