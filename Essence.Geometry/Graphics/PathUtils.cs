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
using Essence.Geometry.Core;

namespace Essence.Geometry.Graphics
{
    public static class PathUtils
    {
        public static IEnumerable<TPoint> GetPoints<TPoint>(IPathIterator2 path)
            where TPoint : IPoint2
        {
            List<TPoint> points = new List<TPoint>();

            TPoint pFirst;
            TPoint p0;
            while (path.Next())
            {
                SegmentType segmentType = path.GetType();
                switch (segmentType)
                {
                    case SegmentType.MoveTo:
                    {
                        p0 = VectorUtils.Convert<TPoint>(path.GetP1());
                        points.Add(p0);

                        pFirst = p0;
                        break;
                    }
                    case SegmentType.LineTo:
                    {
                        TPoint p1 = VectorUtils.Convert<TPoint>(path.GetP1());
                        points.Add(p1);

                        p0 = p1;
                        break;
                    }
                    case SegmentType.CubicTo:
                    {
                        TPoint p1 = VectorUtils.Convert<TPoint>(path.GetP1());
                        points.Add(p1);

                        TPoint p2 = VectorUtils.Convert<TPoint>(path.GetP2());
                        points.Add(p2);

                        p0 = p1;
                        break;
                    }
                    case SegmentType.CuadTo:
                    {
                        TPoint p1 = VectorUtils.Convert<TPoint>(path.GetP1());
                        points.Add(p1);

                        TPoint p2 = VectorUtils.Convert<TPoint>(path.GetP2());
                        points.Add(p2);

                        TPoint p3 = VectorUtils.Convert<TPoint>(path.GetP3());
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