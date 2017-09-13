﻿// Copyright 2017 Jose Luis Rovira Martin
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
    public interface ITransform2D
    {
        IVector2D Transform(IVector2D v);

        IPoint2D Transform(IPoint2D p);

        ITransform2D Concat(ITransform2D transform);

        ITransform2D Inv { get; }

        bool IsIdentity { get; }

        /// <summary>
        /// This method gets the transform as a matrix.
        /// </summary>
        /// <param name="matrix">Matrix.</param>
        void GetMatrix(Matrix3x3d matrix);
    }
}