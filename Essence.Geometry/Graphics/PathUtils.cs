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

using System.Collections.Generic;
using Essence.Geometry.Core.Double;

namespace Essence.Geometry.Graphics
{
    public static class PathUtils
    {
        private static IEnumerable<Point2d> GetPoints(IPathIterator2D path)
        {
            List<Point2d> points = new List<Point2d>();

            CoordinateSetter2d setter = new CoordinateSetter2d();

            Point2d pFirst;
            Point2d p0;
            while (path.Next())
            {
                SegmentType segmentType = path.GetType();
                switch (segmentType)
                {
                    case SegmentType.MoveTo:
                    {
                        path.GetP1(setter);
                        p0 = setter.GetPoint();
                        points.Add(p0);

                        pFirst = p0;
                        break;
                    }
                    case SegmentType.LineTo:
                    {
                        path.GetP1(setter);
                        Point2d p1 = setter.GetPoint();
                        points.Add(p1);

                        p0 = p1;
                        break;
                    }
                    case SegmentType.CubicTo:
                    {
                        path.GetP1(setter);
                        Point2d p1 = setter.GetPoint();
                        points.Add(p1);

                        path.GetP2(setter);
                        Point2d p2 = setter.GetPoint();
                        points.Add(p2);

                        p0 = p1;
                        break;
                    }
                    case SegmentType.CuadTo:
                    {
                        path.GetP1(setter);
                        Point2d p1 = setter.GetPoint();
                        points.Add(p1);

                        path.GetP2(setter);
                        Point2d p2 = setter.GetPoint();
                        points.Add(p2);

                        path.GetP2(setter);
                        Point2d p3 = setter.GetPoint();
                        points.Add(p3);

                        p0 = p1;
                        break;
                    }
                    case SegmentType.Close:
                    {
                        break;
                    }
                }
            }
            return points;
        }
    }
}