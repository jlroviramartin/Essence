#region License

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

#endregion

namespace Essence.Maths.Double
{
    public struct AABoundingBox2
    {
        public AABoundingBox2(Vec2d p0, Vec2d p1)
        {
            if (p0.X <= p1.X)
            {
                this.xMin = p0.X;
                this.xMax = p1.X;
            }
            else
            {
                this.xMin = p1.X;
                this.xMax = p0.X;
            }
            if (p0.Y <= p1.Y)
            {
                this.yMin = p0.Y;
                this.yMax = p1.Y;
            }
            else
            {
                this.yMin = p1.Y;
                this.yMax = p0.Y;
            }
        }

        public AABoundingBox2(double xmin, double ymin, double xmax, double ymax)
        {
            this.xMin = xmin;
            this.yMin = ymin;
            this.xMax = xmax;
            this.yMax = ymax;
        }

        public double XMin
        {
            get { return this.xMin; }
        }

        public double YMin
        {
            get { return this.yMin; }
        }

        public double XMax
        {
            get { return this.xMax; }
        }

        public double YMax
        {
            get { return this.yMax; }
        }

        private readonly double xMin;
        private readonly double yMin;
        private readonly double xMax;
        private readonly double yMax;
    }
}