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

namespace Essence.Geometry.Core
{
    public interface IOpVector2 : IVector2
    {
        /// <summary>
        /// Evaluates the unit vector of <code>this</code> vector.
        /// </summary>
        void Unit();

        void Add(IVector2 v2);

        void Add(IVector2 v1, IVector2 v2);

        void Sub(IVector2 v2);

        void Sub(IVector2 v1, IVector2 v2);

        void Sub(IPoint2 p1, IPoint2 p2);

        void Mul(double c);

        void Mul(IVector2 v1, double c);

        void Div(double c);

        void Div(IVector2 v1, double c);

        void SimpleMul(IVector2 v2);

        void SimpleMul(IVector2 v1, IVector2 v2);

        /// <summary>
        /// Evaluates the sign change of <code>this</code> vector.
        /// </summary>
        void Neg();

        void Neg(IVector2 v1);

        /// <summary>
        /// Evaluates the absolute value of <code>this</code> vector.
        /// </summary>
        void Abs();

        void Abs(IVector2 v1);

        /// <summary>
        /// Evaluates the interpolation of <code>this</code> and <code>v2</code> with <code>alpha</code>.
        /// If alpha = 0 then v1 else if alpha = 1 then v2.
        /// <pre><![CDATA[ (1 - alpha) * v1 + alpha * v2 = v1 + (v2 - v1) * alpha ]]></pre>
        /// </summary>
        void Lerp(IVector2 v2, double alpha);

        void Lerp(IVector2 v1, IVector2 v2, double alpha);

        /// <summary>
        /// Evaluates the lineal combination of <code>this</code> with <code>alpha</code> and <code>v2</code>
        /// with <code>beta</code>.
        /// <pre><![CDATA[ alpha * v1 + beta * v2 ]]></pre>
        /// </summary>
        void Lineal(IVector2 v2, double alpha, double beta);

        void Lineal(IVector2 v1, IVector2 v2, double alpha, double beta);

        /// <summary>
        /// Evaluates the projection vector of <code>v2</code> over <code>this</code>.
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
        void ProjV(IVector2 v2);

        void ProjV(IVector2 v1, IVector2 v2);
    }
}