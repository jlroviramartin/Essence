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

using REAL = System.Double;

namespace Essence.Geometry.Core.Double
{
    public class Rectangle2d
    {
        public Rectangle2d(double x, double y, double dx, double dy)
        {
            this.X = x;
            this.Y = y;
            this.DX = dx;
            this.DY = dy;
        }

        public double X, Y;
        public double DX, DY;
    }
}