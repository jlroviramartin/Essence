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
    public interface IOpVector4 : IVector4
    {
        /// <summary>
        /// Vector unitario: this / Length.
        /// </summary>
        void Unit();

        void Add(IVector4 v2);

        void Add(IVector4 v1, IVector4 v2);

        void Sub(IVector4 v2);

        void Sub(IVector4 v1, IVector4 v2);

        void Mul(double c);

        void Mul(IVector4 v1, double c);

        void Div(double c);

        void Div(IVector4 v1, double c);

        /// <summary>
        ///     Operacion multiplicacion componente a componente: v1 * v2.
        /// </summary>
        void SimpleMul(IVector4 v2);

        void SimpleMul(IVector4 v1, IVector4 v2);

        /// <summary>
        ///     Operacion cambio signo: -vector.
        /// </summary>
        void Neg();

        void Neg(IVector4 v1);

        /// <summary>
        ///     Operacion valor absoluto: Absoluto( vector ).
        /// </summary>
        void Abs();

        void Abs(IVector4 v1);

        /// <summary>
        ///     Operacion interpolar: resultado = (1 - alpha) * v1 + alpha * v2 = v1 + (v2 - v1) * alpha.
        ///     Cuando alpha=0, resultado=v1.
        ///     Cuando alpha=1, resultado=v2.
        /// </summary>
        void Lerp(IVector4 v2, double alpha);

        void Lerp(IVector4 v1, IVector4 v2, double alpha);

        /// <summary>
        ///     Operacion combinacion lineal: alpha * v1 + beta * v2.
        /// </summary>
        void Lineal(IVector4 v2, double alpha, double beta);

        void Lineal(IVector4 v1, IVector4 v2, double alpha, double beta);

        /// <summary>
        /// Proyeccion del vector <c>v2</c> sobre <c>v1</c>.
        /// Se normaliza el vector <c>v1</c> y se proyecta sobre este vector unitario.
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
        void ProjV(IVector4 v2);

        void ProjV(IVector4 v1, IVector4 v2);
    }
}