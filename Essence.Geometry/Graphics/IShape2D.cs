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

using Essence.Geometry.Core;
using Essence.Geometry.Core.Double;

namespace Essence.Geometry.Graphics
{
    public interface IShape2D
    {
        BoundingBox2d GetBounds(IGraphicsContext2D context);

        bool Contains(IPoint2D pt, IGraphicsContext2D context);

        bool Contains(BoundingBox2d rec, IGraphicsContext2D context);

        bool Intersects(BoundingBox2d r, IGraphicsContext2D context);

        IPathIterator2D GetPathIterator(IGraphicsContext2D context, double flatness);
    }
}