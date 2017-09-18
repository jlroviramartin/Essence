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

using System;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Curves;

namespace Essence.Geometry.Distances
{
    public class DistPointSegment2 : IDistance
    {
        public DistPointSegment2()
        {
        }

        public Point2d Point { get; set; }

        public Segment2 Segment { get; set; }

        public Point2d ClosestPoint { get; private set; }

        public double ClosestT { get; private set; }

        public double CalcDistance()
        {
            return Math.Sqrt(this.CalcDistance2());
        }

        public double CalcDistance2()
        {
            double param = this.Segment.Project(this.Point, false);
            double tmin = this.Segment.TMin;
            double tmax = this.Segment.TMax;

            if (tmin < param)
            {
                if (param < tmax)
                {
                    this.ClosestPoint = this.Segment.GetPosition(param);
                }
                else
                {
                    param = tmax;
                    this.ClosestPoint = this.Segment.Point1;
                }
            }
            else
            {
                param = tmin;
                this.ClosestPoint = this.Segment.Point0;
            }
            this.ClosestT = param;

            Vector2d diff = this.ClosestPoint.Sub(this.Point);
            return diff.LengthCuad;
        }
    }
}