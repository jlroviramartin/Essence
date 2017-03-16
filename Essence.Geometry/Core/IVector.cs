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

using Essence.Util.Math;
using System;
using System.Diagnostics.Contracts;
using REAL = System.Double;

namespace Essence.Geometry.Core
{
    public interface IVector : IEpsilonEquatable<IVector>
    {
        [Pure]
        int Dim { get; }

        [Pure]
        IConvertible this[int i] { get; }

        /// <summary>
        ///     Vector unitario: this / Length.
        /// </summary>
        [Pure]
        IVector Unit { get; }

        /// <summary>
        ///     Longitud o modulo del vector.
        /// </summary>
        [Pure]
        REAL Length { get; }

        /// <summary>
        ///     Longitud o modulo del vector al cuadrado.
        /// </summary>
        [Pure]
        REAL LengthCuad { get; }

        /// <summary>
        ///     Longitud L1 (de Manhattan).
        /// </summary>
        [Pure]
        REAL LengthL1 { get; }

        [Pure]
        IVector Add(IVector v2);

        [Pure]
        IVector Sub(IVector v2);

        [Pure]
        IVector Mul(REAL c);

        [Pure]
        IVector Div(REAL c);

        /// <summary>
        ///     Operacion multiplicacion componente a componente: v1 * v2.
        /// </summary>
        [Pure]
        IVector SimpleMul(IVector v2);

        /// <summary>
        ///     Operacion cambio signo: -vector.
        /// </summary>
        [Pure]
        IVector Neg();

        /// <summary>
        ///     Operacion valor absoluto: Absoluto( vector ).
        /// </summary>
        [Pure]
        IVector Abs();

        /// <summary>
        ///     Operacion interpolar: resultado = (1 - alpha) * v1 + alpha * v2 = v1 + (v2 - v1) * alpha.
        ///     Cuando alpha=0, resultado=v1.
        ///     Cuando alpha=1, resultado=v2.
        /// </summary>
        [Pure]
        IVector Lerp(IVector v2, REAL alpha);

        /// <summary>
        ///     Operacion inversa a la interpolacion: (v3 - v1) = (v2 - v1) * alpha.
        /// </summary>
        [Pure]
        REAL InvLerp(IVector v2, IVector vLerp);

        /// <summary>
        ///     Operacion combinacion lineal: alpha * v1 + beta * v2.
        /// </summary>
        [Pure]
        IVector Lineal(IVector v2, REAL alpha, REAL beta);

        /// <summary>
        ///     Operacion producto escalar (dot): v1 · v2.
        ///     A · B = |A| * |B| * cos( angulo( A, B ) )
        ///     <see cref="http://en.wikipedia.org/wiki/Dot_product" />
        ///     <pre><![CDATA[
        ///         ^
        ///  (-)    |   (+)     __ v2
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// <-------+------------+------> v1
        /// ]]></pre>
        ///     Si <c>v1</c> esta normalizado, es la proyeccion de <c>v2</c> sobre <c>v1</c>.
        /// </summary>
        [Pure]
        REAL Dot(IVector v2);

        /// <summary>
        ///     Proyeccion del vector <c>v2</c> sobre <c>v1</c>.
        ///     <pre><![CDATA[
        ///          
        ///  (-)    |   (+)     __ v2
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// --------+------------+------> v1
        /// ]]></pre>
        /// </summary>
        [Pure]
        REAL Proy(IVector v2);

        /// <summary>
        ///     Proyeccion del vector <c>v2</c> sobre <c>v1</c>.
        ///     Se normaliza el vector <c>v1</c> y se proyecta sobre este vector unitario.
        ///     <pre><![CDATA[
        ///          
        ///  (-)        (+)     __ v2
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// --------+----------->+------> v1
        /// ]]></pre>
        /// </summary>
        [Pure]
        IVector ProyV(IVector v2);
    }
}