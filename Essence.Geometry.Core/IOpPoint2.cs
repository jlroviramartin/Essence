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
    public interface IOpPoint2 : IPoint2, IOpTuple2
    {
        void Add(IVector2 v);

        void Add(IPoint2 p, IVector2 v);

        void Sub(IVector2 v);

        void Sub(IPoint2 p, IVector2 v);

        /// <summary>
        /// Evaluates the interpolation between <code>this</code> and <code>p2</code> at <code>alpha</code>.
        /// <list type="bullet">
        ///     <item>If alpha=0 it returns this.</item>
        ///     <item>If alpha=1 it returns p2.</item>
        ///     <item>Else it returns (1 - alpha) * this + alpha * p2 = this + (p2 - this) * alpha</item>
        /// </list>
        /// </summary>
        void Lerp(IPoint2 p2, double alpha);

        void Lerp(IPoint2 p1, IPoint2 p2, double alpha);

        /// <summary>
        /// Evaluates the lineal combination between <code>this</code> at <code>alpha</code> and
        /// <code>p2</code> at <code>beta</code>.
        /// It returns alpha * this + beta * p2.
        /// </summary>
        void Lineal(IPoint2 p2, double alpha, double beta);

        void Lineal(IPoint2 p1, IPoint2 p2, double alpha, double beta);

        /// <summary>
        /// Evaluates the projection vector of <code>this</code> point to <code>where</code>vector.
        /// <pre><![CDATA[
        /// 
        ///   (-)    |   (+)     __ this
        ///          |          _/|
        ///          |       _/   |
        ///          |    _/      |
        ///          | _/         |
        ///  --------+------------+------> where
        ///  ]]></pre>
        /// </summary>
        void ProjectTo(IVector2 where);

        void ProjectTo(IPoint2 p1, IVector2 where);
    }
}