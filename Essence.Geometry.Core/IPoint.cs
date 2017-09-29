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
    public interface IPoint<TPoint> : ITuple<TPoint>
        where TPoint : IPoint<TPoint>
    {
        /// <summary>
        ///  Evaluates the inverse (alpha) of the interpolation between <code>this</code> and <code>p2</code>
        ///  at <code>pLerp</code>.
        ///  It returns (pLerp - this) = (p2 - this) * alpha.
        /// </summary>
        [Pure]
        double InvLerp(TPoint p2, TPoint pLerp);
    }
}