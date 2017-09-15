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
        [Pure]
        int Dim { get; }

        /// <summary>
        /// This method gets the coordinates.
        /// </summary>
        /// <param name="setter">Setter.</param>
        void GetCoordinates(ICoordinateSetter setter);

        [Pure]
        IPoint Add(IVector v);

        [Pure]
        IPoint Sub(IVector v);

        [Pure]
        IVector Sub(IPoint p);

        /// <summary>
        ///     Operacion interpolar: resultado = (1 - alpha) * this + alpha * p2 = this + (p2 - this) * alpha.
        ///     <list type="bullet">
        ///     <item>Cuando alpha=0, resultado=this.</item>
        ///     <item>Cuando alpha=1, resultado=p.</item>
        ///     </list>
        /// </summary>
        [Pure]
        IPoint Lerp(IPoint p2, double alpha);

        /// <summary>
        ///     Operacion inversa a la interpolacion: (pLerp - this) = (p2 - this) * alpha.
        /// </summary>
        [Pure]
        double InvLerp(IPoint p2, IPoint pLerp);

        /// <summary>
        ///     Operacion combinacion lineal: alpha * this + beta * p2.
        /// </summary>
        [Pure]
        IPoint Lineal(IPoint p2, double alpha, double beta);
    }
}