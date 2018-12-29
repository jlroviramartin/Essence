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

namespace ClipperLib
{
    public interface IClipperConverter3d : IClipperConverter
    {
        void Clear();

        IntPoint ToIntPoint(Point3d p);

        Point3d FromIntPoint(IntPoint ip);

        void ZFill(Point3d bot1, Point3d top1,
                   Point3d bot2, Point3d top2,
                   ref Point3d pt);
    }
}