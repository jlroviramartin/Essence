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

using Essence.Geometry.Core.Double;

namespace Essence.Geometry.Core
{
    public interface ITransform2
    {
        IVector2 Transform(IVector2 v);

        void Transform(IVector2 v, IOpVector2 vresult);

        IPoint2 Transform(IPoint2 p);

        void Transform(IPoint2 p, IOpPoint2 presult);

        ITransform2 Concat(ITransform2 transform);

        ITransform2 Inv { get; }

        bool IsIdentity { get; }

        /// <summary>
        /// This method gets the transform as a matrix.
        /// </summary>
        /// <param name="matrix">Matrix.</param>
        void GetMatrix(Matrix3x3d matrix);
    }
}