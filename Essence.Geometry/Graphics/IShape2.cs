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
    public interface IShape2
    {
        BoundingBox2d GetBounds(IGraphicsContext2 context);

        bool Contains(IPoint2 pt, IGraphicsContext2 context);

        bool Contains(BoundingBox2d rec, IGraphicsContext2 context);

        bool Intersects(BoundingBox2d r, IGraphicsContext2 context);

        IPathIterator2 GetPathIterator(IGraphicsContext2 context, double flatness);
    }
}